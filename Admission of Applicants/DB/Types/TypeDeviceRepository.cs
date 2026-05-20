using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models.Types;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class TypeDeviceRepository
{
    MySqlConnection connection;

    public TypeDeviceRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<DeviceType> GetTypeDevices()
    {
        List<DeviceType> deviceTypeList = new List<DeviceType>();
        string sql = "select dt.id as Id, dt.name as DeviceType\nfrom device_types dt ";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    deviceTypeList.Add(new DeviceType
                    {
                        Id = dr.GetInt32("Id"),
                        DeviceTypeName = dr.GetString("DeviceType")
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
        return deviceTypeList;
    }
}