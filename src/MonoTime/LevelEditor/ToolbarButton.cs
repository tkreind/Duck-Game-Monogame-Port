// Decompiled with JetBrains decompiler
// Type: DuckGame.ToolbarButton
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ToolbarButton : Thing
  {
    private ContextToolbarItem _owner;
    private bool _hover;
    public string hoverText = "";

    public bool hover
    {
      get => this._hover;
      set => this._hover = value;
    }

    public ToolbarButton(ContextToolbarItem owner, int image, string ht)
      : base()
    {
      this._owner = owner;
      this.layer = Layer.HUD;
      if (image == 99)
      {
        this.graphic = new Sprite("steamIcon");
        this.graphic.scale = new Vec2(0.5f, 0.5f);
      }
      else
        this.graphic = (Sprite) new SpriteMap("iconSheet", 16, 16)
        {
          frame = image
        };
      this.hoverText = ht;
      this.depth = new Depth(0.88f);
    }

    public override void Update()
    {
      if (Editor.gamepadMode)
        return;
      if ((double) Mouse.x > (double) this.x && (double) Mouse.x < (double) this.x + 16.0 && ((double) Mouse.y > (double) this.y && (double) Mouse.y < (double) this.y + 16.0))
      {
        this._hover = true;
        if (Mouse.left != InputState.Pressed)
          return;
        Level level = this._level;
        Editor.clickedMenu = true;
        this._owner.ButtonPressed(this);
      }
      else
        this._hover = false;
    }

    public override void Draw()
    {
      Graphics.DrawRect(this.position, this.position + new Vec2(16f, 16f), this._hover ? new Color(170, 170, 170) : new Color(70, 70, 70), new Depth(0.87f));
      this.graphic.position = this.position;
      this.graphic.depth = new Depth(0.88f);
      this.graphic.Draw();
    }
  }
}
