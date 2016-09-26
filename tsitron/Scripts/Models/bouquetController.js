var bUrl = "/api/webseller/";

var RouteLincks = {
    GetBouquets: bUrl + "GetBouquets/",
    SaveBouq: bUrl+"CreateBouquet/"
}

var publishBouquet = function (pub) {
    VmPublic.publish(pub);
    $.ajax({
        type: "Post",
        url: "/api/webseller/Publish",
        data: VmPublic,
        success: function (data) {
            updateVm(data);
        }
    });
};

var editBouquet = function(bouquet) {
    getBouquet(bouquet.id);
    ViewModelBouquet.view('edit');
}

var goToMain = function() {
    ViewModelBouquet.view('main');
}

var getBouqList = function (cust) {
    $.ajax({
        type: "Get",
        url: '/api/webseller/GetBouquets/' + cust,
        success: function (data) {
            ViewModelBouquet.customer(cust);
            ViewModelBouquet.bouqList.removeAll();
            ViewModelBouquet.bouqList.push.apply(ViewModelBouquet.bouqList, data);
        }
    });
};

var updateVm = function (dt) {
    BouquetModel.title(dt.title);
    BouquetModel.id(dt.id);
    BouquetModel.art(dt.art);
    BouquetModel.price(dt.price);
    BouquetModel.timeToMake(dt.timeToMake);
    BouquetModel.composition(dt.composition);
    BouquetModel.description(dt.description);
    BouquetModel.description(dt.description);
    BouquetModel.description(dt.description);
    BouquetModel.details.height(dt.details.height);
    BouquetModel.details.width(dt.details.width);
    BouquetModel.photoView.alt(dt.photoView.alt);
    BouquetModel.photoView.path(dt.photoView.path);
    BouquetModel.bouqAttrebutes.Accepted(dt.bouqAttrebutes.accepted);
    BouquetModel.bouqAttrebutes.Moderated(dt.bouqAttrebutes.moderated);
    BouquetModel.bouqAttrebutes.published(dt.bouqAttrebutes.published);
    BouquetModel.bouqAttrebutes.NewInLine(dt.bouqAttrebutes.NewInLine);
    BouquetModel.moderatedResult.resultAnsw(dt.moderatedResult.resultAnsw);
    BouquetModel.bouqAttrebutes.SpecialPrice(dt.bouqAttrebutes.SpecialPrice);
    BouquetModel.colors.removeAll();
    BouquetModel.colors.push.apply(BouquetModel.colors, dt.colors);
    BouquetModel.eventTypes.removeAll();
    BouquetModel.eventTypes.push.apply(BouquetModel.eventTypes, dt.eventTypes);
    BouquetModel.photos.removeAll();
    BouquetModel.photos.push.apply(BouquetModel.photos, dt.photos);
};

var getBouquet = function (id) {
    $.ajax({
        type: "Get",
        url: "/api/webseller/getBouquetData/" + id,
        success: function (data) {
            updateVm(data);
        }
    });
};

var deleteBouquet = function(id) {
    $.ajax({
        type: 'post',
        url: '/api/webseller/deleteBouquet/' + id.id,
        success: function(data) {
            getBouqList(data.code);
        }
    });
}

var saveBouquetData = function () {
    $.ajax({
        type: "Post",
        url: "/api/webseller/SaveBouquetData",
        data: ko.toJS(BouquetModel),
        success: function (data) {
            ViewModelBouquet.view('main');
            getBouqList(ViewModelBouquet.customer());
        }
    });
};