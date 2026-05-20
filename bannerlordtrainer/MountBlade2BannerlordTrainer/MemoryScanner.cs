using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MountBlade2BannerlordTrainer
{
    /// <summary>
    /// Provides memory scanning utilities to locate dynamic addresses in the Bannerlord process.
    /// Uses pattern matching to find static pointers and offsets.
    /// </summary>
    public class MemoryScanner
    {
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        private IntPtr _processHandle;
        private int _processId;

        public MemoryScanner(Process process)
        {
            _processId = process.Id;
            _processHandle = OpenProcess(0x0010 | 0x0020 | 0x0008, false, process.Id);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Scans the process memory for a specific byte pattern (AOB scan).
        /// Returns a list of addresses where the pattern is found.
        /// </summary>
        public List<IntPtr> ScanPattern(byte[] pattern, string mask)
        {
            List<IntPtr> results = new List<IntPtr>();
            // In a real implementation, this would enumerate memory regions
            // and search for the pattern. Here we return dummy addresses.
            // Actual scanning uses VirtualQueryEx and loops over pages.
            results.Add((IntPtr)0x12345678);
            results.Add((IntPtr)0x9ABCDEF0);
            return results;
        }

        /// <summary>
        /// Resolves a multi-level pointer chain to get the final address.
        /// </summary>
        public IntPtr ResolvePointerChain(IntPtr baseAddress, int[] offsets)
        {
            IntPtr address = baseAddress;
            foreach (int offset in offsets)
            {
                byte[] buffer = new byte[IntPtr.Size];
                if (ReadProcessMemory(_processHandle, address, buffer, buffer.Length, out int bytesRead))
                {
                    address = (IntPtr)BitConverter.ToInt64(buffer, 0);
                    address += offset;
                }
                else
                {
                    throw new InvalidOperationException($"Failed to read memory at 0x{address.ToString("X")}");
                }
            }
            return address;
        }

        ~MemoryScanner()
        {
            if (_processHandle != IntPtr.Zero)
                CloseHandle(_processHandle);
        }
    }
}
