using FluentValidation;
using FluentValidation.Results;

namespace Messegify.Application.Service.Extensions;

public static class ValidatorExtensions
{
    public static async Task<ValidationResult> ValidateRequiredAsync<T>(this IValidator<T> validator, T instance)
    {
        var validationResult = await validator.ValidateAsync(instance);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return validationResult;
    }
}