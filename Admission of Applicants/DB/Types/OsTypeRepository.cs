using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models.Types;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class OsTypeRepository
{
    MySqlConnection connection;

    public OsTypeRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<OsType> GetTypeOs()
    {
        List<OsType> osTypeList = new List<OsType>();
        string sql = "select ot.id as Id, ot.name as OsName\nfrom os_types ot  ";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    osTypeList.Add(new OsType
                    {
                        Id = dr.GetInt32("Id"),
                        OsName = dr.GetString("OsName")
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
        return osTypeList;
    }
}