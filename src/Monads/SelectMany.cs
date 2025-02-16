
/// actual implementation is at https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,02f5d280493ce132

/// simplified implementation of SelectMany

static IEnumerable<R> SelectMany<A, R>(this IEnumerable<A> sequence, Func<A, IEnumerable<R>> function)
{
    foreach (A outerItem in sequence)
        foreach (R innerItem in function(outerItem))
            yield return innerItem;
}


/// example on how to make Where

static IEnumerable<T> WhereHelper<T>(T item, Func<T, bool> predicate)
{
    if (predicate(item))
        yield return item;
}

static IEnumerable<T> Where<T>(this IEnumerable<T> items, Func<T, bool> predicate)
{
    return items.SelectMany(item => WhereHelper(item, predicate));
}

IEnumerable<int> query = original.Where(num => num % 2 != 0);


/// example on how to make Select

static IEnumerable<R> SelectHelper<A, R>(A item, Func<A, R> projection)
{
    yield return projection(item);
}

static IEnumerable<R> Select<A, R>(this IEnumerable<A> items, Func<A, R> projection)
{
    return items.SelectMany(item => SelectHelper(item, projection));
}

IEnumerable<int> query = original.Select(num => num + 100);
