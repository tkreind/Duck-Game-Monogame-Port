// Decompiled with JetBrains decompiler
// Type: DuckGame.Sprite
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;

namespace DuckGame
{
  public class Sprite : Transform, ICloneable<Sprite>, ICloneable
  {
    private int _globalIndex = (int) Thing.GetGlobalIndex();
    protected Tex2D _texture;
    protected RenderTarget2D _renderTexture;
    protected bool _flipH;
    protected bool _flipV;
    protected Color _color = Color.White;

    public int globalIndex => this._globalIndex;

    public Tex2D texture
    {
      get => this._texture;
      set => this._texture = value;
    }

    public RenderTarget2D renderTexture
    {
      get => this._renderTexture;
      set => this._renderTexture = value;
    }

    public virtual int width => this._texture.width;

    public virtual int w => this.width;

    public virtual int height => this._texture.height;

    public virtual int h => this.height;

    public bool flipH
    {
      get => this._flipH;
      set => this._flipH = value;
    }

    public bool flipV
    {
      get => this._flipV;
      set => this._flipV = value;
    }

    public float flipMultH => !this._flipH ? 1f : -1f;

    public float flipMultV => !this._flipV ? 1f : -1f;

    public Color color
    {
      get => this._color;
      set => this._color = value;
    }

    public void CenterOrigin() => this.center = new Vec2((float) Math.Round((double) this.width / 2.0), (float) Math.Round((double) this.height / 2.0));

    public Sprite()
    {
    }

    public Sprite(Tex2D tex, float x = 0.0f, float y = 0.0f)
    {
      this._texture = tex;
      this.position = new Vec2(x, y);
    }

    public Sprite(RenderTarget2D tex, float x = 0.0f, float y = 0.0f)
    {
      this._texture = (Tex2D) tex;
      this._renderTexture = tex;
      this.position = new Vec2(x, y);
    }

    public Sprite(string tex, float x = 0.0f, float y = 0.0f)
    {
      this._texture = Content.Load<Tex2D>(tex);
      this.position = new Vec2(x, y);
    }

    public virtual void Draw()
    {
      this._texture.currentObjectIndex = this._globalIndex;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(), this._color * this.alpha, this.angle, this.center, this.scale, this._flipH ? SpriteEffects.FlipHorizontally : (this._flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), this.depth);
    }

    public virtual void Draw(Rectangle r)
    {
      this._texture.currentObjectIndex = this._globalIndex;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(r), this._color * this.alpha, this.angle, this.center, this.scale, this._flipH ? SpriteEffects.FlipHorizontally : (this._flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), this.depth);
    }

    public virtual void CheapDraw(bool flipH)
    {
    }

    public virtual Sprite Clone()
    {
      Sprite sprite = new Sprite(this._texture);
      sprite.flipH = this._flipH;
      sprite.flipV = this._flipV;
      sprite.position = this.position;
      sprite.scale = this.scale;
      sprite.center = this.center;
      sprite.depth = this.depth;
      sprite.alpha = this.alpha;
      sprite.angle = this.angle;
      sprite.color = this.color;
      return sprite;
    }

    public virtual void UltraCheapStaticDraw(bool flipH)
    {
    }

    object ICloneable.Clone() => (object) this.Clone();
  }
}
