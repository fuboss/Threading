using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Threading
{
    [Serializable]
    public class FileInfoWrapper : FileSystemInfoWrapper
    {
        public FileInfoWrapper(string file)
        {
            try
            {
                _info = new FileInfo(file);
            }
            catch (FileNotFoundException ex)
            {
                
            }
        }

        public FileInfoWrapper() { }

        public override string Owner
        {
            get
            {
                var access = info.GetAccessControl();
                try
                {
                    return access.GetOwner(typeof(NTAccount)).Value;
                }
                catch (IdentityNotMappedException ex)
                {
                    return "Can't determine";
                }
            }
            set { }
        }

        public override string Rights
        {
            get
            {
                var access = info.GetAccessControl();

                var builder = new StringBuilder();
                {
                    foreach (FileSystemAccessRule rule in access.GetAccessRules(true, true, typeof(NTAccount)))
                    {
                        builder.Append(rule.IdentityReference.Value + ":" + rule.FileSystemRights + ",");
                    }
                }
                return builder.ToString();
            }
            set { }
        }

        public override string Size
        {
            get
            {
                var bytes = info.Length;
                if (bytes == 0) return "0MB";
                return string.Format("{0:F}MB", bytes / 1024f / 1024f);
            }
            set { }
        }

        private FileInfo info { get { return (FileInfo) _info; } }
    }
}