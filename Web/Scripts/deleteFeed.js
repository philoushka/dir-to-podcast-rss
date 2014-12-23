$(function () {
    $(".deleteFeedLink").click(function () {
        var feedTokenToDel = $(this).attr("id");

        jQuery.ajax({
            type: "POST",
            url: 'delete/' + feedTokenToDel,
            dataType: "json",
            success: function (deleteSuccess) {
                if (deleteSuccess) {
                    $("#row" + feedTokenToDel)
                        .children('td, th')
                        .animate({ margin: 0 }, 500)
                        .wrapInner('<div />')
                        .children()
                        .slideUp(500, function () { $("#row" + feedTokenToDel).remove(); });
                }
            }
        });
    });
});