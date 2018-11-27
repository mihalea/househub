using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HouseHub
{
    public static class Operations
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement()
        {
            Name = Constants.CreateOperationName
        };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";

        public static readonly string Admin = "Admin";
        public static readonly string Officer = "Officer";
        public static readonly string Landlord = "Landlord";
    }
}
