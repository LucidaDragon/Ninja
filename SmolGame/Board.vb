Option Explicit On
Option Strict On

Public Class Board
    Public Data As New List(Of Boolean)

    Public Width As Byte
    Public Height As Byte

    Private Const One As Byte = 1

    Default Public Property Tile(x As Byte, y As Byte) As Boolean
        Get
            If Width * y + x < Data.Count Then
                Return Data(Width * y + x)
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            Data(Width * y + x) = value
        End Set
    End Property

    Sub New(width As Byte, height As Byte)
        Me.Width = width
        Me.Height = height
    End Sub

    Sub Print()
        For y As Byte = 0 To Height - One
            For x As Byte = 0 To Width - One
                Console.Write(Me(x, y))
            Next
            Console.Write(Environment.NewLine)
        Next
    End Sub

    Function GetNeighbours(x As Byte, y As Byte) As Boolean() 'Top, right, bottom, left
        Return {Tile(x, y - One), Tile(x + One, y), Tile(x, y + One), Tile(x - One, y)}
    End Function
End Class
