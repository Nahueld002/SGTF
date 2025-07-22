document.addEventListener("DOMContentLoaded", () => {
    Listar();
    cargarComboBoxes(); // Cargar los comboboxes al iniciar

    // Filtros
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
});

const filtros = { nombre: '', tipo: '', categoria: '', estado: '' };

function Listar() {
    $.get('/Torneo/ListarTorneos', function (data) {
        crearListado(data);
    });
}

function crearListado(data) {
    let contenido = "";
    let filaPar = true;

    const filtrado = data.filter(t =>
        (!filtros.nombre || (t.Nombre ?? '').toLowerCase().includes(filtros.nombre)) &&
        (!filtros.tipo || (t.TipoTorneo ?? '') === filtros.tipo) &&
        (!filtros.categoria || (t.Categoria ?? '') === filtros.categoria) &&
        (!filtros.estado || (t.Estado ?? '') === filtros.estado)
    );

    filtrado.forEach(torneo => {
        const claseFila = filaPar ? 'table-row-even' : 'table-row-odd';
        filaPar = !filaPar;

        let estadoClase = 'bg-gray-500/20 text-gray-300';
        if (torneo.Estado === 'Activo') estadoClase = 'bg-green-500/20 text-green-400';
        else if (torneo.Estado === 'Extinto') estadoClase = 'bg-red-500/20 text-red-400';

        contenido += `
            <tr class="${claseFila} hover:bg-gray-600/50 transition-colors">
                <td class="p-4 font-medium">${torneo.Nombre}</td>
                <td class="p-4">${torneo.TipoTorneo}</td>
                <td class="p-4">${torneo.Categoria}</td>
                <td class="p-4"><span class="px-2 py-1 text-xs font-semibold rounded-full ${estadoClase}">${torneo.Estado}</span></td>
                <td class="p-4">${torneo.Ciudad}</td>
                <td class="p-4 text-center">
                <div class="flex items-center justify-center gap-2">
                    <button class="p-2 rounded-md hover:bg-gray-600" title="Ver Detalles"
                            onclick="window.location.href='/Torneo/TablaPosiciones?torneoID=${torneo.TorneoID}&torneoNombre=${encodeURIComponent(torneo.Nombre)}'">
                            <i data-lucide="eye" class="w-4 h-4 text-blue-400"></i>
                        </button>
                    <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="AbrirEditar(${torneo.TorneoID})">
                        <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                    </button>
                    <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="ConfirmarEliminar(${torneo.TorneoID})">
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
            </tr>`;
    });

    const tablaHTML = `
        <table class="w-full text-left">
            <thead class="table-header">
                <tr>
                    <th class="p-4 font-semibold">Nombre</th>
                    <th class="p-4 font-semibold">Tipo</th>
                    <th class="p-4 font-semibold">Categoría</th>
                    <th class="p-4 font-semibold">Estado</th>
                    <th class="p-4 font-semibold">Ciudad</th>
                    <th class="p-4 font-semibold text-center">Acciones</th>
                </tr>
            </thead>
            <tbody>
                ${contenido}
            </tbody>
        </table>`;

    document.getElementById("DIVTorneo").innerHTML = tablaHTML;
    lucide.createIcons();
}

// Función para cargar todos los comboboxes
function cargarComboBoxes() {
    cargarCiudades();
    cargarPaises();
    cargarRegiones();
    cargarConfederaciones();
}

// Función para cargar ciudades con debugging
function cargarCiudades() {
    console.log('Iniciando carga de ciudades...');
    $.get('/Torneo/CargarCiudades')
        .done(function (data) {
            console.log('Datos de ciudades recibidos:', data);
            const select = document.getElementById('txtCiudadID');
            if (!select) {
                console.error('Elemento txtCiudadID no encontrado');
                return;
            }

            select.innerHTML = '<option value="">Seleccionar ciudad...</option>';

            if (data && Array.isArray(data)) {
                data.forEach(ciudad => {
                    const option = document.createElement('option');
                    option.value = ciudad.CiudadID;
                    option.textContent = ciudad.Nombre;
                    select.appendChild(option);
                });
                console.log(`${data.length} ciudades cargadas`);
            } else {
                console.error('Los datos de ciudades no son un array válido:', data);
            }
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar ciudades:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);
        });
}

// Función para cargar países con debugging
function cargarPaises() {
    console.log('Iniciando carga de países...');
    $.get('/Torneo/CargarPaises')
        .done(function (data) {
            console.log('Datos de países recibidos:', data);
            const select = document.getElementById('txtPaisID');
            if (!select) {
                console.error('Elemento txtPaisID no encontrado');
                return;
            }

            select.innerHTML = '<option value="">Seleccionar país...</option>';

            if (data && Array.isArray(data)) {
                data.forEach(pais => {
                    const option = document.createElement('option');
                    option.value = pais.PaisID;
                    option.textContent = pais.Nombre;
                    select.appendChild(option);
                });
                console.log(`${data.length} países cargados`);
            } else {
                console.error('Los datos de países no son un array válido:', data);
            }
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar países:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);
        });
}

// Función para cargar regiones con debugging
function cargarRegiones() {
    console.log('Iniciando carga de regiones...');
    $.get('/Torneo/CargarRegiones')
        .done(function (data) {
            console.log('Datos de regiones recibidos:', data);
            const select = document.getElementById('txtRegionID');
            if (!select) {
                console.error('Elemento txtRegionID no encontrado');
                return;
            }

            select.innerHTML = '<option value="">Seleccionar región...</option>';

            if (data && Array.isArray(data)) {
                data.forEach(region => {
                    const option = document.createElement('option');
                    option.value = region.RegionID;
                    option.textContent = region.Nombre;
                    select.appendChild(option);
                });
                console.log(`${data.length} regiones cargadas`);
            } else {
                console.error('Los datos de regiones no son un array válido:', data);
            }
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar regiones:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);
        });
}

// Función para cargar confederaciones con debugging
function cargarConfederaciones() {
    console.log('Iniciando carga de confederaciones...');
    $.get('/Torneo/CargarConfederaciones')
        .done(function (data) {
            console.log('Datos de confederaciones recibidos:', data);
            const select = document.getElementById('txtConfederacionID');
            if (!select) {
                console.error('Elemento txtConfederacionID no encontrado');
                return;
            }

            select.innerHTML = '<option value="">Seleccionar confederación...</option>';

            if (data && Array.isArray(data)) {
                data.forEach(confederacion => {
                    const option = document.createElement('option');
                    option.value = confederacion.ConfederacionID;
                    option.textContent = confederacion.Nombre;
                    select.appendChild(option);
                });
                console.log(`${data.length} confederaciones cargadas`);
            } else {
                console.error('Los datos de confederaciones no son un array válido:', data);
            }
        })
        .fail(function (xhr, status, error) {
            console.error('Error al cargar confederaciones:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);
        });
}

// Función para cargar todos los comboboxes con debugging
function cargarComboBoxesJerarquicos() {
    cargarConfederaciones();

    $('#txtConfederacionID').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarPaisesPorConfederacion', { confederacionID: id }, function (data) {
                llenarCombo('txtPaisID', data, 'Seleccionar país...');
                limpiarCombo('txtRegionID');
                limpiarCombo('txtCiudadID');
            });
        }
    });

    $('#txtPaisID').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarRegionesPorPais', { paisID: id }, function (data) {
                llenarCombo('txtRegionID', data, 'Seleccionar región...');
                limpiarCombo('txtCiudadID');
            });
        }
    });

    $('#txtRegionID').on('change', function () {
        const id = $(this).val();
        if (id) {
            $.get('/Torneo/CargarCiudadesPorRegion', { regionID: id }, function (data) {
                llenarCombo('txtCiudadID', data, 'Seleccionar ciudad...');
            });
        }
    });
}

function llenarCombo(id, data, placeholder) {
    const select = document.getElementById(id);
    select.innerHTML = `<option value="">${placeholder}</option>`;
    data.forEach(item => {
        const option = document.createElement('option');
        option.value = item.ID || item.ConfederacionID || item.PaisID || item.RegionID || item.CiudadID;
        option.textContent = item.Nombre;
        select.appendChild(option);
    });
}

function limpiarCombo(id) {
    const select = document.getElementById(id);
    if (select) select.innerHTML = '<option value="">Seleccionar...</option>';
}


// Función para abrir el modal (nuevo torneo)
function AbrirModal() {
    Limpiar();
    cargarComboBoxesJerarquicos(); // Recargar los comboboxes
    document.getElementById('modal-title').textContent = 'Nuevo Torneo';
    document.getElementById('modalTorneo').classList.remove('hidden');

    // Animación de entrada
    setTimeout(() => {
        document.getElementById('modalTorneoContent').classList.remove('scale-95', 'opacity-0');
        document.getElementById('modalTorneoContent').classList.add('scale-100', 'opacity-100');
    }, 10);
}

// Función para cerrar el modal
function CerrarModal() {
    document.getElementById('modalTorneoContent').classList.add('scale-95', 'opacity-0');
    document.getElementById('modalTorneoContent').classList.remove('scale-100', 'opacity-100');

    setTimeout(() => {
        document.getElementById('modalTorneo').classList.add('hidden');
    }, 200);
}

// Función para guardar (crear o actualizar)
function Guardar() {
    const frm = new FormData();
    frm.append('TorneoID', document.getElementById('txtTorneoID').value);
    frm.append('Nombre', document.getElementById('txtNombre').value);
    frm.append('TipoTorneo', document.getElementById('txtTipoTorneo').value);
    frm.append('Categoria', document.getElementById('txtCategoria').value);
    frm.append('Estado', document.getElementById('txtEstado').value);
    frm.append('CiudadID', document.getElementById('txtCiudadID').value);
    frm.append('PaisID', document.getElementById('txtPaisID').value);
    frm.append('RegionID', document.getElementById('txtRegionID').value);
    frm.append('ConfederacionID', document.getElementById('txtConfederacionID').value);

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
                alert('Error al guardar el registro.');
            }
        },
        error: function () {
            alert('Error de conexión al guardar el registro.');
        }
    });
}

// Función para confirmar eliminación
function ConfirmarEliminar(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este torneo?')) {
        Eliminar(id);
    }
}

// Función para eliminar
function Eliminar(id) {
    $.ajax({
        type: 'POST',
        url: '/Torneo/Eliminar/' + id,
        success: function (data) {
            if (data == 1) {
                alert('Registro eliminado correctamente!');
                Listar();
            } else {
                alert('Error al eliminar el registro.');
            }
        },
        error: function () {
            alert('Error de conexión al eliminar el registro.');
        }
    });
}

// Función para abrir modal de edición con datos
function AbrirEditar(id) {
    $.get('/Torneo/RecuperarTorneo/' + id, function (data) {
        const t = data[0];

        // Cargar comboboxes primero
        cargarComboBoxesJerarquicos();

        // Esperar un poco para que se carguen los comboboxes y luego establecer los valores
        setTimeout(() => {
            // Llenar los campos del formulario
            document.getElementById('txtTorneoID').value = t.TorneoID;
            document.getElementById('txtNombre').value = t.Nombre || '';
            document.getElementById('txtTipoTorneo').value = t.TipoTorneo || '';
            document.getElementById('txtCategoria').value = t.Categoria || '';
            document.getElementById('txtEstado').value = t.Estado || '';
            document.getElementById('txtCiudadID').value = t.CiudadID || '';
            document.getElementById('txtPaisID').value = t.PaisID || '';
            document.getElementById('txtRegionID').value = t.RegionID || '';
            document.getElementById('txtConfederacionID').value = t.ConfederacionID || '';

            // Cambiar el título del modal
            document.getElementById('modal-title').textContent = 'Editar Torneo';

            // Mostrar el modal
            document.getElementById('modalTorneo').classList.remove('hidden');

            // Animación de entrada
            setTimeout(() => {
                document.getElementById('modalTorneoContent').classList.remove('scale-95', 'opacity-0');
                document.getElementById('modalTorneoContent').classList.add('scale-100', 'opacity-100');
            }, 10);
        }, 500); // Esperar 500ms para que se carguen los comboboxes
    })
        .fail(function () {
            alert('Error al recuperar los datos del torneo.');
        });
}

// Función para limpiar el formulario
function Limpiar() {
    document.getElementById('formTorneo').reset();
    document.getElementById('txtTorneoID').value = '';
}

// Cerrar modal al hacer clic fuera de él
document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalTorneo');
    const modalContent = document.getElementById('modalTorneoContent');

    if (event.target === modal && !modalContent.contains(event.target)) {
        CerrarModal();
    }
});

// Cerrar modal con tecla ESC
document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalTorneo');
        if (!modal.classList.contains('hidden')) {
            CerrarModal();
        }
    }
});