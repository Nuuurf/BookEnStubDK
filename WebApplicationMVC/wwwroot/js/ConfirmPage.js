// Function to add the reserved appointments from localstorage to the list
$(document).ready(function () {
    var bookedAppointments = JSON.parse(localStorage.getItem('selectedAppointments'));

    if (bookedAppointments && bookedAppointments.length > 0) {
        bookedAppointments.forEach(function (appointment) {
            var formattedDateTime = appointment.date + ' - ' + appointment.time;
            var listItem = $('<li>', {
                class: 'list-group-item',
                text: formattedDateTime
            });
            $('#booked-times').append(listItem);
        });
    } else {
        $('#booked-times').append($('<li>', {
            class: 'list-group-item',
            text: 'Ingen reserveret tider fundet'
        }));
    }
});