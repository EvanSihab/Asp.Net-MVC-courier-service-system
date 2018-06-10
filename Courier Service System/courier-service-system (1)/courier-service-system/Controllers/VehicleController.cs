using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSSService;
using CSSEntity;

namespace courier_service_system.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        EmployeeService employeeService = new EmployeeService();
        VehicleService vehicleService = new VehicleService();
        OfficeService officeService = new OfficeService();
        public ActionResult AllVehicle()
        {
            if (Session["uname"] != null)
            {
                Session["VehicleNumber"] = null;
                Session.Remove("VehicleNumber");
                return View(this.vehicleService.GetAll());
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult AddNewVehicle()
        {
            if (Session["uname"] != null)
            {
                List<SelectListItem> OfficeList = new List<SelectListItem>();
                foreach (Office office in officeService.GetAll())
                {
                    SelectListItem option = new SelectListItem();
                    option.Text = office.OfficeId;
                    option.Value = office.OfficeId;
                    OfficeList.Add(option);
                }
                ViewBag.OfficeList = OfficeList;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult AddNewVehicle(Vehicle vehicle)
        {
            if (Session["uname"] != null)
            {
                List<Vehicle> vehicleList = vehicleService.GetAllTypeForCheck(vehicle);
                if (vehicleList.Count == 0)
                {
                    int i = 0;
                    vehicle.VehicleRegisteredByEmployee = Session["id"].ToString();
                    vehicle.VehicleRegisterDate = DateTime.Now;
                    vehicle.VehicleEntryByEmployeeId = Session["id"].ToString();
                    i = vehicleService.Insert(vehicle);
                    if (i != 0)
                    {
                        TempData["message"] = "Successfully Added";
                    }
                    return RedirectToAction("AllVehicle", "Vehicle");
                }
                else
                {
                    TempData["VehicleError"] = "A Vehicle is already added with this number : " + vehicle.VehicleNumber;
                    return RedirectToAction("AddNewVehicle", "Vehicle");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["uname"] != null)
            {
                Vehicle vcl = this.vehicleService.Get(id);
                Session["VehicleNumber"] = vcl.VehicleNumber;
                return View(vcl);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (Session["uname"] != null)
            {
                int i = 0;
                i = this.vehicleService.UpdateVehicle(vehicle, Session["VehicleNumber"].ToString());
                if (i != 0)
                {
                    TempData["message"] = "Successfully Updated";
                }
                return RedirectToAction("AllVehicle", "Vehicle");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            if (Session["uname"] != null)
            {
                return View(this.vehicleService.Get(id));
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["uname"] != null)
            {
                return View(this.vehicleService.Get(id));
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            if (Session["uname"] != null)
            {
                int i = 0;
                i = vehicleService.Delete(id);
                if (i != 0)
                { TempData["message"] = "Successfully Deleted"; }
                return RedirectToAction("AllVehicle", "Vehicle");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}