﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HouseHub.Extensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ImageFileAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public ImageFileAttribute() : this("png,jpg,jpeg,gif")
        {
        }

        public ImageFileAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            ErrorMessage = "Unsupported file extension. Please use one of the following: " + fileExtensions;
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                var fileName = file.FileName;

                return AllowedExtensions.Any(y => fileName.ToLower().EndsWith("." + y));
            }

            return true;
        }
    }
}
