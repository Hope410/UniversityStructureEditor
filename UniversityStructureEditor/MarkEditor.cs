using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class MarkEditor : Form
    {
        Action<Subject, int> callback;
        List<Subject> subjects;

        public MarkEditor(Action<Subject, int> callback, List<Subject> subjects)
        {
            InitializeComponent();
            this.callback = callback;
            this.subjects = subjects;

            foreach (Subject subject in subjects)
            {
                comboBox1.Items.Add(subject.title);
            }
        }

        public MarkEditor(Action<Subject, int> callback, List<Subject> subjects, Mark selectedMark)
        {
            InitializeComponent();
            this.callback = callback;
            this.subjects = subjects;

            foreach (Subject subject in subjects)
            {
                comboBox1.Items.Add(subject.title);
            }

            comboBox1.Text = selectedMark.subject.title;
            comboBox1.Enabled = false;

            textBox1.Text = selectedMark.points.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int points = Int32.Parse(textBox1.Text);

                if (points < 1 || points > 5)
                {
                    throw new FormatException();
                }

                Subject subject = new Subject(comboBox1.Text);

                if (comboBox1.Text == "")
                {
                    throw new Exception("Название предмета не может быть пустым");
                }

                if (this.subjects.Find(sub => sub.title == subject.title) == null)
                {
                    this.subjects.Add(subject);
                }

                callback(subject, points);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Оценка должна быть целым числом больше нуля и меньше пяти", "Ошибка");
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button2_Click(new object(), new EventArgs());
            }
        }
    }
}
