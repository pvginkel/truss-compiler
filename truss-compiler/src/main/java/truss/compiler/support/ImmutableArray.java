package truss.compiler.support;

import org.apache.commons.lang.ArrayUtils;
import org.apache.commons.lang.Validate;

import java.lang.reflect.Array;
import java.util.*;

@SuppressWarnings("NullableProblems")
public class ImmutableArray<T> implements List<T> {
    private static final Object[] EMPTY_ARRAY = new Object[0];

    private final Object[] array;

    @SafeVarargs
    public static <T1> ImmutableArray<T1> asList(T1... items) {
        Validate.notNull(items, "items");

        return new ImmutableArray<>(items.clone());
    }

    public static <T1> ImmutableArray<T1> empty() {
        return new ImmutableArray<>(EMPTY_ARRAY);
    }

    private ImmutableArray(Object[] array) {
        this.array = array;
    }

    @Override
    public int size() {
        return array.length;
    }

    @Override
    public boolean isEmpty() {
        return array.length == 0;
    }

    @Override
    public boolean contains(Object item) {
        return indexOf(item) != -1;
    }

    @Override
    public Iterator<T> iterator() {
        return new ListIteratorImpl(-1);
    }

    @Override
    public Object[] toArray() {
        return array.clone();
    }

    @SuppressWarnings({"unchecked", "SuspiciousSystemArraycopy"})
    @Override
    public <T1> T1[] toArray(T1[] target) {
        Validate.notNull(target, "target");

        if (target.length < array.length) {
            target = (T1[])Array.newInstance(target.getClass(), array.length);
        }

        System.arraycopy(array, 0, target, 0, array.length);

        return target;
    }

    @Override
    public boolean add(T item) {
        throw createException();
    }

    @Override
    public boolean remove(Object item) {
        throw createException();
    }

    @Override
    public boolean containsAll(Collection<?> items) {
        Validate.notNull(items, "items");

        for (Object item : items) {
            if (indexOf(item) == -1) {
                return false;
            }
        }

        return true;
    }

    @Override
    public boolean addAll(Collection<? extends T> items) {
        throw createException();
    }

    @Override
    public boolean addAll(int i, Collection<? extends T> items) {
        throw createException();
    }

    @Override
    public boolean removeAll(Collection<?> items) {
        throw createException();
    }

    @Override
    public boolean retainAll(Collection<?> items) {
        throw createException();
    }

    @Override
    public void clear() {
        throw createException();
    }

    @SuppressWarnings("unchecked")
    @Override
    public T get(int index) {
        return (T)array[index];
    }

    @Override
    public T set(int index, T item) {
        throw createException();
    }

    @Override
    public void add(int index, T item) {
        throw createException();
    }

    @Override
    public T remove(int index) {
        throw createException();
    }

    @Override
    public int indexOf(Object item) {
        return ArrayUtils.indexOf(array, item);
    }

    @Override
    public int lastIndexOf(Object items) {
        return ArrayUtils.lastIndexOf(array, items);
    }

    @Override
    public ListIterator<T> listIterator() {
        return new ListIteratorImpl(-1);
    }

    @Override
    public ListIterator<T> listIterator(int index) {
        return new ListIteratorImpl(index - 1);
    }

    @SuppressWarnings({"unchecked"})
    @Override
    public List<T> subList(int start, int length) {
        List<T> result = new ArrayList<>();

        for (int i = start; i < start + length; i++) {
            result.add((T)array[i]);
        }

        return result;
    }

    private ImmutableArrayException createException() {
        return new ImmutableArrayException("Immutable array cannot be modified");
    }

    private class ListIteratorImpl implements ListIterator<T> {
        private int index;

        public ListIteratorImpl(int index) {
            this.index = index;
        }

        @Override
        public boolean hasNext() {
            return index < array.length - 1;
        }

        @SuppressWarnings("unchecked")
        @Override
        public T next() {
            return (T)array[++index];
        }

        @Override
        public boolean hasPrevious() {
            return index >= 0;
        }

        @SuppressWarnings("unchecked")
        @Override
        public T previous() {
            return (T)array[--index];
        }

        @Override
        public int nextIndex() {
            return index + 1;
        }

        @Override
        public int previousIndex() {
            return index - 1;
        }

        @Override
        public void remove() {
            throw createException();
        }

        @Override
        public void set(T t) {
            throw createException();
        }

        @Override
        public void add(T t) {
            throw createException();
        }
    }

    public static class Builder<T> implements List<T> {
        private final List<T> list = new ArrayList<>();

        public ImmutableArray<T> build() {
            if (list.size() == 0) {
                return empty();
            }

            return new ImmutableArray<>(list.toArray());
        }

        @Override
        public int size() {
            return list.size();
        }

        @Override
        public boolean isEmpty() {
            return list.isEmpty();
        }

        @Override
        public boolean contains(Object items) {
            Validate.notNull(items, "items");

            return list.contains(items);
        }

        @Override
        public Iterator<T> iterator() {
            return list.iterator();
        }

        @Override
        public Object[] toArray() {
            return list.toArray();
        }

        @SuppressWarnings("SuspiciousToArrayCall")
        @Override
        public <T1> T1[] toArray(T1[] target) {
            Validate.notNull(target, "target");

            return list.toArray(target);
        }

        public boolean add(T item) {
            Validate.notNull(item, "item");

            return list.add(item);
        }

        @Override
        public boolean remove(Object item) {
            Validate.notNull(item, "item");

            return list.remove(item);
        }

        @Override
        public boolean containsAll(Collection<?> items) {
            Validate.notNull(items, "items");

            return list.containsAll(items);
        }

        public boolean addAll(Collection<? extends T> items) {
            Validate.notNull(items, "items");

            validateList(items);

            return list.addAll(items);
        }

        private void validateList(Collection<?> items) {
            for (Object item : items) {
                Validate.notNull(item, "item");
            }
        }

        public boolean addAll(int index, Collection<? extends T> items) {
            Validate.notNull(items, "items");

            validateList(items);

            return list.addAll(index, items);
        }

        @Override
        public boolean removeAll(Collection<?> items) {
            Validate.notNull(items, "items");

            validateList(items);

            return list.removeAll(items);
        }

        @Override
        public boolean retainAll(Collection<?> items) {
            Validate.notNull(items, "items");

            validateList(items);

            return list.retainAll(items);
        }

        @Override
        public void clear() {
            list.clear();
        }

        @Override
        public T get(int index) {
            return list.get(index);
        }

        public T set(int index, T item) {
            Validate.notNull(item, "item");

            return list.set(index, item);
        }

        public void add(int index, T item) {
            Validate.notNull(item, "item");

            list.add(index, item);
        }

        @Override
        public T remove(int index) {
            return list.remove(index);
        }

        @Override
        public int indexOf(Object item) {
            Validate.notNull(item, "item");

            return list.indexOf(item);
        }

        @Override
        public int lastIndexOf(Object item) {
            Validate.notNull(item, "item");

            return list.lastIndexOf(item);
        }

        @Override
        public ListIterator<T> listIterator() {
            return list.listIterator();
        }

        @Override
        public ListIterator<T> listIterator(int index) {
            return list.listIterator(index);
        }

        @Override
        public List<T> subList(int start, int length) {
            return list.subList(start, length);
        }
    }
}
