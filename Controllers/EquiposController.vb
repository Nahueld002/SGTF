Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class EquiposController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function Listar() As JsonResult
            Dim equipos = db.Equipo.Include("Region").Include("Ciudad").Select(Function(e) New With {
                .EquipoID = e.EquipoID,
                .Nombre = e.Nombre,
                .CodigoEquipo = e.CodigoEquipo,
                .RegionID = e.RegionID,
                .CiudadID = e.CiudadID,
                .AñoFundacion = e.AñoFundacion,
                .ELO = e.ELO,
                .TipoEquipo = e.TipoEquipo,
                .Estado = e.Estado,
                .NombreRegion = If(e.Region IsNot Nothing, e.Region.Nombre, Nothing),
                .NombreCiudad = If(e.Ciudad IsNot Nothing, e.Ciudad.Nombre, Nothing)
            }).ToList()
            Return Json(New With {.data = equipos}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetRegiones() As JsonResult
            Dim regiones = db.Region.Select(Function(r) New With {
                .RegionID = r.RegionID,
                .Nombre = r.Nombre
            }).OrderBy(Function(r) r.Nombre).ToList()
            Return Json(regiones, JsonRequestBehavior.AllowGet)
        End Function

        Function GetCiudades(Optional regionId As Integer? = Nothing) As JsonResult
            Dim ciudadesQuery = db.Ciudad.AsQueryable()

            If regionId.HasValue AndAlso regionId.Value > 0 Then
                ciudadesQuery = ciudadesQuery.Where(Function(c) c.RegionID = regionId.Value)
            End If

            Dim ciudades = ciudadesQuery.Select(Function(c) New With {
                .CiudadID = c.CiudadID,
                .Nombre = c.Nombre
            }).OrderBy(Function(c) c.Nombre).ToList()
            Return Json(ciudades, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function Guardar(equipo As Equipo) As JsonResult
            Try
                If equipo.EquipoID = 0 Then
                    db.Equipo.Add(equipo)
                Else
                    Dim existingEquipo = db.Equipo.Find(equipo.EquipoID)
                    If existingEquipo IsNot Nothing Then
                        existingEquipo.Nombre = equipo.Nombre
                        existingEquipo.CodigoEquipo = equipo.CodigoEquipo
                        existingEquipo.RegionID = equipo.RegionID
                        existingEquipo.CiudadID = equipo.CiudadID
                        existingEquipo.AñoFundacion = equipo.AñoFundacion
                        existingEquipo.ELO = equipo.ELO
                        existingEquipo.TipoEquipo = equipo.TipoEquipo
                        existingEquipo.Estado = equipo.Estado
                    Else
                        Return Json(New With {.success = False, .message = "Equipo no encontrado para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar el equipo. " & ex.Message})
            End Try
        End Function

        Function Buscar(id As Integer) As JsonResult
            Dim equipo = db.Equipo.Find(id)
            If equipo Is Nothing Then
                Return Json(New With {.success = False, .message = "Equipo no encontrado."}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(equipo, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim equipo = db.Equipo.Find(id)
                If equipo Is Nothing Then
                    Return Json(New With {.success = False, .message = "Equipo no encontrado."})
                End If

                db.Equipo.Remove(equipo)
                db.SaveChanges()

                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar el equipo. Verifique si está asociada a alguna entidad."})
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
