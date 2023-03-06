let GigsController = function (attendanceService) {

    let button;

    let init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    }

    let toggleAttendance = function () {
        button = $(this);

        let gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    }

    let done = function () {
        button
            .toggleClass("btn-info")
            .toggleClass("btn-default")

        let text = button.hasClass("btn-info") ? "Going" : "Going?";

        button.text(text);
    }

    let fail = function () {
        alert("Something failed!");
    }

    return {
        init: init
    };
}(AttendanceService);