using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;
using System.Windows.Forms;
using System.Drawing;

namespace Xu.Test.Graphics
{
    public static class Test
    {
        public static TestGrid GridW = new TestGrid();


        public static List<(string Name, int Id, DateTime Birthday)> Data = new List<(string Name, int Age, DateTime Birthday)>()
        {
            ("Basic Dude", 21122, new DateTime(1972, 10, 1)),
            ("Mike J", 15121, new DateTime(1989, 2, 16)),
            ("John Lame", 532312, new DateTime(1952, 5, 30)),
            ("Ted Warm Beer", 64212312, new DateTime(2010, 7, 3)),
        };
    }

    public class TestGrid : GridWidget 
    {
        public TestGrid() 
        {
            Dock = System.Windows.Forms.DockStyle.Fill;
            BackColor = Color.Magenta;
            //Columns = new List<GridColumn>();
        }

        public override ICollection<GridColumn> Columns { get; } = new List<GridColumn>();
    }
}
