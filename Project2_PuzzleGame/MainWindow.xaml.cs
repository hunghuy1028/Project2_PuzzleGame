using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Project2_PuzzleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        const int _size = 3; //adjust your number of row and col here

        Image[,] image_cropped = new Image[_size, _size];
        int[,] _imageCheck = new int[_size, _size];
        bool _isDragging = false;
        bool _canPlay = false;
        Image _selectedBitmap = null;
        Point _lastPosition;
        Point _lastPosition2;
        const int startX = 30;
        const int startY = 30;
        const int width = 100;
        const int height = 100;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;
        private void chooseImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {

                var source = new BitmapImage(
                    new Uri(screen.FileName, UriKind.Absolute));
                Debug.WriteLine($"{source.Width} - {source.Height}");

                previewImage.Width = 350;
                previewImage.Height = 280;
                previewImage.Source = source;

                Canvas.SetLeft(previewImage, 400);
                Canvas.SetTop(previewImage, 0);

                // Bat dau cat thanh 9 manh

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!((i == 2) && (j == 2)))
                        {
                            var h = (int)source.Height / 3;
                            var w = (int)source.Height / 3;
                            //Debug.WriteLine($"Len = {len}");
                            var rect = new Int32Rect(i * w, j * h, w, h);
                            var cropBitmap = new CroppedBitmap(source,
                                rect);

                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = width;
                            cropImage.Height = height;
                            cropImage.Source = cropBitmap;
                            image_cropped[i, j] = cropImage;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + i * (width + 2));
                            Canvas.SetTop(cropImage, startY + j * (height + 2));

                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_PreviewMouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                            //cropImage.MouseLeftButtonUp
                            _imageCheck[i, j] = _size * i + j;
                        }
                    }
                }
                //the empty image is marked -1 value
                _imageCheck[2, 2] = -1;
            }
            
        }

        private void CropImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            var position = e.GetPosition(this);
            var image = sender as Image;
            var (i, j) = image.Tag as Tuple<int, int>; //i,j: position of selected bitmap on 2 dimensions array
            int x = (int)(position.X - startX) / (width + 2) * (width + 2) + startX;
            int y = (int)(position.Y - startY) / (height + 2) * (height + 2) + startY;
            int pos_x, pos_y, pos_x1, pos_y1;

            pos_x = (x - startX) / (width + 2);
            pos_y = (y - startY) / (height + 2);

            pos_x1 = (int)(_lastPosition2.X - startX) / (width + 2);
            pos_y1 = (int)(_lastPosition2.Y - startY) / (height + 2);

            bool isValidDrag;
            if (pos_x < _size && pos_y < _size)
            {
                isValidDrag = true;
                if (_imageCheck[pos_x, pos_y] == -1)
                {
                    isValidDrag = true;
                    if ((Math.Abs(pos_x - pos_x1) < 1 && Math.Abs(pos_y - pos_y1) < 2) || (Math.Abs(pos_x - pos_x1) < 2 && Math.Abs(pos_y - pos_y1) < 1))
                        isValidDrag = true;
                    else
                    {
                        isValidDrag = false;
                    }
                } 
                else
                {
                    isValidDrag = false;
                }
            } 
            else
            {
                isValidDrag = false;
            }

            if (!isValidDrag)
            {
                x = (int)(_lastPosition2.X - startX) / (width + 2) * (width + 2) + startX;
                y = (int)(_lastPosition2.Y - startY) / (height + 2) * (height + 2) + startY;
            }
            else
            {
                Swap(ref _imageCheck[pos_x, pos_y], ref _imageCheck[pos_x1, pos_y1]);
                Swap<Image>(ref image_cropped[pos_x, pos_y], ref image_cropped[pos_x1, pos_y1]);
            }

            Canvas.SetLeft(_selectedBitmap, x);
            Canvas.SetTop(_selectedBitmap, y);

            if (isValidDrag && checkWin(_imageCheck, _size))
            {
                StopTime();
                MessageBox.Show("You Win!");
            }

        }

        static void Swap<T>(ref T x, ref T y)
        {
            T t = y;
            y = x;
            x = t;
        }

        private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_canPlay)
            {
                _isDragging = true;
                _selectedBitmap = sender as Image;
                _lastPosition = e.GetPosition(this);
                _lastPosition2 = e.GetPosition(this);
                Start_Click(sender, e);
            }
            else
            {
                MessageBox.Show("You can only play after shuffle", "Play");
            }
            
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / height;
            int j = ((int)position.X - startX) / width;

            this.Title = $"{position.X} - {position.Y}, a[{i}][{j}]";

            if (_isDragging)
            {
                var dx = position.X - _lastPosition.X;
                var dy = position.Y - _lastPosition.Y;

                var lastLeft = Canvas.GetLeft(_selectedBitmap);
                var lastTop = Canvas.GetTop(_selectedBitmap);
                Canvas.SetLeft(_selectedBitmap, lastLeft + dx);
                Canvas.SetTop(_selectedBitmap, lastTop + dy);

                _lastPosition = position;
            }
        }

        private void Information_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("8 Puzzle Game\n1712472 - Lo Huy Hung\n1712555 - Chau Vinh Lap", "Information");

        }

        private void MoveUp()
        {
            var empty_pos = emptyPos(_imageCheck, _size);
            int i = empty_pos.Item1;
            int j = empty_pos.Item2;

            if (j + 1 < _size)
            {
                _selectedBitmap = image_cropped[i, j + 1];
                int x = i * (width + 2) + startX;
                int y = j * (height + 2) + startY;
                //_imageCheck
                Swap<int>(ref _imageCheck[i, j], ref _imageCheck[i, j + 1]);
                Swap<Image>(ref image_cropped[i, j], ref image_cropped[i, j + 1]);

                Canvas.SetLeft(_selectedBitmap, x);
                Canvas.SetTop(_selectedBitmap, y);
            }
        }

        private void MoveDown()
        {
            var empty_pos = emptyPos(_imageCheck, _size);
            int i = empty_pos.Item1;
            int j = empty_pos.Item2;

            if (j - 1 >= 0)
            {
                _selectedBitmap = image_cropped[i, j - 1];
                int x = i * (width + 2) + startX;
                int y = j * (height + 2) + startY;
                //_imageCheck
                Swap<int>(ref _imageCheck[i, j], ref _imageCheck[i, j - 1]);
                Swap<Image>(ref image_cropped[i, j], ref image_cropped[i, j - 1]);
                Canvas.SetLeft(_selectedBitmap, x);
                Canvas.SetTop(_selectedBitmap, y);
            }
        }

        private void MoveLeft()
        {
            var empty_pos = emptyPos(_imageCheck, _size);
            int i = empty_pos.Item1;
            int j = empty_pos.Item2;
            if (i + 1 < _size)
            {
                _selectedBitmap = image_cropped[i + 1, j];
                int x = i * (width + 2) + startX;
                int y = j * (height + 2) + startY;
                //_imageCheck
                Swap<int>(ref _imageCheck[i, j], ref _imageCheck[i + 1, j]);
                Swap<Image>(ref image_cropped[i, j], ref image_cropped[i + 1, j]);
                Canvas.SetLeft(_selectedBitmap, x);
                Canvas.SetTop(_selectedBitmap, y);
            }
        }

        private void MoveRight()
        {
            var empty_pos = emptyPos(_imageCheck, _size);
            int i = empty_pos.Item1;
            int j = empty_pos.Item2;

            if (i-1 >= 0)
            {
                _selectedBitmap = image_cropped[i - 1, j];
                int x = i * (width + 2) + startX;
                int y = j * (height + 2) + startY;
                //_imageCheck
                Swap<int>(ref _imageCheck[i, j], ref _imageCheck[i - 1, j]);
                Swap<Image>(ref image_cropped[i, j], ref image_cropped[i - 1, j]);
                Canvas.SetLeft(_selectedBitmap, x);
                Canvas.SetTop(_selectedBitmap, y);
            }
        }

        private static Tuple<int, int> emptyPos(int[,] a, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (a[i, j] == -1)
                        return Tuple.Create(i, j);
                }
                
            }
            return null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(_canPlay)
            {
                if (e.Key == Key.Up)
                {
                    MoveUp();
                }
                if (e.Key == Key.Down)
                {
                    MoveDown();
                }
                if (e.Key == Key.Left)
                {
                    MoveLeft();
                }
                if (e.Key == Key.Right)
                {
                    MoveRight();
                }
                if (checkWin(_imageCheck, _size))
                {
                    StopTime();
                    MessageBox.Show("You Win!");
                }
            }
        }

        

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        { 
            var random = new Random();
            bool isShuffle = false;
            do
            {
                for (int i = 0; i < 128; i++)
                {
                    int x = random.Next(2000);
                    x %= 4;
                    if (x == 0)
                    {
                        MoveUp();
                    }
                    if (x == 1)
                    {
                        MoveLeft();
                    }
                    if (x == 2)
                    {
                        MoveRight();
                    }
                    if (x == 3)
                    {
                        MoveDown();
                    }
                }
                int count = 0;
                for (int j = 0; j < _size; j++)
                {
                    if (_imageCheck[0, j] == j)
                        count++;
                }
                if (count == _size) isShuffle = true;
                else isShuffle = false;
            } while (isShuffle);
            _canPlay = true;
        }

        private static bool checkWin(int[,]a,int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size - 1 && j == size - 1)
                        return true;
                    if (a[i, j] != _size * i + j)
                        return false;
                }
            }
            return true;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Timing.Content = currentTime;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            stopWatch.Start();
            dispatcherTimer.Start();
        }

        private void StopTime()
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }           
        }

    }
}