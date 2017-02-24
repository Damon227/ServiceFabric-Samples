// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.AspNetCore
// File             : TelemetryClientExtensions.cs
// Created          : 2016-07-04  9:43 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Credit.Kolibre.Foundation.ServiceFabric.Insights
{
    public static class TelemetryClientExtensions
    {
        public static void TrackDependency(this TelemetryClient telemetryClient, string dependencyTypeName, string dependencyName, string methodName, Action action)
        {
            bool success = false;
            DateTime startTime = DateTime.UtcNow;
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                action();
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            finally
            {
                timer.Stop();

                DependencyTelemetry dependencyTelemetry = new DependencyTelemetry(dependencyName, methodName, startTime, timer.Elapsed, success)
                {
                    DependencyTypeName = dependencyTypeName
                };
                telemetryClient?.TrackDependency(dependencyTelemetry);
            }
        }

        public static T TrackDependency<T>(this TelemetryClient telemetryClient, string dependencyTypeName, string dependencyName, string methodName, Func<T> action)
        {
            T result;
            bool success = false;
            DateTime startTime = DateTime.UtcNow;
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                result = action();
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            finally
            {
                timer.Stop();

                DependencyTelemetry dependencyTelemetry = new DependencyTelemetry(dependencyName, methodName, startTime, timer.Elapsed, success)
                {
                    DependencyTypeName = dependencyTypeName,
                    CommandName = methodName
                };
                telemetryClient?.TrackDependency(dependencyTelemetry);
            }

            return result;
        }

        public static async Task<T> TrackDependencyAsync<T>(this TelemetryClient telemetryClient, string dependencyTypeName, string typeName, string methodName, Func<Task<T>> action)
        {
            T result;
            bool success = false;
            DateTime startTime = DateTime.UtcNow;
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                result = await action();
                success = true;
            }
            catch (Exception)
            {
                success = false;
                throw;
            }
            finally
            {
                timer.Stop();

                DependencyTelemetry dependencyTelemetry = new DependencyTelemetry(typeName, methodName, startTime, timer.Elapsed, success)
                {
                    DependencyTypeName = dependencyTypeName
                };
                telemetryClient?.TrackDependency(dependencyTelemetry);
            }

            return result;
        }
    }
}