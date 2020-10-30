using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace TestFSQ
{
    public class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, SpectrumTable st) : base(name)
        {
            SpectrumTable = st;
            SpectrumTable.Status = TableStatus.Downloading;
            SpectrumTable.DataViews.Add(this);

            AddArea(MainArea = new OscillatorArea(this, "Main", 0.3f)
            {
                HasXAxisBar = true,
                Reference = -50,
                UpperLimit = -20,
                LowerLimit = -80,
                UpperColor = Color.Green,
                LowerColor = Color.DarkOrange,
                FixedTickStep_Right = 10,

            });

            MainArea.AddSeries(MainSeries = new LineSeries(SpectrumDatum.Column_Amplitude)
            {
                Color = Color.Gray,
                IsAntialiasing = true,
                Tension = 0
            });
        }

        public override int RightBlankAreaWidth => 0;

        public SpectrumTable SpectrumTable { get; private set; }

        public OscillatorArea MainArea { get; }

        public LineSeries MainSeries { get; }

        public override string this[int i]
        {
            get
            {
                if (SpectrumTable[i] is SpectrumDatum sp && sp.Frequency is double d)
                    return d.ToString();
                else
                    return string.Empty;
            }
        }

        public override ITable Table
        {
            get => SpectrumTable;

            set
            {
                if (value is SpectrumTable st)
                    SpectrumTable = st;
                else
                    SpectrumTable = null;
            }
        }

        public override void PointerToEnd()
        {
            if (Table is ITable t)
            {
                StartPt = 0;
                IndexCount = Table.Count; //- 1;
                StopPt = t.Count;
            }
            else
            {
                StopPt = 0;
            }

            SetAsyncUpdateUI(); // async update
        }

        public override void CoordinateOverlay()
        {
            ResumeLayout(true);
            ChartBounds = new Rectangle(
                LeftYAxisLabelWidth + Margin.Left,
                Margin.Top,
                ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                ClientRectangle.Height - Margin.Top - Margin.Bottom
                );

            if (ReadyToShow)
            {
                lock (SpectrumTable.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        AxisX.TickList.Clear();

                        int tickMulti = 1;
                        int tickWidth = AxisX.TickWidth;

                        int minorTextWidth = TextRenderer.MeasureText("000", Style[Importance.Minor].Font).Width;

                        while (tickWidth <= minorTextWidth) { tickWidth++; tickMulti++; }
                        int px = 0;
                        for (int i = StartPt; i < StopPt; i++)
                        {
                            //DateTime time = m_BarTable.IndexToTime(i);
                            //if ((time.Month - 1) % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-YY")));
                            //if ((time.Month - 1) % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("MM")));





                            px++;
                        }

                        if (ChartBounds.Width > RightBlankAreaWidth)
                        {
                            AxisX.IndexCount = IndexCount;
                            AxisX.Coordinate(ChartBounds.Width - RightBlankAreaWidth);

                            int ptY = ChartBounds.Top;
                            float totalY = TotalAreaHeightRatio;

                            if (AutoScaleFit)
                            {
                                foreach (Area ca in Areas)
                                {
                                    if (ca.Visible && ca.Enabled)
                                    {
                                        if (ca.HasXAxisBar)
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY - AxisXLabelHeight).ToInt32());
                                            ptY += ca.Bounds.Height + AxisXLabelHeight;
                                            ca.TimeLabelY = ca.Bounds.Bottom + AxisXLabelHeight / 2 + 1;
                                        }
                                        else
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY).ToInt32());
                                            ptY += ca.Bounds.Height;
                                        }
                                        ca.Coordinate();
                                    }
                                }
                            }
                            else { }
                        }
                    }
            }
        }
    }
}
