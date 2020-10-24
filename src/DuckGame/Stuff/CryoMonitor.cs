// Decompiled with JetBrains decompiler
// Type: DuckGame.CryoMonitor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("survival")]
  public class CryoMonitor : Thing
  {
    public CryoMonitor(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("survival/cryoMonitor");
      this.center = new Vec2((float) (this.graphic.w / 2), (float) (this.graphic.h / 2));
      this._collisionSize = new Vec2(32f, 32f);
      this._collisionOffset = new Vec2(-16f, -16f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
    }
  }
}
