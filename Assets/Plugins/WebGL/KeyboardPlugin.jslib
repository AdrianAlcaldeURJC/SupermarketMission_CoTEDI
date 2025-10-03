mergeInto(LibraryManager.library, {
    FocusInputField: function (id) {
        var input = document.getElementById("input-" + id);
        if (input) {
            input.focus();
        }
    }
});