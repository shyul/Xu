using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xu;

namespace Xu.EE
{
    public abstract class SpectrumAnalyzerChannel
    {
        public double Center
        {
            get => (FrequencyRange.Maximum + FrequencyRange.Minimum) / 2;

            set 
            {
                double halfSpan = Span / 2;
                double center = value;
                FrequencyRange.Set(center - halfSpan, center + halfSpan);
            }
        }

        public double Span
        {
            get => FrequencyRange.Maximum - FrequencyRange.Minimum;

            set 
            {
                double center = Center;
                double halfSpan = value / 2;
                FrequencyRange.Set(center - halfSpan, center + halfSpan);
            }
        }

        public double Start { get => FrequencyRange.Minimum; }

        public double Stop { get => FrequencyRange.Maximum; }

        public Range<double> FrequencyRange { get; } = new Range<double>(499e6, 501e6);

        public bool IsAutoRBW { get; set; } = true;

        public double RBW { get; set; }

        public double ReferenceLevel { get; set; }

        public bool IsAutoAttenuation { get; set; }

        public double InputAttenuation { get; set; }


    }
}
