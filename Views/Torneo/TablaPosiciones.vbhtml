@Code
    ViewBag.Title = "Tabla de Posiciones"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Tabla de Posiciones: <span id="standingsTorneoName" class="text-green-400"></span></h2>
    <a href="@Url.Action("Index", "Torneo")" class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
        <i data-lucide="arrow-left" class="w-5 h-5"></i> Volver a Torneos
    </a>
</div>

<div class="card p-4 mb-6 rounded-lg">
    <div class="mb-4">
        <label for="standingsYear" class="block text-sm font-medium text-gray-300 mb-2">Año:</label>
        <select id="standingsYear" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white w-full md:w-auto">
            <option value="2024">2024</option>
            <option value="2025" selected>2025</option>
            <option value="2026">2026</option>
            <option value="2027">2027</option>
            <option value="2028">2028</option>
            <option value="2029">2029</option>
            <option value="2030">2030</option>
        </select>
    </div>

    <div id="standingsTableContainer" class="overflow-x-auto rounded-lg">
        <table class="min-w-full text-sm text-left text-gray-400">
            <thead class="text-xs uppercase bg-gray-700 text-gray-400">
                <tr>
                    <th scope="col" class="px-6 py-3">Equipo</th>
                    <th scope="col" class="px-6 py-3">PJ</th>
                    <th scope="col" class="px-6 py-3">PG</th>
                    <th scope="col" class="px-6 py-3">PE</th>
                    <th scope="col" class="px-6 py-3">PP</th>
                    <th scope="col" class="px-6 py-3">GF</th>
                    <th scope="col" class="px-6 py-3">GC</th>
                    <th scope="col" class="px-6 py-3">DG</th>
                    <th scope="col" class="px-6 py-3">PTS</th>
                </tr>
            </thead>
            <tbody id="tblStandingsBody">
            </tbody>
        </table>
    </div>
    <p id="noStandingsMessage" class="text-gray-400 text-center mt-4 hidden">No hay partidos finalizados para mostrar la tabla de posiciones en el año seleccionado.</p>
</div>

<style>
    /* Add any specific styles for this page here if needed, or rely on shared _Layout.vbhtml styles */
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
    <script src="/scripts/jquery-3.7.1.min.js"></script>
    <script>
        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const torneoID = urlParams.get('torneoID');
            const torneoNombre = urlParams.get('torneoNombre');

            if (torneoID && torneoNombre) {
                $('#standingsTorneoName').text(decodeURIComponent(torneoNombre));
                loadStandings(torneoID, $('#standingsYear').val());
            } else {
                $('#standingsTorneoName').text('No se encontró el torneo.');
                $('#standingsTableContainer').addClass('hidden');
                $('#noStandingsMessage').removeClass('hidden').text('No se encontró un ID de torneo válido.');
            }

            $('#standingsYear').change(function () {
                if (torneoID) {
                    loadStandings(torneoID, $(this).val());
                }
            });
        });

        function loadStandings(torneoID, year) {
            $.ajax({
                url: '@Url.Action("RecuperarTablaPosiciones", "Torneo")',
                type: 'GET',
                data: { torneoID: torneoID, año: year },
                dataType: 'json',
                success: function (data) {
                    var tbody = $('#tblStandingsBody');
                    tbody.empty();
                    if (data && data.length > 0) {
                        $('#standingsTableContainer').removeClass('hidden');
                        $('#noStandingsMessage').addClass('hidden');
                        $.each(data, function (i, item) {
                            var row = `
                                <tr class="${i % 2 === 0 ? 'table-row-even' : 'table-row-odd'} border-b border-gray-700">
                                    <td class="px-6 py-4 font-medium text-white">${item.Equipo}</td>
                                    <td class="px-6 py-4">${item.PJ}</td>
                                    <td class="px-6 py-4">${item.PG}</td>
                                    <td class="px-6 py-4">${item.PE}</td>
                                    <td class="px-6 py-4">${item.PP}</td>
                                    <td class="px-6 py-4">${item.GF}</td>
                                    <td class="px-6 py-4">${item.GC}</td>
                                    <td class="px-6 py-4">${item.DG}</td>
                                    <td class="px-6 py-4 font-bold text-green-400">${item.PTS}</td>
                                </tr>
                            `;
                            tbody.append(row);
                        });
                    } else {
                        $('#standingsTableContainer').addClass('hidden');
                        $('#noStandingsMessage').removeClass('hidden').text('No hay partidos finalizados para mostrar la tabla de posiciones en el año seleccionado.');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al cargar tabla de posiciones:", error);
                    alert("Error al cargar la tabla de posiciones.");
                    $('#standingsTableContainer').addClass('hidden');
                    $('#noStandingsMessage').removeClass('hidden').text('Error al cargar la tabla de posiciones.');
                }
            });
        }
    </script>
    End Section