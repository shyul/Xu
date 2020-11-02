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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Xu
{
    public class CSVData
    {



        public CSVDataRow this[int index] 
        {
            get 
            {
                if (index < Count && index > -1)
                    return Rows[index];
                else
                    return null;
            }
        }

        public int Count => Rows.Count;

        public List<string> Columns { get; } = new List<string>();

        public List<CSVDataRow> Rows { get; } = new List<CSVDataRow>();


        public void LoadFile(FileInfo fileInfo) 
        {
            // Read Header

            // Start Read Rows... Providing Progress as well!
        
        }

        public void SaveFile(FileInfo fileInfo)
        {


        }

        public static string[] CsvReadFields(string str, string regexFormat = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)") => Regex.Split(str, regexFormat);
        public static string[] CsvReadFields(StreamReader sr, string regexFormat = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)") => CsvReadFields(sr.ReadLine(), regexFormat);

        public static string TrimCsvValueField(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.Replace("\"", "").Trim();
            else
                return string.Empty;
        }

        public static string CsvEncode(string value)
        {
            string res = value.TrimCsvValueField();
            if (Regex.IsMatch(res, @"^\d+")) res = "\t" + res;
            if (res.Contains(",")) res = "\"" + res + "\"";
            return res;
        }


    }

    public class CSVDataRow
    {
        public string this[string columnName]
        {
            get
            {
                if (Data.ContainsKey(columnName))
                    return Data[columnName];
                else
                    return null;
            }
            set
            {
                if (value is string d)
                    Data[columnName] = d;
            }
        }

        public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();


    }
}
