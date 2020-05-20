using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Xu.EE
{
    public static class Access
    {
        public static OleDbConnection Connect(string fileName)
            => new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Persist Security Info = False; ");

        public static bool TableExists(this OleDbConnection conn, string tableName)
            => conn.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;

        public static void DeleteTable(this OleDbConnection conn, string tableName)
        {
            using (OleDbCommand cmd = new OleDbCommand("DROP TABLE [" + tableName + "]", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreateTable(this OleDbConnection conn, DataTable dt)
        {
            if (!conn.TableExists(dt.TableName))
            {
                string sqlCreateCmd = "CREATE TABLE [" + dt.TableName + "](";
                foreach (DataColumn dc in dt.Columns)
                {
                    sqlCreateCmd += "[" + dc.ColumnName + "] VARCHAR(255), ";
                }

                if (dt.PrimaryKey.Count() > 0)
                {
                    sqlCreateCmd += "PRIMARY KEY (";
                    foreach (DataColumn dc in dt.PrimaryKey)
                    {
                        sqlCreateCmd += "[" + dc.ColumnName + "],";
                    }
                    sqlCreateCmd = sqlCreateCmd.Trim().Trim(',') + ")";
                }

                sqlCreateCmd = sqlCreateCmd.Trim().Trim(',') + ");";
                Console.WriteLine("Creating Table: " + sqlCreateCmd);
                OleDbCommand addtablecmd = new OleDbCommand(sqlCreateCmd, conn);
                addtablecmd.ExecuteNonQuery();
            }
        }

        public static void PrepareTable(this OleDbConnection conn, DataTable dt)
        {
            if (TableExists(conn, dt.TableName))
                DeleteTable(conn, dt.TableName);
            CreateTable(conn, dt);
        }

        public static void InsertTable(this OleDbConnection conn, DataTable dt)
        {
            string columnLine = "(";
            string valueLine = "(";
            Dictionary<string, string> columnToValue = new Dictionary<string, string>();

            foreach (DataColumn dc in dt.Columns)
            {
                string colName = "[" + dc.ColumnName + "]";
                string valueName = "@" + dc.ColumnName.ToLower().Replace(" ", string.Empty);

                columnToValue.Add(dc.ColumnName, valueName);
                columnLine += colName + ", ";
                valueLine += valueName + ", ";
            }

            columnLine = columnLine.Trim().Trim(',') + ")";
            valueLine = valueLine.Trim().Trim(',') + ")";

            using (OleDbCommand cmd = new OleDbCommand("INSERT INTO [" + dt.TableName + "]" + columnLine + " VALUES " + valueLine + ";", conn))
            {
                Console.WriteLine(cmd.CommandText);

                foreach (string volName in columnToValue.Values)
                    cmd.Parameters.Add(volName, OleDbType.VarChar, 255);

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        cmd.Parameters[columnToValue[dc.ColumnName]].Value = dr[dc.ColumnName].ToString();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable CreateTable(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            DataColumn keyColumn = new DataColumn("Component Name", typeof(string));
            dt.Columns.Add(keyColumn);
            dt.Columns.Add("Item ID", typeof(string));
            dt.Columns.Add("Comment", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Package Name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Mfg Name", typeof(string));
            dt.Columns.Add("Mfg PN", typeof(string));
            dt.Columns.Add("Library Ref", typeof(string));
            dt.Columns.Add("Library Path", typeof(string));
            dt.Columns.Add("Footprint Ref", typeof(string));
            dt.Columns.Add("Footprint Path", typeof(string));
            dt.Columns.Add("Sim Description", typeof(string)); // Vendor Data
            dt.Columns.Add("Sim Kind", typeof(string)); // General
            dt.Columns.Add("Sim SubKind", typeof(string)); // Spice Subcircuit
            dt.Columns.Add("Sim Spice Prefix", typeof(string)); // X
            dt.Columns.Add("Sim Netlist", typeof(string)); // @DESIGNATOR %1 %2 @MODEL
            dt.Columns.Add("Sim Port Map", typeof(string)); // (1:Port1),(2:Port2)
            dt.Columns.Add("Sim File", typeof(string)); // Basic\Simulation\Murata\GRM.ckt *** first three letters.
            dt.Columns.Add("Sim Model Name", typeof(string)); // --- VendorPartNumber ---
            dt.Columns.Add("Sim Parameters", typeof(string));

            //dt.Columns.Add("Update Values", typeof(string));
            //dt.Columns.Add("Add To Design", typeof(string));
            //dt.Columns.Add("Visible On Add", typeof(string));
            //dt.Columns.Add("Remove From Design", typeof(string));

            dt.PrimaryKey = new DataColumn[] { keyColumn };
            dt.AcceptChanges();
            return dt;
        }


        public static void BuildPartNumberWithSim()
        {
            FileInfo[] Files = UtilSerialization.GetFiles(@"C:\Users\shyul\OneDrive - Facebook\Projects\Altium\Library\Basic\Simulation\Murata", "*.ckt");
            foreach (FileInfo file in Files)
            {
                //Console.WriteLine(file.FullName); //file.Name;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith(".SUBCKT"))
                        {
                            string[] vals = line.Split(' ');
                            //Console.WriteLine(vals[1]);
                            Murata.PartNumberWithSim.CheckAdd(vals[1]);
                        }
                    }

                }
                
            }
        }

        public static void ImportMurataCapacitors()
        {
            Dictionary<string, DataTable> ds = new Dictionary<string, DataTable>();

            DataTable dt = CreateTable("Capacitor - 008004");
            dt.Rows.Add(new object[] { "NC_008004", "N/A", "NC", "0pF", "008004", "CAP,SM,008004,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "008004_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 01005");
            dt.Rows.Add(new object[] { "NC_01005", "N/A", "NC", "0pF", "01005", "CAP,SM,01005,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "01005_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 015008");
            dt.Rows.Add(new object[] { "NC_015008", "N/A", "NC", "0pF", "015008", "CAP,SM,015008,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "015008_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 0201");
            dt.Rows.Add(new object[] { "NC_0201", "N/A", "NC", "0pF", "0201", "CAP,SM,0201,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "0201_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 0402");
            dt.Rows.Add(new object[] { "NC_0402", "N/A", "NC", "0pF", "0402", "CAP,SM,0402,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "0402_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 0603");
            dt.Rows.Add(new object[] { "NC_0603", "N/A", "NC", "0pF", "0603", "CAP,SM,0603,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "0603_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 0704");
            dt.Rows.Add(new object[] { "NC_0704", "N/A", "NC", "0pF", "0704", "CAP,SM,0704,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "0704_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 0805");
            dt.Rows.Add(new object[] { "NC_0805", "N/A", "NC", "0pF", "0805", "CAP,SM,0805,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "0805_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 1111");
            dt.Rows.Add(new object[] { "NC_1111", "N/A", "NC", "0pF", "1111", "CAP,SM,1111,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "1111_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 1206");
            dt.Rows.Add(new object[] { "NC_1206", "N/A", "NC", "0pF", "1206", "CAP,SM,1206,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "1206_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 1210");
            dt.Rows.Add(new object[] { "NC_1210", "N/A", "NC", "0pF", "1210", "CAP,SM,1210,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "1210_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 1808");
            dt.Rows.Add(new object[] { "NC_1808", "N/A", "NC", "0pF", "1808", "CAP,SM,1808,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "1808_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 1812");
            dt.Rows.Add(new object[] { "NC_1812", "N/A", "NC", "0pF", "1812", "CAP,SM,1812,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "1812_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            dt = CreateTable("Capacitor - 2220");
            dt.Rows.Add(new object[] { "NC_2220", "N/A", "NC", "0pF", "2220", "CAP,SM,2220,NO POP/DEBUG/PLACE HOLDER", "N/A", "N/A",
                "CAPACITOR", @"Basic\Basic.SchLib", "2220_NC", @"Basic\Capacitor.PcbLib",
                "Ideal Simulation Data", "General", "Capacitor", "X", string.Empty, "(1:1),(2:2)", string.Empty, "Ideal" });
            dt.AcceptChanges();
            ds.Add(dt.TableName, dt);

            string filePath = @"C:\Users\shyul\OneDrive - Facebook\Projects\Altium\Tools\Components\capacitor_murata\";
            string[] files = new string[] { "GRM", "GJM", "GQM", "GCM", "GCQ" };

            foreach (string file in files)
            {
                DataTable csv = GetDataTableFromCsv(filePath + file + ".csv");

                foreach (DataRow dw in csv.Rows)
                {
                    string mfrpn = dw["Part number (#:Packing code)"].ToString(); // Console.WriteLine(dw["Part number (#:Packing code)"].ToString());
                    (Capacitor c, bool isValid) = Murata.DecodeCapacitor(mfrpn);

                    if (isValid)
                    {
                        if (!ds.Keys.Contains(c.TableName)) ds.Add(c.TableName, CreateTable(c.TableName));
                        dt = ds[c.TableName];
                        if (!dt.Rows.Contains(c.Name))
                        {
                            dt.Rows.Add(c.DataRow);
                        }
                        else
                        {
                            DataRow[] foundRows = dt.Select("[Component Name]='" + c.Name + "'");

                            string l = mfrpn + ": ";

                            foreach (DataRow fr in foundRows)
                            {
                                l += fr["Mfg PN"].ToString() + ", ";
                            }

                            Console.WriteLine(l);// + dt.Rows[c.Name]["Mfg PN"]);
                        }

                    }
                }
            }

            Console.WriteLine("\n");
            foreach (string s in Murata.PackSizes.Keys)
            {
                Console.WriteLine(s + ": " + Murata.PackSizes[s]);
            }
            Console.WriteLine("\n");

            WriteToFile(ds);
            /*
            foreach (DataTable dts in ds.Values)
            {
                dts.AcceptChanges();
                using (OleDbConnection conn = Connect(@"C:\Users\shyul\OneDrive - Facebook\Projects\Altium\Library\Basic\Basic.accdb"))
                {
                    try
                    {
                        conn.Open();
                        conn.PrepareTable(dts);
                        conn.InsertTable(dts);

                        Console.WriteLine("\n\ndoing good.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failure: " + ex);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }*/

        }

        public static void WriteToFile(IDictionary<string, DataTable> ds)
        {
            foreach (DataTable dts in ds.Values)
            {
                dts.AcceptChanges();
                using (OleDbConnection conn = Connect(@"C:\Users\shyul\OneDrive - Facebook\Projects\Altium\Library\Basic\Basic.accdb"))
                {
                    try
                    {
                        conn.Open();
                        conn.PrepareTable(dts);
                        conn.InsertTable(dts);

                        Console.WriteLine("\n\ndoing good.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failure: " + ex);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader = true)
        {
            string hasHeader = isFirstRowHeader ? "Yes" : "No";
            string pathName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            using (OleDbConnection conn = new OleDbConnection(
                      @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName +
                      ";Extended Properties=\"Text;HDR=" + hasHeader + "\""))
            using (OleDbCommand command = new OleDbCommand(@"SELECT * FROM [" + fileName + "]", conn))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dt = new DataTable
                {
                    Locale = CultureInfo.CurrentCulture
                };

                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
