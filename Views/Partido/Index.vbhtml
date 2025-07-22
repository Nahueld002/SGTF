@Code
    ViewBag.Title = "Partidos"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Partidos</h2>
    <button onclick="AbrirModal()" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
        <i data-lucide="plus-circle" class="w-5 h-5"></i> Crear Partido
    </button>
</div>

<!-- Filtros -->
<div class="card p-4 mb-6 rounded-lg">
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <input id="filter-torneo" type="text" placeholder="Filtrar por torneo..." class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400">
        <select id="filter-fase" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white">
            <option value="">Todas las Fases</option>
            <option value="Primera Ronda">Primera Ronda</option>
            <option value="Fase 1">Fase 1</option>
            <option value="Segunda Ronda">Segunda Ronda</option>
        </select>
        <select id="filter-estado" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white">
            <option value="">Todos los Estados</option>
            <option value="Pendiente">Pendiente</option>
            <option value="Jugado">Jugado</option>
            <option value="Suspendido">Suspendido</option>
        </select>
    </div>
</div>

<!-- Tabla dinámica -->
<div id="DIVPartido" class="overflow-x-auto card rounded-lg"></div>

<!-- Modal Partido -->
<div id="modalPartido" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center p-4 hidden z-50">
    <div id="modalPartidoContent" class="bg-gray-800 w-full max-w-2xl rounded-lg shadow-lg p-8 transform transition-all scale-95 opacity-0">
        <h3 id="modal-title" class="text-2xl font-bold mb-6 text-white">Nuevo Partido</h3>
        <form id="formPartido" onsubmit="Guardar(); return false;">
            <input type="hidden" id="txtPartidoID">

            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Torneo</label>
                    <select id="txtTorneoID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required></select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Equipo Local</label>
                    <select id="txtLocalID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required></select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Equipo Visitante</label>
                    <select id="txtVisitanteID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required></select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Goles Local</label>
                    <input type="number" id="txtGolesLocal" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" min="0" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Goles Visitante</label>
                    <input type="number" id="txtGolesVisitante" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" min="0" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Nro Fecha</label>
                    <input type="number" id="txtFechaNro" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" min="1" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Año Participación</label>
                    <input type="number" id="txtAnio" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Fase</label>
                    <select id="txtFase" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required>
                        <option value="">Seleccionar fase...</option>
                        <option value="Primera Ronda">Primera Ronda</option>
                        <option value="Fase 1">Fase 1</option>
                        <option value="Segunda Ronda">Segunda Ronda</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Grupo</label>
                    <select id="txtGrupo" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required>
                        <option value="">Seleccionar fase...</option>
                        <option value="A">A</option>
                        <option value="B">B</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Estado</label>
                    <select id="txtEstado" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white" required>
                        <option value="">Seleccionar estado...</option>
                        <option value="Pendiente">Pendiente</option>
                        <option value="Jugado">Jugado</option>
                        <option value="Suspendido">Suspendido</option>
                    </select>
                </div>
            </div>

            <div class="flex justify-end gap-4 mt-8">
                <button type="button" onclick="CerrarModal()" class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg">Cancelar</button>
                <button type="submit" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg">Guardar</button>
            </div>
        </form>
    </div>
</div>

<style>
    .table-row-even {
        background-color: rgba(55, 65, 81, 0.5);
    }

    .table-row-odd {
        background-color: rgba(75, 85, 99, 0.5);
    }

    .table-header {
        background-color: rgba(31, 41, 55, 0.8);
        border-bottom: 2px solid rgba(107, 114, 128, 0.5);
    }

    .card {
        background-color: rgba(31, 41, 55, 0.8);
        border: 1px solid rgba(107, 114, 128, 0.3);
    }

    .scale-95 {
        transform: scale(0.95);
    }

    .scale-100 {
        transform: scale(1);
    }

    .opacity-0 {
        opacity: 0;
    }

    .opacity-100 {
        opacity: 1;
    }
</style>

@section Scripts
    <script src="/scripts/jquery-3.7.1.min.js"></script>
    <script src="/scripts/js/partido.js"></script>
    <script>lucide.createIcons();</script>
End Section
