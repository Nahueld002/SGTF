' En SGTF.Controllers
Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity

Namespace Controllers
    Public Class PaisesController
        Inherits Controller

        Private db As New FutbolDB2Entities()
        ' GET: Paises
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Paises/Listar
        Function Listar() As JsonResult
            Dim paises = db.Pais.Include("Confederacion").Select(Function(p) New With {
                .PaisID = p.PaisID,
                .Nombre = p.Nombre,
                .CodigoFIFA = p.CodigoFIFA,
                .ConfederacionID = p.ConfederacionID,
                .NombreConfederacion = p.Confederacion.Nombre ' Obtenemos el nombre de la Confederación
            }).ToList()
            Return Json(New With {.data = paises}, JsonRequestBehavior.AllowGet)
        End Function

        ' GET: Paises/GetConfederaciones
        ' Función para obtener las confederaciones para el DropDownList
        Function GetConfederaciones() As JsonResult
            Dim confederaciones = db.Confederacion.Select(Function(c) New With {
                .ConfederacionID = c.ConfederacionID,
                .Nombre = c.Nombre
            }).ToList()
            Return Json(confederaciones, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Paises/Guardar
        <HttpPost()>
        Function Guardar(pais As Pais) As JsonResult
            Try
                If pais.PaisID = 0 Then
                    ' Añadir nuevo país
                    db.Pais.Add(pais)
                Else
                    ' Actualizar país existente
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

        ' GET: Paises/Buscar/5
        Function Buscar(id As Integer) As JsonResult
            Dim pais = db.Pais.Find(id)
            If pais Is Nothing Then
                Return Json(New With {.success = False, .message = "País no encontrado."}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(pais, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Paises/Eliminar/5
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