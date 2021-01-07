using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace DigitNeuralNetwork
{
    /// <summary>
    /// Logika interakcji dla klasy ReadDataPage.xaml
    /// </summary>
    public partial class ReadDataPage : Page
    {
        IEnumerable<byte> readedBytes;
        List<byte> readedBytesList;
        public ReadDataPage()
        {
            InitializeComponent();
            InitializePixels();
        }

        void InitializePixels()
        {
            List<List<Button>> buttons = new List<List<Button>>();
            for (int i = 0; i < 8; i++)
            {
                buttons.Add(new List<Button>());
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    //button.Background = Brushes.LightGray;
                    //button.IsEnabled = false;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons.Last().Add(button);
                    GridPixels.Children.Add(buttons.Last().Last());
                }
            }
            ResetButtonColors();
        }

        void ResetButtonColors()
        {
            foreach (var children in GridPixels.Children)
            {
                Button pixel = children as Button;
                pixel.Background = Brushes.LightGray;
            }
        }

        

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".bin";
            dialog.Filter = "Binary File (.bin)|*.bin";
            Nullable<bool> result = dialog.ShowDialog();
            if(result == true)
            {
                string filename = dialog.FileName;
                //Console.WriteLine("SCIEZKA: " + filename);
                readedBytes = File.ReadAllBytes(filename);
                readedBytesList = readedBytes.ToList();
                SldPicture.Maximum = readedBytes.Count() / 64;
                SldPicture_ValueChanged(SldPicture, null);

            }
        }

        private void SldPicture_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ResetButtonColors();
                Slider slider = sender as Slider;

                int offset = (int)slider.Value - 1;

                for (int i = 0; i < 64; i++)
                {
                    Button pixel = GridPixels.Children[i] as Button;
                    if (Convert.ToBoolean(readedBytesList[i + offset * 64] * 255) == true)
                    {
                        pixel.Background = Brushes.Black;
                    }
                }
            } catch(Exception ex)
            {

            }

            

        }
    }
}
