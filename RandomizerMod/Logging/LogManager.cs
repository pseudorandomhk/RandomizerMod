﻿using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using static Shims.NET.System.IO.DirectoryInfo;
using _Path = Shims.NET.System.IO.Path;

namespace RandomizerMod.Logging
{
    public static class LogManager
    {
        public static readonly string R4Directory = Path.Combine(Application.persistentDataPath, "Randomizer 4");
        public static readonly string RecentDirectory = _Path.Combine(Application.persistentDataPath, "Randomizer 4", "Recent");
        public static string UserDirectory => _Path.Combine(Application.persistentDataPath, "Randomizer 4", "user" + RandomizerMod.RS.ProfileID);

        /// <summary>
        /// Loggers which are activated when the save is created.
        /// </summary>
        private static readonly List<RandoLogger> loggers = new()
        {
            new SettingsLog(),
            new ItemSpoilerLog(),
            new TransitionSpoilerLog(),
            new NotchCostSpoilerLog(),
        };

        public static void AddLogger(RandoLogger rl)
        {
            loggers.Add(rl);
        }

        public static void RemoveLogger(RandoLogger rl)
        {
            loggers.Remove(rl);
        }

        internal static void Initialize()
        {
            logRequestConsumer = new Thread(() =>
            {
                foreach (Action a in logRequests.GetConsumingEnumerable())
                {
                    try
                    {
                        a?.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogError($"Error in log request:\n{e}");
                    }
                }
            })
            { IsBackground = true, Priority = System.Threading.ThreadPriority.Lowest };
            logRequestConsumer.Start();

            Modding.ModHooks.ApplicationQuitHook += CloseLogRequests;
        }

        internal static void WaitForQueue()
        {
            if (logRequests.Count == 0 || logRequestConsumer is null || !logRequestConsumer.IsAlive) return;

            EventWaitHandle h = new(false, EventResetMode.AutoReset);
            Do(EmptyQueue);
            h.WaitOne();

            void EmptyQueue()
            {
                if (logRequests.Count == 0) h.Set();
                else Do(EmptyQueue);
            }
        }

        private static void CloseLogRequests()
        {
            try
            {
                logRequests.CompleteAdding();
                logRequestConsumer.Join();
                logRequests.Dispose();
            }
            catch (Exception e)
            {
                LogError($"Error disposing LogManager:\n{e}");
            }
        }

        private static readonly BlockingCollection<Action> logRequests = new();
        private static Thread logRequestConsumer;

        /// <summary>
        /// Enqueues an operation on the logging thread to write to the specified file in the log directories.
        /// </summary>
        public static void Write(string contents, string fileName)
        {
            void WriteLog()
            {
                try
                {
                    string userPath = Path.Combine(UserDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(userPath));
                    File.WriteAllText(userPath, contents);
                }
                catch (Exception e)
                {
                    LogError($"Error printing log request to {UserDirectory}:\n{e}");
                }
                try
                {
                    string recentPath = Path.Combine(RecentDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(recentPath));
                    File.WriteAllText(recentPath, contents);
                }
                catch (Exception e)
                {
                    LogError($"Error printing log request to {RecentDirectory}:\n{e}");
                }
            }

            logRequests.Add(WriteLog);
        }

        /// <summary>
        /// Enqueues the operation on the logging thread to write to the specified file in the log directories.
        /// </summary>
        public static void Write(Action<TextWriter> a, string fileName)
        {
            void WriteLog()
            {
                try
                {
                    string userPath = Path.Combine(UserDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(userPath));
                    using FileStream fs = File.Create(userPath);
                    using StreamWriter sr = new(fs);
                    a?.Invoke(sr);
                    sr.Close();

                    string recentPath = Path.Combine(RecentDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(recentPath));
                    File.Copy(userPath, recentPath, true);
                }
                catch (Exception e)
                {
                    LogError($"Error printing log request for {fileName}:\n{e}");
                }
            }

            logRequests.Add(WriteLog);
        }

        /// <summary>
        /// Enqueues an operation on the logging thread to append the contents to the specified file in the log directories.
        /// </summary>
        public static void Append(string contents, string fileName)
        {
            void AppendLog()
            {
                try
                {
                    string userPath = Path.Combine(UserDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(userPath));
                    File.AppendAllText(userPath, contents);
                }
                catch (Exception e)
                {
                    LogError($"Error appending log request to {UserDirectory}:\n{e}");
                }
                try
                {
                    string recentPath = Path.Combine(RecentDirectory, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(recentPath));
                    File.AppendAllText(recentPath, contents);
                }
                catch (Exception e)
                {
                    LogError($"Error appending log request to {RecentDirectory}:\n{e}");
                }
            }

            logRequests.Add(AppendLog);
        }

        /// <summary>
        /// Enqueues an action on the logging thread.
        /// </summary>
        public static void Do(Action a)
        {
            logRequests.Add(a);
        }

        internal static void InitDirectory()
        {
            DirectoryInfo userDI;
            try
            {
                userDI = Directory.CreateDirectory(UserDirectory);
                foreach (FileInfo fi in userDI.EnumerateFiles())
                {
                    fi.Delete();
                }
            }
            catch (Exception e)
            {
                LogError($"Error initializing user logging directory:\n{e}");
                return;
            }

            DirectoryInfo recentDI;
            try
            {
                recentDI = Directory.CreateDirectory(RecentDirectory);
                foreach (FileInfo fi in recentDI.EnumerateFiles())
                {
                    fi.Delete();
                }
            }
            catch (Exception e)
            {
                LogError($"Error initializing recent logging directory:\n{e}");
                return;
            }
        }

        internal static void WriteLogs(LogArguments args)
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();
            foreach (var rl in loggers) logRequests.Add(() => rl.DoLog(args));
            logRequests.Add(() =>
            {
                sw.Stop();
                Log($"Printed new game logs in {sw.Elapsed.TotalSeconds} seconds.");
            });
        }

        internal static void UpdateRecent(int profileID)
        {
            void MoveFiles()
            {
                try
                {
                    DirectoryInfo recentDI = Directory.CreateDirectory(RecentDirectory);
                    DirectoryInfo userDI = Directory.CreateDirectory(Path.Combine(R4Directory, "user" + profileID));
                    foreach (FileInfo fi in recentDI.EnumerateFiles())
                    {
                        fi.Delete();
                    }
                    foreach (FileInfo fi in userDI.EnumerateFiles())
                    {
                        fi.CopyTo(Path.Combine(recentDI.FullName, fi.Name), true);
                    }
                }
                catch (Exception e)
                {
                    LogError($"Error overwriting recent log directory:\n{e}");
                }
            }

            logRequests.Add(MoveFiles);
        }
    }
}
