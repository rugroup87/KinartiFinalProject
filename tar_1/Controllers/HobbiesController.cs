using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
//using WebApplication2.Models;

namespace WebApplication1.Controllers
{

    public class HobbiesController : ApiController
    {


        [HttpGet]
        [Route("api/hobbies")]
        public IEnumerable<Hobbies> Get()
        {
            Hobbies H = new Hobbies();
            List<Hobbies> hobbieslist = new List<Hobbies>();
            hobbieslist = H.Read();
            return hobbieslist;
        }

    }
}
