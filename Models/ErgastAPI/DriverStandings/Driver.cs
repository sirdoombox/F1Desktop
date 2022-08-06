namespace F1Desktop.Models.ErgastAPI.DriverStandings;

public class Driver
{
    public string DriverId { get; set; }
    public ushort PermanentNumber { get; set; }
    public string Code { get; set; }
    public string Url { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string DateOfBirth { get; set; }
    public string Nationality { get; set; }
}