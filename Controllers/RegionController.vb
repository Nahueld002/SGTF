Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class RegionController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function Listar() As JsonResult
            Dim regiones = db.Region.Include("Pais").Select(Function(r) New With {
                .RegionID = r.RegionID,
                .Nombre = r.Nombre,
                .TipoRegion = r.TipoRegion,
                .PaisID = r.PaisID,
                .NombrePais = r.Pais.Nombre
            }).ToList()
            Return Json(New With {.data = regiones}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetPaises() As JsonResult
            Dim paises = db.Pais.Select(Function(p) New With {
                .PaisID = p.PaisID,
                .Nombre = p.Nombre
            }).OrderBy(Function(p) p.Nombre).ToList()
            Return Json(paises, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function Guardar(region As Region) As JsonResult
            Try
                If region.RegionID = 0 Then
                    db.Region.Add(region)
                Else
                    Dim existingRegion = db.Region.Find(region.RegionID)
                    If existingRegion IsNot Nothing Then
                        existingRegion.Nombre = region.Nombre
                        existingRegion.TipoRegion = region.TipoRegion
                        existingRegion.PaisID = region.PaisID
                    Else
                        Return Json(New With {.success = False, .message = "Región no encontrada para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar región: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar la región. " & ex.Message})
            End Try
        End Function

        Function Buscar(id As Integer) As ActionResult
            Try
                db.Configuration.LazyLoadingEnabled = False
                db.Configuration.ProxyCreationEnabled = False

                Dim regionDTO = db.Region.AsNoTracking() _
            .Where(Function(r) r.RegionID = id) _
            .Select(Function(r) New With {
                .success = True,
                .RegionID = r.RegionID,
                .Nombre = r.Nombre,
                .TipoRegion = r.TipoRegion,
                .PaisID = r.PaisID,
                .NombrePais = r.Pais.Nombre
            }).FirstOrDefault()

                If regionDTO Is Nothing Then
                    Response.StatusCode = 404
                    Return Json(New With {.success = False, .message = "Región no encontrada."},
                        JsonRequestBehavior.AllowGet)
                End If

                Return Json(regionDTO, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error Buscar Región: {ex}")
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message},
                    JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim region = db.Region.Find(id)
                If region Is Nothing Then
                    Return Json(New With {.success = False, .message = "Región no encontrada."})
                End If

                db.Region.Remove(region)
                db.SaveChanges()

                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar región: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar la región. Verifique si está asociada a alguna entidad."})
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