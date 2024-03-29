﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HouseHub.Model
{
    public class Accommodation
    {       
        public int AccommodationID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Approved { get; set; }
        public bool Pending { get; set; }
        public string Reason { get; set; }
        public string OwnerID { get; set; }

        public Accommodation()
        {
            Pending = true;
            Approved = false;
        }
    }
}
