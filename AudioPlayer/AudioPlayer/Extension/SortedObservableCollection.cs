using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace AudioPlayer.Extension
{
    /// <summary>
    /// Collection implementation that supports binding and sorting by default.
    /// </summary>
    [Serializable]
    public class SortedObservableCollection<K, T> : ICollection<T>,
                                                    IEnumerable<T>,
                                                    IList<T>,
                                                    INotifyCollectionChanged,
                                                    INotifyPropertyChanged,
                                                    ISerializable
                                                    where K : IComparable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        readonly Func<T, K> _keySelector;
        readonly bool _ignoreDuplicates;

        List<T> _list;

        public int Count
        {
            get { return _list.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int index]
        {
            get { return (T)_list[index]; }
            set { throw new NotSupportedException("Set indexer not support. Use Add(..) method"); }
        }

        public T Get(K key)
        {
            return _list.First(x => _keySelector(x).CompareTo(key) == 0);
        }

        public SortedObservableCollection(Func<T, K> keySelector, bool ignoreDuplicates)
        {
            _keySelector = keySelector;
            _ignoreDuplicates = ignoreDuplicates;
            _list = new List<T>();
        }

        public SortedObservableCollection(IEnumerable<T> collection, Func<T, K> keySelector, bool ignoreDuplicates)
        {
            _keySelector = keySelector;
            _ignoreDuplicates = ignoreDuplicates;
            _list = new List<T>();

            foreach (var item in collection)
                Add(item);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // TODO
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Insert not support. Use Add(..) method");
        }

        public void RemoveAt(int index)
        {
            // Item to remove
            var item = this[index];

            // Remove the item
            _list.RemoveAt(index);

            // Notify collection changed
            OnRemove(item, index);
            OnPropertyChanged("Count");
        }

        public void Add(T item)
        {
            // Procedure
            //
            // 1) Perform binary search by key
            // 2) Perform "Insert"
            //      - Notify Add using the index
            //

            // Binary search for the insert location
            var index = _list.Count > 0 ? BinarySearch(item, 0, _list.Count - 1)
                                        : 0;

            _list.Insert(index, item);

            OnInsert(item, index);
            OnPropertyChanged("Count");
        }

        public void Clear()
        {
            _list.Clear();

            OnReset();
            OnPropertyChanged("Count");
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return _list.Any(x => _keySelector(x).CompareTo(key) == 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            var index = _list.IndexOf(item);

            if (_list.Remove(item))
            {
                OnRemove(item, index);
                OnPropertyChanged("Count");

                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        int BinarySearch(T searchItem, int lowerIndex, int upperIndex)
        {
            // Check to see whether list contains item
            if (lowerIndex == upperIndex)
                return lowerIndex;

            // Choose the middle index to compare
            var middleIndex = (int)Math.Floor((upperIndex + lowerIndex) / 2.0);

            // Check to see whether middle falls on the end (floor)
            if (middleIndex == lowerIndex)
                return middleIndex;

            // Perform key comparison
            var comparison = Compare(searchItem, _list[middleIndex]);

            if (comparison < 0)
            {
                return BinarySearch(searchItem, lowerIndex, middleIndex);
            }
            else if (comparison == 0)
            {
                if (!_ignoreDuplicates)
                    throw new Exception("Duplicate key found SortedObservableCollection");

                else
                    return middleIndex;
            }
            else
            {
                return BinarySearch(searchItem, middleIndex, upperIndex);
            }
        }

        int Compare(T item1, T item2)
        {
            var value1 = _keySelector(item1);
            var value2 = _keySelector(item2);

            return value1.CompareTo(value2);
        }

        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        void OnInsert(object item, int index)
        {
            // Hopefully a performance boost - depending on the UI implementation
            if (this.CollectionChanged != null)
                this.CollectionChanged(
                        this, 
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add, item, index));
        }

        void OnRemove(object item, int index)
        {
            // Hopefully a performance boost - depending on the UI implementation
            if (this.CollectionChanged != null)
                this.CollectionChanged(
                        this,
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Remove, item, index));
        }

        void OnReset()
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
