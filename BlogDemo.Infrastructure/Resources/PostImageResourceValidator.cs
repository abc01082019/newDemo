using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    /// <summary>
    /// Use third part library (FulentValidation) for validation
    /// </summary>
    public class PostImageResourceValidator: AbstractValidator<PostImageResource>
    {
        public PostImageResourceValidator()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .WithName("Filnavn")
                .WithMessage("required|{PropertyName} skal være udfyldt")
                .MaximumLength(100)
                .WithMessage("maxlength|{PropertyName}'s navn må max være {MaxLength} tegn langt. ");
        }
    }
}
