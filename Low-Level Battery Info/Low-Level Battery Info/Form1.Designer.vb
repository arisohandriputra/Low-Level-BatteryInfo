<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.bManufacturer = New System.Windows.Forms.TextBox()
        Me.bDevice = New System.Windows.Forms.TextBox()
        Me.bSerialNumber = New System.Windows.Forms.TextBox()
        Me.bUniqueID = New System.Windows.Forms.TextBox()
        Me.bTech = New System.Windows.Forms.TextBox()
        Me.bChem = New System.Windows.Forms.TextBox()
        Me.bDCapacity = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.bFCharge = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.bVoltage = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.bRate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.bPower = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.bCurrent = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.bCondition = New System.Windows.Forms.Label()
        Me.bRuntime = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.bAlert1 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.bAlert2 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.bCritical = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.bShort = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.bCapabilities = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.bRelative = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.lbstatuscharge = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ProgressBar2 = New Low_Level_Battery_Info.BatteryHorizontalProgressBar()
        Me.ProgressBar1 = New Low_Level_Battery_Info.BatteryVerticalProgressBar()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(14, 34)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(238, 22)
        Me.ComboBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label1.Location = New System.Drawing.Point(12, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Manufacturer"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label2.Location = New System.Drawing.Point(12, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 15)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Device/Name"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(744, 550)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(34, 16)
        Me.TextBox1.TabIndex = 9
        Me.TextBox1.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label3.Location = New System.Drawing.Point(12, 159)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 15)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Serial No."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.Location = New System.Drawing.Point(12, 179)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 15)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Unique ID"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label5.Location = New System.Drawing.Point(12, 199)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 15)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Technology"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label6.Location = New System.Drawing.Point(12, 219)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 15)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Chemistry"
        '
        'bManufacturer
        '
        Me.bManufacturer.BackColor = System.Drawing.SystemColors.Control
        Me.bManufacturer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bManufacturer.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bManufacturer.Location = New System.Drawing.Point(218, 64)
        Me.bManufacturer.Name = "bManufacturer"
        Me.bManufacturer.ReadOnly = True
        Me.bManufacturer.Size = New System.Drawing.Size(289, 14)
        Me.bManufacturer.TabIndex = 17
        Me.bManufacturer.TabStop = False
        Me.bManufacturer.Text = "?"
        '
        'bDevice
        '
        Me.bDevice.BackColor = System.Drawing.SystemColors.Control
        Me.bDevice.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bDevice.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bDevice.Location = New System.Drawing.Point(218, 140)
        Me.bDevice.Name = "bDevice"
        Me.bDevice.ReadOnly = True
        Me.bDevice.Size = New System.Drawing.Size(289, 14)
        Me.bDevice.TabIndex = 18
        Me.bDevice.TabStop = False
        Me.bDevice.Text = "?"
        '
        'bSerialNumber
        '
        Me.bSerialNumber.BackColor = System.Drawing.SystemColors.Control
        Me.bSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bSerialNumber.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bSerialNumber.Location = New System.Drawing.Point(218, 160)
        Me.bSerialNumber.Name = "bSerialNumber"
        Me.bSerialNumber.ReadOnly = True
        Me.bSerialNumber.Size = New System.Drawing.Size(440, 14)
        Me.bSerialNumber.TabIndex = 19
        Me.bSerialNumber.TabStop = False
        Me.bSerialNumber.Text = "?"
        '
        'bUniqueID
        '
        Me.bUniqueID.BackColor = System.Drawing.SystemColors.Control
        Me.bUniqueID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bUniqueID.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bUniqueID.Location = New System.Drawing.Point(218, 180)
        Me.bUniqueID.Name = "bUniqueID"
        Me.bUniqueID.ReadOnly = True
        Me.bUniqueID.Size = New System.Drawing.Size(440, 14)
        Me.bUniqueID.TabIndex = 20
        Me.bUniqueID.TabStop = False
        Me.bUniqueID.Text = "?"
        '
        'bTech
        '
        Me.bTech.BackColor = System.Drawing.SystemColors.Control
        Me.bTech.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bTech.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bTech.Location = New System.Drawing.Point(218, 200)
        Me.bTech.Name = "bTech"
        Me.bTech.ReadOnly = True
        Me.bTech.Size = New System.Drawing.Size(440, 14)
        Me.bTech.TabIndex = 21
        Me.bTech.TabStop = False
        Me.bTech.Text = "?"
        '
        'bChem
        '
        Me.bChem.BackColor = System.Drawing.SystemColors.Control
        Me.bChem.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bChem.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bChem.Location = New System.Drawing.Point(218, 220)
        Me.bChem.Name = "bChem"
        Me.bChem.ReadOnly = True
        Me.bChem.Size = New System.Drawing.Size(440, 14)
        Me.bChem.TabIndex = 22
        Me.bChem.TabStop = False
        Me.bChem.Text = "?"
        '
        'bDCapacity
        '
        Me.bDCapacity.BackColor = System.Drawing.SystemColors.Control
        Me.bDCapacity.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bDCapacity.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bDCapacity.Location = New System.Drawing.Point(218, 240)
        Me.bDCapacity.Name = "bDCapacity"
        Me.bDCapacity.ReadOnly = True
        Me.bDCapacity.Size = New System.Drawing.Size(440, 14)
        Me.bDCapacity.TabIndex = 24
        Me.bDCapacity.TabStop = False
        Me.bDCapacity.Text = "?"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label7.Location = New System.Drawing.Point(12, 239)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(112, 15)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Design Capacity"
        '
        'bFCharge
        '
        Me.bFCharge.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.bFCharge.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bFCharge.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bFCharge.ForeColor = System.Drawing.Color.Blue
        Me.bFCharge.Location = New System.Drawing.Point(218, 278)
        Me.bFCharge.Name = "bFCharge"
        Me.bFCharge.ReadOnly = True
        Me.bFCharge.Size = New System.Drawing.Size(203, 15)
        Me.bFCharge.TabIndex = 26
        Me.bFCharge.TabStop = False
        Me.bFCharge.Text = "?"
        Me.bFCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label8.Location = New System.Drawing.Point(12, 277)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(147, 15)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "Full Charge Capacity"
        '
        'bVoltage
        '
        Me.bVoltage.BackColor = System.Drawing.SystemColors.Info
        Me.bVoltage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bVoltage.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bVoltage.Location = New System.Drawing.Point(218, 318)
        Me.bVoltage.Name = "bVoltage"
        Me.bVoltage.ReadOnly = True
        Me.bVoltage.Size = New System.Drawing.Size(203, 15)
        Me.bVoltage.TabIndex = 28
        Me.bVoltage.TabStop = False
        Me.bVoltage.Text = "?"
        Me.bVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Goldenrod
        Me.Label9.Location = New System.Drawing.Point(12, 317)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 15)
        Me.Label9.TabIndex = 27
        Me.Label9.Text = "Voltage"
        '
        'bRate
        '
        Me.bRate.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.bRate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bRate.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bRate.Location = New System.Drawing.Point(218, 338)
        Me.bRate.Name = "bRate"
        Me.bRate.ReadOnly = True
        Me.bRate.Size = New System.Drawing.Size(203, 15)
        Me.bRate.TabIndex = 30
        Me.bRate.TabStop = False
        Me.bRate.Text = "?"
        Me.bRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.Label10.Location = New System.Drawing.Point(12, 337)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(182, 15)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Charging/Discharging Rate"
        '
        'bPower
        '
        Me.bPower.BackColor = System.Drawing.SystemColors.Control
        Me.bPower.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bPower.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bPower.Location = New System.Drawing.Point(218, 357)
        Me.bPower.Name = "bPower"
        Me.bPower.ReadOnly = True
        Me.bPower.Size = New System.Drawing.Size(440, 14)
        Me.bPower.TabIndex = 32
        Me.bPower.TabStop = False
        Me.bPower.Text = "?"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label11.Location = New System.Drawing.Point(12, 356)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(84, 15)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "Power State"
        '
        'bCurrent
        '
        Me.bCurrent.BackColor = System.Drawing.SystemColors.Info
        Me.bCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bCurrent.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bCurrent.Location = New System.Drawing.Point(218, 298)
        Me.bCurrent.Name = "bCurrent"
        Me.bCurrent.ReadOnly = True
        Me.bCurrent.Size = New System.Drawing.Size(203, 15)
        Me.bCurrent.TabIndex = 34
        Me.bCurrent.TabStop = False
        Me.bCurrent.Text = "?"
        Me.bCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label12.Location = New System.Drawing.Point(12, 297)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(119, 15)
        Me.Label12.TabIndex = 33
        Me.Label12.Text = "Current Capacity"
        '
        'bCondition
        '
        Me.bCondition.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bCondition.ForeColor = System.Drawing.Color.Maroon
        Me.bCondition.Location = New System.Drawing.Point(12, 451)
        Me.bCondition.Name = "bCondition"
        Me.bCondition.Size = New System.Drawing.Size(706, 36)
        Me.bCondition.TabIndex = 35
        Me.bCondition.Text = "Power State"
        Me.bCondition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'bRuntime
        '
        Me.bRuntime.BackColor = System.Drawing.SystemColors.Control
        Me.bRuntime.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bRuntime.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bRuntime.Location = New System.Drawing.Point(218, 377)
        Me.bRuntime.Name = "bRuntime"
        Me.bRuntime.ReadOnly = True
        Me.bRuntime.Size = New System.Drawing.Size(440, 14)
        Me.bRuntime.TabIndex = 37
        Me.bRuntime.TabStop = False
        Me.bRuntime.Text = "?"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label13.Location = New System.Drawing.Point(12, 376)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(126, 15)
        Me.Label13.TabIndex = 36
        Me.Label13.Text = "Estimated Runtime"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(218, 102)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(289, 14)
        Me.TextBox2.TabIndex = 39
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = "?"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label14.Location = New System.Drawing.Point(12, 101)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(105, 15)
        Me.Label14.TabIndex = 38
        Me.Label14.Text = "System Battery"
        '
        'bAlert1
        '
        Me.bAlert1.BackColor = System.Drawing.SystemColors.Control
        Me.bAlert1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bAlert1.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bAlert1.Location = New System.Drawing.Point(427, 396)
        Me.bAlert1.Name = "bAlert1"
        Me.bAlert1.ReadOnly = True
        Me.bAlert1.Size = New System.Drawing.Size(80, 14)
        Me.bAlert1.TabIndex = 41
        Me.bAlert1.TabStop = False
        Me.bAlert1.Text = "?"
        Me.bAlert1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label15.Location = New System.Drawing.Point(302, 395)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(119, 15)
        Me.Label15.TabIndex = 40
        Me.Label15.Text = "Capacity Alert 1"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label16.Location = New System.Drawing.Point(513, 396)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(119, 15)
        Me.Label16.TabIndex = 42
        Me.Label16.Text = "Capacity Alert 2"
        '
        'bAlert2
        '
        Me.bAlert2.BackColor = System.Drawing.SystemColors.Control
        Me.bAlert2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bAlert2.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bAlert2.Location = New System.Drawing.Point(638, 396)
        Me.bAlert2.Name = "bAlert2"
        Me.bAlert2.ReadOnly = True
        Me.bAlert2.Size = New System.Drawing.Size(80, 14)
        Me.bAlert2.TabIndex = 43
        Me.bAlert2.TabStop = False
        Me.bAlert2.Text = "?"
        Me.bAlert2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(12, 16)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(123, 14)
        Me.Label17.TabIndex = 45
        Me.Label17.Text = "Select Battery Slot"
        '
        'bCritical
        '
        Me.bCritical.BackColor = System.Drawing.SystemColors.Control
        Me.bCritical.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bCritical.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bCritical.Location = New System.Drawing.Point(218, 396)
        Me.bCritical.Name = "bCritical"
        Me.bCritical.ReadOnly = True
        Me.bCritical.Size = New System.Drawing.Size(77, 14)
        Me.bCritical.TabIndex = 47
        Me.bCritical.TabStop = False
        Me.bCritical.Text = "?"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label18.Location = New System.Drawing.Point(12, 396)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(98, 15)
        Me.Label18.TabIndex = 46
        Me.Label18.Text = "Critical Bias"
        '
        'bShort
        '
        Me.bShort.BackColor = System.Drawing.SystemColors.Control
        Me.bShort.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bShort.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bShort.Location = New System.Drawing.Point(218, 83)
        Me.bShort.Name = "bShort"
        Me.bShort.ReadOnly = True
        Me.bShort.Size = New System.Drawing.Size(289, 14)
        Me.bShort.TabIndex = 49
        Me.bShort.TabStop = False
        Me.bShort.Text = "?"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label19.Location = New System.Drawing.Point(12, 82)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(147, 15)
        Me.Label19.TabIndex = 48
        Me.Label19.Text = "Short-Term/Fail-Safe"
        '
        'bCapabilities
        '
        Me.bCapabilities.BackColor = System.Drawing.SystemColors.Control
        Me.bCapabilities.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bCapabilities.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bCapabilities.Location = New System.Drawing.Point(218, 121)
        Me.bCapabilities.Name = "bCapabilities"
        Me.bCapabilities.ReadOnly = True
        Me.bCapabilities.Size = New System.Drawing.Size(289, 14)
        Me.bCapabilities.TabIndex = 51
        Me.bCapabilities.TabStop = False
        Me.bCapabilities.Text = "?"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label20.Location = New System.Drawing.Point(12, 120)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(91, 15)
        Me.Label20.TabIndex = 50
        Me.Label20.Text = "Capabilities"
        '
        'bRelative
        '
        Me.bRelative.BackColor = System.Drawing.SystemColors.Control
        Me.bRelative.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.bRelative.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bRelative.Location = New System.Drawing.Point(218, 259)
        Me.bRelative.Name = "bRelative"
        Me.bRelative.ReadOnly = True
        Me.bRelative.Size = New System.Drawing.Size(440, 14)
        Me.bRelative.TabIndex = 53
        Me.bRelative.TabStop = False
        Me.bRelative.Text = "?"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label21.Location = New System.Drawing.Point(12, 258)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(126, 15)
        Me.Label21.TabIndex = 52
        Me.Label21.Text = "Relative Capacity"
        '
        'lbstatuscharge
        '
        Me.lbstatuscharge.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lbstatuscharge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbstatuscharge.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbstatuscharge.Location = New System.Drawing.Point(516, 63)
        Me.lbstatuscharge.Name = "lbstatuscharge"
        Me.lbstatuscharge.Size = New System.Drawing.Size(155, 90)
        Me.lbstatuscharge.TabIndex = 54
        Me.lbstatuscharge.Text = "-"
        Me.lbstatuscharge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label22
        '
        Me.Label22.BackColor = System.Drawing.Color.White
        Me.Label22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(516, 29)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(155, 34)
        Me.Label22.TabIndex = 55
        Me.Label22.Text = "Power Source"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 505)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(134, 23)
        Me.Button1.TabIndex = 56
        Me.Button1.Text = "&Show R.A.W Value"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ProgressBar2
        '
        Me.ProgressBar2.BackColor = System.Drawing.SystemColors.Control
        Me.ProgressBar2.BatteryColor = System.Drawing.Color.Lime
        Me.ProgressBar2.BodyPadding = 3
        Me.ProgressBar2.BorderColor = System.Drawing.Color.Gray
        Me.ProgressBar2.BorderWidth = 3
        Me.ProgressBar2.CriticalBatteryColor = System.Drawing.Color.Red
        Me.ProgressBar2.CriticalThreshold = 15
        Me.ProgressBar2.EmptyColor = System.Drawing.Color.White
        Me.ProgressBar2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold)
        Me.ProgressBar2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ProgressBar2.GridCount = 5
        Me.ProgressBar2.Location = New System.Drawing.Point(12, 417)
        Me.ProgressBar2.LowBatteryColor = System.Drawing.Color.Yellow
        Me.ProgressBar2.LowThreshold = 30
        Me.ProgressBar2.Maximum = 100
        Me.ProgressBar2.Minimum = 0
        Me.ProgressBar2.MinimumSize = New System.Drawing.Size(80, 24)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.SegmentCount = 5
        Me.ProgressBar2.ShowGrid = True
        Me.ProgressBar2.ShowPercentage = True
        Me.ProgressBar2.ShowSegments = True
        Me.ProgressBar2.ShowValue = True
        Me.ProgressBar2.Size = New System.Drawing.Size(706, 31)
        Me.ProgressBar2.TabIndex = 44
        Me.ProgressBar2.TerminalWidth = 6
        Me.ProgressBar2.Text = "BatteryHorizontalProgressBar1"
        Me.ProgressBar2.TextColor = System.Drawing.Color.Black
        Me.ProgressBar2.UseLevelColors = True
        Me.ProgressBar2.Value = 75
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.SystemColors.Control
        Me.ProgressBar1.BatteryColor = System.Drawing.Color.Lime
        Me.ProgressBar1.BodyPadding = 3
        Me.ProgressBar1.BorderColor = System.Drawing.Color.Gray
        Me.ProgressBar1.BorderWidth = 3
        Me.ProgressBar1.CriticalBatteryColor = System.Drawing.Color.Red
        Me.ProgressBar1.CriticalThreshold = 15
        Me.ProgressBar1.EmptyColor = System.Drawing.Color.White
        Me.ProgressBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold)
        Me.ProgressBar1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ProgressBar1.GridCount = 5
        Me.ProgressBar1.Location = New System.Drawing.Point(677, 28)
        Me.ProgressBar1.LowBatteryColor = System.Drawing.Color.Yellow
        Me.ProgressBar1.LowThreshold = 30
        Me.ProgressBar1.Maximum = 100
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.MinimumSize = New System.Drawing.Size(30, 60)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.SegmentCount = 5
        Me.ProgressBar1.ShowGrid = True
        Me.ProgressBar1.ShowPercentage = True
        Me.ProgressBar1.ShowSegments = True
        Me.ProgressBar1.ShowValue = False
        Me.ProgressBar1.Size = New System.Drawing.Size(41, 125)
        Me.ProgressBar1.TabIndex = 4
        Me.ProgressBar1.TerminalHeight = 6
        Me.ProgressBar1.Text = "BatteryVerticalProgressBar1"
        Me.ProgressBar1.TextColor = System.Drawing.Color.Black
        Me.ProgressBar1.UseLevelColors = True
        Me.ProgressBar1.Value = 75
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 550)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.lbstatuscharge)
        Me.Controls.Add(Me.bRelative)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.bCapabilities)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.bShort)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.bCritical)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.ProgressBar2)
        Me.Controls.Add(Me.bAlert2)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.bAlert1)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.bRuntime)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.bCondition)
        Me.Controls.Add(Me.bCurrent)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.bPower)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.bRate)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.bVoltage)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.bFCharge)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.bDCapacity)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.bChem)
        Me.Controls.Add(Me.bTech)
        Me.Controls.Add(Me.bUniqueID)
        Me.Controls.Add(Me.bSerialNumber)
        Me.Controls.Add(Me.bDevice)
        Me.Controls.Add(Me.bManufacturer)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Low-Level Battery Info"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ProgressBar1 As Low_Level_Battery_Info.BatteryVerticalProgressBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents bManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents bDevice As System.Windows.Forms.TextBox
    Friend WithEvents bSerialNumber As System.Windows.Forms.TextBox
    Friend WithEvents bUniqueID As System.Windows.Forms.TextBox
    Friend WithEvents bTech As System.Windows.Forms.TextBox
    Friend WithEvents bChem As System.Windows.Forms.TextBox
    Friend WithEvents bDCapacity As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents bFCharge As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents bVoltage As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents bRate As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents bPower As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents bCurrent As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents bCondition As System.Windows.Forms.Label
    Friend WithEvents bRuntime As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents bAlert1 As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents bAlert2 As System.Windows.Forms.TextBox
    Friend WithEvents ProgressBar2 As Low_Level_Battery_Info.BatteryHorizontalProgressBar
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents bCritical As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents bShort As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents bCapabilities As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents bRelative As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lbstatuscharge As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
