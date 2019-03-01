using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data_Layer.Models.Home;
using Data_Layer.Repository;
 

namespace DMS.Controllers
{

    public class HomeController : Controller
    {
        //List<long> SelectedIds = null;
        #region Login Controller Action
        [AllowAnonymous]
        public ActionResult Login()
        {            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            using (DMSEntities dms = new DMSEntities())
                {
                    var test = (from user in dms.tbl_mst_Users where user.userName == model.UserName && user.userPassword == model.Password select user).FirstOrDefault();
                    if (test != null)
                    {
                    if (test.is_active == true)
                    {
                        Session["Username"] = test.userName;
                        Session["roleid"] = test.roleId;

                        FormsAuthentication.SetAuthCookie(test.userName, false);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), true, test.roleId.ToString(), FormsAuthentication.FormsCookiePath);
                        string encyptedticket = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encyptedticket);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Request.QueryString["ReturnUrl"] == null)
                        {
                            if (Session["roleid"].ToString() == "1")
                                return RedirectToAction("Welcomepage", "Home");
                            else if (Session["roleid"].ToString() == "2")
                                return RedirectToAction("Accounts_Index", "Accounts");
                            else if (Session["roleid"].ToString() == "3")
                                return RedirectToAction("OfficeIndex", "Office");
                            else if (Session["roleid"].ToString() == "4")
                                return RedirectToAction("Recruit_Index", "Recruitment");
                            else if (Session["roleid"].ToString() == "5")
                                return RedirectToAction("PurchaseIndex", "Purchase");
                            else if (Session["roleid"].ToString() == "6")
                                return RedirectToAction("Dean_Index", "Dean");
                            else
                                return RedirectToAction("login", "Home");
                        }
                        else
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"]);
                        }
                    }
                    else
                        ViewBag.ErrorActive = "Your profile is not active. To activate please contact administrator.";
                    }
                    else
                {
                    ViewBag.ErrorActive = "Incorrect Username and Password";
                }
                }            
            return View();          

        }
        #endregion
        #region welcomepage
        [Authorize(Roles ="1")]
        public ActionResult Welcomepage()
        {
            if (Session["Username"] != null)
            {
                ViewData["UserName"] = Session["Username"];

            }
            else
            {
                return RedirectToAction("login", "Home");
            }
            return View();
        }
        #endregion

        #region logout
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session["Username"] = null;
            return RedirectToAction("login", "Home");
        }
        #endregion

        #region Bindmenu View Action
        //public PartialViewResult BindMenu()// Method used to build the menu.
        //{
        //    ViewData["Username"] = Session["Username"];
        //    using (DMSEntities dms = new DMSEntities())
        //    {
        //        string yourEncodedHtml = "";
        //        if (Session["roleid"] != null)
        //        {
        //            try
        //            {
        //                int rollid = Convert.ToInt16(Session["roleid"]);
        //                var test = (from menu in dms.tbl_mst_Mapping where menu.roleId == rollid select menu).FirstOrDefault();
        //                string role = test.func_allowed.ToString();
        //                role = role.Substring(0, role.Length);
        //                SelectedIds = role.Split(',').Select(long.Parse).ToList<long>();
        //                var menusql = (from menu in dms.tbl_mst_Menu where SelectedIds.Contains(menu.menuLevel) orderby menu.menuLevel select menu);
        //                yourEncodedHtml += "<ul class='sidebar-menu'>";
        //                foreach (var menutest in menusql.ToList())
        //                {
        //                    int mod = menutest.menuSubLevel;
        //                    if (mod == 0)
        //                    {
        //                        int menusub = menutest.menuLevel;
        //                        yourEncodedHtml += "<li class='treeview'><a href='#'><i class='fa fa-files-o'></i><span>" + menutest.menuName + "</span><i class='fa fa-angle-right pull-right'></i></a>";
        //                        var sublevel = (from value in menusql where value.menuSubLevel == menusub orderby value.menuLevel select value).ToList();
        //                        if (sublevel != null)
        //                        {
        //                            yourEncodedHtml += "<ul class='treeview-menu'>";
        //                            foreach (var lists in sublevel.ToList())
        //                            {
        //                                yourEncodedHtml += "<li><a href='" + '.' + '.' + '/' + lists.controller + '/' + lists.pageName + "' alt='" + lists.controller + '/' + lists.pageName + "' id='" + lists.pageName + "' class='" + lists.controller + "'><i class='fa fa-circle-o'></i>" + lists.menuName + "</a></li>";
        //                            }
        //                            yourEncodedHtml += "</ul>";
        //                        }

        //                        yourEncodedHtml += "</li>";
        //                    }
        //                }
        //                yourEncodedHtml += "</ul>";
        //            }
        //            catch (Exception ex)
        //            {
        //                return PartialView("Error", new HandleErrorInfo(ex, "Home", "BindMenu"));
        //            }
        //        }
        //        var ViewHtml = new MvcHtmlString(yourEncodedHtml);
        //        ViewBag.Tiffen = ViewHtml;
        //        return PartialView("_BindMenus");
        //    }

        //}

        #endregion
        
    }
}