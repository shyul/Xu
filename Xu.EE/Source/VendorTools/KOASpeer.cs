using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public static class KOASpeer
    {
        private static readonly Dictionary<string, Resistor> RK73H_Table = new Dictionary<string, Resistor>()
        {
            { "RK73H1HTTC", new Resistor(){ PackageName = "0201", Voltage = 25, PowerRating = 0.05 } }
        };


        public static readonly Dictionary<string, (string pakageName, string pack, double power, double voltage, double current)> Packages = 
            new Dictionary<string, (string pakageName, string pack, double power, double voltage, double current)>()
        {
            { "1F", ("01005", "TBL", 0.03, 30, 0.5) },
            { "1H", ("0201", "TC", 0.05, 50, 0.5) },
            { "1E", ("0402", "TP", 0.1, 75, 1) },
            { "1J", ("0603", "TD", 0.1, 75, 1) },
            { "2A", ("0805", "TD", 0.25, 150, 2) },
            { "2B", ("1206", "TD", 0.25, 200, 2) },
            { "2E", ("1210", "TD", 0.5, 200, 2) },
            { "2H", ("2010", "TE", 0.75, 200, 2) },
            { "3A", ("2512", "TE", 1, 200, 2) },
        };


        public static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();


        public static DataTable CreateEmptyTable(string packageCode)
        {
            string packageName = Packages[packageCode].pakageName;
            DataTable dt = Access.CreateTable("Resistor - " + packageName);
            dt.Rows.Add(new object[] { "NC_" + packageName, "N/A", "NC", "1/GMIN", packageName, "RES,SM," + packageName + ",NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "RESISTOR", @"Basic\Basic.SchLib", packageName +"_NC", @"Basic\Resistor.PcbLib",
                "Ideal Simulation Data", "General", "Resistor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.Rows.Add(new object[] { "0R_" + Packages[packageCode].current + "A", "210-000000A-01", "0R", "0.05", packageName,
                "RES,SM," + packageName + ",0.05R MAX," + Packages[packageCode].current + "A,THICK_FILM,-55~" + ((packageName == "01005") ? "+125" : "+155") + "DEG(TJ),AEC-Q200",
                "KOA Speer", "RK73Z" + packageCode + "T" + Packages[packageCode].pack,
                "RESISTOR", @"Basic\Basic.SchLib", packageName + ((packageName == "0201" || packageName == "0402") ? "_GREEN" : string.Empty), @"Basic\Resistor.PcbLib",
                "Ideal Simulation Data", "General", "Resistor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            return dt;
        }

        public static void PrepareTables()
        {
            foreach(string p in Packages.Keys)
            {
                DataTable dt = CreateEmptyTable(p);
                tables.Add(dt.TableName, dt);
            }
        }

        public static void WriteToTable(Resistor r)
        {
            if (!tables.Keys.Contains(r.TableName)) tables.Add(r.TableName, Access.CreateTable(r.TableName));
            DataTable dt = tables[r.TableName];
            if (!dt.Rows.Contains(r.Name))
            {
                dt.Rows.Add(r.DataRow);
            }
        }

        public static void Run()
        {
            PrepareTables();
            RK73H_Generator();
            Access.WriteToFile(tables);
        }

        public static void RK73H_Generator()
        {
            Dictionary<string, double> values = new Dictionary<string, double>();

            foreach (string p in Packages.Keys)
            {
                Resistor r = new Resistor()
                {
                    VendorName = "KOA Speer",

                    Tolerance = 1,
                    Voltage = Packages[p].voltage,
                    PackageName = Packages[p].pakageName,
                    PowerRating = Packages[p].power,

                    TableName = "Resistor - " + Packages[p].pakageName,
                };
                r.Tags.Add("AEC-Q200");

                values.Clear();

                switch (r.PackageName)
                {
                    case ("01005"):
                        {
                            r.TemperatureRange = new Range<double>(-55, 125);
                            r.FootprintName = "01005";
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(10, 2e6));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";

                                if (r.Value >= 10 && r.Value <= 91000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value > 100000 && r.Value <= 2000000)
                                {
                                    r.TemperatureCoefficient = 250;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                    case ("0201"):
                        {
                            r.FootprintName = "0201";
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(1, 9.1));
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(10, 1e6 - 1));
                            Decade.ValueGenerator(values, Decade.E24Values, new Range<double>(1e6, 1e7));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                else
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                    case ("0402"):
                        {
                            r.FootprintName = r.PackageName + "_BLUE";
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                    case ("0603"):
                        {
                            r.FootprintName = r.PackageName + "_BLUE";
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                    r.PowerRating = 0.125;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000)
                                {
                                    r.TemperatureCoefficient = 100;
                                    r.PowerRating = 0.125;
                                }
                                else if (r.Value > 1000 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                    r.PowerRating = 0.1;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                    case ("0805"):
                        {
                            r.FootprintName = r.PackageName + "_BLUE";
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000)
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                    case ("1206"):
                    case ("1210"):
                    case ("2010"):
                    case ("2512"):
                        {
                            r.FootprintName = r.PackageName + "_BLUE";
                            Decade.ValueGenerator(values, Decade.E96Values, new Range<double>(1, 1e7));
                            foreach (string val in values.Keys)
                            {
                                r.Value = values[val];
                                r.VendorPartNumber = "RK73H" + p + "T" + Packages[p].pack + val + "F";
                                if (r.Value >= 1 && r.Value < 10)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else if (r.Value >= 10 && r.Value <= 1000000)
                                {
                                    r.TemperatureCoefficient = 100;
                                }
                                else if (r.Value > 1000000 && r.Value <= 5600000)
                                {
                                    r.TemperatureCoefficient = 200;
                                }
                                else
                                {
                                    r.TemperatureCoefficient = 400;
                                }
                                WriteToTable(r);
                                //Console.WriteLine(r.VendorPartNumber);
                            }
                        }
                        break;
                }
            }
        }
    }
}
