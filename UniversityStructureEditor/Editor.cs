using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp2
{
    public partial class Editor : Form
    {
        Action<string, string> callback;
        string initialText;

        public Editor(Action<string, string> callback, string initialText, string fieldName)
        {
            InitializeComponent();
            this.callback = callback;
            this.initialText = initialText;
            textBox1.Text = initialText;
            label1.Text = fieldName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    throw new Exception("Название предмета не может быть пустым");
                }

                callback(this.initialText, textBox1.Text);
                Close();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1_Click(new object(), new EventArgs());
            }
        }
    }
}
