using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class PersonController : ApiController
    {

        //POST api/values
        //public void Post([FromBody]string value)
        public int Post([FromBody]Person p)
        {
            int num = p.Insert();
            return num;
            // enter your code here
        }


        //[HttpPost]
        //[Route("api/person/filter")]
        //public IEnumerable<Person> useFilter(Filter filter)
        //{
        //    Person p = new Person();
        //    List<Person> personList = new List<Person>();
        //    personList = p.Filter(filter);

        //    //#region // This code is only used for example, change it with your own
        //    //List<Person> personList = new List<Person>();
        //    //personList.Add(new Person("bibi", "netanyahu", "male", 67, 175, "", "Jerusalem"));
        //    ////personList.Add(new Person("sara", "netanyahu", "female", 57, 165, "", "Jerusalem"));
        //    //personList.Add(new Person("rubi", "rivlin", "male", 75, 170, "", "Jerusalem"));
        //    //#endregion

        //    return personList;

        //}

        [HttpGet]
        [Route("api/persons")]
        public IEnumerable<Person> Get()
        {
            Person p = new Person();
            List<Person> personList = new List<Person>();
            personList = p.GetAllPersons();
            return personList;
        }

        [HttpGet]
        [Route("api/persons")]
        public string Get(string email, string password)
        {
            Person p = new Person(); 
            return p.UserValidation(email, password);
        }

        [HttpGet]
        [Route("api/persons")]
        public Person Get(string email)
        {
            Person p = new Person();
            return p.GetUserDetailsByEmail(email);
        }

        //1-active 0-non active
        [HttpPut]
        [Route("api/persons")]
        public void Put(int Active, int PersonId)
        {
            Person p = new Person();
            p.ActivityChange(Active, PersonId);
        }

        public void Put([FromBody]Person p)
        {  
            p.UpdatePerson();
        }

    }
}
