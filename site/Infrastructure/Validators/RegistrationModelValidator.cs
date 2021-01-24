using System.Linq;
using FluentValidation;
using site.Pages.Registration.Models;

namespace site.Infrastructure.Validators
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .NotEmpty()
                .WithMessage("A name is required in order to identify the streamer");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(
                    "The description is displayed under name and helps people find streamers they are interested in.");

            RuleFor(x => x.Platforms)
                .NotEmpty()
                .WithMessage("You must register at least one platform for a streamer");

            RuleFor(x => x.Platforms)
                .Must(collection => collection.All(platform => !platform.AlreadyRegistered))
                .WithMessage("You can not have the same url registered under multiple streamers");
        }
    }
}