using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmsShoppingCart.Infrastructure
{
    public class FileExtensionAttribute: ValidationAttribute
    {

        string[] extensions = new[] { "jpg", "png" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if(file!=null)
            {
                var extension = Path.GetExtension(file.FileName);

              

                bool result = extensions.Any(x => extension.EndsWith(x));

                if(!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }

                
            }
            return ValidationResult.Success;

        }

        private string GetErrorMessage()
        {
            return $"Allowed extensions are: {string.Join(",", extensions)}";
        }
    }
}
