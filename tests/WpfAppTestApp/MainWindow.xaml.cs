using System;
using System.Collections.Generic;
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
using Argus.Memory;

namespace WpfAppTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            var op = new ObjectPool<Person>();
            var p = op.Get();
            p.FirstName = "Blake";
            p.LastName = "Pell";
            op.Return(p);
            p.FirstName = "Edwardo";
            op.Return(p);

            var xy = op.Get();
            

            int x = 0;

        }


        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

    }
}
