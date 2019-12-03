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

        private void chooseImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                const int startX = 30;
                const int startY = 30;
                const int width = 100;
                const int height = 100;
                var source = new BitmapImage(
                    new Uri(screen.FileName, UriKind.Absolute));
                Debug.WriteLine($"{source.Width} - {source.Height}");

                previewImage.Width = 350;
                previewImage.Height = 300;
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
                            var h = (int)source.Height/3;
                            var w = (int)source.Height/3;
                            //Debug.WriteLine($"Len = {len}");
                            var rect = new Int32Rect(j * w, i * h, w, h);
                            var cropBitmap = new CroppedBitmap(source,
                                rect);

                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = width;
                            cropImage.Height = height;
                            cropImage.Source = cropBitmap;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + j * (width + 2));
                            Canvas.SetTop(cropImage, startY + i * (height + 2));

                            cropImage.Tag = new Tuple<int, int>(i, j);
                            //cropImage.MouseLeftButtonUp
                        }
                    }
                }

            }
        }

        private void Information_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("8 Puzzle Game\n1712472 - Lo Huy Hung\n1712555 - Chau Vinh Lap","Information");

        }
    }
}
