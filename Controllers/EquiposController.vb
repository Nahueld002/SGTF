Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class EquiposController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        ' GET: Equipos
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Equipos/Listar
        Function Listar() As JsonResult
            Dim equipos = db.Equipo _
        .Include(Function(e) e.Ciudad) _ ' Eagerly load Ciudad
        .Include(Function(e) e.Region) _ ' Eagerly load Region
        .Select(Function(e) New With {
            e.EquipoID,
            e.Nombre,
            e.CodigoEquipo,
            e.TipoEquipo,
            e.AñoFundacion,
            e.ELO,
            e.Estado,
            .Ciudad = If(e.Ciudad IsNot Nothing, e.Ciudad.Nombre, Nothing),
            .Region = If(e.Region IsNot Nothing, e.Region.Nombre, Nothing)
        }).ToList()

            Return Json(New With {.data = equipos}, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Equipos/Guardar
        <HttpPost>
        Function Guardar(equipo As Equipo) As JsonResult
            Try
                If equipo.EquipoID = 0 Then
                    ' Add new team
                    db.Equipo.Add(equipo)
                Else
                    ' Update existing team
                    Dim existingEquipo = db.Equipo.Find(equipo.EquipoID)
                    If existingEquipo IsNot Nothing Then
                        existingEquipo.Nombre = equipo.Nombre
                        existingEquipo.CodigoEquipo = equipo.CodigoEquipo
                        existingEquipo.AñoFundacion = equipo.AñoFundacion
                        existingEquipo.ELO = equipo.ELO
                        existingEquipo.TipoEquipo = equipo.TipoEquipo
                        existingEquipo.Estado = equipo.Estado
                        existingEquipo.CiudadID = equipo.CiudadID ' Update FK
                        existingEquipo.RegionID = equipo.RegionID ' Update FK
                        ' No need to set state to Modified explicitly when tracking the entity
                    Else
                        ' Equipo not found for update - handle this case
                        Return Json(New With {.success = False, .message = "Equipo no encontrado para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                ' Log the exception (e.g., to a file or database)
                System.Diagnostics.Debug.WriteLine($"Error al guardar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar el equipo. " & ex.Message})
            End Try
        End Function

        ' GET: Equipos/Buscar/5
        Function Buscar(id As Integer) As JsonResult
            Dim equipo = db.Equipo _
        .Include(Function(e) e.Ciudad) _
        .Include(Function(e) e.Region) _
        .Where(Function(e) e.EquipoID = id) _
        .Select(Function(e) New With {
            e.EquipoID,
            e.Nombre,
            e.CodigoEquipo,
            e.TipoEquipo,
            e.AñoFundacion,
            e.ELO,
            e.Estado,
            e.CiudadID,
            e.RegionID,
            .CiudadNombre = If(e.Ciudad IsNot Nothing, e.Ciudad.Nombre, ""),
            .RegionNombre = If(e.Region IsNot Nothing, e.Region.Nombre, "")
        }).FirstOrDefault()

            If equipo Is Nothing Then
                Return Json(New With {.success = False, .message = "Equipo no encontrado."}, JsonRequestBehavior.AllowGet)
            End If

            Return Json(equipo, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Equipos/Eliminar/5
        <HttpPost>
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
                ' Log the exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar equipo: {ex.Message}")
                ' Return a specific error message if applicable (e.g., FK constraint)
                Return Json(New With {.success = False, .message = "Error al eliminar el equipo. Verifique si está asociado a torneos o partidos."})
            End Try
        End Function

        ' Métodos jerárquicos para combos ---------------------------

        Function CargarConfederaciones() As JsonResult
            Dim lista = db.Confederacion.OrderBy(Function(c) c.Nombre).ToList()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        End Function

        Function CargarPaisesPorConfederacion(idConfederacion As Integer) As JsonResult
            Dim lista = db.Pais.Where(Function(p) p.ConfederacionID = idConfederacion).OrderBy(Function(p) p.Nombre).ToList()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        End Function

        Function CargarRegionesPorPais(idPais As Integer) As JsonResult
            Dim lista = db.Region.Where(Function(r) r.PaisID = idPais).OrderBy(Function(r) r.Nombre).ToList()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        End Function

        Function CargarCiudadesPorRegion(idRegion As Integer) As JsonResult
            Dim lista = db.Ciudad.Where(Function(c) c.RegionID = idRegion).OrderBy(Function(c) c.Nombre).ToList()
            Return Json(lista, JsonRequestBehavior.AllowGet)
        End Function

    End Class
End Namespace
