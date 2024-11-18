using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cosmetify.RenderView
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();

            double[] values = { 26, 20, 23, 7, 16 };

            // add a bar graph to the plot
            var bar = barPlot.Plot.Add.Bars(values);
            // adjust axis limits so there is no padding below the bar graph
            barPlot.Plot.Axes.SetLimitsY(0, 0);

            double[] pvalues = { 778, 43, 283, 76, 184 };
            var pie = piePlot.Plot.Add.Pie(pvalues);
            pie.ShowSliceLabels = true;

            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            dotPlot.Plot.Add.Scatter(dataX, dataY);
            dotPlot.Refresh();

            double[] p1values = { 283, 20, 283, 100, 778 };
            var pie1 = barHPlot.Plot.Add.Pie(p1values);
            pie1.ShowSliceLabels = true;
            pie1.ExplodeFraction = 0.1;

        }
    }
}
