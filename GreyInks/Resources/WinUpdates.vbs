WScript.Echo 
WScript.Echo "������ ������Ʈ ��ũ��Ʈ (�����̽ʻ�(��) - �ý��ۺ�����Ʈ)"
WScript.Echo 
WScript.Echo 

SET UpdateSession = CreateObject("Microsoft.Update.Session")
UpdateSession.ClientApplicationID = "Yes24 Corp."
SET UpdateSearcher = UpdateSession.CreateUpdateSearcher()
SET SearchResult = UpdateSearcher.Search("IsInstalled=0 and Type='Software' and IsHidden=0")


If SearchResult.Updates.Count = 0 Then
WScript.Echo "[#] ���� ������ ������Ʈ ����� ����"
WScript.Quit
End IF

WSCRIPT.ECHO vbCRLF & _
"[!] ���� ������ ������Ʈ ��� �Դϴ�."
FOR I = 0 TO SearchResult.Updates.Count-1
	SET Update = SearchResult.Updates.Item(I)
	WSCRIPT.ECHO I+1 & ") " & Update.Title
NEXT

WSCRIPT.ECHO vbCRLF & _
"[!] �ٿ�ε带 ���� �÷����� �����մϴ�."
SET UpdatesToDownload = CreateObject("Microsoft.Update.UpdateColl")
FOR I = 0 TO SearchResult.Updates.Count-1
	SET Update = SearchResult.Updates.Item(I)

	IF Update.InstallationBehavior.CanRequestUserInput = False THEN
		IF Update.EulaAccepted = False THEN
			Update.AcceptEula()
			UpdatesToDownload.Add(Update)
		ELSE 
			UpdatesToDownload.Add(Update)
		END IF 
	END IF
NEXT

SET Downloader = UpdateSession.CreateUpdateDownloader() 
Downloader.Updates = UpdatesToDownload
Downloader.Download()

SET UpdatesToInstall = CreateObject("Microsoft.Update.UpdateColl")
FOR I = 0 TO SearchResult.Updates.Count-1
	SET Update = SearchResult.Updates.Item(I)
	IF Update.IsDownloaded = True THEN 
		UpdatesToInstall.Add(Update)
	END IF
NEXT

SET Installer = UpdateSession.CreateUpdateInstaller()
Installer.Updates = UpdatesToInstall
SET InstallationResult = Installer.Install()
WSCRIPT.ECHO InstallationResult

WScript.Echo "Press [ENTER] to continue..."
WScript.StdIn.ReadLine

' Read dummy input. This call will not return until [ENTER] is pressed.

