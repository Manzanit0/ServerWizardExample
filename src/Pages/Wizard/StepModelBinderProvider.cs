using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ServerWizardExample.Pages.Wizard
{
    public class StepModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(StepViewModel))
            {
                return null;
            }

            // TODO do this with reflection.
            var subclasses = new[]
            {
                typeof(PersonalInformationStep),
                typeof(ContactInformationStep),
            };

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();

            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new StepModelBinder(binders);
        }
    }
}