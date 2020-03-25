using System;

namespace CommandTree
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NamedValueAttribute : Attribute
    {
        public NamedValueAttribute(string name)
        {
            Name = name;
            HelpName = name;
        }

        public string Name { get; private set; }

        public string HelpName { get; set; }

        public string HelpText { get; set; }

        public bool Required { get; set; }

        public string DefaultValue { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FlagValueAttribute : Attribute
    {
        public FlagValueAttribute(char shortname, string name)
        {
            Name = name;
            ShortName = shortname;
        }

        public char ShortName { get; private set; }

        public string Name { get; private set; }

        public string HelpText { get; set; }

        public object ValueIfSet{ get; set; } = true;

        public object ValueIfUnset { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name)
        {
            Name = name;
            
        }
        
        public string Name { get; private set; }

        public string HelpText { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandDescriptionAttribute : Attribute
    {
        public CommandDescriptionAttribute(string text)
        {
            HelpText = text;
        }
        public string HelpText { get; private set; }
    }
}