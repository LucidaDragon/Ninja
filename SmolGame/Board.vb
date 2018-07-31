Option Explicit On
Option Strict On

Public Class Board
    Public Data As New List(Of Boolean)

    Public Width As Integer
    Public Height As Integer

    Public NinjaX As Integer
    Public NinjaY As Integer
    Public HasBomb As Boolean = True

    Public LootX As Integer
    Public LootY As Integer

    Public Guards As New List(Of Guard)
    Public GuardData As New List(Of Boolean)

    Public Level As Integer = 0
    Public Finished As Boolean = False
    Public GameOver As Boolean = False

    Public rand As New Random

    Public Property GuardTile(x As Integer, y As Integer) As Boolean
        Get
            If Width * y + x < Data.Count And Width * y + x >= 0 Then
                Return GuardData(Width * y + x)
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            GuardData(Width * y + x) = value
        End Set
    End Property

    Default Public Property WallTile(x As Integer, y As Integer) As Boolean
        Get
            If Width * y + x < Data.Count And Width * y + x >= 0 Then
                Return Data(Width * y + x)
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            Data(Width * y + x) = value
        End Set
    End Property

    Sub New(width As Integer, height As Integer, level As Integer)
        Me.Width = width
        Me.Height = height
        Me.Level = level
        For i As Integer = 0 To width * height - 1
            Data.Add(False)
            GuardData.Add(False)
        Next
        For i As Integer = 0 To CInt(level ^ 2)
            Guards.Add(New Guard(Me))
        Next
        For i As Integer = 0 To width - 1
            For j As Integer = 0 To height - 1
                If i = 0 Or j = 0 Or i = width - 1 Or j = height - 1 Then
                    Me(i, j) = False
                ElseIf j Mod 2 > 0 Then
                    Me(i, j) = rand.NextDouble() > 0.02
                ElseIf i Mod 2 > 0 Then
                    Me(i, j) = rand.NextDouble() > 0.7
                Else
                    Me(i, j) = False
                End If
            Next
        Next

        Print()

        Dim placeX As Integer = 0
        Dim placeY As Integer = 0
        For i As Integer = 0 To Guards.Count - 1
            While Me(placeX, placeY) = False
                placeX = rand.Next(10, width)
                placeY = rand.Next(10, height)
            End While
            Guards(i).SetPos(placeX, placeY)
            placeX = 0
            placeY = 0
        Next

        While Me(placeX, placeY) = False
            placeX = rand.Next(0, 10)
            placeY = rand.Next(0, 10)
        End While
        NinjaX = placeX
        NinjaY = placeY
        Console.SetCursorPosition(NinjaX, NinjaY + 12)
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write("#")

        placeX = 0
        placeY = 0
        While Me(placeX, placeY) = False
            placeX = rand.Next(0, width)
            placeY = rand.Next(height \ 2, height)
        End While
        LootX = placeX
        LootY = placeY
        Console.SetCursorPosition(LootX, LootY + 12)
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.Write("@")
    End Sub

    Sub Print()
        Dim strBuilder As New Text.StringBuilder
        For y As Integer = 0 To Height - 1
            For x As Integer = 0 To Width - 1
                strBuilder.Append(GetSymbol(x, y))
            Next
            strBuilder.Append(Environment.NewLine)
        Next
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.WriteLine("Heist " & Level)
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.Write(strBuilder.ToString())
    End Sub

    Function GetSymbol(x As Integer, y As Integer) As Char
        If Me(x, y) Then
            Return " ".First
        Else
            Return "█".First
        End If
    End Function

    Function ValidWalkDirections(x As Integer, y As Integer) As List(Of GameAction)
        Dim dirs As New List(Of GameAction) From {GameAction.None}
        If Me(x, y - 1) Then
            dirs.Add(GameAction.Up)
        End If
        If Me(x, y + 1) Then
            dirs.Add(GameAction.Down)
        End If
        If Me(x - 1, y) Then
            dirs.Add(GameAction.Left)
        End If
        If Me(x + 1, y) Then
            dirs.Add(GameAction.Right)
        End If
        Return dirs
    End Function

    Sub Tick()
        For Each guard In Guards
            Dim dirs As List(Of GameAction) = ValidWalkDirections(guard.PosX, guard.PosY)
            Dim selected As GameAction = dirs(rand.Next(0, dirs.Count))
            If NinjaX = guard.PosX Then
                If NinjaY < guard.PosY And dirs.Contains(GameAction.Up) Then
                    selected = GameAction.Up
                ElseIf NinjaY > guard.PosY And dirs.Contains(GameAction.Down) Then
                    selected = GameAction.Down
                End If
            ElseIf NinjaY = guard.PosY Then
                If NinjaX < guard.PosX And dirs.Contains(GameAction.Left) Then
                    selected = GameAction.Left
                ElseIf Ninjax > guard.Posx And dirs.Contains(GameAction.Right) Then
                    selected = GameAction.Right
                End If
            End If
            If selected = GameAction.Up Then
                guard.SetPos(guard.PosX, guard.PosY - 1)
            ElseIf selected = GameAction.Down Then
                guard.SetPos(guard.PosX, guard.PosY + 1)
            ElseIf selected = GameAction.Left Then
                guard.SetPos(guard.PosX - 1, guard.PosY)
            ElseIf selected = GameAction.Right Then
                guard.SetPos(guard.PosX + 1, guard.PosY)
            End If
        Next
        Console.SetCursorPosition(LootX, LootY + 12)
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.Write("@")
    End Sub

    Sub DoInput(action As GameAction)
        Console.SetCursorPosition(NinjaX, NinjaY + 12)
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write(" ")
        If action = GameAction.Finish Then
            End
        ElseIf action = GameAction.Up Then
            If Me(NinjaX, NinjaY - 1) Then
                NinjaY -= 1
                Sound.Beep(300, 100)
            Else
                Sound.Beep(200, 100)
            End If
        ElseIf action = GameAction.Down Then
            If Me(NinjaX, NinjaY + 1) Then
                NinjaY += 1
                Sound.Beep(300, 100)
            Else
                Sound.Beep(200, 100)
            End If
        ElseIf action = GameAction.Left Then
            If Me(NinjaX - 1, NinjaY) Then
                NinjaX -= 1
                Sound.Beep(300, 100)
            Else
                Sound.Beep(200, 100)
            End If
        ElseIf action = GameAction.Right Then
            If Me(NinjaX + 1, NinjaY) Then
                NinjaX += 1
                Sound.Beep(300, 100)
            Else
                Sound.Beep(200, 100)
            End If
        ElseIf action = GameAction.Bomb Then
            If HasBomb Then
                Console.Beep(250, 100)
                For i As Integer = NinjaX - 3 To NinjaX + 3
                    For j As Integer = NinjaY - 3 To NinjaY + 3
                        If i > 0 And j > 0 And i < Width - 1 And j < Height - 1 And Me(i, j) = False Then
                            Me(i, j) = True
                            Console.SetCursorPosition(i, j + 12)
                            Console.Write(" ")
                        End If
                    Next
                Next
                For i As Integer = 275 To 250 Step -5
                    Sound.Beep(i, 100)
                    Threading.Thread.Sleep(10)
                Next
                HasBomb = False
            Else
                Sound.Beep(200, 100)
            End If
        End If
        Console.SetCursorPosition(NinjaX, NinjaY + 12)
        Console.Write("#")

        If NinjaX = LootX And NinjaY = LootY Then
            Finished = True
        End If
    End Sub
End Class
