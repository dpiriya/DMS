
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class DeanFile_trxModel
    {
        public long deanFile_trx_id { get; set; }
        [Required]
        public string Category { get; set; }
        public string SubCategory { get; set; }
        [Display(Name ="Source of File")]
        public string Source { get; set; }
        [Display(Name ="Received Date")]
        public string ReceivedDt { get; set; }
        public string Remarks { get; set; }
        public int page_count { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string uploadedBy { get; set; }
        public System.DateTime uploadedOn { get; set; }
        public Nullable<bool> is_active { get; set; }
        public string path { get; set; }
    }
}
