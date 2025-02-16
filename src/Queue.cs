
// from https://ericlippert.com/2007/12/10/immutability-in-c-part-four-an-immutable-queue/

public interface IQueue<T> : IEnumerable<T>
{
    bool IsEmpty { get; }
    T Peek();
    IQueue<T> Enqueue(T value);
    IQueue<T> Dequeue();
}

static public IStack<T> Reverse<T>(this IStack<T> stack)
{
    IStack<T> r = Stack<T>.Empty;
    for (IStack<T> f = stack; !f.IsEmpty; f = f.Pop())
        r = r.Push(f.Peek());
    return r;
}

public sealed class Queue<T> : IQueue<T>
{
    private sealed class EmptyQueue : IQueue<T>
    {
        public bool IsEmpty { get { return true; } }
        public T Peek() { throw new Exception("empty queue"); }
        public IQueue<T> Enqueue(T value) => new Queue<T>(Stack<T>.Empty.Push(value), Stack<T>.Empty);
        public IQueue<T> Dequeue() { throw new Exception("empty queue"); }
        public IEnumerator<T> GetEnumerator() { yield break; }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private static readonly IQueue<T> empty = new EmptyQueue();
    public static IQueue<T> Empty { get { return empty; } }
    public bool IsEmpty { get { return false; } }

    private readonly IStack<T> backwards;
    private readonly IStack<T> forwards;

    private Queue(IStack<T> f, IStack<T> b)
    {
        forwards = f;
        backwards = b;
    }

    public T Peek() => forwards.Peek();
    public IQueue<T> Enqueue(T value) => new Queue<T>(forwards, backwards.Push(value));

    public IQueue<T> Dequeue()
    {
        IStack<T> f = forwards.Pop();
        if (!f.IsEmpty)
            return new Queue<T>(f, backwards);
        else if (backwards.IsEmpty)
            return Queue<T>.Empty;
        else
            return new Queue<T>(backwards.Reverse(), Stack<T>.Empty);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T t in forwards) yield return t;
        foreach (T t in backwards.Reverse()) yield return t;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
