// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : TypeExtensions.cs
// Created          : 2016-09-06  4:24 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Credit.Kolibre.Foundation.Sys
{
    /// <summary>
    ///     <see cref="System.Type" /> 的扩展类。
    /// </summary>
    public static class TypeExtensions
    {
    }

    /// <summary>
    ///     Options for formatting type names.
    /// </summary>
    public class TypeFormattingOptions : IEquatable<TypeFormattingOptions>
    {
        public TypeFormattingOptions(
            string nameSuffix = null,
            bool includeNamespace = true,
            bool includeGenericParameters = true,
            bool includeTypeParameters = true,
            char nestedClassSeparator = '.',
            bool includeGlobal = true)
        {
            NameSuffix = nameSuffix;
            IncludeNamespace = includeNamespace;
            IncludeGenericTypeParameters = includeGenericParameters;
            IncludeTypeParameters = includeTypeParameters;
            NestedTypeSeparator = nestedClassSeparator;
            IncludeGlobal = includeGlobal;
        }

        /// <summary>
        ///     Gets a value indicating whether or not to include the fully-qualified namespace of the class in the result.
        /// </summary>
        public bool IncludeNamespace { get; }

        /// <summary>
        ///     Gets a value indicating whether or not to include concrete type parameters in the result.
        /// </summary>
        public bool IncludeTypeParameters { get; }

        /// <summary>
        ///     Gets a value indicating whether or not to include generic type parameters in the result.
        /// </summary>
        public bool IncludeGenericTypeParameters { get; }

        /// <summary>
        ///     Gets the separator used between declaring types and their declared types.
        /// </summary>
        public char NestedTypeSeparator { get; }

        /// <summary>
        ///     Gets the name to append to the formatted name, before any type parameters.
        /// </summary>
        public string NameSuffix { get; }

        /// <summary>
        ///     Gets a value indicating whether or not to include the global namespace qualifier.
        /// </summary>
        public bool IncludeGlobal { get; }

        #region IEquatable<TypeFormattingOptions> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(TypeFormattingOptions other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return IncludeNamespace == other.IncludeNamespace
                   && IncludeTypeParameters == other.IncludeTypeParameters
                   && IncludeGenericTypeParameters == other.IncludeGenericTypeParameters
                   && NestedTypeSeparator == other.NestedTypeSeparator
                   && string.Equals(NameSuffix, other.NameSuffix) && IncludeGlobal == other.IncludeGlobal;
        }

        #endregion

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///     <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((TypeFormattingOptions)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = IncludeNamespace.GetHashCode();
                hashCode = (hashCode * 397) ^ IncludeTypeParameters.GetHashCode();
                hashCode = (hashCode * 397) ^ IncludeGenericTypeParameters.GetHashCode();
                hashCode = (hashCode * 397) ^ NestedTypeSeparator.GetHashCode();
                hashCode = (hashCode * 397) ^ (NameSuffix != null ? NameSuffix.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IncludeGlobal.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TypeFormattingOptions left, TypeFormattingOptions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TypeFormattingOptions left, TypeFormattingOptions right)
        {
            return !Equals(left, right);
        }
    }
}