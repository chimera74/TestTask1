var dataTable;

$(document).ready(function () {
   loadData(); 
});

function loadData() {
    dataTable = $('#jstable').DataTable({
        "ajax": {
            "url": "/Api",
            "type": "GET",
            "datatype": "json",
            "dataSrc": (d) => {
                return d;
            }
        },
        "columns": [
            {"data": "id", "width": "30%" },
            {"data": "code", "width": "30%" },
            {"data": "value", "width": "30%" }
        ],
        "language": {
            "emptyTable": "No data"
        },
        "width": "100%"
    })
}