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
using LiveCharts;
using LiveCharts.Wpf;

namespace AnalyzerDatabase.View
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();

            PointLabel = chartPoint =>
                $"{chartPoint.Y} ({chartPoint.Participation:P})";


            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Duplikaty",
                    Values = new ChartValues<double> { 150 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new RowSeries
            {
                Title = "Publikacje",
                Values = new ChartValues<double> { 46 }
            });

            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            //Labels = new[] { "Duplikaty", "Publikacje"};
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        public Func<ChartPoint, string> PointLabel { get; set; }
    }
}
