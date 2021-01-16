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
    /// Logika interakcji dla klasy WriteDataPage.xaml
    /// </summary>
    public partial class WriteDataPage : Page
    {
        public WriteDataPage()
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
                    button.Background = Brushes.LightGray;
                    button.Click += Button_Click;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons.Last().Add(button);
                    GridPixels.Children.Add(buttons.Last().Last());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxStatus.Visibility = Visibility.Hidden;
            Button button = sender as Button;
            if (button.Background == Brushes.LightGray)
                button.Background = Brushes.Black;
            else button.Background = Brushes.LightGray;
            //throw new NotImplementedException();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxStatus.Visibility = Visibility.Hidden;
            foreach (var children in GridPixels.Children)
            {
                Button pixel = children as Button;
                pixel.Background = Brushes.LightGray;
            }
        }

        

        private List<bool> GetPixelsValues()
        {
            List<bool> pixels = new List<bool>();
            foreach (var children in GridPixels.Children)
            {
                Button button = children as Button;
                bool pixel = button.Background == Brushes.Black ? true : false;
                pixels.Add(pixel);
            }
            return pixels;
        }

        private void BtnSavePixels_Click(object sender, RoutedEventArgs e)
        {
            string errorCode = "";
            string path = TxPath.Text;
            string subDirectory = "data";
            bool[] pixels = GetPixelsValues().ToArray();
            //byte[] bytes = Array.ConvertAll(pixels, b => b ? (byte)1 : (byte)0);
            IEnumerable<byte> bytes = Array.ConvertAll(pixels, b => b ? (byte)1 : (byte)0);
            IEnumerable<byte> bytesFromFile = Enumerable.Empty<byte>();
            if(!Directory.Exists(subDirectory))
            {
                Directory.CreateDirectory(subDirectory);
            }
            try
            {
                bytesFromFile = File.ReadAllBytes(subDirectory + "\\" + path + ".bin");
                errorCode = "Dopisano do pliku!";
                TxStatus.Foreground = Brushes.Green;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Nie znaleziono pliku");
                errorCode = "Utworzono i poprawnie zapisano plik!";
                TxStatus.Foreground = Brushes.Green;
            }

            TxStatus.Text = errorCode;
            TxStatus.Visibility = Visibility.Visible;

            IEnumerable<byte> bytesToWrite = bytesFromFile.Concat(bytes);

            File.WriteAllBytes(subDirectory + "\\" + path + ".bin", bytesToWrite.ToArray());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
