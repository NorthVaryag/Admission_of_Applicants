using System;
using System.Collections.Generic;
using System.Data;
using Admission_of_Applicants.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class EmployeesRepository
{
    MySqlConnection connection;

    public EmployeesRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<Employees> GetAllEmployees()
    {
        List<Employees> serviceList = new List<Employees>();
        string sql = "select e.id as Id, e.first_name as FirstName, e.last_name as LastName, e.surname as Surname," +
                     " e.gender as Gender, e.age as Age, e.number_phone as NumberPhone, e.email as Email," +
                     " e.address as Adress, e.experience as Experience, e.salary as Salary \n            " +
                     "           from employees e ";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    serviceList.Add(new Employees
                    {
                        Id = dr.GetInt32("Id"),
                        FirstName = dr.GetString("FirstName"),
                        LastName = dr.GetString("LastName"),
                        Surname = dr.GetString("Surname"),
                        Gender = dr.GetString("Gender"),
                        Age = dr.GetInt32("Age"),
                        NumberPhone = dr.GetString("NumberPhone"),
                        Email = dr.GetString("Email"),
                        Adress = dr.GetString("Adress"),
                        Experience = dr.GetInt32("Experience"),
                        Salary = dr.GetDecimal("Salary")
                    });
                }
            }
            connection.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return serviceList;
    }
}