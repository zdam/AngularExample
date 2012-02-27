using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AngularExample.Infrastructure;

namespace AngularExample.Controllers
{
    public class ExampleController : Controller
    {
        private DataContext context = new DataContext();

        // GET /Example
        // Return all persons.
        [EnableJson, EnableXml]
        [HttpGet, OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public virtual ActionResult Index()
        {
            var data = context.Persons;
                // this approach sends the list, with all it's relationships!!! Only suitable when prototyping

            return View(data.ToList());
        }

        // POST /Example
        // Add a new person.
        [EnableJson, EnableXml]
        [HttpPost, OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        [ActionName("Index")]
        public virtual ActionResult AddNew(Person newPerson)
        {
            newPerson.Id = Guid.NewGuid();

            context.Persons.Add(newPerson);
            context.SaveChanges();

            return RedirectToAction("SingleEntity", new {entityId = newPerson.Id});
        }

        // GET /Example/someGuid
        // Return a single customer data.
        [EnableJson, EnableXml]
        [HttpGet, OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public virtual ActionResult SingleEntity(Guid entityId)
        {
            Person person = context.Persons.FirstOrDefault(x => x.Id == entityId);

            if (person == null)
                return new HttpNotFoundResult("Person with ID: " + entityId + " not found");

            return View("SinglePerson", person);
        }

        // DELETE /Example/SomeGuid
        // Delete a single person.
        [EnableJson, EnableXml]
        [HttpDelete]
        [ActionName("SingleEntity")]
        public virtual ActionResult SingleEntityDelete(Guid entityId)
        {
            var found = context.Persons.FirstOrDefault(x => x.Id == entityId);
            if (found != null)
            {
                context.Persons.Remove(found);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Example");
        }

        // POST /Example/SomeGuid(?verb=Delete)
        // Update/Delete a single person
        [HttpPost]
        [EnableJson, EnableXml]
        public virtual ActionResult SingleEntity(Person changePerson, Guid entityId)
        {
            if (ModelState.IsValid)
            {
                var existing = context.Persons.FirstOrDefault(x => x.Id == entityId);
                if (existing != null)
                {
                    existing.FirstName = changePerson.FirstName;
                    existing.LastName = changePerson.LastName;
                    context.SaveChanges();
                }

                ViewData["Message"] = "Saved";
                return SingleEntity(entityId);
            }
            else
            {
                // really we should encode our errors as part of some well known json structure that the client knows to interrogate.
                var err = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in err)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
                throw new ApplicationException("Invalid model state");
            }
        }
    }
}

