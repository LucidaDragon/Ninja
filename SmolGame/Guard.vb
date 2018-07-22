Public Class Guard
    Private X As Integer
    Private Y As Integer
    Private Parent As Board

    Public ReadOnly Property PosX As Integer
        Get
            Return X
        End Get
    End Property

    Public ReadOnly Property PosY As Integer
        Get
            Return Y
        End Get
    End Property

    Sub New(parent As Board)
        Me.Parent = parent
    End Sub

    Sub SetPos(x As Integer, y As Integer)
        Parent.GuardTile(Me.X, Me.Y) = False
        Parent.GuardTile(x, y) = True
        Console.ForegroundColor = ConsoleColor.Red
        If Parent(Me.X, Me.Y) Then
            Console.SetCursorPosition(Me.X, Me.Y + 12)
            Console.Write(" ")
        End If
        Console.SetCursorPosition(x, y + 12)
        Console.Write("#")

        Me.X = x
        Me.Y = y

        If Me.X >= Parent.NinjaX - 1 And Me.X <= Parent.NinjaX + 1 And Me.Y >= Parent.NinjaY - 1 And Me.Y <= Parent.NinjaY + 1 Then
            Parent.GameOver = True
        End If
    End Sub
End Class
