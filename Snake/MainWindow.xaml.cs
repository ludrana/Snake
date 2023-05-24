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

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer _mpBgr;
        private bool isPlaying = true;
        public MainWindow()
        {
            InitializeComponent();
            _mpBgr = new MediaPlayer();
            PlayBG();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO : better transition
            Window1 mainWindow = new Window1();
            //ShowInTaskbar = false;
            mainWindow.Top = Top;
            mainWindow.Left = Left;
            mainWindow.Show();
            Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void PlayBG()
        {
            _mpBgr.Open(new Uri(@"res/SugarHaze.mp3", UriKind.Relative));
            _mpBgr.Play();
            _mpBgr.MediaEnded += new EventHandler(BGEnded);
        }
        private void BGEnded(object sender, EventArgs e)
        {
            _mpBgr.Position = TimeSpan.Zero;
            _mpBgr.Play();
        }

        private void StopBG()
        {
            _mpBgr.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                StopBG();
                isPlaying = false;
            }
            else
            {
                PlayBG();
                isPlaying= true;
            }
        }

        private void Button_Click_3()
        {

        }
    }
}
