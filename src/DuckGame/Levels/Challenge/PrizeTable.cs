﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PrizeTable
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("special|arcade")]
  [BaggedProperty("isOnlineCapable", false)]
  public class PrizeTable : Thing
  {
    private SpriteMap _sprite;
    private Sprite _outline;
    private float _hoverFade;
    private SpriteMap _light;
    private Sprite _fixture;
    private Sprite _prizes;
    private Sprite _hoverSprite;
    public bool hoverChancyChallenge;
    private DustSparkleEffect _dust;
    public bool hover;
    public bool _unlocked = true;
    private ArcadeTableLight _lighting;
    private bool _hasEligibleChallenges;

    public override bool visible
    {
      get => base.visible;
      set
      {
        base.visible = value;
        this._dust.visible = base.visible;
      }
    }

    public PrizeTable(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("arcade/prizeCounter", 69, 30);
      this.graphic = (Sprite) this._sprite;
      this.depth = (Depth) -0.5f;
      this._outline = new Sprite("arcade/prizeCounterOutline");
      this._outline.depth = this.depth + 1;
      this._outline.CenterOrigin();
      this.center = new Vec2((float) (this._sprite.width / 2), (float) (this._sprite.h / 2));
      this._collisionSize = new Vec2(16f, 15f);
      this._collisionOffset = new Vec2(-8f, 0.0f);
      this._light = new SpriteMap("arcade/prizeLights", 107, 55);
      this._fixture = new Sprite("arcade/bigFixture");
      this._prizes = new Sprite("arcade/prizes");
      this._hoverSprite = new Sprite("arcade/chancyHover");
      this.hugWalls = WallHug.Floor;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._dust = new DustSparkleEffect(this.x - 54f, this.y - 40f, true, true);
      Level.Add((Thing) this._dust);
      this._dust.depth = this.depth - 2;
      this._lighting = new ArcadeTableLight(this.x, this.y - 43f);
      Level.Add((Thing) this._lighting);
    }

    public override void Update()
    {
      this._hasEligibleChallenges = Challenges.GetEligibleChancyChallenges(Profiles.active[0]).Count > 0;
      Duck duck1 = Level.Nearest<Duck>(this.x, this.y);
      if (duck1 != null)
      {
        if (duck1.grounded && (double) (duck1.position - this.position).length < 20.0)
        {
          this._hoverFade = Lerp.Float(this._hoverFade, 1f, 0.1f);
          this.hover = true;
        }
        else
        {
          this._hoverFade = Lerp.Float(this._hoverFade, 0.0f, 0.1f);
          this.hover = false;
        }
      }
      if (this._hasEligibleChallenges)
      {
        Vec2 vec2 = new Vec2(40f, 0.0f);
        Duck duck2 = Level.Nearest<Duck>(this.x + vec2.x, this.y + vec2.y);
        if (duck2 != null)
          this.hoverChancyChallenge = duck2.grounded && (double) (duck2.position - (this.position + vec2)).length < 20.0;
      }
      this._dust.fade = 0.5f;
      this._dust.visible = this._unlocked && this.visible;
    }

    public override void Draw()
    {
      this._light.depth = this.depth - 9;
      this._prizes.depth = this.depth - 7;
      Graphics.Draw(this._prizes, this.x - 28f, this.y - 33f);
      if (this._unlocked)
        this.graphic.color = Color.White;
      else
        this.graphic.color = Color.Black;
      Graphics.Draw((Sprite) this._light, this.x - 53f, this.y - 40f);
      if (Chancy.atCounter && !(Level.current is Editor))
      {
        Vec2 vec2 = new Vec2(32f, -15f);
        Chancy.body.flipH = true;
        if (this._hasEligibleChallenges)
        {
          vec2 = new Vec2(42f, -10f);
          Chancy.body.flipH = false;
        }
        Chancy.body.depth = this.depth - 6;
        Graphics.Draw(Chancy.body, this.x + vec2.x, this.y + vec2.y);
        if (this.hoverChancyChallenge)
          this._hoverSprite.alpha = Lerp.Float(this._hoverSprite.alpha, 1f, 0.05f);
        else
          this._hoverSprite.alpha = Lerp.Float(this._hoverSprite.alpha, 0.0f, 0.05f);
        if ((double) this._hoverSprite.alpha > 0.00999999977648258)
        {
          this._hoverSprite.depth = (Depth) 0.0f;
          this._hoverSprite.flipH = Chancy.body.flipH;
          if (this._hoverSprite.flipH)
            Graphics.Draw(this._hoverSprite, (float) ((double) this.x + (double) vec2.x + 1.0), (float) ((double) this.y + (double) vec2.y - 1.0));
          else
            Graphics.Draw(this._hoverSprite, (float) ((double) this.x + (double) vec2.x - 1.0), (float) ((double) this.y + (double) vec2.y - 1.0));
        }
      }
      base.Draw();
      if ((double) this._hoverFade <= 0.0)
        return;
      this._outline.alpha = this._hoverFade;
      Graphics.Draw(this._outline, this.x + 1f, this.y);
    }
  }
}
