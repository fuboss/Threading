using System.Collections.Generic;
using System.Linq;

namespace Threading
{
    public class ModelHolder
    {
        public ModelHolder(DirectoryInfoWrapper hierarchy)
        {
            _hierarchy = hierarchy;
        }

        public DirectoryInfoWrapper Hierarchy
        {
            get { return _hierarchy; }
        }

        private DirectoryInfoWrapper _hierarchy;

    }
}
