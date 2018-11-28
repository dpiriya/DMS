using BusinessLayer;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.UI;
using Data_Layer.Repository;
using Data_Layer.Models;
using System;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Data.Entity.Core.Metadata.Edm;
using PagedList;
using System.Configuration;


namespace DMS.Controllers
{

    public class RecruitmentController : Controller
    {
        DMS_businessLayer dMS_BusinessLayer = new DMS_businessLayer();
        // GET: Recruitment
        public ActionResult Recruit_Index()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dMS_BusinessLayer.Summary("Recruit"));
                return View(ds);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();
            }
        }

        public ActionResult _UploadRecruit(string AppMode)
        {
            try
            {
                return PartialView("_UploadRecruit", AppMode);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();

            }
        }

        [HttpPost]
        public ActionResult UploadFiles(string AppMode)
        {
            if (Session["RoleId"] != null)
            {
                int roleid = Convert.ToInt32(Session["RoleId"]);
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    int iTotalFileCount = Request.Files.Count;
                    int iFileExistsCount = 0;
                    int iFileUploaded = 0;
                    int iFileTypeMismatch_Count = 0;
                    int iFileNotUploaded = 0;


                    List<string> strArray = new List<string>() { };

                    try
                    {
                        string path;
                        //if (roleid != 1)
                        //{
                        //    path = dMS_BusinessLayer.GetPath(roleid);
                        //}
                        //else
                        //{
                        if (AppMode == "Adhoc" || AppMode == "Selection" || AppMode == "Part")
                            path = @"\\Dms_san\dms\Recruitment\Appointment\";
                        else if (AppMode == "Outsource")
                            path = @"\\DMS_SAN\dms\Recruitment\Outsource\";
                        else
                            return Json(new { success = false, title = "Error in selecting the mode.. Redo the process!!" });

                        //}
                        if (!Directory.Exists(path))
                        {
                            return Json(new { success = false, title = "Path Error " });
                        }

                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < iTotalFileCount; i++)
                        {
                            HttpPostedFileBase postedFile = files[i];

                            if (postedFile.ContentType == "application/pdf")
                            {
                                if (!System.IO.File.Exists(path + postedFile.FileName))
                                {
                                    using (DMSEntities dms = new DMSEntities())
                                    {
                                        string tblName;
                                        tblName = dMS_BusinessLayer.Verify_Recruit(postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')));
                                        if (tblName == null)
                                        {
                                            if (iTotalFileCount == 1)
                                                return Json(new { success = false, title = "Invalid Fileno!!", message = "File Name :" + postedFile.FileName });
                                            else
                                            {
                                                strArray.Add(postedFile.FileName + "-Invalid FileNo");
                                                iFileUploaded++;
                                            }
                                        }
                                        else if (tblName != AppMode) {
                                            if (iTotalFileCount == 1)
                                                return Json(new { success = false, title = "Kindly upload this file under " + tblName + " mode" });
                                            else
                                            {
                                                strArray.Add(postedFile.FileName + "-Not Uploaded..Kindly upload this file under " + tblName + " mode");
                                                iFileUploaded++;
                                            }
                                        }
                                        else
                                        {
                                            string fileName = Path.GetFileName(postedFile.FileName);
                                            postedFile.SaveAs(path + fileName);
                                            int numberOfPages; string filenamewoext; int iInsertFileUpload;
                                            using (PdfReader pdfReader = new PdfReader(path + fileName))
                                            {
                                                numberOfPages = pdfReader.NumberOfPages;
                                                filenamewoext = Path.GetFileNameWithoutExtension(fileName);
                                            }
                                            iInsertFileUpload = dMS_BusinessLayer.FileUpload_recruit(filenamewoext, AppMode, numberOfPages, path, Session["Username"].ToString());
                                            if (iInsertFileUpload == 1)
                                            {
                                                if (iTotalFileCount == 1)
                                                    return Json(new { success = true, title = "File Uploaded !!", message = "File Name : " + postedFile.FileName + " - " + numberOfPages + " pages" });
                                                else
                                                {
                                                    strArray.Add(postedFile.FileName + " - Uploaded ");
                                                    iFileUploaded++;
                                                }
                                            }
                                            else
                                            {
                                                if (System.IO.File.Exists(path + postedFile.FileName))
                                                {
                                                    System.IO.File.Delete(path + postedFile.FileName);
                                                }
                                                if (iTotalFileCount == 0)

                                                    return Json(new { success = false, title = "Oops", message = "Server Error !!" });
                                                else
                                                {
                                                    strArray.Add(postedFile.FileName + " - Not Uploaded ");
                                                    iFileNotUploaded++;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (iTotalFileCount == 1)
                                        return Json(new { success = false, title = "File already Exists", message = "File Name : " + postedFile.FileName });
                                    else
                                    {
                                        strArray.Add(postedFile.FileName + " - Already Exists ");
                                        iFileExistsCount++;
                                    }
                                }

                            }
                            else
                            {
                                if (iTotalFileCount == 1)
                                    return Json(new
                                    {
                                        success = false,
                                        title = "File Type is not PDF",
                                        message = "File Name : " + postedFile.FileName
                                    });
                                else
                                {
                                    strArray.Add(postedFile.FileName + " - Non PDF File ");
                                    iFileTypeMismatch_Count++;
                                }
                            }

                        }
                        string output = string.Join(",", strArray);
                        return Json(new
                        {
                            success = true,
                            title = "File Upload Status",
                            message = output
                        });
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message += ex;
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json(new { success = false, title = "No File Selected", message = "Kindly select files to Upload" });
                }
            }
            else
            {
                return Json(new { success = false, title = "Session Expired", message = "Page will be redirected to Login screen" });
            }
        }

        #region search
        public ActionResult _Search()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();
            }
        }

        public ActionResult ViewDoc(string iID)
        {
            try
            {             
                string document = iID + ".pdf";
                //string path = @"E:\priya\DMS\Files\Recruitment\";
                //path += document;               
                if (System.IO.File.Exists(document))
                {
                    var filestream = new FileStream(document, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(filestream, "application/pdf");
                }
                else
                {
                    return Json(new { success = false, data = "File Not Exists in the specified path" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return RedirectToAction("RecruitIndex");
            }
        }

        public ActionResult Download(string iID)
        {
            try
            {
                if (iID != null)
                {
                    string document = iID + ".pdf";
                    //string path = @"E:\priya\DMS\Files\Recruitment\";
                    //path += document;
                    return File(document, "application/pdf", document);
                }
                else
                    return Json("The respective file is not available", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();
            }
        }
        #endregion

        #region Advanced Search
        [Route(Name = "ASearch")]
        public ActionResult _AdvancedSearch()
        {
            DropdownList list = new DropdownList();
            list.Departments = list.DepartmentList();
            list.CoorList = list.CoorNameList();
            list.AppointmentMode = list.Dropdown();
            return PartialView("_AdvancedSearch", list);
        }


        #endregion

        [HttpPost]
        public ActionResult LoadData()
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search_Recruit = dMS_BusinessLayer.Search_recruit(search,sortColumn,sortColumnDir);
            //using (DMSEntities dms = new DMSEntities())
            //{

                //var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                //if (search != "")
                //{
                //    lists = lists.Where(m => m.EmployeeID.ToUpper().Contains(search.ToUpper()) || m.EmployeeName.ToUpper().Contains(search.ToUpper()) || (m.Appoint_mode != null && m.Appoint_mode.ToUpper().Contains(search.ToUpper())) || m.file_name.ToUpper().Contains(search.ToUpper()) || m.uploadedBy.ToUpper().Contains(search.ToUpper())).ToList();
                //}
                ////var lists = dms.tbl_trx_recruitment.Select(m => new Recruit_trxModel() { recruit_trx_id = m.recruit_trx_id, EmployeeID = m.EmployeeID, EmployeeName = m.EmployeeName, Appoint_mode = m.Appoint_mode, page_count = m.page_count, file_name = m.file_name, uploadedBy = m.uploadedBy, is_active = m.is_active, path = m.file_path + m.file_name }).ToList();
                ////sort
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    lists = lists.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                recordsTotal = search_Recruit.Count();
            var data = search_Recruit.ToList();
            if (pageSize!=-1)
            {
                data = search_Recruit.Skip(skip).Take(pageSize).ToList();
            }
                
                JsonResult json= Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;

            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            //var v = (from a in dc.Customers select a);

            //SORT



        }

        [HttpPost]
        public ActionResult Adv_Load(string mode, string dept, string coor)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var adv_search_recruit = dMS_BusinessLayer.Adv_search_recruit(search, sortColumn, sortColumnDir, mode, dept, coor);

                recordsTotal = adv_search_recruit.Count();
            var data = adv_search_recruit.ToList();
            if (pageSize!=-1)
            {
                 data = adv_search_recruit.Skip(skip).Take(pageSize).ToList();
            }
                
                JsonResult json= Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        //#region Exception Handling
        //public override void OnException(ExceptionContext filterContext)
        //{
        //    Exception e = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;
        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error"
        //    };
        //}
        //#endregion
    }
}