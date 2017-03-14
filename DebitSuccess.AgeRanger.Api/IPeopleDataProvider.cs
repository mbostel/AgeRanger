using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    public interface IPeopleDataProvider {

        IEnumerable<IPersonEntity> ListPeople(IPeopleFilter filter);
        IEnumerable<IPersonEntity> ListPeople();
        IPersonEntity GetPerson(int ID);
        int UpdatePeople(IEnumerable<IPersonEntity> people);
        int CreatePerson(IPersonEntity person);
        int DeletePerson(int ID);
        string GetAgeGroup(int age);
    }
}
