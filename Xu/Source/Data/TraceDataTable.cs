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
    public class TraceDataTable
    {
        public int Count => Rows.Count;

        public virtual void Clear()
        {
            lock (this) Rows.Clear();
        }

        public Dictionary<double, TraceDataRow> Rows { get; protected set; }
            = new Dictionary<double, TraceDataRow>();

        public virtual TraceDataRow this[double x] 
        {
            get 
            {
                lock (this)
                {
                    if (!Rows.ContainsKey(x))
                        Rows.Add(x, new TraceDataRow(x));

                    return Rows[x];
                }
            }
        }

        public void Add(TraceDataRow row)
        {
            lock (this)
                Rows[row.X] = row;
        }
    }

    public class TraceDataRow : IEquatable<TraceDataRow>, IEquatable<double>
    {
        public TraceDataRow(double x) 
        {
            X = x;
            HashCode = X.GetHashCode();
            Time = DateTime.Now;
        }

        public DateTime Time { get; set; }

        public double X { get; }

        public double this[TraceDataColumn column]
        {
            get
            {
                if (Data.ContainsKey(column))
                    return Data[column];
                else
                    return double.NaN;
            }
            set
            {
                if (value is double d && !double.IsNaN(d))
                    Data[column] = d;
            }
        }

        public Dictionary<TraceDataColumn, double> Data { get; } = new Dictionary<TraceDataColumn, double>();



        public int HashCode { get; }
        public override int GetHashCode() => HashCode;
        public bool Equals(TraceDataRow other) => X == other.X;
        public bool Equals(double other) => X == other;
    }

    public class TraceDataColumn : IEquatable<TraceDataColumn>, IEquatable<string>
    {
        public TraceDataColumn(string name)
        {
            Name = name;
            HashCode = Name.GetHashCode();
        }


        public string Name { get; }


        public int HashCode { get; }
        public override int GetHashCode() => HashCode;
        public bool Equals(TraceDataColumn other) => HashCode == other.HashCode;
        public bool Equals(string other) => HashCode == other.GetHashCode();
    }
}
