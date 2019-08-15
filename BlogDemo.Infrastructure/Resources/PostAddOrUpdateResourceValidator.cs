using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    /// <summary>
    /// Set validation for the data-resources using FluentValidation-package
    /// </summary>
    public class PostAddOrUpdateResourceValidator<T>: AbstractValidator<T> where T: PostAddOrUpdateResource
    {
        public PostAddOrUpdateResourceValidator()
        {
            RuleFor(r => r.Title)
                .NotNull()
                .WithName("Titel")
                .WithMessage("required|{PropertyName} skal være udfyldt")
                .MaximumLength(30)
                .WithMessage("minlength|{PropertyName}s navn må max være 30 tegn langt.");

            RuleFor(r => r.Body)
                .NotNull()
                .WithName("Indhold")
                .WithMessage("require|{PropertyName} skal være udfyldt")
                .MinimumLength(10)
                .WithMessage("minlength|{PropertyName}s navn skal minimum være 10 tegn langt.");

        }
    }
}
