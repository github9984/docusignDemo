$(document)
    .ready(function() {

        $("#btnLogin")
            .click(function(e) {
                var $myForm = $("#frmLogin");
                if ($myForm[0].checkValidity()) {
                    $(this).html("Loading... <i class='fa fa-spinner fa-spin'></i>");
                }
            });

        $(".login-form")
            .find("input, textarea")
            .on("keyup blur focus",
                function(e) {

                    var $this = $(this),
                        label = $this.prev("label");

                    if (e.type === "keyup") {
                        if ($this.val() === "") {
                            label.removeClass("active highlight");
                        } else {
                            label.addClass("active highlight");
                        }
                    } else if (e.type === "blur") {
                        if ($this.val() === "") {
                            label.removeClass("active highlight");
                        } else {
                            label.removeClass("highlight");
                        }
                    } else if (e.type === "focus") {

                        if ($this.val() === "") {
                            label.removeClass("highlight");
                        } else if ($this.val() !== "") {
                            label.addClass("highlight");
                        }
                    }

                });
    });