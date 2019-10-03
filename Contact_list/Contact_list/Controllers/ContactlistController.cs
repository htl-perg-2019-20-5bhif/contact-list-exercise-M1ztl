using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Contact_list.Controllers
{
    public class Person
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string email { get; set; }

        [MaxLength(50)]
        public string firstname { get; set; }
        public string lastname { get; set; }
      
    }

    [Route("api/contact-list")]
    [ApiController]
    public class ContactlistController : ControllerBase
    {
        public static int count = 0;

        private static readonly List<Person> contacts =
           new List<Person> {
                new Person{id = count, email = "asdd",firstname = "Lukas", lastname="mistl"}};
      

        [HttpGet]
        public IActionResult getAllPeople()
        {
            return Ok(contacts);
        }

        [HttpPost]
        public IActionResult addPerson([FromHeader] Person newPerson)
        {
            if (newPerson.id.Equals(null))
            {
                return BadRequest("no id entered");
            }
            if(newPerson.id.Equals(contacts.Find(person => person.id == newPerson.id)))
            {
                return BadRequest("Id already in use");
            }

            contacts.Add(newPerson);
            return Created("",newPerson);
        }

        [HttpDelete]
        [Route ("/{personId}", Name = "personId")]
        public IActionResult deletePerson(int id)
        {
           
            if (contacts.FindIndex(person => person.id == id).Equals(null)){
                return BadRequest("Person not found");
            }
            if(contacts.FindIndex(person => person.id == id) < 0)
            {
                return BadRequest("Invalid ID supplied");
            }

            contacts.RemoveAt(id);
            return NoContent();

        }

        [HttpGet]
        [Route("/findByName", Name = "nameFilter")]
        public IActionResult findPersonByName(string filter)
        {
            if (filter == null)
            {
                return BadRequest("Please enter a filter");
            }

            for (int i = 0; i < contacts.Count; i++)
            {
                
                if (contacts[i].firstname.ToUpper().Contains(filter.ToUpper()) || contacts[i].lastname.ToUpper().Contains(filter.ToUpper()))
                {
                    return Ok(contacts[i]);
                }
            }

            return BadRequest("Name not found");
        }

    }
}
