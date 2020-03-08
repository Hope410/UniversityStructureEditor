using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public List<Institute> institutes = new List<Institute>();
        public List<Subject> subjects = new List<Subject>();

        public object selectedCourse { 
            get
            {
                int index1 = listBox1.SelectedIndex;
                int index2 = listBox2.SelectedIndex;
                if (index1 != -1 && index2 != -1)
                {
                    return institutes[index1].courses[index2];
                } else
                {
                    return null;
                }
            }
        }

        public object selectedGroup
        {
            get
            {
                int index1 = listBox1.SelectedIndex;
                int index2 = listBox2.SelectedIndex;
                int index3 = listBox3.SelectedIndex;
                if (index1 != -1 && index2 != -1 && index3 != -1)
                {
                    return institutes[index1].courses[index2].groups[index3];
                }
                else
                {
                    return null;
                }
            }
        }

        public object selectedStudent
        {
            get
            {
                int index1 = listBox1.SelectedIndex;
                int index2 = listBox2.SelectedIndex;
                int index3 = listBox3.SelectedIndex;
                int index4 = listBox4.SelectedIndex;
                if (index1 != -1 && index2 != -1 && index3 != -1 && index4 != -1)
                {
                    return institutes[index1].courses[index2].groups[index3].students[index4];
                }
                else
                {
                    return null;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        //=========================Институты=========================

        private void button3_Click(object sender, EventArgs e)
        {
            Action<string, string> callback = addInstitute;
            Editor editor = new Editor(callback, "", "Название");
            editor.Show();
        }

        public void addInstitute(string initialTitle, string newTitle)
        {
            institutes.Add(new Institute(newTitle, new List<Course>()));
            listBox1.Items.Add(newTitle);
            listBox1.SetSelected(listBox1.Items.Count - 1, true);
        }

        public void editInstituteTitle(string initialTitle, string newTitle)
        {
            Institute editing = institutes.Find(institute => institute.title == initialTitle);
            editing.title = newTitle;

            int listIndex = listBox1.FindString(initialTitle);
            listBox1.Items[listIndex] = newTitle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Action<string, string> callback = editInstituteTitle;
            Editor editor = new Editor(callback, listBox1.SelectedItem.ToString(), "Название");
            editor.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            institutes.Remove(institutes.Find(institute => 
                institute.title == listBox1.SelectedItem.ToString())
            );

            listBox1.Items.Remove(listBox1.SelectedItem);
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listView1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;

            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listView1.Items.Clear();

            foreach (Course course in institutes[index].courses)
            {
                listBox2.Items.Add(course.number.ToString());
            }
        }

        //=========================Курсы=========================

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            Action<string, string> callback = addCourse;
            Editor editor = new Editor(callback, "", "Номер");
            editor.Show();
        }

        public void addCourse(string initialNumber, string newNumber)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index == -1) return;

                int number = Int32.Parse(newNumber);

                if (number <= 0)
                {
                    throw new FormatException();
                }

                institutes[index].courses.Add(new Course(number, new List<Group>()));
                listBox2.Items.Add(newNumber);
                listBox2.SetSelected(listBox2.Items.Count - 1, true);
            } catch (FormatException)
            {
                MessageBox.Show("Номер курса должен быть целым числом больше нуля", "Ошибка");
            }
        }

        public void editCourseNumber(string initialNumber, string newNumber)
        {
            try
            {
                int index = listBox1.SelectedIndex;
                if (index == -1) return;

                int number = Int32.Parse(newNumber);

                if (number <= 0)
                {
                    throw new FormatException();
                }

                Course editing = institutes[index].courses.Find(course => course.number == Int32.Parse(initialNumber));
                editing.number = number;

                int listIndex = listBox2.FindString(initialNumber);
                listBox2.Items[listIndex] = newNumber;
            }
            catch (FormatException)
            {
                MessageBox.Show("Номер курса должен быть целым числом больше нуля", "Ошибка");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            Action<string, string> callback = editCourseNumber;
            Editor editor = new Editor(callback, listBox2.SelectedItem.ToString(), "Номер");
            editor.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int index = listBox2.SelectedIndex;
            if (listBox1.SelectedIndex == -1) return;

            institutes[index].courses.Remove(
                institutes[index].courses.Find(course =>
                    course.number == Int32.Parse(listBox2.SelectedItem.ToString())
                )
            );

            listBox2.Items.Remove(listBox2.SelectedItem);
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listView1.Items.Clear();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedCourse == null) return;

            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listView1.Items.Clear();

            foreach (Group group in ((Course)selectedCourse).groups)
            {
                listBox3.Items.Add(group.title);
            }
        }

        //=========================Группы=========================

        private void button8_Click(object sender, EventArgs e)
        {
            if (selectedCourse == null) return;

            Action<string, string> callback = addGroup;
            Editor editor = new Editor(callback, "", "Название");
            editor.Show();
        }

        public void addGroup(string initialTitle, string newTitle)
        {
            if (selectedCourse == null) return;
            Course currentCourse = (Course)selectedCourse;

            currentCourse.groups.Add(new Group(newTitle, new List<Student>()));
            listBox3.Items.Add(newTitle);
            listBox3.SetSelected(listBox3.Items.Count - 1, true);
        }

        public void editGroupTitle(string initialTitle, string newTitle)
        {
            if (selectedCourse == null) return;
            Course currentCourse = (Course)selectedCourse;

            Group editing = (Group)currentCourse.getChild(initialTitle);
            editing.title = newTitle;

            int listIndex = listBox3.FindString(initialTitle);
            listBox3.Items[listIndex] = newTitle;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selectedCourse == null) return;

            Action<string, string> callback = editGroupTitle;
            Editor editor = new Editor(callback, listBox3.SelectedItem.ToString(), "Название");
            editor.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (selectedCourse == null) return;
            Course currentCourse = (Course)selectedCourse;

            currentCourse.groups.Remove(
                (Group)currentCourse.getChild(listBox3.SelectedItem.ToString())
            );

            listBox3.Items.Remove(listBox3.SelectedItem);
            listBox4.Items.Clear();
            listView1.Items.Clear();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedGroup == null) return;

            listBox4.Items.Clear();
            listView1.Items.Clear();

            foreach (Student student in ((Group)selectedGroup).students)
            {
                listBox4.Items.Add(student.lastname);
            }
        }

        //=========================Студенты=========================

        private void button11_Click(object sender, EventArgs e)
        {
            if (selectedGroup == null) return;

            Action<string, string> callback = addStudent;
            Editor editor = new Editor(callback, "", "Фамилия");
            editor.Show();
        }

        public void addStudent(string initialName, string newName)
        {
            if (selectedGroup == null) return;
            Group currentGroup = (Group)selectedGroup;

            currentGroup.students.Add(new Student(newName, new List<Mark>()));
            listBox4.Items.Add(newName);
            listBox4.SetSelected(listBox4.Items.Count - 1, true);
        }

        public void editStudentName(string initialName, string newName)
        {
            if (selectedGroup == null) return;
            Group currentGroup = (Group)selectedGroup;

            Student editing = (Student)currentGroup.getChild(initialName);
            editing.lastname = newName;

            int listIndex = listBox4.FindString(initialName);
            listBox4.Items[listIndex] = newName;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (selectedGroup == null) return;

            Action<string, string> callback = editStudentName;
            Editor editor = new Editor(callback, listBox4.SelectedItem.ToString(), "Фамилия");
            editor.Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (selectedGroup == null) return;
            Group currentGroup = (Group)selectedGroup;

            currentGroup.students.Remove(
                (Student)currentGroup.getChild(listBox4.SelectedItem.ToString())
            );

            listBox4.Items.Remove(listBox4.SelectedItem);
            listView1.Items.Clear();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedStudent == null) return;

            listView1.Items.Clear();

            foreach (Mark mark in ((Student)selectedStudent).marks)
            {
                ListViewItem listViewItem = new ListViewItem(new string[2] {
                    mark.subject.title,
                    mark.points.ToString()
                });
                listView1.Items.Add(listViewItem);
            }
        }

        //=========================Оценки=========================

        private void button14_Click(object sender, EventArgs e)
        {
            if (selectedStudent == null) return;

            Action<Subject, int> callback = addMark;
            MarkEditor editor = new MarkEditor(callback, subjects);
            editor.Show();
        }

        public void addMark(Subject subject, int points)
        {
            try
            {
                if (selectedStudent == null) return;

                Student student = (Student)selectedStudent;

                if (student.getChild(subject.title) != null)
                {
                    throw new Exception();
                }

                student.marks.Add(new Mark(subject, points));

                ListViewItem listViewItem = new ListViewItem(new string[2] {
                    subject.title,
                    points.ToString()
                });

                listView1.Items.Add(listViewItem);
            } catch(Exception)
            {
                MessageBox.Show("Оценка по данному предмету уже выставлена, вы можете ее изменить, нажав на кнопку \"Редактировать\"", "Ошибка");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (selectedStudent == null) return;
            if (listView1.SelectedItems.Count == 0) return;

            Student student = (Student)selectedStudent;

            Mark selectedMark = (Mark)student.getChild(listView1.SelectedItems[0].SubItems[0].Text);

            Action<Subject, int> callback = editMark;
            MarkEditor editor = new MarkEditor(callback, subjects, selectedMark);
            editor.Show();
        }

        public void editMark(Subject subject, int points)
        {
            if (selectedStudent == null) return;
            Student student = (Student)selectedStudent;

            ((Mark)student.getChild(subject.title)).points = points;
            listView1.FindItemWithText(subject.title).SubItems[1].Text = points.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (selectedStudent == null) return;
            if (listView1.SelectedItems.Count == 0) return;

            Student student = (Student)selectedStudent;

            Mark selectedMark = (Mark)student.getChild(listView1.SelectedItems[0].SubItems[0].Text);

            student.marks.Remove(selectedMark);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("out.txt"))
            {
                sw.WriteLine("Список отличников:");

                foreach (Institute institute in institutes)
                {
                    foreach (Course course in institute.courses)
                    {
                        foreach (Group group in course.groups)
                        {
                            foreach (Student student in group.students)
                            {
                                if (student.allMatch(5))
                                {
                                    sw.WriteLine($"Фамилия: {student.lastname}, Группа: {group.title}, Курс: {course.number}, Институт: {institute.title}");
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Готово", "Вывод результата в файл");
            }
        }
    }
}
