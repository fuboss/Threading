using System;
using System.IO;

namespace Threading
{
    public abstract class FileSystemInfoWrapper
    {
        public virtual string Name
        {
            get { return _info.Name; }
            set { }
        }

        public virtual string CreationTime
        {
            get { return _info.CreationTime.ToShortDateString(); }
            set { }
        }

        public virtual string ModificationTime
        {
            get { return _info.LastWriteTime.ToShortDateString(); }
            set { }
        }

        public virtual string AccessTime
        {
            get { return _info.LastAccessTime.ToShortDateString(); }
            set { }
        }

        public virtual string Attributes
        {
            get { return _info.Attributes.ToString(); }
            set { }
        }

        public virtual string Size
        {
            get { return "NotImplemented"; }
            set { }
        }

        public virtual string Owner
        {
            get { return "NotImplemented"; }
            set { }
        }

        public virtual string Rights
        {
            get { return "NotImplemented"; }
            set { }
        }

        protected FileSystemInfo _info;

        public FileSystemInfoWrapper()
        {
        }
    }
}