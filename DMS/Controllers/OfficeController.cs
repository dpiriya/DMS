using BusinessLayer;
using Data_Layer.Models;
using Data_Layer.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Configuration;
using System.Data.SqlClient;

namespace DMS.Controllers
{
    public class OfficeController : Controller
    {
        DMS_businessLayer dMS_BusinessLayer = new DMS_businessLayer();

        // GET: Office
        public ActionResult OfficeIndex()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dMS_BusinessLayer.Summary("Office"));
                return View(ds);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();
            }
        }
        #region upload partial
        public ActionResult _UploadOffice(string AppMode)
        {
            try
            {
                return PartialView("_UploadOffice", AppMode);
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
                int roleId = Convert.ToInt32(Session["RoleId"]);
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
                        //if (roleId != 1)
                        //{
                        //    path = dMS_BusinessLayer.GetPath(roleId);
                        //}
                        //else
                        //{
                        if (AppMode == "Sponsored")
                            path = @"\\DMS_SAN\dms\Office\Sponsored\";
                        else if (AppMode == "Consultancy")
                            path = @"\\DMS_SAN\dms\Office\Consultancy\";
                        else
                            return Json(new { success = false, title = "Error in selecting the mode.. Redo the process!!" }, JsonRequestBehavior.AllowGet);
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
                                        tblName = dMS_BusinessLayer.Verify_Office(postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')));
                                        if (tblName == null)
                                            return Json(new { success = false, title = "Invalid Fileno!!", message = "File Name :" + postedFile.FileName }, JsonRequestBehavior.AllowGet);
                                        else if (tblName != AppMode)
                                            return Json(new { success = false, title = "Kindly upload this file under " + tblName + " mode" }, JsonRequestBehavior.AllowGet);
                                        else
                                        {
                                            string fileName = Path.GetFileName(postedFile.FileName);
                                            postedFile.SaveAs(path + fileName);
                                            int numberOfPages; string filenamewoext; int iInsertFileUpload; string type;
                                            using (PdfReader pdfReader = new PdfReader(path + fileName))
                                            {
                                                numberOfPages = pdfReader.NumberOfPages;
                                                filenamewoext = Path.GetFileNameWithoutExtension(fileName);
                                                char check = filenamewoext[2];
                                                if (Char.IsLetter(check))
                                                {
                                                    type = "Sponsor";
                                                }
                                                else
                                                {
                                                    type = fileName.Substring(0, 2);
                                                }
                                            }
                                            iInsertFileUpload = dMS_BusinessLayer.FileUpload_office(filenamewoext, type, numberOfPages, path, Session["Username"].ToString());
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

                                                    return Json(new { success = false, title = "Oops", message = "Server Error !!" }, JsonRequestBehavior.AllowGet);
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
                                        return Json(new { success = false, title = "File already Exists", message = "File Name : " + postedFile.FileName }, JsonRequestBehavior.AllowGet);
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
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json(new { success = false, title = "No File Selected" });
                }
            }
            else
            {
                return Json(new { success = false, title = "Session Expired" });
            }
        }
        #endregion

        #region search
        public ActionResult _Search()
        {
            return View();
        }


        public ActionResult ViewDoc(string iID)
        {
            try
            {
                if (iID != null)
                {
                    string document = iID + ".pdf";
                    //string path = @"E:\priya\DMS\Files\Office\";
                    //path += document;             
                    var filestream = new FileStream(document, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(filestream, "application/pdf");
                }
                else
                    return Json("The respective file is not available");
            }
            catch (Exception ex)
            {
                return Json("The respective file is not available. Error details: " + ex.Message);
            }
        }
        public ActionResult Download(string iID)
        {
            try
            {
                string document = iID + ".pdf";
                //string path = @"E:\priya\DMS\Files\Office\";
                //path += document;
                return File(document, "application/pdf", document);

            }
            catch (Exception ex)
            {
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
            list.types = list.TypeList();
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
            var search_office = dMS_BusinessLayer.Search_office(search, sortColumn, sortColumnDir);
            recordsTotal = search_office.Count();
            var data = search_office.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Adv_Load(string mode, string dept, string coor,string yr)
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
            var adv_search_office = dMS_BusinessLayer.Adv_search_office(search, sortColumn, sortColumnDir, mode, dept, coor,yr);

            recordsTotal = adv_search_office.Count();
            var data = adv_search_office.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }
    }
}
