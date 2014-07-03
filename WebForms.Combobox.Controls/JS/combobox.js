ComboboxWrapper = function (id, contentId, selectedId) {
    this.id = id;
    this.$selected = $("#" + selectedId);

    var selected = this.$selected.val();
    this.selected = ko.observable(selected != "" ? $.parseJSON(selected) : null);
    this.selected.subscribe(this.onSelected, this);

    this.options = {
        valueMember: "Name",
        dataSource: this.getData.bind(this)
    };

    ko.applyBindings(this, $("#" + contentId)[0]);
};

ComboboxWrapper.prototype = {
    getData: function (options) {
        WebForm_DoCallback(this.id, ko.toJSON(options), function (data) {
            options.callback($.parseJSON(data));
        }.bind(this), null, null, false);
    },
    onSelected: function (value) {
        this.$selected.val(ko.toJSON(value));
    }
};