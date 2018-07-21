Public Class Sound
    Shared Thread As Threading.Thread
    Shared Timescale As Double = 1
    Public Shared Json As New Web.Script.Serialization.JavaScriptSerializer()

    Shared Sub Play(notes As List(Of Note), loopMusic As Boolean)
        While True
            Dim i As Integer = 0
            For Each note In notes
                Console.Beep(27.5 * (2 ^ ((note.Midi - 21) / 12)), note.Duration * 1000 * Timescale)
                If i + 1 < notes.Count Then
                    If Not notes(i + 1).Time = note.Time Then
                        Threading.Thread.Sleep((notes(i + 1).Time - note.Time) * 1000 * Timescale)
                    End If
                End If
                i += 1
            Next
            If Not loopMusic Then
                Exit Sub
            End If
        End While
    End Sub

    Shared Function ReadMidi(file As String) As List(Of List(Of Note))
        Dim midi As Midi = Json.Deserialize(Of Midi)(IO.File.ReadAllText(file))
        Dim result As New List(Of List(Of Note))
        For Each track In midi.Tracks
            If track.Notes.Count > 0 Then
                result.Add(track.Notes)
            End If
        Next
        Return result
    End Function

    Shared Sub PlayLoopedMidi(file As String)
        SpawnMidiThread(ReadMidi(file).First)
    End Sub

    Private Shared Sub SpawnMidiThread(data As List(Of Note))
        Dim bkgWork As New ComponentModel.BackgroundWorker
        AddHandler bkgWork.DoWork, AddressOf MidiThread
        bkgWork.RunWorkerAsync(data)
    End Sub

    Private Shared Sub MidiThread(sender As Object, e As ComponentModel.DoWorkEventArgs)
        Play(e.Argument, True)
    End Sub
End Class
