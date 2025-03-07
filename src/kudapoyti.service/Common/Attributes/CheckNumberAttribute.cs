﻿using System.ComponentModel.DataAnnotations;

namespace kudapoyti.Service.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            double result = 0;
            if (value is not null && double.TryParse(value.ToString(), out result))
            {
                if (result <= 0)
                    return new ValidationResult("Must be given");
                return ValidationResult.Success;
            }
            return new ValidationResult("Must contain only number!");
        }
    }
}
