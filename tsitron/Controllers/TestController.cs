using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace tsitron.Controllers
{
    public class TestController : ApiController
    {
        public string GetString()
        {
            return "Test message";
        }
    }
}
