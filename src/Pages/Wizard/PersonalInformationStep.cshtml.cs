namespace ServerWizardExample.Pages.Wizard
{
    public class PersonalInformationStep : StepViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonalInformationStep()
        {
            Position = 0;
        }
    }
}