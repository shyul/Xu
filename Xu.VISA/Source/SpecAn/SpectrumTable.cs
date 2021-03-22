using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Xu.EE.Visa
{
    public class SpectrumTable : ITable, IDataProvider
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

        public bool ReadyToShow => Count > 0 && Status >= TableStatus.DataReady;

        public TableStatus Status
        {
            get => m_Status;

            set
            {
                m_Status = value;

                lock (DataConsumers)
                {
                    if (ReadyToShow)
                    {
                        if (m_Status == TableStatus.CalculateFinished)
                        {
                            foreach (var idc in DataConsumers) if(idc is IDataRenderer idr) idr.PointerToEnd();
                        }
                    }

                    DataConsumers.ForEach(n => n.DataIsUpdated(this));
                }
            }
        }

        private TableStatus m_Status = TableStatus.Default;

        public List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk) => DataConsumers.CheckAdd(idk);

        public bool RemoveDataConsumer(IDataConsumer idk) => DataConsumers.CheckRemove(idk);

        public DateTime UpdateTime { get; private set; } = TimeTool.MinInvalid;

        public object DataLockObject { get; } = new object();
    }
}
