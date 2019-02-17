using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LexicalAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Source Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = null;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                fctb.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            breakKeywords();
        }

        public void breakKeywords()
        {
            string sourceCode = fctb.Text, temp = "";
            bool stringFlag = false;
            int lineNumber = 1, index = 1;
            string[] lines = sourceCode.Split("\n".ToCharArray());
            foreach(string s in lines){
                string[] words = s.Split(' ');
                foreach(string ss in words){
                    foreach(char c in ss){
                        if (c != '\r')
                        {
                            //if c is not a punctuator
                            if (!regexCheck(c, 5))
                            {
                                temp += c;
                            }
                            //is a punctuator
                            else
                            {
                                // types . " and others
                                //string
                                if (c=='"')
                                {
                                    stringFlag = !stringFlag;
                                    if (!string.IsNullOrWhiteSpace(temp))
                                    {
                                        addTokenToList(temp, index, lineNumber);
                                        index++;
                                        temp = "";
                                    }
                                }
                                else if (c == '.')
                                {
                                    //check if temp is only numeric
                                    if (regexCheck(temp, 3))
                                    {
                                        temp += c;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(temp))
                                        {
                                            addTokenToList(temp, index, lineNumber);
                                            index++;
                                        }
                                        addTokenToList(c.ToString(), index, lineNumber);
                                        index++;
                                        temp = "";
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(temp))
                                    {
                                        addTokenToList(temp, index, lineNumber);
                                        index++;
                                    }
                                    addTokenToList(c.ToString(), index, lineNumber);
                                    index++;
                                    temp = "";
                                }
                            }
                        }
                    }
                    //end of word, if temp is not empty insert into keywords
                    if (!string.IsNullOrWhiteSpace(temp))
                    {
                        addTokenToList(temp, index, lineNumber);
                        temp = "";
                    }
                    index++;
                }
                lineNumber++;
                index = 1;
            }
            string mes = "";
            foreach (Token t in StaticComponents.tokenSet)
            {
                mes += t.ToString() + "\n";
            }
            MessageBox.Show(mes);
        }
        public bool regexCheck(dynamic keyword, int type)
        {
            string regex = "";
            switch (type)
            {
                //string check if alphanumeric or _
                case 1:
                    regex = @"^[a-zA-Z0-9_]*$";
                    break;
                //char check if alphanumeric or _
                case 2:
                    regex = @"[a-zA-Z0-9_]";
                    break;
                //string check if only numric
                case 3:
                    regex = @"^[0-9]*$";
                    break;
                //char check if only numric
                case 4:
                    regex = @"[0-9]";
                    break;
                //char check if punctuator
                case 5:
                    regex = @"[^a-zA-Z0-9_]";
                    break;
                default:
                    break;
            }
            return regexCheck(keyword.ToString(), regex);
        }

        public bool regexCheck(dynamic keyword, string regex)
        {
            Match m = Regex.Match(keyword.ToString(), regex);
            if (m.Success)
            {
                return true;
            }
            else
                return false;
        }

        public void addTokenToList(string value, int index, int lineNumber)
        {
            StaticComponents.tokenSet.Add(new Token("", value, index, lineNumber));
        }
    }
}