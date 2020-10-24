// Decompiled with JetBrains decompiler
// Type: DuckGame.WoodDebris
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WoodDebris : PhysicsParticle
  {
    private static int kMaxObjects = 64;
    private static WoodDebris[] _objects = new WoodDebris[WoodDebris.kMaxObjects];
    private static int _lastActiveObject = 0;
    private SpriteMap _sprite;

    public static WoodDebris New(float xpos, float ypos)
    {
      WoodDebris woodDebris;
      if (WoodDebris._objects[WoodDebris._lastActiveObject] == null)
      {
        woodDebris = new WoodDebris();
        WoodDebris._objects[WoodDebris._lastActiveObject] = woodDebris;
      }
      else
        woodDebris = WoodDebris._objects[WoodDebris._lastActiveObject];
      WoodDebris._lastActiveObject = (WoodDebris._lastActiveObject + 1) % WoodDebris.kMaxObjects;
      woodDebris.ResetProperties();
      woodDebris.Init(xpos, ypos);
      woodDebris._sprite.globalIndex = (int) Thing.GetGlobalIndex();
      woodDebris.globalIndex = Thing.GetGlobalIndex();
      return woodDebris;
    }

    public WoodDebris()
      : base(0.0f, 0.0f)
    {
      this._sprite = new SpriteMap("woodDebris", 8, 8);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(4f, 4f);
    }

    private void Init(float xpos, float ypos)
    {
      this.position.x = xpos;
      this.position.y = ypos;
      this.hSpeed = -4f - Rando.Float(3f);
      this.vSpeed = (float) -((double) Rando.Float(1.5f) + 1.0);
      this._sprite.frame = Rando.Int(4);
      this._bounceEfficiency = 0.3f;
    }

    public override void Update() => base.Update();
  }
}
