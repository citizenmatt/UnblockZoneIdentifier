# UnblockZoneIdentifier #

A simple command line utility to remove any ZoneIdentifier marks from files downloaded from the internet.

Usage:

	UnblockZoneIdentifier <filenames>

Internet Explorer introduced Attachment Services in Windows XP Service Pack 2. Attachment Services is a set of COM objects that email clients and browsers can use when saving and opening files downloaded from other computers. When savving the files, the client uses IAttachmentExecute.SetSource to specify the URL the file was retrieved from. This stores the URLs (Internet Explorer) internet zone in an NTFS alternate data stream, which is checked when the file is about to be opened. If this is set to an internet zone, you are prompted before the file is opened.

This is nice, and everything, except .net also uses this flag when deciding on trust levels, which can make life awkward for downloading plugins that suddenly don't work.
