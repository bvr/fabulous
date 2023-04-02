
public abstract class SimpleList<T>
{
    public static readonly SimpleList<T> Empty = new EmptyList();
    public static SimpleList<T> Single(T t) => Empty.Push(t);
    private SimpleList() { }
    public abstract bool IsEmpty { get; }
    public abstract T Peek { get; }

    // If this is a, b, c, then this.Pop is b, c
    public abstract SimpleList<T> Pop { get; }

    // If this is a, b, c and s is d, e, f, then
    // this.Concatenate(s) is a, b, c, d, e, f
    public abstract SimpleList<T> Concatenate(SimpleList<T> s);

    // If this is b, c, and t is a, then this.Push(t) is a, b, c
    public SimpleList<T> Push(T t) => new NonEmptyList(t, this);

    // If this is b, c, and t is a, then this.Append(t) is b, c, a
    public SimpleList<T> Append(T t) => this.Concatenate(Single(t));

    private sealed class EmptyList : SimpleList<T>
    {
        public override bool IsEmpty => true;
        public override T Peek => throw new InvalidOperationException();
        public override SimpleList<T> Pop => throw new InvalidOperationException();
        public override SimpleList<T> Concatenate(SimpleList<T> s) => s;
        public override string ToString() => "";
    }

    private sealed class NonEmptyList : SimpleList<T>
    {
        private readonly T head;
        private readonly SimpleList<T> tail;
        public NonEmptyList(T head, SimpleList<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }

        public override bool IsEmpty => false;

        public override T Peek => head;
        public override SimpleList<T> Pop => tail;
        public override SimpleList<T> Concatenate(SimpleList<T> s) =>
            s.IsEmpty ? this : this.Pop.Concatenate(s).Push(this.Peek);
    }

    public override string ToString() =>
      this.Pop.IsEmpty ?
        $"{this.Peek}" :
        $"{this.Peek}, {this.Pop}";
}
