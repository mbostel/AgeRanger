using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DebitSuccess.AgeRanger.WebApi {

    public class PersonEntity : IPersonEntity {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string AgeRange { get; set; }
    }
}