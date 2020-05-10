
$(document).ready(function () {

    var mode = localStorage.getItem("mode") == null ? localStorage.setItem("mode", 1): localStorage.getItem("mode");
    var quoteID;

    initialize();

    $(".answers-group .btn").click(function (event) {
        var author;
        event.stopPropagation();
        if (mode == 1) {
            author = $(this).val() === "Yes" ? $(".binary .author").text() : "None";
        } else if (mode == 2) {
            author = $(this).val();
        } else {
            author = "None";
        }
        getAnswer(quoteID, author);
    });

    $(".author-wrapper .btn").click(function (event) {
        $(".author-wrapper").fadeOut("fast").addClass("hidden");
        initialize();
    });

    function initialize() {
        mode = localStorage.getItem("mode");

        switchMode();
        getQuote();
    }

    function getQuote() {
        var message;
        var error;

        $.ajax({
            type: "GET",
            url: "QuotesAPI/Quote",
            data: { mode: mode},
            dataType: "Json",
            success: function (data) {
                setQuote(data.value);
                getAuthor(data.value);
            },
            error: function (req, status, error) {
                error = "Error unable to retrieve quote";
            }
        });
    }

    function setQuote(quote) {
        quoteID = quote.id;
        $(".quote").text(quote.description);
    }

    function getAuthor(quote) {
        $.ajax({
            type: "GET",
            url: "QuotesAPI/Author",
            data: { mode: mode, id: quote.id },
            dataType: "Json",
            success: function (msg) {
                setAuthor(msg);
            },
            error: function (req, status, error) {
                error = "Error unable to retrieve quote";
            }
        });
    }

    function setAuthor(author) {
        if (mode == 1) {
            $(".binary > .author").text(author.value);
        } else if (mode == 2) {
            var authors = author.value;
            var buttons = $(".multiple > :button");
            $.each(authors, function (key, value) {
                buttons[key].value = authors[key];
            });
        }

        $(".quiz").fadeIn("slow").removeClass("hidden");
    }

    function switchMode() {
        if (mode == 1) {
            $(".answers-group").find(".hidden").removeClass("hidden");
            $(".answers-group").find(".multiple").addClass("hidden");
            $(".binary .author").fadeIn("slow").removeClass("hidden");            
        } else if (mode == 2) {
            $(".answers-group").find(".hidden").removeClass("hidden");
            $(".answers-group").find(".binary").addClass("hidden");
        }
    }

    function getAnswer(id, author) {

        $.ajax({
            type: "GET",
            url: "QuotesAPI/Answer",
            data: { id: id },
            dataType: "Json",
            success: function (msg) {
                setAnswer(msg, author);
            },
            error: function (req, status, error) {
                error = "Error unable to retrieve quote";
            }
        });

    }

    function setAnswer(answer, author) {
        var $question = $(".question-answer").fadeIn().removeClass("hidden");
        var author_page = (mode === 1 && author === "none") ? author : $(".binary .author").text();
        var answerToDisplay = answer.value;
        var answerToAdd = "false";

        author_page = author_page.toLowerCase();
        answer = answer.value.toLowerCase();
        author = author.toLowerCase();

        if (answer === author) {
            correct(answerToDisplay);
            answerToAdd = "true";
        } else if (author == "none" && author_page !== answer) {
            correct(answerToDisplay);
            answerToAdd = "true";
        } else {
            wrong(answerToDisplay);
            answerToAdd = "false";
        }

        $(".multiple").addClass("hidden");
        $(".binary .btn-wrapper").addClass("hidden");
        
        setTimeout(function () {
         $question.addClass("hidden");
         if (mode == 1) { $(".binary .author").fadeOut("slow").addClass("hidden") };
         $(".quote-author").text(answerToDisplay);
         $(".author-wrapper").fadeIn("fast").removeClass("hidden");

        }, 2000);

        sendData(quoteID, answerToAdd);
    }

    function correct(answerToDisplay) {
        var $question = $(".question-answer").fadeIn().removeClass("hidden");

        $question.addClass("text-success");
        $question.removeClass("text-danger");
        $question.text("Correct! The right answer is: " + answerToDisplay);
    }

    function wrong(answerToDisplay) {
        var $question = $(".question-answer").fadeIn().removeClass("hidden");

        $question.removeClass("text-success");
        $question.addClass("text-danger");
        $question.text("Sorry, you are wrong! The right answer is: " + answerToDisplay);
    }

    function sendData(id, answerAdd) {
        $.ajax({
            type: "POST",
            url: "QuotesAPI/SaveAnswer",
            data: { id: id, result: answerAdd },
            dataType: "Json",
            success: function (msg) {
            },
            error: function (req, status, error) {
                error = "Error unable to save answer";
            }
        });
    }
});