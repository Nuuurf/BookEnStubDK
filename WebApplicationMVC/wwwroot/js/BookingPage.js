// Function to format the date and time
function formatTime(date) {
    var utcDate = date + 'Z';
    var dateTime = new Date(utcDate);

    var formattedTime = dateTime.toLocaleTimeString('da-DK', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
        timeZone: 'Europe/Copenhagen'
    });
    return formattedTime;
}


// Function to format the date only
function formatDate(dateTimeString) {
    var dateTime = new Date(dateTimeString);

    var year = dateTime.getFullYear();
    var month = ('0' + (dateTime.getMonth() + 1)).slice(-2);
    var day = ('0' + dateTime.getDate()).slice(-2);

    return year + '/' + month + '/' + day;
}

// Event delegation for book-btn click events
$(document).on('click', '.book-btn', function () {
    var selectedTime = $(this).data('time');
    var hiddenDate = $(this).closest('.list-group-item').find('.hidden-date').text();
    var fullDateTimeValue = $(this).closest('.list-group-item').data('datetime');
    // Add the selected appointment to the "Selected Appointments" list
    $('#selected-appointments').append(
        '<li class="list-group-item" data-datetime="' + fullDateTimeValue + '">' + hiddenDate + ' - ' + selectedTime +
        ' <button class="btn btn-danger btn-sm cancel-btn"><i class="fas fa-times"></i> Fjern</button></li>'
    );

    // Check the number of selected appointments
    var selectedAppointmentsCount = $('#selected-appointments li').length;

    // Enable or disable the confirm booking button based on the count
    if (selectedAppointmentsCount > 0) {
        $('.confirm-booking-btn').prop('disabled', false);
    } else {
        $('.confirm-booking-btn').prop('disabled', true);
    }
});

// Event handler for cancel buttons
$('#selected-appointments').on('click', '.cancel-btn', function () {
    // Remove the canceled appointment from the list
    $(this).closest('li').remove();

    // Check the number of selected appointments after removal
    var selectedAppointmentsCount = $('#selected-appointments li').length;

    // Enable or disable the confirm booking button based on the count
    if (selectedAppointmentsCount > 0) {
        $('.confirm-booking-btn').prop('disabled', false);
    } else {
        $('.confirm-booking-btn').prop('disabled', true);
    }
});

// Loop through API response and add times to list for display
function processApiResponse(apiResponse) {
    $('#available-times').empty();

    if (apiResponse.error !== undefined) {
        var errorItem = $('<li>', {
            class: 'list-group-item d-flex justify-content-between align-items-center',
            html: '<div class="">' + "Ugyldig dato eller dato er før i dag" + '</div>'
        });
        $('#available-times').append(errorItem);
        return;
    }
    // Iterate through the apiResponse array and append each element to the ul as li items
    $.each(apiResponse, function (index, item) {
        var startTime = formatTime(item.timeStart);
        var endTime = formatTime(item.timeEnd);
        var date = formatDate(item.timeStart);
        var availableStubs = item.availableStubIds.length;
        var todaysDate = new Date();
        var [hours, minutes] = startTime.split('.');
        var [year, month, day] = date.split('/');
        var specificDate = new Date(year, month - 1, day);
        specificDate.setHours(parseInt(hours), parseInt(minutes), 0, 0);

        var buttonClass = 'btn btn-primary btn-sm book-btn';
        var buttonText = 'Book';

        // Check if availableStubs is 0, then disable the button and change its color to danger
        if (availableStubs === 0 || specificDate < todaysDate) {
            buttonClass = 'btn btn-danger btn-sm book-btn disabled';
            buttonText = 'Booket';
        }

        var listItem = $('<li>', {
            class: 'list-group-item d-flex justify-content-between align-items-center',
            html: '<div class="hidden-date" style="display:none;">' + date + '</div>' +
                startTime + ' - ' + endTime +
                '<div class="">' +
                '<small class="text-muted">' + availableStubs + ' tider ledige</small>' +
                '</div>' +
                '<button class="' + buttonClass + '" data-time="' + startTime + ' - ' + endTime + '">' +
                '<i class="fas fa-plus"></i> ' + buttonText +
                '</button>',
            'data-datetime': item.timeStart
        });

        $('#available-times').append(listItem);
    });
}

// Saves reserved timeslots to localstorage
function saveAppointmentsToLocalStorage() {
    var appointments = [];

    // Loop through each selected appointment in the list
    $('#selected-appointments li').each(function () {
        var appointmentText = $(this).text().trim();

        // Trim the last 6 characters
        appointmentText = appointmentText.slice(0, -6);

        // Find the index of the first hyphen ("-")
        var firstHyphenIndex = appointmentText.indexOf('-');

        // Extract date and time based on the first hyphen index
        var hiddenDate = appointmentText.substr(0, firstHyphenIndex).trim();
        var selectedTime = appointmentText.substr(firstHyphenIndex + 1).trim();

        var dateTime = $(this).data('datetime');
        // Create an object for each appointment
        var appointment = {
            date: hiddenDate,
            time: selectedTime,
            datetime: dateTime
        };

        // Push the appointment object to the array
        appointments.push(appointment);
    });

    // Convert the array to a JSON string and save it to localStorage
    localStorage.setItem('selectedAppointments', JSON.stringify(appointments));
}

// Initialize datepicker
$('.datepicker').datepicker({
    "format": 'dd/mm/yyyy',
    "todayHighlight": true,
    "weekStart": 1,
    "autoclose": true,
    "calendarWeeks": true
}).datepicker("setDate", 'now');