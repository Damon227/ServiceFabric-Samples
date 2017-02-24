// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation
// File             : Result.cs
// Created          : 2016-07-14  10:01 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace Credit.Kolibre.Foundation
{
    /// <summary>
    ///     Represents the result of an operation.
    /// </summary>
    public class Result
    {
        protected int? _code;
        protected string _message;

        /// <summary>
        ///     Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        ///     Gets or sets the code for this operation.
        /// </summary>
        public int Code
        {
            get { return _code ?? (Succeeded ? 0 : Error?.Code ?? -1); }
            set { _code = value; }
        }

        /// <summary>
        ///     Gets or sets the message for this operation.
        /// </summary>
        public string Message
        {
            get { return _message ?? (Succeeded ? "ok" : Error?.Message ?? "error"); }
            set { _message = value; }
        }

        /// <summary>
        ///     The first error that occurred during the operation.
        /// </summary>
        /// <value>
        ///     If the <see cref="Succeeded" /> of this <see cref="Result" /> is <c>true</c>, return <c>null</c>.
        /// </value>
        public Error Error
        {
            get { return Errors.FirstOrDefault(); }
        }

        /// <summary>
        ///     An <see cref="List{T}" /> of <see cref="Error" />s containing an errors
        ///     that occurred during the operation.
        /// </summary>
        /// <value>An <see cref="List{T}" /> of <see cref="Error" />s.</value>
        public List<Error> Errors { get; } = new List<Error>();

        /// <summary>
        ///     Returns an <see cref="Result" /> indicating a successful operation.
        /// </summary>
        /// <returns>An <see cref="Result" /> indicating a successful operation.</returns>
        public static Result Success { get; } = new Result { Succeeded = true };

        /// <summary>
        ///     Creates a <see cref="Result" /> indicating a unsuccessfully operation, with a list of <paramref name="errors" /> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error" />s which caused the operation to fail.</param>
        /// <returns>An <see cref="Result" /> indicating a unsuccessfully operation, with a list of <paramref name="errors" /> if applicable.</returns>
        public static Result Failed(params Error[] errors)
        {
            Result result = new Result { Succeeded = false };
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result" /> indicating a unsuccessfully operation.
        /// </summary>
        /// <param name="code">A code of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <param name="message">A message of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <returns>An <see cref="Result" /> indicating a unsuccessfully operation.</returns>
        public static Result Failed(int code, string message)
        {
            Result result = new Result { Succeeded = false };
            message = message ?? "error";
            result.Errors.Add(new Error { Code = code, Message = message });
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result" /> indicating a unsuccessfully operation.
        /// </summary>
        /// <param name="message">A message of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <param name="code">A code of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <returns>An <see cref="Result" /> indicating a unsuccessfully operation.</returns>
        public static Result Failed(string message, int code)
        {
            Result result = new Result { Succeeded = false };
            message = message ?? "error";
            result.Errors.Add(new Error { Code = code, Message = message });
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result" /> indicating a succeeded operation.
        /// </summary>
        /// <param name="code">A code of the <see cref="Result" />.</param>
        /// <param name="message">A message of the <see cref="Result" />.</param>
        /// <returns>An <see cref="Result" /> indicating a succeeded operation.</returns>
        public static Result Succeed(int code, string message)
        {
            message = message ?? "ok";
            return new Result { Succeeded = true, Code = code, Message = message };
        }
    }

    /// <summary>
    ///     Represents the result of an operation.
    /// </summary>
    public class Result<TData> where TData : class
    {
        private int? _code;
        private string _message;

        /// <summary>
        ///     Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        ///     Gets or sets the code for this operation.
        /// </summary>
        public int Code
        {
            get { return _code ?? (Succeeded ? 0 : Error?.Code ?? -1); }
            set { _code = value; }
        }

        /// <summary>
        ///     Gets or sets the message for this operation.
        /// </summary>
        public string Message
        {
            get { return _message ?? (Succeeded ? "ok" : Error?.Message ?? "error"); }
            set { _message = value; }
        }

        /// <summary>
        ///     Gets or sets the data for this operation.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        ///     The first error that occurred during the operation.
        /// </summary>
        /// <value>
        ///     If the <see cref="Succeeded" /> of this <see cref="Result{TData}" /> is <c>true</c>, return <c>null</c>.
        /// </value>
        public Error Error
        {
            get { return Errors.FirstOrDefault(); }
        }

        /// <summary>
        ///     An <see cref="List{T}" /> of <see cref="Error" />s containing an errors
        ///     that occurred during the operation.
        /// </summary>
        /// <value>An <see cref="List{T}" /> of <see cref="Error" />s.</value>
        public List<Error> Errors { get; } = new List<Error>();

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a unsuccessfully operation, with a list of <paramref name="errors" /> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Error" />s which caused the operation to fail.</param>
        /// <returns>An <see cref="Result{TData}" /> indicating a unsuccessfully operation, with a list of <paramref name="errors" /> if applicable.</returns>
        public static Result<TData> Failed(params Error[] errors)
        {
            Result<TData> result = new Result<TData> { Succeeded = false, Data = null };
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a unsuccessfully operation.
        /// </summary>
        /// <param name="code">A code of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <param name="message">A message of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <returns>An <see cref="Result{TData}" /> indicating a unsuccessfully operation.</returns>
        public static Result<TData> Failed(int code, string message)
        {
            Result<TData> result = new Result<TData> { Succeeded = false, Data = null };
            message = message ?? "error";
            result.Errors.Add(new Error { Code = code, Message = message });
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a unsuccessfully operation.
        /// </summary>
        /// <param name="message">A message of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <param name="code">A code of the <see cref="Error" /> which caused the operation to fail.</param>
        /// <returns>An <see cref="Result{TData}" /> indicating a unsuccessfully operation.</returns>
        public static Result<TData> Failed(string message, int code)
        {
            Result<TData> result = new Result<TData> { Succeeded = false, Data = null };
            message = message ?? "error";
            result.Errors.Add(new Error { Code = code, Message = message });
            return result;
        }

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a succeeded operation.
        /// </summary>
        /// <param name="code">A code of the <see cref="Result" />.</param>
        /// <param name="message">A message of the <see cref="Result" />.</param>
        /// <param name="data">A <see cref="TData" /> object contains the data.</param>
        /// <returns>An <see cref="Result" /> indicating a succeeded operation.</returns>
        public static Result<TData> Succeed(int code, string message, TData data)
        {
            message = message ?? "ok";
            return new Result<TData> { Succeeded = true, Code = code, Message = message, Data = data };
        }

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a succeeded operation.
        /// </summary>
        /// <param name="data">A <see cref="TData" /> object contains the data.</param>
        /// <param name="message">A message of the <see cref="Result" />.</param>
        /// <param name="code">A code of the <see cref="Result" />.</param>
        /// <returns>An <see cref="Result" /> indicating a succeeded operation.</returns>
        public static Result<TData> Succeed(TData data, string message, int code)
        {
            message = message ?? "ok";
            return new Result<TData> { Succeeded = true, Code = code, Message = message, Data = data };
        }

        /// <summary>
        ///     Creates a <see cref="Result{TData}" /> indicating a succeeded operation.
        /// </summary>
        /// <param name="data">A <see cref="TData" /> object contains the data.</param>
        /// <returns>An <see cref="Result" /> indicating a succeeded operation.</returns>
        public static Result<TData> Succeed(TData data)
        {
            return new Result<TData> { Succeeded = true, Code = 0, Message = "ok", Data = data };
        }
    }
}