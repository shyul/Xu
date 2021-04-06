using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE
{
    public interface IPort
    {
        string Name { get; }

        bool Enabled { get; }
    }

    public interface IDigitalPort : IPort
    {
        bool Value { get; set; }
    }

    public interface ISerialDataInput : IDigitalPort
    {
        List<bool> Data { get; }
    }

    public interface IAnalogPort : IPort
    {
        double Value { get; set; }
    }

    public interface ITriggerSource : IPort 
    {
        TriggerEdge TriggerEdge { get; set; }
    }

    public interface IReceiver : IPort
    {
        void StartReceive();

        bool IsReceiving { get; }

        double SampleRate { get; set; }

        List<double> Samples { get; set; }
    }

    public interface ITransmitter : IPort
    {
        void StartTransmit();

        bool IsTransmitting { get; }

        double SampleRate { get; set; }

        List<double> Samples { get; set; }
    }
}
