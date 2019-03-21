using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Assessment.ViewModel
{
    public class SearchViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}