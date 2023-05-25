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

namespace Snake
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public MediaPlayer _mpBgr;
        public bool isPlaying = true;
        public Window2()
        {
            InitializeComponent();
            _mpBgr = new MediaPlayer();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        public void PlayBG()
        {
            _mpBgr.Open(new Uri(@"res/SugarHaze.mp3", UriKind.Relative));
            _mpBgr.Play();
            _mpBgr.MediaEnded += new EventHandler(BGEnded);
        }
        public void BGEnded(object sender, EventArgs e)
        {
            _mpBgr.Position = TimeSpan.Zero;
            _mpBgr.Play();
        }

        public void StopBG()
        {
            _mpBgr.Stop();
        }
    }
}
