using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileClip
{
    public class FileData
    {
        public long Id { get; set; }
        public Guid UId { get; set; }
        public string OriginalName { get; set; }
        public string StoredName { get; set; }
        public decimal Version { get; set; }
        public string Extension { get; set; }
        public Stream FileStream { get; set; }
    }
}
