using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerWizardExample.Contacts;

namespace ServerWizardExample.Pages.ContactDirectory
{
    public class Index : PageModel
    {
        private readonly ContactService _service;
        public IList<Contact> Contacts { get; set; }

        public void OnGet()
        {
            Contacts = _service.All();
        }

        public Index(ContactService service)
        {
            _service = service;
        }
    }
}