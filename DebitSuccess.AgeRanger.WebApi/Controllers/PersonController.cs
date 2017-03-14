using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace DebitSuccess.AgeRanger.WebApi.Controllers {
    public class PersonController : ApiController {

        ILogger _serviceLogger;
        IPeopleDataProvider _dataProvider;

        public PersonController() {

            // Not, strictly speaking DI, since we're self-injecting, but the only dependency is the factory
            // so we'll let it go for now. It's a pattern that's sometimes hard to avoid without tying yourself
            // up in knots - I still haven't found a satisfactory (!) alternative for injecting DataContext
            // (ViewModels) into Views without adopting a full/over-blown framework that insists I frame my
            // application architecture to fit their model (eg., Prism)
            _serviceLogger = ContainerFactory.ResolveInstance<ILogger>();
            _dataProvider = ContainerFactory.ResolveInstance<IPeopleDataProvider>();
        }

        /// <summary>
        /// Potentially unfiltered list of each person on the database. 
        /// No limits applied (as per instructions), so this could get quite large
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<IPersonEntity> List() {
            
            var filter = BuildFilterFromUri();
            return _dataProvider.ListPeople(filter);

        }

        [HttpGet]
        public IPersonEntity GetByID(int ID) {
            return _dataProvider.GetPerson(ID);
        }

        /// <summary>
        /// Update an existing Person
        /// </summary>
        /// <param name="person"></param>
        [HttpPut]
        public void Update([FromBody] PersonEntity person) {

            if(person == null) {
                // Yes, I know ...
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }

            _dataProvider.UpdatePeople(new List<IPersonEntity>() { person });

        }
        
        /// <summary>
        /// Create a new Person
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void Create([FromBody] PersonEntity person) {

            if (person == null) {
                // Yes, I know ...
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }

            _dataProvider.CreatePerson(person);
        }

        [HttpDelete]
        public void Delete(int ID) {
            _dataProvider.DeletePerson(ID);
        }

        private IPeopleFilter BuildFilterFromUri() {

            var filter = new PeopleFilter();

            var query = Request.RequestUri.Query;
            var parmValues = System.Web.HttpUtility.ParseQueryString(query);

            filter.FirstName = parmValues.Get("FirstName");
            filter.LastName = parmValues.Get("LastName");

            int n;

            string Value = parmValues.Get("MinAge");
            
            if (int.TryParse(Value, out n)) {
                filter.MinAge = n;
            }

            Value = parmValues.Get("MaxAge");

            if (int.TryParse(Value, out n)) {
                filter.MaxAge = n;
            }


            return filter;

        }



    }
}