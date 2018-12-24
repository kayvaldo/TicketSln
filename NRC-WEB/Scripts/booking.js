$(function ()
{
    $('#bookingForm').hide();
    SetAjaxImageMargins();
})

function ShowLoading()
{
    $('#ajax').modal();

}

function EndLoading()
{
    $('#ajax').modal('hide');

}

function ShowBookingForm()
{
    $('#bookingForm').show();

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

function SetTicketMax(count)
{
    var max = Number(count);
    $('#numberOfTickets').prop('max', count);
    //alert(count);
}

function ValidateForm()
{
    var selectedCheckboxes = $("input:checked").length;
    if (selectedCheckboxes < 1)
    {
        alert("Please select a trip from the provided options.")
        return false;
    }
    else
    {
        $("#submitBtn").trigger("click");
    }

}


function SetCountRange(checkboxId)
{
    var checkbox = document.getElementById(checkboxId);
    var textBoxId = $(checkbox).attr('textboxId');
    var textBox = document.getElementById(textBoxId);

    //Set required && min
    if ($(checkbox).prop('checked'))
    {
        $(textBox).attr('required', 'required');
        $(textBox).attr('min', 1);
        $(textBox).attr('disabled', false);
        $(textBox).val(1);
    } else
    {
        $(textBox).removeAttr('required');
        $(textBox).removeAttr('min');
        $(textBox).attr('disabled', true);
        $(textBox).val(0);
    }

    //alert('this.checked: ' + $(checkbox).prop('checked'));
  
}