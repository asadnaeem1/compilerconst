using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    public class Token
    {
        public string classKeyword { get; set; }
        public string value { get; set; }
        public int index { get; set; }
        public int lineNumber { get; set; }

        public Token(string _classKeyword, string _value, int _index, int _lineNumber)
        {
            classKeyword = _classKeyword;
            value = _value;
            index = _index;
            lineNumber = _lineNumber;
        }

        public override string ToString()
        {
            return String.Format("Class Keyword: {0} Value: {1} Index: {2} Line Number: {3}", classKeyword, value, index, lineNumber);
        }

        public void regexCheck(dynamic keyword, int type)
        {
            string regex = "";
            switch (type)
            {
                //check if alphanumeric or _
                case 1:
                    regex = @"^[a-zA-Z0-9_]*$";
                    break;
                //string check if only numric
                case 2:
                    regex = @"^[0-9]*$";
                    break;
                //char check if punctuator
                case 3:
                    regex = @"[^a-zA-Z0-9_]";
                    break;
                //check string if numeric decimal
                case 4:
                    regex = @"^[0-9]*[.]{1}[0-9]*$";
                    break;
                //check if string = '.'
                case 5:
                    regex = @"^[.]{1}$";
                    break;
                //check string start with " can contain multiple \"
                case 6:
                    regex = "^\"[^\"]*(\\\\\")*[^\"]*$";
                    break;
                //check string is .numalpha
                case 7:
                    regex = @"^[0-9]*[.]?[a-zA-Z0-9_]*$";
                    break;
                // check string if only alphabets
                case 8:
                    regex = @"^[a-zA-Z]*$";
                    break;
                default:
                    break;
            }
        }
    }
}
