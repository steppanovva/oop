using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Tools;

namespace Backups.Services
{
    public class RestorePoint
    {
        public RestorePoint(List<JobObject> jobObjects, string storageMethod, string pathToRestorePoints)
        {
            Name = "RestorePoint" + BackupJob.RestorePointId;

            if (storageMethod == "split")
            {
                foreach (JobObject jobObject in jobObjects)
                {
                    if (!Directory.Exists(pathToRestorePoints + "/" + Name))
                        Directory.CreateDirectory(pathToRestorePoints + "/" + Name);

                    if (File.Exists(pathToRestorePoints + "/" + Name + "/" + jobObject.FileName + "_" +
                                    BackupJob.RestorePointId + ".zip"))
                        throw new BackupException("Archive already exists");

                    string pathToZip = pathToRestorePoints + "/" + Name + "/" + jobObject.FileName + "_" +
                                       BackupJob.RestorePointId + ".zip";
                    ZipArchive zipArchive = ZipFile.Open(pathToZip, ZipArchiveMode.Create);
                    zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.FileName);
                    FilesInPoint.Add(jobObject.FileName);
                }
            }

            if (storageMethod == "single")
            {
                string pathToZip = pathToRestorePoints + "/" + Name + "/" + "storage" + ".zip";
                if (!Directory.Exists(pathToRestorePoints + "/" + Name))
                    Directory.CreateDirectory(pathToRestorePoints + "/" + Name);
                if (File.Exists(pathToZip))
                    throw new BackupException("Archive already exists");
                ZipArchive zipArchive = ZipFile.Open(pathToZip, ZipArchiveMode.Create);
                foreach (JobObject jobObject in jobObjects)
                {
                    zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.FileName);
                    FilesInPoint.Add(jobObject.FileName);
                }
            }
        }

        public string Name { get; }
        public List<string> FilesInPoint { get; } = new ();
    }
}