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
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;

namespace WpfActiveDefenceSystem
{
    /// <summary>
    /// PageDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class PageDisplay : Page
    {
        private MainWindow mainWD;
        public PlotViewModel.PlotPointCollection plotPointCollection;
        private int i = 0;

        public PageDisplay()
        {
            InitializeComponent();

            plotPointCollection = new PlotViewModel.PlotPointCollection();

            var ds = new EnumerableDataSource<PlotViewModel.PlotPoint>(plotPointCollection);
            ds.SetXMapping(x => (x.Time));
            ds.SetYMapping(y => y.Value);
            plotterAttackAngle.AddLineGraph(ds, Colors.Green, 2, "Value");

        }

        public void SetPage(MainWindow mainWindow)
        {
            this.mainWD = mainWindow;
        }

        public void RefreshPage()
        {
            RadioButtonSimJet1.Content = mainWD.pageMissionConfig.textBox_OurAircraft.Text;
            RadioButtonSimJet2.Content = mainWD.pageMissionConfig.textBox_EnemyAircraft.Text;
            if (RadioButtonSimJet1.IsChecked == true)
            {
                Photo_CurrentSimJet.Source = mainWD.pageMissionConfig.CurrentPhoto_OurAircraft.Source;
            }
            else if (RadioButtonSimJet2.IsChecked == true)
            {
                Photo_CurrentSimJet.Source = mainWD.pageMissionConfig.CurrentPhoto_EnemyAircraft.Source;
            }
        }

        private void RadioButtonSimJet1_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWD != null)
            {
                Photo_CurrentSimJet.Source = mainWD.pageMissionConfig.CurrentPhoto_OurAircraft.Source;
            }
        }

        private void RadioButtonSimJet2_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWD != null)
            {
                Photo_CurrentSimJet.Source = mainWD.pageMissionConfig.CurrentPhoto_EnemyAircraft.Source;
            }
        }

        public void PlotAttackAngle()
        {
            i++;
            plotPointCollection.Add(new PlotViewModel.PlotPoint(Math.Sin(i*0.1),i));
        }


    }
}
