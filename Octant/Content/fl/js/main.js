/* main.js */

$(document).ready(function () {

    /* Datepickers (only if no native datepicker exists in used browser) */
    if (!Modernizr.inputtypes.date) {
        $('input[type="date"].iso').datepicker({
            format: "yyyy-mm-dd"
        });

        $('input[type="date"]').datepicker({
            format: "dd/mm/yy"
        });
    }

});

