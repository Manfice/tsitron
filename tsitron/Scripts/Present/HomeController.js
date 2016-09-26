var getBouquets = function() {
    $.ajax({
        type: "POST",
        url: "/api/whome/getbouquets/",
        data: bouquets,
        success: function(data) {
                bouquets.goods.removeAll(),
                bouquets.goods.push.apply(bouquets.goods, data);
        }
    });
}

ko.showMoreArray = function(initial) {
    var observable = ko.observableArray(initial);
    observable.limit = ko.observable(5);
    observable.showAll = ko.observable(false);
    observable.toggleShowAll = function() {
        observable.showAll(!observable.showAll());
    };
    observable.display = ko.computed(function() {
        if (observable.showAll()) { return observable(); }
        return observable().slice(0,observable.limit());
    }, observable);
    return observable;
};

var showMore = function () {
    var items = bouquets.goods.limit() + 5;
    bouquets.goods.limit(items);
}

//var vm = function () {
//    this.orders = ko.showMoreArray(["Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9", "Option 10"]);
//}
