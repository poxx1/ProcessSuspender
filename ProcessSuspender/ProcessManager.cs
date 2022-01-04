using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ProcessSuspender
{
    public class ProcessManager
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            //Resume and suspend
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        public static void SuspendProcess(int pid)
        {
            //var proces = Process.GetProcessById(pid); // throws exception if process does not exist
            var processList = Process.GetProcessesByName("GTA5");
            var process = processList.First();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Process ID: " + process.Id);
            Console.WriteLine("Process Name: " + process.ProcessName);

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                //Check if this works
                try
                {
                    //SuspendProcess(pT.Id);
                    SuspendThread(pOpenThread);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Can't suspend process. Details:");
                    Console.WriteLine(e);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            //var process = Process.GetProcessById(pid);
            var processList = Process.GetProcessesByName("GTA5");
            var process = processList.First();

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The process has been resumed");
            }
        }
    }
}
