//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data_Layer.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_mst_dropdown
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
    }
}
