// Decompiled with JetBrains decompiler
// Type: DuckGame.WireButtonTop
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", true)]
  public class WireButtonTop : MaterialThing
  {
    private WireButton _button;
    private int _orientation;

    public WireButtonTop(float xpos, float ypos, WireButton b, int orientation)
      : base(xpos, ypos)
    {
      this._button = b;
      this._orientation = orientation;
      switch (orientation)
      {
        case 0:
          this.collisionSize = new Vec2(12f, 4f);
          this.collisionOffset = new Vec2(-6f, -2f);
          break;
        case 1:
          this.collisionSize = new Vec2(4f, 12f);
          this.collisionOffset = new Vec2(-2f, -6f);
          break;
        case 2:
          this.collisionSize = new Vec2(12f, 4f);
          this.collisionOffset = new Vec2(-6f, -2f);
          break;
        case 3:
          this.collisionSize = new Vec2(4f, 12f);
          this.collisionOffset = new Vec2(-2f, -6f);
          break;
      }
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (with is PhysicsObject)
      {
        if (this._orientation == 0 && (double) with.vSpeed > -0.100000001490116)
          this._button.ButtonPressed(with as PhysicsObject);
        else if (this._orientation == 1 && (double) with.hSpeed < 0.100000001490116)
          this._button.ButtonPressed(with as PhysicsObject);
        else if (this._orientation == 2 && (double) with.vSpeed < 0.100000001490116)
          this._button.ButtonPressed(with as PhysicsObject);
        else if (this._orientation == 3 && (double) with.hSpeed > -0.100000001490116)
          this._button.ButtonPressed(with as PhysicsObject);
      }
      base.OnSoftImpact(with, from);
    }
  }
}
