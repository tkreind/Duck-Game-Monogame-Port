// Decompiled with JetBrains decompiler
// Type: DuckGame.MetalRebound
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class MetalRebound : Thing
  {
    private static int kMaxObjects = 32;
    private static MetalRebound[] _objects = new MetalRebound[MetalRebound.kMaxObjects];
    private static int _lastActiveObject = 0;
    private SpriteMap _sprite;

    public static MetalRebound New(float xpos, float ypos, int offDir)
    {
      MetalRebound metalRebound;
      if (MetalRebound._objects[MetalRebound._lastActiveObject] == null)
      {
        metalRebound = new MetalRebound();
        MetalRebound._objects[MetalRebound._lastActiveObject] = metalRebound;
      }
      else
        metalRebound = MetalRebound._objects[MetalRebound._lastActiveObject];
      MetalRebound._lastActiveObject = (MetalRebound._lastActiveObject + 1) % MetalRebound.kMaxObjects;
      metalRebound.Init(xpos, ypos, offDir);
      metalRebound.ResetProperties();
      return metalRebound;
    }

    public MetalRebound()
      : base()
    {
      this._sprite = new SpriteMap("metalRebound", 16, 16);
      this.graphic = (Sprite) this._sprite;
    }

    private void Init(float xpos, float ypos, int offDir)
    {
      this.position.x = xpos;
      this.position.y = ypos;
      this.alpha = 1f;
      this._sprite.frame = Rando.Int(3);
      this._sprite.flipH = offDir < 0;
      this.center = new Vec2(16f, 8f);
    }

    public override void Update()
    {
      this.alpha -= 0.1f;
      if ((double) this.alpha >= 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
