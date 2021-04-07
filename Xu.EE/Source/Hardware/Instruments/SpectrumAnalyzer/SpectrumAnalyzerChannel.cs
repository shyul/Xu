using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xu;

namespace Xu.EE
{
    public abstract class SpectrumAnalyzerChannel
    {
        public double Center { get; set; }

        public double Span { get; set; }

        public double Start { get => FrequencyRange.Minimum; }

        public double Stop { get => FrequencyRange.Maximum; }

        public bool IsAutoRBW { get; set; }

        public double RBW { get; set; }

        public double ReferenceLevel { get; set; }

        public bool IsAutoAttenuation { get; set; }

        public double InputAttenuation { get; set; }

        public Range<double> FrequencyRange { get; } = new Range<double>(499e6, 501e6);
    }
}
