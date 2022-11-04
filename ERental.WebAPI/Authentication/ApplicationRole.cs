using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ERental.WebAPI.Authentication
{
    public class ApplicationRole:IdentityRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
