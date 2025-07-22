@Code
    Dim currentController = ViewContext.RouteData.Values("controller").ToString().ToLower()
    Dim currentAction = ViewContext.RouteData.Values("action").ToString().ToLower()
End Code


<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <title>@ViewBag.Title - SGTF</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <script src="https://cdn.tailwindcss.com"></script>
    <script src="https://unpkg.com/lucide@latest"></script>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

    <style>
        body {
            font-family: 'Inter', sans-serif;
            background-color: #111827; /* bg-gray-900 */
        }

        .sidebar {
            background-color: #1f2937; /* bg-gray-800 */
        }

        .main-content {
            background-color: #111827; /* bg-gray-900 */
        }

        .card {
            background-color: #1f2937; /* bg-gray-800 */
        }

        .table-header {
            background-color: #374151; /* bg-gray-700 */
        }

        .table-row-even {
            background-color: #1f2937; /* bg-gray-800 */
        }

        .table-row-odd {
            background-color: #263142;
        }

        .modal-backdrop {
            background-color: rgba(0, 0, 0, 0.7);
        }

        .modal-content {
            background-color: #1f2937; /* bg-gray-800 */
        }

        ::-webkit-scrollbar {
            width: 8px;
        }

        ::-webkit-scrollbar-track {
            background: #1f2937;
        }

        ::-webkit-scrollbar-thumb {
            background: #4b5563;
            border-radius: 4px;
        }

            ::-webkit-scrollbar-thumb:hover {
                background: #6b7280;
            }

        .nav-link.active {
            background-color: rgba(16, 185, 129, 0.1); /* bg-green-500/10 */
            color: #34d399; /* text-green-400 */
            border-left: 4px solid #10b981; /* border-green-500 */
        }

        .nav-link:not(.active):hover {
            background-color: #374151; /* bg-gray-700 */
        }
    </style>

    @RenderSection("Head", required:=False)
</head>
<body class="text-gray-200">
    <div class="flex h-screen">
        <aside class="sidebar w-64 p-6 hidden lg:flex flex-col justify-between">
            <div>
                <div class="flex items-center gap-3 mb-10">
                    <div class="bg-green-600 p-2 rounded-lg">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" data-lucide="shield-check" class="lucide lucide-shield-check text-white"><path d="M20 13c0 5-3.5 7.5-7.66 8.95a1 1 0 0 1-.67-.01C7.5 20.5 4 18 4 13V6a1 1 0 0 1 1-1c2 0 4.5-1.2 6.24-2.72a1.17 1.17 0 0 1 1.52 0C14.51 3.81 17 5 19 5a1 1 0 0 1 1 1z"></path><path d="m9 12 2 2 4-4"></path></svg>
                    </div>
                    <h1 class="text-xl font-bold text-white">SGTF</h1>
                </div>
                <h2 class="text-xs font-semibold text-gray-400 uppercase tracking-wider mb-3">Menú Principal</h2>
                <nav class="flex flex-col gap-2">
                    <a href="@Url.Action("Index", "Home")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "home", "active", ""))"
                       data-section="dashboard">
                        <i data-lucide="layout-dashboard"></i><span>Dashboard</span>
                    </a>

                    <a href="@Url.Action("Index", "Torneo")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "torneo", "active", ""))">
                        <i data-lucide="trophy"></i><span>Torneos</span>
                    </a>

                    <a href="@Url.Action("Index", "Equipos")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "equipos", "active", ""))">
                        <i data-lucide="users"></i><span>Equipos</span>
                    </a>

                    <a href="@Url.Action("Index", "Partido")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "partido", "active", ""))">
                        <i data-lucide="calendar-days"></i><span>Partidos</span>
                    </a>

                    <a href="@Url.Action("Index", "Confederaciones")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "confederaciones", "active", ""))">
                        <i data-lucide="globe-2"></i><span>Confederaciones</span>
                    </a>

                    <a href="@Url.Action("Index", "Paises")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "paises", "active", ""))">
                        <i data-lucide="map-pin"></i><span>Países</span>
                    </a>

                    <a href="@Url.Action("Index", "Region")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "region", "active", ""))">
                        <i data-lucide="map"></i><span>Regiones</span>
                    </a>

                    <a href="@Url.Action("Index", "Ciudad")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "ciudad", "active", ""))">
                        <i data-lucide="building"></i><span>Ciudades</span>
                    </a>

                    <a href="@Url.Action("Index", "Palmares")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "palmares", "active", ""))">
                        <i data-lucide="award"></i><span>Palmarés</span>
                    </a>

                    <a href="@Url.Action("Index", "Estadisticas")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "estadisticas", "active", ""))">
                        <i data-lucide="bar-chart-3"></i><span>Estadísticas</span>
                    </a>

                    <div class="border-t border-gray-700 my-2"></div>

                    <a href="@Url.Action("Index", "Configuracion")"
                       class="nav-link flex items-center gap-3 px-4 py-2 rounded-lg transition-colors @(If(currentController = "configuracion", "active", ""))">
                        <i data-lucide="settings"></i><span>Configuración</span>
                    </a>
                </nav>
            </div>
            <div class="text-center text-gray-500 text-sm">
                <p>&copy; 2025 - SGTF</p>
            </div>
        </aside>

        <main class="flex-1 p-4 md:p-8 overflow-y-auto main-content">
            @RenderBody()
        </main>
    </div>

    <footer class="text-center mt-10 text-sm text-gray-500 hidden lg:block">
        <p>&copy; 2025 SGTF</p>
    </footer>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            lucide.createIcons();
        });
    </script>

    @RenderSection("Scripts", required:=False)
</body>
</html>