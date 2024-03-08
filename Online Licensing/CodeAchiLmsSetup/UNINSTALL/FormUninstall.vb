Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports Microsoft.Win32.RegistryKey

Public Class FormUninstall
    Dim p_name As String
    Dim isUninstall As Boolean
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myResult As DialogResult = MessageBox.Show("Are you sure you want to uninstall the software?", "Uninstaller", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If myResult = DialogResult.Yes Then
            Dim myProcess As Process
            For Each myProcess In Process.GetProcessesByName((Application.ProductName).Substring(0, Len(Application.ProductName) - 2))
                Try
                    Dim myResult1 As DialogResult = MessageBox.Show("Program is still running!!!" + vbCrLf + "Do you still want to continue?", "Uninstaller", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If myResult1 = DialogResult.Yes Then
                        myProcess.Kill()
                        myProcess.WaitForExit()
                    Else
                        Application.Exit()
                        Exit Sub
                    End If
                Catch ex As Exception
                    'skip
                End Try
            Next

            Dim sFile As String
            For Each sFile In Directory.GetFiles(Application.StartupPath)
                Try
                    File.Delete(sFile)
                Catch

                End Try
            Next sFile

            For Each sFile In Directory.GetDirectories(Application.StartupPath)
                Try
                    Directory.Delete(sFile, True)
                Catch

                End Try
            Next sFile
            Dim deskShortcutPath As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
            Dim strtmenuPath As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)
            If File.Exists(deskShortcutPath + "\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2) + ".lnk") Then
                File.Delete(deskShortcutPath + "\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2) + ".lnk")
            End If
            If Directory.Exists(strtmenuPath + "\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2)) Then
                Directory.Delete(strtmenuPath + "\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2), True)
            End If
            Dim regLocation As String = "Software\Microsoft\Windows\CurrentVersion\Uninstall\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2)
            Dim exeLocation As String = ""
            Using regKey As Microsoft.Win32.RegistryKey = Registry.LocalMachine.OpenSubKey(regLocation)
                If regKey IsNot Nothing Then
                    Try
                        Dim regObj As Object = regKey.GetValue("InstallLocation")
                        exeLocation = regObj.ToString()
                        My.Computer.Registry.LocalMachine.DeleteSubKey("Software\Microsoft\Windows\CurrentVersion\Uninstall\" + Application.ProductName.Substring(0, Len(Application.ProductName) - 2), True)
                    Catch ex As Exception

                    End Try
                End If
            End Using
            Dim cmdLines As New System.Text.StringBuilder
            cmdLines.AppendLine("cd " & exeLocation)
            cmdLines.AppendLine("del UNINSTALL.exe")
            cmdLines.AppendLine("cd..")
            cmdLines.AppendLine("rmdir '" & exeLocation & "'")
            IO.File.WriteAllText(Path.GetTempPath & "uninstall.bat", cmdLines.ToString())
            isUninstall = True
            'Application.Exit()
        Else
            'Application.Exit()
            isUninstall = False
        End If

        'Dim data As String = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\CodeAchi Library Management System\LMS.js")
        'Dim byteList As List(Of Byte) = New List(Of Byte)()
        'For i As Integer = 0 To data.Length - 1 Step 8
        '    byteList.Add(Convert.ToByte(data.Substring(i, 8), 2))
        'Next
        Dim macAddress As String
        Dim byteData As Byte() = Encoding.UTF8.GetBytes(Application.ProductName.Replace("UI", ""))
        Dim compName As Byte() = Encoding.UTF8.GetBytes("CodeAchi")
        Dim newappRegkey As RegistryKey
        Dim appregKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\" + Convert.ToBase64String(compName) + "\" + Convert.ToBase64String(byteData), True)
        If appregKey Is Nothing Then

            If Environment.Is64BitProcess Then
                newappRegkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + "\" + Convert.ToBase64String(byteData), True) 'For 32 bit machine
                Dim o1 As Object = newappRegkey.GetValue("Data1")
                byteData = Convert.FromBase64String(o1.ToString())
                macAddress = Encoding.UTF8.GetString(byteData)
            Else
                newappRegkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\" + Convert.ToBase64String(compName) + "\" + Convert.ToBase64String(byteData), True) ' for 64 bit machine
                Dim o1 As Object = newappRegkey.GetValue("Data1")
                byteData = Convert.FromBase64String(o1.ToString())
                macAddress = Encoding.UTF8.GetString(byteData)
            End If
        Else
            Dim o1 As Object = appregKey.GetValue("Data1")
            byteData = Convert.FromBase64String(o1.ToString())
            macAddress = Encoding.UTF8.GetString(byteData)
        End If
        Try
            Dim queryToUpdate As String
            Dim webRequest As WebRequest
            Dim webResponse As WebResponse
            Dim productName As String
            productName = Application.ProductName.Replace("UI", "")
            queryToUpdate = "update installationDetails set productUninstalled='True' where productName='" + productName + "' and mac='" + macAddress + "'"
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
            webRequest = WebRequest.Create("https://codeachi.com/Product/LMS/UpdateData.php?Q=" & queryToUpdate)
            webRequest.Timeout = 8000
            webResponse = webRequest.GetResponse()

            Dim queryToCheck = "SELECT installUrl1,installUrl2 FROM installUninstallUrl WHERE productName='" + Application.ProductName.Replace("UI", "") + "'"
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
            webRequest = WebRequest.Create("https://codeachi.com/Product/LMS/SelectData.php?Q=" + queryToCheck)
            webRequest.Timeout = 8000
            Dim dataStream = webResponse.GetResponseStream()
            Dim strmReader = New StreamReader(dataStream)
            Dim requestResult As String = strmReader.ReadLine()
            If requestResult <> "" Then
                Dim splitData() As String = requestResult.Split("$")
                Try
                    Process.Start(splitData(0))
                Catch ex As Exception

                End Try
                Try
                    Process.Start(splitData(1))
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
        Me.Refresh()
        Application.Exit()
    End Sub

    Public Function GetMACAddress() As String
        Dim nics As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        Dim sMacAddress As String = String.Empty

        For Each adapter As NetworkInterface In nics

            If sMacAddress = String.Empty Then
                Dim properties As IPInterfaceProperties = adapter.GetIPProperties()
                sMacAddress = adapter.GetPhysicalAddress().ToString()
            End If
        Next

        sMacAddress = Regex.Replace(sMacAddress, ".{2}", "$0:")
        sMacAddress = sMacAddress.Remove(sMacAddress.Length - 1, 1)
        Return sMacAddress
    End Function

    Private Sub FormUninstall_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isUninstall = True Then
            Try
                Process.Start(Path.GetTempPath & "uninstall.bat")
            Catch ex As Exception
                Application.Exit()
            End Try
        End If
    End Sub
End Class
