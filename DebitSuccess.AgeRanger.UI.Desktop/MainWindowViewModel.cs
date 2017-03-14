using Azoth.Data.MVVM;
using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    public class MainWindowViewModel :ViewModelBase {

        PersonApi _personApi;
        ILogger _logger;

        public MainWindowViewModel() {

            // Causes rendering preview to fail if allowed to continue
            if (this.IsInDesignMode) return;

            _logger = ContainerFactory.ResolveInstance<ILogger>();
            Mediator.Register(this);
            PopulatePeopleList();

        }

        private async void PopulatePeopleList() {

            _logger.Write("Initialising ... ", enLogType.Info);

            _personApi = new PersonApi();
            var list = await _personApi.List();
            People = new SafeObservableCollection<IPersonEntity>(list);

            if (list.Count() == 0) {
                _logger.Write("No people loaded. Did you remember to rebuid the solution?", enLogType.Error);
            } else {
                _logger.Write($"Complete. {list.Count()} people loaded.", enLogType.Info);
            }
        }

        private SafeObservableCollection<string> _LogItems = new SafeObservableCollection<string>();
        public SafeObservableCollection<string> LogItems {
            get { return _LogItems; }
            set {
                _LogItems = value;
                OnPropertyChanged(nameof(LogItems));
            }
        }

        private SafeObservableCollection<IPersonEntity> _People;
        public SafeObservableCollection<IPersonEntity> People {
            get { return _People; }
            set {
                _People = value;
                OnPropertyChanged(nameof(People));
            }
        }

        private IPersonEntity _SelectedPerson;
        public IPersonEntity SelectedPerson {
            get { return _SelectedPerson; }
            set {
                _SelectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }


        private IPeopleFilter _SearchFilter = new PeopleFilter();
        public IPeopleFilter SearchFilter {
            get { return _SearchFilter; }
            set {
                _SearchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
            }
        }

        [MediatorMessageSink("LogEvent", ParameterType = typeof(ILogMessage))]
        private void OnMessageReceivedEvent(ILogMessage msg) {

            while (LogItems.Count > 10000) {
                LogItems.RemoveAt(LogItems.Count - 1);
            }
            
            var uiText = string.Format("{0} {1} : {2}",
                   msg.TimeStamp.ToString("HH:mm:ss").PadRight(8, ' '),
                   msg.LogType.ToString().PadRight(8, ' '),
                   msg.LogText);

            LogItems.Insert(0, uiText);
        }

        #region ----- SavePerson Command -----

        private RelayCommand _SavePersonCommand;
        public ICommand SavePersonCommand {
            get {
                if (_SavePersonCommand == null) {
                    _SavePersonCommand = new RelayCommand(param => this.SavePerson(), param => this.CanSavePerson);
                }
                return _SavePersonCommand;
            }
        }

        private async void SavePerson() {

            if (SelectedPerson == null) return;

            _logger.Write($"Saving {SelectedPerson.ID} ({SelectedPerson.FirstName} {SelectedPerson.LastName})", enLogType.Info);

            var currentPerson = SelectedPerson;

            await _personApi.Save(SelectedPerson);

            _logger.Write("Reloading current search...", enLogType.Info);
            var list = await _personApi.List(SearchFilter);
            People = new SafeObservableCollection<IPersonEntity>(list);

            // Restore the current selected / new person
            if (SelectedPerson != null && SelectedPerson.ID > 0) {
                SelectedPerson = People.FirstOrDefault(p => p.ID == currentPerson.ID);
            } else {
                // Best effort. There could be duplicates in this schema
                if (currentPerson != null) {
                    SelectedPerson = People.FirstOrDefault(p => p.FirstName == currentPerson.FirstName && p.LastName == currentPerson.LastName && p.Age == currentPerson.Age);
                }
            }

        }

        private bool CanSavePerson {
            get { return SelectedPerson != null; }
        }

        #endregion SavePerson Command

        #region ----- AddPerson Command -----

        private RelayCommand _AddPersonCommand;
        public ICommand AddPersonCommand {
            get {
                if (_AddPersonCommand == null) {
                    _AddPersonCommand = new RelayCommand(param => this.AddPerson(), param => this.CanAddPerson);
                }
                return _AddPersonCommand;
            }
        }

        private void AddPerson() {
            SelectedPerson = new PersonEntity();
        }

        private bool CanAddPerson {
            get { return true; }
        }

        #endregion AddPerson Command

        #region ----- Search Command -----

        private RelayCommand _SearchCommand;
        public ICommand SearchCommand {
            get {
                if (_SearchCommand == null) {
                    _SearchCommand = new RelayCommand(param => this.Search(), param => this.CanSearch);
                }
                return _SearchCommand;
            }
        }

        private async void Search() {

            _logger.Write("Searching ...", enLogType.Info);

            var list = await _personApi.List(SearchFilter);
            People = new SafeObservableCollection<IPersonEntity>(list);

            _logger.Write($"Search complete: {list.Count()} matches found", enLogType.Info);
        }

        private bool CanSearch {
            get { return true; }
        }

        #endregion Search Command


    }
}
