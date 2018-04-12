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
    /// PageMissionConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PageMissionConfig : Page
    {
        public PageMissionConfig()
        {
            InitializeComponent();

            textBox_OurAircraft.Text = "";
            textBox_EnemyAircraft.Text = "";
        }

        private void PhotoListSelection_OurAircraft(object sender, RoutedEventArgs e)
        {
            var path = ((sender as ListBox)?.SelectedItem.ToString());
            BitmapSource img = BitmapFrame.Create(new Uri(path));
            CurrentPhoto_OurAircraft.Source = img;
            string[] strPhotoPath = path.Split('\\','.');
            textBox_OurAircraft.Text = strPhotoPath[strPhotoPath.Length - 2];
        }

        private void PhotoListSelection_EnemyAircraft(object sender, RoutedEventArgs e)
        {
            var path = ((sender as ListBox)?.SelectedItem.ToString());
            BitmapSource img = BitmapFrame.Create(new Uri(path));
            CurrentPhoto_EnemyAircraft.Source = img;
            string[] strPhotoPath = path.Split('\\', '.');
            textBox_EnemyAircraft.Text = strPhotoPath[strPhotoPath.Length - 2];
        }


    }
}
