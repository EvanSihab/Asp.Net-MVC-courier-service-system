﻿using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CSSApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        OfficeService officeService = new OfficeService();
        MailModelService mailModelService = new MailModelService();
        public IHttpActionResult Get()
        {
            List<Office> officeList = officeService.GetAll();
            return Ok(officeList);
        }

        // GET api/values/5

        // POST api/values
        public IHttpActionResult Post([FromBody]MailModel mail)
        {
            mail.To = "Admin";
            mail.Date = DateTime.Now;
            mailModelService.InsertMail(mail);
            return Ok("Success");
        }
    }
}
