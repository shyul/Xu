using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.FPGA
{
    public class FPGA
    {

        public string Designator { get; set; }

        public Dictionary<string, FPGAPin> PinList { get; } = new Dictionary<string, FPGAPin>();

        public void ReadXilinxPackageFile(string textFileName)
        {
            if (File.Exists(textFileName))
            {
                using var fs = new FileStream(textFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();

                    if (line.Length > 0 && !line.StartsWith("--") && !line.StartsWith("Pin"))
                    {
                        Console.WriteLine(line);
                    }


                    //if()


                }
            }

        }

        public void ReadAltiumPinMapReport(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                while (!sr.EndOfStream)
                {


                    //if()


                }
            }
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

        public bool IsIO { get; set; }

        public string NetName { get; set; }





        public string IOStandard { get; set; } = "LVCMOS33";

        public string PullType { get; set; }

    }
}
