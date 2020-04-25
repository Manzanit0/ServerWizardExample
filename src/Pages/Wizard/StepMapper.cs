using System.Collections.Generic;
using ServerWizardExample.Contacts;

namespace ServerWizardExample.Pages.Wizard
{
    internal static class StepMapper
    {
        public static void EnrichClient(Contact contact, IEnumerable<StepViewModel> steps)
        {
            foreach (var step in steps)
            {
                switch (step)
                {
                    case PersonalInformationStep s:
                        contact.FirstName = s.FirstName;
                        contact.LastName = s.LastName;
                        break;
                    case ContactInformationStep s:
                        contact.Email = s.Email;
                        contact.Phone = s.Phone;
                        break;
                }
            }
        }

        public static IEnumerable<StepViewModel> ToSteps(Contact contact)
        {
            return new List<StepViewModel>
            {
                new PersonalInformationStep {FirstName = contact.FirstName, LastName = contact.LastName},
                new ContactInformationStep {Email = contact.Email, Phone = contact.Phone}
            };
        }
    }
}