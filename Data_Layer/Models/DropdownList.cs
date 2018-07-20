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
    public class DropdownList
    {
       
        [Display(Name = "Designation Code")]
        public string DesignationCode { get; set; }

        [Display(Name = "Project Number")]
        public string ProjectNo { get; set; }

        [Display(Name = "Voucher Number")]
        public string VoucherNo { get; set; }

        [Display(Name = "Coordinator Code")]
        public string PICode { get; set; }

        [Display(Name = "Coordinator Name")]
        public string PIName { get; set; }
        public IEnumerable<SelectListItem> CoorList { get; set; }
       
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        public string doc_type { get; set; }

        public IEnumerable<SelectListItem> types { get; set; }

        public string spon_con { get; set; }
        public IEnumerable<SelectListItem> spon_con_List { get; set; }
        public string Mode { get; set; }
        public IEnumerable<SelectListItem> AppointmentMode { get; set; }

        public string AgreementType { get; set; }

        public IEnumerable<SelectListItem> AType { get; set; }

        public List<SelectListItem> AtypeList()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<SelectListItem> alist = dms.tbl_mst_dropdown.Where(m => m.ListName == "Agreement" && m.ListGroup == "Dean").Select(em => new SelectListItem { Value = em.ListItemValue, Text = em.ListItemText }).ToList();
                return alist;
            }
        }

        public List<SelectListItem> DesignationList()
        {
            using (RecruitEntities Recruit = new RecruitEntities())
            {
                List<SelectListItem> dList = Recruit.OutSourcingDesignations.Select(em => new SelectListItem { Value = em.DesignationCode, Text = em.DesignationCode + " -- " + em.DesignationName }).ToList();
                return dList;
            }
        }
        public List<SelectListItem> TypeList()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<SelectListItem> dlist = dms.tbl_trx_office.Select(em => new SelectListItem { Value = em.doc_type, Text = em.doc_type }).Distinct().ToList();
                return dlist;
            }
        }
        public List<SelectListItem> sponconlist()
        {
            List<SelectListItem> gList = new List<SelectListItem>();
            gList = new[]
            {
                new SelectListItem {Value="Sponsored", Text="Sponsored"},
                new SelectListItem {Value="Consultancy", Text="Consultancy"}
            }.ToList();
            return gList;
        }

        public List<SelectListItem> DepartmentList()
        {
            using (RecruitEntities Recruit = new RecruitEntities())
            {
                List<SelectListItem> dList = Recruit.Department().Select(em => new SelectListItem { Value = em.DepartmentCode, Text = em.DepartmentCode + "--" + em.DepartmentName }).ToList();
                return dList;
            }
        }

        public List<SelectListItem> CoorNameList()
        {
            using (RecruitEntities recruit = new RecruitEntities())
            {
                //modified by priya
                //var apl = recruit.AppointmentProjects.Select(a => new SelectListItem() { Text = a.PICode + "-" + a.PIName, Value = a.PICode }).ToList();
                //return apl;
                var apl = recruit.AppointmentProjects.GroupBy(em => new { em.PICode, em.PIName }).OrderBy(g => g.Key).Select(g => g.Key).ToList();
                return apl.Select(a => new SelectListItem() { Text = a.PICode + "-" + a.PIName, Value = a.PICode }).ToList();
                //original
                //List<string> apl = recruit.AppointmentProjects.GroupBy(em => em.PICode).OrderBy(g=>g.Key).Select(g => g.Key).ToList();
                //List<SelectListItem> cl = new List<SelectListItem>();
                //modified by priya
                //apl.ForEach(delegate (string piCode)
                //{
                //    cl.Add(recruit.Coordinator(piCode).Select(em => new SelectListItem { Text = em.CoorCode + " - " + em.CoorName, Value = em.CoorCode }).FirstOrDefault());
                //});

                return null; 
            }
        }
        public List<SelectListItem> Dropdown()
        {
            using (DMSEntities dms = new DMSEntities())
            {
               List<SelectListItem> list = dms.tbl_mst_dropdown.OrderBy(m => m.ListItemText).Where(M => M.ListName == "Appointment").Select(m => new SelectListItem { Value = m.ListItemValue, Text = m.ListItemText }).ToList(); //(from listdb in tbl_mst_dropdown where ListName == 'Recruitment' orderby ListItemText);
                return list;
            }
        }


    }
}
