
using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.WebApi.IntegrationTests {

    public class PeopleFilter : IPeopleFilter {
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
