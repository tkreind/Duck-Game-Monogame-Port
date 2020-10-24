// Decompiled with JetBrains decompiler
// Type: DuckGame.ControlWave
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class ControlWave : Thing, ITeleport
  {
    public StateBinding _positionBinding = (StateBinding) new InterpolatedVec2Binding("position");
    public StateBinding _angleBinding = (StateBinding) new CompressedFloatBinding("_angle", 0.0f, 8, true);
    public StateBinding _alphaBinding = (StateBinding) new CompressedFloatBinding("alpha", 1f, 8, false);
    private MindControlRay _owner;
    private float _fade = 1f;
    private bool _isNotControlRay;

    public ControlWave(float xpos, float ypos, float dir, MindControlRay owner)
      : base(xpos, ypos)
    {
      this._owner = owner;
      this.graphic = new Sprite("controlWave");
      this.graphic.flipH = this.offDir < (sbyte) 0;
      this.center = new Vec2(8f, 8f);
      this.xscale = this.yscale = 0.2f;
      this.angle = dir;
    }

    public override void Update()
    {
      if (this.isServerForObject)
      {
        this.xscale = this.yscale = Maths.CountUp(this.yscale, 0.05f);
        this._fade -= 0.05f;
        if ((double) this._fade < 0.0)
          Level.Remove((Thing) this);
        this.alpha = Maths.NormalizeSection(this._fade, 0.2f, 0.3f);
        Vec2 p2 = Vec2.Zero;
        if (this._owner.controlledDuck == null && !this._isNotControlRay)
        {
          p2 = new Vec2((float) Math.Cos((double) this.angle), (float) -Math.Sin((double) this.angle));
          foreach (IAmADuck amAduck in Level.CheckCircleAll<IAmADuck>(this.position, 3f))
          {
            switch (amAduck)
            {
              case Duck _:
                if (!(amAduck as Duck).HasEquipment(typeof (TinfoilHat)) && !((amAduck as Duck).holdObject is MindControlRay))
                {
                  this._owner.ControlDuck(amAduck as Duck);
                  continue;
                }
                continue;
              case RagdollPart _ when (amAduck as RagdollPart).doll.captureDuck != null:
                this._owner.ControlDuck((amAduck as RagdollPart).doll.captureDuck);
                continue;
              case TrappedDuck _ when (amAduck as TrappedDuck).captureDuck != null:
                this._owner.ControlDuck((amAduck as TrappedDuck).captureDuck);
                continue;
              default:
                continue;
            }
          }
        }
        else
        {
          if (this._owner.controlledDuck != null)
          {
            p2 = this._owner.controlledDuck.cameraPosition - this.position;
            p2.Normalize();
            this.angleDegrees = -Maths.PointDirection(Vec2.Zero, p2);
          }
          this._isNotControlRay = true;
        }
        ControlWave controlWave = this;
        controlWave.position = controlWave.position + p2 * 2.6f;
      }
      else
      {
        this.xscale = this.yscale = 1f;
        Vec2 vec2 = new Vec2((float) Math.Cos((double) this.angle), (float) -Math.Sin((double) this.angle));
        ControlWave controlWave = this;
        controlWave.position = controlWave.position + vec2 * 2.6f;
      }
    }
  }
}
