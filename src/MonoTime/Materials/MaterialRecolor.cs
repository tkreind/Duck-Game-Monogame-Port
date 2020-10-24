// Decompiled with JetBrains decompiler
// Type: DuckGame.MaterialRecolor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;

namespace DuckGame
{
  public class MaterialRecolor : Material
  {
    public Vec3 color;

    public MaterialRecolor(Vec3 col)
    {
      this.color = col;
      this._effect = Content.Load<MTEffect>("Shaders/recolor");
    }

    public override void Update()
    {
    }

    public override void Apply() => this._effect.effect.Parameters["fcol"].SetValue((Vector3) this.color);
  }
}
