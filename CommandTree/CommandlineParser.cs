using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandTree
{

    public static class CommandlineParser
    {
        class NamedValue
        {
            public NamedValueAttribute Arguments { get; set; }
            public PropertyInfo Property { get; set; }            
            public object Instance { get; set; }
            public List<string> Values { get; } = new List<string>();
        }


        class FlagValue
        {
            public FlagValueAttribute Arguments { get; set; }
            public PropertyInfo Property { get; set; }
            public object Instance { get; set; }
            public bool IsSet { get; set; } = false;
        }

        class CommandValue
        {
            public CommandAttribute Arguments { get; set; }
            public MethodInfo Method { get; set; }
            public PropertyInfo Property { get; set; }
            public object Instance { get; set; }
        }

        class CommandContext : ICommandContext
        {
            public CommandContext(Assembly assembly)
            {
                ObjectMap.Add(typeof(ICommandContext), this);
                FileName = System.IO.Path.GetFileName(assembly.Location);
            }
            public string FileName { get; set; }
            public List<string> CommandStack { get; } = new List<string>();
            public Dictionary<string, NamedValue> NamedValues { get; } = new Dictionary<string, NamedValue>();
            public Dictionary<string, FlagValue> LongFlags { get; } = new Dictionary<string, FlagValue>();
            public Dictionary<char, FlagValue> ShortFlags { get; } = new Dictionary<char, FlagValue>();
            public Dictionary<string, CommandValue> Commands { get; } = new Dictionary<string, CommandValue>();
            public NamedValue DefaultValue { get; set; }

            public object CurrentObject { get; set; }
            public string CommandHelp { get; set; }
            public CommandValue CurrentAction { get; set; }

            public Dictionary<Type, object> ObjectMap { get; set; } = new Dictionary<Type, object>();
        }

        static Regex mRegEx_NamedValue = new Regex(@"^--(?<Name>.*?):(?<Value>.*)$");
        static Regex mRegEx_NamedFlag = new Regex(@"^--(?<Name>[^:]*)$");
        static Regex mRegEx_FlagSet = new Regex(@"^-(?!-)(?<Flags>.*)$");

        public static int Parse<T>(string[] args)
            where T : class, new()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return Parse<T>(args, assembly);
        }

        public static int Parse<T>(string[] args, Assembly assembly) 
            where T : class, new()
        {
            CommandContext state = new CommandContext(assembly);
            state.CurrentObject = new T();
            

            state.ObjectMap.Add(state.CurrentObject.GetType(), state.CurrentObject);

            FetchOptions(state);

            int index = 0;

            //Parse commands first
            SetupContext(args, state, ref index);

            if (!ParseArguments(args, state, index))
                return 0;

            AssignValues(state);

            return RunCommand(state);
        }

        public static ICommandContext GetHelpContext<T>(string[] args)
            where T : class, new()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return GetHelpContext<T>(args, assembly);
        }

        public static ICommandContext GetHelpContext<T>(string[] args, Assembly assembly)
            where T : class, new()
        {
            CommandContext state = new CommandContext(assembly);
            state.CurrentObject = new T();
            CommandDescriptionAttribute attr = typeof(T).GetCustomAttribute<CommandDescriptionAttribute>();
            state.CommandHelp = attr?.HelpText ?? "";

            state.ObjectMap.Add(state.CurrentObject.GetType(), state.CurrentObject);

            FetchOptions(state);

            int index = 0;

            //Parse commands first
            SetupContext(args, state, ref index);

            return state;
        }

        public static void PrintHelp(ICommandContext context, string error_message = null)
        {
            CommandContext state = (CommandContext)context;
            if(error_message != null)
                PrintColor(error_message, ConsoleColor.Red);


            if (state.Commands.Count > 0)
                PrintCommandList(state);
            else
                PrintCommandUsage(state);
            
        }

        private static void PrintCommandUsage(CommandContext state)
        {
            Console.WriteLine(state.CommandHelp);
            Console.WriteLine();
            PrintColor("Usage:", ConsoleColor.White);
            StringBuilder line = new StringBuilder();
            line.Append(" ");
            line.Append(string.Join(" ",state.CommandStack));

            bool has_other_options = state.LongFlags.Count > 0;

            foreach(var i in state.NamedValues.Values)
            {
                if (i.Arguments.Name == "")
                    continue;

                 if(!i.Arguments.Required)
                {
                    has_other_options = true;
                    continue;
                }
                line.Append($" --{i.Arguments.Name}:<{i.Property.PropertyType.Name}>");
            }
            if (has_other_options)
                line.Append(" [Other Options]");
            if(state.DefaultValue != null)
            {
                line.Append(" ");

                if (!state.DefaultValue.Arguments.Required)
                    line.Append("[");

                line.Append(state.DefaultValue.Arguments.HelpName ?? "File");

                if(state.DefaultValue.Property.PropertyType.IsArray)
                    line.Append(" ...");

                if (!state.DefaultValue.Arguments.Required)
                    line.Append("]");
            }

            Console.WriteLine(line.ToString());
            line.Clear();

            Console.WriteLine();

            PrintColor("Options:", ConsoleColor.White);

            ConsoleTable table = new ConsoleTable(2, 2);

            foreach(var i in state.LongFlags.Values)
            {
                table.AddRow($" --{i.Arguments.Name}(-{i.Arguments.ShortName})",i.Arguments.HelpText ?? "No help available");
            }

            foreach (var i in state.NamedValues.Values)
            {
                Type type = i.Property.PropertyType;
                
                if (type.IsArray)
                    type = type.GetElementType();

                if (i.Arguments.Name == "")
                    table.AddRow($" {i.Arguments.HelpName}({type.Name})", i.Arguments.HelpText);
                else
                    table.AddRow($" --{i.Arguments.Name}:<{type.Name}>", i.Arguments.HelpText);
            }
            table.Sort(0);
            table.Print();
            Console.WriteLine();
        }

        private static void PrintCommandList(CommandContext state)
        {
            PrintColor("Commands:", ConsoleColor.White);
            ConsoleTable table = new ConsoleTable(2, 2);
            string cmdpath = string.Join(" ", state.CommandStack);

            foreach (var cmd in state.Commands.Values)
            {
                table.AddRow($" {cmdpath} {cmd.Arguments.Name}", cmd.Arguments.HelpText ?? "No help available");
            }

            table.Print();
        }

        private static void PrintColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static int RunCommand(CommandContext state)
        {
            if(state.CurrentAction == null)
            {
                state.CurrentAction = new CommandValue()
                {
                    Arguments = new CommandAttribute("Run"),
                    Instance = state.CurrentObject,
                    Method = state.CurrentObject.GetType().GetMethod("Run")
                };
                if (state.CurrentAction.Method == null)
                    throw new CommandLineException("Invalid command", state);
            }
            
            if (state.CurrentAction.Method.ReturnType != typeof(int) && state.CurrentAction.Method.ReturnType != typeof(void))
                throw new CommandTreeException($"Action '{state.CurrentAction.Method.Name}' does not return int or void");

            ParameterInfo[] prms = state.CurrentAction.Method.GetParameters();
            object[] args = new object[prms.Length];

            for (int i = 0; i < args.Length; i++)
            {
                var prm = prms[i];
                if (prm.ParameterType.IsByRef && prm.IsOut)
                    throw new CommandTreeException("out parameters not supported in command functions");
                object inject;
                if(!state.ObjectMap.TryGetValue(prm.ParameterType,out inject))
                    throw new CommandTreeException($"Unable to find value for paramter '{prm.Name}'");
                args[i] = inject;
            }

            object ret = state.CurrentAction.Method.Invoke(state.CurrentAction.Instance, args) ?? 0;

            return (int)ret;
        }

        private static bool ParseArguments(string[] args, CommandContext state, int index)
        {
            for (; index < args.Length; index++)
            {
                var arg = args[index];

                switch (ParseFlags(arg, state))
                {
                    case ParseFlagResult.Handled:
                        continue;
                    case ParseFlagResult.NotHandled:
                        break;
                    case ParseFlagResult.Help:
                        PrintHelp(state);
                        return false;
                }
                ParseNamedValues(arg, state);
                    
            }

            return true;
        }

        private static void SetupContext(string[] args, CommandContext state, ref int index)
        {
            state.CommandStack.Add(state.FileName);
            for (; index < args.Length && !args[index].StartsWith("-"); index++)
            {
                var arg = args[index];
                CommandValue value;
                if (!state.Commands.TryGetValue(arg, out value))
                    break;

                state.CommandStack.Add(arg);
                state.CommandHelp = value.Arguments.HelpText;

                if (value.Method != null)
                {
                    state.CurrentAction = value;
                    state.Commands.Clear();
                    index++;
                    break;
                }

                object obj = value.Property.GetValue(state.CurrentObject);
                if (obj == null)
                    throw new CommandTreeException($"Command property \"{value.Property.Name}\" returned null");
                state.CurrentObject = obj;
                state.ObjectMap.Add(state.CurrentObject.GetType(), state.CurrentObject);
                
                FetchOptions(state);
            }
        }

        private static void ParseNamedValues(string arg, CommandContext state)
        {
            
            Match match;

            string value;
            string name;
            if (mRegEx_NamedValue.Match(arg, out match))
            {
                name = match.Groups["Name"].Value;
                value = match.Groups["Value"].Value;
            }
            else
            {
                name = "";
                value = arg;
            }

            NamedValue nvalue;
            if(!state.NamedValues.TryGetValue(name,out nvalue))
            {
                if (name == "")
                    throw new CommandLineException($"Unexpected argument '{arg}'.", state);
                else
                    throw new CommandLineException($"Unrecognized argument '--{name}'.", state);
            }

            nvalue.Values.Add(value);
            
        }

        enum ParseFlagResult
        {
            NotHandled,
            Handled,
            Help
        }

        private static ParseFlagResult ParseFlags(string arg, CommandContext state)
        {
            Match match;

            if (arg == "--help")
                return ParseFlagResult.Help;

            if (mRegEx_NamedFlag.Match(arg,out match))
            {
                string name = match.Groups["Name"].Value;
                if (!state.LongFlags.TryGetValue(name, out FlagValue value))
                {
                    if(state.NamedValues.ContainsKey(name))
                        throw new CommandLineException($"Argument '{arg}' missing value", state);
                    else
                        throw new CommandLineException($"Unrecognized argument '{arg}'", state);
                }
                value.IsSet = true;
                return ParseFlagResult.Handled;
            }
            if (!mRegEx_FlagSet.Match(arg, out match))
                return ParseFlagResult.NotHandled;

            foreach (var name in match.Groups["Flags"].Value)
            {
                if (!state.ShortFlags.TryGetValue(name, out FlagValue value))
                    throw new CommandLineException($"Unrecongnized flag '-{name}'", state);
                value.IsSet = true;
            }

            return ParseFlagResult.Handled;
        }

        private static void FetchOptions(CommandContext state)
        {
            state.Commands.Clear();

            foreach(MethodInfo i in state.CurrentObject.GetType().GetMethods(BindingFlags.Public|BindingFlags.Instance))
            {
                var cmd = i.GetCustomAttribute(typeof(CommandAttribute)) as CommandAttribute;
                
                if (cmd == null)
                    continue;

                CommandValue value = new CommandValue()
                {
                    Arguments = cmd,
                    Method = i,
                    Instance = state.CurrentObject
                };
                state.Commands.Replace(value.Arguments.Name, value);
            }

            foreach(PropertyInfo i in state.CurrentObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var cmd = i.GetCustomAttribute(typeof(CommandAttribute)) as CommandAttribute;
                var nvalue = i.GetCustomAttribute(typeof(NamedValueAttribute)) as NamedValueAttribute;
                var flag = i.GetCustomAttribute(typeof(FlagValueAttribute)) as FlagValueAttribute;
                if(cmd != null)
                {
                    CommandValue value = new CommandValue()
                    {
                        Arguments = cmd,
                        Property = i,
                        Instance = state.CurrentObject
                    };
                    state.Commands.Replace(value.Arguments.Name, value);
                }

                if (nvalue != null)
                {
                    NamedValue value = new NamedValue()
                    {
                        Arguments = nvalue,
                        Property = i,
                        Instance = state.CurrentObject
                    };
                    state.NamedValues.Replace(value.Arguments.Name, value);
                    if (value.Arguments.Name == "")
                        state.DefaultValue = value;
                }

                if (flag != null)
                {
                    FlagValue value = new FlagValue()
                    {
                        Arguments = flag,
                        Property = i,
                        Instance = state.CurrentObject
                    };
                    state.LongFlags.Replace(value.Arguments.Name, value);
                    state.ShortFlags.Replace(value.Arguments.ShortName, value);
                }
            }
        }

        private static void AssignValues(CommandContext state)
        {
            foreach(var i in state.LongFlags.Values)
            {
                i.Property.SetValue(i.Instance, i.IsSet ? i.Arguments.ValueIfSet : i.Arguments.ValueIfUnset);
            }
            object value = null;
            foreach(var i in state.NamedValues.Values)
            {
                if (i.Values.Count == 0)
                {
                    if (i.Arguments.DefaultValue != null)
                        i.Values.Add(i.Arguments.DefaultValue);
                    else if (i.Arguments.Required)
                    {
                        throw new CommandLineException($"Missing {i.Arguments.HelpName} argument!", state);
                    }
                    else
                        continue;
                }
                if (i.Property.PropertyType.IsArray)
                    value = GetValueArray(state, i);
                else if (i.Values.Count != 0)
                {
                    if (!ConvertValue(i.Property.PropertyType, i.Values[0], out value))                    
                        throw new CommandLineException($"'value '{i.Values[0]}' is not a valid format for parameter '{i.Arguments.HelpName}'.", state);
                    
                }
                i.Property.SetValue(i.Instance, value);
            }
        }

        private static bool ConvertValue(Type type, string text, out object value)
        {
            Int32 int32val;
            UInt32 uint32val;
            Int64 int64val;
            UInt64 uint64val;
            Int16 int16val;
            UInt16 uint16val;
            Boolean boolval;

            value = null;

            switch (type.FullName)
            {
                case "System.String":
                    value = text;
                    return true;

                case "System.Int32":
                    if (!Int32.TryParse(text, out int32val))
                        return false;
                    value = int32val;
                    return true;

                case "System.Int16":
                    if (!Int16.TryParse(text, out int16val))
                        return false;
                    value = int16val;
                    return true;

                case "System.Int64":
                    if (!Int64.TryParse(text, out int64val))
                        return false;
                    value = int64val;
                    return true;

                case "System.UInt32":
                    if (!UInt32.TryParse(text, out uint32val))
                        return false;
                    value = uint32val;
                    return true;

                case "System.UInt16":
                    if (!UInt16.TryParse(text, out uint16val))
                        return false;
                    value = uint16val;
                    return true;

                case "System.UInt64":
                    if (!UInt64.TryParse(text, out uint64val))
                        return false;
                    value = uint64val;
                    return true;

                case "System.Boolean":
                    if (!Boolean.TryParse(text, out boolval))
                        return false;
                    value = boolval;
                    return true;
                default:
                    return false;
            }
        }

        private static object GetValueArray(CommandContext state, NamedValue nvalue)
        {
            Array array = nvalue.Property.PropertyType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[] { nvalue.Values.Count }) as Array;
            Type etype = nvalue.Property.PropertyType.GetElementType();

            for(int i = 0;i < array.Length;i++)
            {
                object value;
                if (!ConvertValue(etype, nvalue.Values[i], out value))                
                    throw new CommandLineException($"'value '{nvalue.Values[i]}' is not a valid format for parameter '{nvalue.Arguments.HelpName}'.", state);                    
                

                array.SetValue(value, i);
            }

            return array;
        }
    }

    public class CommandTreeException : Exception
    {
        public CommandTreeException()
        {
        }

        public CommandTreeException(string message) 
            : base(message)
        {
        }

        public CommandTreeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CommandTreeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }

    public class CommandLineException : Exception
    {
        public CommandLineException(ICommandContext context)
        {
            CommandContext = context;
        }

        public CommandLineException(string message, ICommandContext context)
            : base(message)
        {
            CommandContext = context;
        }

        public CommandLineException(string message, Exception innerException, ICommandContext context)
            : base(message, innerException)
        {
            CommandContext = context;
        }

        public ICommandContext CommandContext { get; private set; }
    }

    public interface ICommandContext
    {
    };
}

