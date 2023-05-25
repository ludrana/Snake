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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            //((MainWindow)System.Windows.Application.Current.MainWindow).FunctionName(params);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.MainWindow).FrameContent.Navigate(new Page2());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (((Window2)Application.Current.MainWindow).isPlaying)
            {
                ((Window2)Application.Current.MainWindow).StopBG();
                ((Window2)Application.Current.MainWindow).isPlaying = false;
                Color clr = (Color)ColorConverter.ConvertFromString("#ffa8a8");
                SolidColorBrush bg = new SolidColorBrush(clr);
                bgm_btn.Background = bg;
            }
            else
            {
                ((Window2)Application.Current.MainWindow).PlayBG();
                ((Window2)Application.Current.MainWindow).isPlaying = true;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!((Window2)Application.Current.MainWindow).isPlaying)
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
