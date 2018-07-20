using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class Purchase_trxModel
    {
        public long purchase_trx_id { get; set; }
        public string indent_no { get; set; }
        public string project_no { get; set; }
        //public string indent_desc { get; set; }
        public string project_coordinator { get; set; }
        //public string file_path { get; set; }
        public string file_namee { get; set; }
        public Nullable<int> page_count { get; set; }
        public string uploadedBy { get; set; }
        //public Nullable<System.DateTime> uploadedOn { get; set; }
        public bool is_active { get; set; }
       
        public string path { get; set; }
    }
}
