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

namespace WpfActiveDefenceSystem
{
    /// <summary>
    /// PageConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PageConfig : Page
    {
        private MainWindow mainWD;

        public PageConfig()
        {
            InitializeComponent();
        }

        public void SetPage(MainWindow mainWindow)
        {
            this.mainWD = mainWindow;
        }

        private void tbuttonMissionConfig_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonMissionConfig.IsChecked == true)
            {
                tbuttonInitialStateConfig.IsChecked = false;
                tbuttonCalculationConfig.IsChecked = false;
                mainWD.frameMissionConfig.Content = mainWD.pageMissionConfig;
            }
            else if (tbuttonMissionConfig.IsChecked == false)
            {
                mainWD.frameMissionConfig.Content = null;
            }
        }

        private void tbuttonInitialStateConfig_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonInitialStateConfig.IsChecked == true)
            {
                tbuttonMissionConfig.IsChecked = false;
                tbuttonCalculationConfig.IsChecked = false;

                mainWD.frameMissionConfig.Content = mainWD.pageInitialStateConfig;
                mainWD.pageInitialStateConfig.RefreshPage();
            }

            else if (tbuttonInitialStateConfig.IsChecked == false)
            {
                mainWD.frameMissionConfig.Content = null;
            }
        }

        private void tbuttonCalculationConfig_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonCalculationConfig.IsChecked == true)
            {
                tbuttonMissionConfig.IsChecked = false;
                tbuttonInitialStateConfig.IsChecked = false;

                mainWD.frameMissionConfig.Content = mainWD.pageCalculationModel;
            }

            else if (tbuttonCalculationConfig.IsChecked == false)
            {
                mainWD.frameMissionConfig.Content = null;
            }
        }
    }
}
