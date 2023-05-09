using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBusinessObjects;
using EmployeeDataAccessLayer;

namespace EmployeeBusinessLayer
{
    public interface IEmployeeBL
    {
        string SaveEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        string DeleteEmployee(int employeeid);
        List<Employee> GetEmployees();
        Employee GetEmployeebyID(int id);
    }
}
