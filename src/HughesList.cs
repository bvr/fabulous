
// from https://ericlippert.com/2019/01/24/an-interesting-list-structure-part-2/

public struct HughesList<T>
{
    private readonly Func<SimpleList<T>, SimpleList<T>> f;

    private HughesList(Func<SimpleList<T>, SimpleList<T>> f) => this.f = f;
    private static HughesList<T> Make(Func<SimpleList<T>, SimpleList<T>> f) => new HughesList<T>(f);

    // constructors
    public static readonly HughesList<T> Empty = Make(ts => ts);
    public static HughesList<T> Single(T t) => Make(ts => ts.Push(t));
    public static HughesList<T> FromSimple(SimpleList<T> ts) => Make(ts.Concatenate);

    public SimpleList<T> ToSimple() => this.f(SimpleList<T>.Empty);

    public T Peek => this.ToSimple().Peek;
    public bool IsEmpty => this.ToSimple().IsEmpty;
    public HughesList<T> Pop => FromSimple(this.ToSimple().Pop);
    public override string ToString() => this.ToSimple().ToString();

    private static HughesList<T> Make2(Func<SimpleList<T>, SimpleList<T>> f1, Func<SimpleList<T>, SimpleList<T>> f2) => new HughesList<T>(a => f1(f2(a)));
    public HughesList<T> Push(T t) => Make2(ts2 => ts2.Push(t), this.f);
    public HughesList<T> Append(T t) => Make2(this.f, ts2 => ts2.Push(t));
    public HughesList<T> Concatenate(HughesList<T> ts2) => Make2(this.f, ts2.f);
}
