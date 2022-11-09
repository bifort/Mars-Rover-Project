using MarsRoverProject.Domain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MarsRoverProject.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Solution()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadInstructions(HttpPostedFileBase file)
        {
            try
            {
                var allowedFileExtension = ".txt";
                if (file != null && file.ContentLength > 0)
                {
                    string fileExt = Path.GetExtension(file.FileName).ToLower();
                    if (fileExt == allowedFileExtension)
                    {
                        List<string> lines = new List<string>();
                        using (var reader = new StreamReader(file.InputStream))
                        {
                            var line = string.Empty;
                            while ((line = reader.ReadLine()) != null)
                            {
                                lines.Add(line.Trim());
                            }
                        }

                        MissionControlService mcs = new MissionControlService();
                        var result = mcs.ProcessInstructions(lines.ToArray());
                        ViewBag.JSON = JsonConvert.SerializeObject(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Only TXT files are allowed");
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View("Solution");
        }        
    }
}