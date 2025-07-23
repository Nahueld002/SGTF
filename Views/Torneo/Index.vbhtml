@Code
    ViewBag.Title = "Torneos"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Torneos</h2>
    <button onclick="AbrirModal()" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
        <i data-lucide="plus-circle" class="w-5 h-5"></i> Crear Torneo
    </button>
</div>

<div class="card p-4 mb-6 rounded-lg">
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <input id="filter-name" type="text" placeholder="Filtrar por nombre..." class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400">
        <select id="filter-type" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white">
            <option value="">Todos los Tipos</option>
            <option value="Liga">Liga</option>
            <option value="Copa">Copa</option>
            <option value="Eliminatoria">Eliminatoria</option>
        </select>
        <select id="filter-category" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white">
            <option value="">Todas las Categorías</option>
            <option value="Amateur">Amateur</option>
            <option value="Copa Nacional">Copa Nacional</option>
            <option value="Cuarta División">Cuarta División</option>
            <option value="Primera División">Primera División</option>
            <option value="Segunda División">Segunda División</option>
            <option value="Super Copa Nacional">Super Copa Nacional</option>
            <option value="Tercera División">Tercera División</option>
            <option value="Torneo Preparación">Torneo Preparación</option>
        </select>
        <select id="filter-status" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white">
            <option value="">Todos los Estados</option>
            <option value="Activo">Activo</option>
            <option value="Extinto">Extinto</option>
            <option value="Suspendido">Suspendido</option>
        </select>
    </div>
</div>

<div id="DIVTorneo" class="overflow-x-auto card rounded-lg">
    <table class="min-w-full text-sm text-left text-gray-400">
        <thead class="text-xs uppercase bg-gray-700 text-gray-400">
            <tr>
                <th scope="col" class="px-6 py-3">Nombre</th>
                <th scope="col" class="px-6 py-3">Tipo de Torneo</th>
                <th scope="col" class="px-6 py-3">Categoría</th>
                <th scope="col" class="px-6 py-3">Estado</th>
                <th scope="col" class="px-6 py-3">Confederación</th>
                <th scope="col" class="px-6 py-3">País</th>
                <th scope="col" class="px-6 py-3">Región</th>
                <th scope="col" class="px-6 py-3">Ciudad</th>
                <th scope="col" class="px-6 py-3">Acciones</th>
            </tr>
        </thead>
        <tbody id="tblTorneosBody">
        </tbody>
    </table>
</div>

<div id="modalTorneo" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center p-4 hidden z-50">
    <div id="modalTorneoContent" class="bg-gray-800 w-full max-w-2xl rounded-lg shadow-lg p-8 transform transition-all scale-95 opacity-0">
        <h3 id="modal-title" class="text-2xl font-bold mb-6 text-white">Nuevo Torneo</h3>
        <form id="formTorneo" onsubmit="Guardar(); return false;">
            <input type="hidden" id="txtTorneoID">

            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Nombre</label>
                    <input type="text" id="txtNombre" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-500" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Tipo de Torneo</label>
                    <select id="txtTipoTorneo" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500" required>
                        <option value="">Seleccionar...</option>
                        <option value="Liga">Liga</option>
                        <option value="Copa">Copa</option>
                        <option value="Eliminatoria">Eliminatoria</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Categoría</label>
                    <select id="txtCategoria" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500" required>
                        <option value="">Seleccionar...</option>
                        <option value="Amateur">Amateur</option>
                        <option value="Copa Nacional">Copa Nacional</option>
                        <option value="Cuarta División">Cuarta División</option>
                        <option value="Primera División">Primera División</option>
                        <option value="Segunda División">Segunda División</option>
                        <option value="Super Copa Nacional">Super Copa Nacional</option>
                        <option value="Tercera División">Tercera División</option>
                        <option value="Torneo Preparación">Torneo Preparación</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Estado</label>
                    <select id="txtEstado" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500" required>
                        <option value="">Seleccionar...</option>
                        <option value="Activo">Activo</option>
                        <option value="Extinto">Extinto</option>
                        <option value="Suspendido">Suspendido</option>
                    </select>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Ciudad</label>
                    <select id="txtCiudadID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                        <option value="">Seleccionar ciudad...</option>
                    </select>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">País</label>
                    <select id="txtPaisID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                        <option value="">Seleccionar país...</option>
                    </select>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Región</label>
                    <select id="txtRegionID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                        <option value="">Seleccionar región...</option>
                    </select>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-300 mb-2">Confederación</label>
                    <select id="txtConfederacionID" class="w-full bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-green-500">
                        <option value="">Seleccionar confederación...</option>
                    </select>
                </div>
            </div>
            <div class="flex justify-end gap-4 mt-8">
                <button type="button" class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg transition-colors" onclick="CerrarModal()">Cancelar</button>
                <button type="submit" class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">Guardar</button>
            </div>
        </form>
    </div>
</div>

<style>
    /* Estilos para autocomplete */
    .autocomplete-item {
        border-bottom: 1px solid rgba(75, 85, 99, 0.5);
        transition: background-color 0.2s ease;
    }

        .autocomplete-item:last-child {
            border-bottom: none;
        }

        .autocomplete-item:hover {
            background-color: rgba(75, 85, 99, 0.7);
        }

    /* Estilos para el modal */
    .modal-backdrop {
        backdrop-filter: blur(4px);
    }

    .modal-content {
        max-height: 90vh;
        overflow-y: auto;
        box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
    }

    .modal-content {
        transition: transform 0.3s ease-out, opacity 0.3s ease-out;
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
</style>

@section Scripts
    <script src="~/scripts/jquery-3.7.1.min.js"></script>
    <script src="~/scripts/js/Torneo.js"></script>
    <script>
        // These global functions were likely meant for the HTML,
        // but it's better to keep the main Listar/filter logic in Torneo.js.
        // If AbrirModal or CerrarModal are *only* called from HTML attributes,
        // they need to be globally accessible, so we make them window properties.

        window.AbrirModal = function (torneoID = 0) {
            // Reiniciar formulario y título para nuevo torneo
            Limpiar(); // Llamar a la función Limpiar para resetear el formulario
            cargarComboBoxesJerarquicos(); // Recargar los comboboxes jerárquicos

            if (torneoID > 0) {
                // Si está editando, poblar el formulario con datos existentes
                Recuperar(torneoID); // Esta función debería poblar los campos del formulario
                $('#modal-title').text('Editar Torneo');
            } else {
                $('#modal-title').text('Nuevo Torneo');
            }

            $('#modalTorneo').removeClass('hidden');
            $('#modalTorneoContent').removeClass('opacity-0 scale-95').addClass('opacity-100 scale-100');
        };

        window.CerrarModal = function () {
            $('#modalTorneoContent').removeClass('opacity-100 scale-100').addClass('opacity-0 scale-95');
            setTimeout(() => {
                $('#modalTorneo').addClass('hidden');
            }, 300); // Permite tiempo para la transición
        };

        // Función para redirigir a la tabla de posiciones
        window.VerTablaPosiciones = function (torneoID, nombre) {
            const encodedNombre = encodeURIComponent(nombre);
            window.location.href = `/Torneo/TablaPosiciones?torneoID=${torneoID}&torneoNombre=${encodedNombre}`;
        };
    </script>
End Section