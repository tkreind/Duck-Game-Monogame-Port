// Decompiled with JetBrains decompiler
// Type: DuckGame.CryoTube
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("survival")]
  public class CryoTube : Thing
  {
    private CryoPlug _plug;

    public CryoTube(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("survival/cryoTube");
      this.center = new Vec2(16f, 15f);
      this._collisionSize = new Vec2(18f, 32f);
      this._collisionOffset = new Vec2(-9f, -16f);
      this.depth = (Depth) 0.9f;
      this.hugWalls = WallHug.Floor;
    }

    public override void Initialize()
    {
      this._plug = new CryoPlug(this.x - 20f, this.y);
      Level.Add((Thing) this._plug);
      this._plug.AttachTo((Thing) this);
    }

    public override void Terminate() => Level.Remove((Thing) this._plug);
  }
}
