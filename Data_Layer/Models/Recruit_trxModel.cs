using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class Recruit_trxModel
    {

        public long recruit_trx_id { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Appoint_mode { get; set; }
        public int page_count { get; set; }
        //public string file_path { get; set; }
        public string file_name { get; set; }
        public string uploadedBy { get; set; }
       
        public Nullable<bool> is_active { get; set; }
        
        public string path { get; set; }
   

    }
}
