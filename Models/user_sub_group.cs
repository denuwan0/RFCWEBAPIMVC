//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RFCWEBAPIMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_sub_group
    {
        public int group_id { get; set; }
        public int sub_group_id { get; set; }
        public string sub_group_name { get; set; }
        public string sub_group_desc { get; set; }
        public byte is_active { get; set; }
    }
}