using EmployeeBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDataAccessLayer
{
    public interface IEmployeeDAO
    {
        string SaveEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        string DeleteEmployee(int employeeid);
        List<Employee> GetEmployees();
        Employee GetEmployeebyID(int id);
    }
}
