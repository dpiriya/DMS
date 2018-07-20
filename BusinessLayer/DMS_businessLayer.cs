using Data_Layer.Models;
using Data_Layer.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DMS_businessLayer
    {

        public DataTable Summary(string section)
        {

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                if (section == "Recruit")
                    cmd.CommandText = "sp_recruit_summary";
                else if (section == "Office")
                    cmd.CommandText = "sp_office_summary";
                else if (section == "Accounts")
                    cmd.CommandText = "sp_accounts_summary";
                else if (section == "Purchase")
                    cmd.CommandText = "sp_purchase_summary";
                else if(section=="Dean")
                    cmd.CommandText = "sp_all_summary";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }

        public string GetPath(string sroleid)
        {
            using (DMSEntities dms = new DMSEntities())
            {

                string path = (from m in dms.tbl_trx_dean where m.Agreement_No == sroleid select m.file_path+m.file_name).FirstOrDefault()+".pdf";
                //dms.Database.ExecuteSqlCommand(string.Format(@"EXEC sp_getpath @roleid='{0}'", sroleid)).ToString();
                return path;
            }
        }

        public DataTable AppointHome()
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_appointment_summary";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }

        public DataTable OutsourceHome()
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_outsource_summary";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }

        #region search
        public IEnumerable<Recruit_trxModel> UploadedView(string saction, string ssearchfile, string ssearchname)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    List<Recruit_trxModel> dmslist = dms.Database.SqlQuery<Recruit_trxModel>(@"EXEC sp_search_recruit @mode='" + saction + "',@fileno='" + ssearchfile + "',@employeename='" + ssearchname + "'").ToList();
                    return dmslist;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Recruit_trxModel> Search_recruit(string ssearch, string ssort, string ssortdir)
        {
            using (DMSEntities dms = new DMSEntities())
            {

                var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                if (ssearch != "")
                {
                    lists = lists.Where(m => m.EmployeeID.ToUpper().Contains(ssearch.ToUpper()) || m.EmployeeName.ToUpper().Contains(ssearch.ToUpper()) || (m.Appoint_mode != null && m.Appoint_mode.ToUpper().Contains(ssearch.ToUpper())) || m.file_name.ToUpper().Contains(ssearch.ToUpper()) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }

        public IEnumerable<Office_trxModel> Search_office(string ssearch, string ssort, string ssortdir)
        {
            using (DMSEntities dms = new DMSEntities())
            {

                var lists = dms.tbl_trx_office.Select(m => new Office_trxModel() { office_trx_id = m.office_trx_id, project_no = m.project_no, doc_type = m.doc_type, file_Namee = m.file_Namee, page_count = m.page_count, uploadedBy = m.uploadedBy, path = m.file_path + m.file_Namee }).ToList();
                if (ssearch != "")
                {
                    lists = lists.Where(m => m.file_Namee.ToUpper().Contains(ssearch.ToUpper()) || (m.project_no != null && m.project_no.ToUpper().Contains(ssearch.ToUpper())) || (m.doc_type != null && m.doc_type.ToUpper().Contains(ssearch.ToUpper())) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }

        public IEnumerable<Purchase_trxModel> Search_purchase(string ssearch, string ssort, string ssortdir)
        {
            using (DMSEntities dms = new DMSEntities())
            {

                var lists = dms.tbl_trx_purchase.Select(m => new Purchase_trxModel() { purchase_trx_id = m.purchase_trx_id, indent_no = m.indent_no, project_no = m.project_no, project_coordinator = m.project_coordinator, file_namee = m.file_namee, page_count = m.page_count, uploadedBy = m.uploadedBy, path = m.file_path + m.file_namee }).ToList();
                if (ssearch != "")
                {
                    lists = lists.Where(m => m.indent_no.ToUpper().Contains(ssearch.ToUpper()) || (m.project_no != null && m.project_no.ToUpper().Contains(ssearch.ToUpper())) || (m.file_namee != null && m.file_namee.ToUpper().Contains(ssearch.ToUpper())) || (m.project_coordinator != null && m.project_coordinator.ToUpper().Contains(ssearch.ToUpper())) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }

        public IEnumerable<Accounts_trxModel> Search_accounts(string ssearch, string ssort, string ssortdir)
        {
            using (DMSEntities dms = new DMSEntities())
            {

                var lists = dms.tbl_trx_accounts.Select(m => new Accounts_trxModel() { accounts_trx_id = m.accounts_trx_id, voucher_no = m.voucher_no, project_no = m.project_no, file_voucher = m.file_voucher, file_Namee = m.file_Namee, page_count = m.page_count, uploadedBy = m.uploadedBy, is_spon = m.is_spon, path = m.file_path + m.file_Namee }).ToList();
                if (ssearch != "")
                {
                    lists = lists.Where(m => m.file_Namee.ToUpper().Contains(ssearch.ToUpper()) || (m.voucher_no != null && m.voucher_no.ToUpper().Contains(ssearch.ToUpper())) || (m.project_no != null && m.project_no.ToUpper().Contains(ssearch.ToUpper())) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }
        public IEnumerable<Dean_trxModel> Search_dean(string ssearch, string ssort, string ssortdir)
        {
            using (DMSEntities dms = new DMSEntities())
            {
                List<Dean_trxModel> tbldean = new List<Dean_trxModel>();
                //                var lists = dms.tbl_trx_dean.Select(m => new Dean_trxModel() { dean_trx_id = m.dean_trx_id, MOU_No = m.MOU_No, Year = m.Year, Agreement_type = m.Agreement_type, Partner = m.Partner, Title = m.Title, Expiry_date = m.Expiry_date, Followup = m.Followup, projectno = m.projectno, file_name = m.file_name, page_count = m.page_count, uploadedBy = m.uploadedBy, path = m.file_path + m.file_name }).ToList();
                var lists = dms.tbl_trx_dean.ToList();
                foreach (var list in lists)
                {
                    tbldean.Add(new Dean_trxModel()
                    {
                        dean_trx_id = list.dean_trx_id,
                        Agreement_No = list.Agreement_No,
                        Year = list.Year,
                        Agreement_type = list.Agreement_type,
                        Partner = list.Partner,
                        Title = list.Title,
                        Expiry_date = list.Expiry_date == null ? "" : Convert.ToDateTime(list.Expiry_date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Followup = list.Followup,
                        projectno = list.projectno,
                        file_name = list.file_name,
                        page_count = list.page_count,
                        uploadedBy = list.uploadedBy,
                        path = list.file_path + list.file_name
                    });
                }
                if (!string.IsNullOrEmpty(ssearch))
                {
                    tbldean = tbldean.Where(m => m.Agreement_No.ToUpper().Contains(ssearch.ToUpper()) || m.Agreement_type.ToUpper().Contains(ssearch.ToUpper()) || ((!String.IsNullOrEmpty(m.Title)) && m.Title.ToUpper().Contains(ssearch.ToUpper())) || (!String.IsNullOrEmpty(m.projectno) && m.projectno.ToUpper().Contains(ssearch.ToUpper()))).ToList();
                }
                if (!(String.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    tbldean = tbldean.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return tbldean;
            }
        }

        public IEnumerable<Accounts_trxModel> Adv_search_accounts(string ssearch, string ssort, string ssortdir, string type, string coor, string dept)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Accounts_trxModel> lists = new List<Accounts_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string coor1 = coor.Substring(coor.LastIndexOf('-') + 1);
                SqlCommand cmd = new SqlCommand("sp_search_accounts", con);
                cmd.Parameters.Add("@type", SqlDbType.NVarChar, 10).Value = type;
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
                if (!string.IsNullOrEmpty(ssearch))
                {
                    lists = lists.Where(m => m.file_Namee.ToUpper().Contains(ssearch.ToUpper()) || (m.voucher_no != null && m.voucher_no.ToUpper().Contains(ssearch.ToUpper())) || (m.project_no != null && m.project_no.ToUpper().Contains(ssearch.ToUpper())) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }
        public IEnumerable<Purchase_trxModel> Adv_search_purchase(string ssearch, string ssort, string ssortdir, string coor, string dept)
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


                if (ssearch != "")
                {
                    lists = lists.Where(m => m.indent_no.ToUpper().Contains(ssearch.ToUpper()) || (m.project_no != null && m.project_no.ToUpper().Contains(ssearch.ToUpper())) || (m.project_coordinator != null && m.project_coordinator.ToUpper().Contains(ssearch.ToUpper())) || m.file_namee.ToUpper().Contains(ssearch.ToUpper()) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }
        public IEnumerable<Office_trxModel> Adv_search_office(string ssearch, string ssort, string ssortdir, string mode, string dept, string coor)
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
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
                if (ssearch != "")
                {
                    lists = lists.Where(m => m.project_no.ToUpper().Contains(ssearch.ToUpper()) || m.doc_type.ToUpper().Contains(ssearch.ToUpper()) || (m.file_Namee.ToUpper().Contains(ssearch.ToUpper())) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }
        public IEnumerable<Recruit_trxModel> Adv_search_recruit(string ssearch, string ssort, string ssortdir, string mode, string dept, string coor)
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


                if (ssearch != "")
                {
                    lists = lists.Where(m => m.EmployeeID.ToUpper().Contains(ssearch.ToUpper()) || m.EmployeeName.ToUpper().Contains(ssearch.ToUpper()) || (m.Appoint_mode != null && m.Appoint_mode.ToUpper().Contains(ssearch.ToUpper())) || m.file_name.ToUpper().Contains(ssearch.ToUpper()) || m.uploadedBy.ToUpper().Contains(ssearch.ToUpper())).ToList();
                }
                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //sort
                if (!(string.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
            }
            return lists;
        }

        public IEnumerable<Dean_trxModel> Adv_search_dean(string ssearch,string ssort,string ssortdir,string stype,Int16 syear)
        {
            string cs = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
            List<Dean_trxModel> lists = new List<Dean_trxModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_search_dean", con);
                cmd.Parameters.Add("@type", SqlDbType.NVarChar, 10).Value = stype;
                cmd.Parameters.Add("@year", SqlDbType.SmallInt).Value = syear;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Dean_trxModel list = new Dean_trxModel();
                    list.Agreement_No = dr["Agreement_No"].ToString();
                    list.Year = Convert.ToInt16(dr["Year"]);
                    list.Agreement_type = dr["Agreement_type"].ToString();
                    list.Partner = dr["Partner"].ToString();
                    list.Title = dr["Title"].ToString();
                    list.Expiry_date = dr["Expiry_date"].ToString();
                    list.projectno = dr["projectno"].ToString();
                    list.file_name = dr["file_name"].ToString();
                    list.page_count = Convert.ToInt32(dr["page_count"]);
                    list.uploadedBy = dr["uploadedBy"].ToString();                    
                    list.path = dr["path"].ToString();
                    lists.Add(list);
                }
                if(string.IsNullOrEmpty(ssearch))
                {
                    lists=lists.Where(m => m.Agreement_No.ToUpper().Contains(ssearch.ToUpper()) || m.Agreement_type.ToUpper().Contains(ssearch.ToUpper()) || ((!String.IsNullOrEmpty(m.Title)) && m.Title.ToUpper().Contains(ssearch.ToUpper())) || (!String.IsNullOrEmpty(m.projectno) && m.projectno.ToUpper().Contains(ssearch.ToUpper()))).ToList();
                }
                if (!(String.IsNullOrEmpty(ssort) && string.IsNullOrEmpty(ssortdir)))
                {
                    lists = lists.OrderBy(ssort + " " + ssortdir).ToList();
                }
                return lists;
            }
        }

        #endregion

        #region Verify FileName
        public string Verify_Recruit(string sFileno)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    string table = dms.Database.SqlQuery<string>(@"EXEC sp_verify_recruit @fileno='" + sFileno + "'").FirstOrDefault();
                    return table;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string Verify_Office(string sFileno)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    string table = dms.Database.SqlQuery<string>(@"EXEC sp_verify_office @fileno='" + sFileno + "'").FirstOrDefault();
                    return table;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string Verify_Purchase(string sFileno)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    string table = dms.Database.SqlQuery<string>(@"EXEC sp_verify_purchase @fileno='" + sFileno + "'").FirstOrDefault();
                    return table;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region fileupload db insert for appointment recruitment
        public int FileUpload_recruit(string sFileno, string sMode, int iPgcount, string sPath, string sUsername)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    int iFileUpload = dms.Database.ExecuteSqlCommand(string.Format(@"EXEC sp_insert_fileupload @Fileno='{0}', @mode='{1}',@pages='{2}',@path='{3}',@uploadedBy='{4}'", sFileno, sMode, iPgcount, sPath, sUsername));
                    return iFileUpload;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int FileUpload_purchase(string sFileno, int iPgcount, string sPath, string sUsername)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    int iFileUpload = dms.Database.ExecuteSqlCommand(string.Format(@"EXEC sp_fileupload_purchase @Fileno='{0}',@pages={1},@path='{2}',@uploadedBy='{3}'", sFileno, iPgcount, sPath, sUsername));
                    return iFileUpload;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int FileUpload_dean(string smou_no, short iyr, string spartner, string stype, string sprojectno, string stitle,string sfaculty,string sdept,string dtsign, string dtexpiry, bool bfollow, string sFileno, int iPgcount, string sPath, string sUsername)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    tbl_trx_dean Mdean = new tbl_trx_dean()
                    {
                        Agreement_No = smou_no,
                        Year = iyr,
                        Agreement_type = stype,
                        Faculty = sfaculty,
                        DepartmentCode = sdept,
                        Signed_date = dtsign != null ? (DateTime.ParseExact(dtsign, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null,
                        Expiry_date = dtexpiry!=null? (DateTime.ParseExact(dtexpiry,"dd/MM/yyyy",CultureInfo.InvariantCulture)):(DateTime?) null,
                        file_name = smou_no,
                        file_path = sPath,
                        Followup = bfollow,
                        page_count = iPgcount,
                        Partner = spartner,                        
                        projectno = sprojectno,
                        Title = stitle,
                        uploadedBy = sUsername,
                        uploadedOn=DateTime.Now
                    };
                    dms.tbl_trx_dean.Add(Mdean);
                    dms.SaveChanges();
                    return 1;
                }
                
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion



        #region fileupload db insert for Office
        public int FileUpload_office(string sFileno, string sMode, int iPgcount, string sPath, string sUsername)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    int iFileUpload = dms.Database.ExecuteSqlCommand(string.Format(@"EXEC sp_fileupload_office @Fileno='{0}', @mode='{1}' , @pages={2} ,@path='{3}',@uploadedBy='{4}' ", sFileno, sMode, iPgcount, sPath, sUsername));
                    return iFileUpload;
                }
            }
            catch (Exception EX)
            {

                return 0;
            }
        }
        #endregion       

        #region fileupload db insert for Accounts
        public int FileUpload_Accounts(string sFileno, string sVoucherNo, string sMode, int iPgcount, string sPath, string sFilename, string sUsername, bool Is_Spon)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    int iFileUpload = dms.Database.ExecuteSqlCommand(string.Format(@"EXEC sp_Acc_fileupload @Fileno='{0}', @voucher_no='{1}', @mode='{2}',@pages='{3}',@path='{4}',@file_Namee='{5}',@uploadedBy='{6}', @is_spon='{7}'", sFileno, sVoucherNo, sMode, iPgcount, sPath, sFilename, sUsername, Is_Spon));
                    // iFileUpload = 1;
                    return iFileUpload;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion


        #region Check if the project Name is valid
        public DataTable AccProjectName_verification(string sFileno, bool Is_spon)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["DMS"].ConnectionString;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "sp_AccProjectNo_verify";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Connection = con;
                    SqlParameter paramFileName = new SqlParameter();
                    paramFileName.ParameterName = "@Fileno";
                    paramFileName.Value = sFileno;
                    cmd.Parameters.Add(paramFileName);
                    SqlParameter paramIs_spon = new SqlParameter();
                    paramIs_spon.ParameterName = "@is_spon";
                    paramIs_spon.Value = Is_spon;
                    cmd.Parameters.Add(paramIs_spon);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region update dean entry

        public int Update_Dean(Dean_trxModel model)
        {
            try
            {
                using (DMSEntities dms = new DMSEntities())
                {
                    var result = (from m in dms.tbl_trx_dean where m.Agreement_No == model.Agreement_No select m).FirstOrDefault();
                    result.Agreement_type = model.Agreement_type;
                    result.DepartmentCode = model.DepartmentCode;
                    result.Expiry_date = model.Expiry_date != null ? (DateTime.ParseExact(model.Expiry_date, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                    result.Faculty = model.Faculty;
                    result.Partner = model.Partner;
                    result.projectno = model.projectno;
                    result.Signed_date = model.Signed_date != null ? (DateTime.ParseExact(model.Signed_date, "dd/MM/yyyy", CultureInfo.InvariantCulture)) : (DateTime?)null;
                    result.Title = model.Title;
                    dms.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion




    }
}
