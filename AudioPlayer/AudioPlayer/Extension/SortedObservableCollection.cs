using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AudioPlayer.Extension
{
    /// <summary>
    /// A simple ordered list implementation - sorts items when inserted and removed. NOTE*** The binding views seemed to "want"
    /// the IList implementation (!!!?) It must've been required for bindings to operate.
    /// </summary>
    [Serializable]
    public class SortedObservableCollection<T> : IList<T>, IList, INotifyPropertyChanged, INotifyCollectionChanged, ISerializable
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Protected list of items
        /// </summary>
        protected List<T> ItemList { get; private set; }

        /// <summary>
        /// Equality comparer for sorting
        /// </summary>
        protected Comparer<T> ItemComparer { get; private set; }

        const int UNSUCCESSFUL_SEARCH = -1;

        public SortedObservableCollection()
        {
            this.ItemList = new List<T>();
            this.ItemComparer = Comparer<T>.Default;

            OnPropertyChanged("Count");
        }

        public SortedObservableCollection(Comparer<T> comparer)
        {
            this.ItemList = new List<T>();
            this.ItemComparer = comparer;

            OnPropertyChanged("Count");
        }

        public SortedObservableCollection(IEnumerable<T> items)
        {
            this.ItemList = new List<T>(items);
            this.ItemComparer = Comparer<T>.Default;
        }

        public SortedObservableCollection(IEnumerable<T> items, Comparer<T> itemComparer)
        {
            this.ItemList = new List<T>(items);
            this.ItemComparer = itemComparer;
        }

        public SortedObservableCollection(SerializationInfo info, StreamingContext context)
        {
            this.ItemList = (List<T>)info.GetValue("List", typeof(List<T>));
            this.ItemComparer = (Comparer<T>)info.GetValue("Comparer", typeof(Comparer<T>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("List", this.ItemList);
            info.AddValue("Comparer", this.ItemComparer);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);              // Notify
        }

        // Functions Watched:  Add, Remove, RemoveAt, Clear
        private void OnCollectionChanged_Add(T item, int index)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }

            // Also -> Property Changed (Count)
            OnPropertyChanged("Count");
        }
        private void OnCollectionChanged_Remove(T item, int index)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }

            // Also -> Property Changed (Count)
            OnPropertyChanged("Count");
        }
        private void OnCollectionChanged_Clear(IList<T> removedItems)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, removedItems));
            }

            // Also -> Property Changed (Count)
            OnPropertyChanged("Count");
        }

        // Functions Watched:  Count (also, follows collection changes)
        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region (public) IList<T> IList

        public T this[int index]
        {
            get { return this.ItemList[index]; }
            set { throw new Exception("Manual insert not supported for SimpleOrderedList<>"); }
        }

        public int Count
        {
            get { return this.ItemList.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        object IList.this[int index]
        {
            get
            {
                return this.ItemList[(int)index];
            }
            set
            {
                this.Insert(index, value);
            }
        }

        // O(log n)
        public void Add(T item)
        {
            var index = GetInsertIndex(item);

            this.ItemList.Insert(index, item);

            OnCollectionChanged_Add(item, index);
        }

        public void Clear()
        {
            var list = this.ItemList.ToArray(); // Copy the list to pass on to listeners

            this.ItemList.Clear();

            OnCollectionChanged_Clear(list);
        }

        // O(log n)
        public bool Contains(T item)
        {
            return GetInsertIndex(item) != UNSUCCESSFUL_SEARCH;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.ItemList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.ItemList.GetEnumerator();
        }

        // O(log n)
        public int IndexOf(T item)
        {
            return GetInsertIndex(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Manual insertion not allowed for SimpleOrderedList<>");
        }

        // O(log n)
        public bool Remove(T item)
        {
            var index = GetInsertIndex(item);

            if (index == UNSUCCESSFUL_SEARCH)
                throw new Exception("Item not found in collection SimpleOrderedList.cs");

            this.ItemList.RemoveAt(index);

            OnCollectionChanged_Remove(item, index);

            return true;
        }

        public void RemoveAt(int index)
        {
            var item = this.ItemList[index];

            this.ItemList.RemoveAt(index);

            OnCollectionChanged_Remove(item, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.ItemList.GetEnumerator();
        }

        // O(log n)
        public int Add(object value)
        {
            if (!(value is T))
                throw new Exception("Trying to insert non-template type:  SimpleOrderedList");

            var index = GetInsertIndex((T)value);

            this.ItemList.Insert(index, (T)value);

            OnCollectionChanged_Add((T)value, index);

            return index;
        }

        // O(log n)
        public bool Contains(object value)
        {
            if (!(value is T))
                throw new Exception("Trying to operate on non-template type:  SimpleOrderedList");

            return GetInsertIndex((T)value) != UNSUCCESSFUL_SEARCH;
        }

        // O(log n)
        public int IndexOf(object value)
        {
            if (!(value is T))
                throw new Exception("Trying to operate on non-template type:  SimpleOrderedList");

            return GetInsertIndex((T)value);
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException("Manual insertion not allowed for SimpleOrderedList<>");
        }

        // O(log n)
        public void Remove(object value)
        {
            if (!(value is T))
                throw new Exception("Trying to operate on non-template type:  SimpleOrderedList");

            var index = GetInsertIndex((T)value);

            if (index == UNSUCCESSFUL_SEARCH)
                throw new Exception("Item not found in collection SimpleOrderedList.cs");

            this.ItemList.RemoveAt(index);

            OnCollectionChanged_Remove((T)value, index);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region (private) Binary Search Implementation

        private int BinarySearch(T searchItem, out int insertIndex)
        {
            /*
                function binary_search(A, n, T) is
                    L := 0
                    R := n − 1
                    while L ≤ R do
                        m := floor((L + R) / 2)
                        if A[m] < T then
                            L := m + 1
                        else if A[m] > T then
                            R := m − 1
                        else:
                            return m
                    return unsuccessful
             */

            var leftIndex = 0;
            var rightIndex = this.ItemList.Count - 1;

            // Initialize insert index to be the left index
            insertIndex = leftIndex;

            while (leftIndex <= rightIndex)
            {
                var middleIndex = (int)Math.Floor((leftIndex + rightIndex) / 2.0D);
                var item = this.ItemList[middleIndex];

                // Set insert index
                insertIndex = middleIndex;

                // Item's value is LESS THAN search value
                if (this.ItemComparer.Compare(item, searchItem) < 0)
                {
                    leftIndex = middleIndex + 1;

                    // Set insert index for catching final iteration
                    insertIndex = leftIndex;
                }

                // GREATER THAN
                else if (this.ItemComparer.Compare(item, searchItem) > 0)
                    rightIndex = middleIndex - 1;

                else
                    return middleIndex;
            }

            return UNSUCCESSFUL_SEARCH;
        }

        private int GetInsertIndex(T item)
        {
            var insertIndex = UNSUCCESSFUL_SEARCH;
            var searchIndex = BinarySearch(item, out insertIndex);

            // NOT FOUND
            if (searchIndex == UNSUCCESSFUL_SEARCH)
            {
                return insertIndex;
            }
            else
            {
                return searchIndex;
            }
        }
        #endregion
    }
}
