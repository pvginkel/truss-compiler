using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public class ImmutableArray<T> : IEnumerable<T> {
        public static readonly ImmutableArray<T> Empty = new ImmutableArray<T>(new T[0]);

        private readonly T[] _array;

        public static ImmutableArray<T> Create(params T[] items) {
            if (items == null) {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items) {
                if (item == null) {
                    throw new System.ArgumentException("Immutable array cannot contain null elements");
                }
            }

            return new ImmutableArray<T>(items.ToArray());
        }

        private ImmutableArray(T[] array) {
            _array = array;
        }

        public int Count {
            get { return _array.Length; }
        }

        public bool Contains(T item) {
            return IndexOf(item) != -1;
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T this[int index] {
            get { return _array[index]; }
        }

        public int IndexOf(T item) {
            return Array.IndexOf(_array, item);
        }

        public int LastIndexOf(T item) {
            return Array.LastIndexOf(_array, item);
        }

        public class Builder {
            private readonly List<T> _list = new List<T>();

            public ImmutableArray<T> Build() {
                if (_list.Count == 0) {
                    return Empty;
                }

                return new ImmutableArray<T>(_list.ToArray());
            }

            public int Count {
                get { return _list.Count; }
            }

            public bool Contains(T items) {
                if (items == null) {
                    throw new ArgumentNullException("items");
                }

                return _list.Contains(items);
            }

            public void Add(T item) {
                if (item == null) {
                    throw new ArgumentNullException("item");
                }

                _list.Add(item);
            }

            public void Remove(T item) {
                if (item == null) {
                    throw new ArgumentNullException("item");
                }

                _list.Remove(item);
            }

            public void AddRange(IEnumerable<T> items) {
                if (items == null) {
                    throw new ArgumentNullException("items");
                }

                ValidateList(items);

                _list.AddRange(items);
            }

            private static void ValidateList(IEnumerable<T> items) {
                foreach (var item in items) {
                    if (item == null) {
                        throw new ArgumentNullException("item");
                    }
                }
            }

            public void Clear() {
                _list.Clear();
            }

            public T this[int index] {
                get { return _list[index]; }
            }

            public int IndexOf(T item) {
                if (item == null) {
                    throw new ArgumentNullException("item");
                }

                return _list.IndexOf(item);
            }

            public int LastIndexOf(T item) {
                if (item == null) {
                    throw new ArgumentNullException("item");
                }

                return _list.LastIndexOf(item);
            }
        }
    }
}
