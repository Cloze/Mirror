﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Mirror.Calendar
{
    class Content
    {
        internal string Name { get; private set; }
        internal string Value { get; private set; }
        internal Dictionary<string, List<string>> Parameters { get; } = new Dictionary<string, List<string>>();

        internal Content(string contentline)
        {
            contentline = contentline.Trim();
            Name = Regex.Match(contentline, @"(.*?)[;:]").Groups[1].Value;
            Value = Regex.Match(contentline, @".*?:(.*(\n\s.*)*)").Groups[1].Value;

            foreach (Match paramValue in Regex.Matches(contentline, @"^.*?;(.*:)"))
            {
                foreach (Match paramValueSplit in Regex.Matches(paramValue.Groups[1].Value, @"(.+?)=(.+?)[;:]"))
                {
                    Parameters.Add(paramValueSplit.Groups[1].Value, paramValueSplit.Groups[2].Value.Split(',').ToList());
                }
            }
        }

        internal bool HasParameterAndValue(string key, string value)
        {
            try
            {
                return Parameters[key].Contains(value);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Regex.Unescape(Value);
            //Regex.Replace(Value.Replace(Environment.NewLine + "\t", string.Empty)
            //            .Replace(Environment.NewLine + " ", "")
            //            .Replace("\n\r", Environment.NewLine)
            //            .Replace("\\n\\r", Environment.NewLine)
            //            .Replace("\n", Environment.NewLine)
            //            .Replace("\\n", Environment.NewLine)
            //            .Replace("\r", Environment.NewLine), @"\\(.)", "$1")
            //            .Trim();
    }
}