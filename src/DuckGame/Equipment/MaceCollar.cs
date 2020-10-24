// Decompiled with JetBrains decompiler
// Type: DuckGame.MaceCollar
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("equipment")]
  public class MaceCollar : ChokeCollar
  {
    public MaceCollar(float xpos, float ypos)
      : base(xpos, ypos)
    {
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._ball = new WeightBall(this.x, this.y, (PhysicsObject) this, (ChokeCollar) this, true);
      this.ReturnItemToWorld((Thing) this._ball);
      Level.Add((Thing) this._ball);
    }
  }
}
