Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models ' Asegúrate de que tu modelo Region esté en este Namespace
Imports System.Data.Entity

Namespace Controllers
    Public Class RegionController ' Renombrado de PaisesController a RegionController
        Inherits Controller

        Private db As New FutbolDB2Entities() ' Tu contexto de base de datos

        ' GET: Region
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Region/Listar
        Function Listar() As JsonResult
            ' Incluimos el objeto Pais para poder acceder al nombre del país
            Dim regiones = db.Region.Include("Pais").Select(Function(r) New With {
                .RegionID = r.RegionID,
                .Nombre = r.Nombre,
                .TipoRegion = r.TipoRegion, ' Añadido TipoRegion
                .PaisID = r.PaisID,
                .NombrePais = r.Pais.Nombre ' Obtenemos el nombre del País relacionado
            }).ToList()
            Return Json(New With {.data = regiones}, JsonRequestBehavior.AllowGet)
        End Function

        ' GET: Region/GetPaises
        ' Función para obtener los países para el DropDownList en el formulario de Región
        Function GetPaises() As JsonResult
            Dim paises = db.Pais.Select(Function(p) New With {
                .PaisID = p.PaisID,
                .Nombre = p.Nombre
            }).OrderBy(Function(p) p.Nombre).ToList() ' Opcional: ordenar por nombre
            Return Json(paises, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Region/Guardar
        <HttpPost()>
        Function Guardar(region As Region) As JsonResult ' Cambiado de Pais a Region
            Try
                If region.RegionID = 0 Then
                    ' Añadir nueva región
                    db.Region.Add(region)
                Else
                    ' Actualizar región existente
                    Dim existingRegion = db.Region.Find(region.RegionID)
                    If existingRegion IsNot Nothing Then
                        existingRegion.Nombre = region.Nombre
                        existingRegion.TipoRegion = region.TipoRegion ' Actualizado TipoRegion
                        existingRegion.PaisID = region.PaisID ' Actualizado PaisID
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

        ' GET: Region/Buscar/5
        Function Buscar(id As Integer) As JsonResult
            Dim region = db.Region.Find(id) ' Cambiado de db.Pais a db.Region
            If region Is Nothing Then
                Return Json(New With {.success = False, .message = "Región no encontrada."}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(region, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Region/Eliminar/5
        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim region = db.Region.Find(id) ' Cambiado de db.Pais a db.Region
                If region Is Nothing Then
                    Return Json(New With {.success = False, .message = "Región no encontrada."})
                End If

                db.Region.Remove(region) ' Cambiado de db.Pais a db.Region
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