
// from https://ericlippert.com/2007/12/18/immutability-in-c-part-six-a-simple-binary-tree/

public interface IBinaryTree<V>
{
    bool IsEmpty { get; }
    V Value { get; }
    IBinaryTree<V> Left { get; }
    IBinaryTree<V> Right { get; }
}

public sealed class BinaryTree<V> : IBinaryTree<V>
{
    private sealed class EmptyBinaryTree : IBinaryTree<V>
    {
        public bool IsEmpty { get { return true; } }

        public IBinaryTree<V> Left
        { 
            get { throw new Exception("Empty tree"); } 
        }

        public IBinaryTree<V> Right
        { 
            get { throw new Exception("Empty tree"); } 
        }

        public V Value 
        { 
            get { throw new Exception("Empty tree"); } 
        }
    }

    private static readonly EmptyBinaryTree empty = new EmptyBinaryTree();
    public static IBinaryTree<V> Empty { get { return empty; } }

    private readonly V value;
    private readonly IBinaryTree<V> left;
    private readonly IBinaryTree<V> right;

    public bool IsEmpty { get { return false; } }
    public V Value { get { return value; } }
    public IBinaryTree<V> Left { get { return left; } }
    public IBinaryTree<V> Right { get { return right; } }
    public BinaryTree(V value, IBinaryTree<V> left, IBinaryTree<V> right)
    {
        this.value = value;
        this.left = left ?? Empty;
        this.right = right ?? Empty;
    }
}

public static IEnumerable<V> InOrder<V>(this IBinaryTree<V> tree)
{
    // BAD IMPLEMENTATION, DO NOT DO THIS
    if (tree.IsEmpty) yield break;
    foreach (V v in tree.Left.InOrder()) 
        yield return v;
    yield return tree.Value;
    foreach (V v in tree.Right.InOrder()) 
        yield return v;
}
