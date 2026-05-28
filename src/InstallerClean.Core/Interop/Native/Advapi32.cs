using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace InstallerClean.Interop.Native;

/// <summary>
/// P/Invoke surface for advapi32.dll. Used only by
/// <see cref="Helpers.UnelevatedLauncher"/>.
/// </summary>
internal static partial class Advapi32
{
    private const string Library = "advapi32.dll";

#if NET7_0_OR_GREATER
    [LibraryImport(Library, EntryPoint = "OpenProcessToken", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool OpenProcessToken(
        SafeProcessHandle processHandle,
        uint desiredAccess,
        out SafeAccessTokenHandle tokenHandle);

    [LibraryImport(Library, EntryPoint = "DuplicateTokenEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool DuplicateTokenEx(
        SafeAccessTokenHandle existingToken,
        uint desiredAccess,
        IntPtr tokenAttributes,
        SecurityImpersonationLevel impersonationLevel,
        TokenType tokenType,
        out SafeAccessTokenHandle newToken);

    [LibraryImport(Library, EntryPoint = "CreateProcessWithTokenW", SetLastError = true,
                   StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CreateProcessWithTokenW(
        SafeAccessTokenHandle token,
        uint logonFlags,
        string applicationName,
        string commandLine,
        uint creationFlags,
        IntPtr environment,
        string? currentDirectory,
        ref STARTUPINFO startupInfo,
        out PROCESS_INFORMATION processInformation);
#else
    [DllImport(Library, EntryPoint = "OpenProcessToken", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool OpenProcessToken(
        SafeProcessHandle processHandle,
        uint desiredAccess,
        out SafeAccessTokenHandle tokenHandle);

    [DllImport(Library, EntryPoint = "DuplicateTokenEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DuplicateTokenEx(
        SafeAccessTokenHandle existingToken,
        uint desiredAccess,
        IntPtr tokenAttributes,
        SecurityImpersonationLevel impersonationLevel,
        TokenType tokenType,
        out SafeAccessTokenHandle newToken);

    [DllImport(Library, EntryPoint = "CreateProcessWithTokenW", SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CreateProcessWithTokenW(
        SafeAccessTokenHandle token,
        uint logonFlags,
        string applicationName,
        string commandLine,
        uint creationFlags,
        IntPtr environment,
        string? currentDirectory,
        ref STARTUPINFO startupInfo,
        out PROCESS_INFORMATION processInformation);
#endif

    // CharSet is omitted because [assembly: DisableRuntimeMarshalling]
    // ignores it for managed structs; every string-shaped field below
    // is already IntPtr. A managed string field added here would not
    // get auto-marshalled by the attribute either, so the cue would
    // mislead more than it helps.
    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public uint cb;
        public IntPtr lpReserved;
        public IntPtr lpDesktop;
        public IntPtr lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public ushort wShowWindow;
        public ushort cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }

    public enum SecurityImpersonationLevel
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation,
    }

    public enum TokenType
    {
        TokenPrimary = 1,
        TokenImpersonation,
    }

    public const uint TOKEN_ASSIGN_PRIMARY  = 0x0001;
    public const uint TOKEN_DUPLICATE       = 0x0002;
    public const uint TOKEN_QUERY           = 0x0008;
}
