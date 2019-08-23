using System.Collections.Generic;
using System.Linq;

namespace PTB.Core.FolderAccess
{
    public class PTBFolder<T> where T: BasePTBFile, new()
    {
        public string DefaultFileName { get; set; }
        public string Name { get; set; }
        public List<T> Files { get; set; }

        public T GetDefaultFile() => Files.First(file => System.IO.Path.GetFileNameWithoutExtension(file.FileName) == DefaultFileName);
    }
}