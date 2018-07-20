(function ($) {
    $(function () {


        $.widget("zpd.paging", {
            options: {
                limit: 5,
                limitPaging: 5,
                rowDisplayStyle: 'block',
                activePage: 0,
                rows: []
            },
            _create: function () {
                var rows = $("tbody", this.element).children();               
                var emptyclass = $("#Emptyid").hasClass("Empty");
                //alert(Empty);
                //alert(emptyclass);
                //alert(($('#Emptyid.Empty').length));
                if (rows.length != 0 && emptyclass!=true) {
                    this.options.rows = rows;
                    this.options.rowDisplayStyle = rows.css('display');
                    var nav = this._getNavBar();
                    var TotalRecords = this._getTotalRecoreds();
                    this.element.after(nav);
                    this.element.after(TotalRecords);
                    // $("#Pagination").append('<label id="total" for="Total" style="color: #4482a2;margin-left: 502px;margin-top: 8px;"></label>')

                    this.showPage(0);

                    $("#Page-0").attr('class', "selected-page");
                    var id = $(".selected-page").attr('id');

                    this.totalrecords(id);
                }
            },
            _getNavBar: function () {
                var rows = this.options.rows;
                var nav = $('<div id= "Pagination" class="paging-nav" style="float: left; width: 74%;">', { class: 'paging-nav ' });
                var displayVal;
                for (var i = 0; i < Math.ceil(rows.length / this.options.limit) ; i++) {
                    displayVal = 'display:inline-block';
                    if (i > this.options.limitPaging) displayVal = 'display:none';

                    this._on($('<a>', {
                        href: '#',
                        text: (i + 1),
                        id: ("Page-" + i),
                        "data-page": (i),
                        style: displayVal
                    }).appendTo(nav), {
                        click: "pageClickHandler"
                    });

                }
                //create previous link
                this._on($('<a>', {
                    href: '#',
                    text: '<<',
                    "data-direction": -1
                }).prependTo(nav), {
                    click: "pageStepHandler"
                });
                //create next link
                this._on($('<a>', {
                    href: '#',
                    text: '>>',
                    "data-direction": +1
                }).appendTo(nav), {
                    click: "pageStepHandler"
                });
                return nav;
            },
            _getTotalRecoreds: function () {
                var rows = this.options.rows;
                var nav = $('<div id= "Labeldiv " ><label for="Total" style="color: #4482a2;float: right;width: 25%;text-align: right;"></div>', { class: '' });

                return nav;

            },
            showPage: function (pageNum) {
                var num = pageNum * 1; //it has to be numeric
                this.options.activePage = num;
                var rows = this.options.rows;
                var limit = this.options.limit;
                for (var i = 0; i < rows.length; i++) {
                    if (i >= limit * num && i < limit * (num + 1)) {
                        $(rows[i]).css('display', this.options.rowDisplayStyle);
                    } else {
                        $(rows[i]).css('display', 'none');
                    }
                }
            },

            totalrecords: function (id) {

                var TotalRow = $('#ListviewTable >tbody >tr').length;
                var paginationlimit = 5;
                var temp = new Array();
                temp = id.split("-");
                var Getid = temp[1];
                var multiple = parseInt(Getid) + 1;
                var ShowingDateTo = parseInt(multiple) * parseInt(paginationlimit);
                var ShowingDateFrom = parseInt(ShowingDateTo) - 4;
                if (ShowingDateTo > TotalRow) {
                    ShowingDateTo = TotalRow;
                }
                jQuery("label[for='Total']").html('Showing ' + ShowingDateFrom + ' to ' + ShowingDateTo + ' of ' + TotalRow + ' entries');


            },
            pageClickHandler: function (event) {
                event.preventDefault();
                $(event.target).siblings().attr('class', "");
                $(event.target).attr('class', "selected-page");
                var pageNum = $(event.target).attr('data-page');
                var itemsOnEachSide = this.options.limitPaging / 2;
                var startPage = Math.floor(parseInt(pageNum) - itemsOnEachSide + 1); // Plus 1 because the array of links starts in "1" index and not "0". "0" index is the next (">>") button 
                var endPage = Math.ceil(parseInt(pageNum) + itemsOnEachSide + 1);
                var pagingLinks = $(event.target).parent().children();
                for (var i = 1; i < pagingLinks.length - 1; i++) {
                    if (i >= startPage && i <= endPage) $(pagingLinks[i]).css('display', 'inline-block');
                    else $(pagingLinks[i]).css('display', 'none');
                }

                var id = $(".selected-page").attr('id');
                this.showPage(pageNum);
                this.totalrecords(id);
            },
            pageStepHandler: function (event) {
                event.preventDefault();
                //get the direction and ensure it's numeric
                var dir = $(event.target).attr('data-direction') * 1;
                var pageNum = this.options.activePage + dir;
                //if we're in limit, trigger the requested pages link
                if (pageNum >= 0 && pageNum < this.options.rows.length) {
                    $("a[data-page=" + pageNum + "]", $(event.target).parent()).click();
                }
            }
        });
        $('#ListviewGrid').paging({
            limit: 5,
            limitPaging: 3
        });
    });


})(jQuery);



