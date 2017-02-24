// ***********************************************************************
// Solution         : Kolibre
// Project          : Credit.Kolibre.Foundation.Logging
// File             : MultiFileWatcher.cs
// Created          : 2016-09-28  8:17 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Credit.Kolibre.Foundation.Logging.Internal
{
    /// <summary>
    ///     Watches multiple files at the same time and raises an event whenever
    ///     a single change is detected in any of those files.
    /// </summary>
    internal class MultiFileWatcher : IDisposable
    {
        private readonly Dictionary<string, FileSystemWatcher> _watcherDic = new Dictionary<string, FileSystemWatcher>();

        public MultiFileWatcher() :
            this(NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.Security | NotifyFilters.Attributes)
        {
        }

        public MultiFileWatcher(NotifyFilters notifyFilters)
        {
            NotifyFilters = notifyFilters;
        }

        /// <summary>
        ///     The types of changes to watch for.
        /// </summary>
        public NotifyFilters NotifyFilters { get; set; }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            StopWatching();
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Occurs when a change is detected in one of the monitored files.
        /// </summary>
        public event FileSystemEventHandler OnChange;

        /// <summary>
        ///     Stops watching all files.
        /// </summary>
        public void StopWatching()
        {
            lock (this)
            {
                foreach (FileSystemWatcher watcher in _watcherDic.Values)
                {
                    StopWatching(watcher);
                }

                _watcherDic.Clear();
            }
        }

        /// <summary>
        ///     Stops watching the specified file.
        /// </summary>
        /// <param name="fileName"></param>
        public void StopWatching(string fileName)
        {
            lock (this)
            {
                FileSystemWatcher watcher;
                if (_watcherDic.TryGetValue(fileName, out watcher))
                {
                    StopWatching(watcher);
                    _watcherDic.Remove(fileName);
                }
            }
        }

        /// <summary>
        ///     Watches the specified files for changes.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        public void Watch(IEnumerable<string> fileNames)
        {
            if (fileNames == null)
            {
                return;
            }

            foreach (string s in fileNames)
            {
                Watch(s);
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Watcher is released in Dispose()")]
        internal void Watch(string fileName)
        {
            string directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
            {
                return;
            }

            lock (this)
            {
                if (_watcherDic.ContainsKey(fileName))
                    return;

                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = directory,
                    Filter = Path.GetFileName(fileName),
                    NotifyFilter = NotifyFilters
                };

                watcher.Created += OnWatcherChanged;
                watcher.Changed += OnWatcherChanged;
                watcher.Deleted += OnWatcherChanged;
                watcher.EnableRaisingEvents = true;

                _watcherDic.Add(fileName, watcher);
            }
        }

        private void OnWatcherChanged(object source, FileSystemEventArgs e)
        {
            OnChange?.Invoke(source, e);
        }

        private static void StopWatching(FileSystemWatcher watcher)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
    }
}