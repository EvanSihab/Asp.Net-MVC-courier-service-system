using CSSEntity;
using CSSService;
using System;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class ManagerController : Controller
    {
        EmployeeService employeeService = new EmployeeService();
        TripService tripService = new TripService();
        MailModelService mailModelService = new MailModelService();
        RequestUpdateService requestUpdateService = new RequestUpdateService();
        public ActionResult Dashboard()
        {
            if (Session["uname"] != null)
            {
                Employee employee = employeeService.Get(Session["Id"].ToString());
                ViewBag.SinceUser = Math.Round((DateTime.Now - employee.RegistrationTime).TotalDays, 2);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Trips()
        {
            if (Session["uname"] != null)
            {
                return View(tripService.GetAllById(Session["Id"].ToString()));
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ViewProfile()
        {
            if (Session["uname"] != null)
            {
                Employee employee = employeeService.Get(Session["Id"].ToString());
                return View(employee);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["uname"] != null)
            {
                Employee employee = employeeService.Get(Session["Id"].ToString());
                return View(employee);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost, ActionName("EditProfile")]
        public ActionResult SendRequeset()
        {
            if (Session["uname"] != null)
            {
                MailModel mailModel = new MailModel();
                mailModel.To = "Admin";
                mailModel.From = Session["id"].ToString();
                mailModel.Subject = "Update Request";
                mailModel.Body = "<a>You Have got an Update Request From " + mailModel.From + "</a>";
                mailModel.Date = DateTime.Now;
                mailModel.Status = 1;
                mailModelService.InsertMail(mailModel);
                RequestUpdate requestUpdate = new RequestUpdate();
                requestUpdate.Phone = Request["PhoneNumber"];
                requestUpdate.NID = Request["NationalId"];
                requestUpdate.DOB = Convert.ToDateTime(Request["Birthday"]);
                requestUpdate.Address = Request["Address"];
                requestUpdate.Name = Request["Name"];
                requestUpdate.Id = Session["id"].ToString();
                if (requestUpdateService.Get(requestUpdate.Id) != null)
                {
                    requestUpdateService.Delete(requestUpdate.Id);
                    requestUpdateService.Insert(requestUpdate);
                }
                else
                {
                    requestUpdateService.Insert(requestUpdate);
                }
                TempData["Reply"] = "Your request has been sent to the proper authority";
                return RedirectToAction("EditProfile", "Manager");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["uname"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost, ActionName("ChangePassword")]
        public ActionResult ConfirmPassword()
        {
            if (Session["uname"] != null)
            {
                string Id = Session["Id"].ToString();
                Employee employee = employeeService.Get(Id);
                if (Request["newPassword"].Equals(Request["retypeNewPassword"]) && employee.Password.Equals(Request["Password"]))
                {
                    Session["message"] = null;
                    employee.Password = Request["newPassword"];
                    employeeService.Update(employee);
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["message"] = "Opps!!! Something Wrong... Try Again";
                    return View("ChangePassword");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        //[HttpGet]
        //public ActionResult Inbox()
        //{
        //    if (Session["uname"] != null)
        //    {
        //        return View();
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
    }
}