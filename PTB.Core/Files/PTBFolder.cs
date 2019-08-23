using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.Files
{
    public class PTBFolder
    {
        public string DefaultFileName { get; set; }
        public string Name { get; set; }
        public List<PTBFile> Files { get; set; }

        public PTBFile GetDefault() => Files.First(file => file.FileName == DefaultFileName);
    }
}