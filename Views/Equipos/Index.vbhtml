@Code
    ViewData("Title") = "Equipos"
End Code

<div class="flex justify-between items-center mb-4">
    <h1 class="text-2xl font-bold">Equipos</h1>
    <button onclick="abrirModalNuevo()" class="bg-green-600 hover:bg-green-700 text-white font-semibold px-4 py-2 rounded">
        + Crear Equipo
    </button>
</div>

<div class="bg-white rounded shadow p-4">
    <table id="tablaEquipos" class="min-w-full text-sm">
        <thead>
            <tr>
                <th>Nombre</th>
                <th>Código</th>
                <th>Tipo</th>
                <th>Año</th>
                <th>ELO</th>
                <th>Estado</th>
                <th>Ciudad</th>
                <th>Región</th>
                <th>Acciones</th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
</div>

<!-- Modal -->
<div id="modalEquipo" class="fixed inset-0 bg-black bg-opacity-50 hidden items-center justify-center z-50">
    <div class="bg-white p-6 rounded-lg shadow-lg w-full max-w-2xl">
        <h2 class="text-xl font-bold mb-4" id="tituloModal">Crear Equipo</h2>

        <form id="formEquipo">
            <input type="hidden" id="txtEquipoID" />

            <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                    <label>Nombre</label>
                    <input type="text" id="txtNombre" class="w-full border rounded p-2" required />
                </div>
                <div>
                    <label>Código</label>
                    <input type="text" id="txtCodigo" class="w-full border rounded p-2" required />
                </div>
                <div>
                    <label>Tipo</label>
                    <input type="text" id="txtTipo" class="w-full border rounded p-2" />
                </div>
                <div>
                    <label>Año Fundación</label>
                    <input type="number" id="txtAnho" class="w-full border rounded p-2" />
                </div>
                <div>
                    <label>ELO</label>
                    <input type="number" id="txtELO" step="0.01" class="w-full border rounded p-2" />
                </div>
                <div>
                    <label>Estado</label>
                    <select id="txtEstado" class="w-full border rounded p-2">
                        <option>Activo</option>
                        <option>Desaparecido</option>
                        <option>Desafiliado</option>
                        <option>Inactivo</option>
                        <option>Suspendido</option>
                        <option>Fusionado</option>
                    </select>
                </div>
            </div>

            <h3 class="text-lg font-semibold mt-4 mb-2">Ubicación</h3>
            <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                    <label>Confederación</label>
                    <select id="txtConfederacionID" class="w-full border rounded p-2"></select>
                </div>
                <div>
                    <label>País</label>
                    <select id="txtPaisID" class="w-full border rounded p-2"></select>
                </div>
                <div>
                    <label>Región</label>
                    <select id="txtRegionID" class="w-full border rounded p-2"></select>
                </div>
                <div>
                    <label>Ciudad</label>
                    <select id="txtCiudadID" class="w-full border rounded p-2"></select>
                </div>
            </div>

            <div class="flex justify-end gap-2 mt-6">
                <button type="button" onclick="cerrarModal()" class="px-4 py-2 rounded bg-gray-300 hover:bg-gray-400">Cancelar</button>
                <button type="submit" class="px-4 py-2 rounded bg-blue-600 hover:bg-blue-700 text-white">Guardar</button>
            </div>
        </form>
    </div>
</div>

@section Scripts
    <script src="/scripts/jquery-3.7.1.min.js"></script>
    <script src="/scripts/js/equipo.js"></script>
End Section