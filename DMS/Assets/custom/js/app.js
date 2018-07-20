//windowHeight = $(window).height() - 155;
//$('.box-header').css('min-height', windowHeight);

/*! cciplObj app.js
 * ================
 * Main JS application file for cciplObj v2. This file
 * should be included in all pages. It controls some layout
 * options and implements exclusive cciplObj plugins.
 *
 * @Author  Almsaeed Studio
 * @Support <http://www.almsaeedstudio.com>
 * @Email   <support@almsaeedstudio.com>
 * @version 2.0.4
 * @license MIT <http://opensource.org/licenses/MIT>
 */

'use strict';

//Make sure jQuery has been loaded before app.js
if (typeof jQuery === "undefined") {
    throw new Error("requires jQuery");
}

/* cciplObj
 *
 * @type Object
 * @description $.cciplObj is the main object for the template's app.
 *              It's used for implementing functions and options related
 *              to the template. Keeping everything wrapped in an object
 *              prevents conflict with other plugins and is a better
 *              way to organize our code.
 */
$.cciplObj = {};

/* --------------------
 * - cciplObj Options -
 * --------------------
 * Modify these options to suit your implementation
 */
$.cciplObj.options = {
    //Add slimscroll to navbar menus
    //This requires you to load the slimscroll plugin
    //in every page before app.js
    navbarMenuSlimscroll: true,
    navbarMenuSlimscrollWidth: "3px", //The width of the scroll bar
    navbarMenuHeight: "200px", //The height of the inner menu
    //Sidebar push menu toggle button selector
    sidebarToggleSelector: "[data-toggle='offcanvas']",
    //Activate sidebar push menu
    sidebarPushMenu: true,
    //Activate sidebar slimscroll if the fixed layout is set (requires SlimScroll Plugin)
    sidebarSlimScroll: true,
    //BoxRefresh Plugin
    enableBoxRefresh: true,
    //Bootstrap.js tooltip
    enableBSToppltip: true,
    BSTooltipSelector: "[data-toggle='tooltip']",
    //Enable Fast Click. Fastclick.js creates a more
    //native touch experience with touch devices. If you
    //choose to enable the plugin, make sure you load the script
    //before cciplObj's app.js
    enableFastclick: true,
    //Box Widget Plugin. Enable this plugin
    //to allow boxes to be collapsed and/or removed
    enableBoxWidget: true,
    //Box Widget plugin options
    boxWidgetOptions: {
        boxWidgetIcons: {
            //The icon that triggers the collapse event
            collapse: 'fa fa-minus',
            //The icon that trigger the opening event
            open: 'fa fa-plus',
            //The icon that triggers the removing event
            remove: 'fa fa-times'
        },
        boxWidgetSelectors: {
            //Remove button selector
            remove: '[data-widget="remove"]',
            //Collapse button selector
            collapse: '[data-widget="collapse"]'
        }
    },
    //Direct Chat plugin options
    directChat: {
        //Enable direct chat by default
        enable: true,
        //The button to open and close the chat contacts pane
        contactToggleSelector: '[data-widget="chat-pane-toggle"]'
    },
    //Define the set of colors to use globally around the website
    colors: {
        lightBlue: "#3c8dbc",
        red: "#f56954",
        green: "#00a65a",
        aqua: "#00c0ef",
        yellow: "#f39c12",
        blue: "#0073b7",
        navy: "#001F3F",
        teal: "#39CCCC",
        olive: "#3D9970",
        lime: "#01FF70",
        orange: "#FF851B",
        fuchsia: "#F012BE",
        purple: "#8E24AA",
        maroon: "#D81B60",
        black: "#222222",
        gray: "#d2d6de"
    },
    //The standard screen sizes that bootstrap uses.
    //If you change these in the variables.less file, change
    //them here too.
    screenSizes: {
        xs: 480,
        sm: 768,
        md: 992,
        lg: 1200
    }
};

/* ------------------
 * - Implementation -
 * ------------------
 * The next block of code implements cciplObj's
 * functions and plugins as specified by the
 * options above.
 */
$(function () {
    //Easy access to options
    var o = $.cciplObj.options;

    //Activate the layout maker
    $.cciplObj.layout.activate();

    //Enable sidebar tree view controls
    $.cciplObj.tree('.sidebar');

    $.cciplObj.sideMenuOpn();

    //Add slimscroll to navbar dropdown
    if (o.navbarMenuSlimscroll && typeof $.fn.slimscroll != 'undefined') {
        $(".navbar .menu").slimscroll({
            height: "200px",
            alwaysVisible: false,
            size: "3px"
        }).css("width", "100%");
    }

    //Activate sidebar push menu
    if (o.sidebarPushMenu) {
        $.cciplObj.pushMenu(o.sidebarToggleSelector);
    }

    //Activate Bootstrap tooltip
    if (o.enableBSToppltip) {
        $(o.BSTooltipSelector).tooltip();
    }

    //Activate box widget
    if (o.enableBoxWidget) {
        $.cciplObj.boxWidget.activate();
    }

    //Activate fast click
    if (o.enableFastclick && typeof FastClick != 'undefined') {
        FastClick.attach(document.body);
    }

    //Activate direct chat widget
    if (o.directChat.enable) {
        $(o.directChat.contactToggleSelector).click(function () {
            var box = $(this).parents('.direct-chat').first();
            box.toggleClass('direct-chat-contacts-open');
        });
    }

    /*
     * INITIALIZE BUTTON TOGGLE
     * ------------------------
     */
    $('.btn-group[data-toggle="btn-toggle"]').each(function () {
        var group = $(this);
        $(this).find(".btn").click(function (e) {
            group.find(".btn.active").removeClass("active");
            $(this).addClass("active");
            e.preventDefault();
        });

    });
});

/* ----------------------
 * - cciplObj Functions -
 * ----------------------
 * All cciplObj functions are implemented below.
 */

/* prepareLayout
 * =============
 * Fixes the layout height in case min-height fails.
 *
 * @type Object
 * @usage $.cciplObj.layout.activate()
 *        $.cciplObj.layout.fix()
 *        $.cciplObj.layout.fixSidebar()
 */
$.cciplObj.layout = {
    activate: function () {
        var _this = this;
        _this.fix();
        _this.fixSidebar();
        $(window, ".wrapper").resize(function () {
            _this.fix();
            _this.fixSidebar();
        });
    },
    fix: function () {
        //Get window height and the wrapper height
        var neg = $('.main-header').outerHeight() + $('.main-footer').outerHeight();
        var window_height = $(window).height();
        var sidebar_height = $(".sidebar").height();
        //Set the min-height of the content and sidebar based on the
        //the height of the document.
        if ($("body").hasClass("fixed")) {
            $(".content-wrapper, .right-side").css('min-height', window_height - $('.main-footer').outerHeight());
        } else {
            if (window_height >= sidebar_height) {
                $(".content-wrapper, .right-side").css('min-height', window_height - neg);
            } else {
                $(".content-wrapper, .right-side").css('min-height', sidebar_height);
            }
        }
    },
    fixSidebar: function () {
        //Make sure the body tag has the .fixed class
        if (!$("body").hasClass("fixed")) {
            if (typeof $.fn.slimScroll != 'undefined') {
                $(".sidebar").slimScroll({ destroy: true }).height("auto");
            }
            return;
        } else if (typeof $.fn.slimScroll == 'undefined' && console) {
            console.error("Error: the fixed layout requires the slimscroll plugin!");
        }
        //Enable slimscroll for fixed layout
        if ($.cciplObj.options.sidebarSlimScroll) {
            if (typeof $.fn.slimScroll != 'undefined') {
                //Distroy if it exists
                $(".sidebar").slimScroll({ destroy: true }).height("auto");
                //Add slimscroll
                $(".sidebar").slimscroll({
                    height: ($(window).height() - $(".main-header").height()) + "px",
                    color: "rgba(0,0,0,0.2)",
                    size: "3px"
                });
            }
        }
    }
};

/* PushMenu()
 * ==========
 * Adds the push menu functionality to the sidebar.
 *
 * @type Function
 * @usage: $.cciplObj.pushMenu("[data-toggle='offcanvas']")
 */
$.cciplObj.pushMenu = function (toggleBtn) {
    //Get the screen sizes
    var screenSizes = this.options.screenSizes;

    //Enable sidebar toggle
    $(toggleBtn).click(function (e) {
        e.preventDefault();

        //Enable sidebar push menu
        if ($(window).width() > (screenSizes.sm - 1)) {
            $("body").toggleClass('sidebar-collapse');
        }
            //Handle sidebar push menu for small screens
        else {
            if ($("body").hasClass('sidebar-open')) {
                $("body").removeClass('sidebar-open');
                $("body").removeClass('sidebar-collapse')
            } else {
                $("body").addClass('sidebar-open');
            }
        }
    });

    $(".content-wrapper").click(function () {
        //Enable hide menu when clicking on the content-wrapper on small screens
        if ($(window).width() <= (screenSizes.sm - 1) && $("body").hasClass("sidebar-open")) {
            $("body").removeClass('sidebar-open');
        }
    });

};

/* Tree()
 * ======
 * Converts the sidebar into a multilevel
 * tree view menu.
 *
 * @type Function
 * @Usage: $.cciplObj.tree('.sidebar')
 */
$.cciplObj.tree = function (menu) {
    var _this = this;

    $("li a", $(menu)).click(function (e) {
        //Get the clicked link and the next element
        var $this = $(this);
        var checkElement = $this.next();

        //Check if the next element is a menu and is visible
        if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible'))) {
            //Close the menu
            checkElement.slideUp('normal', function () {
                checkElement.removeClass('menu-open');
                //Fix the layout in case the sidebar stretches over the height of the window
                //_this.layout.fix();
            });
            checkElement.parent("li").removeClass("active");
        }
            //If the menu is not visible
        else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
            //Get the parent menu
            var parent = $this.parents('ul').first();
            //Close all open menus within the parent
            var ul = parent.find('ul:visible').slideUp('normal');
            //Remove the menu-open class from the parent
            ul.removeClass('menu-open');
            //Get the parent li
            var parent_li = $this.parent("li");

            //Open the target menu and add the menu-open class
            checkElement.slideDown('normal', function () {
                //Add the class active to the parent li
                checkElement.addClass('menu-open');
                parent.find('li.active').removeClass('active');
                parent_li.addClass('active');
                //Fix the layout in case the sidebar stretches over the height of the window
                _this.layout.fix();
            });
        }
        //if this isn't a link, prevent the page from being redirected
        if (checkElement.is('.treeview-menu')) {
            e.preventDefault();
        }
    });
};

/*
 * 
 * Side Menuoptions
 * 
 */
/*For your Machine SideMenu Open*/
// $.cciplObj.sideMenuOpn = function () {
//     var pageVal = location.pathname.split('/');
//     var pageVal = location.pathname;
//     var result = pageVal.substring(1, pageVal.length - 0);

//     //alert(location.pathname);
//     //alert(pageVal[0]+pageVal[1]);
//	$("li.treeview").each(function() {		

//		var dfhgdsgj=$(this).children().length;			
//		if(dfhgdsgj>1)
//		{		
//			var fhdsf=$(this).find('ul.treeview-menu li a').attr('href');
//			var parentli=$(this);
//			var fhdsf=$(this).find('ul.treeview-menu li');						
//			fhdsf.each(function(index, value){

//			    var getlival=$(this).find('a').attr('href');
//			    var getlival = $(this).find('a').attr('alt');
//			  //  alert(getlival)
//			    if (result == 'Home/UnderConstruction' || result == 'Home/Welcome')
//			    {
//			        return false;
//			    }
//			    if (result == getlival) {
//			        parentli.addClass('active1').addClass('active');
//			        $(this).find('ul.treeview-menu').addClass('menu-open');
//			        parentli.find('ul.treeview-menu').slideDown('normal', function () {
//			            parentli.find('ul.treeview-menu').addClass('menu-open');
//			        });
//			    }
//			    else {
//			        $(this).addClass('active1');
//			    }
//			});			
//		}
//		else
//		{				
//		    var without_ul = $(this).find('a').attr('alt');

//		    if (result == without_ul)
//		    {

//				$(this).addClass('active1');

//			}		
//		}					
//	});
//};


/////*For IIS Server SideMenu OPen*/
$.cciplObj.sideMenuOpn = function () {

    //  var pageval = ;
    var pageval = location.pathname;
      var result2 = pageval.split('/').slice(-1)[0];
    var result1 = pageval.split('/').slice(1)[1];
    var result = result1 + '/' + result2;

    $("li.treeview").each(function () {

        var dfhgdsgj = $(this).children().length;
        if (dfhgdsgj > 1) {
            var fhdsf=$(this).find('ul.treeview-menu li a').attr('href');
            var parentli = $(this);
           var fhdsf = $(this).find('ul.treeview-menu li');
            fhdsf.each(function (index, value) {

                //var getlival=$(this).find('a').attr('href');
                var getlival = $(this).find('a').attr('alt');
                //alert(getlival)
                if (result == 'Home/UnderConstruction' || result == 'Home/Welcome') {
                    return false;
                }
                if (result == getlival) {
                    parentli.addClass('active1').addClass('active');
                    //$(this).find('ul.treeview-menu').addClass('menu-open');
                    parentli.find('ul.treeview-menu').slideDown('normal', function () {
                       parentli.find('ul.treeview-menu').addClass('menu-open');
                    });
                }
                else {
                    $(this).addClass('active1');
                }
            });
        }
        else {
            var without_ul = $(this).find('a').attr('alt');

            if (result == without_ul) {

                $(this).addClass('active1');

            }
        }
    });
};


/* BoxWidget
 * =========
 * BoxWidget is plugin to handle collapsing and
 * removing boxes from the screen.
 *
 * @type Object
 * @usage $.cciplObj.boxWidget.activate()
 *        Set all of your option in the main $.cciplObj.options object
 */
$.cciplObj.boxWidget = {
    activate: function () {
        var o = $.cciplObj.options;
        var _this = this;
        //Listen for collapse event triggers
        $(o.boxWidgetOptions.boxWidgetSelectors.collapse).click(function (e) {
            e.preventDefault();
            _this.collapse($(this));
        });

        //Listen for remove event triggers
        $(o.boxWidgetOptions.boxWidgetSelectors.remove).click(function (e) {
            e.preventDefault();
            _this.remove($(this));
        });
    },
    collapse: function (element) {
        //Find the box parent
        var box = element.parents(".box").first();
        //Find the body and the footer
        var bf = box.find(".box-body, .box-footer");
        if (!box.hasClass("collapsed-box")) {
            //Convert minus into plus
            element.children(".fa-minus").removeClass("fa-minus").addClass("fa-plus");
            bf.slideUp(300, function () {
                box.addClass("collapsed-box");
            });
        } else {
            //Convert plus into minus
            element.children(".fa-plus").removeClass("fa-plus").addClass("fa-minus");
            bf.slideDown(300, function () {
                box.removeClass("collapsed-box");
            });
        }
    },
    remove: function (element) {
        //Find the box parent
        var box = element.parents(".box").first();
        box.slideUp();
    },
    options: $.cciplObj.options.boxWidgetOptions
};

/* ------------------
 * - Custom Plugins -
 * ------------------
 * All custom plugins are defined below.
 */

/*
 * BOX REFRESH BUTTON
 * ------------------
 * This is a custom plugin to use with the compenet BOX. It allows you to add
 * a refresh button to the box. It converts the box's state to a loading state.
 *
 * @type plugin
 * @usage $("#box-widget").boxRefresh( options );
 */
(function ($) {

    $.fn.boxRefresh = function (options) {

        // Render options
        var settings = $.extend({
            //Refressh button selector
            trigger: ".refresh-btn",
            //File source to be loaded (e.g: ajax/src.php)
            source: "",
            //Callbacks
            onLoadStart: function (box) {
            }, //Right after the button has been clicked
            onLoadDone: function (box) {
            } //When the source has been loaded

        }, options);

        //The overlay
        var overlay = $('<div class="overlay"><div class="fa fa-refresh fa-spin"></div></div>');

        return this.each(function () {
            //if a source is specified
            if (settings.source === "") {
                if (console) {
                    console.log("Please specify a source first - boxRefresh()");
                }
                return;
            }
            //the box
            var box = $(this);
            //the button
            var rBtn = box.find(settings.trigger).first();

            //On trigger click
            rBtn.click(function (e) {
                e.preventDefault();
                //Add loading overlay
                start(box);

                //Perform ajax call
                box.find(".box-body").load(settings.source, function () {
                    done(box);
                });
            });
        });

        function start(box) {
            //Add overlay and loading img
            box.append(overlay);

            settings.onLoadStart.call(box);
        }

        function done(box) {
            //Remove overlay and loading img
            box.find(overlay).remove();

            settings.onLoadDone.call(box);
        }

    };

})(jQuery);

/*
 * TODO LIST CUSTOM PLUGIN
 * -----------------------
 * This plugin depends on iCheck plugin for checkbox and radio inputs
 *
 * @type plugin
 * @usage $("#todo-widget").todolist( options );
 */
(function ($) {
    $.fn.todolist = function (options) {
        // Render options
        var settings = $.extend({
            //When the user checks the input
            onCheck: function (ele) {
            },
            //When the user unchecks the input
            onUncheck: function (ele) {
            }
        }, options);

        return this.each(function () {

            if (typeof $.fn.iCheck != 'undefined') {
                $('input', this).on('ifChecked', function (event) {
                    var ele = $(this).parents("li").first();
                    ele.toggleClass("done");
                    settings.onCheck.call(ele);
                });

                $('input', this).on('ifUnchecked', function (event) {
                    var ele = $(this).parents("li").first();
                    ele.toggleClass("done");
                    settings.onUncheck.call(ele);
                });
            } else {
                $('input', this).on('change', function (event) {
                    var ele = $(this).parents("li").first();
                    ele.toggleClass("done");
                    settings.onCheck.call(ele);
                });
            }
        });
    };
}(jQuery));
