using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeGenWPF.ViewModels;

namespace CodeGenWPF.Views
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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutBox(this);
            about.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).New();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).New();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).CompileCSharp30();
        }

        private void RunUnitTests_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).RunUnitTests();
        }

        private void ReGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).GenerateCode();
        }

        private void textOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectionStart = textBox.Text.Length - 1;
            textBox.ScrollToEnd();
            textBox.UpdateLayout();
        }
    }
}
