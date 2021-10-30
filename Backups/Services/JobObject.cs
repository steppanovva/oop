namespace Backups.Services
{
    public class JobObject
    {
        public JobObject(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}