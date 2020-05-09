$(document).ready(function () {

    $(".settings__wrapper > li > :radio").click(function () {

        var mode = $(this).attr("name");
        console.log(mode);
        if (mode === "binary") {
            unCheckAll(1);
            setMode(1);
        } else if (mode === "multiple") {
            unCheckAll(2);
            setMode(2);
        } else {
            unCheckAll(1);
            setMode(1);
        }

    });

    function setMode(mode) {
        localStorage.setItem("mode", mode);
    }

    function unCheckAll(checked) {
        $(".settings__wrapper > li > :radio").each(function (index) {
            if (index == checked-1) {
                $(this).prop('checked', true);
            } else {
                $(this).prop('checked', false);
            }
        });
    }

});
