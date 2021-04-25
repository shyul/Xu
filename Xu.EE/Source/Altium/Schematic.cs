using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Xu.EE
{
    public enum PinType : int
    {
        Passive = 0,
        Input = 1,
        Output = 2,
        IO = 3,
        Power = 4,
    }

    public class SchematicPin
    {
        public string Designator { get; set; }

        public string Name { get; set; }

        public bool IsClock { get; set; } = false;

        public bool IsLowActive { get; set; } = false;

        public PinType Type { get; set; } = PinType.Passive;
    }

    public static class Schematic
    {
        public static void ReadPinoutFile(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, SchematicPin> pinList = new();

                StringBuilder sb = new("StartComponents\n\nComponent (Name \"ADRV9001\") (PartCount 1) (DesPrefix \"U ? \")\n");

                List<string> pinDesList = new();
                int LocationX = -10000;
                int LocationY = 1000;
                int pinCount = 0;


                while (!sr.EndOfStream)
                {
                    string[] fields = sr.CsvReadFields();


                    if (fields.Length == 5 && !string.IsNullOrEmpty(fields[0]))
                    {
                        string name = fields[0].TrimCsvValueField();
                        string pinDesignator = fields[1].TrimCsvValueField();

                        if (!pinDesList.Contains(pinDesignator))
                        {
                            pinDesList.Add(pinDesignator);
                        }
                        else
                        {
                            throw new Exception("Duplicated Pin Found! " + pinDesignator + " | " + name);
                        }

                        string pinType = fields[2].TrimCsvValueField();

                        if (string.IsNullOrEmpty(pinType)) pinType = "Passive";

                        bool isLowActive = fields[3].TrimCsvValueField().ToLower() == "dot";
                        bool isClock = fields[4].TrimCsvValueField().ToLower() == "clock";

                        if (LocationY < -2000)
                        {
                            LocationX += 1000;
                            LocationY = 1000;
                        }

                        string row = "Pin ";
                        row += "(Location " + LocationX + ", " + LocationY + ") ";
                        row += "(Rotation 0) ";
                        row += "(PinType " + pinType + ") (Length 300) (Width 0) ";

                        if (isClock)
                            row += "(HasClock 1) ";

                        if (isLowActive)
                            row += "(HasDot 1) ";

                        row += "(Designator Visible \"" + pinDesignator + "\") ";
                        row += "(Name Visible \"" + name + "\")";
                        sb.AppendLine(row);
                        Console.WriteLine(row);

                        pinList.Add(pinDesignator, new SchematicPin()
                        {
                            Designator = pinDesignator,
                            Name = name,
                            IsClock = isClock,
                            IsLowActive = isLowActive,

                        });

                        LocationY -= 100;
                        pinCount++;
                    }
                    else
                    {
                        LocationX += 1000;
                        LocationY = 1000;
                    }
                }

                sb.AppendLine("EndComponent\nEndComponents\n");
                sb.ToFile(csvFileName.Replace(".csv", string.Empty) + "_altium.txt");

                Console.WriteLine("Pin Count = " + pinCount);

                pinList.OrderBy(n => n.Key).RunEach(n => Console.WriteLine("\n" + n.Key + " | " + n.Value.Name));
            }
        }
    }
}
