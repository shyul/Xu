/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Text based data functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Xu
{
    public static class TextTool
    {
        public const char ValueSeparator = '|';

        /// <summary>
        /// Replace the string end
        /// </summary>
        /// <param name="str">Input String</param>
        /// <param name="s1">If the ending matches s1</param>
        /// <param name="s2">The replace s1 with s2</param>
        /// <returns></returns>
        public static string ReplaceEnd(this string str, string s1, string s2)
        {
            int len = s1.Length;
            int len_diff = str.Length - len;
            if (len_diff < 0) return str;
            string end = str.Substring(len_diff, len);
            if (end == s1)
                return str.Substring(0, len_diff) + s2;
            else
                return str;
        }

        public static string GetLast(this string str, int len)
        {
            if (len > str.Length)
                return str;
            else
                return str.Substring(str.Length - len);
        }

        #region Text Files

        public static string ReadAllText(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }

        public static bool ToFile(this StringBuilder sb, string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
                File.WriteAllText(path, sb.ToString());
                return true;
            }
            catch (Exception e) when (e is IOException || e is PathTooLongException || e is UnauthorizedAccessException)
            {
                return false;
            }
        }

        #endregion Text Files

        #region Coding

        public static string CleanUpExtraBracket(this string s)
        {
            while (s.Length > 1)
            {
                if (s[0] == '(' && s[s.Length - 1] == ')')
                {
                    s = s.Substring(1, s.Length - 2);
                }
                else if (s[0] == '[' && s[s.Length - 1] == ']')
                {
                    s = s.Substring(1, s.Length - 2);
                }
                else if (s[0] == '{' && s[s.Length - 1] == '}')
                {
                    s = s.Substring(1, s.Length - 2);
                }
                else break;
            }
            return s;
        }

        //public static Regex TokenRegex = new Regex("[^a-zA-Z0-9]");
        public static string[] GetTokens(this string s)
        {
            s = s.Replace(" ", string.Empty);

            List<string> tokenList = new();
            string token = string.Empty;

            char currentBracket = '(';
            char currentCounterBracket = ')';
            int currentBracketLevel = 0;

            foreach (char c in s)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    token += c;
                }
                else if (currentBracketLevel > 0)
                {
                    if (c == currentBracket) currentBracketLevel++;
                    else if (c == currentCounterBracket) currentBracketLevel--;

                    if (currentBracketLevel > 0)
                        token += c;
                    else
                    {
                        token = token.CleanUpExtraBracket();

                        if (token.Length > 0)
                        {
                            tokenList.Add(token);
                        }
                        token = string.Empty;
                    }
                }
                // Initialize Parentheses mode
                else
                {
                    if (token.Length > 0) tokenList.Add(token);
                    token = string.Empty;

                    switch (c)
                    {
                        case '(':
                            currentBracket = '(';
                            currentCounterBracket = ')';
                            currentBracketLevel = 1;
                            break;
                        case '[':
                            currentBracket = '[';
                            currentCounterBracket = ']';
                            currentBracketLevel = 1;
                            break;
                        case '{':
                            currentBracket = '{';
                            currentCounterBracket = '}';
                            currentBracketLevel = 1;
                            break;
                        case '<':
                            currentBracket = '<';
                            currentCounterBracket = '>';
                            currentBracketLevel = 1;
                            break;

                    }
                }
            }

            if (token.Length > 0) tokenList.Add(token);

            return tokenList.ToArray();
        }


        #endregion Coding

        #region CSV File

        public static string[] CsvReadFields(this string str, string regexFormat = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)") => Regex.Split(str, regexFormat);
        public static string[] CsvReadFields(this StreamReader sr, string regexFormat = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)") => CsvReadFields(sr.ReadLine(), regexFormat);

        public static string TrimCsvValueField(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.Replace("\"", "").Trim();
            else
                return string.Empty;
        }

        public static string CsvEncode(this string value)
        {
            string res = value.TrimCsvValueField();
            if (Regex.IsMatch(res, @"^\d+")) res = "\t" + res;
            if (res.Contains(",")) res = "\"" + res + "\"";
            return res;
        }

        #endregion CSV File
    }


}
