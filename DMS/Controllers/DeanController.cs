using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data_Layer.Repository;
using Data_Layer.Models;
using System.Data;
using BusinessLayer;
using System.IO;
using iTextSharp.text.pdf;

namespace DMS.Controllers
{
    public class DeanController : Controller
    {
        DMS_businessLayer dMS_BusinessLayer = new DMS_businessLayer();
        // GET: Dean
        public ActionResult Dean_Index()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dMS_BusinessLayer.Summary("Dean"));
                return View(ds);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;

                return View();
            }
        }
              
        public ActionResult _UploadDean(string AppMode)
        {
            try
            {
                Dean_trxModel deanModel = new Dean_trxModel();                
                deanModel.AgreementtypeList = deanModel.agreementList();
                deanModel.Appmode = AppMode;
                return PartialView("_UploadDean", deanModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return View();

            }
        }
        [HttpPost]
        public ActionResult UploadFiles(Dean_trxModel deanModel)
        {
            if (ModelState.IsValid)
            {
                if (Session["RoleId"] != null)
                {
                    int roleId = Convert.ToInt32(Session["RoleId"]);
                    if (Request.Files.Count > 0)
                    {
                        int iFileExistsCount = 0;
                        int iFileUploaded = 0;
                        int iFileTypeMismatch_Count = 0;
                        int iFileNotUploaded = 0;
                        try
                        {
                            string path;
                            if (deanModel.Appmode == "Agreement")
                                path = @"\\10.18.0.29\dms\DeanOffice\Agreement\";
                            else
                                return Json(new { success = false, title = "Mode error",message="Error in selecting the mode..Redo the process!!" });
                            if (!Directory.Exists(path))
                            {
                                return Json(new { success = false, title = "Path Error ",message="Network path error" });
                            }
                            var file = Request.Files[0];
                            if (file != null && file.ContentLength > 0)
                            {
                                if (file.ContentType == "application/pdf")
                                {
                                    if (!System.IO.File.Exists(path + deanModel.Agreement_No+".pdf"))
                                    {
                                        using (DMSEntities dms = new DMSEntities())
                                        {
                                            //string fileName = Path.GetFileName(file.FileName);
                                            string fileName = deanModel.Agreement_No + ".pdf";
                                            file.SaveAs(path + fileName);
                                            int numberOfPages; string filenamewoext; int iInsertFileUpload;
                                            using (PdfReader pdfReader = new PdfReader(path + fileName))
                                            {
                                                numberOfPages = pdfReader.NumberOfPages;
                                                filenamewoext = Path.GetFileNameWithoutExtension(fileName);
                                            }
                                            iInsertFileUpload = dMS_BusinessLayer.FileUpload_dean(deanModel.Agreement_No, deanModel.Year, deanModel.Partner, deanModel.Agreement_type, deanModel.FacultyID, deanModel.Title,deanModel.Faculty,deanModel.DepartmentCode,deanModel.Signed_date, deanModel.Expiry_date, deanModel.Followup, filenamewoext, numberOfPages, path, Session["Username"].ToString());
                                            if (iInsertFileUpload == 1)
                                                return Json(new { success = true, title = "File Upload Status", message = "File Name : " + fileName + " - " + numberOfPages + " pages" });
                                            else
                                            {
                                                if (System.IO.File.Exists(path + fileName))
                                                {
                                                    System.IO.File.Delete(path + fileName);
                                                    return Json(new { success = false, title = "File Upload Status", message = fileName + "Not Uploaded" });
                                                }
                                                else
                                                    return Json(new { success = false, title = "Oops", message = "Server Error !!" });
                                            }
                                        }
                                    }
                                    else { return Json(new { success = false, title = "File already Exists", message = "File Name : " + deanModel.Agreement_No }); }
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        success = false,
                                        title = "File Type is not PDF",
                                        message = "File Name : " + file.FileName
                                    });
                                }

                            }
                            else { return Json(new { success = false, title = "No File Selected", message = "Kindly select a file to Upload" }); }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message += ex;
                            return Json("Error occurred. Error details: " + ex.Message);
                        }
                    }
                    else { return Json(new { success = false, title = "No File Selected", message = "Kindly select a file to Upload" }); }
                }
                else { return Json(new { success = false, title = "Session Expired", message = "Page will be redirected to Login screen" }); }
            }
            else
            {
                return View("_UploadDean",deanModel);
            }
        }
        
        [HttpPost]
        public ActionResult Update(Dean_trxModel deanModel)
        {           
            try
            {
                if(!string.IsNullOrEmpty(deanModel.Agreement_No))
                {
                    int update = dMS_BusinessLayer.Update_Dean(deanModel);
                    if (update == 1)
                    {
                        TempData["msg"] = "Updated Successfully";
                        TempData["no"] = deanModel.Agreement_No;
                        return RedirectToAction("Dean_Index");


                        //return Json(new { success = true, title = "Update Success", message = "Updated Successfully" });
                    }
                    else
                    {
                        TempData["msg"] = "Update Failed";
                        return RedirectToAction("Dean_Index");
                    }
                      
                }
                TempData["msg"] = "Failed..Try Again";
                return RedirectToAction("Dean_Index");
            }
            catch(Exception ex)
            {
                ViewBag.Message += ex;
                TempData["msg"] = ex;
                return RedirectToAction("Dean_Index");
            }
        }
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
        #region Advanced Search
        [Route(Name = "ASearch")]
        public ActionResult _AdvancedSearch()
        {
            Dean_trxModel list = new Dean_trxModel();
            list.AgreementtypeList = list.agreementList();
            return PartialView("_AdvancedSearch", list);
        }
        #endregion

        [HttpPost]
        public ActionResult LoadData(string col)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            //Find Order Column
            //var column = Request.Form.GetValues().FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var search_dean = dMS_BusinessLayer.Search_dean(search, sortColumn, sortColumnDir);
            recordsTotal = search_dean.Count();
            var data = search_dean.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Adv_Load(string type,Int16 year)
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
            var adv_search_dean = dMS_BusinessLayer.Adv_search_dean(search, sortColumn, sortColumnDir, type,year);

            recordsTotal = adv_search_dean.Count();
            var data = adv_search_dean.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Delete(Int64 iID)
        {
            try
            {
                if(iID!=0)
                {
                    string model = dMS_BusinessLayer.Find(iID);
                    if (model != null)
                    {
                        string File = model + ".pdf";

                        int deleted = dMS_BusinessLayer.Delete_Dean(iID);
                        if (deleted == 1)
                        {
                            if (System.IO.File.Exists(File))
                            {
                                System.IO.File.Delete(File);
                            }
                            return Json(new { success = true, data = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new { success = false, data = "Select the row to delete" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false, data = "Failed to delete the record" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false, data = "Select the row to delete" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message += ex;
                return RedirectToAction("Dean_Index");
            }
        }
        #region viewndownload
        public ActionResult ViewDoc(string iID)
        {
            try
            {
                string doc = iID + ".pdf";
                //string path = dMS_BusinessLayer.GetPath(iID);
                if (System.IO.File.Exists(doc))
                {
                    var filestream = new FileStream(doc, FileMode.Open, FileAccess.Read);
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
                return RedirectToAction("Dean_Index");
            }
        }

        public ActionResult Download(string iID)
        {
            try
            {
                if (iID != null)
                {
                    string document = iID + ".pdf";
                    //string path = dMS_BusinessLayer.GetPath(iID);
                    //string path = @"\\10.18.0.29\dms\DeanOffice\Agreement\";
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
    }
}