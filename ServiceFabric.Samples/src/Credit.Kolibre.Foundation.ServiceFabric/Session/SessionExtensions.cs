// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : SessionExtensions.cs
// Created          : 2016-07-05  1:00 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Credit.Kolibre.Foundation.Static;
using Credit.Kolibre.Foundation.Sys;
using Microsoft.AspNetCore.Http;

namespace Credit.Kolibre.Foundation.ServiceFabric.Session
{
    public static class SessionExtensions
    {
        public static byte[] Get(this ISession session, string key)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            byte[] value;
            session.TryGetValue(key, out value);
            return value;
        }

        public static int? GetInt(this ISession session, string key)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            string data = session.GetString(key);
            if (data.IsNullOrEmpty())
            {
                return null;
            }
            return data.ToInt32();
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            string data = session.GetString(key);
            if (data.IsNullOrEmpty())
            {
                return null;
            }
            return data.FromJson<T>(SETTING.DATA_JSON_SETTINGS);
        }

        public static string GetString(this ISession session, string key)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            byte[] data = session.Get(key);
            return data?.ToUTF8String();
        }

        public static void SetInt(this ISession session, string key, int value)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            session.SetString(key, value.ToString());
        }

        public static void SetObject<T>(this ISession session, string key, T value) where T : class
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            session.SetString(key, value.ToJson(SETTING.DATA_JSON_SETTINGS));
        }

        public static void SetString(this ISession session, string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentException(SR.Argument_EmptyOrNullString, nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            session.Set(key, value.GetBytesOfUTF8());
        }
    }
}