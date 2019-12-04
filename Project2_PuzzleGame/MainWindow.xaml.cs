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
        Image _selectedBitmap = null;
        Point _lastPosition;
        Point _lastPosition2;
        const int startX = 30;
        const int startY = 30;
        const int width = 100;
        const int height = 100;
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

                Canvas.SetLeft(previewImage, 0);
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


                        }

                        //image 8 is empty (_imageCheck[2,2] = 8)
                        _imageCheck[i, j] = _size * i + j;
                    }
                }

            }
        }

        private void CropImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            var position = e.GetPosition(this);

            int x = (int)(position.X - startX) / (width + 2) * (width + 2) + startX;
            int y = (int)(position.Y - startY) / (height + 2) * (height + 2) + startY;

            bool isValidDrag = true;
            if (((int)position.X < startX) || ((int)position.X > startX + width * 3 + 4))
                isValidDrag = false;
            if (((int)position.Y < startY) || ((int)position.Y > startY + height * 3 + 4))
                isValidDrag = false;

            if (!isValidDrag)
            {
                x = (int)(_lastPosition2.X - startX) / (width + 2) * (width + 2) + startX;
                y = (int)(_lastPosition2.Y - startY) / (height + 2) * (height + 2) + startY;
            }

            Canvas.SetLeft(_selectedBitmap, x);
            Canvas.SetTop(_selectedBitmap, y);
            var image = sender as Image;
            var (i, j) = image.Tag as Tuple<int, int>;

            //MessageBox.Show($"{i} - {j}");
        }

        private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _selectedBitmap = sender as Image;
            _lastPosition = e.GetPosition(this);
            _lastPosition2 = e.GetPosition(this);
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

        private void Up_Key_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Down_Key_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Left_Key_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Right_Key_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var empty_pos = emptyPos(_imageCheck, _size);
            int i = empty_pos.Item1;
            int j = empty_pos.Item2;
            if (e.Key == Key.Up)
            {
                if (j + 1 < 3)
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
            if (e.Key == Key.Down)
            {
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
            if (e.Key == Key.Left)
            {
                if (i + 1 < 3)
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
            if (e.Key == Key.Right)
            {
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

        }

        private static Tuple<int, int> emptyPos(int[,] a, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (a[i, j] == 8)
                        return Tuple.Create(i, j);
                }
            }
            return null;
        }

        static void Swap<T>(ref T x, ref T y)
        {
            T t = y;
            y = x;
            x = t;
        }

    }
}
