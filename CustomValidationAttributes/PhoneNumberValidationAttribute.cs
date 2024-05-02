using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CodinaxProjectMvc.DataList;

namespace CodinaxProjectMvc.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "This Phone Number Code Does Not Exists !";

        public PhoneNumberValidationAttribute() : base(DefaultErrorMessage) { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string? phoneNumber = value as string;

                bool is_phone_valid = false;

                foreach (Country data in Countries.All) 
                {
                    if (phoneNumber == $"+{data.Phone}")
                    {
                        return new ValidationResult("Phone number is required.");
                    }
                    else if (phoneNumber.Contains($"+{data.Phone}"))
                    {
                        phoneNumber = phoneNumber.Replace($"+{data.Phone}", "");

                        try
                        {
                            int converted_phoneNumber = Int32.Parse(phoneNumber);
                        }
                        catch (FormatException)
                        {
                            return new ValidationResult("Letters are not allowed in phone number field.");
                        }
                        catch(OverflowException)
                        {
                            try
                            {
                                Int64 converted_phoneNumber = Int64.Parse(phoneNumber);
                            }
                            catch (FormatException)
                            {
                                return new ValidationResult("Letters are not allowed in phone number field.");
                            }
                        }
                        finally
                        {
                            is_phone_valid = true;
                        }
                    }
                }

                if (!is_phone_valid)
                {
                    return new ValidationResult(ErrorMessageString);
                }




            }

            return ValidationResult.Success;
        }
    }
}
