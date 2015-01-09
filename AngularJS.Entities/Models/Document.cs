using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AngularJS.Entities.Models
{
    public class Document : Entity
    {
        public Document() { }

        public int DocumentID { get; set; }
        public Int16? ReferenceID { get; set; }
        public string ReferenceName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime UploadTime { get; set; }
        public int UploadBy { get; set; }

    }
}
