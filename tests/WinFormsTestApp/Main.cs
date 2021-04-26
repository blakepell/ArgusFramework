using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Argus.Extensions;
using Argus.Memory;
using Argus.Windows.Hardware;

namespace WinFormsTestApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        private ObjectPool<Person> _pool = new ObjectPool<Person>();


        private void Main_Load(object sender, EventArgs e)
        {
            var p = new Person();
            p.FirstName = "Blake";
            p.LastName = "Pell";
            _pool.Return(p);

            var p2 = new Person();
            p2.FirstName = "John";
            p2.LastName = "Doe";
            _pool.Return(p2);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void ButtonDoSomething_Click(object sender, EventArgs e)
        {
            _pool.InvokeAll((item) =>
            {
                item.LastName = "Smith";
            });

            var p = _pool.Get();
            MessageBox.Show($"{p.FirstName} {p.LastName}");

            p = _pool.Get();
            MessageBox.Show($"{p.FirstName} {p.LastName}");

        }

    }
}
