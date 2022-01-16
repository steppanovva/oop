using System;
using System.IO;
using Backups.Services;
using Backups.Tools;
using NUnit.Framework;
using BackupsExtra.Services;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class BackupsExtraTests
    {
        // [Test]
        public void CreateRestorePointsWithSplitStorage_RestorePointsAreCreated()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            var jobObject2 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\2.txt", "2");
            var jobObject3 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\3.txt", "3");
            backupJob.AddObject(jobObject1);
            backupJob.AddObject(jobObject2);
            backupJob.AddObject(jobObject3);
            RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();
            Assert.IsTrue(restorePoint1.FilesInPoint.Count == 3);
        }

        // [Test]
        public void MergeRestorePoints_Merged()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            var jobObject2 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\2.txt", "2");
            backupJob.AddObject(jobObject1);
            RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();  
            backupJob.DeleteObject(jobObject1);
            backupJob.AddObject(jobObject2);
            RestorePointExtra restorePoint2 = backupJob.CreateRestorePoint();
            backupJob.Merge(restorePoint1, restorePoint2);
            Assert.IsTrue(restorePoint2.FilesInPoint.Count == 2 && restorePoint1.FilesInPoint.Count == 0);
        }

        // [Test]
        public void AttemptToCreateExistingArchive_ThrowException()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\TestArchive", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            backupJob.AddObject(jobObject1);
            Assert.Catch<BackupException>(() =>
            {
                RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();
            });
        }
        
        // [Test]
        public void LimitReached_RestorePointDeleted()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            var jobObject2 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\2.txt", "2");
            var jobObject3 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\3.txt", "3");
            var jobObject4 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\3.txt", "4");
            backupJob.SetLimitsPreferences(3, DateTime.Today.AddDays(2), 1);
            backupJob.AddObject(jobObject1);
            RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();
            backupJob.AddObject(jobObject2);
            RestorePointExtra restorePoint2 = backupJob.CreateRestorePoint();
            backupJob.AddObject(jobObject3);
            RestorePointExtra restorePoint3 = backupJob.CreateRestorePoint();
            backupJob.AddObject(jobObject4);
            RestorePointExtra restorePoint4 = backupJob.CreateRestorePoint();
            Assert.IsTrue(backupJob.GetRestorePoints.Count == 3);
        }

        // [Test]
        public void AttemptToDeleteAllPoints_ThrowException()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            backupJob.SetLimitsPreferences(3, DateTime.Today.AddDays(-2), 2);
            backupJob.AddObject(jobObject1);
            Assert.Catch<BackupException>(() =>
            {
                RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();
            });
        }
        

        // [Test]
        public void RestoreFromBackUp_Restored()
        {
            var backupJob = new BackupJobExtra("split", "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository", "file",
                "C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\log.txt");
            var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt", "1");
            backupJob.AddObject(jobObject1);
            RestorePointExtra restorePoint1 = backupJob.CreateRestorePoint();  
            File.Delete("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1.txt");
            backupJob.RestoreToTheSameDirectory(restorePoint1);
            Assert.IsTrue(File.Exists("C:\\GitHub\\steppanovva\\BackupsExtra\\RootRepository\\1"));
        }
    }
}