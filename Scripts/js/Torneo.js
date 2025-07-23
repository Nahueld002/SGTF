const filtros = { nombre: '', tipo: '', categoria: '', estado: '' };

$(document).ready(function () {
    Listar();
    cargarComboBoxes();

    document.getElementById('filter-name').addEventListener('input', e => {
        filtros.nombre = e.target.value.toLowerCase();
        Listar();
    });

    document.getElementById('filter-type').addEventListener('change', e => {
        filtros.tipo = e.target.value;
        Listar();
    });

    document.getElementById('filter-category').addEventListener('change', e => {
        filtros.categoria = e.target.value;
        Listar();
    });

    document.getElementById('filter-status').addEventListener('change', e => {
        filtros.estado = e.target.value;
        Listar();
    });

    const btnAbrirModalNuevo = document.getElementById('btnAbrirModalNuevo');
    if (btnAbrirModalNuevo) {
        btnAbrirModalNuevo.addEventListener('click', () => AbrirModal(0));
    }

    const btnGuardar = document.getElementById('btnGuardar');
    if (btnGuardar) {
        btnGuardar.addEventListener('click', Guardar);
    }

    document.addEventListener('click', function (event) {
        const modal = document.getElementById('modalTorneo');
        const modalContent = document.getElementById('modalTorneoContent');

        if (event.target === modal && !modalContent.contains(event.target)) {
            CerrarModal();
        }
    });

    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
            const modal = document.getElementById('modalTorneo');
            if (!modal.classList.contains('hidden')) {
                CerrarModal();
            }
        }
    });
});

function Listar() {
    $.get('/Torneo/ListarTorneos', function (response) {
        const torneos = response.data && Array.isArray(response.data) ? response.data : response;

        const torneosFiltrados = torneos.filter(torneo => {
            const nombreTorneo = (torneo.Nombre || '').toLowerCase();
            const tipoTorneo = (torneo.TipoTorneo || '');
            const categoriaTorneo = (torneo.Categoria || '');
            const estadoTorneo = (torneo.Estado || '');

            return (
                (filtros.nombre === '' || nombreTorneo.includes(filtros.nombre)) &&
                (filtros.tipo === '' || tipoTorneo === filtros.tipo) &&
                (filtros.categoria === '' || categoriaTorneo === filtros.categoria) &&
                (filtros.estado === '' || estadoTorneo === filtros.estado)
            );
        });
        crearListado(torneosFiltrados);
    }).fail(function (xhr, status, error) {
        console.error("Error al listar torneos:", error);
        $('#tblTorneosBody').empty().append('<tr><td colspan="9" class="px-6 py-4 text-center text-red-400">Error al cargar los torneos.</td></tr>');
    });
}

function crearListado(data) {
    const tablaBody = document.querySelector("#tblTorneosBody");
    if (!tablaBody) {
        console.error("Error: Elemento <tbody> con ID #tblTorneosBody no encontrado.");
        return;
    }

    tablaBody.innerHTML = "";
    let filaPar = true;

    if (data && Array.isArray(data) && data.length > 0) {
        data.forEach(item => {
            const claseFila = filaPar ? "table-row-even" : "table-row-odd";
            filaPar = !filaPar;

            let estadoClase = '';
            switch (item.Estado) {
                case 'Activo': estadoClase = 'bg-green-500 text-white'; break;
                case 'Extinto': estadoClase = 'bg-red-500 text-white'; break;
                case 'Suspendido': estadoClase = 'bg-yellow-500 text-black'; break;
                default: estadoClase = 'bg-gray-500 text-white'; break;
            }

            const row = `
                <tr class="${claseFila} border-b border-gray-700">
                    <td class="px-6 py-4 font-medium text-white">${item.Nombre || ''}</td>
                    <td class="px-6 py-4">${item.TipoTorneo || ''}</td>
                    <td class="px-6 py-4">${item.Categoria || ''}</td>
                    <td class="px-6 py-4"><span class="px-2 py-1 text-xs font-semibold rounded-full ${estadoClase}">${item.Estado || ''}</span></td>
                    <td class="px-6 py-4">${item.Confederacion || ''}</td>
                    <td class="px-6 py-4">${item.Pais || ''}</td>
                    <td class="px-6 py-4">${item.Region || ''}</td>
                    <td class="px-6 py-4">${item.Ciudad || ''}</td>
                    <td class="px-6 py-4 text-center">
                        <div class="flex items-center justify-center gap-2">
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Ver Tabla de Posiciones"
                                    onclick="VerTablaPosiciones(${item.TorneoID}, '${(item.Nombre || '').replace(/'/g, "\\'")}')">
                                <i data-lucide="award" class="w-4 h-4 text-purple-400"></i>
                            </button>
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="AbrirModal(${item.TorneoID})">
                                <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                            </button>
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="ConfirmarEliminar(${item.TorneoID})">
                                <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                            </button>
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Asignar Equipos" onclick="alert('Asignar equipos no implementado')">
                                <i data-lucide="users" class="w-4 h-4 text-green-400"></i>
                            </button>
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Generar Fixture" onclick="alert('Generar fixture no implementado')">
                                <i data-lucide="calendar-plus" class="w-4 h-4 text-purple-400"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `;
            tablaBody.insertAdjacentHTML('beforeend', row);
        });
        if (typeof lucide !== 'undefined' && lucide.createIcons) {
            lucide.createIcons();
        }
    } else {
        tablaBody.innerHTML = '<tr><td colspan="9" class="px-6 py-4 text-center text-gray-400">No se encontraron torneos.</td></tr>';
    }
}

function cargarComboBoxes() {
    cargarCiudades();
    cargarPaises();
    cargarRegiones();
    cargarConfederaciones();
}

function cargarCiudades() {
    $.get('/Torneo/CargarCiudades')
        .done(function (data) {
            llenarCombo('txtCiudadID', data, 'Seleccionar ciudad...');
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar ciudades:', error);
        });
}

function cargarPaises() {
    $.get('/Torneo/CargarPaises')
        .done(function (data) {
            llenarCombo('txtPaisID', data, 'Seleccionar país...');
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar países:', error);
        });
}

function cargarRegiones() {
    $.get('/Torneo/CargarRegiones')
        .done(function (data) {
            llenarCombo('txtRegionID', data, 'Seleccionar región...');
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar regiones:', error);
        });
}

function cargarConfederaciones() {
    $.get('/Torneo/CargarConfederaciones')
        .done(function (data) {
            llenarCombo('txtConfederacionID', data, 'Seleccionar confederación...');
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar confederaciones:', error);
        });
}

function cargarComboBoxesJerarquicos() {
    cargarConfederaciones();

    $('#txtConfederacionID').off('change').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarPaisesPorConfederacion', { confederacionID: id }, function (data) {
                llenarCombo('txtPaisID', data, 'Seleccionar país...');
                limpiarCombo('txtRegionID', 'Seleccionar región...');
                limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
            }).fail(function (xhr, status, error) {
                console.error('Error al cargar países por confederación:', error);
            });
        } else {
            limpiarCombo('txtPaisID', 'Seleccionar país...');
            limpiarCombo('txtRegionID', 'Seleccionar región...');
            limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
        }
    });

    $('#txtPaisID').off('change').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarRegionesPorPais', { paisID: id }, function (data) {
                llenarCombo('txtRegionID', data, 'Seleccionar región...');
                limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
            }).fail(function (xhr, status, error) {
                console.error('Error al cargar regiones por país:', error);
            });
        } else {
            limpiarCombo('txtRegionID', 'Seleccionar región...');
            limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
        }
    });

    $('#txtRegionID').off('change').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarCiudadesPorRegion', { regionID: id }, function (data) {
                llenarCombo('txtCiudadID', data, 'Seleccionar ciudad...');
            }).fail(function (xhr, status, error) {
                console.error('Error al cargar ciudades por región:', error);
            });
        } else {
            limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
        }
    });
}

function llenarCombo(id, data, placeholder) {
    const select = document.getElementById(id);
    if (!select) {
        console.error(`Elemento select con ID ${id} no encontrado.`);
        return;
    }
    select.innerHTML = `<option value="">${placeholder}</option>`;
    if (data && Array.isArray(data)) {
        data.forEach(item => {
            const option = document.createElement('option');
            option.value = item.ID || item.ConfederacionID || item.PaisID || item.RegionID || item.CiudadID;
            option.textContent = item.Nombre;
            select.appendChild(option);
        });
    }
}

function limpiarCombo(id, placeholder = 'Seleccionar...') {
    const select = document.getElementById(id);
    if (select) {
        select.innerHTML = `<option value="">${placeholder}</option>`;
    }
}

function Guardar() {
    const frm = new FormData();
    frm.append('TorneoID', document.getElementById('txtTorneoID').value);
    frm.append('Nombre', document.getElementById('txtNombre').value);
    frm.append('TipoTorneo', document.getElementById('txtTipoTorneo').value);
    frm.append('Categoria', document.getElementById('txtCategoria').value);
    frm.append('Estado', document.getElementById('txtEstado').value);
    frm.append('CiudadID', document.getElementById('txtCiudadID').value || '');
    frm.append('PaisID', document.getElementById('txtPaisID').value || '');
    frm.append('RegionID', document.getElementById('txtRegionID').value || '');
    frm.append('ConfederacionID', document.getElementById('txtConfederacionID').value || '');

    $.ajax({
        type: 'POST',
        url: '/Torneo/Guardar',
        processData: false,
        contentType: false,
        data: frm,
        success: function (data) {
            if (data == 1) {
                alert('Registro guardado con éxito!');
                CerrarModal();
                Listar();
            } else {
                alert('Error al guardar el registro. (Respuesta del servidor: ' + data + ')');
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al guardar el registro:", error, xhr.responseText);
            alert('Error de conexión al guardar el registro. Verifique la consola para más detalles.');
        }
    });
}

window.ConfirmarEliminar = function (id) {
    if (confirm('¿Estás seguro de que deseas eliminar este torneo? Esta acción es irreversible.')) {
        Eliminar(id);
    }
}

function Eliminar(id) {
    $.ajax({
        type: 'POST',
        url: '/Torneo/Eliminar/' + id,
        success: function (data) {
            if (data == 1) {
                alert('Registro eliminado correctamente!');
                Listar();
            } else {
                alert('Error al eliminar el registro. (Respuesta del servidor: ' + data + ')');
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al eliminar el registro:", error, xhr.responseText);
            alert('Error de conexión al eliminar el registro. Verifique la consola para más detalles.');
        }
    });
}

function Limpiar() {
    document.getElementById('formTorneo').reset();
    document.getElementById('txtTorneoID').value = '';
    limpiarCombo('txtConfederacionID', 'Seleccionar confederación...');
    limpiarCombo('txtPaisID', 'Seleccionar país...');
    limpiarCombo('txtRegionID', 'Seleccionar región...');
    limpiarCombo('txtCiudadID', 'Seleccionar ciudad...');
}

window.AbrirModal = function (torneoID = 0) {
    Limpiar();
    cargarComboBoxesJerarquicos();

    if (torneoID > 0) {
        Recuperar(torneoID);
        document.getElementById('modal-title').textContent = 'Editar Torneo';
    } else {
        document.getElementById('modal-title').textContent = 'Nuevo Torneo';
    }

    document.getElementById('modalTorneo').classList.remove('hidden');
    setTimeout(() => {
        document.getElementById('modalTorneoContent').classList.remove('scale-95', 'opacity-0');
        document.getElementById('modalTorneoContent').classList.add('scale-100', 'opacity-100');
    }, 10);
};

window.CerrarModal = function () {
    document.getElementById('modalTorneoContent').classList.add('scale-95', 'opacity-0');
    document.getElementById('modalTorneoContent').classList.remove('scale-100', 'opacity-100');

    setTimeout(() => {
        document.getElementById('modalTorneo').classList.add('hidden');
    }, 200);
};

window.VerTablaPosiciones = function (torneoID, nombre) {
    const encodedNombre = encodeURIComponent(nombre);
    window.location.href = `/Torneo/TablaPosiciones?torneoID=${torneoID}&torneoNombre=${encodedNombre}`;
};

function Recuperar(id) {
    $.get('/Torneo/RecuperarTorneo/' + id)
        .done(function (data) {
            if (!data || data.length === 0) {
                console.error("No data received for TorneoID:", id);
                alert('No se encontraron datos para el torneo.');
                return;
            }
            const t = data[0];

            document.getElementById('txtTorneoID').value = t.TorneoID;
            document.getElementById('txtNombre').value = t.Nombre || '';
            document.getElementById('txtTipoTorneo').value = t.TipoTorneo || '';
            document.getElementById('txtCategoria').value = t.Categoria || '';
            document.getElementById('txtEstado').value = t.Estado || '';

            Promise.resolve()
                .then(() => {
                    document.getElementById('txtConfederacionID').value = t.ConfederacionID || '';
                    return new Promise(resolve => {
                        let processed = false;
                        $('#txtConfederacionID').on('change.temp', function () { processed = true; });
                        $('#txtConfederacionID').trigger('change');
                        const checkProcessed = () => {
                            if (processed || !t.ConfederacionID) {
                                $('#txtConfederacionID').off('change.temp');
                                resolve();
                            } else {
                                setTimeout(checkProcessed, 50);
                            }
                        };
                        setTimeout(checkProcessed, 50);
                    });
                })
                .then(() => {
                    document.getElementById('txtPaisID').value = t.PaisID || '';
                    return new Promise(resolve => {
                        let processed = false;
                        $('#txtPaisID').on('change.temp', function () { processed = true; });
                        $('#txtPaisID').trigger('change');
                        const checkProcessed = () => {
                            if (processed || !t.PaisID) {
                                $('#txtPaisID').off('change.temp');
                                resolve();
                            } else {
                                setTimeout(checkProcessed, 50);
                            }
                        };
                        setTimeout(checkProcessed, 50);
                    });
                })
                .then(() => {
                    document.getElementById('txtRegionID').value = t.RegionID || '';
                    return new Promise(resolve => {
                        let processed = false;
                        $('#txtRegionID').on('change.temp', function () { processed = true; });
                        $('#txtRegionID').trigger('change');
                        const checkProcessed = () => {
                            if (processed || !t.RegionID) {
                                $('#txtRegionID').off('change.temp');
                                resolve();
                            } else {
                                setTimeout(checkProcessed, 50);
                            }
                        };
                        setTimeout(checkProcessed, 50);
                    });
                })
                .then(() => {
                    document.getElementById('txtCiudadID').value = t.CiudadID || '';
                })
                .catch(error => {
                    console.error("Error during chained combobox loading:", error);
                    alert('Hubo un problema al cargar la ubicación del torneo para edición.');
                });
        })
        .fail(function (xhr, status, error) {
            console.error("Error al recuperar el torneo:", error, xhr.responseText);
            alert('Error al recuperar los datos del torneo para edición. Verifique la consola.');
        });
}