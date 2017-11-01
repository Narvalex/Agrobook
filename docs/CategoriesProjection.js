fromStream('$streams')
.when({
    $init: function () {
        return []
    },
    $any: function (s, e) {
        if (e.streamId === undefined) return;
        var category = e.streamId.split("-")[0];

        if (s.includes(category))
            return;

        s.push(category);
    }
})