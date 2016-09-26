var ViewModelBouquet = {
    view: ko.observable('main'),
    customer: ko.observable(),
    modResult: ko.pureComputed(function() {
        return BouquetModel.bouqAttrebutes.Accepted ? "panel-success" : "panel-warning";
    }),
    bouqList: ko.observableArray([])
};

var BouquetModel = {
    id: ko.observable(),
    art: ko.observable(),
    title: ko.observable(),
    price: ko.observable(),
    timeToMake: ko.observable(),
    composition: ko.observable(),
    description: ko.observable(),
    moderatedResult: {
        resultAnsw:ko.observable()
    },
    bouqAttrebutes: {
        published: ko.observable(),
        Moderated:ko.observable(),
        Accepted:ko.observable(),
        SpecialPrice: ko.observable(),
        NewInLine: ko.observable()
    },
    details: {
        height: ko.observable(),
        width: ko.observable()
    },
    photoView: {
        alt: ko.observable(),
        path: ko.observable()
    },
    colors: ko.observableArray([]),
    eventTypes: ko.observableArray([]),
    photos: ko.observableArray([]),
    modResult: ko.pureComputed(function () {
        return BouquetModel.bouqAttrebutes.Accepted() ? "panel-success" : "panel-warning";
    })
};

var VmPublic = {
    id: BouquetModel.id,
    publish: ko.observable()
};

var showModel = {
    addBoqu: ko.observable(false)
};


var publishBouquet = function(id) {
    $.ajax({
        type: "Post",
        uri: '/api/webseller/Publish/' + id,
        success: getBouqList(ViewModelBouquet.customer())
});
};

var addBShow = function (show) {
    ViewModelBouquet.view(show);
}
