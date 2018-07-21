Module Game
    Public ViewportWidth As Integer = 75
    Public ViewportHeight As Integer = 56
    Public PlayerBoard As Board
    Public AiBoard As Board

    Sub Main()
        LoadMusic()
        Console.SetWindowSize(ViewportWidth, ViewportHeight)
        Console.SetBufferSize(ViewportWidth, ViewportHeight)
        Console.WriteLine("".PadRight(ViewportWidth, "="))
        CenterPrint("███╗   ██╗██╗███╗   ██╗     ██╗ █████╗ ")
        CenterPrint("████╗  ██║██║████╗  ██║     ██║██╔══██╗")
        CenterPrint("██╔██╗ ██║██║██╔██╗ ██║     ██║███████║")
        CenterPrint("██║╚██╗██║██║██║╚██╗██║██   ██║██╔══██║")
        CenterPrint("██║ ╚████║██║██║ ╚████║╚█████╔╝██║  ██║")
        CenterPrint("╚═╝  ╚═══╝╚═╝╚═╝  ╚═══╝ ╚════╝ ╚═╝  ╚═╝")
        CenterPrint("")
        CenterPrint("by LucidaDragon")
        CenterPrint("Made for Floppy Jam 2018")
        Console.WriteLine("".PadRight(ViewportWidth, "="))
        Console.WriteLine("")
        Console.ReadKey()
    End Sub

    Sub CenterPrint(str As String)
        Console.WriteLine(str.PadLeft((ViewportWidth / 2) + (str.Length) / 2))
    End Sub

    Sub LoadMusic()
        If IO.File.Exists("music.json") Then
            Sound.PlayLoopedMidi("music.json")
        End If
    End Sub
End Module
