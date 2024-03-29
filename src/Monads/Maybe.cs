
// from https://ericlippert.com/2013/04/02/monads-part-twelve/

struct Maybe<T>
{
    public static Maybe<T> Null = default(Maybe<T>);
    public bool HasValue { get; private set; }
    private T value;
    public T Value
    {
        get
        {
            if (!HasValue) throw new Exception();
            return value;
        }
    }

    public Maybe(T value) : this()
    {
        this.HasValue = true;
        this.value = value;
    }

    public override string ToString() => HasValue ? Value.ToString() : "NULL";
}

static class Extensions
{
    public static Maybe<T> Unit<T>(this T value) => new Maybe<T>(value);

    public static Maybe<R> Bind<A, R>(this Maybe<A> maybe, Func<A, Maybe<R>> function)
    {
        return maybe.HasValue ? function(maybe.Value) : Maybe<R>.Null;
    }

    public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> maybe, Func<A, Maybe<B>> function, Func<A, B, C> projection)
    {
        return maybe.Bind(
          outer => function(outer).Bind(
            inner => projection(outer, inner).Unit()));
    }

    public static Maybe<short> AsSmall(this int x)
    {
        return 0 <= x && x <= 100 ?
          ((short)x).Unit() :
          Maybe<short>.Null;
    }

    public static Maybe<short> QuerySyntax(this Maybe<int> maybe)
    {
        return from outer in maybe
               from inner in outer.AsSmall()
               select inner;
    }
}

class P
{
    static void Main()
    {
        System.Console.WriteLine(1.Unit().QuerySyntax());
        System.Console.WriteLine(1000.Unit().QuerySyntax());
        System.Console.WriteLine(Maybe<int>.Null.QuerySyntax());
    }
}
