namespace websitebenhvien.Helper
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInBytes;
        public MaxFileSizeAttribute(int maxFileSizeInMB)
        {
            _maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > _maxFileSizeInBytes)
            {
                return new ValidationResult($"Kích thước file tối đa là {_maxFileSizeInBytes / (1024 * 1024)}MB.");
            }
            return ValidationResult.Success;
        }
    }
}

