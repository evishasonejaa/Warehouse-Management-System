﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WMS_dbEntities1 : DbContext
    {
        public WMS_dbEntities1()
            : base("name=WMS_dbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SKU> SKUs { get; set; }
        public virtual DbSet<tbl_asn> tbl_asn { get; set; }
        public virtual DbSet<tbl_client> tbl_client { get; set; }
    }
}
