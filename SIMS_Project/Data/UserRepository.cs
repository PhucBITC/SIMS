using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Models;
using System.Globalization;

namespace SIMS_Project.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath = "users.csv";

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
    }
}