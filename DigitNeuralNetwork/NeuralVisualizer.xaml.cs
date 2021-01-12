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
    /// Logika interakcji dla klasy NeuralVisualizer.xaml
    /// </summary>
    public partial class NeuralVisualizer : Page
    {
        private List<Category> categories;
        private List<byte> readedBytes;

        public NeuralVisualizer()
        {
            InitializeComponent();

            NeuralNetwork nn = new NeuralNetwork(64, 32, 2, 0.2);

            

            //Category zero = new Category()



            //TxWeights.Text = "";
            //foreach (var row in nn.weightsIH)
            //{
            //    foreach (var item in row)
            //    {
            //        TxWeights.Text += "[" + item + "]";
            //    }
            //    TxWeights.Text += "\n";
            //}
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".bin";
            dialog.Filter = "Binary File (.bin)|*.bin";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                //Console.WriteLine("SCIEZKA: " + filename);
                readedBytes = File.ReadAllBytes(filename).OfType<byte>().ToList();

                Category zeros = new Category(readedBytes, DigitNeuralNetwork.categories.zero, 64);

                String s = "Training: " + zeros.GetTraining().Count;
                s += "\n Testing: " + zeros.GetTesting().Count;
                s += "\n Kategoria: " + zeros.GetTraining().First().category;

                //Console.WriteLine("Training: " + zeros.GetTraining().Count);
                //Console.WriteLine("Testing: " + zeros.GetTesting().Count);
                TxWeights.Text = s;

                //readedBytesList = readedBytes.ToList();
                //SldPicture.Maximum = readedBytes.Count() / 64;
                //SldPicture_ValueChanged(SldPicture, null);

            }
        }
    }
}
