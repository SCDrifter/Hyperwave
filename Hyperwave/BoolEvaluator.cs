using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkzideGames.Lexer;
using System.Globalization;

namespace Hyperwave
{
    class BoolEvaluator
    {
        enum ExpTokens
        {
            Literal_String,
            Literal_Bool,
            Literal_Number,
            Operand_Arg,
            Operand_Const,
            Op_And,
            Op_Or,
            Op_Not,
            Op_Equals,
            Op_NotEquals,
            Op_LessThan,
            Op_LessThanEquals,
            Op_GreaterThan,
            Op_GreaterThanEquals,
            Op_SubExpressionStart,
            Op_SubExpressionEnd,
            Error,
            Eof
        }

        static LexEngine<ExpTokens> mLexer;

        static BoolEvaluator()
        {
            mLexer = new LexEngine<ExpTokens>()
            {
                ErrorToken = ExpTokens.Error,
                EofToken = ExpTokens.Eof
            };

            mLexer.AddOperand(@"'(?<value>[^']+)'", ExpTokens.Literal_String);
            mLexer.AddOperand(@"""(?<value>[^""]+)""", ExpTokens.Literal_String);
            mLexer.AddOperand(@"\b(true|false)\b", ExpTokens.Literal_Bool);
            mLexer.AddOperand(@"\-?\d+(\.\d+(e\d+)?)?", ExpTokens.Literal_Number);
            mLexer.AddOperand(@"\$(?<value>\d+)\b", ExpTokens.Operand_Arg);
            mLexer.AddOperand(@"\@[a-zA-Z_0-9]+\b", ExpTokens.Operand_Const);
            mLexer.AddOperand(@"and\b", ExpTokens.Op_And);
            mLexer.AddOperand(@"\&\&", ExpTokens.Op_And);
            mLexer.AddOperand(@"\&", ExpTokens.Op_And);
            mLexer.AddOperand(@"or\b", ExpTokens.Op_Or);
            mLexer.AddOperand(@"\|\|", ExpTokens.Op_Or);
            mLexer.AddOperand(@"\|", ExpTokens.Op_Or);
            mLexer.AddOperand(@"!=", ExpTokens.Op_NotEquals);
            mLexer.AddOperand(@"!", ExpTokens.Op_Not);
            mLexer.AddOperand(@"==", ExpTokens.Op_Equals);
            mLexer.AddOperand(@"=", ExpTokens.Op_Equals);
            mLexer.AddOperand(@"<=", ExpTokens.Op_LessThanEquals);
            mLexer.AddOperand(@">=", ExpTokens.Op_GreaterThanEquals);
            mLexer.AddOperand(@"<", ExpTokens.Op_LessThan);
            mLexer.AddOperand(@">", ExpTokens.Op_GreaterThan);
            mLexer.AddOperand(@"\(", ExpTokens.Op_SubExpressionStart);
            mLexer.AddOperand(@"\)", ExpTokens.Op_SubExpressionEnd);
        }

        public static bool Evaluate(string exp,object[] values,CultureInfo culture)
        {
            bool result = false;

            var tokens = mLexer.ProcessTokens(exp).GetEnumerator();

            if (tokens.MoveNext())
                result = ParseAndOr(values, tokens, culture, ExpTokens.Eof);

            return result;
        }

        private static bool ParseAndOr(object[] values, IEnumerator<Token<ExpTokens>> tokens, CultureInfo culture, ExpTokens end_token)
        {
            bool ret = ParseCompare(values, tokens, culture);
            while (tokens.Current.Symbol != end_token)
            {
                switch (tokens.Current.Symbol)
                {
                    case ExpTokens.Op_And:
                        tokens.MoveNext();
                        ret = ParseCompare(values, tokens, culture) && ret;
                        break;
                    case ExpTokens.Op_Or:
                        tokens.MoveNext();
                        ret = ParseCompare(values, tokens, culture) || ret;
                        break;
                    case ExpTokens.Error:
                        throw new Exception(tokens.Current.ErrorText);
                    default:
                        throw new Exception(string.Format("Unexpected token '{0}'", tokens.Current.Symbol));
                }
            }

            tokens.MoveNext();

            return ret;
        }

        private static bool ParseCompare(object[] values, IEnumerator<Token<ExpTokens>> tokens, CultureInfo culture)
        {
            IComparable val1 = ParseNot(values, tokens, culture);
            bool? ret = null;

            while (tokens.Current.Symbol >= ExpTokens.Op_Equals && tokens.Current.Symbol <= ExpTokens.Op_GreaterThanEquals)
            {
                var cmptoken = tokens.Current;

                tokens.MoveNext();
                IComparable val2 = ParseNot(values, tokens, culture);

                if (val2.GetType() != val1.GetType())
                {
                    val2 = (IComparable)System.Convert.ChangeType(val2, val1.GetType());
                }

                int cmp = val1.CompareTo(val2);

                switch (cmptoken.Symbol)
                {
                    case ExpTokens.Op_Equals:
                        ret = (cmp == 0);
                        break;
                    case ExpTokens.Op_NotEquals:
                        ret = (cmp != 0);
                        break;
                    case ExpTokens.Op_LessThan:
                        ret = (cmp < 0);
                        break;
                    case ExpTokens.Op_GreaterThan:
                        ret = (cmp > 0);
                        break;
                    case ExpTokens.Op_LessThanEquals:
                        ret = (cmp <= 0);
                        break;
                    case ExpTokens.Op_GreaterThanEquals:
                        ret = (cmp >= 0);
                        break;
                }
                val1 = ret.Value;
            }

            if (!ret.HasValue)
                ret = System.Convert.ToBoolean(val1, culture);

            return ret.Value;
        }

        private static IComparable ParseNot(object[] values, IEnumerator<Token<ExpTokens>> tokens, CultureInfo culture)
        {
            bool invert = tokens.Current.Symbol == ExpTokens.Op_Not;

            if (invert)
                tokens.MoveNext();

            IComparable ret = ParseValue(values, tokens, culture);

            if (invert)
            {
                bool val = System.Convert.ToBoolean(ret, culture);
                ret = !val;
            }

            return ret;
        }

        private static IComparable ParseValue(object[] values, IEnumerator<Token<ExpTokens>> tokens, CultureInfo culture)
        {
            IComparable ret = null;
            switch (tokens.Current.Symbol)
            {
                case ExpTokens.Literal_Bool:
                    ret = (tokens.Current.Text == "true");
                    break;
                case ExpTokens.Literal_Number:
                    ret = double.Parse(tokens.Current.Text);
                    break;
                case ExpTokens.Literal_String:
                    ret = tokens.Current.Value;
                    break;
                case ExpTokens.Operand_Arg:
                    ret = (IComparable)values[int.Parse(tokens.Current.Value)];
                    break;
                case ExpTokens.Op_SubExpressionStart:
                    tokens.MoveNext();
                    return ParseAndOr(values, tokens, culture, ExpTokens.Op_SubExpressionEnd);
                case ExpTokens.Error:
                    throw new Exception(tokens.Current.ErrorText);
                default:
                    throw new Exception(string.Format("Unexpected token '{0}'", tokens.Current.Symbol));
            }

            tokens.MoveNext();
            return ret;
        }
    }
}
