using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models.Types;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class ClientTypeRepository
{
    MySqlConnection connection;

    public ClientTypeRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<ClientType> GetClientTypes()
    {
        List<ClientType> clientTypeList = new List<ClientType>();
        string sql = "select ct.id as Id, ct.name_type as NameType\nfrom client_types ct ";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    clientTypeList.Add(new ClientType
                    {
                        Id = dr.GetInt32("Id"),
                        NameType = dr.GetString("NameType")
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
        return clientTypeList;
    }
}