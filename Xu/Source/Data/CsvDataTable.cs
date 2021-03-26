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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Xu
{
    public class CsvDataTable
    {



        public CsvDataRow this[int index]
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

        public List<CsvDataRow> Rows { get; } = new List<CsvDataRow>();

        public void Clear()
        {
            Columns.Clear();
            Rows.Clear();
        }

        public void LoadFile(string fileName) => LoadFile(new FileInfo(fileName), new CancellationTokenSource(), null);

        public void LoadFile(FileInfo fi, CancellationTokenSource cts, IProgress<float> progress)
        {
            if (fi.Exists)
            {
                long byteRead = 0;
                long totalSize = fi.Length;

                try
                {
                    using StreamReader sr = fi.OpenText();

                    // Read Header
                    string line = sr.ReadLine();
                    byteRead += line.Length + 1;

                    Clear();
                    Columns.AddRange(DecodeFields(line));

                    // Start Read Rows... Providing Progress as well!
                    while (!sr.EndOfStream && !cts.IsCancellationRequested)
                    {
                        line = sr.ReadLine();
                        if (progress != null)
                        {
                            byteRead += line.Length + 1;
                            progress.Report(byteRead * 100.0f / totalSize);
                        }

                        string[] fields = DecodeFields(line);

                        if (fields.Length >= Columns.Count)
                        {
                            CsvDataRow dr = new();

                            for (int i = 0; i < Columns.Count; i++)
                            {
                                dr[Columns[i]] = fields[i];
                            }

                            Rows.Add(dr);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File error: " + e.Message);
                }
            }
        }

        public void SaveFile(string fileName) => SaveFile(new FileInfo(fileName));

        public void SaveFile(FileInfo fi)
        {
            using StreamWriter sr = fi.CreateText();

            string header = string.Join(",", Columns);
            sr.WriteLine(header);

            foreach (var row in Rows) sr.WriteLine(EncodeFields(row));

            sr.Close();
        }

        public string EncodeFields(CsvDataRow dr)
        {
            string line = string.Empty;
            for (int i = 0; i < Columns.Count; i++)
            {
                line += EncodeField(dr[Columns[i]]) + ",";
            }
            return line.Trim(',').Trim();
        }

        public static string EncodeField(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim();

                if (Regex.IsMatch(value, @"^\d+"))
                    value = "\t" + value;

                if (value.Contains(","))
                    value = "\"" + value.Replace("\"", "").Trim() + "\"";

                return value;
            }

            return string.Empty;
        }

        public static string[] DecodeFields(string str, string regexFormat = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)") => Regex.Split(str, regexFormat);

    }

    public class CsvDataRow
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
