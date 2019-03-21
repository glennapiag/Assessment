using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment.Models;

namespace Assessment.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string strStartDate = "2018-05";
            string strEndDate = "2019-03";
            List<Record> records = new List<Record>();

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

                if (!string.IsNullOrEmpty(strStartDate) && !string.IsNullOrEmpty(strEndDate))
                {
                    DateTime startDate = DateTime.ParseExact(strStartDate, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(strEndDate, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);

                    records = records.Where(a => DateTime.ParseExact(a.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture) >= startDate && DateTime.ParseExact(a.end_of_month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture) <= endDate).ToList();

                    Assert.AreEqual(10, records.Count());
                }

            }
        }
    }
}
