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

    // Save user to database with hashed password and salt
    public bool SaveNewUser(string firstName, string lastName, string username, string password, string email)
    {


        // Create password hash and salt
        var (hashedSaltPassword, salt) = CreatePasswordHash(password);

        using (var connection = new MySql.Data.MySqlClient.MySqlConnection(CONNECTION_STRING))
        {
            connection.Open();

            // SQL command with parameterized query to prevent SQL injection
            string sql = @"INSERT INTO webapp.new_tableuserregistration (username,firstname,lastname,password, password_hash, salt, email) 
                             VALUES (@UserName,@FirstName,@LastName,@Password, @password_Hash, @salt, @Email)";

            using (var cmd = new MySqlCommand(sql, connection))
            {
                // Use parameters to securely add user input
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@password_Hash", hashedSaltPassword);

                cmd.ExecuteNonQuery();

            }
            return true;
        }
    }

    // Verify password during login


        public VerificationResult VerifyHashPassword(string username, string password)
        {
            using (var connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                // Get stored hash, salt, and user's full name
                string sql = "SELECT password_hash, salt, firstname, lastname FROM new_tableuserregistration WHERE username = @username";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return new VerificationResult
                            {
                                IsValid = false,
                                Message = "User not found"
                            };
                        }

                        string storedHash = reader.GetString(0);
                        string storedSalt = reader.GetString(1);
                        string firstName = reader.GetString(2);
                        string lastName = reader.GetString(3);

                        // Convert stored salt from base64
                        byte[] saltBytes = Convert.FromBase64String(storedSalt);
                        // Hash the input password with stored salt
                        byte[] hashBytes = HashPassword(password, saltBytes);
                        string hashedInput = Convert.ToBase64String(hashBytes);

                        // Compare hashes
                        bool isValid = storedHash.Equals(hashedInput);

                    if (isValid)
                    {
                        return new VerificationResult
                        {
                            IsValid = isValid,
                            Message = $"{firstName} {lastName}"
                        };
                    }
                    else
                    {
                        return new VerificationResult
                        {
                            IsValid = isValid,
                            Message =  "Invalid password"
                        };
                    }
                      
                    }
                }
            }
        }
    }


