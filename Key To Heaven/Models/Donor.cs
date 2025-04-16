public class Donor
{
    public int Donor_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Image { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
