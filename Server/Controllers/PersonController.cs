using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Utils;
using System.IO;
using System.Text.Json;

namespace Server.Controllers
{
    public class PersonController : Controller
    {
        [HttpGet]
        public IActionResult AddPerson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPerson(string name, IFormFile image)
        {
            if (image == null || string.IsNullOrEmpty(name))
            {
                ViewBag.Error = "Invalid input.";
                return View();
            }

            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            var result = PythonCaller.CallPythonCLI("add-person", name, tempFilePath);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

            if (response.ContainsKey("Error"))
            {
                ViewBag.Error = response["Error"];
            }
            else if (response.ContainsKey("Success"))
            {
                ViewBag.Success = response["Success"];
            }
            else
            {
                ViewBag.Unknown = "Unknown response from add-person command";
            }

            return View();
        }

        [HttpGet]
        public IActionResult SearchPerson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchPerson(IFormFile image)
        {
            if (image == null)
            {
                ViewBag.Error = "Invalid input.";
                return View();
            }

            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            var result = PythonCaller.CallPythonCLI("search-person", tempFilePath);
            var jsonResult = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);

            if (jsonResult.ContainsKey("Error"))
            {
                ViewBag.Error = jsonResult["Error"].GetString();
            }
            else if (jsonResult.ContainsKey("Found"))
            {
                var mainImagePath = jsonResult["main_image"].GetString();
                var fileName = Path.GetFileName(mainImagePath);
                ViewBag.MainImage = Url.Content("~/images/" + fileName);
                ViewBag.Found = jsonResult["Found"].GetString();
            }
            else
            {
                ViewBag.Unknown = "Unknown response from search-person command";
            }
            
            return View();
        }


        [HttpGet]
        public IActionResult UpdatePerson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePerson(string name, IFormFile image)
        {
            if (image == null || string.IsNullOrEmpty(name))
            {
                ViewBag.Error = "Invalid input.";
                return View();
            }

            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            var result = PythonCaller.CallPythonCLI("update-person", name, tempFilePath);
            var response = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

            if (response.ContainsKey("Error"))
            {
                ViewBag.Error = response["Error"];
            }
            else if (response.ContainsKey("Success"))
            {
                ViewBag.Success = response["Success"];
            }
            else
            {
                ViewBag.Unknown = "Unknown response from update-person command";
            }

            return View();
        }

        [HttpGet]
        public IActionResult SearchName()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ViewBag.Error = "Invalid input.";
                return View();
            }

            var result = PythonCaller.CallPythonCLI("search-name", name);
            var response = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);

            if (response.ContainsKey("Error"))
            {
                ViewBag.Error = response["Error"].GetString();
            }
            else if (response.ContainsKey("Success"))
            {
                var results = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response["Success"].ToString());

                // Adjust image paths
                foreach (var item in results)
                {
                    var fullPath = item["main_image"];
                    var fileName = Path.GetFileName(fullPath);
                    item["main_image"] = Url.Content("~/images/" + fileName);
                }

                ViewBag.Results = results;
            }
            else
            {
                ViewBag.Unknown = "Unknown response from search-name command";
            }

            return View();
        }
    }
}
