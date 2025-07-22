Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity
Imports System.Linq

Namespace Controllers
    Public Class PalmaresController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        ' GET: Palmares
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Palmares/Listar
        ' Returns a JSON list of all palmares, including associated Equipo and Torneo names.
        Function Listar() As JsonResult
            Try
                Dim palmaresList = db.Palmares.Include("Equipo").Include("Torneo").Select(Function(p) New With {
                    .PalmaresID = p.PalmaresID,
                    .AñoTitulo = p.AñoTitulo,
                    .EquipoID = p.EquipoID,
                    .TorneoID = p.TorneoID,
                    .NombreEquipo = If(p.Equipo IsNot Nothing, p.Equipo.Nombre, "N/A"),
                    .NombreTorneo = If(p.Torneo IsNot Nothing, p.Torneo.Nombre, "N/A")
                }).OrderBy(Function(p) p.AñoTitulo).ToList() ' Added OrderBy for consistency

                Return Json(New With {.data = palmaresList}, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al listar palmares (individual): {ex.Message}")
                Return Json(New With {.data = New List(Of Object)(), .error = "Error al cargar los palmares individuales."}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        ' GET: Palmares/GetTotalTitulos
        ' Returns a JSON list of total titles per team.
        Function GetTotalTitulos() As JsonResult
            Try
                Dim totalTitulos = (From p In db.Palmares
                                    Join e In db.Equipo On p.EquipoID Equals e.EquipoID
                                    Group p By e.Nombre Into Group
                                    Order By Group.Count() Descending
                                    Select New With {
                                        .Equipo = Nombre,
                                        .TotalTitulos = Group.Count()
                                    }).ToList()

                Return Json(New With {.data = totalTitulos}, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al obtener total de títulos: {ex.Message}")
                Return Json(New With {.data = New List(Of Object)(), .error = "Error al cargar el total de títulos por equipo."}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace