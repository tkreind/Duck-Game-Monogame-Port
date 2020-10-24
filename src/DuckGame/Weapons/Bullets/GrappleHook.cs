// Decompiled with JetBrains decompiler
// Type: DuckGame.GrappleHook
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class GrappleHook : PhysicsObject
  {
    private Grapple _owner;
    private bool _inGun = true;
    private bool _stuck;

    public bool inGun => this._inGun;

    public GrappleHook(Grapple ownerVal)
      : base(0.0f, 0.0f)
    {
      this._owner = ownerVal;
      this.graphic = new Sprite("harpoon");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-5f, -1.5f);
      this.collisionSize = new Vec2(10f, 5f);
    }

    public override void Update()
    {
      if (!this._stuck)
        base.Update();
      if (!this._inGun)
        return;
      this.position = this._owner.barrelPosition;
      this.depth = this._owner.depth - 1;
      this.hSpeed = 0.0f;
      this.vSpeed = 0.0f;
      this.graphic.flipH = (double) this._owner.offDir < 0.0;
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (this._inGun || !(with is Block))
        return;
      this._stuck = true;
    }

    public void Fire()
    {
      if (!this._inGun)
        return;
      this._inGun = false;
      this.hSpeed = (float) this._owner.offDir * 6f;
      this.vSpeed = -8f;
    }

    public void Return()
    {
      if (this._inGun)
        return;
      this._inGun = true;
      this.hSpeed = 0.0f;
      this.vSpeed = 0.0f;
      this._stuck = false;
    }

    public override void Draw() => base.Draw();
  }
}
