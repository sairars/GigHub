let GigDetailsController = function (followingService) {
    let button;

    let init = function () {
        $(".js-toggle-following").click(toggleFollowing);
    };

    let toggleFollowing = function () {
        button = $(this);

        let artistId = button.attr("data-artist-id");

        if (button.hasClass("btn-default"))
            followingService.createFollowing(artistId, done, fail);
        else
            followingService.deleteFollowing(artistId, done, fail);
    };

    let done = function () {
        button
            .toggleClass("btn-default")
            .toggleClass("btn-info");
        let text = button.hasClass("btn-default") ? "Follow" : "Following";

        button.text(text);
    };

    let fail = function () {
        alert("Something failed.");
    };

    return {
        init: init
    };
}(FollowingService);