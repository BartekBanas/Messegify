using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Messegify.Application.Errors;
using Microsoft.AspNetCore.Authorization;

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