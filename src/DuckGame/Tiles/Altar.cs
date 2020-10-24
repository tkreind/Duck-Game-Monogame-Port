// Decompiled with JetBrains decompiler
// Type: DuckGame.Altar
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|pyramid")]
  public class Altar : Platform
  {
    public EditorProperty<int> wide;
    public bool kill;
    public Platform leftPlat;
    public Platform rightPlat;

    public override void EditorPropertyChanged(object property)
    {
      (this.graphic as SpriteMap).frame = this.wide.value - 1;
      this.UpdateSize();
    }

    public void UpdateSize()
    {
      if ((this.graphic as SpriteMap).frame == 0)
      {
        this.center = new Vec2(8f, 8f);
        this.collisionSize = new Vec2(12f, 13f);
        this.collisionOffset = new Vec2(-6f, -5f);
      }
      else if ((this.graphic as SpriteMap).frame == 1)
      {
        this.center = new Vec2(16f, 8f);
        this.collisionSize = new Vec2(28f, 13f);
        this.collisionOffset = new Vec2(-14f, -5f);
      }
      else
      {
        if ((this.graphic as SpriteMap).frame != 2)
          return;
        this.center = new Vec2(24f, 8f);
        this.collisionSize = new Vec2(44f, 13f);
        this.collisionOffset = new Vec2(-22f, -5f);
      }
    }

    public Altar(float xpos, float ypos, int dir)
      : base(xpos, ypos)
    {
      this.wide = new EditorProperty<int>(1, (Thing) this, 1f, 3f, 1f);
      this.graphic = (Sprite) new SpriteMap("altar", 48, 16);
      this.hugWalls = WallHug.Floor;
      this.UpdateSize();
      this.thickness = 0.0f;
      this.depth = new Depth(0.01f);
      this.placementLayerOverride = Layer.Blocks;
    }

    public override void Draw()
    {
      this.flipHorizontal = false;
      base.Draw();
    }

    public override void Terminate()
    {
      if (this.leftPlat != null)
        Level.Remove((Thing) this.leftPlat);
      if (this.rightPlat != null)
        Level.Remove((Thing) this.rightPlat);
      base.Terminate();
    }
  }
}
