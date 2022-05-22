using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentInfoSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Student student1;
        public List<string> StudStatusChoices { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FillStudStatusChoices();
            this.DataContext = this;
            disableFields();
            btnExamNumber.IsEnabled = false;
        }

        private void FillStudStatusChoices()
        {
            StudStatusChoices = new List<string>();
            using (IDbConnection connection = new
            SqlConnection(Properties.Settings.Default.DbConnect))
            {
                string sqlquery =
                @"SELECT StatusDescr FROM StudStatus";
                IDbCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = sqlquery;
                IDataReader reader = command.ExecuteReader();
                bool notEndOfResult;
                notEndOfResult = reader.Read();
                while (notEndOfResult)
                {
                    string s = reader.GetString(0);
                    StudStatusChoices.Add(s);
                    notEndOfResult = reader.Read();
                }
            }
        }

        private void clearFields()
        {
            foreach (var item in MainGrid.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Clear();
                }
            }
        }
        private Boolean isStudentDataCorrect(Student student)
        {
            return student != null 
                && !String.IsNullOrWhiteSpace(student.firstName) 
                && !String.IsNullOrWhiteSpace(student.secondName) 
                && !String.IsNullOrWhiteSpace(student.lastName)
                && !String.IsNullOrWhiteSpace(student.faculty) 
                && !String.IsNullOrWhiteSpace(student.program) 
                && !String.IsNullOrWhiteSpace(student.degree)
                && !String.IsNullOrWhiteSpace(student.status) 
                && !String.IsNullOrWhiteSpace(student.facultyNumber) 
                && student.course != 0
                && student.flow != 0 
                && student.group != 0;
        }
        private void setStudent(Student student)
        {
            if (isStudentDataCorrect(student))
            {
                fillStudentInfo(student);
            }
            else
            {
                clearFields();
            }
        }
        private void fillStudentInfo(Student student)
        {
            this.student1 = student;

            txtFirstName.Text = this.student1.firstName;
            txtSecondName.Text = this.student1.secondName;
            txtLastName.Text = this.student1.lastName;
            txtFaculty.Text = this.student1.faculty;
            txtSpeciality.Text = this.student1.program;
            txtDegree.Text = this.student1.degree;
            txtStatus.SelectedItem = this.student1.status;
            txtFacultyNumber.Text = this.student1.facultyNumber;
            txtCourse.Text = Convert.ToString(this.student1.course);
            txtFlow.Text = Convert.ToString(this.student1.flow);
            txtGroup.Text = Convert.ToString(this.student1.group);
        }


        private void disableFields()
        {
            txtFirstName.IsEnabled = false;
            txtSecondName.IsEnabled = false;
            txtLastName.IsEnabled = false;
            txtFaculty.IsEnabled = false;
            txtSpeciality.IsEnabled = false;
            txtDegree.IsEnabled = false;
            txtStatus.IsEnabled = false;
            txtFacultyNumber.IsEnabled = false;
            txtCourse.IsEnabled = false;
            txtFlow.IsEnabled = false;
            txtGroup.IsEnabled = false;
        }

        private void enableFields()
        {
            txtFirstName.IsEnabled = true;
            txtSecondName.IsEnabled = true;
            txtLastName.IsEnabled = true;
            txtFaculty.IsEnabled = true;
            txtSpeciality.IsEnabled = true;
            txtDegree.IsEnabled = true;
            txtStatus.IsEnabled = true;
            txtFacultyNumber.IsEnabled = true;
            txtCourse.IsEnabled = true;
            txtFlow.IsEnabled = true;
            txtGroup.IsEnabled = true;

        }

        private void checkBoxEnableFields_Checked(object sender, RoutedEventArgs e)
        {
            enableFields();
        }

        private void checkBoxDisableFields_Unchecked(object sender, RoutedEventArgs e)
        {
            disableFields();
        }

        private Boolean TestStudentsIfEmpty()
        {
            StudentInfoContext context = new StudentInfoContext();          
            IEnumerable<Student> queryStudents = context.Students;
           
            int countStudents = queryStudents.Count();
            if (queryStudents == null)
            {
                return true;
            }
            else
                return false;
        }

        private void checkBoxFillStudent_Checked(object sender, RoutedEventArgs e)
        {
            btnExamNumber.IsEnabled = true;
            setStudent(createAndGetFirstStudentFromDB());
        }

        private Student createAndGetFirstStudentFromDB()
        {
            StudentInfoContext studentInfoContext = new StudentInfoContext();
            if (studentInfoContext.Students.FirstOrDefault() == null)
            {
                CopyTestStudents();
            }
            return studentInfoContext.Students.First();
        }

        private void CopyTestStudents()
        {
            StudentData studentData = new StudentData();
            StudentInfoContext context = new StudentInfoContext();
            foreach (Student st in studentData.getStudents())
            {
                context.Students.Add(st);
            }
            context.SaveChanges();
        }

        private void checkBoxFillStudent_Unchecked(object sender, RoutedEventArgs e)
        {
            btnExamNumber.IsEnabled = false;
            setStudent(null);
        }

        private void btnExamNumber_Click(object sender, RoutedEventArgs e)
        {
            int examNumber = 1;
            if (this.student1.StudentId % 2 == 0)
            {
                examNumber = 2;
            }
            MessageBox.Show("Студентът получава изпитен вариант номер " + examNumber);
        }
    }
}
