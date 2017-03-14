using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Data.SQLite {
    internal class GroupEntity {

        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string Description { get; set; }

    }
}
