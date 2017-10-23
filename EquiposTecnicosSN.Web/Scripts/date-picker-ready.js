$(function () {
    // This will make every element with the class "date-picker" into a DatePicker element
    $('.date-picker').datetimepicker({
        locale: 'es',
        format: 'D/M/YYYY hh:mm a',
        keepOpen: false,
        useCurrent: false
    });

    //Fue anulado para realizar la busqueda de las OT ya que  no me dejaba buscar si no ponia la fecha -- jgutierrez 20/10/2017

    //(function () {
    //    // overrides the jquery date validator method    
    //    jQuery.validator.methods.date = function (value, element) {
    //        // We want to validate date and datetime
    //        var formats = [" ", "D/M/YYYY", "D/M/YYYY hh:mm a"];
    //        // Validate the date and return
    //        return moment(value, formats, true).isValid();
    //    };
    //})(jQuery, moment);
})