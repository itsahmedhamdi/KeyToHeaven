using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using MySql.Data;


namespace KeyToHeaven.Data
{
    public static class DatabaseHelper
    {
        private const string connectionString = "Server=localhost;Port=3306;Database=keytoheaven;User ID=root;Password=Bohmyd123*$@;SslMode=Preferred;";




        // 🔹 Method to Authenticate Donor or Association
        public static async Task<(bool isAuthenticated, string userType, int userId)> AuthenticateUser(string email, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // Check Donor login
                    string queryDonor = "SELECT Donor_Id FROM Donor WHERE Email = @Email AND Password = SHA2(@Password, 512)";
                    using (var cmd = new MySqlCommand(queryDonor, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null)
                        {
                            int donorId = Convert.ToInt32(result);
                            return (true, "Donor", donorId);
                        }
                    }

                    // Check Association login
                    string queryAssoc = "SELECT Association_Id FROM Association WHERE Email = @Email AND Password = SHA2(@Password, 512)";
                    using (var cmd = new MySqlCommand(queryAssoc, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null)
                        {
                            int associationId = Convert.ToInt32(result);
                            return (true, "Association", associationId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication Error: {ex.Message}");
            }

            return (false, string.Empty, 0);
        }



        // 🔹 Method to Register Donor
        public static async Task<bool> RegisterDonor(string firstName, string lastName, string email, string password, string phone)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Donor (FirstName, LastName, Email, Password, Phone) VALUES (@FirstName, @LastName, @Email, SHA2(@Password, 512), @Phone)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }

        // 🔹 Method to Register Association
        public static async Task<bool> RegisterAssociation(string firstName, string lastName, string email, string password, string phone, string idMinistry, string description)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Association (FirstName, LastName, Email, Password, Phone, IDMinistry, Description) VALUES (@FirstName, @LastName, @Email, SHA2(@Password, 512), @Phone, @IDMinistry, @Description)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@IDMinistry", idMinistry);
                        cmd.Parameters.AddWithValue("@Description", description);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<List<Association>> GetAssociations()
        {
            List<Association> associations = new List<Association>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Association", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        associations.Add(new Association
                        {
                            Association_Id = reader.GetInt32("Association_Id"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email"),
                            Phone = reader.GetString("Phone"),
                            IDMinistry = reader.GetString("IDMinistry"),
                            Description = reader.GetString("Description"),
                            Image = reader.IsDBNull("Image") ? "default.jpg" : reader.GetString("Image")
                        });
                    }
                }
            }
            return associations;
        }

        public static async Task<List<Beneficiary>> GetBeneficiaries()
        {
            List<Beneficiary> beneficiaries = new List<Beneficiary>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

               
                string query = @"
        SELECT b.*, a.FirstName AS Association_Name 
        FROM Beneficiary b
        JOIN Association a ON b.Association_Id = a.Association_Id";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        beneficiaries.Add(new Beneficiary
                        {
                            Beneficiary_Id = reader.GetInt32("Beneficiary_Id"),
                            Donor_Id = reader.GetInt32("Donor_Id"),
                            Association_Id = reader.GetInt32("Association_Id"),
                            Association_Name = reader.GetString("Association_Name"),
                            Beneficiary_First_Name = reader.GetString("Beneficiary_First_Name"),
                            Beneficiary_Last_Name = reader.GetString("Beneficiary_Last_Name"),
                            Beneficiary_Email = reader.GetString("Beneficiary_Email"),
                            Beneficiary_Phone_Number = reader.IsDBNull("Beneficiary_Phone_Number") ? "" : reader.GetString("Beneficiary_Phone_Number"),
                            Image = reader.IsDBNull("Image") ? "default.jpg" : reader.GetString("Image"),
                            Needs = reader.GetString("Needs"),
                            Birth_Date = reader.GetDateTime("Birth_Date"),
                            Goal_Amount = reader.GetDecimal("Goal_Amount"),
                            Raised_Amount = reader.GetDecimal("Raised_Amount"),
                            Gender = reader.IsDBNull("Gender") ? "" : reader.GetString("Gender"),
                            Income = reader.GetDecimal("Income"),
                            Family_Size = reader.GetInt32("Family_Size"),
                            Beneficiary_Address = reader.GetString("Beneficiary_Address"),
                            Beneficiary_Status = reader.GetString("Beneficiary_Status"),
                            Registration_Date = reader.GetDateTime("Registration_Date"),
                            Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description") // ✅ Added Description Field
                        });
                    }
                }
            }
            return beneficiaries;
        }



        public static async Task<List<Beneficiary>> FilterBeneficiaries(string needCategory)
        {
            var filteredBeneficiaries = new List<Beneficiary>();
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Beneficiary WHERE Needs = @Needs";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Needs", needCategory);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                filteredBeneficiaries.Add(new Beneficiary
                {
                    Beneficiary_Id = reader.GetInt32("Beneficiary_Id"),
                    Donor_Id = reader.IsDBNull("Donor_Id") ? 0 : reader.GetInt32("Donor_Id"),
                    Association_Id = reader.GetInt32("Association_Id"),
                    Beneficiary_First_Name = reader.GetString("Beneficiary_First_Name"),
                    Beneficiary_Last_Name = reader.GetString("Beneficiary_Last_Name"),
                    Beneficiary_Email = reader.GetString("Beneficiary_Email"),
                    Beneficiary_Phone_Number = reader.IsDBNull("Beneficiary_Phone_Number") ? null : reader.GetString("Beneficiary_Phone_Number"),
                    Image = reader.IsDBNull("Image") ? null : reader.GetString("Image"),
                    Needs = reader.GetString("Needs"),
                    Birth_Date = reader.IsDBNull("Birth_Date") ? DateTime.MinValue : reader.GetDateTime("Birth_Date"),
                    Goal_Amount = reader.IsDBNull("Goal_Amount") ? 0 : reader.GetDecimal("Goal_Amount"),
                    Raised_Amount = reader.IsDBNull("Raised_Amount") ? 0 : reader.GetDecimal("Raised_Amount"),
                    Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
                    Income = reader.IsDBNull("Income") ? 0 : reader.GetDecimal("Income"),
                    Family_Size = reader.IsDBNull("Family_Size") ? 0 : reader.GetInt32("Family_Size"),
                    Beneficiary_Address = reader.IsDBNull("Beneficiary_Address") ? null : reader.GetString("Beneficiary_Address"),
                    Beneficiary_Status = reader.GetString("Beneficiary_Status"),
                    Registration_Date = reader.GetDateTime("Registration_Date")
                });
            }
            return filteredBeneficiaries;
        }
        public static async Task<List<Beneficiary>> GetBeneficiariesByAssociation(int associationId)
        {
            List<Beneficiary> beneficiaries = new List<Beneficiary>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Beneficiary WHERE Association_Id = @AssociationId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssociationId", associationId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            beneficiaries.Add(new Beneficiary
                            {
                                Beneficiary_Id = reader.GetInt32("Beneficiary_Id"),
                                Donor_Id = reader.GetInt32("Donor_Id"),
                                Association_Id = reader.GetInt32("Association_Id"),
                                Beneficiary_First_Name = reader.GetString("Beneficiary_First_Name"),
                                Beneficiary_Last_Name = reader.GetString("Beneficiary_Last_Name"),
                                Beneficiary_Email = reader.GetString("Beneficiary_Email"),
                                Beneficiary_Phone_Number = reader.IsDBNull("Beneficiary_Phone_Number") ? "" : reader.GetString("Beneficiary_Phone_Number"),
                                Image = reader.IsDBNull("Image") ? "default.jpg" : reader.GetString("Image"),
                                Needs = reader.GetString("Needs"),
                                Birth_Date = reader.GetDateTime("Birth_Date"),
                                Goal_Amount = reader.GetDecimal("Goal_Amount"),
                                Raised_Amount = reader.GetDecimal("Raised_Amount"),
                                Gender = reader.IsDBNull("Gender") ? "" : reader.GetString("Gender"),
                                Income = reader.GetDecimal("Income"),
                                Family_Size = reader.GetInt32("Family_Size"),
                                Beneficiary_Address = reader.GetString("Beneficiary_Address"),
                                Beneficiary_Status = reader.GetString("Beneficiary_Status"),
                                Registration_Date = reader.GetDateTime("Registration_Date")
                            });
                        }
                    }
                }
            }
            return beneficiaries;
        }
        public static async Task<List<(int AssociationId, string AssociationName, decimal TotalAmount)>> GetDonationSummary()
        {
            List<(int, string, decimal)> donationSummary = new List<(int, string, decimal)>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = @"
            SELECT a.Association_Id, a.FirstName, SUM(d.Amount) AS TotalAmount
            FROM Donation_Transaction d
            JOIN Association a ON d.Association_Id = a.Association_Id
            GROUP BY a.Association_Id, a.FirstName";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int associationId = reader.GetInt32("Association_Id");
                        string associationName = reader.GetString("FirstName"); // You can change to Full Name if needed
                        decimal totalAmount = reader.IsDBNull("TotalAmount") ? 0 : reader.GetDecimal("TotalAmount");

                        donationSummary.Add((associationId, associationName, totalAmount));
                    }
                }
            }
            return donationSummary;
        }
        public static List<DonationData> GetDonationHistory()
        {
            List<DonationData> donationHistory = new List<DonationData>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Transaction_Date, Amount FROM Donation_Transaction WHERE Transaction_Status = 'completed' ORDER BY Transaction_Date";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    donationHistory.Add(new DonationData
                    {
                        Date = reader.GetDateTime(0),
                        Amount = reader.GetDecimal(1)
                    });
                }
            }
            return donationHistory;
        }

        public static async Task<bool> UpdateAssociation(Association association)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = @"UPDATE Association 
                         SET FirstName = @FirstName, LastName = @LastName, 
                             Phone = @Phone, Description = @Description 
                         WHERE Association_Id = @AssociationId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", association.FirstName);
                    command.Parameters.AddWithValue("@LastName", association.LastName);
                    command.Parameters.AddWithValue("@Phone", association.Phone);
                    command.Parameters.AddWithValue("@Description", association.Description ?? ""); // ✅ Ensures no bool issue
                    command.Parameters.AddWithValue("@AssociationId", association.Association_Id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


        public static async Task<Association> GetAssociationById(int associationId)
        {
            Association association = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Association WHERE Association_Id = @AssociationId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssociationId", associationId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            association = new Association
                            {
                                Association_Id = reader.GetInt32("Association_Id"),
                                FirstName = reader.GetString("FirstName"),
                                LastName = reader.GetString("LastName"),
                                Email = reader.GetString("Email"),
                                Phone = reader.GetString("Phone"),
                                IDMinistry = reader.GetString("IDMinistry"),
                                Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                                Image = reader.IsDBNull("Image") ? "default.jpg" : reader.GetString("Image")
                            };
                        }
                    }
                }
            }
            return association;
        }


        // Update Donor Information
        public static async Task<bool> UpdateDonor(Donor donor)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE Donor 
                             SET FirstName = @FirstName, 
                                 LastName = @LastName, 
                                 Phone = @Phone, 
                                 Email = @Email 
                             WHERE Donor_Id = @DonorId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", donor.FirstName);
                        command.Parameters.AddWithValue("@LastName", donor.LastName);
                        command.Parameters.AddWithValue("@Phone", donor.Phone);
                        command.Parameters.AddWithValue("@Email", donor.Email);
                        command.Parameters.AddWithValue("@DonorId", donor.Donor_Id);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating donor: {ex.Message}");
                return false;
            }
        }


        // Get Donor by ID
        public static async Task<Donor> GetDonorById(int donorId)
        {
            Donor donor = null;

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Donor WHERE Donor_Id = @DonorId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DonorId", donorId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                donor = new Donor
                                {
                                    Donor_Id = reader.GetInt32("Donor_Id"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName"),
                                    Email = reader.GetString("Email"),
                                    Phone = reader.GetString("Phone"),
                                    Image = reader.IsDBNull("Image") ? "default.jpg" : reader.GetString("Image")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving donor: {ex.Message}");
            }

            return donor;
        }





        public class DonationData
            {
                public DateTime Date { get; set; }
                public decimal Amount { get; set; }
            }
        public static async Task AddNewBeneficiaryAsync(string firstName, string lastName, string description, int days, byte[]? imageBytes)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Beneficiaries (Beneficiary_First_Name, Beneficiary_Last_Name, Description) VALUES (@FirstName, @LastName, @Description)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@Description", description);

            await command.ExecuteNonQueryAsync();
        }


    }
}

