@Code
    ViewBag.Title = "Dashboard"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<main id="main-content-area" class="flex-1 p-4 md:p-8 overflow-y-auto main-content">

    <div id="dashboard-view" class="content-section">
        <div class="flex flex-wrap justify-between items-center gap-4 mb-8">
            <div>
                <h2 class="text-3xl font-bold text-white">Dashboard</h2>
                <p class="text-gray-400">Bienvenido al Sistema de Gestión de Torneos de Fútbol.</p>
            </div>
        </div>

        <!-- Widgets -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            <div class="card p-6 rounded-xl flex items-center gap-6 bg-green-600 text-white shadow-lg">
                <i data-lucide="trophy" class="w-12 h-12 opacity-80"></i>
                <div>
                    <p class="text-lg font-semibold">Torneos Activos</p>
                    <p id="stat-activos" class="text-3xl font-bold">0</p>
                </div>
            </div>
            <div class="card p-6 rounded-xl flex items-center gap-6 bg-gray-800 text-white">
                <i data-lucide="users" class="w-12 h-12 text-green-400"></i>
                <div>
                    <p class="text-lg font-semibold text-gray-400">Equipos Registrados</p>
                    <p id="stat-equipos" class="text-3xl font-bold text-white">0</p>
                </div>
            </div>
            <div class="card p-6 rounded-xl flex items-center gap-6 bg-gray-800 text-white">
                <img src="https://placehold.co/48x48/1f2937/ffffff?text=C" class="w-12 h-12 object-contain" alt="Logo Confederación" />
                <div>
                    <p class="text-lg font-semibold text-gray-400">Confederación</p>
                    <p id="stat-confederacion" class="text-sm font-bold text-white">Cargando...</p>
                </div>
            </div>
        </div>

        <!-- Resultados Recientes -->
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
            <div class="lg:col-span-2 card p-6 rounded-xl text-white">
                <h3 class="text-xl font-semibold mb-4">Resultados Recientes</h3>
                <div class="space-y-3 text-sm">
                    <!-- Se llenan dinámicamente con JS -->
                </div>
            </div>

            <!-- Estadísticas destacadas -->
            <div id="bloqueDestacado" class="card p-6 rounded-xl text-white"></div>

        </div>
    </div>
</main>

@section Scripts
    <script src="/scripts/jquery-3.7.1.min.js"></script>
    <script src="/scripts/js/dashboard.js"></script>
    <script>lucide.createIcons();</script>
End Section
