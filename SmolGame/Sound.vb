Public Class Sound
    Shared Timescale As Double = 0.3
    Shared FrequencyOffset As Integer = 10
    Shared Worker As ComponentModel.BackgroundWorker
    Shared CurrentBeepThread As Threading.Thread
    Public Shared UsingMusic As Boolean = False
    Public Shared Json As New Web.Script.Serialization.JavaScriptSerializer()

    Shared Sub Play(notes As List(Of Note), loopMusic As Boolean, Optional worker As ComponentModel.BackgroundWorker = Nothing)
        Sound.Worker = worker
        While True
            Dim i As Integer = 0
            For Each note In notes
                Console.Beep((27.5 * (2 ^ ((note.Midi - 21) / 12))) + FrequencyOffset, note.Duration * 1000 * Timescale)
                If i + 1 < notes.Count Then
                    If Not notes(i + 1).Time = note.Time Then
                        Threading.Thread.Sleep((notes(i + 1).Time - note.Time) * 1000 * Timescale)
                    End If
                End If
                i += 1
                If worker IsNot Nothing Then
                    If worker.CancellationPending Then
                        Exit Sub
                    End If
                End If
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

    Shared Sub Beep(frequency As Integer, duration As Integer)
        If Not UsingMusic Then
            Dim thread As New Threading.Thread(New Threading.ParameterizedThreadStart(Sub(arg As Point)
                                                                                          Console.Beep(arg.X, arg.Y)
                                                                                      End Sub))
            If CurrentBeepThread IsNot Nothing Then
                CurrentBeepThread.Abort()
            End If
            CurrentBeepThread = thread
            thread.Start(New Point(frequency, duration))
        End If
    End Sub

    Private Shared Sub SpawnMidiThread(data As List(Of Note))
        Dim bkgWork As New ComponentModel.BackgroundWorker With {
            .WorkerSupportsCancellation = True
        }
        AddHandler bkgWork.DoWork, AddressOf MidiThread
        bkgWork.RunWorkerAsync(data)
    End Sub

    Private Shared Sub MidiThread(sender As Object, e As ComponentModel.DoWorkEventArgs)
        Play(e.Argument, True)
    End Sub
End Class
