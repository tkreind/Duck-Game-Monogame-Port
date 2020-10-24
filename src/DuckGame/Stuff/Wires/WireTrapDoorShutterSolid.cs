// Decompiled with JetBrains decompiler
// Type: DuckGame.WireTrapDoorShutterSolid
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WireTrapDoorShutterSolid : Block
  {
    private WireTrapDoor _button;
    private bool _open;

    public WireTrapDoorShutterSolid(float xpos, float ypos, WireTrapDoor b)
      : base(xpos, ypos)
    {
      this._button = b;
      this.collisionSize = new Vec2(39f, 12f);
      this.collisionOffset = new Vec2(-3f, -3f);
      this.center = new Vec2(3f, 3f);
      this.graphic = new Sprite("wireTrapDoorArmBig");
    }

    public override void Update()
    {
      if (this._open && (double) this.angleDegrees == 0.0)
      {
        this.collisionSize = new Vec2(39f, 12f);
        this.collisionOffset = new Vec2(-3f, -3f);
        this._open = false;
      }
      else if (!this._open && (double) this.angleDegrees != 0.0)
      {
        foreach (PhysicsObject physicsObject in Level.CheckRectAll<PhysicsObject>(this.topLeft + new Vec2(0.0f, -8f), this.bottomRight))
          physicsObject.sleeping = false;
        this.collisionSize = new Vec2(1f, 1f);
        this.collisionOffset = new Vec2(-3f, -3f);
        this._open = true;
      }
      base.Update();
    }

    public override void Draw()
    {
      this.graphic.flipH = this.offDir < (sbyte) 0;
      base.Draw();
    }
  }
}
