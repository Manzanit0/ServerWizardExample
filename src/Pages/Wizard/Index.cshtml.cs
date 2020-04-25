using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ServerWizardExample.Contacts;

namespace ServerWizardExample.Pages.Wizard
{
    public class Index : PageModel
    {
        // Regarding cleaning the ModelState:
        // https://stackoverflow.com/questions/54356921/razor-views-bounded-property-not-updating-after-post

        [BindRequired]
        [BindProperty(SupportsGet = true)]
        public int CurrentStepIndex { get; set; }

        public IList<StepViewModel> Steps { get; set; }
        
        private readonly ContactService _service;

        public Index(ContactService service)
        {
            _service = service;
            InitializeSteps();
        }

        private void InitializeSteps()
        {
            Steps = typeof(StepViewModel)
                .Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(StepViewModel).IsAssignableFrom(t))
                .Select(t => (StepViewModel) Activator.CreateInstance(t))
                .OrderBy(x => x.Position)
                .ToList();
        }

        public IActionResult OnGetAsync(int? id)
        {
            if (id != null)
            {
                var client = _service.FindById((int) id);
                if (client != null)
                {
                    LoadWizardData(client);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                SetEmptyTempData();
            }

            return Page();

        }

        public PageResult OnPostNext(StepViewModel currentStep)
        {
            if (ModelState.IsValid) MoveToNextStep(currentStep);
            return Page();
        }

        public PageResult OnPostPrevious(StepViewModel currentStep)
        {
            if (ModelState.IsValid) MoveToPreviousStep(currentStep);
            return Page();
        }

        public IActionResult OnPostFinish(StepViewModel currentStep)
        {
            if (!ModelState.IsValid) return Page();

            var client = ProcessSteps(currentStep);
            _service.Save(client);
            return RedirectToPage("./../Index", new {id = client.Id});
        }

        private void LoadWizardData(Contact client)
        {
            TempData["ClientId"] = client.Id;
            
            Steps = StepMapper.ToSteps(client).OrderBy(x => x.Position).ToList();
            
            for (var i = 0; i < Steps.Count; i++)
            {
                TempData.Set($"Step{i}", Steps[i]);
            }
        }

        private void SetEmptyTempData()
        {
            TempData.Remove("ClientId");
            for (var i = 0; i < Steps.Count; i++)
            {
                TempData.Set($"Step{i}", Steps[i]);
            }
        }

        private void MoveToNextStep(StepViewModel currentStep) => JumpToStep(currentStep, CurrentStepIndex + 1);

        private void MoveToPreviousStep(StepViewModel currentStep) => JumpToStep(currentStep, CurrentStepIndex - 1);

        private void JumpToStep(StepViewModel currentStep, int nextStepPosition)
        {
            TempData.Set($"Step{CurrentStepIndex}", currentStep);
            CurrentStepIndex = nextStepPosition;
            JsonConvert.PopulateObject((string) TempData.Peek($"Step{CurrentStepIndex}"), Steps[CurrentStepIndex]);
            ModelState.Clear();
        }

        private Contact ProcessSteps(StepViewModel finalStep)
        {
            for (var i = 0; i < Steps.Count; i++)
            {
                var data = TempData.Peek($"Step{i}");
                JsonConvert.PopulateObject((string) data, Steps[i]);
            }

            Steps[CurrentStepIndex] = finalStep;

            var contact = new Contact();
            if (TempData.Peek("ClientId") != null)
            {
                contact.Id = (int) TempData["ClientId"];
            }
    
            StepMapper.EnrichClient(contact, Steps);
            return contact;
        }
    }
}