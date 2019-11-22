using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace DarkzideGames.Lexer
{
    public class LexEngine<TToken>
    {
        class SOperand
        {
            public Regex Ex;
            public TToken Token;
        }

        List<SOperand> mOperands = new List<SOperand>();
        
                
        public TToken ErrorToken
        {
            get;
            set;
        }

        public TToken EofToken
        {
            get;
            set;
        }

        public void AddOperand(string exp, TToken token)
        {            
            SOperand op = new SOperand();
            op.Ex = new Regex(string.Format(@"^(?<ws>[\s\r\n]*)(?<exp>{0})", exp));
            op.Token = token;
            mOperands.Add(op);
            
        }
        static Regex mRegEx_Unknown = new Regex(@"^(?<ws>[\s\r\n]*)(?<text>.*)$");
        public IEnumerable<Token<TToken>> ProcessTokens(string text)
        {
            int linecount = 0;
            while (text != null)
            {
                Token<TToken> tok = ProcessToken(ref text,ref linecount);
                if (tok != null)
                    yield return tok;
            }
            
        }

        private Token<TToken> ProcessToken(ref string text,ref int linecount)
        {
            Match match;
            foreach (SOperand i in mOperands)
            {
                match = i.Ex.Match(text);
                if (match.Success)
                {
                    Token<TToken> ret = new Token<TToken>();
                    ret.Symbol = i.Token;
                    ret.Text = match.Groups["exp"].Value;
                    ret.Value = match.Groups["value"].Success ? match.Groups["value"].Value : ret.Text;
                    ret.Whitespace = match.Groups["ws"].Value;
                    text = text.Substring(match.Index + match.Length);
                    linecount += ret.Whitespace.Count('\n');
                    ret.Line = linecount;
                    return ret;
                }
            }
            if (!string.IsNullOrWhiteSpace(text))
            {
                Token<TToken> ret = new Token<TToken>();
                ret.Symbol = ErrorToken;
                ret.Text = text;
                ret.Whitespace = "";
                text = null;
                ret.ErrorText = "Unexpected symbol";
                linecount += ret.Whitespace.Count('\n');
                ret.Line = linecount;
                return ret;
            }
            else
            {
                Token<TToken> ret = new Token<TToken>();
                ret.Symbol = EofToken;
                ret.Text = "";
                ret.Whitespace = text;
                text = null;
                linecount += ret.Whitespace.Count('\n');
                ret.Line = linecount;
                return ret;
            }

            
        }
        
    }
    public class Token<TToken>
    {
        public TToken Symbol { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Whitespace { get; set; }
        public int Line { get; set; }
        public string ErrorText { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Symbol, Text);
        }
    }
}
