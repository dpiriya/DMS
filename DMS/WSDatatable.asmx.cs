using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using DMS;
using Data_Layer.Models;
using Data_Layer.Repository;
using BusinessLayer;
using System.Web.Mvc;

namespace DMS
{
    //[WebService(Namespace = "http://tempuri.org/")]
    // [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WSDatatable : System.Web.Services.WebService
    {
        [WebMethod]

        public void GetData_Office()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<Office_trxModel> lists = dms.tbl_trx_office.Select(m => new Office_trxModel() { office_trx_id = m.office_trx_id, project_no = m.project_no, doc_type = m.doc_type, file_Namee = m.file_Namee, page_count = m.page_count, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_Namee }).ToList();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Context.Response.Write(jss.Serialize(lists));
            }
            //string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            //List<Office_trxModel> lists = new List<Office_trxModel>();
            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_search_office", con);
            //    //cmd.Parameters.Add("@mode", SqlDbType.NVarChar, 20).Value = "LastUpdated";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    //cmd.Parameters.Add(new SqlParameter("@mode", "LastUploaded"));
            //    con.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        Office_trxModel list = new Office_trxModel();
            //        list.office_trx_id = Convert.ToInt32(dr["office_trx_id"]);
            //        list.project_no = dr["project_no"].ToString();
            //        list.doc_type = dr["doc_type"].ToString();                    
            //        list.file_path = dr["file_path"].ToString();
            //        list.file_Namee = dr["file_Namee"].ToString();
            //        list.page_count = Convert.ToInt32(dr["page_count"]);
            //        list.uploadedBy = dr["uploadedBy"].ToString();
            //        list.is_active = Convert.ToBoolean(dr["is_active"]);
            //        list.path = dr["path"].ToString();
            //        lists.Add(list);
            //    }

            //}

        }
        [WebMethod]
        public void GetData_Recruit()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<Recruit_trxModel> lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;

                Context.Response.Write(jss.Serialize(lists));
            }
            //string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            //List<Recruit_trxModel> lists = new List<Recruit_trxModel>();
            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_search_recruit", con);
            //    cmd.Parameters.Add("@mode", SqlDbType.NVarChar, 20).Value = "LastUpdated";
            //    cmd.Parameters.Add("@dept", SqlDbType.NVarChar, 10).Value = "";
            //    cmd.Parameters.Add("@coor", SqlDbType.NVarChar, 50).Value = "";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    con.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        Recruit_trxModel list = new Recruit_trxModel();
            //        list.recruit_trx_id = Convert.ToInt32(dr["recruit_trx_id"]);
            //        list.EmployeeID = dr["EmployeeID"].ToString();
            //        list.EmployeeName = dr["EmployeeName"].ToString();
            //        list.Appoint_mode = dr["Appoint_mode"].ToString();
            //        list.file_name = dr["file_name"].ToString();
            //        list.file_path = dr["file_path"].ToString();
            //        list.page_count = Convert.ToInt32(dr["page_count"]);
            //        list.uploadedBy = dr["uploadedBy"].ToString();
            //        list.is_active = Convert.ToBoolean(dr["is_active"]);
            //        list.path = dr["path"].ToString();
            //        lists.Add(list);
            //    }
            //}
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //jss.MaxJsonLength = Int32.MaxValue;
            ////var result = new ContentResult
            ////{
            ////    Content = jss.Serialize(lists),
            ////    ContentType = "application/json"
            ////};
            //Context.Response.Write(jss.Serialize(lists));
            ////DMS_businessLayer dms_BussinessLayer = new DMS_businessLayer();
            ////string action = "LastUpdated";
            ////List<Recruit_trxModel> list = dms_BussinessLayer.UploadedView(action, "", "").ToList();
            ////JavaScriptSerializer jss = new JavaScriptSerializer();
            ////Context.Response.Write(jss.Serialize(list));
        }
        [WebMethod]
        public void CustomSearch_Recruit(string mode, string dept, string coor)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Recruit_trxModel> lists = new List<Recruit_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_search_recruit", con);
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar, 20).Value = mode;
                cmd.Parameters.Add("@dept", SqlDbType.NVarChar, 10).Value = dept;
                cmd.Parameters.Add("@coor", SqlDbType.NVarChar, 50).Value = coor;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Recruit_trxModel list = new Recruit_trxModel();
                    list.recruit_trx_id = Convert.ToInt32(dr["recruit_trx_id"]);
                    list.EmployeeID = dr["EmployeeID"].ToString();
                    list.EmployeeName = dr["EmployeeName"].ToString();
                    list.Appoint_mode = dr["Appoint_mode"].ToString();
                    list.file_name = dr["file_name"].ToString();
                    list.page_count = Convert.ToInt32(dr["page_count"]);
                    list.uploadedBy = dr["uploadedBy"].ToString();
                    list.is_active = Convert.ToBoolean(dr["is_active"]);
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Context.Response.Write(jss.Serialize(lists));
        }
        [WebMethod]
        public void GetData_Accounts()
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<Accounts_trxModel> lists = dms.tbl_trx_accounts.Select(m => new Accounts_trxModel() { accounts_trx_id = m.accounts_trx_id, voucher_no = m.voucher_no, project_no = m.project_no, file_voucher = m.file_voucher, file_Namee = m.file_Namee, page_count = m.page_count, uploadedBy = m.uploadedBy, is_spon = m.is_spon, path = m.file_path + m.file_Namee }).ToList();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Context.Response.Write(jss.Serialize(lists));
            }
            //string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            //List<Accounts_trxModel> lists = new List<Accounts_trxModel>();
            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_search_accounts", con);               
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    con.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        Accounts_trxModel list = new Accounts_trxModel();
            //        list.accounts_trx_id = Convert.ToInt32(dr["accounts_trx_id"]);
            //        list.voucher_no = dr["voucher_no"].ToString();
            //        list.project_no = dr["project_no"].ToString();
            //        list.file_voucher = dr["file_voucher"].ToString();
            //        list.file_Namee = dr["file_Namee"].ToString();
            //        list.page_count = Convert.ToInt32(dr["page_count"]);
            //        list.uploadedBy = dr["uploadedBy"].ToString();
            //        list.is_active = Convert.ToBoolean(dr["is_active"]);
            //        list.path = dr["path"].ToString();
            //        lists.Add(list);
            //    }
            //}


        }

        [WebMethod]

        public void GetData_Purchase()
        {
            //string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            //List<Purchase_trxModel> lists = new List<Purchase_trxModel>();
            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_search_purchase", con);
            //    //cmd.Parameters.Add("@mode", SqlDbType.NVarChar, 20).Value = "LastUpdated";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    //cmd.Parameters.Add(new SqlParameter("@mode", "LastUploaded"));
            //    con.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        Purchase_trxModel list = new Purchase_trxModel();
            //        list.purchase_trx_id = Convert.ToInt32(dr["purchase_trx_id"]);
            //        list.indent_no = dr["indent_no"].ToString();
            //        list.project_no = dr["project_no"].ToString();
            //        list.project_coordinator = dr["project_coordinator"].ToString();
            //        list.file_path = dr["file_path"].ToString();
            //        list.file_namee = dr["file_namee"].ToString();
            //        list.page_count = Convert.ToInt32(dr["page_count"]);
            //        list.uploadedBy = dr["uploadedBy"].ToString();
            //        list.is_active = Convert.ToBoolean(dr["is_active"]);
            //        list.path = dr["path"].ToString();
            //        lists.Add(list);
            //    }

            //}
            using (DMSEntities dms = new DMSEntities())
            {
                List<Purchase_trxModel> lists = dms.tbl_trx_purchase.Select(m => new Purchase_trxModel() { purchase_trx_id = m.purchase_trx_id, indent_no = m.indent_no, project_no = m.project_no, project_coordinator = m.project_coordinator, file_namee = m.file_namee, page_count = m.page_count, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_namee }).ToList();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Context.Response.Write(jss.Serialize(lists));
            }
        }
        [WebMethod]
        public void CustomSearch_Purchase(string dept, string coor)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Purchase_trxModel> lists = new List<Purchase_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string coor1 = coor.Substring(coor.LastIndexOf('-') + 1);
                SqlCommand cmd = new SqlCommand("sp_search_purchase", con);               
                cmd.Parameters.Add("@dept", SqlDbType.NVarChar, 10).Value = dept;
                cmd.Parameters.Add("@coor", SqlDbType.NVarChar, 50).Value = coor1;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Purchase_trxModel list = new Purchase_trxModel();
                    list.purchase_trx_id = Convert.ToInt32(dr["purchase_trx_id"]);
                    list.indent_no = dr["indent_no"].ToString();
                    list.project_no = dr["project_no"].ToString();
                    list.project_coordinator = dr["project_coordinator"].ToString();
                    list.file_namee = dr["file_namee"].ToString();
                    list.page_count = Convert.ToInt32(dr["page_count"]);
                    list.uploadedBy = dr["uploadedBy"].ToString();
                    list.is_active = Convert.ToBoolean(dr["is_active"]);
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Context.Response.Write(jss.Serialize(lists));
        }
        [WebMethod]
        public void CustomSearch_Accounts(string dept, string coor)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Accounts_trxModel> lists = new List<Accounts_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string coor1 = coor.Substring(coor.LastIndexOf('-') + 1);
                SqlCommand cmd = new SqlCommand("sp_search_accounts", con);
                cmd.Parameters.Add("@dept", SqlDbType.NVarChar, 10).Value = dept;
                cmd.Parameters.Add("@coor", SqlDbType.NVarChar, 50).Value = coor1;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Accounts_trxModel list = new Accounts_trxModel();
                    list.accounts_trx_id = Convert.ToInt32(dr["accounts_trx_id"]);
                    list.voucher_no = dr["voucher_no"].ToString();
                    list.project_no = dr["project_no"].ToString();
                    list.file_voucher = dr["file_voucher"].ToString();
                    list.file_Namee = dr["file_Namee"].ToString();
                    list.page_count = Convert.ToInt32(dr["page_count"]);
                    list.uploadedBy = dr["uploadedBy"].ToString();
                    list.is_spon = Convert.ToBoolean(dr["is_spon"]);
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Context.Response.Write(jss.Serialize(lists));
        }
        [WebMethod]
        public void CustomSearch_Office(string mode, string dept, string coor)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Office_trxModel> lists = new List<Office_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string coor1 = coor.Substring(coor.LastIndexOf('-') + 1);
                SqlCommand cmd = new SqlCommand("sp_search_office", con);
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar, 20).Value = mode;
                cmd.Parameters.Add("@dept", SqlDbType.NVarChar, 10).Value = dept;
                cmd.Parameters.Add("@coor", SqlDbType.NVarChar, 50).Value = coor1;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Office_trxModel list = new Office_trxModel();
                    list.office_trx_id = Convert.ToInt32(dr["office_trx_id"]);
                    list.project_no = dr["project_no"].ToString();
                    list.doc_type = dr["doc_type"].ToString();
                    list.file_Namee = dr["file_Namee"].ToString();                    
                    list.page_count = Convert.ToInt32(dr["page_count"]);
                    list.uploadedBy = dr["uploadedBy"].ToString();
                    list.is_active = Convert.ToBoolean(dr["is_active"]);
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Context.Response.Write(jss.Serialize(lists));
        }
    }
}
