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
using Xceed.Wpf.Toolkit;

namespace WpfActiveDefenceSystem
{
    /// <summary>
    /// PageInitialStateConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PageInitialStateConfig : Page
    {
        private MainWindow mainWD;

        public PageInitialStateConfig()
        {
            InitializeComponent();
        }

        public void SetPage(MainWindow mainWindow)
        {
            this.mainWD = mainWindow;
        }

        public void RefreshPage()
        {
            RadioButton_Jet1.Content = mainWD.pageMissionConfig.textBox_OurAircraft.Text;
            RadioButton_Jet2.Content = mainWD.pageMissionConfig.textBox_EnemyAircraft.Text;
            if (RadioButton_Jet1.IsChecked == true)
            {
                Photo_CurrentJet.Source = mainWD.pageMissionConfig.CurrentPhoto_OurAircraft.Source;
                Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\DefenceMissile.jpg", UriKind.Relative));
            }
            else if (RadioButton_Jet2.IsChecked == true)
            {
                Photo_CurrentJet.Source = mainWD.pageMissionConfig.CurrentPhoto_EnemyAircraft.Source;
                if (mainWD.pageMissionConfig.ComboBoxItem_9X.IsSelected == true)
                    Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\AIM-9X.jpg", UriKind.Relative));
                else if (mainWD.pageMissionConfig.ComboBoxItem_120D.IsSelected == true)
                    Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\AIM-120D.jpg", UriKind.Relative));
            }
        }

        private void RadioButton_Jet1_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWD != null)
            {
                Photo_CurrentJet.Source = mainWD.pageMissionConfig.CurrentPhoto_OurAircraft.Source;

                Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\DefenceMissile.jpg", UriKind.Relative));
            }
        }

        private void RadioButton_Jet2_Checked(object sender, RoutedEventArgs e)
        {
            if (mainWD != null)
            {
                Photo_CurrentJet.Source = mainWD.pageMissionConfig.CurrentPhoto_EnemyAircraft.Source;

                if (mainWD.pageMissionConfig.ComboBoxItem_9X.IsSelected == true)
                    Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\AIM-9X.jpg", UriKind.Relative));
                else if (mainWD.pageMissionConfig.ComboBoxItem_120D.IsSelected == true)
                    Photo_CurrentMissile.Source = BitmapFrame.Create(new Uri("..\\..\\Resources\\Photos_Missile\\AIM-120D.jpg", UriKind.Relative));
            }
        }
    }
}
