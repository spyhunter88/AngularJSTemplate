using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class DocumentDTO
    {
        public int DocumentID { get; set; }
        public int ReferenceID { get; set; }
        public string ReferenceName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public string RelativePath { get; set; }
        public DateTime UploadTime { get; set; }
        public int UploadBy { get; set; }

        // Extra
        public String UploadUser { get; set; }
        public string TempName { get; set; }
    }
}
