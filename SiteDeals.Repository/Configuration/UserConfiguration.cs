﻿//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using SiteDeals.Core.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SiteDeals.Repository.Configuration
//{
//    public class UserConfiguration : IEntityTypeConfiguration<User>
//    {
//        public void Configure(EntityTypeBuilder<User> builder)
//        {
//            builder.HasKey(x=> x.Id); 
//            builder.Property(x=>x.Id).UseIdentityColumn();
//            builder.Property(x=>x.UserName).IsRequired();   
//            builder.Property(x=>x.Email).IsRequired();  
//            builder.Property(x=>x.PasswordHash).IsRequired();
//            builder.Property(x => x.CreatedAt).IsRequired();
//            builder.Property(x=>x.IsDeleted).IsRequired();
//        }
//    }
//}
