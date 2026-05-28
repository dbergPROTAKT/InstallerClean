using System.Runtime.InteropServices;
#if NET7_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

namespace InstallerClean.Interop.Native;

/// <summary>
/// P/Invoke surface for msi.dll (Windows Installer API). All entry
/// points are the Unicode ("W") variants.
/// </summary>
internal static partial class Msi
{
    private const string Library = "msi.dll";

    public const int GuidBufferLength = 39;

#if NET7_0_OR_GREATER
    [LibraryImport(Library, EntryPoint = "MsiEnumProductsExW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiEnumProductsEx(
        string? szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        uint dwIndex,
        [MarshalUsing(ConstantElementCount = GuidBufferLength)] char[]? szInstalledProductCode,
        out MsiInstallContext pdwInstalledContext,
        [MarshalUsing(CountElementName = nameof(pcchSid))] char[]? szSid,
        ref uint pcchSid);

    [LibraryImport(Library, EntryPoint = "MsiGetProductInfoExW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiGetProductInfoEx(
        string szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        string szProperty,
        [MarshalUsing(CountElementName = nameof(pcchValue))] char[]? szValue,
        ref uint pcchValue);

    [LibraryImport(Library, EntryPoint = "MsiEnumPatchesExW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiEnumPatchesEx(
        string? szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        MsiPatchFilter dwFilter,
        uint dwIndex,
        [MarshalUsing(ConstantElementCount = GuidBufferLength)] char[]? szPatchCode,
        [MarshalUsing(ConstantElementCount = GuidBufferLength)] char[]? szTargetProductCode,
        out MsiInstallContext pdwTargetProductContext,
        [MarshalUsing(CountElementName = nameof(pcchTargetUserSid))] char[]? szTargetUserSid,
        ref uint pcchTargetUserSid);

    [LibraryImport(Library, EntryPoint = "MsiGetPatchInfoExW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiGetPatchInfoEx(
        string szPatchCode,
        string szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        string szProperty,
        [MarshalUsing(CountElementName = nameof(pcchValue))] char[]? szValue,
        ref uint pcchValue);

    [LibraryImport(Library, EntryPoint = "MsiGetSummaryInformationW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiGetSummaryInformation(
        uint hDatabase,
        string? szDatabasePath,
        uint uiUpdateCount,
        out uint phSummaryInfo);

    [LibraryImport(Library, EntryPoint = "MsiSummaryInfoGetPropertyW",
                   StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint MsiSummaryInfoGetProperty(
        uint hSummaryInfo,
        uint uiProperty,
        out uint puiDataType,
        out int piValue,
        IntPtr pftValue,
        [MarshalUsing(CountElementName = nameof(pcchValueBuf))] char[]? szValueBuf,
        ref uint pcchValueBuf);

    [LibraryImport(Library, EntryPoint = "MsiCloseHandle")]
    public static partial uint MsiCloseHandle(uint hAny);
#else
    [DllImport(Library, EntryPoint = "MsiEnumProductsExW", CharSet = CharSet.Unicode)]
    public static extern uint MsiEnumProductsEx(
        string? szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        uint dwIndex,
        [Out] char[]? szInstalledProductCode,
        out MsiInstallContext pdwInstalledContext,
        [Out] char[]? szSid,
        ref uint pcchSid);

    [DllImport(Library, EntryPoint = "MsiGetProductInfoExW", CharSet = CharSet.Unicode)]
    public static extern uint MsiGetProductInfoEx(
        string szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        string szProperty,
        [Out] char[]? szValue,
        ref uint pcchValue);

    [DllImport(Library, EntryPoint = "MsiEnumPatchesExW", CharSet = CharSet.Unicode)]
    public static extern uint MsiEnumPatchesEx(
        string? szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        MsiPatchFilter dwFilter,
        uint dwIndex,
        [Out] char[]? szPatchCode,
        [Out] char[]? szTargetProductCode,
        out MsiInstallContext pdwTargetProductContext,
        [Out] char[]? szTargetUserSid,
        ref uint pcchTargetUserSid);

    [DllImport(Library, EntryPoint = "MsiGetPatchInfoExW", CharSet = CharSet.Unicode)]
    public static extern uint MsiGetPatchInfoEx(
        string szPatchCode,
        string szProductCode,
        string? szUserSid,
        MsiInstallContext dwContext,
        string szProperty,
        [Out] char[]? szValue,
        ref uint pcchValue);

    [DllImport(Library, EntryPoint = "MsiGetSummaryInformationW", CharSet = CharSet.Unicode)]
    public static extern uint MsiGetSummaryInformation(
        uint hDatabase,
        string? szDatabasePath,
        uint uiUpdateCount,
        out uint phSummaryInfo);

    [DllImport(Library, EntryPoint = "MsiSummaryInfoGetPropertyW", CharSet = CharSet.Unicode)]
    public static extern uint MsiSummaryInfoGetProperty(
        uint hSummaryInfo,
        uint uiProperty,
        out uint puiDataType,
        out int piValue,
        IntPtr pftValue,
        [Out] char[]? szValueBuf,
        ref uint pcchValueBuf);

    [DllImport(Library, EntryPoint = "MsiCloseHandle")]
    public static extern uint MsiCloseHandle(uint hAny);
#endif
}
