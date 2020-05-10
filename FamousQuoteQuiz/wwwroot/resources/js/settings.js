$(document).ready(function () {

    $(".settings__wrapper > li > :radio").click(function () {
        ;
        var mode = $(this).attr("name");
        var text = "";
        var $changes = $(".changes-text"); 
        if (mode === "binary") {
            unCheckAll(1);
            setMode(1);

            text = "Yes and No Mode Set. Changes Saved";
            $changes.removeClass("text-danger");
            $changes.addClass("text-success");
        } else if (mode === "multiple") {
            unCheckAll(2);
            setMode(2);

            text = "Multiple Choice Mode Set. Changes Saved";
            $changes.removeClass("text-danger");
            $changes.addClass("text-success");
        } else {
            unCheckAll(1);
            setMode(1);

            text = "Couldn't be saved"
            $changes.removeClass("text-success");
            $changes.addClass("text-danger");
        }

        $changes.text(text);

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
