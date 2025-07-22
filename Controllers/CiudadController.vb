Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models ' Asegúrate de que tu modelo Ciudad esté en este Namespace
Imports System.Data.Entity

Namespace Controllers
    Public Class CiudadController ' Renombrado de RegionController a CiudadController
        Inherits Controller

        Private db As New FutbolDB2Entities() ' Tu contexto de base de datos

        ' GET: Ciudad
        Function Index() As ActionResult
            Return View()
        End Function

        ' GET: Ciudad/Listar
        Function Listar() As JsonResult
            ' Incluimos el objeto Region para poder acceder al nombre de la región
            Dim ciudades = db.Ciudad.Include("Region").Select(Function(c) New With {
                .CiudadID = c.CiudadID,
                .Nombre = c.Nombre,
                .RegionID = c.RegionID,
                .EsCapital = c.EsCapital, ' Añadido EsCapital
                .NombreRegion = c.Region.Nombre ' Obtenemos el nombre de la Región relacionada
            }).ToList()
            Return Json(New With {.data = ciudades}, JsonRequestBehavior.AllowGet)
        End Function

        ' GET: Ciudad/GetRegiones
        ' Función para obtener las regiones para el DropDownList en el formulario de Ciudad
        Function GetRegiones() As JsonResult
            Dim regiones = db.Region.Select(Function(r) New With {
                .RegionID = r.RegionID,
                .Nombre = r.Nombre
            }).OrderBy(Function(r) r.Nombre).ToList() ' Opcional: ordenar por nombre
            Return Json(regiones, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Ciudad/Guardar
        <HttpPost()>
        Function Guardar(ciudad As Ciudad) As JsonResult ' Cambiado de Region a Ciudad
            Try
                If ciudad.CiudadID = 0 Then
                    ' Añadir nueva ciudad
                    db.Ciudad.Add(ciudad)
                Else
                    ' Actualizar ciudad existente
                    Dim existingCiudad = db.Ciudad.Find(ciudad.CiudadID)
                    If existingCiudad IsNot Nothing Then
                        existingCiudad.Nombre = ciudad.Nombre
                        existingCiudad.RegionID = ciudad.RegionID
                        existingCiudad.EsCapital = ciudad.EsCapital ' Actualizado EsCapital
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

        ' GET: Ciudad/Buscar/5
        Function Buscar(id As Integer) As JsonResult
            Dim ciudad = db.Ciudad.Find(id) ' Cambiado de db.Region a db.Ciudad
            If ciudad Is Nothing Then
                Return Json(New With {.success = False, .message = "Ciudad no encontrada."}, JsonRequestBehavior.AllowGet)
            End If
            Return Json(ciudad, JsonRequestBehavior.AllowGet)
        End Function

        ' POST: Ciudad/Eliminar/5
        <HttpPost()>
        Function Eliminar(id As Integer) As JsonResult
            Try
                Dim ciudad = db.Ciudad.Find(id) ' Cambiado de db.Region a db.Ciudad
                If ciudad Is Nothing Then
                    Return Json(New With {.success = False, .message = "Ciudad no encontrada."})
                End If

                db.Ciudad.Remove(ciudad) ' Cambiado de db.Region a db.Ciudad
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