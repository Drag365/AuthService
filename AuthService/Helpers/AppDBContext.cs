﻿using AuthService.Models.Logs;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Helpers
{
    public class AppDBContext : IdentityDbContext<IdentityUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<LogsModel> logs { get; set; }
        public DbSet<FileUploadMetadata> FileUploads { get; set; }
    }
}
