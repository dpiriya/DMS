using Data_Layer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Data_Layer.Models
{
    public class ListItemModel
    {
        public int ListId { get; set; }
        public string ListName { get; set; }
        public string ListItemValue { get; set; }
        public string ListItemText { get; set; }
        public string ListGroup { get; set; }
        public bool is_active { get; set; }
        public string createdBy { get; set; }
        public System.DateTime createdOn { get; set; }
        public string updatedBy { get; set; }
        public Nullable<System.DateTime> updatedOn { get; set; }
        public IList<SelectListItem> Dropdown()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                IList<SelectListItem> list = dms.tbl_mst_dropdown.OrderBy(m => m.ListItemText).Where(M => M.ListName == "Appointment").Select(m => new SelectListItem { Value = m.ListItemValue, Text = m.ListItemText }).ToList(); //(from listdb in tbl_mst_dropdown where ListName == 'Recruitment' orderby ListItemText);
                return list;
            }
        }
        public IEnumerable<SelectListItem> AppointmentMode { get; set; }

        //public virtual IEnumerable<Recruit_trxModel> rec_model { get; set; }

    }
}
