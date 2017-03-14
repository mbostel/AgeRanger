using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    public interface IPeopleFilter {

        int? MinAge { get; set; }
        int? MaxAge { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }

    }
}
