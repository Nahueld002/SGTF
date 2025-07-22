@Code
    ViewData("Title") = "Ciudades"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Ciudades</h2>
    <button onclick="abrirModalNuevo()" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
        <i data-lucide="plus-circle" class="w-5 h-5"></i> Crear Ciudad
    </button>
</div>

<div class="card p-4 rounded-lg overflow-x-auto">
    <table id="tablaCiudades" class="w-full text-left">
        <thead class="table-header">
            <tr>
                <th class="p-4 font-semibold">Nombre</th>
                <th class="p-4 font-semibold">Región</th>
                <th class="p-4 font-semibold text-center">Es Capital</th>
                <th class="p-4 font-semibold text-center">Acciones</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
</div>

<div id="modalCiudad" class="fixed inset-0 bg-black bg-opacity-50 hidden items-center justify-center z-50 p-4">
    <div id="modalCiudadContent" class="bg-gray-800 w-full max-w-md rounded-lg shadow-lg p-8 transform transition-all scale-95 opacity-0">
        <h2 class="text-2xl font-bold mb-6 text-white" id="tituloModal">Crear Ciudad</h2>
        <form id="formCiudad">
            <input type="hidden" id="txtCiudadID" />

            <div class="mb-6">
                <label for="txtNombre" class="block text-sm font-medium text-gray-300 mb-2">Nombre</label>
                <input type="text" id="txtNombre" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" required />
            </div>

            <div class="mb-6">
                <label for="selectRegion" class="block text-sm font-medium text-gray-300 mb-2">Región</label>
                <select id="selectRegion" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                </select>
            </div>

            <div class="mb-6 flex items-center">
                <input type="checkbox" id="chkEsCapital" class="h-5 w-5 text-green-600 rounded border-gray-600 focus:ring-green-500 mr-2" />
                <label for="chkEsCapital" class="text-sm font-medium text-gray-300">Es Capital</label>
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
    <script src="~/Scripts/js/ciudad.js"></script>
End Section