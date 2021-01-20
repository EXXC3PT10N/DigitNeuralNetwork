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
using System.Windows.Threading;

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
        private const int NUM_OF_DATA = 10;
        private int hiddenNeurons = 16;
        private double learningRate = 0.2;
        private bool stopTraining = false;
        private List<LabeledImage> training = new List<LabeledImage>();
        private List<LabeledImage> testing = new List<LabeledImage>();
        private NeuralNetwork nn;

        //public delegate void TrainingDelegate();

        public NeuralVisualizer()
        {
            InitializeComponent();
            InitializePixels();
          
            //Making neural network
            nn = new NeuralNetwork(NUM_OF_PIXELS, hiddenNeurons, NUM_OF_DATA, learningRate);
            
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
                    button.Click += Button_Click;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons.Last().Add(button);
                    GridPixels.Children.Add(buttons.Last().Last());
                }
            }
            ResetButtonColors();
            GridPixels.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Background == Brushes.LightGray)
                button.Background = Brushes.Black;
            else button.Background = Brushes.LightGray;
            QueryButtons();
        }

        private void QueryButtons()
        {
            double[,] inputs = new double[NUM_OF_PIXELS, 1];
           
            for(int i = 0; i < 64; i++)
            {
                Button button = GridPixels.Children[i] as Button;
                inputs[i, 0] = button.Background == Brushes.Black ? 1 : 0;
            }
            var results = nn.Query(inputs);

            var m = results[0, 0];
            foreach (var r in results)
            {
                m = Math.Max(m, r);
            }

            //List<double> resultAsList = results.Cast<double>().ToList();
            //var guess = resultAsList.IndexOf(m);
            var guess = results.Cast<double>().ToList().IndexOf(m);
            TxResult.Text = guess.ToString();
        }

        void ResetButtonColors()
        {
            foreach (var children in GridPixels.Children)
            {
                Button pixel = children as Button;
                pixel.Background = Brushes.LightGray;
            }
        }

        private void LoadData(string path, categories label,int numOfPixels)
        {
            path = path.Insert(0, DATA_PATH);
            List<byte> bytes = File.ReadAllBytes(path).OfType<byte>().ToList();
            dataset.Add(new Category(bytes, label, numOfPixels));
        }

        private double Train()
        {


            int incorrects = 4;
            double errorRate = 100;
            //int stop = 0;
            int iterations = 0;
            while (errorRate > 10 && !stopTraining)
            {
                iterations++;
                //stop++;
               
                incorrects = 0;
                foreach (var item in training)
                {
                    double[,] inputData = new double[64, 1];
                    for (int i = 0; i < 64; i++)
                    {
                        inputData[i, 0] = item.pixels[i];
                    }
                    double[,] target = new double[10, 1] { { 0 }, { 0 }, { 0 }, { 0 }, { 0 }, { 0 }, { 0 }, { 0 }, { 0 }, { 0 } };
                    target[(int)item.category, 0] = 1;
                    nn.Train(inputData, target);

                }

                errorRate = Test();
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => this.PgBarTrain.Value = 100 - errorRate));
                

                Console.WriteLine(errorRate);
                
            }

            Console.WriteLine("Iteracje: " + iterations);
            Console.WriteLine("END");

            stopTraining = false;
            //*********************************
            
            return errorRate;

        }

        private double Test()
        {
            double errorRate;
            int count = 0;
            int errors = 0;
            foreach (var test in testing)
            {
                count++;
                double[,] inputData = new double[64, 1];
                for (int i = 0; i < 64; i++)
                {
                    inputData[i, 0] = test.pixels[i];
                }

                double[,] wynik = nn.Query(inputData);

                var m = wynik[0, 0];
                foreach (var w in wynik)
                {
                    m = Math.Max(m, w);
                }

                List<double> asd = wynik.Cast<double>().ToList();
                var guess = asd.IndexOf(m);
                var real = test.category;
                if (guess == (int)real)
                {
                    //Console.WriteLine("!!!Correct! guess: " + guess + ", real: " + real);
                }
                else
                {
                    errors++;
                    //Console.WriteLine("!!!INCORRECT! guess: " + guess + ", real: " + real);
                }

            }
            errorRate = (double)errors / (double)count * 100;
            return errorRate;

        }
        
        private async void BtnTrain_Click(object sender, RoutedEventArgs e)
        {
            BtnStop.IsEnabled = true;
            SldLr.IsEnabled = false;
            SldNeurons.IsEnabled = false;

            double errorRate = await Task.Run(() => Train());
            TxError.Text = Math.Round(errorRate, 2) + "%";
            

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            stopTraining = true;
            BtnStop.IsEnabled = false;

            SldLr.IsEnabled = true;
            SldNeurons.IsEnabled = true;

        }


        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            BtnLoadData.IsEnabled = false;
            //Loading and preparing data
            LoadData("zero.bin", categories.zero, NUM_OF_PIXELS);
            LoadData("one.bin", categories.one, NUM_OF_PIXELS);
            LoadData("two.bin", categories.two, NUM_OF_PIXELS);
            LoadData("three.bin", categories.three, NUM_OF_PIXELS);
            LoadData("four.bin", categories.four, NUM_OF_PIXELS);
            LoadData("five.bin", categories.five, NUM_OF_PIXELS);
            LoadData("six.bin", categories.six, NUM_OF_PIXELS);
            LoadData("seven.bin", categories.seven, NUM_OF_PIXELS);
            LoadData("eight.bin", categories.eight, NUM_OF_PIXELS);
            LoadData("nine.bin", categories.nine, NUM_OF_PIXELS);

            foreach (var category in dataset)
            {
                training.AddRange(category.GetTraining());
                testing.AddRange(category.GetTesting());
            }
            training.Shuffle();
            //Console.WriteLine("Training: " + training.Count);
            testing.Shuffle();
            BtnTrain.IsEnabled = true;
            BtnTest.IsEnabled = true;
            GridPixels.IsEnabled = true;
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            Test();
        }

        private void SldLr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            try
            { 
                Slider slider = sender as Slider;
                double val = slider.Value;
                learningRate = Math.Round(val,3);
                nn = new NeuralNetwork(NUM_OF_PIXELS, hiddenNeurons, NUM_OF_DATA, learningRate);
                TxLr.Text = learningRate.ToString();

            }
            catch (Exception ex)
            {

            }

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Slider slider = sender as Slider;
                int val = (int) slider.Value;
                hiddenNeurons = val;
                nn = new NeuralNetwork(NUM_OF_PIXELS, hiddenNeurons, NUM_OF_DATA, learningRate);
                TxNeurons.Text = hiddenNeurons.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var children in GridPixels.Children)
            {
                Button pixel = children as Button;
                pixel.Background = Brushes.LightGray;
            }
        }
    }
}
