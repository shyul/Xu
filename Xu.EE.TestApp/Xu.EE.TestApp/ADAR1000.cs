using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;

namespace ADAR1000
{
    public class RegisterValue
    {
        public int OffsetAddress { get; set; }

        public int Data { get; set; }
    }

    public interface IRegister
    {
        int BaseAddress { get; } 

        List<RegisterValue> RegisterValues { get; }

        void UpdateRegisterValues();
    }

    public class ADAR1000 
    {
        // 0 ~ 121
        public Dictionary<int, BeamPosition> RxBeamPositionList { get; } = new Dictionary<int, BeamPosition>();

        // 0 ~ 121
        public Dictionary<int, BeamPosition> TxBeamPositionList { get; } = new Dictionary<int, BeamPosition>();

        // 1 ~ 7
        public Dictionary<int, ReceiverBias> RxBiasList { get; } = new Dictionary<int, ReceiverBias>();

        // 1 ~ 7
        public Dictionary<int, TransmitterBias> TxBiasList { get; } = new Dictionary<int, TransmitterBias>();

        public void WriteAllRegisters() 
        {
        
        }

        public void SetTxBeamChannel(int position, int channel, bool atten, int gain, double phase)
        {
        
        }

        public void WriteTxRegisters(int position) 
        {
        
        
        }

        public void SwitchTxPosition(int position) 
        {
        
        
        }

        public int BaseAddress { get; set; }

        public Dictionary<(char, char), double> RegToPhaseLUT { get; } = new Dictionary<(char, char), double>();

        public Dictionary<Range<double>, (char, char)> PhaseToRegLUT { get; } = new Dictionary<Range<double>, (char, char)>();
    }

    public class BeamPosition : IRegister
    {
        // 1 ~ 4
        public Dictionary<int, BeamPositionChannel> Channels { get; } = new Dictionary<int, BeamPositionChannel>();

        public int BaseAddress { get; set; }

        public List<RegisterValue> RegisterValues { get; } = new List<RegisterValue>();

        public void UpdateRegisterValues()
        {
     
        }
    }

    public class BeamPositionChannel
    {
        public double Phase
        {
            get => m_Phase;
            set => m_Phase = value - (Math.Truncate(value / 360) * 360);
        }

        private double m_Phase;

        public int Gain
        {
            get => m_Gain;

            set
            {
                if (value < 0)
                    m_Gain = 0;
                else if (value > 127)
                    m_Gain = 127;
                else
                    m_Gain = value;
            }
        }

        private int m_Gain;

        public bool Attenuator { get; set; }

        public int RegisterValue { get; }
    }



    public class ReceiverBias
    {
        public int LNA_Bias { get; set; }
        
        public int VM_Bias { get; set; }

        public int VGA_Bias { get; set; }

        public int ExternalLNABias_On { get; set; }

        public int ExternalLNABias_Off { get; set; }

        public char[] RegisterValue { get; }
    }

    public class TransmitterBias 
    {
        public int External_PA_Ch1_On { get; set; }

        public int External_PA_Ch1_Off { get; set; }

        public int External_PA_Ch2_On { get; set; }

        public int External_PA_Ch2_Off { get; set; }

        public int External_PA_Ch3_On { get; set; }

        public int External_PA_Ch3_Off { get; set; }

        public int External_PA_Ch4_On { get; set; }

        public int External_PA_Ch4_Off { get; set; }

        public int Driver_Bias { get; set; }

        public int VM_Bias { get; set; }

        public int VGA_Bias { get; set; }
    }
}
