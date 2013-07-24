$(function () {

    if ($('.SyosMap')[0]) {

        $('head').append('<link type="text/css" href="SyosMap.css" rel="stylesheet">');

        $('.SyosSeat').each(function () {
            $(this).css('background-color', $(this).css('color'));
        });

        $('.SyosSeatAvail').each(function () {
            $(this).bind('click', showInfoBox);
        });

        $('input[value~="Add"]').remove();

        $(".ui-widget-overlay").live("click", function () {
            $(".ui-dialog-content").dialog("close");
        });

        setTimeout(function () {
            $('.ArrangedByJs').css('visibility', 'visible');
        }, 1500);

    }
});

function showInfoBox(event)
{
    var clickedSeatDd = event.target;

    $(clickedSeatDd).addClass('SyosClickedSeat');

    var clickedInputName = $($(clickedSeatDd).children('input')[0]).attr('name');
    var containingSectionDiv = $('.SyosSection').has(clickedSeatDd)[0];
    var sectionName = $($(containingSectionDiv).children()[0]).html();
    var disclaimers = $($(containingSectionDiv).children('.SyosDisclaimers')[0]).html();
    var containingZoneDiv = $('.SyosZone').has(clickedSeatDd)[0];
    var zoneName = $($(containingZoneDiv).children()[0]).html();

    var seatDdGroup = $(clickedSeatDd).prevUntil('dt')
        .add($(clickedSeatDd).nextUntil('dt'))
        .add($(clickedSeatDd));

    var takenSeatDdGroup = seatDdGroup.has('input[disabled]');
    var reserveGroupButtonText = "Add "
    var groupPartiallyReservedNote;
    if (seatDdGroup.length < 6 && takenSeatDdGroup.length > 0) {
        groupPartiallyReservedNote =
            "<p>* Some seats at this table have already been reserved.</p>";
        reserveGroupButtonText += "remaining "
    }
    else {
        groupPartiallyReservedNote = "";
        reserveGroupButtonText += "all "
    }
    reserveGroupButtonText += (seatDdGroup.length - takenSeatDdGroup.length).toString()
        + " seats at table";

    var buttons = {};
    buttons["Add seat to cart"] = function () {
            var checkBoxNameToCheck = $(this).attr('id');
            $('input[name="' + checkBoxNameToCheck + '"]').attr('checked', 'checked');
            $('form').has('.SyosMap').submit();
        };
    if (seatDdGroup.length - takenSeatDdGroup.length > 1
        && seatDdGroup.length - takenSeatDdGroup.length < 6)
        buttons[reserveGroupButtonText] = function () {
                for (var i = 0; i < seatDdGroup.length; i++)
                    $(seatDdGroup[i]).children('input').attr('checked', 'checked');
                $('form').has('.SyosMap').submit();
            }
        
    var dialogDiv = $('<div>');
    dialogDiv.attr('id', clickedInputName);
    dialogDiv.attr('title', 'Seat Information');
    dialogDiv.attr('class', 'SyosSeatInfo');
    dialogDiv.append('<dl>'
        + '<dt>Section:</dt>'
        + '<dd>' + sectionName + '</dd>'
        + '<dt>Price:</dt>'
        + '<dd>' + zoneName + '</dd>'
        + '</dl>'
        + '<div class="SyosDisclaimers">' + disclaimers + '</div>'
        + groupPartiallyReservedNote);
    dialogDiv.dialog({
        modal: true,
        close: function (event) {
            $(clickedSeatDd).removeClass('SyosClickedSeat');
        },
        width: 550,
        buttons: buttons,
        /*buttons: {
            "Add seat to cart": function () {
                var checkBoxNameToCheck = $(this).attr('id');
                $('input[name="' + checkBoxNameToCheck + '"]').attr('checked', 'checked');
                $('form').has('.SyosMap').submit();
            },
            reserveGroupButtonText.toString() : function () {
                for (var i = 0; i < seatDdGroup.length; i++)
                    $(seatDdGroup[i]).children('input').attr('checked', 'checked');
                $('form').has('.SyosMap').submit();
            }
        },*/
        draggable: true
    });
    //alert('hi');
}