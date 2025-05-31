using System;

using AudioPlayer.Extension;

namespace AudioPlayer.Model
{
    public class LibraryStatistic : ModelBase
    {
        readonly Func<LibraryEntry, IComparable> _keySelector;
        readonly Func<LibraryEntry, bool> _statisticSelector;

        string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public SortedObservableCollection<IComparable, LibraryEntry> Collection { get; set; }

        public LibraryStatistic(string name,
                                Func<LibraryEntry, IComparable> keySelector,
                                Func<LibraryEntry, bool> statisticSelector)
        {
            _keySelector = keySelector;
            _statisticSelector = statisticSelector;

            this.Name = name;
            this.Collection = new SortedObservableCollection<IComparable, LibraryEntry>();

            this.OnPropertyChanged("Collection");
        }

        /// <summary>
        /// Clears statistic collection
        /// </summary>
        public void Clear()
        {
            this.Collection.Clear();
        }

        /// <summary>
        /// Adds the entry to the statistic collection if it passes the criteria (passed in at constructor)
        /// </summary>
        public void FilteredAdd(LibraryEntry entry)
        {
            if (_statisticSelector(entry))
                this.Collection.Add(entry, _keySelector);
        }
    }
}
