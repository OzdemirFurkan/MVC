//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3DProject.Models.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BlogTable
    {
        public int blogID { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> blogCategoryID { get; set; }
        public string blogName { get; set; }
        public string blogContent { get; set; }
        public string blogImage { get; set; }
        public Nullable<System.DateTime> blogAddDate { get; set; }
        public Nullable<int> blogLikeID { get; set; }
    
        public virtual UserTable UserTable { get; set; }
    }
}
