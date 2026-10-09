Imports System
Imports System.IO
Imports System.Text
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class Form1

    Private llbPath As String = String.Empty
    Private latestReport As String = String.Empty
    Private latestRawReport As String = String.Empty
    Private loadingSlots As Boolean = False
    Private selectedSlotToKeep As Integer = 0
    Private WithEvents batteryWorker As BackgroundWorker
    Private selectedSlotNumber As Integer = 0
    Private refreshTimer As System.Windows.Forms.Timer
    Private closingForm As Boolean = False

    Public Event BatteryDataUpdated As EventHandler

    Private Class RefreshRequest
        Public ExeFile As String
        Public IncludeRaw As Boolean
    End Class

    Private Class RefreshResult
        Public Output As String
        Public IncludeRaw As Boolean
    End Class

    Public ReadOnly Property SelectedBatterySlotNumber() As Integer
        Get
            Return selectedSlotNumber
        End Get
    End Property

    Public Function GetSelectedBatteryValue(ByVal fieldName As String) As String
        If selectedSlotNumber <= 0 Then
            Return String.Empty
        End If

        Return GetBatteryValue(selectedSlotNumber, fieldName)
    End Function

    Public Function GetBatteryValue(ByVal slotNumber As Integer, ByVal fieldName As String) As String
        If slotNumber <= 0 OrElse fieldName Is Nothing OrElse fieldName.Trim().Length = 0 Then
            Return String.Empty
        End If

        Dim slotText As String = GetReadableSlotText(latestReport, slotNumber)
        Return ExtractBatteryField(slotText, fieldName)
    End Function

    Private Function GetSelectedBatteryValueOrDefault(ByVal fieldName As String) As String
        Dim value As String = GetSelectedBatteryValue(fieldName)
        If value Is Nothing OrElse value.Trim().Length = 0 Then
            Return "Not reported"
        End If
        Return value
    End Function

    Private Sub UpdateBatteryLabels(ByVal sender As Object, ByVal e As EventArgs) Handles Me.BatteryDataUpdated
        bManufacturer.Text = GetSelectedBatteryValueOrDefault("manufacturer")
        bDevice.Text = GetSelectedBatteryValueOrDefault("name")
        bSerialNumber.Text = GetSelectedBatteryValueOrDefault("serial")
        bUniqueID.Text = GetSelectedBatteryValueOrDefault("unique-id")
        bTech.Text = GetSelectedBatteryValueOrDefault("technology")
        bChem.Text = GetSelectedBatteryValueOrDefault("chemistry")
        bDCapacity.Text = GetSelectedBatteryValueOrDefault("designed-capacity")
        bFCharge.Text = GetSelectedBatteryValueOrDefault("full-charge-capacity")
        bVoltage.Text = GetSelectedBatteryValueOrDefault("voltage")
        bRate.Text = GetSelectedBatteryValueOrDefault("rate")
        bPower.Text = GetSelectedBatteryValueOrDefault("power-state")
        bCurrent.Text = GetSelectedBatteryValueOrDefault("charge-level")
        bCondition.Text = GetSelectedBatteryValueOrDefault("condition")
        bRuntime.Text = GetSelectedBatteryValueOrDefault("runtime")
        TextBox2.Text = GetSelectedBatteryValueOrDefault("system-battery")
        bAlert1.Text = GetSelectedBatteryValueOrDefault("default-alert-1")
        bAlert2.Text = GetSelectedBatteryValueOrDefault("default-alert-2")
        UpdateBatteryProgressBar(GetSelectedBatteryValue("percentage"))
    End Sub

    Private Sub UpdateBatteryProgressBar(ByVal percentageText As String)
        If ProgressBar1 Is Nothing Then
            Return
        End If

        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100

        If percentageText Is Nothing OrElse percentageText.Trim().Length = 0 Then
            ProgressBar1.Value = 0
            Return
        End If

        Dim match As Match = Regex.Match(percentageText, "[-+]?[0-9]+(?:\.[0-9]+)?")
        Dim percentage As Double

        If Not match.Success OrElse Not Double.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, percentage) Then
            ProgressBar1.Value = 0
            Return
        End If

        If percentage < 0 Then
            percentage = 0
        ElseIf percentage > 100 Then
            percentage = 100
        End If

        ProgressBar1.Value = CInt(Math.Round(percentage, MidpointRounding.AwayFromZero))
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            Me.KeyPreview = True
            ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
            SetupRawTextBox()

            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = 100
            ProgressBar1.Value = 0

            llbPath = ExtractLlb()
            batteryWorker = New BackgroundWorker()

            refreshTimer = New System.Windows.Forms.Timer()
            refreshTimer.Interval = 1000
            AddHandler refreshTimer.Tick, AddressOf RefreshTimer_Tick

            StartRefresh(True)
            refreshTimer.Start()
        Catch ex As Exception
            MessageBox.Show("Unable to start Low-Level Battery Info:" & Environment.NewLine & ex.Message, "Low-Level Battery Info", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RefreshTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        If closingForm Then
            Return
        End If

        StartRefresh(False)
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closingForm = True
        If refreshTimer IsNot Nothing Then
            refreshTimer.Stop()
            RemoveHandler refreshTimer.Tick, AddressOf RefreshTimer_Tick
            refreshTimer.Dispose()
            refreshTimer = Nothing
        End If
    End Sub


    Private Sub SetupRawTextBox()
        TextBox1.Multiline = True
        TextBox1.ReadOnly = True
        TextBox1.ScrollBars = ScrollBars.Both
        TextBox1.WordWrap = False
        TextBox1.HideSelection = False
        TextBox1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular)
    End Sub

    Private Function ReadEmbeddedLlbBytes() As Byte()
        Dim currentAssembly As System.Reflection.Assembly = GetType(Form1).Assembly
        Dim resourceNames() As String = currentAssembly.GetManifestResourceNames()
        Dim resourceName As String = String.Empty
        Dim itemName As String

        For Each itemName In resourceNames
            If String.Compare(itemName, "llb.exe", True) = 0 OrElse itemName.EndsWith(".llb.exe", StringComparison.OrdinalIgnoreCase) Then
                resourceName = itemName
                Exit For
            End If
        Next

        If resourceName.Length = 0 Then
            Dim availableResources As New StringBuilder()
            For Each itemName In resourceNames
                availableResources.AppendLine(itemName)
            Next

            Throw New InvalidOperationException("The embedded file 'llb.exe' was not found. Add llb.exe directly to the project, set its Build Action to Embedded Resource, and rebuild the project." & Environment.NewLine & Environment.NewLine & "Embedded resources found:" & Environment.NewLine & availableResources.ToString())
        End If

        Dim resourceStream As Stream = currentAssembly.GetManifestResourceStream(resourceName)
        If resourceStream Is Nothing Then
            Throw New InvalidOperationException("Unable to open embedded resource: " & resourceName)
        End If

        Using inputStream As Stream = resourceStream
            Using memoryStream As New MemoryStream()
                Dim buffer(8191) As Byte
                Dim bytesRead As Integer

                Do
                    bytesRead = inputStream.Read(buffer, 0, buffer.Length)
                    If bytesRead <= 0 Then
                        Exit Do
                    End If
                    memoryStream.Write(buffer, 0, bytesRead)
                Loop

                Return memoryStream.ToArray()
            End Using
        End Using
    End Function

    Private Function ExtractLlb() As String
        Dim localFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim appFolder As String = Path.Combine(localFolder, "AriProject")
        Dim llbFolder As String = Path.Combine(appFolder, "LLB")
        Dim exeFile As String = Path.Combine(llbFolder, "llb.exe")
        Dim exeBytes() As Byte = ReadEmbeddedLlbBytes()

        If exeBytes Is Nothing OrElse exeBytes.Length = 0 Then
            Throw New InvalidOperationException("The embedded file 'llb.exe' is empty. Check that llb.exe is added directly to the project and its Build Action is set to Embedded Resource.")
        End If

        If Not Directory.Exists(llbFolder) Then
            Directory.CreateDirectory(llbFolder)
        End If

        Try
            Dim folderInfo As New DirectoryInfo(llbFolder)
            folderInfo.Attributes = folderInfo.Attributes Or FileAttributes.Hidden
        Catch
        End Try

        If File.Exists(exeFile) Then
            Try
                File.SetAttributes(exeFile, FileAttributes.Normal)
            Catch
            End Try
        End If

        File.WriteAllBytes(exeFile, exeBytes)

        Try
            File.SetAttributes(exeFile, FileAttributes.Hidden)
        Catch
        End Try

        Return exeFile
    End Function

    Private Sub StartRefresh(ByVal includeRaw As Boolean)
        If closingForm OrElse batteryWorker Is Nothing OrElse batteryWorker.IsBusy Then
            Return
        End If

        selectedSlotToKeep = GetSelectedSlotNumber()

        If includeRaw Then
            TextBox1.Text = "Reading raw battery data..." & Environment.NewLine & "Please wait."
        End If

        Dim request As New RefreshRequest()
        request.ExeFile = llbPath
        request.IncludeRaw = includeRaw

        Try
            batteryWorker.RunWorkerAsync(request)
        Catch ex As Exception
            If includeRaw OrElse latestReport.Length = 0 Then
                TextBox1.Text = "Unable to read raw battery data." & Environment.NewLine & ex.Message
            End If
        End Try
    End Sub

    Private Sub batteryWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles batteryWorker.DoWork
        Dim request As RefreshRequest = CType(e.Argument, RefreshRequest)
        Dim arguments As String = "--once --no-pause"

        If Not request.IncludeRaw Then
            arguments &= " --no-raw"
        End If

        Dim result As New RefreshResult()
        result.Output = RunLlb(request.ExeFile, arguments)
        result.IncludeRaw = request.IncludeRaw
        e.Result = result
    End Sub

    Private Sub batteryWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles batteryWorker.RunWorkerCompleted
        If closingForm OrElse Me.IsDisposed OrElse Me.Disposing Then
            Return
        End If

        If e.Error IsNot Nothing Then
            If latestReport.Length = 0 Then
                loadingSlots = True
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("No battery data")
                ComboBox1.SelectedIndex = 0
                loadingSlots = False
                ComboBox1.Enabled = False
                selectedSlotNumber = 0
                RaiseEvent BatteryDataUpdated(Me, EventArgs.Empty)
            End If

            If latestRawReport.Length = 0 OrElse TextBox1.Text.Length = 0 OrElse TextBox1.Text.StartsWith("Reading raw battery data", StringComparison.OrdinalIgnoreCase) Then
                TextBox1.Text = "Unable to read battery data." & Environment.NewLine & e.Error.Message
            End If
            Return
        End If

        Dim result As RefreshResult = CType(e.Result, RefreshResult)
        latestReport = If(result.Output, String.Empty)

        If result.IncludeRaw Then
            latestRawReport = latestReport
        End If

        Dim slotNumbers As List(Of Integer) = FindSlots(latestReport)
        UpdateSlotList(slotNumbers)

        If slotNumbers.Count = 0 Then
            selectedSlotNumber = 0
            If result.IncludeRaw Then
                TextBox1.Text = "No battery slot data found in the LLB report."
            End If
            RaiseEvent BatteryDataUpdated(Me, EventArgs.Empty)
            Return
        End If

        selectedSlotNumber = GetSelectedSlotNumber()

        If result.IncludeRaw Then
            ShowRawDataForSlot(selectedSlotNumber, GetSlotLabel(latestReport, selectedSlotNumber))
        End If

        RaiseEvent BatteryDataUpdated(Me, EventArgs.Empty)
    End Sub

    Private Sub UpdateSlotList(ByVal slotNumbers As List(Of Integer))
        Dim newItems As New List(Of String)()
        Dim slotNumber As Integer

        For Each slotNumber In slotNumbers
            newItems.Add(GetSlotLabel(latestReport, slotNumber))
        Next

        If newItems.Count = 0 Then
            newItems.Add("No battery found")
        End If

        Dim itemsChanged As Boolean = (ComboBox1.Items.Count <> newItems.Count)
        Dim i As Integer

        If Not itemsChanged Then
            For i = 0 To newItems.Count - 1
                If String.Compare(CStr(ComboBox1.Items(i)), newItems(i), StringComparison.Ordinal) <> 0 Then
                    itemsChanged = True
                    Exit For
                End If
            Next
        End If

        If itemsChanged Then
            loadingSlots = True
            ComboBox1.BeginUpdate()
            Try
                ComboBox1.Items.Clear()
                For Each item As String In newItems
                    ComboBox1.Items.Add(item)
                Next

                If slotNumbers.Count = 0 Then
                    ComboBox1.SelectedIndex = 0
                    ComboBox1.Enabled = False
                Else
                    ComboBox1.Enabled = True
                    Dim indexToSelect As Integer = 0

                    If selectedSlotToKeep > 0 Then
                        For i = 0 To ComboBox1.Items.Count - 1
                            Dim foundSlot As Integer = 0
                            If TryGetSlotNumber(CStr(ComboBox1.Items(i)), foundSlot) AndAlso foundSlot = selectedSlotToKeep Then
                                indexToSelect = i
                                Exit For
                            End If
                        Next
                    End If

                    ComboBox1.SelectedIndex = indexToSelect
                End If
            Finally
                ComboBox1.EndUpdate()
                loadingSlots = False
            End Try
        Else
            ComboBox1.Enabled = (slotNumbers.Count > 0)
        End If

        selectedSlotNumber = GetSelectedSlotNumber()
    End Sub

    Private Function RunLlb(ByVal exeFile As String, ByVal arguments As String) As String
        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = exeFile
        startInfo.Arguments = arguments
        startInfo.WorkingDirectory = Path.GetDirectoryName(exeFile)
        startInfo.UseShellExecute = False
        startInfo.CreateNoWindow = True
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardError = True

        Dim process As Process = Nothing
        Try
            process = Process.Start(startInfo)

            If process Is Nothing Then
                Throw New InvalidOperationException("Windows could not start llb.exe.")
            End If

            Dim output As String = process.StandardOutput.ReadToEnd()
            Dim errorOutput As String = process.StandardError.ReadToEnd()
            process.WaitForExit()

            If errorOutput IsNot Nothing AndAlso errorOutput.Length > 0 Then
                If output.Length > 0 Then
                    output &= Environment.NewLine
                End If
                output &= errorOutput
            End If

            If output.Length = 0 Then
                output = "llb.exe returned no output. Exit code: " & process.ExitCode.ToString()
            ElseIf process.ExitCode <> 0 Then
                output &= Environment.NewLine & "llb.exe exit code: " & process.ExitCode.ToString()
            End If

            Return output
        Finally
            If process IsNot Nothing Then
                process.Close()
            End If
        End Try
    End Function

    Private Function FindSlots(ByVal report As String) As List(Of Integer)
        Dim slotNumbers As New List(Of Integer)()
        Dim lines() As String = SplitLines(report)
        Dim line As String

        For Each line In lines
            Dim slotNumber As Integer = 0
            If TryGetSlotNumber(line, slotNumber) Then
                If Not slotNumbers.Contains(slotNumber) Then
                    slotNumbers.Add(slotNumber)
                End If
            End If
        Next

        slotNumbers.Sort()
        Return slotNumbers
    End Function

    Private Function TryGetSlotNumber(ByVal line As String, ByRef slotNumber As Integer) As Boolean
        slotNumber = 0
        If line Is Nothing Then
            Return False
        End If

        Dim trimmed As String = line.Trim()
        Dim startIndex As Integer = -1

        If trimmed.StartsWith("SLOT #", StringComparison.OrdinalIgnoreCase) Then
            startIndex = 6
        ElseIf trimmed.StartsWith("#", StringComparison.Ordinal) Then
            startIndex = 1
        Else
            Return False
        End If

        If startIndex >= trimmed.Length OrElse Not Char.IsDigit(trimmed.Chars(startIndex)) Then
            Return False
        End If

        Dim endIndex As Integer = startIndex
        While endIndex < trimmed.Length AndAlso Char.IsDigit(trimmed.Chars(endIndex))
            endIndex += 1
        End While

        Return Integer.TryParse(trimmed.Substring(startIndex, endIndex - startIndex), slotNumber) AndAlso slotNumber > 0
    End Function

    Private Function SplitLines(ByVal value As String) As String()
        If value Is Nothing Then
            Return New String() {}
        End If

        Return value.Replace(vbCrLf, vbLf).Split(New Char() {ControlChars.Lf})
    End Function

    Private Function GetSelectedSlotNumber() As Integer
        If ComboBox1.SelectedIndex < 0 OrElse ComboBox1.SelectedItem Is Nothing Then
            Return 0
        End If

        Dim slotNumber As Integer = 0
        If TryGetSlotNumber(CStr(ComboBox1.SelectedItem), slotNumber) Then
            Return slotNumber
        End If

        Return 0
    End Function

    Private Function IsRawSlotHeader(ByVal line As String, ByRef slotNumber As Integer) As Boolean
        slotNumber = 0
        If line Is Nothing Then
            Return False
        End If

        Dim trimmed As String = line.Trim()
        If Not trimmed.StartsWith("SLOT #", StringComparison.OrdinalIgnoreCase) Then
            Return False
        End If

        If trimmed.IndexOf("RAW DATA", StringComparison.OrdinalIgnoreCase) < 0 Then
            Return False
        End If

        Return TryGetSlotNumber(trimmed, slotNumber)
    End Function

    Private Function GetSlotLabel(ByVal report As String, ByVal slotNumber As Integer) As String
        Dim slotText As String = GetReadableSlotText(report, slotNumber)
        Dim deviceName As String = GetValueFromText(slotText, "Device name")
        Dim manufacturer As String = GetValueFromText(slotText, "Battery manufacturer")

        If deviceName.Length = 0 OrElse String.Compare(deviceName, "Not reported", True) = 0 Then
            deviceName = manufacturer
        End If

        If deviceName.Length = 0 Then
            deviceName = "Battery"
        End If

        Dim label As String = "#" & slotNumber.ToString() & " " & deviceName
        If manufacturer.Length > 0 AndAlso String.Compare(deviceName, manufacturer, True) <> 0 AndAlso String.Compare(manufacturer, "Not reported", True) <> 0 Then
            label &= " (" & manufacturer & ")"
        End If

        Return label
    End Function

    Private Function GetValueFromText(ByVal text As String, ByVal fieldName As String) As String
        If text Is Nothing OrElse text.Length = 0 Then
            Return String.Empty
        End If

        Dim lines() As String = SplitLines(text)
        Dim line As String
        For Each line In lines
            Dim trimmed As String = line.Trim()
            Dim colonIndex As Integer = trimmed.IndexOf(":"c)
            If colonIndex > 0 Then
                Dim label As String = trimmed.Substring(0, colonIndex).Trim()
                If String.Compare(label, fieldName, True) = 0 Then
                    Return trimmed.Substring(colonIndex + 1).Trim()
                End If
            End If
        Next

        Return String.Empty
    End Function

    Private Function GetReadableSlotText(ByVal report As String, ByVal slotNumber As Integer) As String
        Dim lines() As String = SplitLines(report)
        Dim output As New StringBuilder()
        Dim inSelectedSlot As Boolean = False
        Dim i As Integer

        For i = 0 To lines.Length - 1
            Dim trimmed As String = lines(i).Trim()

            If trimmed.StartsWith("RAW DEVICE DATA", StringComparison.OrdinalIgnoreCase) Then
                Exit For
            End If

            Dim currentSlot As Integer = 0
            If TryGetSlotNumber(lines(i), currentSlot) Then
                If inSelectedSlot AndAlso currentSlot <> slotNumber Then
                    Exit For
                End If

                If Not inSelectedSlot AndAlso currentSlot = slotNumber Then
                    inSelectedSlot = True
                End If
            End If

            If inSelectedSlot Then
                output.AppendLine(lines(i))
            End If
        Next

        Return output.ToString()
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If loadingSlots Then
            Return
        End If

        ShowSelectedSlot()
    End Sub

    Private Sub ShowSelectedSlot()
        Dim slotNumber As Integer = GetSelectedSlotNumber()
        If slotNumber <= 0 Then
            selectedSlotNumber = 0
            RaiseEvent BatteryDataUpdated(Me, EventArgs.Empty)
            Return
        End If

        selectedSlotNumber = slotNumber

        Dim slotLabel As String = GetSlotLabel(latestReport, slotNumber)
        If latestRawReport.Length > 0 Then
            ShowRawDataForSlot(slotNumber, slotLabel)
        End If
        RaiseEvent BatteryDataUpdated(Me, EventArgs.Empty)
    End Sub

    Private Function ExtractBatteryField(ByVal slotText As String, ByVal requestedField As String) As String
        If slotText Is Nothing OrElse slotText.Length = 0 Then
            Return String.Empty
        End If

        Dim key As String = requestedField.Trim().ToLowerInvariant()
        key = key.Replace("_", "-").Replace(" ", "-")

        Dim label As String = String.Empty
        Select Case key
            Case "manufacturer", "battery-manufacturer"
                label = "Battery manufacturer"
            Case "device-name", "name", "battery-name"
                label = "Device name"
            Case "serial-number", "serial"
                label = "Serial number"
            Case "unique-id", "unique-identifier"
                label = "Unique identifier"
            Case "technology", "battery-technology"
                label = "Battery technology"
            Case "chemistry", "battery-chemistry"
                label = "Battery chemistry"
            Case "designed-capacity", "design-capacity"
                label = "Designed capacity"
            Case "full-charge-capacity", "full-charged-capacity"
                label = "Full charged capacity"
            Case "current-capacity"
                label = "Current capacity"
            Case "charge-level", "percentage", "percent"
                label = "Reported charge level"
            Case "voltage", "terminal-voltage"
                label = "Terminal voltage"
            Case "rate", "charge-rate", "charge-discharge-rate"
                label = "Charge/discharge rate"
            Case "power-state"
                label = "Power state"
            Case "cycle-count"
                label = "Reported charge/discharge cycle count"
            Case "capacity-units", "capacity-unit"
                label = "Capacity units"
            Case "system-battery"
                label = "System battery"
            Case "relative-capacity", "relative-capacity-units"
                label = "Relative capacity units"
            Case "short-term-battery", "fail-safe-battery"
                label = "Short-term or fail-safe battery"
            Case "sealed-battery"
                label = "Sealed battery"
            Case "default-alert-1"
                label = "Default alert level 1"
            Case "default-alert-2"
                label = "Default alert level 2"
            Case "critical-bias"
                label = "Critical bias"
            Case "manufacturing-date"
                label = "Manufacturing date"
            Case "temperature", "battery-temperature"
                label = "Battery temperature"
            Case "estimated-runtime", "estimated-remaining-runtime"
                label = "Estimated remaining runtime"
            Case "capacity-scales", "capacity-reporting-scales"
                Return GetCapacityScaleText(slotText)
            Case "condition", "battery-condition"
                Dim conditionValue As String = GetValueFromText(slotText, "Battery condition")
                If conditionValue.Length = 0 Then
                    conditionValue = GetValueFromText(slotText, "Assessment")
                End If
                If conditionValue.Length = 0 Then
                    conditionValue = GetValueFromText(slotText, "Condition")
                End If
                Return conditionValue
            Case Else
                label = requestedField.Trim()
        End Select

        Return GetValueFromText(slotText, label)
    End Function

    Private Function GetCapacityScaleText(ByVal slotText As String) As String
        Dim lines() As String = SplitLines(slotText)
        Dim output As New StringBuilder()
        Dim inScaleBlock As Boolean = False
        Dim line As String

        For Each line In lines
            Dim trimmed As String = line.Trim()
            If trimmed.StartsWith("Capacity reporting scales:", StringComparison.OrdinalIgnoreCase) Then
                inScaleBlock = True
                output.AppendLine(trimmed.Substring(trimmed.IndexOf(":"c) + 1).Trim())
                Continue For
            End If

            If inScaleBlock Then
                If trimmed.StartsWith("Scale #", StringComparison.OrdinalIgnoreCase) Then
                    output.AppendLine(trimmed)
                ElseIf trimmed.Length > 0 AndAlso Not trimmed.StartsWith(" ", StringComparison.Ordinal) Then
                    If trimmed.IndexOf(":"c) >= 0 Then
                        Exit For
                    End If
                End If
            End If
        Next

        Return output.ToString().Trim()
    End Function

    Private Sub ShowRawDataForSlot(ByVal selectedSlot As Integer, ByVal slotLabel As String)
        If TextBox1 Is Nothing Then
            Return
        End If

        If selectedSlot <= 0 Then
            TextBox1.Text = "No battery slot is selected."
            Return
        End If

        If latestRawReport Is Nothing OrElse latestRawReport.Length = 0 Then
            TextBox1.Text = "No raw report has been captured yet. Press F5 to refresh raw data."
            Return
        End If

        Dim lines() As String = SplitLines(latestRawReport)
        Dim rawStart As Integer = -1
        Dim i As Integer

        For i = 0 To lines.Length - 1
            If lines(i).Trim().StartsWith("RAW DEVICE DATA", StringComparison.OrdinalIgnoreCase) Then
                rawStart = i
                Exit For
            End If
        Next

        If rawStart < 0 Then
            TextBox1.Text = "The LLB report does not contain a RAW DEVICE DATA section." & Environment.NewLine & _
                            "Press F5 to collect a new report."
            Return
        End If

        Dim output As New StringBuilder()
        Dim currentSlot As Integer = 0
        Dim selectedSlotFound As Boolean = False
        Dim selectedSlotFinished As Boolean = False

        For i = rawStart To lines.Length - 1
            Dim originalLine As String = lines(i)
            Dim trimmed As String = originalLine.Trim()

            If trimmed.StartsWith("SLOT #", StringComparison.OrdinalIgnoreCase) AndAlso _
               trimmed.IndexOf("RAW DATA", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim foundSlot As Integer = 0
                If IsRawSlotHeader(trimmed, foundSlot) Then
                    If selectedSlotFound AndAlso foundSlot <> selectedSlot Then
                        selectedSlotFinished = True
                        Exit For
                    End If

                    currentSlot = foundSlot
                    If currentSlot = selectedSlot Then
                        selectedSlotFound = True
                        output.AppendLine(originalLine)
                    End If
                    Continue For
                End If
            End If

            If currentSlot = 0 Then
                output.AppendLine(originalLine)
            ElseIf currentSlot = selectedSlot Then
                output.AppendLine(originalLine)
            End If
        Next

        If Not selectedSlotFound Then
            output.Length = 0
            output.AppendLine("RAW DEVICE DATA")
            output.AppendLine("No raw data was found for " & slotLabel & ".")
            output.AppendLine("Press F5 to refresh the raw report.")
        ElseIf selectedSlotFinished Then
            output.AppendLine()
            output.AppendLine("Displayed raw data for " & slotLabel & ".")
            output.AppendLine("Press F5 to refresh the raw data manually.")
        End If

        TextBox1.Text = output.ToString()
        TextBox1.SelectionStart = 0
        TextBox1.SelectionLength = 0
        TextBox1.ScrollToCaret()
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F5 Then
            e.Handled = True
            StartRefresh(True)
        End If
    End Sub

End Class
