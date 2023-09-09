using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieAPI.Application.Extensions
{
    public static class ValidationResultExtension
    {
        public static ModelStateDictionary SendErrosAsValidationProblem(this ValidationResult validationResult)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var failure in validationResult.Errors)
            {
                modelStateDictionary.AddModelError(
                    failure.PropertyName,
                    failure.ErrorMessage);
            }
            return modelStateDictionary;
        }
    }
}
