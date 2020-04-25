namespace ServerWizardExample.Pages.Wizard
{
    public class ContactInformationStep : StepViewModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public ContactInformationStep()
        {
            Position = 1;
        }
    }
}