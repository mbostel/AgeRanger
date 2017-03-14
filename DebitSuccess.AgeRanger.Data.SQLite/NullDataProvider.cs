using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DebitSuccess.AgeRanger.Data.SQLite {

    /// <summary>
    /// This class may be used for unit tests
    /// </summary>
    public class NullDataProvider : IPeopleDataProvider {

        public int CreatePerson(IPersonEntity person) {
            return 1;
        }

        public int DeletePerson(int ID) {
            return 1;
        }

        public string GetAgeGroup(int age) {
            return string.Empty;
        }

        public IPersonEntity GetPerson(int ID) {
            return null;
        }

        public IEnumerable<IPersonEntity> ListPeople() {
            return ListPeople(null);
        }

        public IEnumerable<IPersonEntity> ListPeople(IPeopleFilter filter) {
            return new List<IPersonEntity>();
        }

        public int UpdatePeople(IEnumerable<IPersonEntity> people) {
            return (people == null) ? 0 : people.Count();
        }

    }
}
