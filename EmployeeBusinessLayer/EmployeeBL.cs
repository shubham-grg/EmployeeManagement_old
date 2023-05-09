using EmployeeBusinessObjects;
using EmployeeDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBusinessLayer
{
    public class EmployeeBL :IEmployeeBL
    {
        EmployeeDAO objdao = new EmployeeDAO();
        public string SaveEmployee(Employee employee)
        {
            if (employee.name != "" || employee.email != "" || employee.gender != "" || employee.status != "")
                return objdao.SaveEmployee(employee);
            else
                return "Invalid";
        }

        public Employee UpdateEmployee(Employee employee)
        {
            return objdao.UpdateEmployee(employee);
        }

        public string DeleteEmployee(int employeeid)
        {
            return objdao.DeleteEmployee(employeeid);
        }

        public List<Employee> GetEmployees()
        {
            return objdao.GetEmployees();
        }

        public Employee GetEmployeebyID(int id)
        {
            return objdao.GetEmployeebyID(id);
        }
        
    }
}
