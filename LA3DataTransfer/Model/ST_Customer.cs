//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LA3DataTransfer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ST_Customer
    {
        public int cust_id { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string address { get; set; }
        public string notes { get; set; }
        public Nullable<int> collector_id { get; set; }
        public Nullable<int> week_day { get; set; }
        public Nullable<double> max_loan { get; set; }
        public Nullable<System.DateTime> cust_date { get; set; }
        public Nullable<decimal> debt { get; set; }
        public string telephone { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public bool Locked { get; set; }
    }
}
