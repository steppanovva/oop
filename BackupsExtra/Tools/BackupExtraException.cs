using Backups.Tools;

namespace BackupsExtra.Tools
{
    public class BackupExtraException : BackupException
    {
        public BackupExtraException() { }

        public BackupExtraException(string message)
            : base(message) { }
    }
}