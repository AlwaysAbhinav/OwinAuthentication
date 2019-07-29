using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAuthenticationDemo.Models
{
    //Identity DB context that points to the custom auth DB.
    //string is the base constructor is connection string to the DB.
    //Using EF to create teh DB Migrations
    public class OwinAuthDbContext : IdentityDbContext
    {
        public OwinAuthDbContext() 
            : base("OwinAuthDbContext")
        { 
}
    }
}