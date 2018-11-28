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
using System.Data.SqlClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text.RegularExpressions;
using System.Linq.Dynamic;
using System.Configuration;

namespace DMS.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        DMS_businessLayer dMS_BusinessLayer = new DMS_businessLayer();
        // GET: Accounts
        public ActionResult Accounts_Index()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dMS_BusinessLayer.Summary("Accounts"));
            return View(ds);
        }

        public ActionResult _UploadAccounts(string AppMode)
        {
            try
            {
                return PartialView("_UploadAccounts", AppMode);
            }
            catch (Exception ex)
            {
                return null;

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
                        if (AppMode == "file")
                            path = @"\\DMS_SAN\dms\Accounts\File\";
                        else if (AppMode == "voucher")
                            path = @"\\DMS_SAN\dms\Accounts\Voucher\";
                        else
                            return Json(new { success = false, title = "Error in selecting the mode.. Redo the process!!" });
                        //}
                        if (!Directory.Exists(path))
                        {
                            return Json(new { success = false, title = "Path Error " }, JsonRequestBehavior.AllowGet);
                        }

                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < iTotalFileCount; i++)
                        {
                            HttpPostedFileBase postedFile = files[i];
                            string[] fnArray;
                            string projno = null;
                            //string sThirdChar = postedFile.FileName[2].ToString();
                            //bool Is_Spon = Regex.IsMatch(sThirdChar, @"^[a-zA-Z]+$");

                            bool Is_Spon;
                            string filenamewoext = Path.GetFileNameWithoutExtension(postedFile.FileName);
                            string sVoucher = null;

                            if (postedFile.ContentType == "application/pdf")
                            {
                                //check if project no is valid
                                DataSet ds = new DataSet();
                                



                                if (AppMode == "V" || AppMode == "voucher")
                                {
                                    fnArray = filenamewoext.Split('_');
                                    int fnLength = fnArray.Length;
                                    if(fnLength!=4)
                                    {
                                        if (iTotalFileCount == 1)
                                            return Json(new { success = false, title = "Invalid Project & Voucher No", message = "Filename is not in the correct format" }, JsonRequestBehavior.AllowGet);
                                        else
                                        {
                                            strArray.Add(postedFile.FileName + " - Not Uploaded (Invalid File Format) ");
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        string sThirdChar = fnArray[1].Substring(2, 1);
                                        Is_Spon = Regex.IsMatch(sThirdChar, @"^[a-zA-Z]+$");
                                        sVoucher = fnArray[0];
                                        projno = fnArray[1];
                                    }
                                    //int Index = filenamewoext.IndexOf('_');

                                    //if (Index == -1)
                                    //{                                        
                                    //    if (iTotalFileCount == 1)
                                    //        return Json(new { success = false, title = "Invalid Project & Voucher No", message = "Kindly add Voucher-no to the file name!!" }, JsonRequestBehavior.AllowGet);
                                    //    else
                                    //    {
                                    //        strArray.Add(postedFile.FileName + " - Not Uploaded (No Voucher-no) ");
                                    //        continue;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    try
                                    //    {
                                    //         fnArray= filenamewoext.Split('_');
                                    //        sVoucher = fnArray[0];
                                    //        projno = fnArray[1];
                                    //        //filenamewoext = fnArray[0] + '_' + fnArray[2] + fnArray[3];
                                    //    }
                                    //    catch(Exception ex)
                                    //    {
                                    //        if (iTotalFileCount == 1)
                                    //            return Json(new { success = false, title = "Invalid Project & Voucher No", message = "File Name is not in the correct format" }, JsonRequestBehavior.AllowGet);
                                    //        else
                                    //        {
                                    //            strArray.Add(postedFile.FileName +ex);
                                    //            continue;
                                    //        }
                                    //    }

                                    //   // sVoucher = filenamewoext.Substring(Index + 1);
                                    //}
                                    //filenamewoext = filenamewoext.Substring(0, Index);
                                }
                                else
                                {
                                    int Index = filenamewoext.IndexOf('_');
                                    string sThirdChar = postedFile.FileName[2].ToString();
                                    Is_Spon = Regex.IsMatch(sThirdChar, @"^[a-zA-Z]+$");
                                    projno = filenamewoext;
                                    if (Index != -1)
                                    {
                                        if (iTotalFileCount == 1)
                                            return Json(new { success = false, title = "Invalid Project Name", message = "File Name contains Voucher-No!!" });
                                        else
                                        {
                                            strArray.Add(postedFile.FileName + " - Not Uploaded (Contains Voucher-no) ");
                                            continue;
                                        }
                                    }
                                }


                                //if (AppMode == "V" || AppMode == "voucher")
                                //    //ds.Tables.Add(dMS_BusinessLayer.AccProjectName_verification(postedFile.FileName.Substring(0, postedFile.FileName.IndexOf('_')), Is_Spon));
                                //    ds.Tables.Add(dMS_BusinessLayer.AccProjectName_verification(projno, Is_Spon));
                                //else
                                    ds.Tables.Add(dMS_BusinessLayer.AccProjectName_verification(projno, Is_Spon));

                                if (AppMode == "file" || AppMode == "F")
                                    AppMode = "F";
                                else
                                    AppMode = "V";




                                int iIsValid_ProjectNo = 0;

                                if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                                {
                                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                                    {
                                        iIsValid_ProjectNo = 1;
                                    }
                                }

                                if (iIsValid_ProjectNo != 1)
                                {
                                    if (iTotalFileCount == 1)
                                        return Json(new { success = false, title = "Invalid Project No", message = "There's no project under this name !!" });
                                    else
                                    {
                                        strArray.Add(postedFile.FileName + " - Not Uploaded (Invalid Project No) ");
                                        iFileNotUploaded++;
                                        continue;
                                    }
                                }


                                //////////////////////////////////


                                if (!System.IO.File.Exists(path + postedFile.FileName))
                                {
                                    using (DMSEntities dms = new DMSEntities())
                                    {
                                        string fileName = Path.GetFileName(postedFile.FileName);
                                        int numberOfPages;
                                        postedFile.SaveAs(path + fileName);
                                        using (PdfReader pdfReader = new PdfReader(path + fileName))
                                        {
                                            numberOfPages = pdfReader.NumberOfPages;
                                        }

                                        try
                                        {

                                            // int iInsertFileUpload = dMS_BusinessLayer.FileUpload_Accounts(filenamewoext, sVoucher, AppMode, numberOfPages, path, fileName, Session["Username"].ToString(), Is_Spon);
                                            int iInsertFileUpload = dMS_BusinessLayer.FileUpload_Accounts(projno, sVoucher, AppMode, numberOfPages, path, fileName, Session["Username"].ToString(), Is_Spon);
                                            if (iInsertFileUpload == 1)
                                            {
                                                if (iTotalFileCount == 1)
                                                    return Json(new { success = true, title = "File Uploaded !!", message = "File Name : " + postedFile.FileName + " - " + numberOfPages + " pages" }, JsonRequestBehavior.AllowGet);
                                                else
                                                {
                                                    strArray.Add(postedFile.FileName + " - Uploaded Successfully ");
                                                    iFileUploaded++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                if (System.IO.File.Exists(path + postedFile.FileName))
                                                {
                                                    System.IO.File.Delete(path + postedFile.FileName);
                                                }
                                                if (iTotalFileCount == 1)
                                                    return Json(new { success = false, title = "Invalid Project No", message = "There's no project under this name !!" }, JsonRequestBehavior.AllowGet);
                                                else
                                                {
                                                    strArray.Add(postedFile.FileName + " - Not Uploaded (Invalid Project No) ");
                                                    iFileNotUploaded++;
                                                    continue;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (iTotalFileCount == 1)
                                                return Json(new { success = false, title = "Oops", message = "Server Error !!" });
                                            else
                                            {
                                                strArray.Add(postedFile.FileName + " - Not Uploaded ");
                                                iFileNotUploaded++;
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
                                        continue;
                                    }
                                }

                            }
                            else
                            {
                                if (iTotalFileCount == 1)
                                    return Json(new { success = false, title = "File Type is not PDF", message = "File Name : " + postedFile.FileName }, JsonRequestBehavior.AllowGet);
                                else
                                {
                                    strArray.Add(postedFile.FileName + " - Non PDF File ");
                                    iFileTypeMismatch_Count++;
                                    continue;
                                }
                            }

                        }
                        ////////////////////// return total files status
                        //return Json(new { success = true, title = "File Upload Status", message = "Total Files : " + iTotalFileCount + ", Files Uploaded :" + iFileUploaded +
                        //                                        ", Files Already Exists :" + iFileExistsCount + ", Non PDF Files :" + iFileTypeMismatch_Count }); // + ", Files Not Uploaded :" + iFileNotUploaded });

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
                    return Json(new { success = false, title = "No File Selected", message = "Kindly select files to Upload" },JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, title = "Session Expired", message = "Page will be redirected to Login screen" }, JsonRequestBehavior.AllowGet);
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
                string document = iID;
                if (System.IO.File.Exists(document))
                {
                    var filestream = new FileStream(document, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(filestream, "application/pdf");
                }
                else
                {
                    //Saranya
                    // ViewBag.Message += "File Not Exists in the specified path";
                    return Json(new { success = false, data = "File Not Exists in the specified path" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "File Not Exists in the specified path" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Download(string iID)
        {
            try
            {
                string document = iID;
                //string path = @"E:\priya\DMS\Files\Recruitment\";
                //path += document;
                return File(document, "application/pdf", document);

            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult _Add(string iID)
        {
            return PartialView();
        }
        #endregion
        #region Advanced Search
        [Route(Name = "ASearch")]
        public ActionResult _AdvancedSearch()
        {
            DropdownList list = new DropdownList();
            list.Departments = list.DepartmentList();
            list.CoorList = list.CoorNameList();
            list.spon_con_List = list.sponconlist();
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
            var search_accounts = dMS_BusinessLayer.Search_accounts(search, sortColumn, sortColumnDir);
            recordsTotal = search_accounts.Count();
            var data = search_accounts.ToList();
            if(pageSize!=-1)
            {
                data = search_accounts.Skip(skip).Take(pageSize).ToList();
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

            var adv_search_accounts = dMS_BusinessLayer.Adv_search_accounts(search, sortColumn, sortColumnDir, mode, coor, dept);

                recordsTotal = adv_search_accounts.Count();
                var data = adv_search_accounts.ToList();
            if(pageSize!=-1)
            {
                data = adv_search_accounts.Skip(skip).Take(pageSize).ToList();
            }
            JsonResult json= Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;

        }
    }
}