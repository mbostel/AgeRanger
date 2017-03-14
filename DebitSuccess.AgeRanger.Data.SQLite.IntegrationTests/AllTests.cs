using DebitSuccess.AgeRanger.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Data.SQLite.IntegrationTests {

    /// <summary>
    /// Tests the People Data provider directly, without going through the API
    /// Arrange, act, assert
    /// </summary>


    [TestFixture]
    public class AllTests {

        PeopleProvider peopleProvider;

        [OneTimeSetUp]
        public void ContainerSetup() {
            new ContainerBootstrap().Initialise();
        }

        /// <summary>
        /// Create one person on an empty table
        /// Retrieve the person
        /// Check that the details are correct
        /// </summary>
        [Test]
        public void SQLite_CreatePerson_CheckCorrect() {

            TruncatePersonTable();

            // Check no one on the table
            var people = peopleProvider.ListPeople();
            Assert.AreEqual(0, people.Count());

            var person = GetNewPerson("Joe", "Smith", 40);
            peopleProvider.CreatePerson(person);

            people = peopleProvider.ListPeople();
            Assert.AreEqual(1, people.Count(), $"Expected 1 person, list contains {people.Count()}", null);

            if(people.Count() > 0) {
                var actualPerson = people.ToArray()[0];
                Assert.AreEqual(GetPersonString(person), GetPersonString(actualPerson), $"Expected person {person.FirstName} {person.LastName} {person.Age} but got {actualPerson.FirstName} {actualPerson.LastName} {actualPerson.Age}", null);
            }

            TruncatePersonTable();
            

        }


        /// <summary>
        /// Create 6 people
        /// Run several searches and test the results       
        /// </summary>
        [Test]
        public void SQLite_PersonSearch() {

            TruncatePersonTable();

            var person = GetNewPerson("John", "Smith", 40);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Josephine", "Smith", 30);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Jack", "Smith", 12);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Jill", "Smith", 3);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Jolly", "Rodger", 400);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Jimmy", "Riddle", 67);
            peopleProvider.CreatePerson(person);

            // First name starts with jO (searches are not case-sensitive and are partial)
            var people = peopleProvider.ListPeople(GetFilter("jO", string.Empty, 0, 500));
            // Expect 3 matches
            Assert.AreEqual(3, people.Count(), $"Expected 3 people, got {people.Count()}", null);

            // Last name starts with sMi  and first name starts with jO
            people = peopleProvider.ListPeople(GetFilter("jO", "sMi", 0, 500));
            // Expect 2 matches
            Assert.AreEqual(2, people.Count(), $"Expected 2 people, got {people.Count()}", null);

            // Everyone under 31
            people = peopleProvider.ListPeople(GetFilter(string.Empty, string.Empty, 0, 31));
            // Expect 3 matches
            Assert.AreEqual(3, people.Count(), $"Expected 3 people, got {people.Count()}", null);




            TruncatePersonTable();
        }

        /// <summary>
        /// Create one person on an empty table
        /// Retrieve the person using their ID
        /// Check that it is the same person
        /// </summary>
        [Test]
        public void SQLite_GetpersonByID_CheckCorrect() {

            TruncatePersonTable();

            // Check no one on the table
            var people = peopleProvider.ListPeople();
            Assert.AreEqual(0, people.Count());

            // Create one person
            var person = GetNewPerson("Joe", "Smith", 40);
            peopleProvider.CreatePerson(person);

            // List people, get first off list (already covered this test), use ID
            people = peopleProvider.ListPeople();
            var ID = people.ToArray()[0].ID;

            var actualPerson = peopleProvider.GetPerson(ID);
            Assert.AreEqual(GetPersonString(person), GetPersonString(actualPerson), $"Expected person {person.FirstName} {person.LastName} {person.Age} but got {actualPerson.FirstName} {actualPerson.LastName} {actualPerson.Age}", null);

            TruncatePersonTable();

        }

        /// <summary>
        /// Create one person on an empty table
        /// Update their details
        /// Check that the details have changed
        /// </summary>
        [Test]
        public void SQLite_UpdatePerson_CheckDataChange() {

            TruncatePersonTable();

            // Create one person
            var person = GetNewPerson("Joe", "Smith", 40);
            peopleProvider.CreatePerson(person);

            // Get ID 
            var people = peopleProvider.ListPeople();
            var ID = people.ToArray()[0].ID;

            // Update the Age (use a new entity with a different age, but the same ID)
            var actualPerson = GetNewPerson("Joe", "Smith", 12);
            actualPerson.ID = ID;
            peopleProvider.UpdatePeople(new List<IPersonEntity> { actualPerson });

            // Get by ID (test covered)
            actualPerson = peopleProvider.GetPerson(ID);

            Assert.AreNotEqual(GetPersonString(person), GetPersonString(actualPerson), $"Expected person {person.FirstName} {person.LastName} {person.Age} but got {actualPerson.FirstName} {actualPerson.LastName} {actualPerson.Age}", null);

            TruncatePersonTable();

        }

        /// <summary>
        /// Create one person on an empty table.
        /// Delete that person.
        /// Check that the table is now empty.
        /// </summary>
        [Test]
        public void SQLite_DeletePerson_CheckEmpty() {
            TruncatePersonTable();

            // Create one person
            var person = GetNewPerson("Joe", "Smith", 40);
            peopleProvider.CreatePerson(person);

            // Get ID 
            var people = peopleProvider.ListPeople();
            var ID = people.ToArray()[0].ID;

            // Delete
            peopleProvider.DeletePerson(ID);

            // Check all gone
            people = peopleProvider.ListPeople();

            Assert.AreEqual(0, people.Count(), $"Expected an empty table after delete, but found {people.Count()} item(s)", null);

            TruncatePersonTable();
        }

        /// <summary>
        /// Create two people on an empty table;
        /// Delete one person
        /// Check that one person remains on the table.
        /// </summary>
        [Test]
        public void SQLite_DeletePerson_CheckNotEmpty() {

            TruncatePersonTable();

            // Create two people
            var person = GetNewPerson("Joe", "Smith", 40);
            peopleProvider.CreatePerson(person);

            person = GetNewPerson("Josephine", "Smith", 30);
            peopleProvider.CreatePerson(person);

            // Get ID 
            var people = peopleProvider.ListPeople();
            var ID = people.ToArray()[0].ID;

            // Delete
            peopleProvider.DeletePerson(ID);

            // Check all gone
            people = peopleProvider.ListPeople();

            Assert.AreEqual(1, people.Count(), $"Expected 1 person after delete, but found {people.Count()} item(s)", null);

            TruncatePersonTable();
        }

        /// <summary>
        /// Testing Age groups assuming we know what the current settings are.
        /// These tests are inherently fragile and *probably* wouldn't be
        /// used in a real situation. Edge cases tested.
        /// </summary>
        [TestCase(-1, "Unable to determine Age Group")]
        [TestCase(0, "Toddler")]
        [TestCase(1, "Toddler")]
        [TestCase(2, "Child")]
        [TestCase(22, "Early twenties")]
        [TestCase(199, "Vampire")]
        [TestCase(5000, "Kauri tree")]
        [TestCase(100000, "Kauri tree")]
        public void SQLite_GetAgegroups_CheckCorrectness(int age, string ageGroup) {

            var actualAgeGroup = peopleProvider.GetAgeGroup(age);
            Assert.AreEqual(ageGroup, actualAgeGroup);

        }

        #region ----- Test Helpers -----

        private IPeopleFilter GetFilter(string first, string last, int min, int max) {

            return new PeopleFilter {
                FirstName = first,
                LastName = last,
                MinAge = min,
                MaxAge = max
            };

        }

        /// <summary>
        /// Returns a PersonEntity, without ID, as a concatenated 
        /// string to allow shallow comparison.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private string GetPersonString(IPersonEntity person) {

            return $"{person.FirstName}|{person.LastName}|{person.Age}";

        }

        private IPersonEntity GetNewPerson(string first, string last, int age) {

            var person = ContainerFactory.ResolveInstance<IPersonEntity>();
            person.FirstName = first;
            person.LastName = last;
            person.Age = age;

            return person;

        }

        /// <summary>
        /// Each test will start by testing for an empty table, and each test
        /// will leave the database state as it found it.
        /// </summary>
        private void TruncatePersonTable() {

            // We have to work with what we have :)
            peopleProvider = new PeopleProvider();
            var people = peopleProvider.ListPeople();

            people.ToList().ForEach(p => peopleProvider.DeletePerson(p.ID));
            
        }

        #endregion ----- Test Helpers -----

    }
}
