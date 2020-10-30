using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace TestFSQ
{
    public class SpectrumTable : ITable
    {
        private HashSet<SpectrumDatum> Rows { get; } = new HashSet<SpectrumDatum>();

        public void Add(SpectrumDatum sp)
        {
            lock (Rows)
                if (Rows.Contains(sp))
                    Rows.Where(n => n.Equals(sp)).First().Amplitude = sp.Amplitude;
                else
                    Rows.Add(sp);
        }

        public SpectrumDatum this[int i]
        {
            get
            {
                lock (Rows)
                    if (i >= Count || i < 0)
                        return null;
                    else
                        return Rows.ElementAt(i);
            }
        }

        public double this[int i, NumericColumn column]
        {
            get
            {
                lock (Rows)
                    if (i >= Count || i < 0 || Count == 0)
                        return double.NaN;
                    else
                        return Rows.ElementAt(i)[column];
            }
        }

        public int Count => Rows.Count;

        public void Clear()
        {
            lock (Rows)
                Rows.Clear();
        }

        public bool ReadyToShow => Count > 0 && Status != TableStatus.Default && Status != TableStatus.Loading && Status != TableStatus.Downloading && Status != TableStatus.Maintaining;// (Status == TableStatus.Ready || Status == TableStatus.CalculateFinished || Status == TableStatus.TickingFinished);

        public TableStatus Status
        {
            get => m_Status;

            set
            {
                m_Status = value;

                if (m_Status == TableStatus.CalculateFinished)
                {
                    lock (DataViews) DataViews.ForEach(n => { n.ReadyToShow = true; n.PointerToEnd(); });
                }
                else if (!ReadyToShow)
                {
                    lock (DataViews) DataViews.ForEach(n => { n.ReadyToShow = false; n.SetAsyncUpdateUI(); });
                }
            }
        }

        private TableStatus m_Status = TableStatus.Default;

        public List<IDataView> DataViews { get; } = new List<IDataView>();

        public object DataLockObject { get; } = new object();
    }
}
