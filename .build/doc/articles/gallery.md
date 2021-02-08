---
_disableAffix: true
---
<style>
    #wrapper
    {
        height: 100%;
    }

    [role="main"]
    {
        display: flex;
        height: 100%;
    }

    .article
    {
        width: 100%;
    }

    .article > *
    {
        height: 100%;
    }

    .content
    {
        display: flex;
        height: 100%;
    }

    #mux-container
    {
        flex: 1;
    }

    #mux-container > *
    {
      width: 100%;
      height: 100%;
    }
</style>
<div id="mux-container"><canvas></canvas></div>
<script src="playground/gallery/gallery.loader.js"></script>
<script>
    createUnityInstance(document.getElementById("mux-container").lastChild, {
        dataUrl: "playground/gallery/gallery.data",
        frameworkUrl: "playground/gallery/gallery.framework.js",
        codeUrl: "playground/gallery/gallery.wasm",
        symbolsUrl: "playground/gallery/gallery.symbols.json",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "pixiv",
        productName: "Mux Documentation",
        productVersion: "1.0",
    });
</script>
