﻿using AudioPlayer.Event;
using AudioPlayer.Extension;

using ReactiveUI;

using System;

namespace AudioPlayer.Model
{
    public class LibraryStatistic : ModelBase
    {
        readonly Func<LibraryEntry, object> _keySelector;
        readonly Func<LibraryEntry, bool> _statisticSelector;

        string _name;

        public string Name
        {
            get { return _name; }
            set { Update(ref _name, value); }
        }

        public event SimpleEventHandler<LibraryStatistic> LoadStatisticCollectionEvent;

        public IReactiveCommand LoadCollectionCommand { get; set; }

        public SortedObservableCollection<IComparable, LibraryEntry> Collection { get; set; }

        public LibraryStatistic(string name,
                                Func<LibraryEntry, IComparable> keySelector,
                                Func<LibraryEntry, bool> statisticSelector,
                                bool ignoreDuplicates)
        {
            _keySelector = keySelector;
            _statisticSelector = statisticSelector;

            this.Name = name;
            this.Collection = new SortedObservableCollection<IComparable, LibraryEntry>(keySelector, ignoreDuplicates);

            this.LoadCollectionCommand = ReactiveCommand.Create(() =>
            {
            });

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
        public void FilteredAdd(LibraryEntry entry)
        {
            if (_statisticSelector(entry))
                this.Collection.Add(entry);
        }
    }
}
