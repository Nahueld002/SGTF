'------------------------------------------------------------------------------
' <auto-generated>
'     Este código se generó a partir de una plantilla.
'
'     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
'     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class Equipo
    Public Property EquipoID As Integer
    Public Property Nombre As String
    Public Property CodigoEquipo As String
    Public Property RegionID As Nullable(Of Integer)
    Public Property CiudadID As Nullable(Of Integer)
    Public Property AñoFundacion As Nullable(Of Short)
    Public Property ELO As Nullable(Of Double)
    Public Property TipoEquipo As String
    Public Property Estado As String

    Public Overridable Property Ciudad As Ciudad
    Public Overridable Property Region As Region
    Public Overridable Property Palmares As ICollection(Of Palmares) = New HashSet(Of Palmares)
    Public Overridable Property TablaPosiciones As ICollection(Of TablaPosiciones) = New HashSet(Of TablaPosiciones)
    Public Overridable Property TorneoEquipo As ICollection(Of TorneoEquipo) = New HashSet(Of TorneoEquipo)
    Public Overridable Property TorneoResultados As ICollection(Of TorneoResultados) = New HashSet(Of TorneoResultados)
    Public Overridable Property TorneoResultados1 As ICollection(Of TorneoResultados) = New HashSet(Of TorneoResultados)

End Class
