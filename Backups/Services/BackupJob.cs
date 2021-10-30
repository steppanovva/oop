using System.Collections.Generic;

namespace Backups.Services
{
    public class BackupJob : IRepository<JobObject>
    {
        private readonly List<JobObject> _jobObjects;

        public BackupJob(string storageMethod, string pathToRestorePoints)
        {
            GetRestorePoints = new List<RestorePoint>();
            _jobObjects = new List<JobObject>();
            StorageMethod = storageMethod;
            PathToRestorePoints = pathToRestorePoints;
        }

        public static int RestorePointId { get; private set; }
        public List<RestorePoint> GetRestorePoints { get; }
        private string PathToRestorePoints { get; }
        private string StorageMethod { get; }

        public RestorePoint CreateRestorePoint()
        {
            RestorePointId++;
            var restorePoint = new RestorePoint(_jobObjects, StorageMethod, PathToRestorePoints);
            GetRestorePoints.Add(restorePoint);
            return restorePoint;
        }

        public List<JobObject> GetContent()
        {
            return _jobObjects;
        }

        public void AddObject(JobObject obj)
        {
            _jobObjects.Add(obj);
        }

        public void DeleteObject(JobObject obj)
        {
            _jobObjects.Remove(obj);
        }
    }
}