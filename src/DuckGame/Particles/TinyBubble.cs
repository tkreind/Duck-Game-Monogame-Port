// Decompiled with JetBrains decompiler
// Type: DuckGame.TinyBubble
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class TinyBubble : PhysicsParticle
  {
    private SinWave _wave = new SinWave(0.1f + Rando.Float(0.1f), Rando.Float(3f));
    private float _minY;
    private float _waveSize = 1f;

    public TinyBubble(float xpos, float ypos, float startHSpeed, float minY, bool blue = false)
      : base(xpos, ypos)
    {
      this.alpha = 0.7f;
      this._minY = minY;
      this._gravMult = 0.0f;
      this.vSpeed = -Rando.Float(0.5f, 1f);
      this.hSpeed = startHSpeed;
      this.depth = new Depth(0.3f);
      SpriteMap spriteMap = new SpriteMap("tinyBubbles", 8, 8);
      if (blue)
        spriteMap = new SpriteMap("tinyBlueBubbles", 8, 8);
      spriteMap.frame = Rando.Int(0, 1);
      this.graphic = (Sprite) spriteMap;
      this.center = new Vec2(4f, 4f);
      this._waveSize = Rando.Float(0.1f, 0.3f);
      this.xscale = this.yscale = 0.1f;
    }

    public override void Update()
    {
      this.position.x += this._wave.value * this._waveSize;
      this.position.x += this.hSpeed;
      this.position.y += this.vSpeed;
      this.hSpeed = Lerp.Float(this.hSpeed, 0.0f, 0.1f);
      this.xscale = this.yscale = Lerp.Float(this.xscale, 1f, 0.1f);
      if ((double) this.y < (double) this._minY - 4.0)
        this.alpha -= 0.025f;
      if ((double) this.y < (double) this._minY - 8.0)
        this.alpha = 0.0f;
      if ((double) this.y >= (double) this._minY)
        return;
      this.alpha -= 0.025f;
      if ((double) this.alpha >= 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
