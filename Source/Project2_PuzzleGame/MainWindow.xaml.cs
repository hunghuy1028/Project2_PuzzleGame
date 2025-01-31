﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static int _size = 3; //adjust your number of row and col here

        //Game check
        Image[,] image_cropped;
        int[,] _imageCheck;

        //Check game rule
        bool _isDragging = false;
        bool _isShuffle = false;
        bool _canplay = true;
        int time_gameMode = 1;// game mode 1: unlimited, 2: 3 minutes
        int level_gameMode = 3;// set default game
        Image _selectedBitmap = null;
        Point _lastPosition;
        Point _lastPosition2;

        const int startX = 30;
        const int startY = 30;
        int width = 300/_size;
        int height = 300/_size;
        BitmapImage newGame_image;
        string imgPath;

        //Timing
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer countdown = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;
        int timeCountDown = 180;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            image_cropped = new Image[_size, _size];
            _imageCheck = new int[_size, _size];

            newGame_image = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Images\\a.jpg", UriKind.Absolute));
            imgPath = $"{AppDomain.CurrentDomain.BaseDirectory}Images\\a.jpg";
            previewImage.Width = 350;
            previewImage.Height = 280;
            previewImage.Source = newGame_image;

            Canvas.SetLeft(previewImage, 400);
            Canvas.SetTop(previewImage, 0);
            newGame();

            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            countdown.Tick += new EventHandler(cd_Tick);
            countdown.Interval = new TimeSpan(0, 0, 1);
        }

        private void chooseImage_Button_Click(object sender, RoutedEventArgs e)
        {    
            var screen = new OpenFileDialog();
            screen.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (screen.ShowDialog() == true)
            {
                restartbtn(_size);
                imgPath = screen.FileName;
                var source = new BitmapImage(new Uri(screen.FileName, UriKind.Absolute));
                //Debug.WriteLine($"{source.Width} - {source.Height}");
                newGame_image = source;

                previewImage.Width = 350;
                previewImage.Height = 280;
                previewImage.Source = source;

                Canvas.SetLeft(previewImage, 400);
                Canvas.SetTop(previewImage, startX);

                // Bat dau cat thanh 9 manh
                try
                {
                    for (int i = 0; i < _size; i++)
                    {
                        for (int j = 0; j < _size; j++)
                        {
                            if (!((i == _size-1) && (j == _size-1)))
                            {
                                if (!((i == _size-1) && (j == _size-1)))
                                {
                                    int h, w;
                                    if ((int)source.Height < (int)source.Width)
                                    {
                                        h = (int)source.Height / _size;
                                        w = (int)source.Height / _size;
                                    }
                                    else
                                    {
                                        h = (int)source.Width / _size;
                                        w = (int)source.Width / _size;
                                    }
                                    //MessageBox.Show($"{h}-{w} {source.Width}-{source.Height}");
                                     
                                    //Debug.WriteLine($"Len = {len}");
                                    var rect = new Int32Rect(i * w, j * h, w, h);
                                    var cropBitmap = new CroppedBitmap(source, rect);
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
                                    Mouse.Capture(cropImage);
                                    cropImage.Tag = new Tuple<int, int>(i, j);
                                    //cropImage.MouseLeftButtonUp
                                    _imageCheck[i, j] = _size * i + j;
                                }
                            }
                        }
                    }
                    _canplay = true;
                    //the empty image is marked -1 value
                    _imageCheck[_size-1, _size-1] = -1;
                }
                catch
                {
                    MessageBox.Show("Please choose other image.\nThis image cant crop", "Can't crop image");
                    _canplay = false;
                }
            }
        }

        private void CropImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedBitmap != null && _canplay!= false)
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
                if (pos_x < _size && pos_y < _size && position.X < this.ActualWidth  && position.Y < this.ActualHeight )
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
                    var playtime = Timing.Content.ToString();
                    StopTime();
                    if (time_gameMode == 1)
                    {
                        MessageBox.Show("You Win!\n" + $"Time: {playtime}", "Congratulation");
                    }
                    else if (time_gameMode == 2)
                    {
                        int temp = 180 - timeCountDown;
                        MessageBox.Show(string.Format($"You Win!\n Time 0{temp / 60}:{temp % 60} s"), "Congratulation");
                    }
                    _isShuffle = false;
                }
            }

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / height;
            int j = ((int)position.X - startX) / width;

            //this.Title = $"{position.X} - {position.Y}, a[{i}][{j}]";

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

        private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isShuffle)
            {
                _isDragging = true;
                _selectedBitmap = sender as Image;
                _lastPosition = e.GetPosition(this);
                _lastPosition2 = e.GetPosition(this);
                Start_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Shuffle make game more fun ^^", "Message");
            }

        }

        static void Swap<T>(ref T x, ref T y)
        {
            T t = y;
            y = x;
            x = t;
        }

        public void newGame()
        {
            StopTime();
            var source = newGame_image;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (!((i == _size - 1) && (j == _size - 1)))
                    {
                        int h, w;
                        if ((int)source.Height < (int)source.Width)
                        {
                            h = (int)source.Height / _size;
                            w = (int)source.Height / _size;
                        }
                        else
                        {
                            h = (int)source.Width / _size;
                            w = (int)source.Width / _size;
                        }
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
                        //canvas.CaptureMouse();
                        //Mouse.Capture(cropImage);
                        cropImage.Tag = new Tuple<int, int>(i, j);
                        //cropImage.MouseLeftButtonUp
                        _imageCheck[i, j] = _size * i + j;
                    }
                }
                //the empty image is marked -1 value
                _imageCheck[_size - 1, _size - 1] = -1;
                _isShuffle = false;
            }
            //the empty image is marked -1 value
            _imageCheck[_size - 1, _size - 1] = -1;
            _isShuffle = false;
        }

        private void GameMode_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var gameModeOptionScreen = new GameMode(time_gameMode, level_gameMode);
            if (gameModeOptionScreen.ShowDialog() == true)
            {

                time_gameMode = gameModeOptionScreen.Time_GameMode;
                level_gameMode = gameModeOptionScreen.Level_GameMode;
                restartbtn(_size);
                _size = level_gameMode;
                width = 300 / _size;
                height = 300 / _size;
                image_cropped = new Image[_size, _size];
                _imageCheck = new int[_size, _size];
                if (time_gameMode == 1)
                {
                    Timing.Content = "00:00";
                }
                else if (time_gameMode == 2)
                {
                    Timing.Content = "03:00";
                }

                newGame();
            }
            else
            {
                Debug.WriteLine("");
            }

        }


        private void Information_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("8 Puzzle Game\n1712472 - Lo Huy Hung\n1712555 - Chau Vinh Lap", "Information");

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isShuffle && _canplay)
            {
                Start_Click(sender, e);
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
                    var playtime = Timing.Content.ToString();
                    StopTime();
                    if (time_gameMode == 1)
                    {
                        MessageBox.Show("You Win!\n" + $"Time: {playtime}", "Congratulation");
                    }
                    else if (time_gameMode == 2)
                    {
                        int temp = 180 - timeCountDown;
                        MessageBox.Show(string.Format($"You Win!\n Time 0{temp / 60}:{temp % 60} s"), "Congratulation");
                    }
                    _isShuffle = false;
                }
            }
            else
            {
                MessageBox.Show("Shuffle make game more fun ^^", "Message");
            }
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_canplay)
                {
                    StopTime();
                    var random = new Random();
                    bool isShuffle = false;
                    do
                    {
                        for (int i = 0; i < 1280; i++)
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
                    _isShuffle = true;
                }
            }
            catch
            {
                MessageBox.Show("Please choose image first");
            }
        }

        private void SaveGame_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_canplay)
            {
               
                const string filename = "save.txt";
                var writer = new StreamWriter(filename);
                writer.WriteLine(imgPath);
                writer.WriteLine(_size);
                writer.WriteLine(_isShuffle);
                // Theo sau la ma tran bieu dien game
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        writer.Write($"{_imageCheck[i, j]}");
                        if (j != _size - 1)
                        {
                            writer.Write(" ");
                        }
                    }
                    writer.WriteLine("");
                }
                writer.Close();

                MessageBox.Show("Game saved");
            }
        }

        private void LoadGame_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            var filename = "save.txt";
            try { 

                var reader=new StreamReader(filename);
                restartbtn(_size);
                var firstLine = reader.ReadLine();
                var size = int.Parse(reader.ReadLine());
                _size = size;
                width = 300 / _size;
                height = 300 / _size;
                image_cropped = new Image[_size, _size];
                _imageCheck = new int[_size, _size];
                imgPath = firstLine;
                var shuffle = reader.ReadLine();
                if (shuffle == "True")
                    _isShuffle = true;
                else
                {
                    _isShuffle = false;
                }
                for (int i = 0; i < _size; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    // Model

                    for (int j = 0; j < _size; j++)
                    {
                        _imageCheck[i, j] = int.Parse(tokens[j]);
                    }
                }
                reader.Close();

                var source = new BitmapImage(
                    new Uri(firstLine, UriKind.Absolute));
                Debug.WriteLine($"{source.Width} - {source.Height}");
                newGame_image = source;

                previewImage.Width = 350;
                previewImage.Height = 280;
                previewImage.Source = source;

                Canvas.SetLeft(previewImage, 400);
                Canvas.SetTop(previewImage, 0);

                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (!((i == _size - 1) && (j == _size - 1)))
                        {
                            int h, w;
                            if ((int)source.Height < (int)source.Width)
                            {
                                h = (int)source.Height / _size;
                                w = (int)source.Height / _size;
                            }
                            else
                            {
                                h = (int)source.Width / _size;
                                w = (int)source.Width / _size;
                            }
                            //Debug.WriteLine($"Len = {len}");
                            var rect = new Int32Rect(i * w, j * h, w, h);
                            var cropBitmap = new CroppedBitmap(source,
                                rect);

                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = width;
                            cropImage.Height = height;
                            cropImage.Source = cropBitmap;
                            var img_pos = imgPos(_imageCheck, _size, _size * i + j);
                            int t1 = img_pos.Item1;
                            int t2 = img_pos.Item2;
                            image_cropped[t1, t2] = cropImage;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + t1 * (width + 2));
                            Canvas.SetTop(cropImage, startY + t2 * (height + 2));
                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_PreviewMouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                        }
                    }
                }
                _canplay = true;
            }
            catch
            {
                MessageBox.Show("You haven't saved game yet!!");
            }
            
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_canplay)
                {
                    restartbtn(_size);
                    newGame();
                }
            }
            catch
            { MessageBox.Show("Please choose image first"); }
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

            if (i - 1 >= 0)
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

        private static bool checkWin(int[,] a, int size)
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

        private void cd_Tick(object sender, EventArgs e)
        {
            if(timeCountDown >0)
            {
                timeCountDown--;
                Timing.Content = string.Format($"0{timeCountDown / 60}:{timeCountDown % 60}");
            }
            else
            {
                MessageBox.Show("Better luck next time ^^");
                countdown.Stop();
                timeCountDown = 180;
                Timing.Content = "03:00";
            }
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}",
                ts.Minutes, ts.Seconds);
                Timing.Content = currentTime;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (_canplay)
            {

                if (_isShuffle)
                {
                    if (time_gameMode == 1)
                    {
                        stopWatch.Start();
                        dispatcherTimer.Start();
                    }
                    else if (time_gameMode == 2)
                    {
                        countdown.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Shuffle make game more fun ^^", "Play");
                }
            }
        }

        private void StopTime()
        {
            if (time_gameMode == 1)
            {
                if (stopWatch.IsRunning)
                {
                    stopWatch.Stop();
                }
            }
            else if (time_gameMode == 2)
            {
                countdown.Stop();
            }
            stopWatch.Reset();
            Timing.Content = "00:00";
        }

        public void restartbtn(int row)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (image_cropped[i, j] != null)
                        image_cropped[i, j].Source = null;
                }
            }
        }

        private static Tuple<int, int> imgPos(int[,] a, int size, int pos)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (a[i, j] == pos)
                        return Tuple.Create(i, j);
                }
            }
            return null;
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

        
    }
}
