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

        public void ReadXilinxPackageFile(string textFileName)
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

                Console.WriteLine("Total Number of Pins = " + PinList.Count);


            }

        }

        public void ReadAltiumPinMapReport(string csvFileName)
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

                        if (pin.PinName != pinName)
                        {
                            Console.WriteLine("Error: pinName: " + pin.PinName + " | " + pinName);
                        }
                        else
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
                sb.AppendLine(pin.Value.CsvLine);
            }

            File.WriteAllText(csvFileName, sb.ToString());
        }

        public void ImportPins(string csvFileName)
        {



        }

        public void ExportXdcConstraint(string xdcFileName)
        {
            StringBuilder sb = new();

            foreach (string pairName in DiffPairs.OrderBy(n => n))
            {
                sb.AppendLine("make_diff_pair_ports " + pairName + "_P " + pairName + "_N");
            }

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.Bank).ThenBy(n => n.IOType).ThenBy(n => n.PairName).ThenBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                sb.AppendLine("set_property -dict {PACKAGE_PIN " + pin.Designator + " IOSTANDARD " + pin.IOStandard + "} [get_ports " + pin.NetName.Replace('.', 'p') + "]");
            }

            foreach (var pin in PinList.Values.Where(n => n.IsIO && n.NetName.Length > 0 && !n.NetName.StartsWith("Net")).OrderBy(n => n.Bank).ThenBy(n => n.IOType).ThenBy(n => n.PairName).ThenBy(n => n.NetName).ThenBy(n => n.Designator))
            {
                sb.AppendLine("set_property DIRECTION OUT [get_ports " + pin.NetName.Replace('.', 'p') + "]");
                //set_property  DIRECTION OUT [get_ports DAC_D11_N]
            }

            File.WriteAllText(xdcFileName, sb.ToString());
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

        public bool IsIO { get; set; } = false;

        public bool IsPower { get; set; } = false;

        public string NetName { get; set; }

        public string AssignedName { get; set; }

        public string PairName { get; set; }

        public string IOStandard { get; set; } //= "LVCMOS33";

        public string PullType { get; set; }

        public string CurrentDrive { get; set; }

        public double PackageDelayTime { get; set; } = 0;

        public double PackageDelayLength { get; set; } = 0;

        public const string CsvHeader = "Designator,Pin Name,Net Name,IO Standard,PullType,CurrentDrive,Bank,IO Type,Memory Byte Group,Super Logic Region,Package Delay Time,PackageDelayLength\n";

        public string CsvLine => Designator + "," + PinName + "," + NetName + "," + IOStandard + "," + PullType + "," + CurrentDrive + "," + Bank + "," + IOType + "," + MemoryByteGroup + "," + SuperLogicRegion + "," + PackageDelayTime + "," + PackageDelayLength;
    }

    public class FPGABank
    {
        public string PowerPinName { get; set; }

        public string PowerPinNetName { get; set; }

    }
}
