using Azoth.Data.MVVM;
using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    public class PersonEntity : NotifyBase, IPersonEntity {

        private int _ID;
        public int ID {
            get { return _ID; }
            set {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string _FirstName;
        public string FirstName {
            get { return _FirstName; }
            set {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _LastName;
        public string LastName {
            get { return _LastName; }
            set {
                _LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private int _Age;
        public int Age {
            get { return _Age; }
            set {
                _Age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string _AgeRange;
        public string AgeRange {
            get { return _AgeRange; }
            set {
                _AgeRange = value;
                OnPropertyChanged(nameof(AgeRange));
            }
        }


    }
}
