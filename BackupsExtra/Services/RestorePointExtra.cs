using System;
using System.Collections.Generic;
using Backups.Services;

namespace BackupsExtra.Services
{
    public class RestorePointExtra
    {
        public RestorePointExtra(string storageMethod, string pathToRestorePoints, BackupJob backupJob)
        {
            RestorePoint = backupJob.CreateRestorePoint();
            Name = RestorePoint.Name;
            FilesInPoint = RestorePoint.FilesInPoint;
            BackupFrom = pathToRestorePoints;
            PathToRestorePoints = pathToRestorePoints + "\\" + Name;
            StorageMethod = storageMethod;
            NumberOfPoint = BackupJob.RestorePointId;
            DateOfCreation = DateTime.Now;
        }

        public string Name { get; }
        public int NumberOfPoint { get; }
        public List<string> FilesInPoint { get; }
        public string BackupFrom { get; }
        public string PathToRestorePoints { get; }
        public string StorageMethod { get; }
        public DateTime DateOfCreation { get; }
        private RestorePoint RestorePoint { get; }

        public string GetInfo()
        {
            return " Restore point " + Name + " created";
        }
    }
}