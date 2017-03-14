using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DebitSuccess.AgeRanger.Data.SQLServer {
    public class PeopleDataProvider : IPeopleDataProvider {

        public int CreatePerson(IPersonEntity person) {
            throw new NotImplementedException();
        }

        public int DeletePerson(int ID) {
            throw new NotImplementedException();
        }

        public string GetAgeGroup(int age) {
            throw new NotImplementedException();
        }

        public IPersonEntity GetPerson(int ID) {
            throw new NotImplementedException();
        }

        public IEnumerable<IPersonEntity> ListPeople() {
            return ListPeople(null);
        }

        public IEnumerable<IPersonEntity> ListPeople(IPeopleFilter filter) {
            throw new NotImplementedException();
        }

        public int UpdatePeople(IEnumerable<IPersonEntity> people) {
            throw new NotImplementedException();
        }


    }
}
