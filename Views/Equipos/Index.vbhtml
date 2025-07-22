@Code
    ViewData("Title") = "Equipos"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Equipos</h2>
    <button onclick="abrirModalNuevo()" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
        <i data-lucide="plus-circle" class="w-5 h-5"></i> Crear Equipo
    </button>
</div>

<div class="card p-4 rounded-lg overflow-x-auto">
    <table id="tablaEquipos" class="w-full text-left">
        <thead class="table-header">
            <tr>
                <th class="p-4 font-semibold">Nombre</th>
                <th class="p-4 font-semibold">Código</th>
                <th class="p-4 font-semibold">Región</th>
                <th class="p-4 font-semibold">Ciudad</th>
                <th class="p-4 font-semibold text-center">Año Fundación</th>
                <th class="p-4 font-semibold text-center">ELO</th>
                <th class="p-4 font-semibold">Tipo</th>
                <th class="p-4 font-semibold">Estado</th>
                <th class="p-4 font-semibold text-center">Acciones</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
</div>

<div id="modalEquipo" class="fixed inset-0 bg-black bg-opacity-50 hidden items-center justify-center z-50 p-4">
    <div id="modalEquipoContent" class="bg-gray-800 w-full max-w-md rounded-lg shadow-lg p-8 transform transition-all scale-95 opacity-0">
        <h2 class="text-2xl font-bold mb-6 text-white" id="tituloModal">Crear Equipo</h2>
        <form id="formEquipo">
            <input type="hidden" id="txtEquipoID" />

            <div class="mb-4">
                <label for="txtNombre" class="block text-sm font-medium text-gray-300 mb-2">Nombre</label>
                <input type="text" id="txtNombre" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" required />
            </div>

            <div class="mb-4">
                <label for="txtCodigoEquipo" class="block text-sm font-medium text-gray-300 mb-2">Código Equipo</label>
                <input type="text" id="txtCodigoEquipo" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" required />
            </div>

            <div class="mb-4">
                <label for="selectRegion" class="block text-sm font-medium text-gray-300 mb-2">Región</label>
                <select id="selectRegion" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                    <option value="">-- Seleccione Región --</option>
                </select>
            </div>

            <div class="mb-4">
                <label for="selectCiudad" class="block text-sm font-medium text-gray-300 mb-2">Ciudad</label>
                <select id="selectCiudad" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                    <option value="">-- Seleccione Ciudad --</option>
                </select>
            </div>

            <div class="mb-4">
                <label for="txtAnoFundacion" class="block text-sm font-medium text-gray-300 mb-2">Año de Fundación</label>
                <input type="number" id="txtAnoFundacion" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" />
            </div>

            <div class="mb-4">
                <label for="txtELO" class="block text-sm font-medium text-gray-300 mb-2">ELO</label>
                <input type="number" step="0.01" id="txtELO" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" />
            </div>

            <div class="mb-4">
                <label for="selectTipoEquipo" class="block text-sm font-medium text-gray-300 mb-2">Tipo de Equipo</label>
                <select id="selectTipoEquipo" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500" required>
                    <option value="">-- Seleccione Tipo --</option>
                    <option value="Club">Club</option>
                    <option value="Seleccion Nacional">Selección Nacional</option>
                    <option value="Seleccion Regional">Selección Regional</option>
                    <option value="Universitario">Universitario</option>
                    <option value="Amateur">Amateur</option>
                    <option value="Otro">Otro</option>
                </select>
            </div>

            <div class="mb-6">
                <label for="selectEstado" class="block text-sm font-medium text-gray-300 mb-2">Estado</label>
                <select id="selectEstado" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500" required>
                    <option value="Activo">Activo</option>
                    <option value="Desaparecido">Desaparecido</option>
                    <option value="Desafiliado">Desafiliado</option>
                    <option value="Inactivo">Inactivo</option>
                    <option value="Suspendido">Suspendido</option>
                    <option value="Fusionado">Fusionado</option>
                </select>
            </div>

            <div class="flex justify-end gap-4">
                <button type="button" onclick="cerrarModal()" class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">Cancelar</button>
                <button type="submit" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">Guardar</button>
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
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/js/equipo.js"></script>
End Section
