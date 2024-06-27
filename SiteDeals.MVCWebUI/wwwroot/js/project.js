//// JavaScript Document
//import { Spinner } from "../lib/spin.js/spin.min.js";

function gotoHash(id) {
    setTimeout(function () {
        var $target = $(id),
            scrollOffset = $("#containerHeader").height() * 2,
            y = $target.offset().top - scrollOffset;

        if ($target.length) {
            window.scrollTo(0, y);
        }
    });
}

$('a[href^="#"]').on('click', function () {
    gotoHash($(this).attr('href'));
});

$(document).ready(function () {
    if (location.hash) {
        gotoHash(location.hash);
    }
});

jQuery(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() > 10) {
            $('#containerHeader').addClass('navScrolled');
        } else {
            $('#containerHeader').removeClass('navScrolled');
        }
    });

    var $navList = $('.menu');
    $navList.on('click', 'li:not(.selected)', function (e) {
        $navList.find(".selected").removeClass("selected");
        $(e.currentTarget).addClass("selected");
    });

    $(".hamburger-btn").click(function () {
        $(".hamburger-btn").toggleClass("hamburger-change");
        $("#menu-list").toggleClass("menuResponsive").finish().slideToggle(500);
        $("header").toggleClass("mobilBg");
    });


    //new Spinner({ color: '#fff', lines: 12 }).spin(target);
});

$.blockUI.defaults.css = {
    padding: 0,
    margin: 0,
    width: '30%',
    top: '40%',
    left: '35%',
    textAlign: 'center',
    color: '#000',
    cursor: 'wait'
};

//var spinnerOpts = {
//    lines: 13, // The number of lines to draw
//    length: 38, // The length of each line
//    width: 17, // The line thickness
//    radius: 45, // The radius of the inner circle
//    scale: 1, // Scales overall size of the spinner
//    corners: 1, // Corner roundness (0..1)
//    speed: 1, // Rounds per second
//    rotate: 0, // The rotation offset
//    animation: 'spinner-line-fade-quick', // The CSS animation name for the lines
//    direction: 1, // 1: clockwise, -1: counterclockwise
//    color: '#ffffff', // CSS color or array of colors
//    fadeColor: 'transparent', // CSS color or array of colors
//    top: '50%', // Top position relative to parent
//    left: '50%', // Left position relative to parent
//    shadow: '0 0 1px transparent', // Box-shadow for the lines
//    zIndex: 2000000000, // The z-index (defaults to 2e9)
//    className: 'spinner', // The CSS class to assign to the spinner
//    position: 'absolute', // Element positioning
//};

//function spin(elem) {
//    var spinner = new Spinner(spinnerOpts).spin(elem);
//    return spinner;
//}
//function spinZa() {
//    var sp2 = new Spinner({ color: '#fff', lines: 12 }).spin(document.getElementById("za"));
//}

//function stopSpin(spinner) {
//    new spinner.stop();
//}

////function stopSpin(elem) {
////    new Spinner(opts).stop();
////}

//window.spin = spin;
//window.stopSpin = stopSpin;
//window.spinZa = spinZa;