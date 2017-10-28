using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3DProject.Models.data;

namespace _3DProject
{
    public class CategoryList
    {
        public List<SelectListItem> listPCategory()
        {
            MyDb db = new MyDb();
            List<SelectListItem> listPCategory = new List<SelectListItem>();

            var PCategories = db.ProductCategoryTable.ToList();

            PCategories.ForEach(x =>
            {
                SelectListItem item1 = new SelectListItem();
                item1.Text = x.categoryName;
                item1.Value = x.pcategoryID.ToString();
                item1.Selected = x.pcategoryID == 1 ? true : false;

                listPCategory.Add(item1);
            });

            return listPCategory;
        }
    }
}