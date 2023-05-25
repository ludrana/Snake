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
        public enum SnakeDirection { Left, Right, Up, Down };
        public Window2()
        {
            InitializeComponent();
            _mpBgr = new MediaPlayer();
            _mpBgr.Open(new Uri(@"res/SugarHaze.mp3", UriKind.Relative));
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (FrameContent.Content is Page2)
            {
                Page2 page = FrameContent.Content as Page2;
                SnakeDirection originalSnakeDirection = (SnakeDirection)page.snakeDirection;
                if (page.bdrHighscoreList.IsVisible | page.bdrNewHighscore.IsVisible | page.bdrEndOfGame.IsVisible)
                {
                    return;
                }
                switch (e.Key)
                {
                    case Key.Up:
                    case Key.W:
                        if (page.gameStart)
                        {
                            if ((SnakeDirection)page.snakeDirection != SnakeDirection.Down)
                                page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Up;
                        }
                        else
                        {
                            page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Up;
                            page.gameStart = true;
                            page.StartNewGame();
                        }
                        break;
                    case Key.Down:
                    case Key.S:
                        if (page.gameStart)
                        {
                            if ((SnakeDirection)page.snakeDirection != SnakeDirection.Up)
                                page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Down;
                        }
                        else
                        {
                            page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Down;
                            page.gameStart = true;
                            page.StartNewGame();
                        }
                        break;
                    case Key.Left:
                    case Key.A:
                        if (page.gameStart)
                        {
                            if ((SnakeDirection)page.snakeDirection != SnakeDirection.Right)
                                page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Left;
                        }
                        else
                        {
                            page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Left;
                            page.gameStart = true;
                            page.StartNewGame();
                        }
                        break;
                    case Key.Right:
                    case Key.D:
                        if (page.gameStart)
                        {
                            if ((SnakeDirection)page.snakeDirection != SnakeDirection.Left)
                                page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Right;
                        }
                        else
                        {
                            page.snakeDirection = (Page2.SnakeDirection)SnakeDirection.Right;
                            page.gameStart = true;
                            page.StartNewGame();
                        }
                        break;
                    case Key.Escape:
                        FrameContent.Navigate(new Page1());
                        break;
                }
                if ((SnakeDirection)page.snakeDirection != originalSnakeDirection)
                    page.MoveSnake();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            PlayBG();
        }
    }
}
