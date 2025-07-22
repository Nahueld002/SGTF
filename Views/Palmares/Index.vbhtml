@Code
    ViewData("Title") = "Palmares de Equipos"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Palmarés de Equipos</h2>
</div>

<div class="card p-4 rounded-lg shadow-md overflow-x-auto">
    <h3 class="text-xl font-semibold text-white mb-4">Total de Títulos por Equipo</h3>
    <table id="tablaTotalTitulos" class="w-full text-left">
        <thead class="table-header">
            <tr>
                <th class="p-4 font-semibold">Equipo</th>
                <th class="p-4 font-semibold text-center">Total de Títulos</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
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
</style>

@section Scripts
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/js/palmares.js"></script>
End Section