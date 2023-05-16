using System;
using System.Diagnostics;
using System.Windows;
using Argus.Data.Mustache;

namespace WpfAppTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _saveFile = @"C:\Temp\Mustache.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(_saveFile))
            {
                TextLeft.Text = System.IO.File.ReadAllText(_saveFile);
            }
        }

        private void ButtonExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sw = new Stopwatch();
                System.IO.File.WriteAllText(_saveFile, TextLeft.Text);
                var data = Argus.Data.TestData.GetGenericList(10);

                sw.Start();
                var compiler = new FormatCompiler();
                var generator = compiler.Compile(TextLeft.Text);
                string result = generator.Render(data);
                TextRight.Text = result;
                sw.Stop();

                this.Title = $"{sw.ElapsedMilliseconds}ms";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
