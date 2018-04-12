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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfActiveDefenceSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public PageConfig pageConfig = new PageConfig();
        public PageMissionConfig pageMissionConfig = new PageMissionConfig();
        public PageInitialStateConfig pageInitialStateConfig = new PageInitialStateConfig();
        public PageCalculationModel pageCalculationModel = new PageCalculationModel();
        public PageSystemSetting pageSystemSetting = new PageSystemSetting();
        public PageDisplay pageDisplay = new PageDisplay();
        public PageControlInput pageControlInput = new PageControlInput();
        public PhotoList Photos_OurAircraft;
        public PhotoList Photos_EnemyAircraft;
        DispatcherTimer updateTimer;
        static bool bTimerStarted = false;
        static bool bRuned = false;

        public MainWindow()
        {
            InitializeComponent();
            Initialization();
            Photos_OurAircraft = (PhotoList)(Application.Current.Resources["Photos_OurAircraft"] as ObjectDataProvider)?.Data;
            Photos_OurAircraft.Path = "..\\..\\Resources\\Photos_OurAircraft";
            Photos_EnemyAircraft = (PhotoList)(Application.Current.Resources["Photos_EnemyAircraft"] as ObjectDataProvider)?.Data;
            Photos_EnemyAircraft.Path = "..\\..\\Resources\\Photos_EnemyAircraft";
        }

        private void Initialization()
        {
            pageConfig.SetPage(this);
            pageInitialStateConfig.SetPage(this);
            pageCalculationModel.SetPage(this);
            pageSystemSetting.SetPage(this);
            pageDisplay.SetPage(this);
            pageControlInput.SetPage(this);
        }

        private void ClearAllFrames()
        {
            frameConfig.Content = null;
            frameMissionConfig.Content = null;
            tbuttonSystem.IsChecked = false;
            tbuttonDisplay.IsChecked = false;
            tbuttonControl.IsChecked = false;
            tbuttonConfig.IsChecked = false;
            pageConfig.tbuttonCalculationConfig.IsChecked = false;
            pageConfig.tbuttonInitialStateConfig.IsChecked = false;
            pageConfig.tbuttonMissionConfig.IsChecked = false;
        }

        private void tbuttonConfig_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonConfig.IsChecked == true)
            {
                ClearAllFrames();
                tbuttonConfig.IsChecked = true;
                frameConfig.Content = pageConfig;
            }
            else if (tbuttonConfig.IsChecked == false)
            {
                ClearAllFrames();
            }
        }

        private void tbuttonDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonDisplay.IsChecked == true)
            {
                ClearAllFrames();
                tbuttonDisplay.IsChecked = true;
                frameMissionConfig.Content = pageDisplay;
                pageDisplay.RefreshPage();
            }
            else if (tbuttonDisplay.IsChecked == false)
            {
                ClearAllFrames();
            }
        }

        private void tbuttonControl_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonControl.IsChecked == true)
            {
                ClearAllFrames();
                tbuttonControl.IsChecked = true;
                frameMissionConfig.Content = pageControlInput; 
            }
            else if (tbuttonControl.IsChecked == false)
            {
                ClearAllFrames();
            }
        }

        private void tbuttonSystem_Click(object sender, RoutedEventArgs e)
        {
            if (tbuttonSystem.IsChecked == true)
            {
                ClearAllFrames();
                tbuttonSystem.IsChecked = true;
                frameMissionConfig.Content = pageSystemSetting;
            }
            else if (tbuttonSystem.IsChecked == false)
            {
                ClearAllFrames();
            }
        }

        private void buttonStart_Checked(object sender, RoutedEventArgs e)
        {
            //Initialize
            Command.strSharedParameter.iRTSS = 0;

            if(!bRuned)
            {
                Command.OpenShareMemory();
                bRuned = Command.RunRTSSProcess();
                Command.WriteShareMemory();
            }

            //Timer
            if (!bTimerStarted)
            {
                updateTimer = new DispatcherTimer();
                updateTimer.Interval = TimeSpan.FromMilliseconds(100);
                updateTimer.Tick += new EventHandler(updateTimer_Tick);
                updateTimer.Start();
                bTimerStarted = true;
            }

        }

        private void tbuttonStart_Unchecked(object sender, RoutedEventArgs e)
        {
            Command.strSharedParameter.iRTSS = 1;

        }


        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            tbuttonStart.IsChecked = false;
            Command.strSharedParameter.iRTSS = 2;
            bRuned = false;

        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            //pageDisplay.PlotAttackAngle();
            Command.strSharedParameter.strStick.Roll = pageControlInput.ProgressBar_Roll.Value;
            Command.strSharedParameter.strStick.Pitch = pageControlInput.ProgressBar_Pitch.Value;
            Command.strSharedParameter.strStick.Yaw = pageControlInput.ProgressBar_Yaw.Value;
            Command.strSharedParameter.strStick.Throttle = pageControlInput.ProgressBar_Throttle.Value;
            if ( pageControlInput.tbuttonFire.IsChecked == true)
            {
                Command.strSharedParameter.strStick.bFire = 1.0;
            }
            else
            {
                Command.strSharedParameter.strStick.bFire = 0.0;
            }

            Command.strSharedParameter.strStick2.Roll = pageControlInput.ProgressBar_Roll2.Value;
            Command.strSharedParameter.strStick2.Pitch = pageControlInput.ProgressBar_Pitch2.Value;
            Command.strSharedParameter.strStick2.Yaw = pageControlInput.ProgressBar_Yaw2.Value;
            Command.strSharedParameter.strStick2.Throttle = pageControlInput.ProgressBar_Throttle2.Value;
            if (pageControlInput.tbuttonFire2.IsChecked == true)
            {
                Command.strSharedParameter.strStick2.bFire = 1.0;
            }
            else
            {
                Command.strSharedParameter.strStick2.bFire = 0.0;
            }


            Command.WriteShareMemory();
            Command.ReadShareMemory();

            this.textBox.Text = Command.strSharedParameter.test.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Command.strSharedParameter.iRTSS = 2;
            Command.WriteShareMemory(); //关RTSS进程

        }


    }
}
