using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Assessment.ViewModel;
using Assessment.Models;

namespace Assessment.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            SearchViewModel model = new SearchViewModel();

            return View(model);
        }        

        [HttpGet]
        public JsonResult SearchJson(string strStartDate, string strEndDate)
        {
            List<Record> records = new List<Record>();
            List<Display> display = new List<Display>();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string apiUrl = "https://eservices.mas.gov.sg/api/action/datastore/search.json?resource_id=5f2b18a8-0883-4769-a635-879c63d3caac&limit=10000";
            var json = string.Empty;
            using (var w = new WebClient())
            {
                json = w.DownloadString(apiUrl);
            }
            RootObject result = JsonConvert.DeserializeObject<RootObject>(json);
            if (result != null)
            {
                records = result.result.records;

                if (!string.IsNullOrEmpty(strStartDate) && !string.IsNullOrEmpty(strEndDate) )
                {
                    DateTime startDate = DateTime.ParseExact(strStartDate, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(strEndDate, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);

                    records = records.Where(a => DateTime.ParseExact(a.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture) >= startDate && DateTime.ParseExact(a.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture) <= endDate).ToList();

                    if (records.Count > 0)
                    {
                        foreach (var rec in records)
                        {
                            Display disp = new Display();
                            disp.year = DateTime.ParseExact(rec.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy");
                            disp.month = DateTime.ParseExact(rec.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture).ToString("MMM");
                            disp.prime_lending_rate = string.IsNullOrEmpty(rec.prime_lending_rate) ? "0" : rec.prime_lending_rate;
                            disp.banks_fixed_deposits_3m = string.IsNullOrEmpty(rec.banks_fixed_deposits_3m) ? "0" : rec.banks_fixed_deposits_3m;
                            disp.banks_fixed_deposits_6m = string.IsNullOrEmpty(rec.banks_fixed_deposits_6m) ? "0" : rec.banks_fixed_deposits_6m;
                            disp.banks_fixed_deposits_12m = string.IsNullOrEmpty(rec.banks_fixed_deposits_12m) ? "0" : rec.banks_fixed_deposits_12m;
                            disp.banks_savings_deposits = string.IsNullOrEmpty(rec.banks_savings_deposits) ? "0" : rec.banks_savings_deposits;
                            disp.fc_hire_purchase_motor_3y = string.IsNullOrEmpty(rec.fc_hire_purchase_motor_3y) ? "0" : rec.fc_hire_purchase_motor_3y;
                            disp.fc_housing_loans_15y = string.IsNullOrEmpty(rec.fc_housing_loans_15y) ? "0" : rec.fc_housing_loans_15y;
                            disp.fc_fixed_deposits_3m = string.IsNullOrEmpty(rec.fc_fixed_deposits_3m) ? "0" : rec.fc_fixed_deposits_3m;
                            disp.fc_fixed_deposits_6m = string.IsNullOrEmpty(rec.fc_fixed_deposits_6m) ? "0" : rec.fc_fixed_deposits_6m;
                            disp.fc_fixed_deposits_12m = string.IsNullOrEmpty(rec.fc_fixed_deposits_12m) ? "0" : rec.fc_fixed_deposits_12m;
                            disp.fc_savings_deposits = string.IsNullOrEmpty(rec.fc_savings_deposits) ? "0" : rec.fc_savings_deposits;

                            display.Add(disp);
                        }
                    }
                }
             
                
            }
            return Json(display, JsonRequestBehavior.AllowGet);
        }
    }
}