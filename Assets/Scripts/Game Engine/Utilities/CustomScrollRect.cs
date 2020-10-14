using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    private float verticalScrollbarHandlerSize;
    private float horizontalScrollbarHandlerSize;

    override protected void LateUpdate()
    {
        if (this.horizontalScrollbar)
            horizontalScrollbarHandlerSize = this.horizontalScrollbar.size;
        if (this.verticalScrollbar)
            verticalScrollbarHandlerSize = this.verticalScrollbar.size;

        base.LateUpdate();

        if (this.horizontalScrollbar)
            this.horizontalScrollbar.size = horizontalScrollbarHandlerSize;
        if (this.verticalScrollbar)
            this.verticalScrollbar.size = verticalScrollbarHandlerSize;
    }

    override public void Rebuild(CanvasUpdate executing)
    {
        if (this.horizontalScrollbar)
            horizontalScrollbarHandlerSize = this.horizontalScrollbar.size;
        if (this.verticalScrollbar)
            verticalScrollbarHandlerSize = this.verticalScrollbar.size;

        base.Rebuild(executing);

        if (this.horizontalScrollbar)
            this.horizontalScrollbar.size = horizontalScrollbarHandlerSize;
        if (this.verticalScrollbar)
            this.verticalScrollbar.size = verticalScrollbarHandlerSize;
    }
}