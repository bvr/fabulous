
// from https://ericlippert.com/2007/12/04/immutability-in-c-part-two-a-simple-immutable-stack/

public interface IStack<T> : IEnumerable<T>
{
    IStack<T> Push(T value);
    IStack<T> Pop();
    T Peek();
    bool IsEmpty { get; }
}

public sealed class Stack<T> : IStack<T>
{

    private sealed class EmptyStack : IStack<T>
    {
        public bool IsEmpty { get { return true; } }
        public T Peek() { throw new Exception("Empty stack"); }

        public IStack<T> Push(T value)
        {
            return new Stack<T>(value, this);
        }

        public IStack<T> Pop() { throw new Exception("Empty stack"); }

        public IEnumerator<T> GetEnumerator() { yield break; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    private static readonly EmptyStack empty = new EmptyStack();
    public static IStack<T> Empty { get { return empty; } }
    private readonly T head;
    private readonly IStack<T> tail;

    private Stack(T head, IStack<T> tail)
    {
        this.head = head;
        this.tail = tail;
    }

    public bool IsEmpty { get { return false; } }
    public T Peek() { return head; }
    public IStack<T> Pop() { return tail; }
    public IStack<T> Push(T value) { return new Stack<T>(value, this); }

    public IEnumerator<T> GetEnumerator()
    {
        for (IStack<T> stack = this; !stack.IsEmpty; stack = stack.Pop())
            yield return stack.Peek();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

/*
IStack<int> s1 = Stack<int>.Empty;
IStack<int> s2 = s1.Push(10);
IStack<int> s3 = s2.Push(20);
IStack<int> s4 = s2.Push(30); // shares its tail with s3.
*/
