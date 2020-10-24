// Decompiled with JetBrains decompiler
// Type: DuckGame.DoorOffHinges
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DoorOffHinges : PhysicsObject
  {
    public StateBinding _throwSpinBinding = new StateBinding(nameof (_throwSpin));
    public StateBinding _secondaryBinding = new StateBinding(nameof (_secondaryFrame));
    public bool _secondaryFrame;
    public float _throwSpin;
    private bool sounded;

    public DoorOffHinges(float xpos, float ypos, bool secondaryFrame)
      : base(xpos, ypos)
    {
      this._secondaryFrame = secondaryFrame;
      this._collisionSize = new Vec2(8f, 8f);
      this._collisionOffset = new Vec2(-4f, -6f);
      this.center = new Vec2(16f, 16f);
      this.collideSounds.Add("rockHitGround");
      this.weight = 2f;
    }

    public override void Initialize()
    {
      this.graphic = (Sprite) new SpriteMap(this._secondaryFrame ? "flimsyDoorDamaged" : "doorFucked", 32, 32);
      base.Initialize();
    }

    public void MakeEffects()
    {
      if (this.sounded)
        return;
      Level.Add((Thing) SmallSmoke.New(this.x, this.y + 2f));
      Level.Add((Thing) SmallSmoke.New(this.x, this.y - 16f));
      SFX.Play("doorBreak");
      for (int index = 0; index < 8; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(this.x - 8f + Rando.Float(16f), this.y - 8f + Rando.Float(16f));
        thing.hSpeed = ((double) Rando.Float(1f) > 0.5 ? 1f : -1f) * Rando.Float(3f);
        thing.vSpeed = -Rando.Float(1f);
        Level.Add(thing);
      }
      this.sounded = true;
    }

    public override void Update()
    {
      if (Network.isActive && !this.sounded && this.visible)
        this.MakeEffects();
      this.angleDegrees = this._throwSpin;
      this.center = new Vec2(16f, 16f);
      this._throwSpin %= 360f;
      this._throwSpin = this.offDir <= (sbyte) 0 ? Lerp.Float(this._throwSpin, -90f, 12f) : Lerp.Float(this._throwSpin, 90f, 12f);
      base.Update();
    }
  }
}
