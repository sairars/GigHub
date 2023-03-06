let FollowingService = function () {
    let createFollowing = function (artistId, done, fail) {
        $.post("/api/followings", { artistId: artistId })
            .done(done)
            .fail(fail);
    };

    let deleteFollowing = function (artistId, done, fail) {
        $.ajax({
            url: "/api/followings/" + artistId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    };
}();