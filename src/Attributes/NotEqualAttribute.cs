using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace mmmsl.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotEqualAttribute : ValidationAttribute
    {
        public NotEqualAttribute(string otherProperty)
        {
            if (otherProperty == null) {
                throw new ArgumentNullException(nameof(otherProperty));
            }

            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (Equals(value, otherPropertyValue)) {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}