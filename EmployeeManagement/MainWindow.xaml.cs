using System;
using System.Windows;
using System.Windows.Data;
using Newtonsoft.Json;
using PagedList;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeBusinessObjects;
using EmployeeBusinessLayer;

namespace EmployeeManagement
{    
    public partial class MainWindow : Window
    {
        private readonly IEmployeeBL _service;
        IPagedList<Employee> list;
        int pageNumber = 1; string messageBoxText = "", caption = "Message";
        public MainWindow(IEmployeeBL service)
        {
            _service = service;
            InitializeComponent();
        }
        private void LoadEmployees(object sender, RoutedEventArgs e)
        {
            if (SearchEmployee.Text == null || SearchEmployee.Text =="")                           
                GetEmployees();           
            else 
            {
                if (int.TryParse(SearchEmployee.Text, out int employeeId))
                    GetEmployeeById(employeeId);
                else
                {
                    messageBoxText = "Kindly search with valid employee id.";
                    MessageBox.Show(messageBoxText, caption);
                }
            }                         
        }

        private void GetEmployeeById(int id)
        {            
            List<Employee> employeelist = new List<Employee>();
            try {
                var response = _service.GetEmployeebyID(id);
                if (response != null) {
                    employeelist.Add(response);
                    EmployeeGrid.DataContext = employeelist;
                    NxtPage.IsEnabled = false;
                    PrevPage.IsEnabled = false;
                    PageNumber.Content = string.Format("Page {0}", pageNumber);
                }
            }
            catch {
                messageBoxText = "Please try again after sometime.";
                MessageBox.Show(messageBoxText, caption);
            }
        }
        private async void GetEmployees()
        {
            try {
                list = await GetPageListData();
                PrevPage.IsEnabled = list.HasPreviousPage;
                NxtPage.IsEnabled = list.HasNextPage;
                EmployeeGrid.DataContext = list;
                PageNumber.Content = string.Format("Page {0}/{1}", pageNumber, list.PageCount);
            }
            catch {
                messageBoxText = "Please try again after sometime.";
                MessageBox.Show(messageBoxText, caption);
            }            
        }
       
        public void SaveEmployee(Employee employee)
        {           
            try {
                var response = _service.SaveEmployee(employee);
                if (response == "OK") 
                    messageBoxText = "User added successfully";
                if(response == "Invalid")
                    messageBoxText = "Data entered seems incorrect. Please try again.";
            }
            catch(Exception ex) {
                messageBoxText = "Please try again after sometime.";                
            }
            finally {                
                MessageBox.Show(messageBoxText, caption);
            }
        }
        public void UpdateEmployee(Employee emp)
        {            
            try {
                var response = _service.UpdateEmployee(emp);
                if (response != null) {
                    messageBoxText = "User deatils Modified successfully";                    
                }
            }
            catch (Exception ex) {
                messageBoxText = "Please try again after sometime.";                
            }
            finally {
                GetEmployees();                
                MessageBox.Show(messageBoxText, caption);
            }            
        }
        private void DeleteEmployee(int empid)
        {
            try {
                var response = _service.DeleteEmployee(empid);
                if (response == "OK") {
                    messageBoxText = "User removed successfully";                   
                    MessageBox.Show(messageBoxText, caption);
                    GetEmployees();
                }                
            }
            catch (Exception ex) {
                messageBoxText = "Please try again after sometime.";               
            }
            finally {
                GetEmployees();
                MessageBox.Show(messageBoxText, caption);
            }
        }
        private void SaveEmployeeDetails(object sender, RoutedEventArgs e)
        {
            var employee = new Employee()
            {                
                id = EmployeeId.Text == "" ? 0 : Convert.ToInt32(EmployeeId.Text),
                name = Name.Text.ToString(),
                email = Email.Text.ToString(),
                gender = Gender.Text.ToString(),
                status = Status.Text.ToString()
            };
            if (employee.id == 0)
            {
                SaveEmployee(employee);
                GetEmployees();
            }
            else
            {
                UpdateEmployee(employee);
                GetEmployees();
            }
            EmployeeId.Text = "";
            Name.Text = "";
            Email.Text = "";
            Gender.Text = "";
            Status.Text = "";
            GetEmployees();
        }
        private void EditEmployeeDetails(object sender, RoutedEventArgs e)
        {
            Employee employee = ((FrameworkElement)sender).DataContext as Employee;
            EmployeeId.Text = employee.id.ToString();
            Name.Text = employee.name;
            Email.Text = employee.email;
            Gender.Text = employee.gender;
            Status.Text = employee.status;
        }
        void DeleteEmployeeDetails(object sender, RoutedEventArgs e)
        {
            Employee employee = ((FrameworkElement)sender).DataContext as Employee;
            this.DeleteEmployee(employee.id);
            GetEmployees();            
        }        

        public async Task<IPagedList<Employee>> GetPageListData(int pageNumber =1, int pageSize = 5)
        {            
            var response = _service.GetEmployees();            
            return await Task.Factory.StartNew(() => { return response.ToPagedList(pageNumber, pageSize); });
        }

        private async void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (list.HasPreviousPage) {
                list = await GetPageListData(--pageNumber);
                PrevPage.IsEnabled = list.HasPreviousPage;
                NxtPage.IsEnabled = list.HasNextPage;
                EmployeeGrid.DataContext = list;
                PageNumber.Content = string.Format("Page {0}/{1}", pageNumber, list.PageCount);
            }
        }

        private async void NextPage(object sender, RoutedEventArgs e)
        {
            if (list.HasNextPage) {
                list = await GetPageListData(++pageNumber);
                PrevPage.IsEnabled = list.HasPreviousPage;
                NxtPage.IsEnabled = list.HasNextPage;
                EmployeeGrid.DataContext = list;
                PageNumber.Content = string.Format("Page {0}/{1}", pageNumber, list.PageCount);
            }
        }
    }
}
