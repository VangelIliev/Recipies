﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    public class ErrorsController : Controller
    {
        public async Task<ActionResult> CustomError()
        {
            return View();
        }
    }
}
