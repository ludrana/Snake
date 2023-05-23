using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Serialization;

namespace Snake
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new();
        const int SnakeSquareSize = 20;
        const int SnakeStartLength = 4;
        const int SnakeStartSpeed = 400;
        const int SnakeSpeedThreshold = 100;
        const int MaxHighscoreListEntryCount = 5;
        private int tickNumber = 0;
        private Point foodPosition = new();
        private int currentScore = 0;
        private bool gameStart = false;
        private Random rnd = new Random();
        private Random rand = new Random();

        private static Color body = (Color)ColorConverter.ConvertFromString("#00FF23");
        private Brush snakeBodyBrush = new SolidColorBrush(body);
        private BitmapImage snekHead = new BitmapImage(new Uri("pack://application:,,,/res/snek_head.png"));
        private BitmapImage snekHead1 = new BitmapImage(new Uri("pack://application:,,,/res/snek_red.png"));
        private BitmapImage snekHead2 = new BitmapImage(new Uri("pack://application:,,,/res/snek_ded.png"));
        private BitmapImage snekTail = new BitmapImage(new Uri("pack://application:,,,/res/tail.png"));
     
        private List<SnakePart> snakeParts = new List<SnakePart>();
        public enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection snakeDirection = SnakeDirection.Right;
        private int snakeLength;

        public Window1()
        {
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            LoadHighscoreList();
            UpdateGameStatus();
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Top = Top;
            main.Left = Left;
            main.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
            ShowInTaskbar = false;
            Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        private void DrawGameArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;
            Color accent1 = (Color)ColorConverter.ConvertFromString("#eaae00");
            Color accent2 = (Color)ColorConverter.ConvertFromString("#fecc3a");
            Brush brush1 = new SolidColorBrush(accent1);
            Brush brush2 = new SolidColorBrush(accent2);

            while (doneDrawingBackground == false)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    Fill = nextIsOdd ? brush1 : brush2
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += SnakeSquareSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SnakeSquareSize;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
            }
        }
        private SnakeDirection GetTailDirection(SnakePart prev, SnakePart tail)
        {
            if (prev.Position.X < tail.Position.X)
            {
                return SnakeDirection.Right;
            }
            if (prev.Position.X > tail.Position.X)
            {
                return SnakeDirection.Left;
            }
            if (prev.Position.Y < tail.Position.Y)
            {
                return SnakeDirection.Down;
            }
            if (prev.Position.Y > tail.Position.Y)
            {
                return SnakeDirection.Up;
            }
            return SnakeDirection.Up;
        }
        private BitmapImage Rotate(BitmapImage image, SnakeDirection direction)
        {
            var biRotated = new BitmapImage();
            biRotated.BeginInit();
            biRotated.UriSource = image.UriSource;
            switch (direction)
            {
                case SnakeDirection.Left:
                    biRotated.Rotation = Rotation.Rotate270;
                    break;
                case SnakeDirection.Right:
                    biRotated.Rotation = Rotation.Rotate90;
                    break;
                case SnakeDirection.Down:
                    biRotated.Rotation = Rotation.Rotate180;
                    break;
            }
            biRotated.EndInit();

            return biRotated;
        }

        private void DrawSnake()
        {
            while (snakeParts.Count >= snakeLength)
            {
                GameArea.Children.Remove(snakeParts[0].UiElement);
                snakeParts.RemoveAt(0);
                snakeParts[0].IsTail = true;
            }
            var snekHeadRot = Rotate(snekHead, snakeDirection);
            if (tickNumber % 7 == 0 | (tickNumber + 1) % 7 == 0)
            {
                snekHeadRot = Rotate(snekHead1, snakeDirection);
            }
            ImageBrush head = new ImageBrush(snekHeadRot);
            ImageBrush tail = new ImageBrush();
            if (snakeParts.Count >= SnakeStartLength - 1)
            {
                var snekTailRot = Rotate(snekTail, GetTailDirection(snakeParts[1], snakeParts[0]));
                tail.ImageSource = snekTailRot;
            }
            foreach (SnakePart snakePart in snakeParts)
            {
                if (snakePart.UiElement != null && !snakePart.IsHead && !snakePart.IsTail)
                {
                    (snakePart.UiElement as Rectangle).Fill = snakeBodyBrush;
                }
                else if (snakePart.UiElement != null && snakePart.IsHead)
                {
                    (snakePart.UiElement as Rectangle).Fill = head;
                }
                else if (snakePart.UiElement != null && snakePart.IsTail)
                {
                    (snakePart.UiElement as Rectangle).Fill = tail;
                }
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = SnakeSquareSize,
                        Height = SnakeSquareSize,
                        
                        Fill = (snakePart.IsHead ? head : snakeBodyBrush)
                    };
                    GameArea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
                }
            }
        }

        private void MoveSnake()
        {
            // Remove the last part of the snake, in preparation of the new part added below  
            
            // Next up, we'll add a new element to the snake, which will be the (new) head  
            // Therefore, we mark all existing parts as non-head (body) elements and then  
            // we make sure that they use the body brush  
            foreach (SnakePart snakePart in snakeParts)
            {
                snakePart.IsHead = false;
            }

            // Determine in which direction to expand the snake, based on the current direction  
            SnakePart snakeHead = snakeParts[snakeParts.Count - 1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= SnakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += SnakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= SnakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += SnakeSquareSize;
                    break;
            }

            // Now add the new head part to our list of snake parts...  
            snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY),
                IsHead = true
            });
            //... and then have it drawn!  
            DoCollisionCheck();
            if (gameStart)
            {
                tickNumber++;
                DrawSnake();
            }
            
            //DoCollisionCheck();          
        }

        private void StartNewGame()
        {
            bdrHighscoreList.Visibility = Visibility.Collapsed;
            bdrEndOfGame.Visibility = Visibility.Collapsed;
            // Remove potential dead snake parts and leftover food...
            foreach (SnakePart snakeBodyPart in snakeParts)
            {
                if (snakeBodyPart.UiElement != null)
                    GameArea.Children.Remove(snakeBodyPart.UiElement);
            }
            snakeParts.Clear();
            var images = GameArea.Children.OfType<Image>().ToList();
            foreach (var image in images)
            {
                GameArea.Children.Remove(image);
            }

            // Reset stuff
            currentScore = 0;
            snakeLength = SnakeStartLength;
            snakeParts.Add(new SnakePart() { Position = new Point(SnakeSquareSize * 5, SnakeSquareSize * 5) });
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpeed);
            UpdateGameStatus();
            Tutorial.Visibility = Visibility.Hidden;

            // Draw the snake  
            DrawSnake();
            DrawSnakeFood();

            // Go!          
            gameTickTimer.IsEnabled = true;
        }

        private Point GetNextFoodPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / SnakeSquareSize);
            int maxY = (int)(GameArea.ActualHeight / SnakeSquareSize);
            int foodX = rnd.Next(0, maxX) * SnakeSquareSize;
            int foodY = rnd.Next(0, maxY) * SnakeSquareSize;

            foreach (SnakePart snakePart in snakeParts)
            {
                if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
                    return GetNextFoodPosition();
            }

            return new Point(foodX, foodY);
        }

        private void DrawSnakeFood()
        {
            foodPosition = GetNextFoodPosition();
            Image snakeFood = new Image
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize
            };
            if (rand.Next() % 3 == 0)
            {
                snakeFood.Source = new BitmapImage(new Uri("res/pear.png", UriKind.Relative));
            }
            else
            {
                snakeFood.Source = new BitmapImage(new Uri("res/apple.png", UriKind.Relative));
            }
            GameArea.Children.Add(snakeFood);
            Canvas.SetTop(snakeFood, foodPosition.Y);
            Canvas.SetLeft(snakeFood, foodPosition.X);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            SnakeDirection originalSnakeDirection = snakeDirection;
            if (bdrHighscoreList.IsVisible | bdrNewHighscore.IsVisible | bdrEndOfGame.IsVisible)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    if (gameStart)
                    {
                        if (snakeDirection != SnakeDirection.Down)
                            snakeDirection = SnakeDirection.Up;
                    }
                    else
                    {
                        snakeDirection = SnakeDirection.Up;
                        gameStart = true;
                        StartNewGame();
                    }
                    break;
                case Key.Down:
                case Key.S:
                    if (gameStart)
                    {
                        if (snakeDirection != SnakeDirection.Up)
                            snakeDirection = SnakeDirection.Down;
                    }
                    else
                    {
                        snakeDirection = SnakeDirection.Down;
                        gameStart = true;
                        StartNewGame();
                    }
                    break;
                case Key.Left:
                case Key.A:
                    if (gameStart)
                    {
                        if (snakeDirection != SnakeDirection.Right)
                            snakeDirection = SnakeDirection.Left;
                    }
                    else
                    {
                        snakeDirection = SnakeDirection.Left;
                        gameStart = true;
                        StartNewGame();
                    }
                    break;
                case Key.Right:
                case Key.D:
                    if (gameStart)
                    {
                        if (snakeDirection != SnakeDirection.Left)
                            snakeDirection = SnakeDirection.Right;
                    }
                    else
                    {
                        snakeDirection = SnakeDirection.Right;
                        gameStart = true;
                        StartNewGame();
                    }
                    break;
                case Key.Escape:
                    MainWindow main = new MainWindow();
                    main.Top = Top;
                    main.Left = Left;
                    main.Visibility = Visibility.Visible;
                    Visibility = Visibility.Hidden;
                    ShowInTaskbar = false;
                    Close();
                    break;
            }
            if (snakeDirection != originalSnakeDirection)
                MoveSnake();
        }
        private void DoCollisionCheck()
        {
            SnakePart snakeHead = snakeParts[snakeParts.Count - 1];

            if ((snakeHead.Position.X == foodPosition.X) && (snakeHead.Position.Y == foodPosition.Y))
            {
                EatSnakeFood();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= GameArea.ActualHeight) ||
            (snakeHead.Position.X < 0) || (snakeHead.Position.X >= GameArea.ActualWidth))
            {
                EndGame();
            }

            foreach (SnakePart snakeBodyPart in snakeParts.Take(snakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }
        private void EatSnakeFood()
        {
            snakeLength++;
            currentScore++;
            int timerInterval = Math.Max(SnakeSpeedThreshold, (int)gameTickTimer.Interval.TotalMilliseconds - (currentScore * 2));
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            var images = GameArea.Children.OfType<Image>().ToList();
            foreach (var image in images)
            {
                GameArea.Children.Remove(image);
            }
            DrawSnakeFood();
            UpdateGameStatus();
        }
        private void UpdateGameStatus()
        {
            statusScore.Text = currentScore.ToString();
            if (File.Exists("snake_highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SnakeHighscore>));
                using (Stream reader = new FileStream("snake_highscorelist.xml", FileMode.Open))
                {
                    List<SnakeHighscore> tempList = (List<SnakeHighscore>)serializer.Deserialize(reader);
                    foreach (var item in tempList.OrderByDescending(x => x.Score))
                        statusSpeed.Text = Math.Max(item.Score, int.Parse(statusSpeed.Text)).ToString();
                }
            }
        }
        private void EndGame()
        {
            var snekHeadRot = Rotate(snekHead2, snakeDirection);
            ImageBrush head = new ImageBrush(snekHeadRot);
            (snakeParts[snakeParts.Count - 2].UiElement as Rectangle).Fill = head;
            Tutorial.Visibility = Visibility.Visible;
            bool isNewHighscore = false;
            if (currentScore > 0)
            {
                int lowestHighscore = (this.HighscoreList.Count > 0 ? this.HighscoreList.Min(x => x.Score) : 0);
                if ((currentScore > lowestHighscore) || (this.HighscoreList.Count < MaxHighscoreListEntryCount))
                {
                    bdrNewHighscore.Visibility = Visibility.Visible;
                    txtPlayerName.Focus();
                    isNewHighscore = true;
                }
            }
            if (!isNewHighscore)
            {
                tbFinalScore.Text = currentScore.ToString();
                bdrEndOfGame.Visibility = Visibility.Visible;
            }
            gameTickTimer.IsEnabled = false;
            gameStart = false;
        }
        private void BtnShowHighscoreList_Click(object sender, RoutedEventArgs e) // this goes to main menu as well
        {
            bdrHighscoreList.Visibility = Visibility.Visible;
        }
        public ObservableCollection<SnakeHighscore> HighscoreList
        {
            get; set;
        } = new ObservableCollection<SnakeHighscore>();
        private void LoadHighscoreList()
        {
            if (File.Exists("snake_highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SnakeHighscore>));
                using (Stream reader = new FileStream("snake_highscorelist.xml", FileMode.Open))
                {
                    List<SnakeHighscore> tempList = (List<SnakeHighscore>)serializer.Deserialize(reader);
                    this.HighscoreList.Clear();
                    foreach (var item in tempList.OrderByDescending(x => x.Score))
                        this.HighscoreList.Add(item);
                }
            }
        }
        private void SaveHighscoreList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<SnakeHighscore>));
            using (Stream writer = new FileStream("snake_highscorelist.xml", FileMode.Create))
            {
                serializer.Serialize(writer, this.HighscoreList);
            }
        }
        private void BtnAddToHighscoreList_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;
            // Where should the new entry be inserted?
            if ((this.HighscoreList.Count > 0) && (currentScore < this.HighscoreList.Max(x => x.Score)))
            {
                SnakeHighscore justAbove = this.HighscoreList.OrderByDescending(x => x.Score).First(x => x.Score >= currentScore);
                if (justAbove != null)
                    newIndex = this.HighscoreList.IndexOf(justAbove) + 1;
            }
            // Create & insert the new entry
            this.HighscoreList.Insert(newIndex, new SnakeHighscore()
            {
                PlayerName = txtPlayerName.Text,
                Score = currentScore
            });
            // Make sure that the amount of entries does not exceed the maximum
            while (this.HighscoreList.Count > MaxHighscoreListEntryCount)
                this.HighscoreList.RemoveAt(MaxHighscoreListEntryCount);

            SaveHighscoreList();

            bdrNewHighscore.Visibility = Visibility.Collapsed;
            bdrHighscoreList.Visibility = Visibility.Visible;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseHighscoreList(object sender, RoutedEventArgs e)
        {
            bdrHighscoreList.Visibility = Visibility.Collapsed;
        }
        private void CloseEndScreen(object sender, RoutedEventArgs e)
        {
            bdrEndOfGame.Visibility = Visibility.Collapsed;
        }
    }
}
