using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Xu.EE
{
    public class FPGAPin 
    {
        public string Designator { get; set; }

        public string Name { get; set; }

        public string DiffName { get; set; }

        public string Bank { get; set; }

        public string VREFGroup { get; set; }

        public bool IsClock { get; set; } = false;

        public bool IsLowActive { get; set; } = false;

        public PinType Type { get; set; } = PinType.Passive;
    }


    public enum PinType : int
    {
        Passive = 0,
        Input = 1,
        Output = 2,
        IO = 3,
        Power = 4,
    }

    public static class Altera
    {
        private static void AddPinFunction(this List<string> funcList, string func) 
        {
            if(!string.IsNullOrEmpty(func) && !funcList.Contains(func)) 
            {
                funcList.Add(func);
            }
        }

        public static void ReadPinoutFile(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, FPGAPin> pinList = new();
                List<string> vrefList = new();

                while (!sr.EndOfStream)
                {
                    string[] fields = sr.CsvReadFields();

                    if (fields.Length == 7)
                    {
                        string bankName = fields[0].TrimCsvValueField();

                        string VREF = fields[1].TrimCsvValueField();
                        if (string.IsNullOrEmpty(VREF)) VREF = "General";

                        string basicPinName = fields[2].TrimCsvValueField();
                        string optionalFunc = fields[3].TrimCsvValueField();
                        string ConfigFunc = fields[4].TrimCsvValueField().Replace(',', '/');
                        string DiffChannels = fields[5].TrimCsvValueField();
                        string pinDesignator = fields[6].TrimCsvValueField();

                        if (!pinList.ContainsKey(pinDesignator)) pinList[pinDesignator] = new()
                        { 
                            Designator = pinDesignator, 
                            VREFGroup = VREF, 
                            Bank = bankName
                        };

                        FPGAPin pin = pinList[pinDesignator];
                        if (!vrefList.Contains(VREF)) vrefList.Add(VREF);
                        List<string> pinFuncs = new();

                        pinFuncs.AddPinFunction(ConfigFunc);
                        pinFuncs.AddPinFunction(optionalFunc);
                        pinFuncs.AddPinFunction(DiffChannels);
                        pinFuncs.AddPinFunction(basicPinName);

                        switch (basicPinName) 
                        {
                            case ("IO"):
                                pin.Type = PinType.IO;
                                break;
                            case ("TCK"):
                                pin.Type = PinType.Input;
                                pin.IsClock = true;
                                break;
                            case ("TMS"):
                            case ("TDI"):
                                pin.Type = PinType.Input;
                                break;
                            case ("TDO"):
                                pin.Type = PinType.Output;
                                break;

                            case ("nSTATUS"):
                                pin.Type = PinType.IO;
                                pin.IsLowActive = true;
                                break;

                            case ("CONF_DONE"):
                                pin.Type = PinType.IO;
                                break;

                            case ("nCONFIG"):
                            case ("nCE"):
                                pin.Type = PinType.Input;
                                pin.IsLowActive = true;
                                break;

                            case ("MSEL0"):
                            case ("MSEL1"):
                            case ("MSEL2"):
                            case ("MSEL3"):
                                pin.Type = PinType.Input;
                                break;
                        }

                        pin.Name = string.Join("/", pinFuncs.ToArray());
                        if (pin.Name.Contains("CLK") && !pin.Name.Contains("DPCLK"))
                        { 
                            pin.IsClock = true;
                            if (pin.Type == PinType.Passive) pin.Type = PinType.Input;
                        }
                    }
                }

                StringBuilder sb = new("StartComponents\n\nComponent (Name \"Cyclone10LP\") (PartCount 1) (DesPrefix \"U ? \")\n");

                int LocationX = -10000;
                int LocationY = 1000;
                int pinCount = 0;
                foreach(string vref in vrefList)
                {
                    //Console.WriteLine("\n\nVREF Group: " + vref + "\n");
                    foreach(var pin in pinList.Where(n => n.Value.VREFGroup == vref)) 
                    {
                        //Console.WriteLine(pin.Key + " | " + pin.Value.Type + " | " + pin.Value.Name + (pin.Value.IsLowActive ? " | IsLowActive " : string.Empty) + (pin.Value.IsClock ? " | IsClock " : string.Empty));
                        string row = "Pin ";
                        row += "(Location " + LocationX + ", " + LocationY + ") ";
                        row += "(Rotation 0) ";
                        
                        string pinType = pin.Value.Type.ToString();
                        //if (pinType == "IO") pinType = "I/O";

                        row += "(PinType " + pinType + ") (Length 300) (Width 0) ";

                        if (pin.Value.IsClock)
                            row += "(HasClock 1) ";

                        if (pin.Value.IsLowActive)
                            row += "(HasDot 1) ";

                        row += "(Designator Visible \"" + pin.Key + "\") ";
                        row += "(Name Visible \"" + pin.Value.Name + "\")";
                        sb.AppendLine(row);
                        Console.WriteLine(row);
                        LocationY -= 100;
                        pinCount++;

                        if (LocationY < -2000)
                        {
                            LocationX += 1000;
                            LocationY = 1000;
                        }
                    }

                    LocationX += 1000;
                    LocationY = 1000;
                }

                sb.AppendLine("EndComponent\nEndComponents\n");
                sb.ToFile(csvFileName.Replace(".csv", string.Empty) + "_altium.txt");
             
                Console.WriteLine("Pin Count = " + pinCount);
            }
        }

        // Pin (Location -1400, 1500) (Rotation 180) (PinType Power) (Length 300) (Width 0) (Designator Visible "J14") (Name Visible "VDDA_AVDAC") (PinSwap 0) (PartSwap 0) (PinSeq 1) (Part 14)
    }
}
