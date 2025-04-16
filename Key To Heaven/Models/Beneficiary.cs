public class Beneficiary
{
    public int Beneficiary_Id { get; set; }
    public int Donor_Id { get; set; }
    public int Association_Id { get; set; }
    public string Association_Name { get; set; } 
    public string Beneficiary_First_Name { get; set; }
    public string Beneficiary_Last_Name { get; set; }
    public string Beneficiary_Email { get; set; }
    public string Beneficiary_Phone_Number { get; set; }
    public string Image { get; set; }
    public string Needs { get; set; }
    public DateTime Birth_Date { get; set; }
    public decimal Goal_Amount { get; set; }
    public decimal Raised_Amount { get; set; }
    public string Gender { get; set; }
    public decimal Income { get; set; }
    public int Family_Size { get; set; }
    public string Beneficiary_Address { get; set; }
    public string Beneficiary_Status { get; set; }
    public DateTime Registration_Date { get; set; }
    public string Description { get; set; }
    public string ResultImage { get; set; } 




    // Computed Properties for UI Binding
    public string FullName => $"{Beneficiary_First_Name} {Beneficiary_Last_Name}";

    public double Progress => (double)(Goal_Amount > 0 ? Raised_Amount / Goal_Amount : 0);

    public string ProgressPercentage => $"{(Progress * 100):0}%";

    public string FormattedRaisedAmount => $"${Raised_Amount:N0}";

    public string FormattedGoalAmount => $"${Goal_Amount:N0}";

    public string TimeLeft
    {
        get
        {
            var remainingDays = (Registration_Date.AddMonths(1) - DateTime.Today).Days;
            return remainingDays > 0 ? $"{remainingDays} days left" : "Expired";
        }
    }
}
