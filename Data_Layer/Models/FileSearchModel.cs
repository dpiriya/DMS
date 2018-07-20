using Data_Layer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Data_Layer.Models
{
    public class FileSearchModel
    {
        public Recruit_trxModel Model1 { get; set; }
        public  ListItemModel Model2 { get; set; }
        
        public IEnumerable<Recruit_trxModel> Model3 { get; set; }
    }
}
