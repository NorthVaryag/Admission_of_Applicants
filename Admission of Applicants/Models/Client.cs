namespace Admission_of_Applicants.Models;

public class Client
{
    public int Id { get; set; }
    public string NameType { get; set; }
    public string DisplayName { get; set; }
    public string TariffName { get; set; }
    public int IsCustom { get; set; }
    public decimal MonthlyPayment { get; set; }
}