var bouquets = {
    region: ko.observable(''),
    goods:ko.showMoreArray([])
};

var bouquet = {
    marker: ko.observable(),
    monger:ko.observable(),
    title: ko.observable(),
    price: {
        
        value:ko.observable()
    },
    avatar: {
        path: ko.observable(),
        alt:ko.observable()
    },
    photos:ko.observableArray([])
}

var viewCtrl = {
    totalCount: ko.observable(),
    currentShow: ko.observable()
}