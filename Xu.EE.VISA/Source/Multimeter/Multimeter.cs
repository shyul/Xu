using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xu.EE.Visa
{
    public class Multimeter : ViClient, IMultimeter
    {
        public Multimeter(string resourceName) : base(resourceName)
        {


        }

        public Dictionary<string, MultimeterChannel> MultimeterChannels { get; } = new();

        public void Multimeter_WriteSetting(string channelName)
        {

        }

        public void Multimeter_ReadSetting(string channelName)
        {

        }

        public double Multimeter_Read(string channelName)
        {
            return 0;
        }
    }
}
