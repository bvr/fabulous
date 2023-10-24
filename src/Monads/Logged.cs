
struct Logged<T>
{
    public T Value { get; private set; }
    public string Log { get; private set; }

    public Logged(T value, string log) : this()
    {
        this.value = value;
        this.log = log;
    }

    public Logged(T value) : this(value, null) { }

    public Logged<T> AddLog(string newLog)
    {
        return new Logged(this.value, this.log + newLog);
    }

    public static Logged<R> Bind<A, R>(Logged<A> logged, Func<A, Logged<R>> function)
    {
        Logged result = function(logged.Value);
        return new Logged(result.Value, logged.Log + result.Log);
    }
}
