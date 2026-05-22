using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class ClientRepository
{
    MySqlConnection connection;

    public ClientRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<Client> GetAllClient()
    {
        List<Client> clientList = new List<Client>();
        string sql = "select c.id, ct.name_type as NameType, c.display_name as DisplayName, t.tariff_name as TariffName, c.is_custom as IsCustom," +
                     " c.monthly_payment as MonthlyPayment\nfrom clients c \njoin client_types ct on ct.id  = c.id_type \njoin tariffs t on t.id = c.tariff_id  ";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    clientList.Add(new Client
                    {
                        Id = dr.GetInt32("id"),
                        NameType = dr.GetString("NameType"),
                        DisplayName = dr.GetString("DisplayName"),
                        TariffName = dr.GetString("TariffName"),
                        IsCustom = dr.GetInt32("IsCustom"),
                        MonthlyPayment = dr.GetDecimal("MonthlyPayment")
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
        return clientList;
    }
    
    public void InsertClient(Client client)
    {
        // Предполагаем, что id_type = 1 и tariff_id = 1 для дефолтных значений
        string sql = "insert into clients (display_name, monthly_payment, id_type, tariff_id, is_custom) " +
                     "values (@DisplayName, @MonthlyPayment, 1, 1, @IsCustom)";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@DisplayName", client.DisplayName);
                mc.Parameters.AddWithValue("@MonthlyPayment", client.MonthlyPayment);
                mc.Parameters.AddWithValue("@IsCustom", client.IsCustom);
                mc.ExecuteNonQuery();
            }
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

    public void UpdateClient(Client client)
    {
        string sql = "update clients set display_name = @DisplayName, monthly_payment = @MonthlyPayment, " +
                     "is_custom = @IsCustom where id = @Id";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@DisplayName", client.DisplayName);
                mc.Parameters.AddWithValue("@MonthlyPayment", client.MonthlyPayment);
                mc.Parameters.AddWithValue("@IsCustom", client.IsCustom);
                mc.Parameters.AddWithValue("@Id", client.Id);
                mc.ExecuteNonQuery();
            }
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

    public void DeleteClient(int id)
    {
        string sql = "delete from clients where id = @Id";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", id);
                mc.ExecuteNonQuery();
            }
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
}