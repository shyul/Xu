/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2018 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Xu;

namespace Mosaic
{
    sealed partial class PacMain : MosaicForm
    {
        #region Ctor
        public PacMain() : base(Program.SHOW_PACMIO)
        {

            SuspendLayout();

            HelpLink = Settings.HelpLink;
            Text = Settings.TitleText;
            // Test
            TestRibbon();
            //TestChart();
            TestDockForms();

            RibbonButton rbtn_1 = new RibbonButton(c_IBHistorial, 0, Importance.Major);

            RibbonPane rbtpane_IBClient = new RibbonPane("IBClient Test", 0);
            rbtpane_IBClient.Add(rbtn_1);


            RibbonTabItem rbtIBClient = new RibbonTabItem("IBClient");
            rbtIBClient.Add(rbtpane_IBClient, 0);

            Ribbon.AddRibbonTab(rbtIBClient);

            EnableOutputPanel();

            ResumeLayout(false);
            PerformLayout();

            //Program.IBClient = new IBClient();
            //IBClient.Connect("127.0.0.1", 15060, 156);
        }

        //public override string Text => Settings.TitleText;

        #endregion
        #region Data Components
        //private BarList DataList { get { return Program.DataList; } }
        //private IBClientOld IBClient { get { return Program.IBClient; } }
        /*
        public static void TestPublicHistoricalData()
        {
            HisoricalDataInfo info = new HisoricalDataInfo();

            info.Contract = new Contract() { Symbol = "AAPL", Exchange = Exchange.NASDAQ, Type = SecurityType.STOCK };
            info.Period = new Period(new DateTime(2018, 01, 18, 11, 0, 0), new DateTime(2018, 01, 18, 11, 05, 0));
            info.BarSize = BarSize.Second;
            info.IsRTH = false;
            info.Type = TickDataType.Trades;
            info.Param = new ParamList();

            Program.IBClient.RequestHistoricalData(info);


        }*/

        Command c_IBHistorial = new Command()
        {
            Importance = Importance.Major,
            //Enabled = false,
            Label = "Historical Data Request Awseome with more Awesome!!",
            //Bitmaps = new Dictionary<Size, Bitmap>() {
            //        { new Size(16, 16), Xu.Properties.Resources.Blank_32 },},
            //Action = () => { TestPublicHistoricalData(); },
        };

        public void EnableOutputPanel()
        {
            //Log.InvalidateListeners += (object sender, EventArgs e) => { Invalidate(true); };

            if (ObsoletedEvent.OutputPanel == null || ObsoletedEvent.OutputPanel.IsDisposed)
            {
                ObsoletedEvent.OutputPanel = new EventDockPanel("Output");
            }

            AddForm(DockStyle.Fill, 0, ObsoletedEvent.OutputPanel);
        }

        #endregion
    }

    partial class PacMain
    {
        private void TestChart()
        {
            AddForm(DockStyle.Fill, 0, new TestDockForm("Test Chart"));

        }

        private const string EODFileFullName = "E:\\US_STK_EOD.mdf";
        private const string FundamentalFileFullName = "E:\\US_STK_Fundamental.mdf";

        private const string EODConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + EODFileFullName + @";Integrated Security=True;Connect Timeout=30";
        private const string FundamentalConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + FundamentalFileFullName + @";Integrated Security=True;Connect Timeout=30";

        // Load Table
        // SELECT [time], [freq], [value], [type] FROM AAPL where [type] = 'DIVIDEND' and [freq] = 'Daily' order by [time] desc
        public static DataTable LoadTable(FileInfo DataFile, string TableName, string SqlCmd)
        {
            DataTable t = new DataTable();

            t.TableName = TableName;
            return t;
        }

        private void TestDockForms()
        {
            AddForm(DockStyle.Fill, 0, new TestDockForm("Hello"));
            AddForm(DockStyle.Fill, 0, new TestDockForm("World"));
            AddForm(DockStyle.Fill, 0, new TestDockForm("Gateway"));
            AddForm(DockStyle.Fill, 0, new TestDockForm("Wireless"));
            AddForm(DockStyle.Fill, 1, new TestDockForm("CAN Bus"));
            AddForm(DockStyle.Fill, 2, new TestDockForm("LIN Bus"));
            AddForm(DockStyle.Fill, 2, new TestDockForm("FlexRay"));
            AddForm(DockStyle.Fill, 2, new TestDockForm("ARM"));
            AddForm(DockStyle.Fill, 1, new TestDockForm("FPGA"));
            AddForm(DockStyle.Fill, 1, new TestDockForm("google"));
            AddForm(DockStyle.Fill, 2, new TestDockForm("10:20:33"));

            AddForm(DockStyle.Left, 0, new TestDockForm("google"));
            AddForm(DockStyle.Left, 0, new TestDockForm("yahoo"));
            AddForm(DockStyle.Left, 1, new TestDockForm("juniper"));
            AddForm(DockStyle.Left, 1, new TestDockForm("I-280"));
            AddForm(DockStyle.Left, 2, new TestDockForm("Golden Gate"));
            AddForm(DockStyle.Left, 2, new TestDockForm("Marin Headland"));

            AddForm(DockStyle.Right, 0, new TestDockForm("palo alto"));
            AddForm(DockStyle.Right, 0, new TestDockForm("Mountain View"));
            AddForm(DockStyle.Right, 0, new TestDockForm("san jose"));
            AddForm(DockStyle.Right, 0, new TestDockForm("san martin"));
            AddForm(DockStyle.Right, 0, new TestDockForm("Gilroy"));
            AddForm(DockStyle.Right, 1, new TestDockForm("milpitas"));
            AddForm(DockStyle.Right, 1, new TestDockForm("Reid Hillview"));

            AddForm(DockStyle.Bottom, 0, new TestDockForm("National Instruments"));
            AddForm(DockStyle.Bottom, 0, new TestDockForm("San Francisco"));
            AddForm(DockStyle.Bottom, 0, new TestDockForm("San Leandro"));
            AddForm(DockStyle.Bottom, 1, new TestDockForm("N300"));
            AddForm(DockStyle.Bottom, 1, new TestDockForm("Cessna"));
            AddForm(DockStyle.Bottom, 1, new TestDockForm("Skyhawk"));
            AddForm(DockStyle.Bottom, 2, new TestDockForm("Citabria"));
            AddForm(DockStyle.Bottom, 2, new TestDockForm("10:20:33"));

            AddForm(DockStyle.Top, 0, new TestDockForm("Oakland"));
            AddForm(DockStyle.Top, 0, new TestDockForm("Richmond"));
            AddForm(DockStyle.Top, 0, new TestDockForm("KOAK"));
            AddForm(DockStyle.Top, 1, new TestDockForm("Livermore"));
            AddForm(DockStyle.Top, 1, new TestDockForm("Tracy"));
            AddForm(DockStyle.Top, 2, new TestDockForm("Pleasanton"));
            AddForm(DockStyle.Top, 2, new TestDockForm("KLVK"));
        }
        private void TestRibbon()
        {
            ObsoletedEvent.Debug("SystemInformation.CaptionHeight = " + SystemInformation.CaptionHeight.ToString());



            Command c_StockChart = new Command()
            {
                //Enabled = false,
                Label = "StockChart",
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Mosaic.Properties.Resources.StockChart_16 },
                    { new Size(32, 32), Mosaic.Properties.Resources.StockChart_32 }
                } } },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ObsoletedEvent.Debug("StockChart is clicked"); },
            };

            Command c_Power = new Command()
            {
                //Enabled = false,
                Label = "Power Unit",
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Xu.Properties.Resources.PowerUnit_16},
                    //{ new Size(24, 24), Xu.Properties.Resources.PowerUnit_16},
                    { new Size(33, 33), Xu.Properties.Resources.PowerUnit_32 } ///??????
                } } },
                Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { ObsoletedEvent.Debug("Power Unit is clicked"); },
            };

            RibbonButton rbtn_1 = new RibbonButton(Main.Command_File_Open, 0, Importance.Major);

            RibbonButton rbtn_2 = new RibbonButton(Main.Command_File_Save, 1, Importance.Minor);

            rbtn_2.IsLineEnd = true;

            RibbonButton rbtn_3 = new RibbonButton(Main.Command_Nav_Back, 2, Importance.Minor);

            rbtn_3.IsSectionEnd = true;

            RibbonButton rbtn_4 = new RibbonButton(Main.Command_Clip_Paste, 3, Importance.Minor);

            rbtn_4.IsLineEnd = true;

            RibbonButton rbtn_5 = new RibbonButton(Main.Command_Nav_Next, 4, Importance.Minor);

            rbtn_5.IsLineEnd = true;

            RibbonButton rbtn_6 = new RibbonButton(Main.Command_Clip_Copy, 5, Importance.Minor);

            rbtn_6.IsLineEnd = true;

            RibbonButton rbtn_7 = new RibbonButton(c_StockChart, 10, Importance.Major);

            RibbonPane rbtpane = new RibbonPane("Home Pane", 1);
            rbtpane.CornerButtonCommand = c_StockChart;
            rbtpane.Add(rbtn_1);
            rbtpane.Add(rbtn_2);
            rbtpane.Add(rbtn_3);
            rbtpane.Add(rbtn_4);
            rbtpane.Add(rbtn_5);
            rbtpane.Add(rbtn_6);
            rbtpane.Add(rbtn_7);

            RibbonButton rbtn_8 = new RibbonButton(Main.Command_File_Open, 0, Importance.Major);

            RibbonButton rbtn_9 = new RibbonButton(c_StockChart, 0, Importance.Major);

            RibbonPane rbtpane2 = new RibbonPane("Test Pane", 2);
            rbtpane2.CornerButtonCommand = c_StockChart;
            rbtpane2.Add(rbtn_8);
            rbtpane2.Add(rbtn_9);

            RibbonButton rbtn_20 = new RibbonButton(Main.Command_Clip_Paste, 0, Importance.Major);
            RibbonButton rbtn_21 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_22 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_23 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Minor) { IsSectionEnd = true };

            RibbonPane rbtpane_clip = new RibbonPane("Clip Board", 0);
            rbtpane_clip.CornerButtonCommand = c_StockChart;
            rbtpane_clip.Add(rbtn_20);
            rbtpane_clip.Add(rbtn_21);
            rbtpane_clip.Add(rbtn_22);
            rbtpane_clip.Add(rbtn_23);

            RibbonButton rbtn_120 = new RibbonButton(Main.Command_Clip_Paste, 0, Importance.Major);
            RibbonButton rbtn_121 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_122 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_123 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Minor) { IsSectionEnd = true };
            RibbonButton rbtn_124 = new RibbonButton(c_StockChart, 3, Importance.Major) { IsSectionEnd = true };

            RibbonPane rbtpane_clip2 = new RibbonPane("Clip Board Pro", 0);
            rbtpane_clip2.CornerButtonCommand = c_StockChart;
            rbtpane_clip2.Add(rbtn_120);
            rbtpane_clip2.Add(rbtn_121);
            rbtpane_clip2.Add(rbtn_122);
            rbtpane_clip2.Add(rbtn_123);
            rbtpane_clip2.Add(rbtn_124);

            RibbonTabItem rbtHome = new RibbonTabItem("Home");
            rbtHome.Add(rbtpane);
            rbtHome.Add(rbtpane_clip);

            RibbonTabItem rbtView = new RibbonTabItem("View");
            rbtView.Add(rbtpane2);

            RibbonTabItem rbtCharts = new RibbonTabItem("Charts");
            rbtCharts.Add(rbtpane_clip2);

            RibbonButton rbtn_220 = new RibbonButton(c_Power, 0, Importance.Major);
            RibbonButton rbtn_221 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = false };
            RibbonButton rbtn_222 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Tiny) { IsLineEnd = false };
            RibbonButton rbtn_223 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Tiny) { IsSectionEnd = true };
            RibbonButton rbtn_224 = new RibbonButton(c_StockChart, 3, Importance.Major) { IsSectionEnd = true };

            RibbonPane rbtpane_clip3 = new RibbonPane("Strategy Board Pro", 0);
            rbtpane_clip3.Add(rbtn_220);
            rbtpane_clip3.Add(rbtn_221);
            rbtpane_clip3.Add(rbtn_222);
            rbtpane_clip3.Add(rbtn_223);
            rbtpane_clip3.Add(rbtn_224);

            RibbonTabItem rbtStrategy = new RibbonTabItem("Strategy");
            rbtStrategy.Add(rbtpane_clip3);

            Ribbon.AddRibbonTab(rbtHome);
            Ribbon.AddRibbonTab(rbtView);
            Ribbon.AddRibbonTab(rbtCharts);
            Ribbon.AddRibbonTab(rbtStrategy);
            //Ribbon.AddRibbonTab(new RibbonTab("Home") { BackColor = Color.Beige });
            //Ribbon.AddRibbonTab(new RibbonTab("View") { BackColor = Color.LightBlue });
            //Ribbon.AddRibbonTab(new RibbonTab("Charts") { BackColor = Color.LightSalmon });
            //Ribbon.AddRibbonTab(new RibbonTab("Strategy") { BackColor = Color.LightGreen });
        }
    }
}
