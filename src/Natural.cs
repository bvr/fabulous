
/* from Math from scratch series

 - [ ] [Math from scratch, part thirteen: multiplicative inverses](https://ericlippert.com/2013/11/12/math-from-scratch-part-thirteen-multiplicative-inverses/)
 - [ ] [Math from scratch, part twelve: Euclid and Bézout](https://ericlippert.com/2013/11/04/math-from-scratch-part-twelve-euclid-and-bezout/)

## [October 2013](https://ericlippert.com/2013/10/)
 - [ ] [Math from scratch, part eleven: integer division](https://ericlippert.com/2013/10/31/math-from-scratch-part-eleven-integer-division/)
 - [ ] [Math from scratch, part ten: integer comparisons](https://ericlippert.com/2013/10/28/math-from-scratch-part-ten-integer-comparisons/)
 - [ ] [Math from scratch, part nine: integer arithmetic](https://ericlippert.com/2013/10/21/math-from-scratch-part-nine-integer-arithmetic/)
 - [ ] [Math from scratch, part eight: integers](https://ericlippert.com/2013/10/17/math-from-scratch-part-eight-integers/)
 - [ ] [Math from scratch, part seven: division and remainder](https://ericlippert.com/2013/10/14/math-from-scratch-part-seven-division-and-remainder/)
 - [ ] [Math from scratch, part six: comparisons](https://ericlippert.com/2013/10/07/math-from-scratch-part-six-comparisons/)
 - [ ] [Math from scratch, part five: natural subtraction](https://ericlippert.com/2013/10/03/math-from-scratch-part-five/)

## [September 2013](https://ericlippert.com/2013/09/)
 - [x] [Math from scratch, part four: natural multiplication](https://ericlippert.com/2013/09/30/math-from-scratch-part-four/)
 - [x] [Math from scratch, part three: natural addition](https://ericlippert.com/2013/09/23/math-from-scratch-part-three/)
 - [x] [Math from scratch, part two: zero and one](https://ericlippert.com/2013/09/19/math-from-scratch-part-two/)
 - [x] [Math from scratch, part one](https://ericlippert.com/2013/09/16/math-from-scratch-part-one/)

*/
namespace ImmutableNumbers
{
    public sealed class Natural
    {
        private sealed class Bit
        {
            public override string ToString()
            {
                return this == Natural.ZeroBit ? "0" : "1";
            }
        }

        private static readonly Bit ZeroBit = new Bit();
        private static readonly Bit OneBit = new Bit();

        public static readonly Natural Zero = new Natural(null, ZeroBit);
        public static readonly Natural One = new Natural(Zero, OneBit);

        private readonly Natural tail;
        private readonly Bit head;

        private Natural(Natural tail, Bit head)
        {
            this.head = head;
            this.tail = tail;
        }

        private static Natural Create(Natural tail, Bit head)
        {
            if (ReferenceEquals(tail, Zero))
                return head == ZeroBit ? Zero : One;
            else
                return new Natural(tail, head);
        }

        private static Natural Add(Natural x, Natural y)
        {
            if (ReferenceEquals(x, Zero))
                return y;
            else if (ReferenceEquals(y, Zero))
                return x;
            else if (x.head == ZeroBit)
                return Create(Add(x.tail, y.tail), y.head);
            else if (y.head == ZeroBit)
                return Create(Add(x.tail, y.tail), x.head);
            else
                return Create(Add(Add(x.tail, y.tail), One), ZeroBit);
        }

        public static Natural operator -(Natural x, Natural y)
        {
            if (ReferenceEquals(x, null))
                throw new ArgumentNullException("x");
            else if (ReferenceEquals(y, null))
                throw new ArgumentNullException("y");
            else
                return Subtract(x, y);
        }

        private static Natural Subtract(Natural x, Natural y)
        {
            if (ReferenceEquals(x, y))
                return Zero;
            else if (ReferenceEquals(y, Zero))
                return x;
            else if (ReferenceEquals(x, Zero))
                throw new InvalidOperationException("Cannot subtract greater natural from lesser natural");
            else if (x.head == y.head)
                return Create(Subtract(x.tail, y.tail), ZeroBit);
            else if (x.head == OneBit)
                return Create(Subtract(x.tail, y.tail), OneBit);
            else
                return Create(Subtract(Subtract(x.tail, One), y.tail), OneBit);
        }

        public static Natural operator --(Natural x)
        {
            if (ReferenceEquals(x, null))
                throw new ArgumentNullException("x");
            else if (ReferenceEquals(x, Zero))
                throw new InvalidOperationException();
            else
                return Subtract(x, One);
        }

        public override string ToString()
        {
            if (ReferenceEquals(this, Zero))
                return "0";
            else
                return tail.ToString() + head.ToString();
        }

        public static Natural operator *(Natural x, Natural y)
        {
            if (ReferenceEquals(x, null))
                throw new ArgumentNullException("x");
            else if (ReferenceEquals(y, null))
                throw new ArgumentNullException("y");
            else
                return Multiply(x, y);
        }

        private static Natural Multiply(Natural x, Natural y)
        {
            if (ReferenceEquals(x, Zero))
                return Zero;
            else if (ReferenceEquals(y, Zero))
                return Zero;
            else if (ReferenceEquals(x, One))
                return y;
            else if (ReferenceEquals(y, One))
                return x;
            else if (x.head == ZeroBit)
                return Create(Multiply(x.tail, y), ZeroBit);
            else if (y.head == ZeroBit)
                return Create(Multiply(x, y.tail), ZeroBit);
            else
                return Add(Create(Multiply(x, y.tail), ZeroBit), x);
        }

        public Natural Power(Natural exponent)
        {
            if (ReferenceEquals(exponent, null))
                throw new ArgumentNullException("exponent");
            else
                return Power(this, exponent);
        }

        private static Natural Power(Natural x, Natural y)
        {
            if (ReferenceEquals(y, Zero))
                return One;
            else
            {
                Natural p = Power(x, y.tail);
                Natural result = Multiply(p, p);
                if (y.head == OneBit)
                    result = Multiply(result, x);
                return result;
            }
        }
    }
}
