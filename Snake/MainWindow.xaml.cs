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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 mainWindow = new Window1();
            //ShowInTaskbar = false;
            mainWindow.Top = Top;
            mainWindow.Left = Left;
            mainWindow.Show();
            Visibility = Visibility.Collapsed;
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
                Color clr = (Color)ColorConverter.ConvertFromString("#ffa8a8");
                SolidColorBrush bg = new SolidColorBrush(clr);
                bgm_btn.Background = bg;
            }
            else
            {
                PlayBG();
                isPlaying= true;
                Color clr = (Color)ColorConverter.ConvertFromString("#b6ff9d");
                SolidColorBrush bg = new SolidColorBrush(clr);
                bgm_btn.Background = bg;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Sfx.sfxOn = !Sfx.sfxOn;
            if (Sfx.sfxOn)
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#b6ff9d");
                SolidColorBrush bg = new SolidColorBrush(clr);
                sfx_btn.Background = bg;
            }
            else
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#ffa8a8");
                SolidColorBrush bg = new SolidColorBrush(clr);
                sfx_btn.Background = bg;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _mpBgr = new MediaPlayer();
            PlayBG();
            if (!isPlaying)
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#ffa8a8");
                SolidColorBrush bg = new SolidColorBrush(clr);
                bgm_btn.Background = bg;
            }
            else
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#b6ff9d");
                SolidColorBrush bg = new SolidColorBrush(clr);
                bgm_btn.Background = bg;
            }
            if (Sfx.sfxOn)
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#b6ff9d");
                SolidColorBrush bg = new SolidColorBrush(clr);
                sfx_btn.Background = bg;
            }
            else
            {
                Color clr = (Color)ColorConverter.ConvertFromString("#ffa8a8");
                SolidColorBrush bg = new SolidColorBrush(clr);
                sfx_btn.Background = bg;
            }
        }
    }
}
