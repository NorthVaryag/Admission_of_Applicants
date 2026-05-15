using System;
using System.Collections.Generic;
using Admission_of_Applicants.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Admission_of_Applicants.DB;

public class EquipmentRepository
{
    MySqlConnection connection;

    public EquipmentRepository(IOptions<DBConnections> connect)
    {
        connection = new MySqlConnection(connect.Value.ConnectionString);
    }
    public List<Equipment> GetAllEquipment()
    {
        List<Equipment> serviceList = new List<Equipment>();
        string sql = "select e.id, dt.name AS DeviceType, e.ram, e.vram, e.storage, e.network_throughput," +
                     " ot.name AS OS, e.device_cost, e2.first_name, e2.last_name \n  " +
                     "                     from equipments e  \n       " +
                     "                join device_types dt ON e.device_type_id = dt.Id \n  " +
                     "                     join os_types ot on e.os_id = ot.id\n  " +
                     "                     join employees e2 on e.employee_id = e2.id";
        try
        {
            connection.Open();
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    serviceList.Add(new Equipment
                    {
                        Id = dr.GetInt32("id"),
                        DeviceType = dr.GetString("DeviceType"),
                        Ram = dr.GetInt32("ram"),
                        Vram = dr.GetInt32("vram"),
                        Storage = dr.GetInt32("storage"),
                        NetworkThroughput = dr.GetInt32("network_throughput"),
                        Os = dr.GetString("OS"),
                        DeviceCost = dr.GetDecimal("device_cost"),
                        EmployeeName = dr.GetString("first_name"),
                        EmployeeLastName = dr.GetString("last_name")
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