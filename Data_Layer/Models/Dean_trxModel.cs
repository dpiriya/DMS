using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data_Layer.Repository;

namespace Data_Layer.Models
{
    public class Dean_trxModel
    {
        public long dean_trx_id { get; set; }
        [Required]    
        [RegularExpression(@"^[0-9]{2,}_[0-9]{4}",ErrorMessage ="Eg:01_2015")]
        [Display(Name = "Agreement No")]
        public string Agreement_No { get; set; }
        [Required]
        [Range(1000,3000,ErrorMessage ="Must be a valid year")]
        public short Year { get; set; }
        [Required]
        public string Partner { get; set; }  
        [Required]
        [Display(Name ="Agreement Type")]
        public string Agreement_type { get; set; }
        public string Title { get; set; }
        [Display(Name ="Faculty Coordinator")]
        public string Faculty { get; set; }
        public string DepartmentCode { get; set; }
        [Display(Name ="Signed Date")]
        public string Signed_date { get; set; }
        [Display(Name ="Expiry Date")]
        public string Expiry_date { get; set; }        
        public bool Followup { get; set; }
        [Display(Name ="FacultyID")]
        [RegularExpression(@"[0-9]{4}",ErrorMessage ="Enter Valid Faculty ID")]
        public Nullable<int> FacultyID { get; set; }
        public int page_count { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string uploadedBy { get; set; }
        public Nullable<System.DateTime> uploadedOn { get; set; }
        public Nullable<bool> is_active { get; set; }
        public string path { get; set; }        
        public string Appmode { get; set; }       
        public IEnumerable<SelectListItem> AgreementtypeList { get; set; }
        public List<SelectListItem> agreementList()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<SelectListItem> alist = dms.tbl_mst_dropdown.Where(m=>m.ListGroup=="Dean" && m.ListName=="Agreement").Select(m => new SelectListItem { Text = m.ListItemText, Value = m.ListItemValue }).ToList();
                return alist;
            }
        }
        //public List<SelectListItem> Followuplist()
        //{
        //    List<SelectListItem> fList = new List<SelectListItem>();
        //    fList = new[]
        //    {
        //        new SelectListItem {Value="1", Text="Yes"},
        //        new SelectListItem {Value="0", Text="No"}
        //    }.ToList();
        //    return fList;
        //}
    }
}
