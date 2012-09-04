using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace UnblockZoneIdentifier
{
    class Program
    {
        private const int AccessDenied = unchecked((int)0x80004005);

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("UnblockZoneIdentifier (c) 2012 Matt Ellis");
                Console.WriteLine("Removes the internet zone identifier from downloaded files");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("\tUnblockZoneIdentifier.exe <filenames>");
                Console.WriteLine();
                return 0;
            }

            var currentDiretory = Directory.GetCurrentDirectory();

            foreach (var filename in args)
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Cannot find file: «{0}»", filename);
                    continue;
                }

                var fullFilename = Path.Combine(currentDiretory, filename);

                var persistentZoneIdentifier = new PersistentZoneIdentifier();
                var persistFile = (IPersistFile) persistentZoneIdentifier;
                try
                {
                    persistFile.Load(fullFilename, (int)(STGM.READWRITE | STGM.SHARE_EXCLUSIVE));
                }
                catch(FileNotFoundException e)
                {
                    // When calling persistFile.Load, the object tries to open filename:Zone.Identifier
                    // So, if the file doesn't have an identifier, we get a file not found, and there's
                    // nothing more we can do. Since we've tried to open the alternate data stream, we
                    // can't seem to set the identifier, either. I think you need to use 
                    // IAttachmentExecute.SetSource to set the url which dictates the security zone
                    Console.WriteLine("File «{0}» is not blocked", fullFilename);
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error opening «{0}»: {1}", fullFilename, e.Message);
                    continue;
                }

                var zoneIdentifier = (IZoneIdentifier)persistentZoneIdentifier;
                var zone = zoneIdentifier.GetId();
                if (zone == UrlZone.LocalMachine)
                {
                    Console.WriteLine("File «{0}» is not blocked", fullFilename);
                    continue;
                }

                // zoneIdentifier.Remove doesn't work, failing with an access denied, I have no idea why.
                // Calling SetId and Save opens the alternate data stream and deletes it just fine
                zoneIdentifier.SetId(UrlZone.LocalMachine);

                try
                {
                    persistFile.Save(fullFilename, true);

                    Console.WriteLine("File «{0}» ({1}) has been unblocked", fullFilename, zone);
                }
                catch (COMException e)
                {
                    if (e.ErrorCode == AccessDenied)
                        Console.WriteLine("Cannot update «{0}» - check permissions, or run as administrator", fullFilename);
                    else
                        Console.WriteLine("Error updating «{0}»: {1}", filename, e.Message);
                }
                finally
                {
                    Marshal.ReleaseComObject(persistFile);
                    Marshal.ReleaseComObject(zoneIdentifier);
                }
            }

            return 0;
        }
    }
}
