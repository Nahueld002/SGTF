Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class CiudadController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function Listar() As JsonResult
            Dim ciudades = db.Ciudad.Include("Region").Select(Function(c) New With {
                .CiudadID = c.CiudadID,
                .Nombre = c.Nombre,
                .RegionID = c.RegionID,
                .EsCapital = c.EsCapital,
                .NombreRegion = c.Region.Nombre
            }).ToList()
            Return Json(New With {.data = ciudades}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetRegiones() As JsonResult
            Dim regiones = db.Region.Select(Function(r) New With {
                .RegionID = r.RegionID,
                .Nombre = r.Nombre
            }).OrderBy(Function(r) r.Nombre).ToList()
            Return Json(regiones, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function Guardar(ciudad As Ciudad) As JsonResult
            Try
                If ciudad.CiudadID = 0 Then
                    db.Ciudad.Add(ciudad)
                Else
                    Dim existingCiudad = db.Ciudad.Find(ciudad.CiudadID)
                    If existingCiudad IsNot Nothing Then
                        existingCiudad.Nombre = ciudad.Nombre
                        existingCiudad.RegionID = ciudad.RegionID
                        existingCiudad.EsCapital = ciudad.EsCapital
                    Else
                        Return Json(New With {.success = False, .message = "Ciudad no encontrada para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar ciudad: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar la ciudad. " & ex.Message})
            End Try
        End Function

        Function Buscar(id As Integer) As ActionResult
            Try
                db.Configuration.LazyLoadingEnabled = False
                db.Configuration.ProxyCreationEnabled = False

                Dim ciudadDTO = db.Ciudad.AsNoTracking() _
                    .Where(Function(c) c.CiudadID = id) _
                    .Select(Function(c) New With {
                        .success = True,
                        .CiudadID = c.CiudadID,
                        .Nombre = c.Nombre,
                        .RegionID = c.RegionID,
                        .EsCapital = c.EsCapital
                    }).FirstOrDefault()

                If ciudadDTO Is Nothing Then
                    Response.StatusCode = 404
                    Return Json(New With {.success = False, .message = "Ciudad no encontrada."},
                                 JsonRequestBehavior.AllowGet)
                End If

                Return Json(ciudadDTO, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error Buscar Ciudad: {ex}")
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message},
                                 JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim ciudad = db.Ciudad.Find(id)
                If ciudad Is Nothing Then
                    Return Json(New With {.success = False, .message = "Ciudad no encontrada."})
                End If

                db.Ciudad.Remove(ciudad)
                db.SaveChanges()

                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar ciudad: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar la ciudad. Verifique si está asociada a alguna entidad."})
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