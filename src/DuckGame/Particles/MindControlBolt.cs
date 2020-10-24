// Decompiled with JetBrains decompiler
// Type: DuckGame.MindControlBolt
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class MindControlBolt : Thing
  {
    private bool _fade;
    private Duck _controlledDuck;

    public Duck controlledDuck => this._controlledDuck;

    public MindControlBolt(float xval, float yval, Duck control)
      : base(xval, yval)
    {
      this._controlledDuck = control;
      this.graphic = new Sprite("mindBolt");
      this.center = new Vec2(8f, 8f);
      this.scale = new Vec2(0.1f, 0.1f);
      this.alpha = 0.0f;
    }

    public override void Update()
    {
      Vec2 position = this._controlledDuck.position;
      if (this._controlledDuck.ragdoll != null)
        position = this._controlledDuck.ragdoll.part3.position;
      Vec2 vec2 = position - this.position;
      float length = vec2.length;
      vec2.Normalize();
      this.angleDegrees = (float) (-(double) Maths.PointDirection(this.position, position) + 90.0);
      MindControlBolt mindControlBolt = this;
      mindControlBolt.position = mindControlBolt.position + vec2 * 4f;
      this.xscale = this.yscale = Lerp.Float(this.xscale, 1f, 0.05f);
      if ((double) length < 48.0 || this._controlledDuck.mindControl == null)
        this._fade = true;
      this.alpha = Lerp.Float(this.alpha, this._fade ? 0.0f : 1f, 0.1f);
      if ((double) this.alpha < 0.00999999977648258 && this._fade)
        Level.Remove((Thing) this);
      base.Update();
    }
  }
}
