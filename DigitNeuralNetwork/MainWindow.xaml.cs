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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void BtnWriteData_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WriteDataPage());
        }

        private void BtnReadData_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReadDataPage());
        }

        private void BtnNeuralNetwork_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NeuralVisualizer());
        }
    }
}