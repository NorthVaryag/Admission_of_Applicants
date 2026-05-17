using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class TariffsRepository
{
    MySqlConnection connection;

    public TariffsRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<Tariffs> GetAllTariffs()
    {
        List<Tariffs> tariffsList = new List<Tariffs>();
        string sql = "select t.id as Id, t.tariff_name as TariffName, t.cost_per_month as CostMonth\nfrom tariffs t";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    tariffsList.Add(new Tariffs
                    {
                        Id = dr.GetInt32("id"),
                        TariffName = dr.GetString("TariffName"),
                        CostMonth = dr.GetDecimal("CostMonth")
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
        return tariffsList;
    }
}