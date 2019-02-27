using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Filter
    {
        public double MinAge { get; set; }
        public double MaxAge { get; set; }
        public string Gender { get; set; }

        public Filter()
        {

        }

        public Filter(double minAge,double maxAge,string gender)
        {
            MinAge = minAge;
            MaxAge = maxAge;
            Gender = gender;
        }          
    }
}