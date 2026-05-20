using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MountBlade2BannerlordTrainer
{
    /// <summary>
    /// Entry point for the Mount & Blade II: Bannerlord Trainer.
    /// This trainer modifies game memory to enable cheats such as infinite health,
    /// unlimited gold, and max skills. It uses Windows API calls to read/write process memory.
    /// </summary>
    class Program
    {
        // Windows API imports for memory manipulation
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint PROCESS_VM_OPERATION = 0x0008;

        static void Main(string[] args)
        {
            Console.WriteLine("Mount & Blade II: Bannerlord Trainer v1.0");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Press F1: Infinite Health");
            Console.WriteLine("Press F2: Unlimited Gold");
            Console.WriteLine("Press F3: Max Skills");
            Console.WriteLine("Press F4: Exit");

            // Find Bannerlord process by name
            var processName = "Bannerlord";
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine("Bannerlord process not found. Please start the game first.");
                Console.ReadKey();
                return;
            }

            var targetProcess = processes[0];
            Console.WriteLine($"Attached to process: {targetProcess.ProcessName} (PID: {targetProcess.Id})");

            IntPtr processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, targetProcess.Id);
            if (processHandle == IntPtr.Zero)
            {
                Console.WriteLine("Failed to open process. Run as administrator.");
                Console.ReadKey();
                return;
            }

            // Example addresses (these would need to be updated per game patch)
            // In a real trainer, these are found via pattern scanning
            IntPtr healthAddress = (IntPtr)0x0A1B2C3D; // Placeholder
            IntPtr goldAddress = (IntPtr)0x0E5F6A7B;   // Placeholder
            IntPtr skillAddress = (IntPtr)0x0C8D9E0F; // Placeholder

            bool running = true;
            while (running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.F1:
                            WriteMemory(processHandle, healthAddress, BitConverter.GetBytes(9999.0f));
                            Console.WriteLine("Infinite Health activated.");
                            break;
                        case ConsoleKey.F2:
                            WriteMemory(processHandle, goldAddress, BitConverter.GetBytes(100000));
                            Console.WriteLine("Unlimited Gold set to 100,000.");
                            break;
                        case ConsoleKey.F3:
                            // Max all skills (example: set to 300)
                            for (int i = 0; i < 10; i++)
                            {
                                IntPtr skillAddr = skillAddress + (i * 4);
                                WriteMemory(processHandle, skillAddr, BitConverter.GetBytes(300));
                            }
                            Console.WriteLine("Max Skills applied.");
                            break;
                        case ConsoleKey.F4:
                            running = false;
                            break;
                    }
                }
                Thread.Sleep(100);
            }

            CloseHandle(processHandle);
            Console.WriteLine("Trainer exiting.");
        }

        /// <summary>
        /// Writes bytes to the target process memory at the specified address.
        /// </summary>
        private static void WriteMemory(IntPtr processHandle, IntPtr address, byte[] data)
        {
            if (!WriteProcessMemory(processHandle, address, data, data.Length, out int bytesWritten))
            {
                Console.WriteLine($"Failed to write memory at 0x{address.ToString("X")}");
            }
        }
    }
}
