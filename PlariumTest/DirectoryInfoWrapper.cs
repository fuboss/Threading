using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Threading
{
    [XmlRoot(ElementName = "Hierarchy")]
    public class DirectoryInfoWrapper : FileSystemInfoWrapper
    {
        public DirectoryInfoWrapper(string dir)
        {
            _info = new DirectoryInfo(dir);
        }

        public DirectoryInfoWrapper() { }

        [XmlArray, XmlArrayItem("File")]
        public List<FileInfoWrapper> Files = new List<FileInfoWrapper>();
        [XmlArray, XmlArrayItem("Directory")]
        public List<DirectoryInfoWrapper> SubDirectories = new List<DirectoryInfoWrapper>();

        public override string Owner
        {
            get
            {
                var access = info.GetAccessControl();
                try
                {
                    return access.GetOwner(typeof (NTAccount)).Value;
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
                        builder.Append(rule.IdentityReference.Value +":"+ rule.FileSystemRights + ",");
                    }
                }
                return builder.ToString();
            }
            set { }
        }

        public string Serialize()
        {
            string result;
            XmlSerializer xsSubmit = new XmlSerializer(typeof(DirectoryInfoWrapper));
            using (StringWriter sww = new StringWriter())
            using (XmlTextWriter writer = new XmlTextWriter(sww))
            {
                writer.Formatting = Formatting.Indented;
                xsSubmit.Serialize(writer, this);

                result = sww.ToString();
            }

            return result;
        }

        public void RegisterFiles(IEnumerable<FileInfoWrapper> files)
        {
            Files.AddRange(files);
        }

        public void RegisterDir(DirectoryInfoWrapper dir)
        {
            SubDirectories.Add(dir);
        }

        private DirectoryInfo info { get { return (DirectoryInfo)_info; } }
    }
}
