using AdventOfCode2023.Utils.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Experiment
    {
        public object Run()
        {
            MyClass obj1 = new MyClass { Value = 42 };
            MyClass obj2 = new MyClass { Value = 42 };
            MyClass obj3 = new MyClass { Value = 69 };

            // Using IEquatable<T> for equality comparison
            var areEqual = obj1.Equals(obj2);
            areEqual = obj1 == obj2;
            var isGreater = obj3 > obj2;
            var isGreatrOrEqual = obj2 >= obj1;
            isGreatrOrEqual = obj3 >= obj2;

            Console.WriteLine($"Are equal: {areEqual}");

            return null;
        }

        public class MyClass : IEquatable<MyClass>, IComparable<MyClass>
        {
            public int Value { get; set; }

            public bool Equals(MyClass other)
            {
                // Implement custom equality logic here
                return other != null && this.Value == other.Value;
            }

            // Optionally, override the Equals method from System.Object
            public override bool Equals(object obj)
            {
                if (obj is MyClass other)
                {
                    return Equals(other);
                }
                return false;
            }

            public static bool operator ==(MyClass left, MyClass right)
            {
                if (ReferenceEquals(left, null))
                {
                    return ReferenceEquals(right, null);
                }
                return left.Equals(right);
            }

            public static bool operator !=(MyClass left, MyClass right)
            {
                return !(left == right);
            }

            public static bool operator >(MyClass left, MyClass right)
            {
                if (ReferenceEquals(left, null))
                {
                    return false;
                }
                return left.CompareTo(right) > 0;
            }

            public static bool operator >=(MyClass left, MyClass right)
            {
                return left.CompareTo(right) >= 0;
            }

            public static bool operator <=(MyClass left, MyClass right)
            {
                return left.CompareTo(right) <= 0;
            }

            public static bool operator <(MyClass left, MyClass right)
            {
                if (ReferenceEquals(left, null))
                {
                    return !ReferenceEquals(right, null);
                }
                return left.CompareTo(right) < 0;
            }

            // Optionally, override GetHashCode for better hash code performance
            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public int CompareTo(MyClass other)
            {
                if (other == null)
                {
                    return 1; // Indicates that the current instance is greater
                }

                // Compare based on the 'Value' property
                return this.Value.CompareTo(other.Value);
            }
        }
    }
}
