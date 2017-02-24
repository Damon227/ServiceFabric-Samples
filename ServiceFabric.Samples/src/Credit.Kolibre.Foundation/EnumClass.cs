// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : EnumClass.cs
// Created          : 2016-08-22  4:40 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Credit.Kolibre.Foundation.Exceptions;

namespace Credit.Kolibre.Foundation
{
    public abstract class EnumClass : IEquatable<EnumClass>, IComparable<EnumClass>
    {
        protected List<EnumClass> _values = new List<EnumClass>();

        protected EnumClass(int code, string value)
        {
            Code = code;
            Value = value;
            _values.Add(this);
        }

        public virtual int Code { get; protected set; }

        public virtual string Value { get; protected set; }

        #region IComparable<EnumClass> Members

        /// <summary>Compares the current object with another object of the same type.</summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />. </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(EnumClass other)
        {
            if (other == null)
            {
                return 1;
            }

            return Code.CompareTo(other.Code);
        }

        #endregion

        #region IEquatable<EnumClass> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(EnumClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code;
        }

        #endregion

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        [SuppressMessage("ReSharper", "UseNullPropagation")]
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            EnumClass e = obj as EnumClass;
            if (e == null)
            {
                return false;
            }

            return Equals(e);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Code;
        }

        public static bool operator ==(EnumClass left, EnumClass right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            return left.Code == right.Code;
        }

        public static bool operator >(EnumClass left, EnumClass right)
        {
            int leftCode = left?.Code ?? 0;
            int rightCode = right?.Code ?? 0;

            return leftCode > rightCode;
        }

        public static bool operator >=(EnumClass left, EnumClass right)
        {
            int leftCode = left?.Code ?? 0;
            int rightCode = right?.Code ?? 0;

            return leftCode >= rightCode;
        }

        public static bool operator !=(EnumClass left, EnumClass right)
        {
            return !(left == right);
        }

        public static bool operator <(EnumClass left, EnumClass right)
        {
            int leftCode = left?.Code ?? 0;
            int rightCode = right?.Code ?? 0;

            return leftCode < rightCode;
        }

        public static bool operator <=(EnumClass left, EnumClass right)
        {
            int leftCode = left?.Code ?? 0;
            int rightCode = right?.Code ?? 0;

            return leftCode <= rightCode;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Value;
        }
    }

    public abstract class EnumClass<T> : EnumClass, IEquatable<EnumClass<T>>, IEquatable<T> where T : EnumClass<T>
    {
        private static readonly Dictionary<int, T> s_codes = new Dictionary<int, T>();
        private static readonly Dictionary<string, T> s_values = new Dictionary<string, T>();

        static EnumClass()
        {
            Type type = typeof(T);
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        protected EnumClass(int code, string value) : base(code, value)
        {
        }

        #region IEquatable<EnumClass<T>> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(EnumClass<T> other)
        {
            return Equals(other as T);
        }

        #endregion

        #region IEquatable<T> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            return Equals(other as EnumClass);
        }

        #endregion

        public virtual bool HasValue()
        {
            return Value.Equals("None");
        }

        public static bool IsAvailableCode(int code)
        {
            return s_codes.ContainsKey(code);
        }

        public static bool IsAvailableValue(string value)
        {
            return s_values.ContainsKey(value);
        }

        public virtual bool IsDefault()
        {
            return Code == 0;
        }

        public static T New(int code)
        {
            T t;
            if (s_codes.TryGetValue(code, out t))
            {
                return t;
            }

            throw new InvalidEnumCodeException(code, typeof(T));
        }

        public static T New(string value)
        {
            T t;
            if (s_values.TryGetValue(value, out t))
            {
                return t;
            }

            throw new InvalidEnumValueException(value, typeof(T));
        }

        public static T operator +(EnumClass<T> left, EnumClass<T> right)
        {
            return New(left.Code + right.Code);
        }

        public static bool operator &(EnumClass<T> left, EnumClass<T> right)
        {
            return left._values.Contains(right);
        }

        public static T operator |(EnumClass<T> left, EnumClass<T> right)
        {
            left._values.Add(right);
            left.Code += right.Code;
            left.Value += $", {right.Value}";
            return (T)left;
        }

        public static T operator -(EnumClass<T> left, EnumClass<T> right)
        {
            return New(left.Code - right.Code);
        }

        public static bool Parse(int code, out T quotaPlanStatus)
        {
            return s_codes.TryGetValue(code, out quotaPlanStatus);
        }

        public static bool Parse(string value, out T quotaPlanStatus)
        {
            return s_values.TryGetValue(value, out quotaPlanStatus);
        }

        protected static void Add(T quotaPlanStatus)
        {
            s_codes[quotaPlanStatus.Code] = quotaPlanStatus;
            s_values[quotaPlanStatus.Value] = quotaPlanStatus;
        }
    }
}