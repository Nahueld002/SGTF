Imports System.Net
Imports System.Web.Mvc
Imports SGTF.Models
Imports System.Data.Entity
Imports System.Linq
Imports System.Data.Entity.Validation
Imports System.Data.Entity.Infrastructure

Namespace Controllers
    Public Class EquiposController
        Inherits Controller

        Private db As New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function Listar(Optional nombre As String = Nothing,
                        Optional codigo As String = Nothing,
                        Optional tipo As String = Nothing,
                        Optional estado As String = Nothing,
                        Optional regionId As Integer? = Nothing,
                        Optional ciudadId As Integer? = Nothing,
                        Optional idTorneo As Integer? = Nothing) As JsonResult
            Try
                Dim query = db.Equipo.Include("Region").Include("Ciudad").AsQueryable()

                If Not String.IsNullOrWhiteSpace(nombre) Then
                    query = query.Where(Function(e) e.Nombre.Contains(nombre))
                End If
                If Not String.IsNullOrWhiteSpace(codigo) Then
                    query = query.Where(Function(e) e.CodigoEquipo.Contains(codigo))
                End If
                If Not String.IsNullOrWhiteSpace(tipo) Then
                    query = query.Where(Function(e) e.TipoEquipo = tipo)
                End If
                If Not String.IsNullOrWhiteSpace(estado) Then
                    query = query.Where(Function(e) e.Estado = estado)
                End If
                If regionId.HasValue AndAlso regionId.Value > 0 Then
                    query = query.Where(Function(e) e.RegionID = regionId.Value)
                End If
                If ciudadId.HasValue AndAlso ciudadId.Value > 0 Then
                    query = query.Where(Function(e) e.CiudadID = ciudadId.Value)
                End If

                If idTorneo.HasValue AndAlso idTorneo.Value > 0 Then
                    query = From e In query
                            Join te In db.TorneoEquipo On e.EquipoID Equals te.EquipoID
                            Where te.TorneoID = idTorneo.Value
                            Select e
                End If

                Dim equipos = query.Select(Function(e) New With {
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

                Return Json(New With {.data = equipos, .success = True}, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al listar equipos: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al listar equipos: " & ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
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
                If ModelState.IsValid Then
                    If equipo.EquipoID = 0 Then
                        If db.Equipo.Any(Function(e) e.CodigoEquipo = equipo.CodigoEquipo) Then
                            Return Json(New With {.success = False, .message = "Ya existe un equipo con este código."})
                        End If
                        db.Equipo.Add(equipo)
                    Else
                        Dim existingEquipo = db.Equipo.Find(equipo.EquipoID)
                        If existingEquipo IsNot Nothing Then
                            If db.Equipo.Any(Function(e) e.CodigoEquipo = equipo.CodigoEquipo AndAlso e.EquipoID <> equipo.EquipoID) Then
                                Return Json(New With {.success = False, .message = "Ya existe otro equipo con este código."})
                            End If

                            existingEquipo.Nombre = equipo.Nombre
                            existingEquipo.CodigoEquipo = equipo.CodigoEquipo
                            existingEquipo.RegionID = equipo.RegionID
                            existingEquipo.CiudadID = equipo.CiudadID
                            existingEquipo.AñoFundacion = equipo.AñoFundacion
                            existingEquipo.ELO = equipo.ELO
                            existingEquipo.TipoEquipo = equipo.TipoEquipo
                            existingEquipo.Estado = equipo.Estado
                            db.Entry(existingEquipo).State = EntityState.Modified
                        Else
                            Return Json(New With {.success = False, .message = "Equipo no encontrado para actualizar."})
                        End If
                    End If
                    db.SaveChanges()
                    Return Json(New With {.success = True})
                Else
                    Dim errors = ModelState.Values.SelectMany(Function(v) v.Errors).Select(Function(e) e.ErrorMessage).ToList()
                    Return Json(New With {.success = False, .message = String.Join(" ", errors)})
                End If
            Catch ex As DbEntityValidationException
                For Each validationError In ex.EntityValidationErrors
                    For Each errorProperty In validationError.ValidationErrors
                        System.Diagnostics.Debug.WriteLine($"Property: {errorProperty.PropertyName}, Error: {errorProperty.ErrorMessage}")
                    Next
                Next
                Return Json(New With {.success = False, .message = "Error de validación de datos: " & ex.Message & " Consulte los logs para más detalles."})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al guardar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al guardar el equipo. " & ex.Message})
            End Try
        End Function

        Function Buscar(id As Integer) As JsonResult
            Try
                Dim equipo = db.Equipo.Find(id)
                If equipo Is Nothing Then
                    Return Json(New With {.success = False, .message = "Equipo no encontrado."}, JsonRequestBehavior.AllowGet)
                End If

                Return Json(New With {
                    .success = True,
                    .EquipoID = equipo.EquipoID,
                    .Nombre = equipo.Nombre,
                    .CodigoEquipo = equipo.CodigoEquipo,
                    .RegionID = equipo.RegionID,
                    .CiudadID = equipo.CiudadID,
                    .AñoFundacion = equipo.AñoFundacion,
                    .ELO = equipo.ELO,
                    .TipoEquipo = equipo.TipoEquipo,
                    .Estado = equipo.Estado
                }, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al buscar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al buscar el equipo: " & ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
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
            Catch ex As DbUpdateException
                System.Diagnostics.Debug.WriteLine($"DbUpdateException al eliminar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar el equipo. Es posible que esté asociado a otros registros (ej. jugadores, partidos) y no se pueda eliminar directamente."})
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error general al eliminar equipo: {ex.Message}")
                Return Json(New With {.success = False, .message = "Error al eliminar el equipo: " & ex.Message})
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