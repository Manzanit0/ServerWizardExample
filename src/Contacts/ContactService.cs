using System.Collections.Generic;
using System.Linq;

namespace ServerWizardExample.Contacts
{
    public class ContactService
    {
        private IList<Contact> Contacts { get; set; }

        public ContactService()
        {
            Contacts = new List<Contact>();
        }
        
        public void Save(Contact contact) => Contacts.Add(contact);
        public Contact FindById(int id) => Contacts.FirstOrDefault(x => x.Id == id);
        public IList<Contact> All() => Contacts;
    }
}