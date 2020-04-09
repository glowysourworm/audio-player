using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AudioPlayer.Extension
{
    /// <summary>
    /// Collection implementation that supports binding and sorting by default.
    /// </summary>
    [Serializable]
    public class SortedObservableCollection<K, T> : IList<T>,
                                                    ICollection<T>,
                                                    IEnumerable<T>,
                                                    INotifyCollectionChanged,
                                                    INotifyPropertyChanged,
                                                    ISerializable
                                                    where K : IComparable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        List<K> _keys;
        List<T> _values;
        Dictionary<K, T> _dict;

        public int Count { get { return _keys.Count; } }

        public bool IsReadOnly { get { return false; } }

        public T this[int index] 
        {
            get { return _values[index]; }
            set { throw new NotSupportedException("Please use Add method to insert items"); } 
        }

        public T this[K key]
        {
            get { return _dict[key]; }
            set { throw new NotSupportedException("Not supported -> please use the Add method"); }
        }

        public SortedObservableCollection()
        {
            _dict = new Dictionary<K, T>();
            _keys = new List<K>();
            _values = new List<T>();
        }

        public SortedObservableCollection(IEnumerable<T> collection, Func<T, K> keySelector)
        {
            _dict = new Dictionary<K, T>();
            _keys = new List<K>();
            _values = new List<T>();

            // Load collections
            foreach (var item in collection)
                Add(keySelector(item), item);
        }

        public SortedObservableCollection(SerializationInfo info, StreamingContext context)
        {
            _dict = new Dictionary<K, T>();
            _keys = new List<K>();
            _values = new List<T>();

            // UNSORTED
            var count = info.GetInt32("Count");

            // (SORT) Populate private collections from deserialized items
            for (int i=0;i<count;i++)
            {
                var key = (K)info.GetValue("Key" + i, typeof(K));
                var value = (T)info.GetValue("Value" + i, typeof(T));

                Add(key, value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // UNSORTED
            info.AddValue("Count", _dict.Count);

            var counter = 0;

            foreach (var element in _dict)
            {
                info.AddValue("Key" + counter, element.Key);
                info.AddValue("Value" + counter++, element.Value);
            }
        }

        public void Add(T value)
        {
            throw new NotSupportedException("Please use Add(T, Func<T, K>) method with key selector");
        }

        public void Add(T value, Func<T, K> keySelector)
        {
            Add(keySelector(value), value);
        }

        protected void Add(K key, T value)
        {
            // Procedure
            //
            // 1) Perform binary search by key
            // 2) Perform "Insert"
            //      - Notify Add using the index
            //

            // Binary search for the insert location
            var index = _keys.Count > 0 ? BinarySearch(key, 0, _keys.Count - 1)
                                        : 0;

            _keys.Insert(index, key);
            _values.Insert(index, value);

            // DICTIONARY NOT SORTED
            _dict.Add(key, value);

            OnInsert(value, index);
            OnPropertyChanged("Count");
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("Please use the Remove(T, Func<T, K>) with key selector");
        }

        public bool Remove(T item, Func<T, K> keySelector)
        {
            return Remove(keySelector(item));
        }

        protected bool Remove(K key)
        {
            // Fetch index of item
            var index = _keys.IndexOf(key);

            // Fetch item
            var item = _dict[key];

            // Remove the item
            _keys.RemoveAt(index);
            _values.RemoveAt(index);
            _dict.Remove(key);

            // Notify collection changed
            OnRemove(item, index);
            OnPropertyChanged("Count");

            return true;
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public void Clear()
        {
            _dict.Clear();
            _keys.Clear();
            _values.Clear();
        }

        public int IndexOf(T item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        int BinarySearch(K searchKey, int lowerIndex, int upperIndex)
        {
            // Choose the middle index to compare
            var middleIndex = (int)Math.Floor((upperIndex + lowerIndex) / 2.0);

            // Perform key comparison
            var comparison = searchKey.CompareTo(_keys[middleIndex]);

            if (comparison < 0)
            {
                // Single item left
                if (lowerIndex == upperIndex)
                    return middleIndex;

                // Truncated to lower index
                else if (middleIndex == lowerIndex)
                    return middleIndex;

                return BinarySearch(searchKey, lowerIndex, middleIndex - 1);
            }

            else if (comparison == 0)
                throw new Exception("Duplicate key found SortedObservableCollection");

            else
            {
                // Single item left
                if (lowerIndex == upperIndex)
                    return middleIndex + 1;

                return BinarySearch(searchKey, middleIndex + 1, upperIndex);
            }
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
                        _values,
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add, item, index));
        }

        void OnRemove(object item, int index)
        {
            // Hopefully a performance boost - depending on the UI implementation
            if (this.CollectionChanged != null)
                this.CollectionChanged(
                        _values,
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Remove, item, index));
        }

        void OnReset()
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(_values,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
