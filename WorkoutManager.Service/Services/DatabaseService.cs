using System.IO;
using WorkoutManager.Contract;

namespace WorkoutManager.Service.Services
{
    public class DatabaseService
    {
        private readonly DatabaseConfiguration _configuration;

        public DatabaseService(DatabaseConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void DropDatabase()
        {
            var fileName = _configuration.FileName;
            
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}