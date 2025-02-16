// from https://ericlippert.com/2007/12/06/immutability-in-c-part-three-a-covariant-immutable-stack/

public static class Extensions
{
    public static IStack<T> Push<T>(this IStack<T> s, T t)
    {
        return Stack<T>.Push(t, s);
    }
}

public interface IStack<out T> : IEnumerable<T>
{
    IStack<T> Pop();
    T Peek();
    bool IsEmpty { get; }
}

public sealed class Stack<T> : IStack<T>
{
    private sealed class EmptyStack : IStack<T>
    {
        public bool IsEmpty
        {
            get { return true; }
        }

        public T Peek()
        {
            throw new Exception("Empty stack");
        }

        public IStack<T> Pop()
        {
            throw new Exception("Empty stack");
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private static readonly EmptyStack empty = new EmptyStack();
    public static IStack<T> Empty
    {
        get { return empty; }
    }

    private readonly T head;
    private readonly IStack<T> tail;

    private Stack(T head, IStack<T> tail)
    {
        this.head = head;
        this.tail = tail;
    }

    public bool IsEmpty
    {
        get { return false; }
    }

    public T Peek() => head;

    public IStack<T> Pop() => tail;

    public static IStack<T> Push(T head, IStack<T> tail) => new Stack<T>(head, tail);

    public IEnumerator<T> GetEnumerator()
    {
        for (IStack<T> stack = this; !stack.IsEmpty; stack = stack.Pop())
            yield return stack.Peek();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

/*
IStack<Giraffe> sg1 = Stack<Giraffe>.Empty;
IStack<Giraffe> sg2 = sg1.Push(new Giraffe("Gerry"));
IStack<Giraffe> sg3 = sg2.Push(new Giraffe("Geoffrey"));
IStack<Mammal> sm3 = sg3; // Legal because of covariance.
IStack<Mammal> sm4 = sm3.Push(new Tiger("Tony"));
*/
