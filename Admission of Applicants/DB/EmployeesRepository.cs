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
                     " e.address as Address, e.experience as Experience, e.salary as Salary \n            " +
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
                        Address = dr.GetString("Address"),
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
    
    
public void InsertEmployee(Employees employee)
    {
        string sql = "insert into employees (first_name, last_name, surname, gender, age, number_phone, email, address, experience, salary) " +
                     "values (@FirstName, @LastName, @Surname, @Gender, @Age, @NumberPhone, @Email, @Address, @Experience, @Salary)";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@FirstName", employee.FirstName);
                mc.Parameters.AddWithValue("@LastName", employee.LastName);
                mc.Parameters.AddWithValue("@Surname", employee.Surname);
                mc.Parameters.AddWithValue("@Gender", employee.Gender);
                mc.Parameters.AddWithValue("@Age", employee.Age);
                mc.Parameters.AddWithValue("@NumberPhone", employee.NumberPhone);
                mc.Parameters.AddWithValue("@Email", employee.Email);
                mc.Parameters.AddWithValue("@Address", employee.Address);
                mc.Parameters.AddWithValue("@Experience", employee.Experience);
                mc.Parameters.AddWithValue("@Salary", employee.Salary);

                mc.ExecuteNonQuery();
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
    }

    public void UpdateEmployee(Employees employee)
    {
        string sql = "update employees set first_name = @FirstName, last_name = @LastName, surname = @Surname, " +
                     "gender = @Gender, age = @Age, number_phone = @NumberPhone, email = @Email, " +
                     "address = @Address, experience = @Experience, salary = @Salary where id = @Id";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@FirstName", employee.FirstName);
                mc.Parameters.AddWithValue("@LastName", employee.LastName);
                mc.Parameters.AddWithValue("@Surname", employee.Surname);
                mc.Parameters.AddWithValue("@Gender", employee.Gender);
                mc.Parameters.AddWithValue("@Age", employee.Age);
                mc.Parameters.AddWithValue("@NumberPhone", employee.NumberPhone);
                mc.Parameters.AddWithValue("@Email", employee.Email);
                mc.Parameters.AddWithValue("@Address", employee.Address);
                mc.Parameters.AddWithValue("@Experience", employee.Experience);
                mc.Parameters.AddWithValue("@Salary", employee.Salary);
                mc.Parameters.AddWithValue("@Id", employee.Id);

                mc.ExecuteNonQuery();
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
        finally
        {
            connection.Close();
        } 
    }

    public void DeleteEmployee(int id)
    {
        string sql = "delete from employees where id = @Id";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", id);

                mc.ExecuteNonQuery();
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
    }
}