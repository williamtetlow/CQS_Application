using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CqsApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        public string GetUsername()
        {
            return "User 1";
        }
    }
}
