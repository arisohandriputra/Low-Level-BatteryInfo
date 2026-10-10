Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

' BatteryHorizontalProgressBar
' A classic Windows-style horizontal battery indicator.
' Written by Ari Sohandri Putra.

Public Class BatteryHorizontalProgressBar
    Inherits Control

    Private _minimum As Integer = 0
    Private _maximum As Integer = 100
    Private _value As Integer = 75

    Private _showValue As Boolean = True
    Private _useLevelColors As Boolean = True
    Private _showSegments As Boolean = True

    Private _batteryColor As Color = Color.Lime
    Private _lowBatteryColor As Color = Color.Yellow
    Private _criticalBatteryColor As Color = Color.Red
    Private _emptyColor As Color = Color.White
    Private _textColor As Color = Color.Black
    Private _borderColor As Color = Color.Gray

    Private _lowThreshold As Integer = 30
    Private _criticalThreshold As Integer = 15

    Private _terminalWidth As Integer = 6
    Private _borderWidth As Integer = 3
    Private _segmentCount As Integer = 5

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint Or _
                    ControlStyles.AllPaintingInWmPaint Or _
                    ControlStyles.OptimizedDoubleBuffer Or _
                    ControlStyles.ResizeRedraw, True)

        Me.Size = New Size(220, 42)
        Me.MinimumSize = New Size(80, 24)
        Me.BackColor = SystemColors.Control
        Me.ForeColor = SystemColors.ControlText
        Me.Font = New Font("Microsoft Sans Serif", 8.0!, FontStyle.Bold)
    End Sub

    <Category("Behavior")> _
    Public Property Minimum() As Integer
        Get
            Return _minimum
        End Get
        Set(ByVal value As Integer)
            If value > _maximum Then _maximum = value
            _minimum = value
            If _value < _minimum Then _value = _minimum
            If _value > _maximum Then _value = _maximum
            Me.Invalidate()
        End Set
    End Property

    <Category("Behavior")> _
    Public Property Maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal value As Integer)
            If value < _minimum Then value = _minimum
            _maximum = value
            If _value > _maximum Then _value = _maximum
            Me.Invalidate()
        End Set
    End Property

    <Category("Behavior")> _
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal value As Integer)
            If value < _minimum Then value = _minimum
            If value > _maximum Then value = _maximum
            _value = value
            Me.Invalidate()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Percentage() As Integer
        Get
            If _maximum <= _minimum Then Return 0
            Return CInt(Math.Round(CDbl(_value - _minimum) * 100.0R / _
                                   CDbl(_maximum - _minimum)))
        End Get
    End Property

    <Category("Appearance")> _
    Public Property ShowValue() As Boolean
        Get
            Return _showValue
        End Get
        Set(ByVal value As Boolean)
            _showValue = value
            Me.Invalidate()
        End Set
    End Property

    ' Kept for compatibility with forms that used the old vertical control.
    <Category("Appearance")> _
    Public Property ShowPercentage() As Boolean
        Get
            Return _showValue
        End Get
        Set(ByVal value As Boolean)
            _showValue = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property UseLevelColors() As Boolean
        Get
            Return _useLevelColors
        End Get
        Set(ByVal value As Boolean)
            _useLevelColors = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property ShowSegments() As Boolean
        Get
            Return _showSegments
        End Get
        Set(ByVal value As Boolean)
            _showSegments = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property BatteryColor() As Color
        Get
            Return _batteryColor
        End Get
        Set(ByVal value As Color)
            _batteryColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property LowBatteryColor() As Color
        Get
            Return _lowBatteryColor
        End Get
        Set(ByVal value As Color)
            _lowBatteryColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property CriticalBatteryColor() As Color
        Get
            Return _criticalBatteryColor
        End Get
        Set(ByVal value As Color)
            _criticalBatteryColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property EmptyColor() As Color
        Get
            Return _emptyColor
        End Get
        Set(ByVal value As Color)
            _emptyColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property TextColor() As Color
        Get
            Return _textColor
        End Get
        Set(ByVal value As Color)
            _textColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property LowThreshold() As Integer
        Get
            Return _lowThreshold
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > 100 Then value = 100
            _lowThreshold = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property CriticalThreshold() As Integer
        Get
            Return _criticalThreshold
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            If value > 100 Then value = 100
            _criticalThreshold = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property TerminalWidth() As Integer
        Get
            Return _terminalWidth
        End Get
        Set(ByVal value As Integer)
            If value < 2 Then value = 2
            If value > 20 Then value = 20
            _terminalWidth = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property BorderWidth() As Integer
        Get
            Return _borderWidth
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 6 Then value = 6
            _borderWidth = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property SegmentCount() As Integer
        Get
            Return _segmentCount
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 20 Then value = 20
            _segmentCount = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property BorderColor() As Color
        Get
            Return _borderColor
        End Get
        Set(ByVal value As Color)
            _borderColor = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property BodyPadding() As Integer
        Get
            Return _borderWidth
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 6 Then value = 6
            _borderWidth = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property ShowGrid() As Boolean
        Get
            Return _showSegments
        End Get
        Set(ByVal value As Boolean)
            _showSegments = value
            Me.Invalidate()
        End Set
    End Property

    <Category("Appearance")> _
    Public Property GridCount() As Integer
        Get
            Return _segmentCount
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value > 20 Then value = 20
            _segmentCount = value
            Me.Invalidate()
        End Set
    End Property

    Private Function GetBatteryColor() As Color
        If Not _useLevelColors Then Return _batteryColor

        If Me.Percentage <= _criticalThreshold Then
            Return _criticalBatteryColor
        End If

        If Me.Percentage <= _lowThreshold Then
            Return _lowBatteryColor
        End If

        Return _batteryColor
    End Function

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        Dim w As Integer = Me.ClientSize.Width
        Dim h As Integer = Me.ClientSize.Height

        If w < 30 OrElse h < 16 Then Return

        Using bg As New SolidBrush(Me.BackColor)
            g.FillRectangle(bg, Me.ClientRectangle)
        End Using

        ' The small battery terminal sits on the right.
        Dim terminalW As Integer = _terminalWidth
        If terminalW > w \ 8 Then terminalW = Math.Max(2, w \ 8)

        Dim bodyX As Integer = 2
        Dim bodyY As Integer = 3
        Dim bodyW As Integer = w - terminalW - 5
        Dim bodyH As Integer = h - 6

        If bodyW < 10 OrElse bodyH < 8 Then Return

        Dim bodyRect As New Rectangle(bodyX, bodyY, bodyW, bodyH)
        ControlPaint.DrawBorder3D(g, bodyRect, Border3DStyle.Sunken)

        Dim terminalY As Integer = bodyY + (bodyH \ 4)
        Dim terminalH As Integer = bodyH \ 2
        If terminalH < 3 Then terminalH = 3

        Dim terminalRect As New Rectangle(bodyX + bodyW, terminalY, _
                                          terminalW, terminalH)
        ControlPaint.DrawBorder3D(g, terminalRect, Border3DStyle.Raised)

        Dim pad As Integer = _borderWidth
        If pad * 2 >= bodyW - 2 Then pad = 1
        If pad * 2 >= bodyH - 2 Then pad = 1

        Dim innerX As Integer = bodyX + pad + 1
        Dim innerY As Integer = bodyY + pad + 1
        Dim innerW As Integer = bodyW - (pad * 2) - 2
        Dim innerH As Integer = bodyH - (pad * 2) - 2

        If innerW < 1 OrElse innerH < 1 Then Return

        Dim innerRect As New Rectangle(innerX, innerY, innerW, innerH)
        Using emptyBrush As New SolidBrush(_emptyColor)
            g.FillRectangle(emptyBrush, innerRect)
        End Using

        ' Fill the battery from left to right using the raw value and maximum.
        Dim fillWidth As Integer = 0
        If _maximum > _minimum Then
            fillWidth = CInt(CDbl(innerW) * CDbl(_value - _minimum) / _
                             CDbl(_maximum - _minimum))
        End If

        If fillWidth < 0 Then fillWidth = 0
        If fillWidth > innerW Then fillWidth = innerW

        If fillWidth > 0 Then
            Dim fillRect As New Rectangle(innerX, innerY, fillWidth, innerH)
            Using fillBrush As New SolidBrush(GetBatteryColor())
                g.FillRectangle(fillBrush, fillRect)
            End Using
        End If

        ' Draw the classic rectangular grid over the battery fill.
        If _showSegments AndAlso _segmentCount > 0 Then
            Dim i As Integer
            For i = 1 To _segmentCount
                Dim lineX As Integer = innerX + _
                    CInt(CDbl(innerW) * i / (_segmentCount + 1))

                If lineX >= innerX AndAlso lineX < innerX + innerW Then
                    Using linePen As New Pen(Color.Gray)
                        g.DrawLine(linePen, lineX, innerY, lineX, _
                                   innerY + innerH - 1)
                    End Using
                End If
            Next
        End If

        ' Show raw capacity values instead of a percentage.
        If _showValue Then
            Dim textRect As New Rectangle(bodyX + 2, bodyY + 1, _
                                           bodyW - 4, bodyH - 2)
            Dim flags As TextFormatFlags = _
                TextFormatFlags.HorizontalCenter Or _
                TextFormatFlags.VerticalCenter Or _
                TextFormatFlags.SingleLine Or _
                TextFormatFlags.NoPadding Or _
                TextFormatFlags.EndEllipsis

            Dim label As String = _value.ToString("N0") & _
                                  " mWh / " & _maximum.ToString("N0") & " mWh"

            TextRenderer.DrawText(g, label, Me.Font, textRect, _
                                  _textColor, flags)
        End If
    End Sub
End Class
