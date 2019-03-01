using Data_Layer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;


namespace Data_Layer.Models
{
    public class MailModel
    {
        public string ProposalNo { get; set; }

        public string ProjectNo { get; set; }
        public IEnumerable<SelectListItem> ProposalList { get; set; }
        public List<SelectListItem> Proposals()
        {
            using (FoxOfficeEntities ofc = new FoxOfficeEntities())
            {
                List<SelectListItem> pros = ofc.PROPOSAL.Select(m => new SelectListItem { Text = SqlFunctions.StringConvert((double)m.PRONO), Value = SqlFunctions.StringConvert((double)m.PRONO) }).Distinct().ToList();
                return pros;
            }
        }
        [Display(Name ="Institute ID")]
        public string InstiId { get; set; }
        public string ProposalTitle { get; set; }
        public IEnumerable<SelectListItem> IdList { get; set; }
        public List<SelectListItem> IDS()
        {
            using (FacultyViewEntities fac = new FacultyViewEntities())
            {
                List<SelectListItem> id = fac.VW_AppDev_FacultyDetails.Select(m => new SelectListItem { Text = m.EMPLOYEEID, Value = m.EMPLOYEEID }).ToList();
                return id;
            }
        }
        public List<Dean_trxModel> Dean_trxList { get; set; }

        //public string FromName { get; set; }
        //public string FromEmail { get; set; }
        //public string Message { get; set; }
        //public string SNo { get; set; }
        //public string Partner { get; set; }
        //public string Agreement_type { get; set; }

        //public string Signed_date { get; set; }
        //public string Expiry_date { get; set; }
        //public string Title { get; set; }

        public Faculty FMail { get; set; }

        //public static implicit operator MailModel(tbl_trx_dean dean)
        //{
        //    return new MailModel
        //    {
        //        Agreement_type = dean.Agreement_type,
        //        Partner = dean.Partner,
        //        Signed_date = (dean.Signed_date == null) ? "" : Convert.ToDateTime(dean.Signed_date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        Expiry_date = dean.Expiry_date == null ? "" : Convert.ToDateTime(dean.Expiry_date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        Title = dean.Title
        //    };

        //}
        public MailModel()
        {
            Dean_trxList = new List<Dean_trxModel>();
        }

    }
}
