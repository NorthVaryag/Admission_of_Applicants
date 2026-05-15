namespace Admission_of_Applicants.Models;

public class Equipment
{
    public int Id { get; set; }
    public string DeviceType { get; set; }
    public int Ram { get; set; }
    public int Vram { get; set; }
    public int Storage { get; set; }
    public int NetworkThroughput { get; set; }
    public decimal DeviceCost { get; set; }
    public string Os { get; set; }
    public string FullName => $"{EmployeeName} {EmployeeLastName}";
    public string EmployeeName { get; set; }
    public string EmployeeLastName { get; set; }
}