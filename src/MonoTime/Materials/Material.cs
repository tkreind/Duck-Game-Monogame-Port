// Decompiled with JetBrains decompiler
// Type: DuckGame.Material
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class Material
  {
    protected MTEffect _effect;

    public MTEffect effect => this._effect;

    public Material()
    {
    }

    public Material(string mat) => this._effect = Content.Load<MTEffect>(mat);

    public Material(Effect e) => this._effect = (MTEffect) e;

    public virtual void SetValue(string name, float value) => this._effect.effect.Parameters[name].SetValue(value);

    public virtual void SetValue(string name, Vec2 value) => this._effect.effect.Parameters[name].SetValue((Vector2) value);

    public virtual void SetValue(string name, Matrix value) => this._effect.effect.Parameters[name].SetValue((Microsoft.Xna.Framework.Matrix) value);

    public virtual void Update()
    {
    }

    public virtual void Apply()
    {
      foreach (EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
        pass.Apply();
    }

    public static implicit operator MTEffect(Material val) => val.effect;
  }
}
