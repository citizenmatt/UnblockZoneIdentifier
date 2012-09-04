using System.Runtime.InteropServices;

namespace UnblockZoneIdentifier
{
    /// <summary>
    /// Provides methods for getting and setting the security zone for a file
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("cd45f185-1b21-48e2-967b-ead743a8914e")]
    public interface IZoneIdentifier
    {
        /// <summary>
        /// Gets the current security zone
        /// </summary>
        /// <returns>The current security zone</returns>
        UrlZone GetId();

        /// <summary>
        /// Sets the current zone to <see cref="UrlZone.LocalMachine"/>
        /// </summary>
        void Remove();

        /// <summary>
        /// Sets the current security zone
        /// </summary>
        /// <param name="id">The new security zone</param>
        void SetId(UrlZone id);
    }
}