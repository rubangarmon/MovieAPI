using FluentValidation;
using MovieAPI.Application.ContractsModels;

namespace MovieAPI.Application.Validators;

public class MediaRequestValidator : AbstractValidator<MediaRequest>
{
    public MediaRequestValidator() 
    { 
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Page)
            .GreaterThan(0);
    }
}
