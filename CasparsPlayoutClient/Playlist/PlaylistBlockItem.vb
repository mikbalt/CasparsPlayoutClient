﻿'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports CasparCGNETConnector
Imports logger
Imports System.Threading

Public Class PlaylistBlockItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private lastPlaying As Integer

    Public Sub New(ByVal name As String, ByRef controller As ServerControler, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1)
        MyBase.New(name, PlaylistItemTypes.BLOCK, controller, channel, layer, 0)
    End Sub

    Public Overrides Function isPlaying() As Boolean
        If isWaiting() Then Return True
        For Each child In getChildItems()
            If child.isPlaying OrElse child.isWaiting Then Return True
        Next
        Return False
    End Function

    Public Overrides Sub start(Optional ByVal noWait As Boolean = False)
        'For Each child In getChildItems(True)
        '    If child.isPlayable Then
        '        child.setPosition(0)
        '    End If
        'Next
        MyBase.start(noWait)
        'While isPlaying() 'AndAlso Not isParallel()
        'Thread.Sleep(1)
        'End While
    End Sub

    Public Overrides Function getPosition() As Long
        If IsNothing(getParent) OrElse getParent.isPlaying() Then
            Dim pos As Long
            For Each child In getChildItems()
                If isParallel() Then
                    pos = Math.Max(pos, child.getPosition)
                Else
                    pos = pos + child.getPosition
                End If
            Next
            Return pos
        Else
            Return 0
        End If
    End Function

    Public Overrides Function getDuration() As Long
        Dim duration As Long
        For Each child In getChildItems()
            If isParallel() Then
                duration = Math.Max(duration, child.getDuration)
            Else
                duration = duration + child.getDuration
            End If
        Next
        Return duration
    End Function

End Class
