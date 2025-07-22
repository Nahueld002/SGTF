document.addEventListener("DOMContentLoaded", () => {
    ListarPartidos();
    cargarComboBoxes(); // Cargar los comboboxes al iniciar

    // Filtros dinámicos
    document.getElementById('filter-torneo').addEventListener('input', e => {
        filtros.torneo = e.target.value.toLowerCase();
        ListarPartidos();
    });

    document.getElementById('filter-fase').addEventListener('change', e => {
        filtros.fase = e.target.value;
        ListarPartidos();
    });

    document.getElementById('filter-estado').addEventListener('change', e => {
        filtros.estado = e.target.value;
        ListarPartidos();
    });
});

const filtros = { torneo: '', fase: '', estado: '' };

function ListarPartidos() {
    $.get('/Partido/ListarPartido', function (data) {
        crearListadoPartidos(data);
    });
}

function crearListadoPartidos(data) {
    let contenido = "";
    let filaPar = true;

    const filtrado = data.filter(p =>
        (!filtros.torneo || (p.Torneo ?? '').toLowerCase().includes(filtros.torneo)) &&
        (!filtros.fase || (p.Fase ?? '') === filtros.fase) &&
        (!filtros.estado || (p.Estado ?? '') === filtros.estado)
    );

    filtrado.forEach(partido => {
        const claseFila = filaPar ? 'table-row-even' : 'table-row-odd';
        filaPar = !filaPar;

        let estadoClase = 'bg-gray-500/20 text-gray-300';
        if (partido.Estado === 'Jugado') estadoClase = 'bg-green-500/20 text-green-400';
        else if (partido.Estado === 'Suspendido') estadoClase = 'bg-red-500/20 text-red-400';

        contenido += `
            <tr class="${claseFila} hover:bg-gray-600/50 transition-colors">
                <td class="p-4">${partido.Torneo}</td>
                <td class="p-4">${partido.Local}</td>
                <td class="p-4 text-center font-semibold">${partido.Marcador}</td>
                <td class="p-4">${partido.Visitante}</td>
                <td class="p-4">${partido.Fase} ${partido.Grupo ? '(' + partido.Grupo + ')' : ''}</td>
                <td class="p-4">${partido.Año}</td>
                <td class="p-4"><span class="px-2 py-1 text-xs font-semibold rounded-full ${estadoClase}">${partido.Estado}</span></td>
                <td class="p-4 text-center">
                    <div class="flex items-center justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Ver Detalles" onclick="alert('Ver detalles no implementado')">
                            <i data-lucide="eye" class="w-4 h-4 text-blue-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="AbrirEditar(${partido.PartidoID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="ConfirmarEliminar(${partido.PartidoID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                </td>
            </tr>`;
    });

    const tablaHTML = `
        <table class="w-full text-left">
            <thead class="table-header">
                <tr>
                    <th class="p-4 font-semibold">Torneo</th>
                    <th class="p-4 font-semibold">Local</th>
                    <th class="p-4 font-semibold text-center">Marcador</th>
                    <th class="p-4 font-semibold">Visitante</th>
                    <th class="p-4 font-semibold">Fase</th>
                    <th class="p-4 font-semibold">Año</th>
                    <th class="p-4 font-semibold">Estado</th>
                    <th class="p-4 font-semibold text-center">Acciones</th>
                </tr>
            </thead>
            <tbody>
                ${contenido}
            </tbody>
        </table>`;

    document.getElementById("DIVPartido").innerHTML = tablaHTML;
    lucide.createIcons();
}

function cargarComboBoxes() {
    $.get('/Partido/CargarTorneos', function (torneos) {
        llenarCombo('txtTorneoID', torneos, null, 'TorneoID');
    });

    $.get('/Partido/CargarTorneoEquipos', function (equipos) {
        llenarCombo('txtLocalID', equipos, null, 'TorneoEquipoID');
        llenarCombo('txtVisitanteID', equipos, null, 'TorneoEquipoID');
    });
}

function AbrirModal() {
    Limpiar(); // Limpia el formulario
    cargarComboBoxes(); // Carga torneos y equipos

    document.getElementById('modal-title').textContent = 'Nuevo Partido';
    document.getElementById('modalPartido').classList.remove('hidden');

    setTimeout(() => {
        document.getElementById('modalPartidoContent').classList.remove('scale-95', 'opacity-0');
        document.getElementById('modalPartidoContent').classList.add('scale-100', 'opacity-100');
    }, 10);
}


function CerrarModal() {
    document.getElementById('modalPartidoContent').classList.add('scale-95', 'opacity-0');
    document.getElementById('modalPartidoContent').classList.remove('scale-100', 'opacity-100');

    setTimeout(() => {
        document.getElementById('modalPartido').classList.add('hidden');
        Limpiar(); // opcional
    }, 200);
}


function Guardar() {
    const local = document.getElementById('txtLocalID').value;
    const visitante = document.getElementById('txtVisitanteID').value;

    if (local === visitante) {
        alert('⚠️ No se puede seleccionar el mismo equipo como local y visitante.');
        return;
    }

    const frm = new FormData();
    frm.append('PartidoID', document.getElementById('txtPartidoID').value);
    frm.append('TorneoID', document.getElementById('txtTorneoID').value);
    frm.append('EquipoLocalTorneoEquipoID', local);
    frm.append('EquipoVisitanteTorneoEquipoID', visitante);
    frm.append('GolesLocal', document.getElementById('txtGolesLocal').value);
    frm.append('GolesVisitante', document.getElementById('txtGolesVisitante').value);
    frm.append('NroFecha', document.getElementById('txtFechaNro').value);
    frm.append('AñoParticipacion', document.getElementById('txtAnio').value);
    frm.append('Fase', document.getElementById('txtFase').value);
    frm.append('Grupo', document.getElementById('txtGrupo').value);
    frm.append('Estado', document.getElementById('txtEstado').value);

    $.ajax({
        type: 'POST',
        url: '/Partido/Guardar',
        processData: false,
        contentType: false,
        data: frm,
        success: function (data) {
            if (data == 1) {
                alert('✅ Partido guardado con éxito!');
                CerrarModal();
                ListarPartidos();
            } else {
                alert('❌ Error al guardar el partido.');
            }
        },
        error: function () {
            alert('❌ Error de conexión al guardar el partido.');
        }
    });
}


function ConfirmarEliminar(id) {
    if (confirm('¿Eliminar este partido?')) {
        Eliminar(id);
    }
}

function Eliminar(id) {
    $.ajax({
        type: 'POST',
        url: '/Partido/Eliminar/' + id,
        success: function (data) {
            if (data == 1) {
                alert('Partido eliminado correctamente!');
                ListarPartidos();
            } else {
                alert('Error al eliminar el partido.');
            }
        },
        error: function () {
            alert('Error de conexión al eliminar el partido.');
        }
    });
}
function llenarCombo(id, data, selectedId, keyId) {
    const select = document.getElementById(id);
    if (!select) return;

    select.innerHTML = '<option value="">Seleccionar...</option>';
    data.forEach(item => {
        const opt = document.createElement('option');
        opt.value = item[keyId];
        opt.textContent = item.Nombre;
        if (selectedId && opt.value == selectedId) opt.selected = true;
        select.appendChild(opt);
    });
}

function AbrirEditar(id) {
    $.get('/Partido/RecuperarPartido/' + id, function (data) {
        const p = data[0];

        Promise.all([
            $.get('/Partido/CargarTorneos'),
            $.get('/Partido/CargarTorneoEquipos')
        ]).then(([torneos, equipos]) => {
            llenarCombo('txtTorneoID', torneos, p.TorneoID, 'TorneoID');
            llenarCombo('txtLocalID', equipos, p.EquipoLocalTorneoEquipoID, 'TorneoEquipoID');
            llenarCombo('txtVisitanteID', equipos, p.EquipoVisitanteTorneoEquipoID, 'TorneoEquipoID');

            document.getElementById('txtPartidoID').value = p.PartidoID;
            document.getElementById('txtGolesLocal').value = p.GolesLocal ?? '';
            document.getElementById('txtGolesVisitante').value = p.GolesVisitante ?? '';
            document.getElementById('txtFechaNro').value = p.NroFecha ?? '';
            document.getElementById('txtAnio').value = p.AñoParticipacion ?? '';
            document.getElementById('txtFase').value = p.Fase || '';
            document.getElementById('txtGrupo').value = p.Grupo || '';
            document.getElementById('txtEstado').value = p.Estado || '';

            document.getElementById('modal-title').textContent = 'Editar Partido';
            document.getElementById('modalPartido').classList.remove('hidden');

            setTimeout(() => {
                document.getElementById('modalPartidoContent').classList.remove('scale-95', 'opacity-0');
                document.getElementById('modalPartidoContent').classList.add('scale-100', 'opacity-100');
            }, 10);
        });
    });
}

function Limpiar() {
    document.getElementById('formPartido').reset();
    document.getElementById('txtPartidoID').value = '';
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalPartido');
    const modalContent = document.getElementById('modalPartidoContent');

    if (event.target === modal && !modalContent.contains(event.target)) {
        CerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalPartido');
        if (!modal.classList.contains('hidden')) {
            CerrarModal();
        }
    }
});

