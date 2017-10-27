fromStream('$streams')
.when({
    $init: function () {
        return []
    },
    $any: function (s, e) {
        var category = e.streamId.split("-")[0];

        if (s.includes(category))
            return;

        s.push(category);
    }
})


fromAll()
    .when({
        $init: function () {
            return { eventCount: 0, categories: [] }
        },
        $any: function (s, e) {
            s.eventCount++;
            var category = e.streamId.split("-")[0];

            if (s.categories.includes(category))
                return;

            s.categories.push(category);
        }
    })