using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HouseHub.Model
{
    public class UserMap
    {
        public string Role { get; set; }
        public IList<SelectListItem> Roles { get; set; }
        public IList<ApplicationUser> Users { get; set; }

        public UserMap(string role, IList<SelectListItem> roles, IDictionary<String, IList<ApplicationUser>> users)
        {
            Role = role;
            Users = users[role];
            Roles = roles;
        }
    }
}
