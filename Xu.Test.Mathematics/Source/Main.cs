using System;
using System.Windows.Forms;


namespace Xu.Test.Mathematics
{
    public partial class Main : Form
    {

        public Range<double> Range;

        public Range<DateTime> Times;

        public Main()
        {
            string test = "SMA(VOL,6,(SMA(VOL,6),VOL()6)),VOL()6;MA";
            test = "SMA(VOL,6),VOL()6;MA";
            test = "SMA([([SMA(VOL,6,(SMA(VOL,6),VOL()6)),VOL()6;M       A],6)]       ),VOL()6;MA";
            test = "SMA(CLOSE,6)";
            var list = test.GetTokens();

            foreach(string s in list) 
            {
                Console.WriteLine(s);
            }


            InitializeComponent();
            Range = new Range<double>(0);
            Times = new Range<DateTime>(DateTime.Now);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Range.Insert(new double[] { 10, 30, -1, 45.6, -34, -23, -101.123, -32.5 });
            Console.WriteLine(Range);

            Console.WriteLine(Times);

            Times.Insert(new DateTime[] { new DateTime(2000, 1, 1), new DateTime(1998, 2, 23), new DateTime(2018, 10, 21) });

            Console.WriteLine(Times);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Range.Insert(textBox1.Text.ToDouble());
            Console.WriteLine(Range);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Range.Set(textBox1.Text.ToDouble(), textBox2.Text.ToDouble());
            Console.WriteLine(Range);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Range.Contains(textBox1.Text.ToDouble()));
        }
    }
}
