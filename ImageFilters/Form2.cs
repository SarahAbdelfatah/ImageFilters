using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace ImageFilters
{
    public partial class Form2 : Form
    {
        ImageFilters.ZedGraph medianManager;
        ImageFilters.ZedGraph midPointManager;

        public Form2()
        {
            InitializeComponent();
        }
        public void DrawAllGraphs(double[] window_sizes, double[] q_y, double[] k_y, double[] c_y, double[] h_y, double[] MidpointEfficient_y, double[] NonEfficientMidPoint_y)
        public void DrawAllGraphs(double[] window_sizes, double[] q_y, double[] k_y, double[] MidpointEfficient_y, double[] NonEfficientMidPoint_y)
        {
         
            medianManager = new ImageFilters.ZedGraph(zedGraphControl1, "Median Filter Comparison", "Window Size", "Time (ms)");
            medianManager.AddNewCurve("Quick Sort", window_sizes, q_y);
            medianManager.AddNewCurve("Counting Sort", window_sizes, c_y);
            medianManager.AddNewCurve("Heuristic Method", window_sizes, h_y);
            //medianManager.AddNewCurve("Counting Sort", window_sizes, c_y);
            medianManager.AddNewCurve("K-th Smallest", window_sizes, k_y);

          
            midPointManager = new ImageFilters.ZedGraph(zedGraphControl2, "Mid-Point Filter Performance", "Window Size", "Time (ms)");
            midPointManager.AddNewCurve("Mid-Point-Efficient Algorithm", window_sizes, MidpointEfficient_y);
            midPointManager.AddNewCurve("Mid-Point-Non-Efficient Algorithm", window_sizes, NonEfficientMidPoint_y);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();

            zedGraphControl2.AxisChange();
            zedGraphControl2.Refresh();
        }


    }
}
