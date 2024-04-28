using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CodinaxProjectMvc.CustomValidatorAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnlikeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "The value of {0} cannot be the same as the value of the {1}.";

        public string OtherProperty { get; private set; }

        public UnlikeAttribute(string otherProperty)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
                var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                var otherPropertyDisplayName = GetPropertyDisplayName(otherPropertyInfo);

                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(string.Format(ErrorMessageString,
                        validationContext.DisplayName, otherPropertyDisplayName));
                }
            }

            return ValidationResult.Success;
        }

        private string GetPropertyDisplayName(System.Reflection.PropertyInfo propertyInfo)
        {
            var displayNameAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault() as DisplayNameAttribute;

            return displayNameAttribute != null ? displayNameAttribute.DisplayName : propertyInfo.Name;
        }
    }
}
