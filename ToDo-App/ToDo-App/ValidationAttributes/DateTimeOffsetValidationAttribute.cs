using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;
using ToDo_App.Services;

namespace ToDo_App.ValidationAttributes
{
    public class DateTimeOffsetValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTimeOffset dt;

            try
            {
                dt = (DateTimeOffset)value;
            }
            catch (Exception)
            {
                return new ValidationResult(GetErrorMessage(validationContext));
            }

            if (dt < DateTimeOffset.Now)
            {
                return new ValidationResult(GetErrorMessage(validationContext));
            }

            return ValidationResult.Success;
        }


        private string GetErrorMessage(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return "Invalid data";
            }

            ErrorMessageTranslationService errorTranslation = validationContext.GetService(typeof(ErrorMessageTranslationService)) as ErrorMessageTranslationService;
            return errorTranslation.GetLocalizedError(ErrorMessage);
        }
    }
}
