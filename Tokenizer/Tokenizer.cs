using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Erasystemlevel.Tokenizer
{
    public class Tokenizer
    {
        Regex numericRegex = new Regex("");
        Regex identifierRegex = new Regex("");
        HashSet<string> delimeters = new HashSet<string>(new List<string>() { "", "" });
        HashSet<string> operators = new HashSet<string>(new List<string>() { "", "" });
        HashSet<string> keyword = new HashSet<string>(new List<string>() { "", "" });
        StreamReader reader;

        public Tokenizer(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open,
                FileAccess.Read);
            this.reader = new StreamReader(fs, Encoding.UTF8);
        }

        public void tokenize()
        {
            while (!this.reader.EndOfStream)
            {
                Console.WriteLine(this.reader.ReadLine();
            }
        }






    }
}
