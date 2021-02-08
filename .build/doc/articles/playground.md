---
_disableAffix: true
---
<style>
    #wrapper
    {
        display: flex;
        flex-direction: column;
        height: 100%;
    }

    [role="main"]
    {
        display: flex;
        flex: 1;
        min-height: 0;
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

    #mux-xaml
    {
        flex: 1;
        font-family: monospace;
    }
</style>
<div id="mux-container"><canvas></canvas></div>
<div id="mux-xaml"></div>
<script>
    window.__webpack_public_path__ = "playground/playground/";
    const mux =
    [
        {
            viewNode: document.getElementById("mux-container").lastChild,
            editorNode: document.getElementById("mux-xaml"),
            value: `<m:RectTransform
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:m="clr-namespace:Mux.Markup;assembly=Mux.Markup"
    xmlns:mu="clr-namespace:Mux.Markup;assembly=Mux.Markup.UI"
    xmlns:mue="clr-namespace:Mux.Markup.Extras;assembly=Mux.Markup.UI"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:playground="Mux.Playground"
    x:DataType="playground:PlaygroundViewModel">
    <mu:StandaloneInputModule />
    <mu:Canvas />
    <mu:CanvasScaler UiScale="{mu:ConstantPhysicalSize}" />
    <mu:GraphicRaycaster />
    <mu:Text Font="{Binding Path=Resources.Font}">
        <mu:Text.Content>
See console for logs.
You may also check "The playground and caveats" section in the home page.
        </mu:Text.Content>
    </mu:Text>
</m:RectTransform>
`
        }
    ];
</script>
<script src="playground/playground/playground.loader.js"></script>
<script async src="playground/playground/main.js"></script>
