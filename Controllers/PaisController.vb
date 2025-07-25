Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class PaisesController
        Inherits Controller

        Private db As New FutbolDB2Entities()
        Function Index() As ActionResult
            Return View()
        End Function

        Function Listar() As JsonResult
            Dim paises = db.Pais.Include("Confederacion").Select(Function(p) New With {
                .PaisID = p.PaisID,
                .Nombre = p.Nombre,
                .CodigoFIFA = p.CodigoFIFA,
                .ConfederacionID = p.ConfederacionID,
                .NombreConfederacion = p.Confederacion.Nombre
            }).ToList()
            Return Json(New With {.data = paises}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetConfederaciones() As JsonResult
            Dim confederaciones = db.Confederacion.Select(Function(c) New With {
                .ConfederacionID = c.ConfederacionID,
                .Nombre = c.Nombre
            }).ToList()
            Return Json(confederaciones, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function Guardar(pais As Pais) As JsonResult
            Try
                If pais.PaisID = 0 Then
                    db.Pais.Add(pais)
                Else
                    Dim existingPais = db.Pais.Find(pais.PaisID)
                    If existingPais IsNot Nothing Then
                        existingPais.Nombre = pais.Nombre
                        existingPais.CodigoFIFA = pais.CodigoFIFA
                        existingPais.ConfederacionID = pais.ConfederacionID
                    Else
                        Return Json(New With {.success = False, .message = "País no encontrado para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar país: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar el país. " & ex.Message})
            End Try
        End Function

        Function Buscar(id As Integer) As ActionResult
            Try
                db.Configuration.LazyLoadingEnabled = False
                db.Configuration.ProxyCreationEnabled = False

                Dim paisDTO = db.Pais.AsNoTracking() _
            .Where(Function(p) p.PaisID = id) _
            .Select(Function(p) New With {
                .success = True,
                .PaisID = p.PaisID,
                .Nombre = p.Nombre,
                .CodigoFIFA = p.CodigoFIFA,
                .ConfederacionID = p.ConfederacionID
            }).FirstOrDefault()

                If paisDTO Is Nothing Then
                    Response.StatusCode = 404
                    Return Json(New With {.success = False, .message = "País no encontrado."},
                        JsonRequestBehavior.AllowGet)
                End If

                Return Json(paisDTO, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error Buscar País: {ex}")
                Response.StatusCode = 500
                Return Json(New With {.success = False, .message = ex.Message},
                    JsonRequestBehavior.AllowGet)
            End Try
        End Function

        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim pais = db.Pais.Find(id)
                If pais Is Nothing Then
                    Return Json(New With {.success = False, .message = "País no encontrado."})
                End If

                db.Pais.Remove(pais)
                db.SaveChanges()

                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar país: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar el país. Verifique si está asociado a alguna entidad."})
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