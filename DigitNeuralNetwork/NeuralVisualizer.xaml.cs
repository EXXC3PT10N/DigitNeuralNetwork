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
        private List<Category> dataset = new List<Category>();
        //private List<byte> readedBytes;
        private const string DATA_PATH = "data\\";
        private const int NUM_OF_PIXELS = 64;

        public NeuralVisualizer()
        {
            InitializeComponent();

            NeuralNetwork nn = new NeuralNetwork(64, 32, 2, 0.2);

            LoadData("zero.bin",categories.zero,NUM_OF_PIXELS);
            LoadData("one.bin",categories.one,NUM_OF_PIXELS);
            LoadData("two.bin",categories.two,NUM_OF_PIXELS);
            Console.WriteLine(dataset.Count);

            List<LabeledImage> training = new List<LabeledImage>();
            List<LabeledImage> testing = new List<LabeledImage>();

            foreach (var category in dataset)
            {
                training.AddRange(category.GetTraining());
                testing.AddRange(category.GetTesting());
            }
            training.Shuffle();
            testing.Shuffle();
        }

        private void LoadData(string path, categories label,int numOfPixels)
        {
            path = path.Insert(0, DATA_PATH);
            List<byte> bytes = File.ReadAllBytes(path).OfType<byte>().ToList();
            dataset.Add(new Category(bytes, label, numOfPixels));
        }

        

        //private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.DefaultExt = ".bin";
        //    dialog.Filter = "Binary File (.bin)|*.bin";
        //    Nullable<bool> result = dialog.ShowDialog();
        //    if (result == true)
        //    {
        //        string filename = dialog.FileName;
        //        filename = "zera.bin";
        //        Console.WriteLine("SCIEZKA: " + filename);
        //        readedBytes = File.ReadAllBytes(filename).OfType<byte>().ToList();

        //        Category zeros = new Category(readedBytes, DigitNeuralNetwork.categories.zero, 64);

        //        String s = "Training: " + zeros.GetTraining().Count;
        //        s += "\n Testing: " + zeros.GetTesting().Count;
        //        s += "\n Kategoria: " + zeros.GetTraining().First().category;

        //        //Console.WriteLine("Training: " + zeros.GetTraining().Count);
        //        //Console.WriteLine("Testing: " + zeros.GetTesting().Count);
        //        TxWeights.Text = s;

        //        //readedBytesList = readedBytes.ToList();
        //        //SldPicture.Maximum = readedBytes.Count() / 64;
        //        //SldPicture_ValueChanged(SldPicture, null);

        //    }
        //}
    }
}
