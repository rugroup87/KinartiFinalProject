using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Hobbies
    {
        public string Hname { get; set; }

        public Hobbies(string _hname)
        {
            this.Hname = _hname;
        }
        public Hobbies()
        {

        }
        public List<Hobbies> Read()
        {
            DBservices dbs = new DBservices();
            List<Hobbies> LH = new List<Hobbies>();
            LH = dbs.ReadHobbies();
            return LH;
        }
    }
}