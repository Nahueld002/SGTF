' En SGTF.Controllers
Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class ConfederacionesController
        Inherits Controller

        Private db As New FutbolDB2Entities() ' Ajusta esto al nombre de tu DbContext

        ' GET: Confederaciones
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Confederaciones/Listar
        Function Listar() As JsonResult
            ' Solo seleccionamos las propiedades existentes en tu modelo
            Dim confederaciones = db.Confederacion.Select(Function(c) New With {
                .ConfederacionID = c.ConfederacionID,
                .Nombre = c.Nombre
            }).ToList()
            Return Json(New With {.data = confederaciones}, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Confederaciones/Guardar
        <HttpPost()>
        Function Guardar(confederacion As Confederacion) As JsonResult
            Try
                If confederacion.ConfederacionID = 0 Then
                    ' Añadir nueva confederación
                    db.Confederacion.Add(confederacion)
                Else
                    ' Actualizar confederación existente
                    Dim existingConfederacion = db.Confederacion.Find(confederacion.ConfederacionID)
                    If existingConfederacion IsNot Nothing Then
                        existingConfederacion.Nombre = confederacion.Nombre
                        ' No se hace referencia a Siglas aquí
                    Else
                        Return Json(New With {.success = False, .message = "Confederación no encontrada para actualizar."})
                    End If
                End If
                db.SaveChanges()
                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar confederación: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar la confederación. " & ex.Message})
            End Try
        End Function

        ' GET: Confederaciones/Buscar/5
        Function Buscar(id As Integer) As JsonResult
            ' Solo seleccionamos las propiedades existentes en tu modelo
            Dim confederacion = db.Confederacion.Where(Function(c) c.ConfederacionID = id).Select(Function(c) New With {
                .ConfederacionID = c.ConfederacionID,
                .Nombre = c.Nombre
            }).FirstOrDefault()

            If confederacion Is Nothing Then
                Return Json(New With {.success = False, .message = "Confederación no encontrada."}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(confederacion, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Confederaciones/Eliminar/5
        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim confederacion = db.Confederacion.Find(id)
                If confederacion Is Nothing Then
                    Return Json(New With {.success = False, .message = "Confederación no encontrada."})
                End If

                db.Confederacion.Remove(confederacion)
                db.SaveChanges()

                Return Json(New With {.success = True})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al eliminar confederación: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar la confederación. Verifique si está asociada a países."})
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