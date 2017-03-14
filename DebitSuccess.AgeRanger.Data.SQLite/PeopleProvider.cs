using DebitSuccess.AgeRanger.Api;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Data.SQLite {
    public class PeopleProvider : DataProviderBase, IPeopleDataProvider {

        private AgeGroup _agegroup = new AgeGroup();

        public IEnumerable<IPersonEntity> ListPeople() {
            return ListPeople(null);
        }

        public IPersonEntity GetPerson(int ID) {

            var person = new PersonEntity();

            // No need to worry about SQL Injection ... right?
            var sql = $" SELECT * FROM Person WHERE ID = {ID}";
            var dt = ExecuteQuery(sql);

            // Q: How would this happen without an exception?
            // A: Someone changed the ExecuteQuery method
            if (dt == null || dt.Rows.Count == 0) {
                throw new Exception($"An empty dataset was returned from the query: {sql}");
            }

            DataRow row = dt.Rows[0];

            person.ID = DBHelper.GetDBValue(row["ID"], 0);
            person.FirstName = DBHelper.GetDBValue(row["FirstName"], string.Empty);
            person.LastName = DBHelper.GetDBValue(row["LastName"], string.Empty);
            person.Age = DBHelper.GetDBValue(row["Age"], 0);

            person.AgeRange = _agegroup.Get(person.Age);

            return person;

        }

        public IEnumerable<IPersonEntity> ListPeople(IPeopleFilter filter) {

            var people = new List<IPersonEntity>();

            // For most databases this would be call to a Stored Procedure (or a View) with the SPROC handling the filtering
            // and the age group.
            // But here - particularly since we're allowing unfiltered lists - just do a simple
            // SELECT * and do the filtering with LINQ. 
            var sql = " SELECT * FROM Person ";
            var dt = ExecuteQuery(sql);

            // Q: How would this happen without an exception?
            // A: Someone changed the ExecuteQuery method
            if (dt == null) {
                throw new Exception($"An empty dataset was returned from the query: {sql}");
            }

            foreach (DataRow row in dt.Rows) {

                var person = new PersonEntity();

                person.ID = (int)((Int64)row["ID"]);
                person.FirstName = row["FirstName"].ToString();
                person.LastName = row["LastName"].ToString();
                person.Age = (int)((Int64)row["Age"]);

                person.AgeRange = _agegroup.Get(person.Age);

                people.Add(person);
            }

            if (filter != null) {
                people = people.Where(p => (filter.FirstName == null || p.FirstName.StartsWith(filter.FirstName, StringComparison.CurrentCultureIgnoreCase)) &&
                                           (filter.LastName == null || p.LastName.StartsWith(filter.LastName, StringComparison.CurrentCultureIgnoreCase)) &&
                                           (filter.MinAge == null || filter.MinAge <= p.Age) &&
                                           (filter.MaxAge == null || filter.MaxAge > p.Age)).ToList();
            }


            return people;

        }

        // Decided to go with a cached version.
        private string CalculateAgeRange_deprecated(int age) {

            // This would normally be cached (SELECT * FROM ...) - at least on an hourly basis.
            // or a SPROC (which would make cacheing harder)


            try {

                // Only an INT parameter, so no need for anti-sql injection measures,
                // but we would use @Parms if passing in strings
                var sql = " SELECT description FROM AgeGroup " +
                          $" WHERE IFNULL(minAge, 0) <= {age} " +
                          $" AND IFNULL(maxAge, {age} + 1) > {age} ";
                var dt = ExecuteQuery(sql);


                return dt.Rows[0]["description"].ToString();
            } catch {
                // Some change to the database caused this to fail?
                // I'm choosing not to raise the error, but return "Unavailable"
                // We could have injected an ILogger instance ... but this is eating enough of my weekend as it is :)
                return "Unavailable";
            }

        }

        public int UpdatePeople(IEnumerable<IPersonEntity> people) {

            // Normally we would do this in a sproc ...

            foreach(var person in people) {
                string sql = "UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE ID = @ID ";
                var parms = new List<SQLiteParameter>();
                parms.Add(new SQLiteParameter("@ID", person.ID));
                parms.Add(new SQLiteParameter("@FirstName", person.FirstName));
                parms.Add(new SQLiteParameter("@LastName", person.LastName));
                parms.Add(new SQLiteParameter("@Age", person.Age));
                ExecuteNonQuery(sql, parms);
            }

            return people.Count();
        }

       
        /// <summary>
        /// Create a new person. Normally SPROC time and we would return the ID - not sure
        /// if SQLite supports peeking identity columns, and the day is drawing
        /// to a close.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public int CreatePerson(IPersonEntity person) {

            string sql = "INSERT INTO Person (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) ";
            var parms = new List<SQLiteParameter>();
            parms.Add(new SQLiteParameter("@FirstName", person.FirstName));
            parms.Add(new SQLiteParameter("@LastName", person.LastName));
            parms.Add(new SQLiteParameter("@Age", person.Age));
            ExecuteNonQuery(sql, parms);
            return 1;
        }

        public int DeletePerson(int ID) {

            try {
                var sql = $"DELETE FROM Person WHERE ID = {ID}";
                ExecuteNonQuery(sql, null);
                return 1;
            } catch {
                // TODO: Logging needed here
                return 0;
            }

        }

        // Current just here for testability
        public string GetAgeGroup(int age) {
            return _agegroup.Get(age);
        }

    }
}
