using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace Xu.EE.FPGA.FW
{
    public class FPGA
    {
        public string Designator { get; set; } = "U1";

        public Dictionary<string, FPGAPin> PinList { get; } = new Dictionary<string, FPGAPin>();

        public string PowerPinPrefix { get; set; } = "VCC";

        public string GroundPinPrefix { get; set; } = "GND";

        public Dictionary<string, string> PowerPins { get; } = new();

        public Dictionary<string, FPGABank> Banks { get; } = new();

        public HashSet<string> DiffPairs { get; } = new();

        public void ImportXilinxPackageFile(string textFileName)
        {
            if (File.Exists(textFileName))
            {
                using var fs = new FileStream(textFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();

                    if (line.Length > 0 && !line.StartsWith("--") && !line.StartsWith("Pin") && !line.StartsWith("Total Number"))
                    {
                        string[] fields = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (fields.Length != 6) Console.WriteLine("Error Read Line: " + fields.ToStringWithIndex());

                        FPGAPin pin = new()
                        {
                            Designator = fields[0],
                            PinName = fields[1],
                            MemoryByteGroup = fields[2] != "NA" ? fields[2] : null,
                            Bank = fields[3] != "NA" ? fields[3] : null,
                            IOType = fields[4] != "NA" ? fields[4] : null,
                            SuperLogicRegion = fields[5] != "NA" ? fields[5] : null,
                        };

                        pin.IsIO = pin.IOType == "HD" || pin.IOType == "HP";

                        PinList[pin.Designator] = pin;

                        if (pin.PinName.StartsWith(PowerPinPrefix) || pin.PinName.StartsWith(GroundPinPrefix))
                        {
                            PowerPins[pin.PinName] = string.Empty;
                            pin.IsPower = true;

                            if (pin.Bank is not null)
                            {
                                Banks[pin.Bank] = new FPGABank() { PowerPinName = pin.PinName, PowerPinNetName = string.Empty };
                            }
                        }
                    }
                }

                foreach (var pin in PinList.Where(n => n.Value.Bank is not null && n.Value.IsIO).OrderBy(n => n.Value.Bank))
                {
                    Console.WriteLine(pin.Value.Designator + " = " + pin.Value.PinName);
                }

                Console.WriteLine("\nTotal Number of Pins = " + PinList.Count);


            }

        }

        public void ImportAlteraPackageFile(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);

                string line = sr.ReadLine().Trim();
                string[] fields = line.Split(',');

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Trim();
                    fields = line.Split(',');

                    string designator = fields[0].Trim();
                    string bank = fields[1].Trim();
                    string ioType = fields[2].Trim();

                    string pinName = string.Empty;

                    string func3 = fields[5].Trim();
                    string func4 = fields[6].Trim();
                    bool isIO = false;

                    if (func4 == "TDI") pinName = func4;
                    else if (func4 == "TDO") pinName = func4;
                    else if (func4 == "TMS") pinName = func4;
                    else if (func4 == "TCK") pinName = func4;
                    else if (func4 == "JTAGEN") pinName = func4;
                    else if (func4 == "CONFIG_SEL") pinName = func4;
                    else if (func4 == "nCONFIG") pinName = func4;
                    else if (func4 == "nSTATUS") pinName = func4;
                    else if (func4 == "CONF_DONE") pinName = func4;
                    else if (func3.Contains("VREFB")) pinName = func3;
                    else
                    {
                        string func1 = fields[3].Trim();
                        string func2 = fields[4].Trim();

                        if (func4.Length > 0) pinName += func4 + "/";
                        if (func3.Length > 0) pinName += func3 + "/";
                        if (func2.Length > 0) pinName += func2 + "/";
                        if (func1.Length > 0) pinName += func1 + "/";

                        isIO = func1 == "IO";
                    }

                    pinName = pinName.Trim('/');

                    FPGAPin pin = new()
                    {
                        Designator = fields[0],
                        PinName = pinName,
                        Bank = string.IsNullOrEmpty(bank) ? null : bank,
                        IOType = string.IsNullOrEmpty(ioType) ? null : ioType,
                        IsIO = isIO
                    };



                    PinList[pin.Designator] = pin;

                    if (pin.PinName.StartsWith(PowerPinPrefix) || pin.PinName.StartsWith(GroundPinPrefix))
                    {
                        PowerPins[pin.PinName] = string.Empty;
                        pin.IsPower = true;

                        if (pin.Bank is not null)
                        {
                            Banks[pin.Bank] = new FPGABank() { PowerPinName = pin.PinName, PowerPinNetName = string.Empty };
                        }
                    }
                }

                foreach (var pin in PinList.Where(n => n.Value.Bank is not null && n.Value.IsIO).OrderBy(n => n.Value.Bank))
                {
                    Console.WriteLine(pin.Value.Designator + " = " + pin.Value.PinName);
                }

                Console.WriteLine("\nTotal Number of Pins = " + PinList.Count);
            }
        }

        public void ImportQuartusPinFile(string pinFileName)
        {
            if (File.Exists(pinFileName))
            {
                using var fs = new FileStream(pinFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);

                int i = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();

                    if (!line.StartsWith("-") && line.Split(':') is string[] fields && fields.Length == 7 && !line.Contains("Pin Name/Usage")) 
                    {
                        //Console.WriteLine(ToStringWithIndex(fields));
                        i++;
                        string pinDesignator = fields[1].Trim().ToUpper();
                        FPGAPin pin = PinList[pinDesignator];
                        pin.AssignedName = fields[0].Trim().ToUpper();
                        string voltage = fields[4].Trim().ToUpper();
                        if (voltage.Length > 0) pin.AssignedName = voltage;
                        /*
                        if (pin.AssignedName != pin.NetName) // fields[6].Trim() == "Y" &&  
                        {
                            Console.WriteLine("Found Assignment Difference: " + pin.NetName + " | " + pin.AssignedName);
                        }*/
                    }
                }

                foreach(var pin in PinList.Values.Where(n => n.AssignedName != n.NetName.Replace('.', 'p').ToUpper() && !(n.PinName == "GND" && n.NetName == "VSS")).OrderBy(n => n.AssignedName)) 
                {
                    Console.WriteLine("Found Assignment Difference," + pin.Designator + "," + pin.PinName + "," + pin.NetName + "," + pin.AssignedName);
                }

                Console.WriteLine("\n\nQuartus Pin Imported: " + i);
            }
        }

        public void ImportVivadoIOPlacedReport(string rptFileName)
        {
            if (File.Exists(rptFileName))
            {
                using var fs = new FileStream(rptFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);

                int i = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();

                    if (line.StartsWith("|") && line.Split('|') is string[] fields && fields.Length == 23 && !line.Contains("Pin Number"))
                    {
                        //Console.WriteLine(ToStringWithIndex(fields));
                        i++;

                        string pinDesignator = fields[1].Trim().ToUpper();
                        FPGAPin pin = PinList[pinDesignator];
                        pin.AssignedName = fields[2].Trim().ToUpper();

                        if (pin.AssignedName.Contains("DDR4_")) pin.AssignedName = pin.AssignedName.Replace("DDR4_", "PS_DDR_");

                        string voltage = fields[13].Trim().ToUpper();
                        if (voltage.Length > 0 && voltage != "0.0") pin.AssignedName = voltage;

                    }

                }

                foreach (var pin in PinList.Values.Where(n => n.AssignedName != n.NetName.Replace('.', 'p').ToUpper() && !(n.PinName == "GND" && n.NetName == "VSS")).OrderBy(n => n.AssignedName))
                {
                    Console.WriteLine("Found Assignment Difference," + pin.Designator + "," + pin.PinName + "," + pin.NetName + "," + pin.AssignedName);
                }

                Console.WriteLine("\n\nVivado Report: Pin Imported: " + i);
            }
        }

        public static string ToStringWithIndex(string[] fields)
        {
            string s = string.Empty;
            for (int i = 0; i < fields.Length; i++)
            {
                s += "(" + i.ToString() + ")\"" + fields[i].Trim().ToUpper() + "\"-";
            }

            if (s.Length > 0) return s.TrimEnd('-');
            else return "Empty String Array";
        }

        public void ImportAltiumPinMapReport(string csvFileName, bool checkPinName = true)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);

                HashSet<string> DesignatorList = new();

                int designatorIndex = -1;// 0;
                int netNameIndex = -1;// 1;
                int pinNameIndex = -1;// 2;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();

                    if (line.Contains("Pin Designator") && line.Contains("Net Name") && line.Contains("Display Name"))
                    {
                        string[] headers = line.Split(',');

                        int i = 0;
                        foreach (string header in headers)
                        {
                            if (header.Trim() == "Pin Designator") designatorIndex = i;
                            if (header.Trim() == "Net Name") netNameIndex = i;
                            if (header.Trim() == "Display Name") pinNameIndex = i;
                            i++;
                        }
                    }
                    else if (designatorIndex >= 0)
                    {
                        string[] fields = line.Split(',');

                        string pinDesignator = fields[designatorIndex];
                        string pinName = fields[pinNameIndex];
                        string netName = fields[netNameIndex];

                        if (!DesignatorList.Contains(pinDesignator)) DesignatorList.Add(pinDesignator);

                        FPGAPin pin = PinList[pinDesignator];

                        if (pin.PinName != pinName && checkPinName)
                        {
                            Console.WriteLine("Error: pinName: " + pin.PinName + " | " + pinName);
                        }

                        if (true) // keep it here for now.
                        {
                            pin.NetName = netName;
                            if (pin.IsIO && (pin.NetName.EndsWith("_N") || pin.NetName.EndsWith("_P")))
                            {
                                pin.IOStandard = "LVDS";

                                pin.PairName = netName.Substring(0, netName.Length - 2);

                                if (!DiffPairs.Contains(pin.PairName)) DiffPairs.Add(pin.PairName);

                                Console.WriteLine("pairName = " + pin.PairName);
                            }
                        }

                        if (pin.IsPower)
                        {
                            if (string.IsNullOrWhiteSpace(PowerPins[pin.PinName]))
                            {
                                PowerPins[pin.PinName] = netName;

                                if (!string.IsNullOrWhiteSpace(pin.Bank))
                                {
                                    PinList.Values.Where(n => n.Bank == pin.Bank && n.IsIO).ToList().ForEach(n => {
                                        if (n.IOStandard is null)
                                        {
                                            if (netName.StartsWith("+3.3V")) { n.IOStandard = "LVCMOS33"; if (n.IOType == "HP") Console.WriteLine("Bank " + n.Bank + ": HP Bank does not support 3.3V standard."); }
                                            else if (netName.StartsWith("+2.5V")) { n.IOStandard = "LVCMOS25"; if (n.IOType == "HP") Console.WriteLine("Bank " + n.Bank + ": HP Bank does not support 3.3V standard."); }
                                            else if (netName.StartsWith("+1.8V")) n.IOStandard = "LVCMOS18";
                                        }
                                    });
                                }
                            }
                            else if (PowerPins[pin.PinName] != netName)
                            {
                                Console.WriteLine("Power Pin [" + pin.PinName + "] has two nets: " + PowerPins[pin.PinName] + " and " + netName);
                            }

                        }
                    }
                }

                Console.WriteLine(DesignatorList.Count + " of pins imported from Altium Pin Map File!");

                if (DesignatorList.Count != PinList.Count) Console.WriteLine("Altium Import Error: PinList actually has: " + PinList.Count);

                Console.WriteLine("\n");

                foreach (var powerPin in PowerPins)
                {
                    Console.WriteLine("PowerPin: " + powerPin.Key + " = " + powerPin.Value);
                }
            }
        }

        public void ExportPins(string csvFileName)
        {
            StringBuilder sb = new(FPGAPin.CsvHeader);
            foreach (var pin in PinList.OrderBy(n => n.Value.Bank).ThenBy(n => n.Value.IOType).ThenBy(n => n.Value.PinName).ThenBy(n => n.Value.Designator))
            {
                sb.AppendLine(pin.Value.CsvLine);// 
            }

            File.WriteAllText(csvFileName, sb.ToString());
        }

        public void ExportAllSignals(string fileName)
        {
            StringBuilder sb = new();

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                string pinName = pin.NetName.ToLower().Replace('.', 'p');

                if (pinName.EndsWith("_p"))
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_P";
                else if (pinName.EndsWith("_n"))
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_N";

                sb.AppendLine(pinName + ",");
            }

            File.WriteAllText(fileName, sb.ToString());

        }

        public void ImportPins(string csvFileName)
        {



        }

        public void ExportXdcConstraint(string xdcFileName)
        {
            StringBuilder sb = new();

            foreach (string pairName in DiffPairs.OrderBy(n => n))
            {
                sb.AppendLine("make_diff_pair_ports " + pairName.ToLower() + "_P " + pairName.ToLower() + "_N");
            }

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.Bank).ThenBy(n => n.IOType).ThenBy(n => n.PairName).ThenBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                string pinName = pin.NetName.ToLower().Replace('.', 'p');

                if (pinName.EndsWith("_p")) 
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_P";
                else if (pinName.EndsWith("_n"))
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_N";

                sb.AppendLine("set_property -dict {PACKAGE_PIN " + pin.Designator + " IOSTANDARD " + pin.IOStandard + "} [get_ports " + pinName + "]");
            }

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.Bank).ThenBy(n => n.IOType).ThenBy(n => n.PairName).ThenBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                string pinName = pin.NetName.ToLower().Replace('.', 'p');

                if (pinName.EndsWith("_p"))
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_P";
                else if (pinName.EndsWith("_n"))
                    pinName = pinName.Substring(0, pinName.Length - 2) + "_N";

                sb.AppendLine("set_property DIRECTION OUT [get_ports " + pinName + "]");
                //set_property  DIRECTION OUT [get_ports DAC_D11_N]
            }

            File.WriteAllText(xdcFileName, sb.ToString());
        }

        public void ExportQuartusSpecFileAssignment(string qsfFileName)
        {
            StringBuilder sb = new();

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                sb.AppendLine("set_location_assignment PIN_" + pin.Designator + " -to " + pin.NetName.ToLower().Replace('.', 'p'));
            }

            File.WriteAllText(qsfFileName, sb.ToString());
        }


    }


    public class FPGAPin
    {
        public string Designator { get; set; }

        public string PinName { get; set; }

        public string MemoryByteGroup { get; set; }

        public string Bank { get; set; }

        public string IOType { get; set; }

        public string SuperLogicRegion { get; set; }

        public bool IsPower { get; set; } = false;

        public bool IsIO { get; set; } = false;

        public string Direction { get; set; }

        public string NetName { get; set; }

        public string AssignedName { get; set; }

        public string PairName { get; set; }

        public string IOStandard { get; set; } //= "LVCMOS33";

        public string PullType { get; set; }

        public string CurrentDrive { get; set; }

        public double PackageDelayTime { get; set; } = 0;

        public double PackageDelayLength { get; set; } = 0;

        public const string CsvHeader = "Designator,Pin Name,Net Name,Assigned Name,Direction,IO Standard,PullType,CurrentDrive,Bank,IO Type,Memory Byte Group,Super Logic Region,Package Delay Time,PackageDelayLength\n";

        public string CsvLine => Designator + "," + PinName + "," + NetName + "," + AssignedName + "," + Direction + "," + IOStandard + "," + PullType + "," + CurrentDrive + "," + Bank + "," + IOType + "," + MemoryByteGroup + "," + SuperLogicRegion + "," + PackageDelayTime + "," + PackageDelayLength;
    }

    public class FPGABank
    {
        public string PowerPinName { get; set; }

        public string PowerPinNetName { get; set; }

    }
}
