using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Promotion.Data;
using Promotion.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Promotion.Controllers
{
    public class PromotionController : Controller
    {
        private readonly ILogger<PromotionController> _logger;
        private readonly StoreDBContext _conn;
        private readonly IWebHostEnvironment _env;
        private string message = string.Empty;
        public PromotionController(ILogger<PromotionController> logger, StoreDBContext conn, IWebHostEnvironment _environment)
        {
            _logger = logger;
            _conn = conn;
            _env = _environment;
        }

        public IActionResult Index()
        {
            MPromotion model = new MPromotion();
            List<Store> stores;
            try
            {
                model.Stores = _conn.Stores.ToList();
            }
            catch (Exception ex) { }
            return View(model);
        }
        [HttpPost]
        public IActionResult UploadFile()
        {

            try
            {
                foreach (var formFile in Request.Form.Files)
                {
                    var fulPath = Path.Combine(_env.ContentRootPath, "Upload", formFile.FileName);
                    using (FileStream fs = System.IO.File.Create(fulPath))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }
                    return Json("Upload File Success !");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(message);
        }

        [HttpPost]
        public IActionResult Add(MPromotion promotion)
        {
            List<string> items = new List<string>();
            try
            {
                var fullPathOfUpload = Path.Combine(_env.ContentRootPath, "Upload", "Items.txt");
                var fullPathOfResult = Path.Combine(_env.ContentRootPath, "Result", String.Format("{0}.txt", promotion.promoID));
                string[] lines = System.IO.File.ReadAllLines(fullPathOfUpload);

                int counter = 0;
                foreach (string line in lines)
                {
                    if (counter > 0)
                    {
                        items.Add(line);
                    }
                    counter++;
                }
                if (System.IO.File.Exists(fullPathOfResult))
                {
                    System.IO.File.Delete(fullPathOfResult);
                }
                using (StreamWriter sw = System.IO.File.CreateText(fullPathOfResult))
                {
                    sw.WriteLine("FHEAD|{0}|||;", promotion.promoDescription);
                    foreach (var item in items)
                    {
                        sw.WriteLine("FITEM|{0}|{1}|{2}|;", item, promotion.valueType, promotion.valueTotal);
                    }
                    foreach (var store in promotion.Stores)
                    {
                        sw.WriteLine("FITEM|{0}|{1}|{2}|;", store.storeID, promotion.promoStartDate.ToString("yyyymmdd"), promotion.promoEndDate.ToString("yyyymmdd"));
                    }
                    sw.WriteLine("FHEAD||||;");

                    //string value1 = promotion.promoID.Remove(promotion.promoID.Length - 4, 4);
                    //string value2 = (Convert.ToInt32(promotion.promoID.Substring(promotion.promoID.Length - 4, promotion.promoID.Length - 1)) + 1).ToString();


                    message = promotion.promoID.Remove(promotion.promoID.Length - 4, 4) + runningNUmber(Convert.ToInt32(promotion.promoID.Substring(promotion.promoID.Length - 4)) + 1);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }



            return Json(message);
        }

        public FileResult DownloadFile(string fileName)
        {
            fileName = String.Format("{0}.txt", fileName);
            var fullPathOfResult = Path.Combine(_env.ContentRootPath, "Result", fileName);


            byte[] bytes = System.IO.File.ReadAllBytes(fullPathOfResult);


            return File(bytes, "application/octet-stream", fileName);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private string runningNUmber(int number)
        {
            string runningNumber = string.Empty;
            if (number < 10)
                runningNumber += "000" + number;
            else if (number < 100)
                runningNumber += "00" + number;
            else if (number < 1000)
                runningNumber += "0" + number;
            else if (number < 10000)
                runningNumber += number.ToString();

            return runningNumber;
        }



    }
}
