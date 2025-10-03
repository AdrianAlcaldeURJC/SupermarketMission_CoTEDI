mergeInto(LibraryManager.library, {
    DownloadFile: function(filenamePtr, contentPtr, size) {
        var filename = UTF8ToString(filenamePtr);
        
        // Create byte array from Unity's byte array
        var bytes = new Uint8Array(size);
        for (var i = 0; i < size; i++) {
            bytes[i] = HEAPU8[contentPtr + i];
        }
        
        var blob = new Blob([bytes], { type: 'application/zip' });
        var url = URL.createObjectURL(blob);
        
        var a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        
        setTimeout(function() {
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    }
});