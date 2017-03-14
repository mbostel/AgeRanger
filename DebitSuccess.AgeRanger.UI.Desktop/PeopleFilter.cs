using Azoth.Data.MVVM;
using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    public class PeopleFilter : NotifyBase, IPeopleFilter {
        
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


        private int? _MinAge;
        public int? MinAge {
            get { return _MinAge; }
            set {
                _MinAge = value;
                OnPropertyChanged(nameof(MinAge));
            }
        }


        private int? _MaxAge;
        public int? MaxAge {
            get { return _MaxAge; }
            set {
                _MaxAge = value;
                OnPropertyChanged(nameof(MaxAge));
            }
        }


    }
}
