using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookList.Models
{
    public class UserLoginLog
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}