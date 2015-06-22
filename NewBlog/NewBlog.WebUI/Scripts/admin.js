var hidden = true;

// Toggle click to Fade the Options
$(".controls").click(function () {

    if (hidden === false) {

        $(".more-options .fa").each(function () {
            $(this).css({
                "margin-top": "-45px",
                "opacity": "0"
            });
        });
        hidden = true;

    } else {
        $(".more-options .fa").each(function () {
            $(this).css({
                "margin-top": "0",
                "opacity": "1"
            });
        });
        hidden = false;
    }

});

var menuHidden = true;

// Toggle click the Menu
$(".fa-bars").click(function () {

    if (menuHidden === true) {

        $(".menu").css({
            left: 0 + "px"
        });
        $(".fa-bars").css({
            "margin-left": 0 + "px"
        });
        menuHidden = false;

    } else {
        $(".menu").css({
            left: -150 + "px"
        });
        $(".fa-bars").css({
            "margin-left": -150 + "px"
        });
        menuHidden = true;
    }

});



/*Blog Post Object Constructor
===============================*/

function Blog(topic, author, blogDate) {
    this.topic = topic;
    this.author = author;
    this.blogDate = blogDate;
}







