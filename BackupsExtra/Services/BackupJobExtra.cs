using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Services
{
    public class BackupJobExtra
    {
        public BackupJobExtra(string storageMethod, string pathToRestorePoints, string wayOfLogging, string pathToLogFile)
        {
            PathToRestorePoints = pathToRestorePoints;
            StorageMethod = storageMethod;
            BackupJob = new BackupJob(storageMethod, pathToRestorePoints);
            WayOfLogging = wayOfLogging;
            CreateLogger(pathToLogFile);
        }

        public List<RestorePointExtra> GetRestorePoints { get; } = new ();
        public int MaxNumberOfPoints { get; set; }
        public DateTime MaxLimitationPeriod { get; set; }
        private int LimitPreferences { get; set; }
        private ILog Logger { get; set; }
        private string WayOfLogging { get; }
        private string PathToRestorePoints { get; }
        private string StorageMethod { get; }
        private BackupJob BackupJob { get; }

        public RestorePointExtra CreateRestorePoint()
        {
            var restorePointExtra = new RestorePointExtra(StorageMethod, PathToRestorePoints, BackupJob);
            GetRestorePoints.Add(restorePointExtra);
            CheckLimits();
            Logger.Write(restorePointExtra.GetInfo());
            return restorePointExtra;
        }

        public void AddObject(JobObject obj)
        {
            BackupJob.AddObject(obj);
            Logger.Write(" " + obj.FileName + " added");
        }

        public void DeleteObject(JobObject obj)
        {
            BackupJob.DeleteObject(obj);
            Logger.Write(" " + obj.FileName + " deleted");
        }

        public void RestoreToDirectory(RestorePointExtra restorePointExtra, string pathToDir)
        {
            if (restorePointExtra.StorageMethod == "single")
                ZipFile.ExtractToDirectory(restorePointExtra.PathToRestorePoints + "\\" + restorePointExtra.Name + "\\" + "storage" + ".zip", pathToDir);

            if (restorePointExtra.StorageMethod == "split")
            {
                foreach (string file in restorePointExtra.FilesInPoint)
                    ZipFile.ExtractToDirectory(PathToRestorePoints + "\\" + "RestorePoint" + restorePointExtra.NumberOfPoint + "\\" + file + "_" + restorePointExtra.NumberOfPoint + ".zip", pathToDir);
            }

            Logger.Write(" Files from restore point " + restorePointExtra.Name + " were extracted to " + pathToDir);
        }

        public void RestoreToTheSameDirectory(RestorePointExtra restorePointExtra)
        {
            RestoreToDirectory(restorePointExtra, restorePointExtra.BackupFrom);
        }

        public void Merge(RestorePointExtra lhs, RestorePointExtra rhs)
        {
            RestorePointExtra newerRestorePoint = lhs.DateOfCreation > rhs.DateOfCreation ? lhs : rhs;
            RestorePointExtra olderRestorePoint = lhs.DateOfCreation < rhs.DateOfCreation ? lhs : rhs;

            var differingFiles = olderRestorePoint.FilesInPoint.Where(file => !newerRestorePoint.FilesInPoint.Contains(file)).ToList();

            foreach (string file in differingFiles)
            {
                string pathFrom = olderRestorePoint.PathToRestorePoints + "\\" + file + "_" +
                                  olderRestorePoint.NumberOfPoint + ".zip";
                string pathTo = newerRestorePoint.PathToRestorePoints + "\\" + file + "_" +
                                newerRestorePoint.NumberOfPoint + ".zip";
                File.Move(pathFrom, pathTo);
                File.Delete(olderRestorePoint.PathToRestorePoints + "\\" + file + "_" + olderRestorePoint.NumberOfPoint);
                olderRestorePoint.FilesInPoint.Remove(file);
                newerRestorePoint.FilesInPoint.Add(file);
            }

            GetRestorePoints.Remove(olderRestorePoint);

            Logger.Write(" Points " + lhs.Name + " and " + rhs.Name + " merged");
        }

        public void SetLimitsPreferences(int maxNumberOfPoints, DateTime maxLimitationPeriod, int modeOfLimitation)
        {
            MaxNumberOfPoints = maxNumberOfPoints;
            MaxLimitationPeriod = maxLimitationPeriod;
            LimitPreferences = modeOfLimitation;
        }

        public void CheckLimits()
        {
            if (LimitPreferences == 1)
                CheckNumberOfPointsLimit();

            if (LimitPreferences == 2)
                CheckCreationDataLimit(GetRestorePoints);

            if (LimitPreferences == 3)
            {
                var list = new List<RestorePointExtra>();
                for (int i = 0; i < GetRestorePoints.Count - MaxNumberOfPoints + 1; i++)
                    list.Add(GetRestorePoints[i]);
                CheckCreationDataLimit(list);
            }
        }

        private void CheckNumberOfPointsLimit()
        {
            if (GetRestorePoints.Count <= MaxNumberOfPoints || MaxNumberOfPoints == 0) return;
            RestorePointExtra item = GetRestorePoints[0];
            GetRestorePoints.Remove(item);
            Directory.Delete(item.PathToRestorePoints, true);
            Logger.Write(" RestorePoint " + item.Name + " has been deleted");
        }

        private void CheckCreationDataLimit(List<RestorePointExtra> points)
        {
            var pointsToRemove = points.Where(x => x.DateOfCreation > MaxLimitationPeriod).ToList();
            if (pointsToRemove.Count == points.Count)
                throw new BackupExtraException("You're about to remove all restore points. Change data preferences");
            foreach (RestorePointExtra x in pointsToRemove)
            {
                GetRestorePoints.Remove(x);
                Directory.Delete(x.PathToRestorePoints, true);
                Logger.Write(" RestorePoint " + x.Name + " has been deleted");
            }
        }

        private void CreateLogger(string path)
        {
            if (WayOfLogging == "console")
                Logger = new ConsoleLogging("yes");
            if (WayOfLogging == "file")
                Logger = new FileLogging("yes", path);
        }
    }
}