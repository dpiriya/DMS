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
using System.Net.Mail;
using System.Threading.Tasks;
using Data_Layer.Dataset;

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
                                        {
                                            if (iTotalFileCount == 1)
                                                return Json(new { success = false, title = "Invalid Fileno!!", message = "File Name :" + postedFile.FileName }, JsonRequestBehavior.AllowGet);
                                            else
                                            {
                                                strArray.Add(postedFile.FileName + "-Invalid FileNo");
                                                iFileUploaded++;
                                            }
                                        }
                                        else if (tblName != AppMode)
                                        {
                                            if (iTotalFileCount == 1)
                                                return Json(new { success = false, title = "Kindly upload this file under " + tblName + " mode" }, JsonRequestBehavior.AllowGet);
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
            var data = search_office.ToList();
            if (pageSize != -1)
            {
                data = search_office.Skip(skip).Take(pageSize).ToList();
            }
            JsonResult json = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Adv_Load(string mode, string dept, string coor, string yr)
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
            var adv_search_office = dMS_BusinessLayer.Adv_search_office(search, sortColumn, sortColumnDir, mode, dept, coor, yr);

            recordsTotal = adv_search_office.Count();
            var data = adv_search_office.ToList();
            if (pageSize != -1)
            {
                data = adv_search_office.Skip(skip).Take(pageSize).ToList();
            }
            JsonResult json = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;

        }
        #region Mail
        public ActionResult ProposalReport()
        {
            MailModel m = new MailModel();
            m.ProposalList = m.Proposals();
            m.IdList = m.IDS();
            return View("ProposalReport", m);
        }
        [HttpPost]
        public ActionResult ProposalReport(MailModel m)
        {
            if (m.ProposalNo != null)
            {
                m.ProposalTitle = dMS_BusinessLayer.GetProposalTitle(m.ProposalNo);
            }
            else if (m.ProjectNo != null)
            {
                m.ProposalTitle = dMS_BusinessLayer.GetProjectTitle(m.ProjectNo);
            }
            m.FMail = dMS_BusinessLayer.FacDetails(m.InstiId);
            m.Dean_trxList = dMS_BusinessLayer.MapAgreementToCoor(m.InstiId);
            m.ProposalList = m.Proposals();
            m.IdList = m.IDS();
            return View(m);
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SendMail")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMail(MailModel mail)
        {
            mail.ProposalList = mail.Proposals();
            mail.IdList = mail.IDS();
            string body = "<div style='font-family:Calibri;font-size: 10pt;'><p>Dear Project Investigator, </p><br/>" +
                "<p>Sub: In respect of Project Proposal Title: " + mail.ProposalTitle + "</p><br/>" +
                "<p> As per our IC & SR record, you have undertaken following agreements as witness/confirming party, which are still effective.</p><p> This is to request you to check that your new project proposal is not conflicting with the existing Agreement(s).</p>";
            body += "<table style='font-family:Calibri;font-size: 10pt;' border='1' style='border: 1px solid lightgrey;border-collapse: collapse; border-spacing: 0;' class='table table-striped table-bordered'><tr><th>SI.No</th><th>Industry Partner</th><th>Kind of Agreement</th><th>Effective Date</th><th>End Date</th><th>Remarks</th></tr>";
            foreach (var m in mail.Dean_trxList)
            {
                body += "<tr><td>" + m.Sno + "</td>" +
                    "<td>" + m.Partner + "</td>" +
                    "<td>" + m.Agreement_type + "</td>" +
                    "<td>" + m.Signed_date + "</td>" +
                    "<td>" + m.Expiry_date + "</td>" +
                    "<td>" + m.Title + "</td>" +
                    "</tr>";
            }
            body += "</table>";
            body += "<p>Best Regards</p>";
            body += "<p>Dean IC&SR</p>" +
                "<p>NOTE: This is a System generated mail, for any queries Contact Project Office IC&SR.</p></div>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("cmit-icsr@iitm.ac.in"));
            message.CC.Add(new MailAddress("cmlegal-icsr@imail.iitm.ac.in"));
            message.From = new MailAddress("noreply@ioas.iitm.ac.in");
            message.Subject = "In respect of Project Proposal" + mail.ProposalNo + " Title: " + mail.ProposalTitle;
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                //var credential = new NetworkCredential
                //{
                //    UserName = "noreply@ioas.iitm.ac.in",
                //    Password = "welcomehbs"
                //};
                //smtp.Credentials = credential;
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                TempData["message"] = "Sent";
                return View("ProposalReport", mail);
                //return Json(new { success = true, title = "Mail Sent", message = "Mail sent to : " + message.To, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Print")]
        public ActionResult Print(MailModel m)
        {
            AgreementPrintDS ds = new AgreementPrintDS();
            Report.AgreementReport rpt = new Report.AgreementReport();
            rpt.Load();
            DataRow dr = ds.Tables["ProposalMail"].NewRow();
            dr[0] = m.ProposalNo;
            dr[1] = m.ProjectNo;
            dr[2] = m.FMail.EMPLOYEEID;
            dr[3] = m.ProposalTitle;
            dr[4] = m.FMail.DISPLAYNAME;
            dr[5] = m.FMail.DEPARTMENTNAME;
            ds.Tables["ProposalMail"].Rows.Add(dr);
            foreach(var r in m.Dean_trxList)
            {
                DataRow dr1 = ds.Tables["DeanModel"].NewRow();
                dr1[0] = r.Sno;
                dr1[1] = r.Partner;
                dr1[2] = r.Agreement_type;
                dr1[3] = r.Title;
                dr1[4] = r.Signed_date;
                dr1[5] = r.Expiry_date;
                ds.Tables["DeanModel"].Rows.Add(dr1);
            }
            rpt.SetDataSource(ds);
            Stream filestream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(filestream, "application/pdf");
        }
        #endregion
    }
}
