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

        public static readonly string AdminRole = "Admin";
        public static readonly string OfficerRole = "Officer";
        public static readonly string LandlordRole = "Landlord";
    }
}
