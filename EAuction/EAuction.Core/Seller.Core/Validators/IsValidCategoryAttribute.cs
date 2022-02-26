using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable enable

namespace EAuction.Seller.Core.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IsValidCategory : ValidationAttribute
    {
        private readonly IList<string> values;

        public IsValidCategory()
        {
            this.values = new List<string>() { "Painting", "Ornament", "Scluptor" };
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"Value should exists & fall in anyone of the following category - { String.Join(',', values) }");
            if (!this.values.Any(i => i.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                return new ValidationResult($"Value should fall in anyone of the following category - { String.Join(',', values) }");
            }
            return null;
        }
    }
}