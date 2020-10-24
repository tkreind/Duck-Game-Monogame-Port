// Decompiled with JetBrains decompiler
// Type: DuckGame.RCControlBolt
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class RCControlBolt : Thing
  {
    private bool _fade;
    private RCCar _control;

    public RCCar control => this._control;

    public RCControlBolt(float xval, float yval, RCCar c)
      : base(xval, yval)
    {
      this._control = c;
      this.graphic = new Sprite("rcBolt");
      this.center = new Vec2(8f, 8f);
      this.scale = new Vec2(0.3f, 0.3f);
      this.alpha = 1f;
    }

    public override void Update()
    {
      Vec2 vec2 = this._control.position - this.position;
      float length = vec2.length;
      vec2.Normalize();
      this.angleDegrees = (float) (-(double) Maths.PointDirection(this.position, this._control.position) + 90.0);
      RCControlBolt rcControlBolt = this;
      rcControlBolt.position = rcControlBolt.position + vec2 * 8f;
      this.xscale = this.yscale = Lerp.Float(this.xscale, 1f, 0.1f);
      if ((double) length < 48.0 || this._control.destroyed || !this._control.receivingSignal)
        this._fade = true;
      this.alpha = Lerp.Float(this.alpha, this._fade ? 0.0f : 1f, 0.1f);
      if ((double) this.alpha < 0.00999999977648258 && this._fade)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
