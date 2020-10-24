// Decompiled with JetBrains decompiler
// Type: DuckGame.PowerSocket
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("survival")]
  [BaggedProperty("isOnlineCapable", false)]
  public class PowerSocket : Thing
  {
    public PowerSocket(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("survival/cryoSocket");
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(14f, 14f);
      this._collisionOffset = new Vec2(-7f, -7f);
      this.depth = new Depth(-0.9f);
    }
  }
}
