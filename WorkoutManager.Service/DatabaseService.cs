using System.IO;

namespace WorkoutManager.Service
{
    public class DatabaseService
    {
        private readonly string _dbFileName;

        public DatabaseService(string dbFileName)
        {
            _dbFileName = dbFileName;
        }

        public void DropDatabase()
        {
            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
        }
    }
}