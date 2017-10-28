using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3DProject.Models.data;

namespace _3DProject.Filters
{
    public class LogVM
    {
        private static List<LogTable> _loglar = new List<LogTable>();

        public static List<LogTable> Loglar
        {
            get { return _loglar; }

        }
    }
}