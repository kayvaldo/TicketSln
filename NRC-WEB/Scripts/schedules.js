$(function ()
{

    InitializeMultiSelectListBox();
    SetAjaxImageMargins();
    SetSelectedScheduleDays();
    // ShowSelectedTrainTypeInfo();

});


function ShowSelectedTrainTypeInfo()
{
    //Set selected Schedule days
    if ($('#selectedItems')) // I use this to check if page is Edit
    {
        var selected = $('#TrainTypeId').val();
        var code = { trainTypeId: selected };
        $('#ajax').modal('show');

        $.ajax({
            type: 'GET',
            url: '/TrainTypes/TrainTypeTravelClassInfo',
            dataType: 'html',
            data: code,
            success: function (result)
            {
                //hide loading modal
                // $('#ajax').modal('hide');
                $('#trainTypeInfoDiv').html(result);
                $('#ajax').modal('hide');

            },
            error: function (result)
            {
                //$('#loadingImage').hide(); 
                $('#ajax').modal('hide');
                //ShowMessage('Error occured.The selected transaction(s) could NOT be approved.', 'error', 'Error');
            },

        });
    }

}


function GetStations(element)
{
    var cityId = $(element).val();
    var code = { cityId: cityId };
    var stationsElement = document.getElementById($(element).attr('stationsElement'));
    $('#ajax').modal('show');

    $.ajax({
        type: 'GET',
        url: '/Stations/GetStationsByCity',
        dataType: 'html',
        data: code,
        success: function (result)
        {
            //hide loading modal
            // $('#ajax').modal('hide');
            $(stationsElement).html(result);
            $('#ajax').modal('hide');

        },
        error: function (result)
        {
            //$('#loadingImage').hide(); 
            $('#ajax').modal('hide');
            //ShowMessage('Error occured.The selected transaction(s) could NOT be approved.', 'error', 'Error');
        },

    });
}

function SetSelectedScheduleDays()
{
    //Set selected Schedule days
    if ($('#selectedItems'))
    {
        var data = $('#selectedItems').val();

        //Make an array
        var dataarray = data.split(",");

        // Set the value
        $("#DaysOfWeek").val(dataarray);

        // Then refresh
        $("#DaysOfWeek").multiselect("refresh");
    }
}

function InitializeMultiSelectListBox()
{
    ////Initialize multiselect listbox
    $('.listbox').multiselect({
        includeSelectAllOption: true
    });
}

function SetAjaxImageMargins()
{
    //set ajax image margins
    var windowHeight = Number($(window).height());
    var windowWidth = Number($(window).width());
    var imageheight = Number($("#ajax-image").attr("height"));
    var imagewidth = Number($("#ajax-image").attr("width"));
    var newHeight = ((windowHeight - imageheight) / 2);
    var newWidth = ((windowWidth - imagewidth) / 2);
    $("#ajax-image").css("margin-top", newHeight);
    $("#ajax-image").css("margin-left", newWidth);
}

function GetTrainTypeInfo()
{
    var selected = $('#TrainTypeId').val();
    var code = { trainTypeId: selected };
    $('#ajax').modal('show');

    $.ajax({
        type: 'GET',
        url: '/TrainTypes/TrainTypeTravelClassInfo',
        dataType: 'html',
        data: code,
        success: function (result)
        {
            //hide loading modal
            // $('#ajax').modal('hide');
            $('#trainTypeInfoDiv').html(result);
            $('#ajax').modal('hide');

        },
        error: function (result)
        {
            //$('#loadingImage').hide(); 
            $('#ajax').modal('hide');
            //ShowMessage('Error occured.The selected transaction(s) could NOT be approved.', 'error', 'Error');
        },

    });

}
