// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomParallax
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background|parallax|custom")]
  public class CustomParallax : BackgroundUpdater
  {
    public bool didInit;

    public CustomParallax(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("backgroundIcons", 16, 16)
      {
        frame = 5
      };
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(0.9f);
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Custom Parallax";
    }

    public override void Initialize()
    {
      this.didInit = true;
      if (Level.current is Editor)
        return;
      this.backgroundColor = new Color(25, 38, 41);
      Level.current.backgroundColor = this.backgroundColor;
      CustomTileData data = Custom.GetData(0, CustomType.Parallax);
      if (data != null && data.texture != null)
      {
        this._parallax = new ParallaxBackground(data.texture);
        for (int yPos = 0; yPos < 40; ++yPos)
          this._parallax.AddZone(yPos, 0.0f, 0.0f, true);
        Level.Add((Thing) this._parallax);
      }
      else
      {
        this._parallax = new ParallaxBackground("background/office", 0.0f, 0.0f, 3);
        Level.Add((Thing) this._parallax);
      }
    }

    public override void Update()
    {
      Vec2 wallScissor = BackgroundUpdater.GetWallScissor();
      if (wallScissor != Vec2.Zero)
        this.scissor = new Rectangle((float) (int) wallScissor.x, 0.0f, (float) (int) wallScissor.y, (float) Graphics.height);
      base.Update();
    }

    public override void Terminate() => Level.Remove((Thing) this._parallax);

    public static string customParallax
    {
      get => Custom.data[CustomType.Parallax][0];
      set => Custom.data[CustomType.Parallax][0] = value;
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) null, true);
      editorGroupMenu.AddItem((ContextMenu) new ContextFile("style", (IContextListener) null, new FieldBinding((object) this, "customParallax"), ContextFileType.Parallax));
      return (ContextMenu) editorGroupMenu;
    }
  }
}
