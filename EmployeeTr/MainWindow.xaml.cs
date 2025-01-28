using EmployeeTr.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace EmployeeTr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EmployeeTraining2Entities _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new EmployeeTraining2Entities();
            LoadData();
        }

        private void LoadData()
        {
            // Load groups into ListBox
            GroupsListBox.ItemsSource = _context.Groups.ToList();
            GroupsListBoxForCertificates.ItemsSource = _context.Groups.ToList();

            // Load courses into ListBox
            CoursesListBox.ItemsSource = _context.Courses.ToList();

            // Load instructors into ListBox
            InstructorsListBox.ItemsSource = _context.Instructors.ToList();
        }   

        private void delCourse_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesListBox.SelectedItem is Courses selectedCourse)
            {
                _context.Courses.Remove(selectedCourse);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void addGroup_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(GroupNameTextBox.Text))
            {
                var newGroup = new Groups { GroupName = GroupNameTextBox.Text };
                _context.Groups.Add(newGroup);
                _context.SaveChanges();
                LoadData();
                GroupNameTextBox.Clear();
            }
        }

        private void delGroup_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsListBox.SelectedItem is Groups selectedGroup)
            {
                _context.Groups.Remove(selectedGroup);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void addCourse_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CourseNameTextBox.Text) && int.TryParse(DurationTextBox.Text, out int duration))
            {
                var newCourse = new Courses
                {
                    CourseName = CourseNameTextBox.Text,
                    DurationInHours = duration
                };
                _context.Courses.Add(newCourse);
                _context.SaveChanges();
                LoadData();
                CourseNameTextBox.Clear();
                DurationTextBox.Clear();
            }
        }

        private void issueCert_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsListBoxForCertificates.SelectedItem is Groups selectedGroup)
            {
                var course = _context.Courses.FirstOrDefault(c => c.CourseID == selectedGroup.CourseID);
                if (course != null)
                {
                    var certificate = new Certificate
                    {
                        GroupID = selectedGroup.GroupID,
                        CourseID = course.CourseID,
                        DateIssued = DateTime.Now
                    };
                    //_context.Certificates.Add(certificate);
                    _context.SaveChanges();
                    MessageBox.Show($"Certificate issued for group: {selectedGroup.GroupName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No course is assigned to this group.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
