public class Association
{
    public int Association_Id { get; set; } // Primary Key
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public String IDMinistry { get; set; } // Changed from string to int for proper database mapping
    public string Description { get; set; }
    public string Image { get; set; }
    public string Password { get; set; }

    // Computed Property to Get Full Name
    public string FullName => $"{FirstName} {LastName}";
}
