$.connection.hub.start();
$.connection.notifications.client.notificationAdded = function () {
    var countSpan = $('#notifications-count');
    countSpan.text(parseInt(countSpan.text()) + 1);
}
