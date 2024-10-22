﻿/// ***************************************************************************
/// Shared Libraries and Quick Utilities
/// GPL 2001-2007, 2014-2021 Xu Li - me@xuli.us
/// 
/// Data Serializations and Reflection
/// 
/// Code Author: Xu Li
/// 
/// ***************************************************************************

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Xu
{
    public static partial class Serialization
    {
        public static FileInfo[] GetFileList(string path, string suffix = "*")
        {
            DirectoryInfo d = new(path);
            return d.GetFiles(suffix);
        }

        #region Binary Data

        /// <summary>
        /// Binary Serialization
        /// </summary>
        public static MemoryStream SerializeBinaryStream<T>(this T source)
        {
            if (source != null && typeof(T).IsSerializable)
            {
                MemoryStream stream = new();
                BinaryFormatter formatter = new();
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;
                return stream;
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="stream"></param>
        public static void SerializeBinaryFile<T>(this T source, FileStream stream)
        {
            if (source != null && typeof(T).IsSerializable)
            {
                BinaryFormatter formatter = new();
                formatter.Serialize(stream, source);
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fileName"></param>
        public static void SerializeBinaryFile<T>(this T source, string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            using FileStream stream = File.OpenWrite(fileName);
            SerializeBinaryFile<T>(source, stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] SerializeBinary<T>(this T source)
        {
            using MemoryStream stream = source.SerializeBinaryStream();
            return stream.ToArray();
        }

        /// <summary>
        /// Binary Deserialization
        /// </summary>
        public static T DeserializeBinaryStream<T>(Stream stream)
        {
            BinaryFormatter formatter = new();
            return (T)formatter.Deserialize(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializeBinaryFile<T>(string fileName)
        {
            if (File.Exists(fileName))
                using (FileStream stream = File.OpenRead(fileName))
                {
                    return DeserializeBinaryStream<T>(stream);
                }
            else
                return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeserializeBinary<T>(this byte[] source)
        {
            using MemoryStream stream = new(source);
            return (T)DeserializeBinaryStream<T>(stream);
        }

        /// <summary>
        /// Clone Serializable Object
        /// </summary>
        public static T Clone<T>(this T source)
        {
            if (source != null && typeof(T).IsSerializable)
            {
                using MemoryStream stream = source.SerializeBinaryStream();
                return DeserializeBinaryStream<T>(stream);
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        /// <summary>
        /// Save memorystream to file
        /// </summary>
        public static void ToFile(this MemoryStream s, string fileName)
        {
            s.Position = 0;
            using FileStream fs = new(fileName, FileMode.Create);
            s.WriteTo(fs);
        }

        #endregion Binary Data

        #region XML Data

        /// <summary>
        /// XML Serialization
        /// </summary>
        public static byte[] SerializeXML<T>(this T source)
        {
            if (source != null && typeof(T).IsSerializable)
            {
                using MemoryStream stream = new();
                XmlSerializer xmlSer = new(typeof(T));
                StreamWriter sw = new(stream, Encoding.Unicode);
                xmlSer.Serialize(sw, source);
                stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;
                return stream.ToArray();
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fileName"></param>
        public static void SerializeXMLFile<T>(this T source, string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            File.WriteAllBytes(fileName, source.SerializeXML());
        }

        /// <summary>
        /// XML Deserialization
        /// </summary>
        public static T DeserializeXML<T>(string source) => DeserializeXML<T>(Encoding.ASCII.GetBytes(source));

        /// <summary>
        /// XML Deserialization
        /// </summary>
        public static T DeserializeXML<T>(this byte[] source)
        {
            using MemoryStream stream = new(source);
            XmlSerializer xmlSer = new(typeof(T));
            stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;

            using XmlReader rd = XmlReader.Create(stream);
            return (T)xmlSer.Deserialize(rd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializeXMLFile<T>(string fileName)
        {
            if (File.Exists(fileName))
                return (T)DeserializeXML<T>(File.ReadAllBytes(fileName));
            else
                return default;
        }

        /// <summary>
        /// XML Path Utilities
        /// </summary>
        public static string GetElementValue(this XDocument doc, string path)
        {
            XElement xe = doc.XPathSelectElement(path);
            if (xe != null)
                return xe.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetElementValue(this XElement xe, string name)
        {
            XElement xe2 = xe.Element(name);
            if (xe2 != null)
                return xe2.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XDocument doc, string path, string attrib)
        {
            XElement xe = doc.XPathSelectElement(path);
            if (xe != null)
            {
                XAttribute xa = xe.Attribute(attrib);
                if (xa != null)
                    return xa.Value;
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XElement xe, string name)
        {
            XAttribute xa = xe.Attribute(name);
            if (xa != null)
                return xa.Value;
            else
                return string.Empty;
        }

        #endregion XML Data

        #region Json Data

        /// <summary>
        /// Json Serialization
        /// </summary>
        public static byte[] SerializeJson<T>(this T source)
        {
            if (source is not null && typeof(T).IsSerializable)
            {
                using MemoryStream stream = new();
                DataContractJsonSerializer JsonSer = new(typeof(T));
                JsonSer.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;
                return stream.ToArray();
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        public static void SerializeJsonFile<T>(this T source, string fileName)
        {
            FileInfo fi = new(fileName);
            if (!fi.Directory.Exists) fi.Directory.Create();

            string backup_fileName = fileName + "_backup";

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);

            if (File.Exists(fileName))
                File.Move(fileName, backup_fileName);

            File.WriteAllBytes(fileName, source.SerializeJson());

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);
        }

        //public static void SerializeJsonFile(this IDataFile idf) => idf.SerializeJsonFile(idf.DataFileName);

        /// <summary>
        /// Json Deserialization
        /// </summary>
        public static T DeserializeJson<T>(this byte[] source)
        {
            using MemoryStream stream = new(source);
            DataContractJsonSerializer JsonSer = new(typeof(T));
            return (T)JsonSer.ReadObject(stream);
        }

        public static T DeserializeJsonFile<T>(string fileName)
        {
            string backup_fileName = fileName + "_backup";

            if (!File.Exists(fileName) && File.Exists(backup_fileName))
                File.Move(backup_fileName, fileName);

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);

            if (File.Exists(fileName))
                using (FileStream stream = File.OpenRead(fileName))
                    return DeserializeJson<T>(File.ReadAllBytes(fileName));
            else
                return default;
        }

        #endregion Json Data
    }
}
