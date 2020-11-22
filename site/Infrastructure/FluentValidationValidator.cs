using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace site.Infrastructure
{

    /*
     * Reference; https://blazor-university.com/forms/writing-custom-validation/
     * Thank you CopperBeardy for the suggestion
     *
     */
    public class FluentValidationValidator : ComponentBase
    {
        [CascadingParameter]
        private EditContext EditContext { get; set; }

        [Parameter]
        public Type ValidatorType { get; set; }

        private IValidator Validator { get; set; }
        private ValidationMessageStore MessageStore { get; set; }

        [Inject]
        private IServiceProvider ServiceProvider { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            EditContext previousEditContext = EditContext;

            Type previousValidatorType = ValidatorType;

            await base.SetParametersAsync(parameters);

            if (EditContext == null)
                throw new NullReferenceException($"{nameof(FluentValidationValidator)} must be placed within an {nameof(EditForm)}");

            if (!typeof(IValidator).IsAssignableFrom(ValidatorType))
            {
                throw new ArgumentException($"{ValidatorType.Name} must implement {typeof(IValidator).FullName}");
            }

            if (ValidatorType != previousValidatorType)
            {
                ValidatorTypeChanged();
            }

            if (EditContext != previousEditContext)
                EditContextChanged();
        }

        private void ValidatorTypeChanged()
        {
            Validator = (IValidator)ServiceProvider.GetService(ValidatorType);
        }

        void EditContextChanged()
        {
            MessageStore = new ValidationMessageStore(EditContext);

            ConnectEditContextEvents();
        }

        private void ConnectEditContextEvents()
        {
            EditContext.OnValidationRequested += ValidationRequested;
            EditContext.OnFieldChanged += FieldChanged;
        }

        private async void ValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            MessageStore.Clear();

            var validationContext =
                new ValidationContext<object>(EditContext.Model);

            ValidationResult result =
                await Validator.ValidateAsync(validationContext);

            AddValidationResult(EditContext.Model, result);
        }

        private void AddValidationResult(object model, ValidationResult validationResult)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);

                MessageStore.Add(fieldIdentifier, error.ErrorMessage);
            }
            EditContext.NotifyValidationStateChanged();
        }

        private async void FieldChanged(object sender, FieldChangedEventArgs args)
        {
            FieldIdentifier fieldIdentifier = args.FieldIdentifier;

            MessageStore.Clear(fieldIdentifier);

            var propertiesToValidate = new[] { fieldIdentifier.FieldName };

            var fluentValidationContext =
                new ValidationContext<object>(
                    instanceToValidate: fieldIdentifier.Model,
                    propertyChain: new FluentValidation.Internal.PropertyChain(),
                    validatorSelector: new FluentValidation.Internal.MemberNameValidatorSelector(propertiesToValidate)
                );

            ValidationResult result = await Validator.ValidateAsync(fluentValidationContext);

            AddValidationResult(fieldIdentifier.Model, result);
        }
    }
}