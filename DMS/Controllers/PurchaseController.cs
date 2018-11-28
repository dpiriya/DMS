using BusinessLayer;
using Data_Layer.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text.pdf;
using Data_Layer.Models;
using System.Linq.Dynamic;
using System.Configuration;
using System.Data.SqlClient;

namespace DMS.Controllers
{
    public class PurchaseController : Controller
    {
        DMS_businessLayer dMS_BusinessLayer = new DMS_businessLayer();
        public ActionResult PurchaseIndex()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dMS_BusinessLayer.Summary("Purchase"));
                return View(ds);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();
            }
        }
        public ActionResult _UploadPurchase()
        {
            try
            {
                return PartialView("_UploadPurchase");
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();

            }
        }
        [HttpPost]
        public ActionResult UploadFiles()
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
                        path = @"\\DMS_SAN\dms\Purchase\";
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
                                        tblName = dMS_BusinessLayer.Verify_Purchase(postedFile.FileName.Substring(0, postedFile.FileName.LastIndexOf('.')));
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
                                            iInsertFileUpload = dMS_BusinessLayer.FileUpload_purchase(filenamewoext, numberOfPages, path, Session["Username"].ToString());
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
            return View();
        }


        public ActionResult ViewDoc(string iID)
        {
            try
            {
                if (iID != "null")
                {
                    string document = iID + ".pdf";
                    // string path = @"E:\priya\DMS\Files\Purchase\";
                    //path += document;
                    var filestream = new FileStream(document, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(filestream, "application/pdf");
                }
                else
                    return Json("The respective file is not available", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult Download(string iID)
        {
            try
            {
                if (iID != "null")
                {
                    string document = iID + ".pdf";
                    //string path = @"E:\priya\DMS\Files\Purchase\";
                    //path += document;
                    return File(document, "application/pdf", document);
                }
                else
                    return Json("The respective file is not available", JsonRequestBehavior.AllowGet);

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
            //list.AppointmentMode = list.Dropdown();
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
            var search_purchase = dMS_BusinessLayer.Search_purchase(search, sortColumn, sortColumnDir);
            recordsTotal = search_purchase.Count();
            var data= search_purchase.ToList(); 
            if (pageSize != -1)
            {
                 data = search_purchase.Skip(skip).Take(pageSize).ToList();
            }  
            JsonResult json= Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;

        }
        [HttpPost]
        public ActionResult Adv_Load(string dept, string coor,string year)
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
            var adv_search_purchase = dMS_BusinessLayer.Adv_search_purchase(search, sortColumn, sortColumnDir, dept, coor,year);

                recordsTotal = adv_search_purchase.Count();
                var data = adv_search_purchase.ToList();
           
            if (pageSize != -1)
            {
                data = adv_search_purchase.Skip(skip).Take(pageSize).ToList();
            }
            JsonResult json=Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
            

        }
    }
}
