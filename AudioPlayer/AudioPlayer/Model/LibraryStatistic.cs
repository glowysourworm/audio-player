using AudioPlayer.Extension;
using AudioPlayer.Model.Interface;
using ReactiveUI;

using System;

namespace AudioPlayer.Model
{
    public class LibraryStatistic : ModelBase
    {
        readonly Func<ILibraryEntry, IComparable> _keySelector;
        readonly Func<ILibraryEntry, bool> _statisticSelector;

        string _name;

        public string Name
        {
            get { return _name; }
            set { Update(ref _name, value); }
        }

        public SortedObservableCollection<IComparable, ILibraryEntry> Collection { get; set; }

        public LibraryStatistic(string name,
                                Func<ILibraryEntry, IComparable> keySelector,
                                Func<ILibraryEntry, bool> statisticSelector)
        {
            _keySelector = keySelector;
            _statisticSelector = statisticSelector;

            this.Name = name;
            this.Collection = new SortedObservableCollection<IComparable, ILibraryEntry>();

            this.RaisePropertyChanged("Collection");
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
        public void FilteredAdd(ILibraryEntry entry)
        {
            if (_statisticSelector(entry))
                this.Collection.Add(entry, _keySelector);
        }
    }
}
