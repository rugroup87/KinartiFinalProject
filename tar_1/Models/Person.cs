using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public float Age { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        //public string Image { get; set; }
        public int[] Hobbies { get; set; }
        public bool IsPremium { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        private string image;
        public string Image
        {
            get
            {
                if (image == null)
                {
                     return "https://www.aminz.org.nz/themes/portal/uploads/profile-default-large.jpg";
                }
                else
                {
                    return image;
                }
            }
            set
            {
                image = value;
            }
        }

        public Person(int _id, string _name, string _familyname, float _age, float _height, string _gender, string _address, int[] _hobbies, bool _ispremium, bool _isactive, string _phone, string _email, string _password, string _image = "https://www.aminz.org.nz/themes/portal/uploads/profile-default-large.jpg")
        {
            this.ID = _id;
            this.Name = _name;
            this.FamilyName = _familyname;
            this.Age = _age;
            this.Height = _height;
            this.Gender = _gender;
            this.Address = _address;
            this.Image = _image;
            this.Hobbies = _hobbies;
            this.IsActive = _isactive;
            this.IsPremium = _ispremium;
            this.Phone = _phone;
            this.Password = _password;
            this.Email = _email;
        }
        public Person()
        {
           
        }
        public int Insert()
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.Insert(this);
            return numAffected;
        }

        //public List<Person> Filter(Filter f)
        //{
        //    DBservices dbs = new DBservices();
        //    List<Person> lp = new List<Person>();
        //    lp= dbs.Filter(f);
        //    return lp;
        //}

        public List<Person> GetAllPersons()
        {
            DBservices dbs = new DBservices();
            List<Person> lp = new List<Person>();
            lp = dbs.GetAllPersons();
            return lp;
        }

        public void ActivityChange(int activity, int PersonId)
        {
            DBservices dbs = new DBservices();
            dbs.ActivityChange(activity, PersonId);
        }

        public string UserValidation(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.UserValidation(email, password);
        }

        public Person GetUserDetailsByEmail(string email)
        {
            DBservices dbs = new DBservices();
            return dbs.GetUserDetailsByEmail(email);
        }
        public int UpdatePerson()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdatePerson(this);
        }
    }
}