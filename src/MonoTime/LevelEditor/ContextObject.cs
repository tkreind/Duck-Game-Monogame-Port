// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ContextObject : ContextMenu
  {
    private IReadOnlyPropertyBag _thingBag;
    private Thing _thing;
    private Sprite _image;
    private bool _placement;

    public Thing thing => this._thing;

    public ContextObject(Thing thing, IContextListener owner, bool placement = true)
      : base(owner)
    {
      this._placement = placement;
      this._thing = thing;
      this._image = thing.GeneratePreview();
      this.itemSize.y = 16f;
      this._text = thing.editorName;
      this.itemSize.x = (float) (this._text.Length * 8 + 26);
      this._thingBag = ContentProperties.GetBag(thing.GetType());
      if (Main.isDemo && !this._thingBag.GetOrDefault<bool>("isInDemo", false))
        this.greyOut = true;
      else
        this.greyOut = false;
    }

    public override void Selected()
    {
      if (Main.isDemo && !this._thingBag.GetOrDefault<bool>("isInDemo", false))
        return;
      if (this._placement)
      {
        if (!(Level.current is Editor current))
          return;
        current.placementType = this._thing;
        current.CloseMenu();
        SFX.Play("lowClick", 0.3f);
      }
      else
      {
        if (this._owner == null)
          return;
        this._owner.Selected((ContextMenu) this);
      }
    }

    public override void Draw()
    {
      if (this._hover && !this.greyOut)
        Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70), this.depth + 1);
      Color color = Color.White;
      if (this.greyOut)
        color = Color.White * 0.3f;
      Graphics.DrawFancyString(this._text, this.position + new Vec2(22f, 4f), color, this.depth + 2);
      this._image.depth = this.depth + 3;
      this._image.x = this.x + 1f;
      this._image.y = this.y;
      this._image.color = color;
      this._image.Draw();
    }
  }
}
