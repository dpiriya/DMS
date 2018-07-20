using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
   public class Office_trxModel
    {
        public long office_trx_id { get; set; }
        public string project_no { get; set; }
        public string doc_type { get; set; }
        //public string file_path { get; set; }
        public string file_Namee { get; set; }
        public int page_count { get; set; }
        public string uploadedBy { get; set; }
        public bool is_active { get; set; }

        public string path { get; set; }
    }
}
