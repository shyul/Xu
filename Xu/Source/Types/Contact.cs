/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Xu
{
    /// <summary>
    /// Types of the Contacting methods of Non-physical Address
    /// </summary>
    [Serializable, DataContract]
    public enum ContactDataType : int
    {
        [EnumMember]
        Phone = 10,

        [EnumMember]
        FAX = 20,

        [EnumMember]
        Email = 100,

        [EnumMember]
        WebSite = 200,
    }

    /// <summary>
    /// Physical Address
    /// </summary>
    [Serializable, DataContract]
    public class Address
    {
        [DataMember]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        [DataMember, Browsable(true), Category("Contact"), DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("Contact Title")]
        public string ContactTitle { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("Address Lines")]
        public List<string> Lines { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("City")]
        public string City { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("State")]
        public string State { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("Zip / Postal Code")]
        public string Zip { get; set; }

        [DataMember, Browsable(true), Category("Contact"), DisplayName("Country")]
        public string Country { get; set; }
    }

    [Serializable, DataContract]
    public class Employee : IEquatable<Employee>, IEquatable<Person>
    {
        public Employee(Person p)
        {
            Person = p;
        }

        [DataMember]
        public Person Person { get; private set; }

        [DataMember]
        public int Rank { get; set; } = 0;

        [DataMember]
        public string Id { get; set; } = string.Empty;

        [DataMember]
        public string Title { get; set; } = string.Empty;

        [DataMember]
        public DateTime StartTime { get; set; } //= DateTime.MaxValue.AddYears(-1);

        #region Equality

        public bool Equals(Employee other) => Person == other.Person;
        public bool Equals(Person other) => Person == other;

        public static bool operator ==(Employee s1, Employee s2) => s1.Equals(s2);
        public static bool operator !=(Employee s1, Employee s2) => !s1.Equals(s2);
        public static bool operator ==(Employee s1, Person s2) => s1.Equals(s2);
        public static bool operator !=(Employee s1, Person s2) => !s1.Equals(s2);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Employee))
                return Equals((Employee)obj);
            else if (obj.GetType() == typeof(Person))
                return Equals((Person)obj);
            else
                return false;
        }

        public override int GetHashCode() => Person.GetHashCode();

        #endregion Equality
    }

    [Serializable, DataContract]
    public class Person : IEquatable<Person>, IEquatable<Employee>
    {
        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string MiddleName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public string Salute { get; set; } = string.Empty;

        [DataMember]
        public DateTime Birthday { get; set; }

        [IgnoreDataMember]
        public double Age
        {
            get
            {
                return ((DateTime.Now - Birthday).TotalDays / 365.0).ToInt64();
            }
            set
            {
                DateTime bday = DateTime.Now.AddYears(((-1.0f) * (float)value).ToInt32());
                double error = Math.Abs((bday - Birthday).TotalDays);
                if (error > 365) Birthday = bday;
            }
        }

        #region Equality

        public bool Equals(Employee other) => Equals(other.Person);
        public bool Equals(Person other) => FirstName == other.FirstName && LastName == other.LastName && Salute == other.Salute && Birthday.Year == other.Birthday.Year;
        public static bool operator ==(Person s1, Person s2) => s1.Equals(s2);
        public static bool operator !=(Person s1, Person s2) => !s1.Equals(s2);
        public static bool operator ==(Person s1, Employee s2) => s1.Equals(s2);
        public static bool operator !=(Person s1, Employee s2) => !s1.Equals(s2);

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Person))
                return Equals((Person)obj);
            else if (obj.GetType() == typeof(Employee))
                return Equals((Employee)obj);
            else
                return false;
        }

        public override int GetHashCode() => FirstName.GetHashCode() ^ LastName.GetHashCode() ^ Salute.GetHashCode() ^ Birthday.Year.GetHashCode();

        #endregion Equality
    }
}
