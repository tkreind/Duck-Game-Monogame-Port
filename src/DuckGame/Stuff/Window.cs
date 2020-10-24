// Decompiled with JetBrains decompiler
// Type: DuckGame.Window
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class Window : Block, IPlatform, ISequenceItem, IDontMove
  {
    public StateBinding _positionBinding = new StateBinding(nameof (netPosition));
    public StateBinding _hitPointsBinding = new StateBinding(nameof (hitPoints));
    public StateBinding _destroyedBinding = new StateBinding("_destroyed");
    public StateBinding _damageMultiplierBinding = new StateBinding(nameof (damageMultiplier));
    public StateBinding _shakeTimesBinding = new StateBinding(nameof (shakeTimes));
    public NetIndex4 shakeTimes = (NetIndex4) 0;
    private NetIndex4 _localShakeTimes = (NetIndex4) 0;
    public float maxHealth = 5f;
    public float hitPoints = 5f;
    public float damageMultiplier = 1f;
    protected Sprite _sprite;
    protected Sprite _borderSprite;
    protected Sprite _barSprite;
    public bool landed = true;
    private List<Vec2> _hits = new List<Vec2>();
    private SinWave _shake = (SinWave) 0.8f;
    private float _shakeVal;
    private Vec2 _shakeMult = Vec2.Zero;
    public bool floor;
    public bool doShake;
    protected WindowFrame _frame;
    public EditorProperty<int> windowHeight;
    public EditorProperty<int> tint = new EditorProperty<int>(0, max: ((float) (Window.windowColors.Count - 1)), increment: 1f);
    public EditorProperty<bool> valid;
    public EditorProperty<bool> bars = new EditorProperty<bool>(false);
    public static List<Color> windowColors = new List<Color>()
    {
      new Color(102, 186, 245),
      Color.Red,
      Color.Orange,
      Color.Yellow,
      Color.Pink,
      Color.Purple,
      Color.Green,
      Color.Lime,
      Color.Maroon,
      Color.Magenta,
      Color.Cyan,
      Color.DarkGoldenrod
    };
    public bool noframe;
    public bool lobbyRemoving;
    private Vec2 _enter;
    private bool _wrecked;
    private bool _hasGlass = true;

    public override Vec2 netPosition
    {
      get => this.position;
      set
      {
        if (!(this.position != value))
          return;
        this.position = value;
        if (this._frame != null)
          this._frame.position = this.position;
        Level.current.things.quadTree.Remove((Thing) this);
        Level.current.things.quadTree.Add((Thing) this);
      }
    }

    public override void EditorPropertyChanged(object property)
    {
      this.UpdateHeight();
      this.sequence.isValid = this.valid.value;
    }

    public virtual void UpdateHeight()
    {
      float num = (float) this.windowHeight.value * 16f;
      this.center = new Vec2(3f, 0.0f);
      if (this.floor)
      {
        this.collisionSize = new Vec2(num, 6f);
        this.collisionOffset = new Vec2((float) (-(double) num + 16.0), -2f);
        this._sprite.angleDegrees = -90f;
      }
      else
      {
        this.collisionSize = new Vec2(6f, num);
        this.collisionOffset = new Vec2(-3f, (float) (-(double) num + 8.0));
        this._sprite.angle = 0.0f;
      }
      this._sprite.yscale = num;
      this._borderSprite.yscale = num;
      if (this._frame != null)
        this._frame.high = num;
      this.sequence.isValid = this.valid.value;
    }

    public Window(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.windowHeight = new EditorProperty<int>(2, (Thing) this, 1f, 16f, 1f);
      this.valid = new EditorProperty<bool>(false, (Thing) this);
      this._sprite = new Sprite("window32", 6f, 1f);
      this._barSprite = new Sprite("windowBars", 8f, 1f);
      this._borderSprite = new Sprite("window32border");
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Goody;
      this.center = new Vec2(3f, 24f);
      this.collisionSize = new Vec2(6f, 32f);
      this.collisionOffset = new Vec2(-3f, -24f);
      this.depth = new Depth(-0.5f);
      this._editorName = nameof (Window);
      this.thickness = 0.3f;
      this._sprite.color = new Color(1f, 1f, 1f, 0.2f);
      this.alpha = 0.7f;
      this.breakForce = 3f;
      this._canFlip = false;
      this._translucent = true;
      this.UpdateHeight();
    }

    public override void Initialize()
    {
      if (!this.floor && !this.noframe)
      {
        this._frame = new WindowFrame(this.x, this.y, this.floor);
        Level.Add((Thing) this._frame);
      }
      this.UpdateHeight();
    }

    public override void Terminate()
    {
      if (!(Level.current is Editor) && !this._wrecked && !this.lobbyRemoving)
      {
        this._wrecked = true;
        for (int index = 0; index < 8; ++index)
        {
          Thing thing = (Thing) new GlassParticle(this.x - 4f + Rando.Float(8f), this.y - 16f + Rando.Float(32f), Vec2.Zero, this.tint.value);
          thing.hSpeed = ((double) Rando.Float(1f) > 0.5 ? 1f : -1f) * Rando.Float(3f);
          thing.vSpeed = -Rando.Float(1f);
          Level.Add(thing);
        }
        if (this is FloorWindow)
        {
          for (int index = 0; index < 8; ++index)
            Level.Add((Thing) new GlassDebris(false, this.left + (float) (index * 4), this.y, -Rando.Float(2f), -Rando.Float(2f), 1));
          foreach (PhysicsObject physicsObject in Level.CheckLineAll<PhysicsObject>(this.topLeft + new Vec2(-2f, -3f), this.topRight + new Vec2(2f, -3f)))
          {
            physicsObject._sleeping = false;
            physicsObject.vSpeed -= 2f;
          }
        }
        else
        {
          for (int index = 0; index < 8; ++index)
            Level.Add((Thing) new GlassDebris(false, this.x, this.top + (float) (index * 4), -Rando.Float(2f), -Rando.Float(2f), 1, this.tint.value));
        }
        SFX.Play("glassBreak");
      }
      if (!this.floor && !this._wrecked)
      {
        Level.Remove((Thing) this._frame);
        this._frame = (WindowFrame) null;
      }
      base.Terminate();
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal)
        Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      if (!this._hasGlass)
        return base.Hit(bullet, hitPos);
      this._enter = hitPos + bullet.travelDirNormalized;
      if ((double) this._enter.x < (double) this.x && (double) this._enter.x < (double) this.left + 2.0)
        this._enter.x = this.left;
      else if ((double) this._enter.x > (double) this.x && (double) this._enter.x > (double) this.right - 2.0)
        this._enter.x = this.right;
      if ((double) this._enter.y < (double) this.y && (double) this._enter.y < (double) this.top + 2.0)
        this._enter.y = this.top;
      else if ((double) this._enter.y > (double) this.y && (double) this._enter.y > (double) this.bottom - 2.0)
        this._enter.y = this.bottom;
      if ((double) this.hitPoints <= 0.0)
        return false;
      hitPos -= bullet.travelDirNormalized;
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
        Level.Add((Thing) new GlassParticle(hitPos.x, hitPos.y, bullet.travelDirNormalized, this.tint.value));
      SFX.Play("glassHit", 0.5f);
      if (bullet.isLocal)
      {
        this.hitPoints -= this.damageMultiplier;
        ++this.damageMultiplier;
      }
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      if (!this._hasGlass)
        return;
      this._hits.Add(this._enter);
      Vec2 vec2 = exitPos - bullet.travelDirNormalized;
      if ((double) vec2.x < (double) this.x && (double) vec2.x < (double) this.left + 2.0)
        vec2.x = this.left;
      else if ((double) vec2.x > (double) this.x && (double) vec2.x > (double) this.right - 2.0)
        vec2.x = this.right;
      if ((double) vec2.y < (double) this.y && (double) vec2.y < (double) this.top + 2.0)
        vec2.y = this.top;
      else if ((double) vec2.y > (double) this.y && (double) vec2.y > (double) this.bottom - 2.0)
        vec2.y = this.bottom;
      this._hits.Add(vec2);
      exitPos += bullet.travelDirNormalized;
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
        Level.Add((Thing) new GlassParticle(exitPos.x, exitPos.y, -bullet.travelDirNormalized, this.tint.value));
    }

    public void Shake()
    {
      SFX.Play("glassBump", 0.7f);
      this._shakeVal = 3.141593f;
    }

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      with.Fondle((Thing) this);
      if (this.floor && (double) with.top > (double) this.top && ((double) this.CalculateImpactPower(with, from) > 2.79999995231628 && with.isServerForObject))
      {
        this.Destroy((DestroyType) new DTImpact((Thing) with));
      }
      else
      {
        float num = Math.Abs(with.hSpeed) + Math.Abs(with.vSpeed);
        if (this.destroyed || (double) num <= 1.5)
          return;
        ++this.shakeTimes;
      }
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (!this._hasGlass)
        return false;
      if (this.bars.value)
        this._hasGlass = false;
      else
        Level.Remove((Thing) this);
      if (this.sequence != null && this.sequence.isValid)
      {
        this.sequence.Finished();
        if (ChallengeLevel.running)
          ++ChallengeLevel.goodiesGot;
      }
      return !this.bars.value;
    }

    public override void Update()
    {
      this.breakForce = (float) (6.0 * ((double) this.hitPoints / (double) this.maxHealth));
      if ((double) this.hitPoints <= 0.0)
        this.Destroy((DestroyType) new DTImpact((Thing) null));
      base.Update();
      if ((double) this.damageMultiplier > 1.0)
        this.damageMultiplier -= 0.2f;
      else
        this.damageMultiplier = 1f;
      this._shakeMult = Lerp.Vec2(this._shakeMult, Vec2.Zero, 0.1f);
      if (this._localShakeTimes < this.shakeTimes)
      {
        this.Shake();
        this._localShakeTimes = this.shakeTimes;
      }
      this._shakeVal = Lerp.Float(this._shakeVal, 0.0f, 0.05f);
    }

    public override void Draw()
    {
      Vec2 zero = Vec2.Zero;
      float num1 = (float) ((double) (float) this._shake * (double) this._shakeVal * 0.800000011920929);
      if (this.floor)
        zero.y = num1;
      else
        zero.x = num1;
      Window window1 = this;
      window1.position = window1.position + zero;
      float num2 = (float) this.windowHeight.value * 16f;
      this._sprite.depth = this.depth;
      this._borderSprite.depth = this.depth;
      this._borderSprite.angle = this._sprite.angle;
      this._barSprite.depth = this._sprite.depth + 4;
      this._barSprite.yscale = this._sprite.yscale;
      this._barSprite.alpha = this._sprite.alpha;
      this._barSprite.angle = this.angle;
      if (this._hasGlass)
      {
        Color windowColor = Window.windowColors[this.tint.value];
        windowColor.a = (byte) 51;
        this._sprite.color = windowColor;
        this.alpha = 0.7f;
        if (this.floor)
        {
          Graphics.Draw(this._sprite, (float) ((double) this.x - (double) num2 + 16.0), this.y + 4f);
          Graphics.Draw(this._borderSprite, (float) ((double) this.x - (double) num2 + 16.0), this.y + 4f);
        }
        else
        {
          Graphics.Draw(this._sprite, this.x - 3f, (float) ((double) this.y - (double) num2 + 8.0));
          Graphics.Draw(this._borderSprite, this.x - 3f, (float) ((double) this.y - (double) num2 + 8.0));
        }
        for (int index = 0; index < this._hits.Count; index += 2)
        {
          if (index + 1 > this._hits.Count)
            return;
          Color col = new Color((byte) ((double) windowColor.r * 0.5), (byte) ((double) windowColor.g * 0.5), (byte) ((double) windowColor.b * 0.800000011920929), (byte) 178);
          Graphics.DrawLine(this._hits[index] + zero, this._hits[index + 1] + zero, col);
        }
      }
      Window window2 = this;
      window2.position = window2.position - zero;
      if (this.floor)
      {
        if (this.bars.value)
          Graphics.Draw(this._barSprite, (float) ((double) this.x - (double) num2 + 16.0), this.y + 5f);
      }
      else if (this.bars.value)
        Graphics.Draw(this._barSprite, this.x - 4f, (float) ((double) this.y - (double) num2 + 8.0));
      base.Draw();
    }
  }
}
