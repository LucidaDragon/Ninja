Module Game
    Public ViewportWidth As Integer = 75
    Public ViewportHeight As Integer = 55
    Public PlayerBoard As Board

    Sub Main()
        Console.Title = "Ninja - Floppy Jam 2018"
        PrintTitle()
        Console.WriteLine("Play Music? (y/n)")
        Dim key As ConsoleKey = ConsoleKey.NoName
        While Not (key = ConsoleKey.Y Or key = ConsoleKey.N)
            Console.SetCursorPosition(0, 12)
            key = Console.ReadKey().Key
        End While
        If key = ConsoleKey.Y Then
            LoadMusic()
        End If
        While True
            GameLoop()
        End While
    End Sub

    Sub NewGame()
        PlayerBoard = New Board(ViewportWidth, ViewportHeight - 12, 1)
    End Sub

    Sub GameLoop()
        PrintTitle()
        NewGame()
        While Not PlayerBoard.GameOver
            While Not (PlayerBoard.Finished Or PlayerBoard.GameOver)
                PlayerBoard.DoInput(GetInput())
                PlayerBoard.Tick()
            End While
            If PlayerBoard.Finished And PlayerBoard.Level <= 0 Then
                PrintTitle()
                PlayerBoard = New Board(PlayerBoard.Width, PlayerBoard.Height, PlayerBoard.Level + 1)
            ElseIf PlayerBoard.Finished And PlayerBoard.Level > 0 Then
                Threading.Thread.Sleep(500)
                WipeScreen()
                Console.Clear()
                PrintEnding()
            ElseIf PlayerBoard.GameOver Then
                Threading.Thread.Sleep(500)
                WipeScreen()
                Console.ForegroundColor = ConsoleColor.Red
                Console.SetCursorPosition(0, ViewportHeight / 2 - 3)
                CenterPrint("".PadRight(ViewportWidth))
                CenterPrint("  ▄████  ▄▄▄       ███▄ ▄███▓▓█████     ▒█████   ██▒   █▓▓█████  ██▀███  ")
                CenterPrint(" ██▒ ▀█▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀    ▒██▒  ██▒▓██░   █▒▓█   ▀ ▓██ ▒ ██▒")
                CenterPrint("▒██░▄▄▄░▒██  ▀█▄  ▓██    ▓██░▒███      ▒██░  ██▒ ▓██  █▒░▒███   ▓██ ░▄█ ▒")
                CenterPrint("░▓█  ██▓░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄    ▒██   ██░  ▒██ █░░▒▓█  ▄ ▒██▀▀█▄  ")
                CenterPrint("░▒▓███▀▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒   ░ ████▓▒░   ▒▀█░  ░▒████▒░██▓ ▒██▒")
                CenterPrint(" ░▒   ▒  ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░   ░ ▒░▒░▒░    ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░")
                CenterPrint("  ░   ░   ▒   ▒▒ ░░  ░      ░ ░ ░  ░     ░ ▒ ▒░    ░ ░░   ░ ░  ░  ░▒ ░ ▒░")
                CenterPrint("░ ░   ░   ░   ▒   ░      ░      ░      ░ ░ ░ ▒       ░░     ░     ░░   ░ ")
                CenterPrint("      ░       ░  ░       ░      ░  ░       ░ ░        ░     ░  ░   ░     ")
                CenterPrint("                                                     ░                   ")
                CenterPrint("".PadRight(ViewportWidth))
                Dim pos As Integer = 12
                For i As Integer = 0 To ViewportWidth - 1
                    For j As Integer = 0 To ViewportHeight - 1
                        If j < ViewportHeight / 2 - 3 Or j > ViewportHeight / 2 - 3 + 11 Or i = ViewportWidth - 1 Then
                            Console.SetCursorPosition(i, j)
                            Console.Write(" ")
                        End If
                    Next
                Next
                Console.SetCursorPosition(0, ViewportHeight)
                Threading.Thread.Sleep(1000)
                While Console.KeyAvailable
                    Console.ReadKey()
                End While
                Console.Write("Press any key to restart...")
                Console.ReadKey()
            End If
        End While
    End Sub

    Sub PrintTitle()
        Console.Clear()
        Console.SetWindowSize(ViewportWidth + 1, ViewportHeight + 1)
        Console.SetBufferSize(ViewportWidth + 1, ViewportHeight + 1)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("".PadRight(ViewportWidth, "="))
        CenterPrint("███╗   ██╗██╗███╗   ██╗     ██╗ █████╗ ")
        CenterPrint("████╗  ██║██║████╗  ██║     ██║██╔══██╗")
        CenterPrint("██╔██╗ ██║██║██╔██╗ ██║     ██║███████║")
        CenterPrint("██║╚██╗██║██║██║╚██╗██║██   ██║██╔══██║")
        CenterPrint("██║ ╚████║██║██║ ╚████║╚█████╔╝██║  ██║")
        CenterPrint("╚═╝  ╚═══╝╚═╝╚═╝  ╚═══╝ ╚════╝ ╚═╝  ╚═╝")
        CenterPrint("")
        CenterPrint("A smol game by LucidaDragon")
        CenterPrint("Made for Floppy Jam 2018")
        Console.WriteLine("".PadRight(ViewportWidth, "="))
    End Sub

    Sub PrintEnding()
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("")
        CenterPrint("  ████╗  ")
        CenterPrint("████████╗")
        CenterPrint("╚██████╔╝")
        CenterPrint(" ╚████╔╝ ")
        CenterPrint("  ╚██╔╝  ")
        CenterPrint(" ██████╗ ")
        CenterPrint(" ╚═════╝ ")
        Console.WriteLine("")
        Console.ForegroundColor = ConsoleColor.White
        CenterPrint("Congratulations!")
        CenterPrint("You have beat the game.")
        CenterPrint("An achievement will now be saved to your disk.")
        Try
            Dim name As String = "achievement-" & DateTime.Now.ToShortDateString().Replace("\", "/").Replace("/", "-") & ".txt"
            IO.File.WriteAllText(name, "On " & DateTime.Now.ToShortDateString() & " at " & DateTime.Now.ToShortTimeString() & ", this player finished Ninja, a smol game made for Floppy Jam 2018." & Environment.NewLine & "Thanks for playing!")
        Catch ex As Exception
            CenterPrint("Oh no! We couldn't save to disk because: " & ex.Message)
        End Try
        CenterPrint("Press escape to exit.")
        Dim key As ConsoleKey = ConsoleKey.NoName
        While Not key = ConsoleKey.Escape
            Console.SetCursorPosition(0, 12)
            key = Console.ReadKey().Key
        End While
        End
    End Sub

    Sub CenterPrint(str As String)
        Console.WriteLine(str.PadLeft((ViewportWidth / 2) + (str.Length) / 2))
    End Sub

    Sub WipeScreen()
        Dim pnts As New List(Of Point)
        For i As Integer = 0 To ViewportWidth - 1
            For j As Integer = 0 To ViewportHeight - 1
                pnts.Add(New Point(i, j))
            Next
        Next
        Dim rand As New Random
        For i As Integer = 0 To pnts.Count
            Dim a As Integer = rand.Next(0, pnts.Count)
            Dim b As Integer = rand.Next(0, pnts.Count)
            Dim p As Point = pnts(a)
            pnts(a) = pnts(b)
            pnts(b) = p
        Next
        For i As Integer = 0 To pnts.Count - 1
            Console.SetCursorPosition(pnts(i).X, pnts(i).Y)
            Console.Write(" ")
        Next
    End Sub

    Sub LoadMusic()
        If IO.File.Exists("music.json") Then
            Sound.PlayLoopedMidi("music.json")
        End If
    End Sub

    Function GetInput() As GameAction
        Console.SetCursorPosition(0, ViewportHeight)
        Console.ForegroundColor = ConsoleColor.Black
        Dim key As ConsoleKey = Console.ReadKey().Key
        If key = ConsoleKey.UpArrow Or key = ConsoleKey.W Then
            Return GameAction.Up
        ElseIf key = ConsoleKey.DownArrow Or key = ConsoleKey.S Then
            Return GameAction.Down
        ElseIf key = ConsoleKey.LeftArrow Or key = ConsoleKey.A Then
            Return GameAction.Left
        ElseIf key = ConsoleKey.RightArrow Or key = ConsoleKey.D Then
            Return GameAction.Right
        ElseIf key = ConsoleKey.Spacebar Then
            Return GameAction.Bomb
        ElseIf key = ConsoleKey.Escape Then
            Return GameAction.Finish
        Else
            Return GameAction.None
        End If
    End Function
End Module
