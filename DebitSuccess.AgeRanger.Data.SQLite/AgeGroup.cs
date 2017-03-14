using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Data.SQLite {
    internal class AgeGroup : DataProviderBase {

        private static List<GroupEntity> _groups;

        public AgeGroup() {

            // TODO: Implement a Cache Manager
            LoadCache();
        }

        private void LoadCache() {

            if (_groups != null) return;

            _groups = new List<GroupEntity>();

            string sql = "SELECT * FROM AgeGroup";
            var dt = ExecuteQuery(sql);

            foreach(DataRow row in dt.Rows) {

                var group = new GroupEntity();
                group.MinAge = DBHelper.GetDBValue(row["MinAge"], 0);
                group.MaxAge = DBHelper.GetDBValue(row["MaxAge"], int.MaxValue);
                group.Description = DBHelper.GetDBValue(row["Description"], "<Unknown>");

                _groups.Add(group);
            }

        }

        internal string Get(int Age) {

            var group = _groups.FirstOrDefault(g => g.MinAge <= Age && g.MaxAge > Age);
            return (group != null) ? group.Description :  "Unable to determine Age Group";

        }

    }
}
