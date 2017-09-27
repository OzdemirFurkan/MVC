using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCOdev
{
    public class SlugService
    {
        public static string Slug(string name)
        {
            return name.Replace("ş", "s").Replace("ğ", "g").Replace(" ", "-").Replace("ç", "c").Replace("ö", "o").Replace("ü", "u").Replace("ı", "i").Replace("?", "");
        }
    }
}