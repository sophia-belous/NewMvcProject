$(function () {

    $('#search-form').submit(function () {
        if ($("#s").val().trim())
            return true;
        return false;
    });
});

$(function () {
    $('.button-checkbox').each(function () {
        var $widget = $(this),
			$button = $widget.find('button'),
			$checkbox = $widget.find('input:checkbox'),
			color = $button.data('color'),
			settings = {
			    on: {
			        icon: 'glyphicon glyphicon-check'
			    },
			    off: {
			        icon: 'glyphicon glyphicon-unchecked'
			    }
			};

        $button.on('click', function () {
            $checkbox.prop('checked', !$checkbox.is(':checked'));
            $checkbox.triggerHandler('change');
            updateDisplay();
        });

        $checkbox.on('change', function () {
            updateDisplay();
        });

        function updateDisplay() {
            var isChecked = $checkbox.is(':checked');
            // Set the button's state
            $button.data('state', (isChecked) ? "on" : "off");

            // Set the button's icon
            $button.find('.state-icon')
				.removeClass()
				.addClass('state-icon ' + settings[$button.data('state')].icon);

            // Update the button's color
            if (isChecked) {
                $button
					.removeClass('btn-default')
					.addClass('btn-' + color + ' active');
            }
            else {
                $button
					.removeClass('btn-' + color + ' active')
					.addClass('btn-default');
            }
        }
        function init() {
            updateDisplay();
            // Inject the icon if applicable
            if ($button.find('.state-icon').length == 0) {
                $button.prepend('<i class="state-icon ' + settings[$button.data('state')].icon + '"></i> ');
            }
        }
        init();
    });
});

/*category*/
$(function () {

    /*make the text always fit inside the container*/
    $('.raContent').textTailor({
        justify: true
    });

    /*make sure the nav and the container are equal height*/
    $('.raOuter').each(function () {
        $(this).find('.raEqualHeight').matchHeight();
    });
});

/*! TextTailor - v0.1.0
 * https://github.com/jpntex
 * http://jpntex.com
 * Copyright (c) 2014 João Teixeira; Licensed MIT  */

(function ($, window) {
    'use strict';

    var settings = {
        minFont: 1,
        maxFont: 9999,
        preWrapText: false,
        lineHeight: 1.45,
        resizable: true,
        debounce: false,
        fit: true,
        ellipsis: true,
        center: false,
        justify: false
    };

    function Tailor(el, options) {
        this.el = el;
        this.options = $.extend({}, settings, options);
        this.init();
    }

    Tailor.prototype = {
        init: function () {
            var error = (this.options.minFont > this.options.maxFont),
                el = $(this.el);

            if (error && window.console) {
                console.log('TextTailor error: minFont needs to be smaller than maxFont!');
            }

            // store original html source from element
            this.HTML = this.el.innerHTML;

            // TODO: check here for height and width errors

            if (!error) {
                var resize,
                    _self = this;

                if (this.options.resizable) {
                    $(window).on('resize', function () {
                        if (_self.options.debounce) {
                            clearTimeout(resize);
                            resize = setTimeout(function () {
                                _self.start();
                            }, 200);
                        } else {
                            _self.start();
                        }
                    });
                }

                this.start();
            }
        },
        start: function () {
            var el = $(this.el);

            // reset element
            this.el.innerHTML = this.HTML;
            el.wrapInner('<div/>');
            this.wraped = $(this.el.firstChild);
            this.wraped.css({
                'line-height': this.options.lineHeight,
                'tranform': 'translateZ(0)',
                'height': 'auto'
            });


            if (this.options.preWrapText) this.wraped.css('white-space', 'pre-line');

            this.maxHeight = el.height();
            this.maxWidth = el.width();

            this.fit().ellipsis().center().justify();
        },
        fit: function () {
            if (this.options.fit) {
                var _self = this,
                    el = this.wraped,
                    maxIter = 30,
                    iterCount = 0,
                    fitCalc = function calcMe(size, min, max) {
                        if (++iterCount === maxIter) return size;
                        if (size <= _self.options.minFont) return _self.options.minFont;
                        if (size >= _self.options.maxFont) return _self.options.maxFont;
                        if (min === max) return min;

                        // force DOM height update
                        el.css('fontSize', (size));

                        if (el[0].scrollHeight < _self.maxHeight) {
                            el.css('fontSize', (size + 1));
                            if (el[0].scrollHeight >= _self.maxHeight) return size;
                            return calcMe(Math.round((max + size) / 2), size, max);
                        } else {
                            el.css('fontSize', (size - 1));
                            return calcMe(Math.round((min + size) / 2), min, size);
                        }
                    };

                var m = Math.round((this.options.maxFont + this.options.minFont) / 2);
                el.css('fontSize', m);

                var measure = fitCalc(m, this.options.minFont, this.options.maxFont);
                el.css('fontSize', measure);
            }

            return this;

        },
        ellipsis: function () {
            if (this.options.ellipsis) {
                var el = this.wraped;

                el.css({
                    'overflow': 'hidden',
                    'text-overflow': 'ellipsis'
                });

                if (el.height() > this.maxHeight) {
                    var tmpText = el.html();
                    el.html('O');

                    var rowHeight = el.height(),
                        start = 1,
                        end = tmpText.length;

                    while (start < end) {
                        var length = Math.ceil((start + end) / 2);
                        el.html(tmpText.slice(0, length) + '...');

                        if (el.height() <= this.maxHeight) {
                            start = length;
                        } else {
                            end = length - 1;
                        }
                    }

                    el.html(tmpText.slice(0, start) + '...');
                }
            }

            return this;
        },
        center: function () {
            if (this.options.center) {
                var pos = $(this.el).css('position');

                if (pos !== 'relative' && pos !== 'absolute') {
                    $(this.el).css("position", "relative");
                }

                this.wraped.css({
                    "position": "absolute",
                    "width": this.wraped.width() + "px",
                    "left": "0",
                    "right": "0",
                    "top": "0",
                    "bottom": "0",
                    "height": this.wraped.height() + "px",
                    "margin": "auto"
                });
            }
            return this;
        },
        justify: function () {
            if (this.options.justify) {
                this.wraped.css({
                    "text-align": "justify"
                });
            }
            return this;
        }
    };

    $.fn.textTailor = function (options) {
        return this.each(function () {
            // prevent multiple instantiations
            if (!$.data(this, 'TextTailor')) {
                $.data(this, 'TextTailor',
                       new Tailor(this, options));
            }
        });
    };

})(jQuery, window);


/*start equal height plugin*/

/*jquery.matchHeight-min.js v0.5.2
* http://brm.io/jquery-match-height/
* License: MIT 
https://github.com/liabru/jquery-match-height */
(function (d) { var g = -1, e = -1, n = function (a) { var b = null, c = []; d(a).each(function () { var a = d(this), k = a.offset().top - h(a.css("margin-top")), l = 0 < c.length ? c[c.length - 1] : null; null === l ? c.push(a) : 1 >= Math.floor(Math.abs(b - k)) ? c[c.length - 1] = l.add(a) : c.push(a); b = k }); return c }, h = function (a) { return parseFloat(a) || 0 }, b = d.fn.matchHeight = function (a) { if ("remove" === a) { var f = this; this.css("height", ""); d.each(b._groups, function (a, b) { b.elements = b.elements.not(f) }); return this } if (1 >= this.length) return this; a = "undefined" !== typeof a ? a : !0; b._groups.push({ elements: this, byRow: a }); b._apply(this, a); return this }; b._groups = []; b._throttle = 80; b._maintainScroll = !1; b._beforeUpdate = null; b._afterUpdate = null; b._apply = function (a, f) { var c = d(a), e = [c], k = d(window).scrollTop(), l = d("html").outerHeight(!0), g = c.parents().filter(":hidden"); g.css("display", "block"); f && (c.each(function () { var a = d(this), b = "inline-block" === a.css("display") ? "inline-block" : "block"; a.data("style-cache", a.attr("style")); a.css({ display: b, "padding-top": "0", "padding-bottom": "0", "margin-top": "0", "margin-bottom": "0", "border-top-width": "0", "border-bottom-width": "0", height: "100px" }) }), e = n(c), c.each(function () { var a = d(this); a.attr("style", a.data("style-cache") || "").css("height", "") })); d.each(e, function (a, b) { var c = d(b), e = 0; f && 1 >= c.length || (c.each(function () { var a = d(this), b = "inline-block" === a.css("display") ? "inline-block" : "block"; a.css({ display: b, height: "" }); a.outerHeight(!1) > e && (e = a.outerHeight(!1)); a.css("display", "") }), c.each(function () { var a = d(this), b = 0; "border-box" !== a.css("box-sizing") && (b += h(a.css("border-top-width")) + h(a.css("border-bottom-width")), b += h(a.css("padding-top")) + h(a.css("padding-bottom"))); a.css("height", e - b) })) }); g.css("display", ""); b._maintainScroll && d(window).scrollTop(k / l * d("html").outerHeight(!0)); return this }; b._applyDataApi = function () { var a = {}; d("[data-match-height], [data-mh]").each(function () { var b = d(this), c = b.attr("data-match-height") || b.attr("data-mh"); a[c] = c in a ? a[c].add(b) : b }); d.each(a, function () { this.matchHeight(!0) }) }; var m = function (a) { b._beforeUpdate && b._beforeUpdate(a, b._groups); d.each(b._groups, function () { b._apply(this.elements, this.byRow) }); b._afterUpdate && b._afterUpdate(a, b._groups) }; b._update = function (a, f) { if (f && "resize" === f.type) { var c = d(window).width(); if (c === g) return; g = c } a ? -1 === e && (e = setTimeout(function () { m(f); e = -1 }, b._throttle)) : m(f) }; d(b._applyDataApi); d(window).bind("load", function (a) { b._update(!1, a) }); d(window).bind("resize orientationchange", function (a) { b._update(!0, a) }) })(jQuery);

/*like*/

var il = 0,
    ih = 36,
    hl = 20,
    hh = 10;

$(".icon").transition({ scale: 1.2, opacity: 0.6 });
$(".dicon").transition({ scale: 1.6, opacity: 0, y: il });
$("h3").transition({ scale: 1.8, opacity: 0, y: hl });

var bigIcon = $(".actionIcon");
$.each(bigIcon, function () {

    var icon = $(this).find(".icon"),
        dicon = $(this).find(".dicon"),
        h3 = $(this).find("h3");

    $(this).hover(function () {
        icon.transition({ scale: 2.1, opacity: 0, delay: 150 }, 350, 'ease');
        dicon.transition({ scale: 1, opacity: 1, y: ih }, 150);
        h3.transition({ scale: 1, opacity: 1, y: hh }, 300, 'snap');
    }, function () {
        icon.transition({ scale: 1.2, opacity: 0.6 }, 200);
        dicon.transition({ scale: 1.6, opacity: 0, y: il }, 200, 'ease');
        h3.transition({ scale: 1.8, opacity: 0, y: hl }, 200, 'ease');
    }
	);
});
