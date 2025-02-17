
// from 
// - https://ericlippert.com/2007/12/18/immutability-in-c-part-six-a-simple-binary-tree/
// - https://ericlippert.com/2007/12/19/immutability-in-c-part-seven-more-on-binary-trees/

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

// rewritten without recursion in part 7
public static IEnumerable<T> InOrder<T>(this IBinaryTree<T> tree)
{
    IStack<IBinaryTree<T>> stack = Stack<IBinaryTree<T>>.Empty;
    for (IBinaryTree<T> current = tree; !current.IsEmpty || !stack.IsEmpty; current = current.Right)
    {
        while (!current.IsEmpty)
        {
            stack = stack.Push(current);
            current = current.Left;
        }
        current = stack.Peek();
        stack = stack.Pop();
        yield return current.Value;
    }
}
