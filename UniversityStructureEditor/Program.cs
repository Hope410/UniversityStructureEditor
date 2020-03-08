using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class InvalidCastException : SystemException
    {
        public InvalidCastException(string message)
        {
            MessageBox.Show("Произведено недопустимое преведение", "Ошибка");
        }
    }

    interface IChildren
    {
        object getChild(string title);
    }

    interface IAllMatch
    {
        bool allMatch(object obj);
    }

    public class Institute
    {
        public string title;
        public List<Course> courses;

        public Institute(string title, List<Course> courses)
        {
            this.title = title;
            this.courses = courses;
        }
    }

    public class Course : IChildren
    {
        public int number;
        public List<Group> groups;

        public Course(int number, List<Group> groups)
        {
            this.number = number;
            this.groups = groups;
        }

        public object getChild(string title)
        {
            return groups.Find(group => group.title == title);
        }
    }

    public class Group : IChildren
    {
        public string title;
        public List<Student> students;

        public Group(string title, List<Student> students)
        {
            this.title = title;
            this.students = students;
        }

        public object getChild(string lastname)
        {
            return students.Find(student => student.lastname == lastname);
        }
    }

    public class Subject
    {
        public string title;

        public Subject(string title)
        {
            this.title = title;
        }
    }

    public class Mark
    {
        public Subject subject;
        public int points;

        public Mark(Subject subject, int points)
        {
            this.subject = subject;
            this.points = points;
        }
    }

    public class Student : IChildren, IAllMatch
    {
        public string lastname;
        public List<Mark> marks;

        public Student(string lastname, List<Mark> marks)
        {
            this.lastname = lastname;
            this.marks = marks;
        }

        public object getChild(string title)
        {
            return marks.Find(mark => mark.subject.title == title);
        }

        public bool allMatch (object points)
        {
            bool isMatch = true;

            foreach (Mark mark in marks)
            {
                if (mark.points != (int)points)
                {
                    isMatch = false;
                    break;
                }
            }

            return isMatch;
        }
    }
}
