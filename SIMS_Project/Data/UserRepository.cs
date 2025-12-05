using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;
using System.IO;

namespace SIMS_Project.Data
{
    public class UserRepository : BaseCsvRepository<User>, IUserRepository
    {
       public UserRepository(string filePath = "users.csv") : base(filePath)
       {
       }

        public User Login(string username, string password)
        {
            if (!File.Exists(_filePath)) return null;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var users = csv.GetRecords<User>().ToList();
                // Tìm người có username và password khớp
                return users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }
        public void AddUser(User newUser)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
            using (var stream = File.Open(_filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecord(newUser);
                csv.NextRecord();
            }
        }

        public bool UsernameExists(string username)
        {
            if (!File.Exists(_filePath)) return false;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var users = csv.GetRecords<User>().ToList();
                return users.Any(u => u.Username == username);
            }
        }
    }
}