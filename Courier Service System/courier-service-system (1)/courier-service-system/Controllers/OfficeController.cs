using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class OfficeController : Controller
    {

        // GET: Office
        OfficeService officeService = new OfficeService();
        EmployeeService employeeService = new EmployeeService();
        public ActionResult Index(string id)
        {
            if (Session["uname"] != null)
            {
                Session["CurrentOffice"] = null;
                Session.Remove("CurrentOffice");
                if (id == null)
                {
                    if (Session["officeDivision"] == null)
                    {
                        Session["officeDivision"] = "Barishal";
                    }
                }
                else
                {
                    Session["officeDivision"] = id;
                }

                return View(officeService.GetAllForOffice(Session["officeDivision"].ToString()));
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["uname"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Create(OfficeViewModel ofc)
        {
            if (Session["uname"] != null)
            {
                if (ModelState.IsValid)
                {
                    Office newOffice = new Office();
                    string division = ofc.Division;
                    int count = officeService.getRowCount() + 1;
                    string offcieid = division[0].ToString() + division[1].ToString() + division[2].ToString() + "-" + ofc.Area + "-" + count;
                    newOffice.OfficeId = offcieid;
                    newOffice.Location = "House : " + ofc.House + ", Road : " + ofc.Road + ", " + ofc.Area + ", " + division;
                    newOffice.OfficialNumber = ofc.OfficialNumber;
                    officeService.Insert(newOffice);
                    return RedirectToAction("Index", "Office");
                }
                return View(ofc);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["uname"] != null)
            {
                Office ofc = officeService.Get(id);
                return View(ofc);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(Office ofc)
        {
            if (Session["uname"] != null)
            {
                officeService.Update(ofc);
                return RedirectToAction("Index", "Office");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["uname"] != null)
            {
                Office ofc = officeService.Get(id);
                return View(ofc);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            if (Session["uname"] != null)
            {
                officeService.Delete(id);
                return RedirectToAction("Index", "Office");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ShowEmployees(string id)
        {
            if (Session["uname"] != null)
            {
                Session["CurrentState"] = null;
                Session.Remove("CurrentState");
                Session["officeId"] = id;
                string division = id[0].ToString() + id[1].ToString() + id[2].ToString();
                List<Employee> emplist = employeeService.GetEmployeesForOffice(division, Session["officeId"].ToString());
                return View(emplist);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult AddNewEmployee()
        {
            if (Session["uname"] != null)
            {
                Session["CurrentState"] = "Office";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult AddNewEmployee(Employee employee)
        {
            if (Session["uname"] != null)
            {
                string Position = Request["Position"].ToString();
                employee.RegistrationTime = DateTime.Now;
                employee.Password = employeeService.generatePass();
                employee.Id = employeeService.generateId(Position, employee.RegistrationTime.ToString());
                employee.OfficeId = Session["officeId"].ToString();
                employee.CurrentStatus = "Active";
                employee.CurrentLocation = Session["officeId"].ToString();
                employeeService.Insert(employee);
                Session["CurrentState"] = "Office";
                return RedirectToAction("Confirmation", "SendMailer", new { id = employee.Id });
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Result(string id)
        {
            if (Session["uname"] != null)
            {
                Employee emp = employeeService.Get(id);
                return View(emp);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            if (Session["uname"] != null)
            {
                Employee emp = employeeService.Get(id);
                OfficeService officeRepository = new OfficeService();
                List<SelectListItem> officeList = new List<SelectListItem>();
                foreach (Office office in officeRepository.GetAll())
                {
                    SelectListItem option = new SelectListItem();
                    option.Text = office.OfficeId;
                    option.Value = office.OfficeId;
                    officeList.Add(option);
                }

                ViewBag.Offices = officeList;
                return View(emp);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult EditEmployee(Employee employeeUpdate)
        {
            if (Session["uname"] != null)
            {
                employeeService.Update(employeeUpdate);
                return RedirectToAction("ShowEmployees", "Office", new { id = employeeUpdate.OfficeId });
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult EmployeeDetails(string id)
        {
            if (Session["uname"] != null)
            {
                Employee emp = employeeService.Get(id);
                return View(emp);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public ActionResult DeleteEmployee(string id)
        {
            if (Session["uname"] != null)
            {
                Employee emp = employeeService.Get(id);
                return View(emp);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost,ActionName("DeleteEmployee")]
        public ActionResult ConfirmDeleteEmployee(string id)
        {
            if (Session["uname"] != null)
            {
                employeeService.Delete(id);
                return RedirectToAction("ShowEmployees", "Office", new { id = Session["officeId"] });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}