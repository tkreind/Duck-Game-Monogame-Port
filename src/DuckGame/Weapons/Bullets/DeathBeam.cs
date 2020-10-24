// Decompiled with JetBrains decompiler
// Type: DuckGame.DeathBeam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DeathBeam : Thing
  {
    public float _blast = 1f;
    private Vec2 _target;

    public DeathBeam(Vec2 pos, Vec2 target)
      : base(pos.x, pos.y)
      => this._target = target;

    public override void Initialize()
    {
      Vec2 normalized1 = this._target.Rotate(Maths.DegToRad(-90f), Vec2.Zero).normalized;
      Vec2 normalized2 = this._target.Rotate(Maths.DegToRad(90f), Vec2.Zero).normalized;
      Level.Add((Thing) new LaserLine(this.position, this._target, normalized1, 4f, Color.White, 1f));
      Level.Add((Thing) new LaserLine(this.position, this._target, normalized2, 4f, Color.White, 1f));
      Level.Add((Thing) new LaserLine(this.position, this._target, normalized1, 2.5f, Color.White, 2f));
      Level.Add((Thing) new LaserLine(this.position, this._target, normalized2, 2.5f, Color.White, 2f));
      if (this.isLocal)
      {
        int num = 0;
        Vec2 vec2 = this.position + normalized1 * 16f;
        for (int index = 0; index < 5; ++index)
        {
          Vec2 p1 = vec2 + normalized2 * 8f * (float) index;
          foreach (MaterialThing materialThing in Level.CheckLineAll<MaterialThing>(p1, p1 + this._target))
          {
            if (materialThing is IAmADuck && !materialThing.destroyed && !(materialThing is TargetDuck))
            {
              if (materialThing is RagdollPart && (materialThing as RagdollPart).doll != null && ((materialThing as RagdollPart).doll.captureDuck != null && !(materialThing as RagdollPart).doll.captureDuck.dead))
                ++num;
              if (materialThing is TrappedDuck && (materialThing as TrappedDuck).captureDuck != null && !(materialThing as TrappedDuck).captureDuck.dead)
                ++num;
              if (materialThing is Duck && !(materialThing as Duck).dead)
                ++num;
            }
            materialThing.Destroy((DestroyType) new DTIncinerate((Thing) this));
          }
        }
        if (num > 2)
          Steam.SetAchievement("laser");
        Global.data.giantLaserKills += num;
      }
      if (Recorder.currentRecording == null)
        return;
      Recorder.currentRecording.LogBonus();
    }

    public override void Update()
    {
      this._blast = Maths.CountDown(this._blast, 0.1f);
      if ((double) this._blast >= 0.0)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      double num1 = (double) Maths.NormalizeSection(this._blast, 0.0f, 0.2f);
      double num2 = (double) Maths.NormalizeSection(this._blast, 0.6f, 1f);
      double blast = (double) this._blast;
      Vec2 normalized1 = this._target.Rotate(Maths.DegToRad(-90f), Vec2.Zero).normalized;
      Vec2 normalized2 = this._target.Rotate(Maths.DegToRad(90f), Vec2.Zero).normalized;
      Vec2 vec2 = this.position + normalized1 * 16f;
      for (int index = 0; index < 5; ++index)
      {
        Vec2 p1 = vec2 + normalized2 * 8f * (float) index;
        Graphics.DrawLine(p1, p1 + this._target, Color.Red * (this._blast * 0.5f), 2f, (Depth) 0.9f);
      }
    }
  }
}
