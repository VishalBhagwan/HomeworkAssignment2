using System;
using System.Collections.Generic;
using System.Configuration;  
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HomeworkAssignment2.Models
{
    public class RescuePetDataService
    {
        // Get the databse connection string from the configuration file
        private string ConnectionString;
        public RescuePetDataService()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RescuePetConnection"].ConnectionString;
        }

        // Gets all users from the database
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT UserId, FirstName, LastName, PhoneNumber FROM Users";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    // Loops through each row and adds it to the User list
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString()
                        });
                    }
                }
            }
            return users;
        }

        // Gets all pets with filters
        public List<Pet> GetAllPetsByFilters(string type, string breed, string location, string status)
        {
            List<Pet> pets = new List<Pet>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"SELECT p.PetId, p.Name, p.Type, p.Breed, p.Location, p.Age, p.Weight, 
                                      p.Gender, p.PetStory, p.ImageBase64, p.Status, p.PostedByUserId,
                                      u.FirstName + ' ' + u.LastName as PostedByUserName
                               FROM Pets p
                               INNER JOIN Users u ON p.PostedByUserId = u.UserId
                               WHERE 1=1";

                // Build query dynamically based on filters
                if (type != "All")
                    sql += " AND p.Type = @Type";
                if (breed != "All")
                    sql += " AND p.Breed = @Breed";
                if (location != "All")
                    sql += " AND p.Location = @Location";
                if (status != "All")
                    sql += " AND p.Status = @Status";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    if (type != "All")
                        cmd.Parameters.AddWithValue("@Type", type);
                    if (breed != "All")
                        cmd.Parameters.AddWithValue("@Breed", breed);
                    if (location != "All")
                        cmd.Parameters.AddWithValue("@Location", location);
                    if (status != "All")
                        cmd.Parameters.AddWithValue("@Status", status);

                    // Loops through each row and adds it to the Pets list that match the filters
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        pets.Add(new Pet
                        {
                            PetId = Convert.ToInt32(reader["PetId"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Breed = reader["Breed"].ToString(),
                            Location = reader["Location"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Weight = Convert.ToDecimal(reader["Weight"]),
                            Gender = reader["Gender"].ToString(),
                            PetStory = reader["PetStory"].ToString(),
                            ImageBase64 = reader["ImageBase64"] == DBNull.Value ? null : reader["ImageBase64"].ToString(),
                            Status = reader["Status"].ToString(),
                            PostedByUserId = Convert.ToInt32(reader["PostedByUserId"]),
                            PostedByUserName = reader["PostedByUserName"].ToString()
                        });
                    }
                }
            }
            return pets;
        }

        // Get pet info by ID
        public Pet GetPetById(int petId)
        {
            Pet pet = null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = @"SELECT p.PetId, p.Name, p.Type, p.Breed, p.Location, p.Age, p.Weight, 
                                      p.Gender, p.PetStory, p.ImageBase64, p.Status, p.PostedByUserId,
                                      u.FirstName + ' ' + u.LastName as PostedByUserName
                               FROM Pets p
                               INNER JOIN Users u ON p.PostedByUserId = u.UserId
                               WHERE p.PetId = @PetId";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@PetId", petId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        pet = new Pet
                        {
                            PetId = Convert.ToInt32(reader["PetId"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Breed = reader["Breed"].ToString(),
                            Location = reader["Location"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Weight = Convert.ToDecimal(reader["Weight"]),
                            Gender = reader["Gender"].ToString(),
                            PetStory = reader["PetStory"].ToString(),
                            ImageBase64 = reader["ImageBase64"] == DBNull.Value ? null : reader["ImageBase64"].ToString(),
                            Status = reader["Status"].ToString(),
                            PostedByUserId = Convert.ToInt32(reader["PostedByUserId"]),
                            PostedByUserName = reader["PostedByUserName"].ToString()
                        };
                    }
                }
            }
            return pet;
        }

        // Insert new pet into database
        public bool InsertPet(Pet pet)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO Pets (Name, Type, Breed, Location, Age, Weight, Gender, PetStory, ImageBase64, Status, PostedByUserId)
                                   VALUES (@Name, @Type, @Breed, @Location, @Age, @Weight, @Gender, @PetStory, @ImageBase64, @Status, @PostedByUserId)";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", pet.Name);
                        cmd.Parameters.AddWithValue("@Type", pet.Type);
                        cmd.Parameters.AddWithValue("@Breed", pet.Breed);
                        cmd.Parameters.AddWithValue("@Location", pet.Location);
                        cmd.Parameters.AddWithValue("@Age", pet.Age);
                        cmd.Parameters.AddWithValue("@Weight", pet.Weight);
                        cmd.Parameters.AddWithValue("@Gender", pet.Gender);
                        cmd.Parameters.AddWithValue("@PetStory", pet.PetStory);
                        cmd.Parameters.AddWithValue("@ImageBase64", (object)pet.ImageBase64 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", pet.Status);
                        cmd.Parameters.AddWithValue("@PostedByUserId", pet.PostedByUserId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Create adoption
        public bool CreateAdoption(int petId, int adoptedByUserId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Insert new adoption record
                            string adoptionSql = @"INSERT INTO Adoptions (PetId, AdoptedByUserId, AdoptionDate)
                                                   VALUES (@PetId, @AdoptedByUserId, @AdoptionDate)";

                            using (SqlCommand cmd = new SqlCommand(adoptionSql, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@PetId", petId);
                                cmd.Parameters.AddWithValue("@AdoptedByUserId", adoptedByUserId);
                                cmd.Parameters.AddWithValue("@AdoptionDate", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            }

                            // Update pet status
                            string petSql = @"UPDATE Pets SET Status = 'Adopted' WHERE PetId = @PetId";

                            using (SqlCommand cmd = new SqlCommand(petSql, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@PetId", petId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Insert donation
        public bool InsertDonation(int donatedByUserId, decimal amount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO Donations (DonatedByUserId, Amount, DonationDate)
                                   VALUES (@DonatedByUserId, @Amount, @DonationDate)";

                    // Sets its value in the database to the value of the variable
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@DonatedByUserId", donatedByUserId);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@DonationDate", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Calculates total donations
        public decimal GetTotalDonations()
        {
            decimal total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT ISNULL(SUM(Amount), 0) as Total FROM Donations";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }
            return total;
        }

        // Count adopted pets
        public int GetAdoptedPetsCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // Counts all the entries from the Pets table where status is set to adopted
                connection.Open();
                string sql = "SELECT COUNT(*) FROM Pets WHERE Status = 'Adopted'";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        // Gets 10 most recent adoptions
        public List<Adoption> GetRecentAdoptions(int count = 10)
        {
            List<Adoption> adoptions = new List<Adoption>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = @"SELECT TOP (@Count) a.AdoptionId, a.PetId, a.AdoptedByUserId, a.AdoptionDate,
                                      p.Name as PetName,
                                      u.FirstName + ' ' + u.LastName as AdoptedByUserName
                               FROM Adoptions a
                               INNER JOIN Pets p ON a.PetId = p.PetId
                               INNER JOIN Users u ON a.AdoptedByUserId = u.UserId
                               ORDER BY a.AdoptionDate DESC";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Count", count);
                    // Loops through and adds each row to the Adoption list
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        adoptions.Add(new Adoption
                        {
                            AdoptionId = Convert.ToInt32(reader["AdoptionId"]),
                            PetId = Convert.ToInt32(reader["PetId"]),
                            AdoptedByUserId = Convert.ToInt32(reader["AdoptedByUserId"]),
                            AdoptionDate = Convert.ToDateTime(reader["AdoptionDate"]),
                            PetName = reader["PetName"].ToString(),
                            AdoptedByUserName = reader["AdoptedByUserName"].ToString()
                        });
                    }
                }
            }
            return adoptions;
        }

        
        // Get distinct values for filter dropdowns
        private List<string> GetDistinctValues(string table, string column)
        {
            List<string> values = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT DISTINCT {column} FROM {table} WHERE {column} IS NOT NULL ORDER BY {column}";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        values.Add(reader[0].ToString());
                    }
                }
            }
            return values;
        }

        // Adds distinct pet types, breeds and locations that is already in the Pet table to the filter dropdowns
        public List<string> GetDistinctTypes()
        {
            return GetDistinctValues("Pets", "Type");
        }

        public List<string> GetDistinctBreeds()
        {
            return GetDistinctValues("Pets", "Breed");
        }

        public List<string> GetDistinctLocations()
        {
            return GetDistinctValues("Pets", "Location");
        }

    }
}