// Decompiled with JetBrains decompiler
// Type: DuckGame.PopEffect
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class PopEffect : Thing
  {
    private List<PopEffect.PopEffectPart> parts = new List<PopEffect.PopEffectPart>();
    private SpriteMap _sprite;

    public PopEffect(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("popLine", 16, 16);
      this.center = new Vec2((float) (this._sprite.w / 2), (float) (this._sprite.h / 2));
      this.graphic = (Sprite) this._sprite;
      int num1 = 8;
      for (int index = 0; index < num1; ++index)
      {
        float num2 = 360f / (float) num1 * (float) index;
        this.parts.Add(new PopEffect.PopEffectPart()
        {
          scale = Rando.Float(0.6f, 1f),
          rot = Maths.DegToRad(num2 + Rando.Float(-10f, 10f))
        });
      }
      this.depth = (Depth) 0.9f;
    }

    public override void Update()
    {
      this.xscale -= 0.2f;
      this.yscale = this.xscale;
      if ((double) this.xscale >= 0.00999999977648258)
        return;
      Level.Remove((Thing) this);
    }

    public override void Draw()
    {
      foreach (PopEffect.PopEffectPart part in this.parts)
      {
        this._sprite.angle = part.rot;
        this._sprite.xscale = this._sprite.yscale = this.xscale * part.scale;
        this._sprite.center = new Vec2((float) (this._sprite.w / 2), (float) (this._sprite.h / 2));
        Graphics.Draw((Sprite) this._sprite, this.x, this.y);
      }
      base.Draw();
    }

    public class PopEffectPart
    {
      public float scale;
      public float rot;
    }
  }
}
