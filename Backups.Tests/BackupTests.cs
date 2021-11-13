using NUnit.Framework;
using Backups.Services;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupTests
    {
        [Test]
        public void CreateRestorePointsWithSplitStorage_RestorePointsAreCreated()
        {
            // var backupJob = new BackupJob("split", "C:\\GitHub\\steppanovva\\Backups\\RootRepository");
            // var jobObject1 = new JobObject("C:\\GitHub\\steppanovva\\Backups\\RootRepository\\1.txt", "1");
            // var jobObject2 = new JobObject("C:\\GitHub\\steppanovva\\Backups\\RootRepository\\2.txt", "2");
            // var jobObject3 = new JobObject("C:\\GitHub\\steppanovva\\Backups\\RootRepository\\3.txt", "3");
            // backupJob.AddObject(jobObject1);
            // backupJob.AddObject(jobObject2);
            // backupJob.AddObject(jobObject3);
            // RestorePoint restorePoint1 = backupJob.CreateRestorePoint();
            // Assert.IsTrue(backupJob.GetRestorePoints.Exists(x => x.Name == "RestorePoint1") && restorePoint1.FilesInPoint.Count == 3);
            // backupJob.DeleteObject(jobObject1);
            // RestorePoint restorePoint2 = backupJob.CreateRestorePoint();
            // Assert.IsTrue(backupJob.GetRestorePoints.Exists(x => x.Name == "RestorePoint2") && restorePoint2.FilesInPoint.Count == 2);
        }
    }
}