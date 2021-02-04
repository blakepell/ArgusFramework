using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Argus.Extensions;
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

            var sb = new StringBuilder();
            sb.Append("Blake Pell");

            MessageBox.Show(sb.StartsWith('B', false).ToString()); // true
            MessageBox.Show(sb.StartsWith('b', true).ToString()); // true
            MessageBox.Show(sb.StartsWith('b', false).ToString()); // false

            MessageBox.Show(sb.EndsWith('l', false).ToString()); // true
            MessageBox.Show(sb.EndsWith('L', true).ToString()); // true
            MessageBox.Show(sb.EndsWith('L', false).ToString()); // false



            //var op = new ObjectPool<Person>();
            //op.Max = 10;

            //op.ReturnAction = i =>
            //{
            //    i.FirstName = "Cleared!";
            //    i.LastName = "Cleared!";
            //};

            //var p = op.Get();
            //p.FirstName = "Blake";
            //p.LastName = "Pell";
            //op.Return(p);
            //p.FirstName = "Edwardo";
            //op.Return(p);


            //var xy = op.Get();


            //int x = 0;

            //Parallel.For(0, 10000,
            //             index => {
            //                 var newObject = op.Get();
            //                 op.Return(newObject);

            //                 //FileInfo fi = new FileInfo(files[index]);
            //                 //long size = fi.Length;
            //                 //Interlocked.Add(ref totalSize, size);
            //             });

            ////for (int i = 0; i < 100; i++)
            ////{
            ////    var newObject = op.Get();
            ////    op.Return(newObject);
            ////}

            //MessageBox.Show(op.CounterNewObjects + "|" + op.CounterReusedObjects);

        }


        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

    }
}
