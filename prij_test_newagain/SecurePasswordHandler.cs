using System;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;



public class SecurePasswordHandler
{
    // Constants for password hashing
    private const int SALT_SIZE = 4;
    private const int HASH_SIZE = 16;
    private const int ITERATIONS = 0;// more iteretions- more difficult to bridge

    // Database connection string - replace with your actual connection details
    private string CONNECTION_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

    // Class to store password verification result
    public class VerificationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    // Generate a new random salt
    private byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);
            return salt;
        }
    }

    // Hash password using HMAC-SHA256 with salt
    private byte[] HashPassword(string password, byte[] salt)
    {
        using (var hmac = new HMACSHA256(salt))
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = passwordBytes;

            // Perform multiple iterations of hashing
            for (int i = 0; i < ITERATIONS; i++)
            {
                hash = hmac.ComputeHash(hash);
            }

            return hash;
        }
    }

    // Create password hash with salt for new user registration
    public (string HashedPassword, string Salt) CreatePasswordHash(string password)
    {
        // Generate new salt
        byte[] salt = GenerateSalt();

        // Hash password with salt
        byte[] hashBytes = HashPassword(password, salt);

        // Convert to base64 for storage
        string hashedPassword = Convert.ToBase64String(hashBytes);
        string saltString = Convert.ToBase64String(salt);

        return (hashedPassword, saltString);
    }



   public bool VerifyHashPassword(string username, string password)
    {
        // Retrieve connection string from web.config
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

        using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
        {
            try
            {
                conn.Open();

                // SQL to retrieve password hash and salt for the given username
                string sql = "SELECT password_hash, salt FROM new_tableuserregistration WHERE username = @username";

                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@username", username);

                    // Execute the query and check if user exists
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the stored hash and salt
                            string storedHash = !reader.IsDBNull(0) ? reader.GetString(0) : null;
                            string storedSalt = !reader.IsDBNull(1) ? reader.GetString(1) : null;

                            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
                            {
                                // Log the issue if stored hash or salt is missing
                                return false;
                            }

                            // Convert the stored salt from Base64 to byte array
                            byte[] saltBytes = Convert.FromBase64String(storedSalt);

                            // Hash the entered password with the retrieved salt
                            byte[] hashBytes = HashPassword(password, saltBytes);
                            string hashedInput = Convert.ToBase64String(hashBytes);

                            // Compare the entered password hash with the stored hash
                            if (storedHash.Equals(hashedInput))
                            {
                                // If the hashes match, the password is correct
                                return true;
                            }
                            else
                            {
                                // If the hashes don't match, the password is invalid
                                return false;
                            }
                        }
                        else
                        {
                            // No user found with the given username
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exceptions for debugging
                Console.WriteLine($"Error during password verification for user {username}: {ex.Message}");
                return false;
            }
        }
    }

    public bool IsPasswordInHistory(string email, string password, int history_num)
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

        using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
        {
            try
            {
                conn.Open();

                // SQL to retrieve the current password hash and salts
                string sql = @"SELECT Email, password_hash, salt FROM webapp.new_user_hash_salt_data WHERE Email = @Email ORDER BY Email DESC LIMIT @history_num";

                using (var command = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@HistoryNum", history_num);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read the password hash and salt from each row
                            string storedHash = reader.GetString(0);
                            string storedSalt = reader.GetString(1);

                            // Check if the entered password matches the stored hash and salt
                            if (IsPasswordMatch(password, storedHash, storedSalt))
                            {
                                return true; // Password is in history
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        return false; // No match found 
    }

    private bool IsPasswordMatch(string password, string storedHash, string storedSalt)
    {
        if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
        {
            return false;
        }

        // Convert the stored salt from Base64 to byte array
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        // Hash the entered password with the stored salt
        byte[] hashBytes = HashPassword(password, saltBytes);
        string hashedInput = Convert.ToBase64String(hashBytes);

        // Compare the entered password hash with the stored hash
        return storedHash.Equals(hashedInput);
    }
}




