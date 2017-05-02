using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEdit
{
    public partial class Form2 : Form
    {
        public string find_text ="";
        public string replace_text ="";
        public int number_of_lines = 0;
        public int starting_column = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            find_text = textBox1.Text;
            replace_text = textBox2.Text;
            number_of_lines = Int32.Parse(textBox3.Text);
            starting_column = Int32.Parse(textBox4.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            find_text = textBox1.Text;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            replace_text = textBox2.Text;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            number_of_lines = Int32.Parse(textBox3.Text);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            starting_column = Int32.Parse(textBox4.Text);
        }
    }
}
