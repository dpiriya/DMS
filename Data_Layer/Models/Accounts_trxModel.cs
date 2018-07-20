using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class Accounts_trxModel
    {
        public long accounts_trx_id { get; set; }
        public string voucher_no { get; set; }
        public string project_no { get; set; }
        public string file_voucher { get; set; }
        public string file_path { get; set; }
        public string file_Namee { get; set; }
        public int page_count { get; set; }
        public Nullable<bool> is_spon { get; set; }
        public string uploadedBy { get; set; }       
       

        public string path { get; set; }
    }
}
