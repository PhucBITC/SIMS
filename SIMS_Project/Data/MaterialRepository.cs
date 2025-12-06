using CsvHelper;
using CsvHelper.Configuration;
using SIMS_Project.Interface;
using SIMS_Project.Models;
using System.Globalization;

namespace SIMS_Project.Data
{
    public class MaterialRepository : BaseCsvRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(string filePath = "materials.csv") : base(filePath) { }

        public void Add(Material material)
        {
            var list = GetAll();
            material.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
            list.Add(material);
            WriteFile(list);
        }

        public List<Material> GetByCourse(int courseId)
        {
            return GetAll().Where(m => m.CourseId == courseId).ToList();
        }

        private List<Material> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Material>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, MissingFieldFound = null };
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Material>().ToList();
            }
        }

        private void WriteFile(List<Material> list)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }
    }
}