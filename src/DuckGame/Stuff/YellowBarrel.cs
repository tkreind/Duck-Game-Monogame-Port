// Decompiled with JetBrains decompiler
// Type: DuckGame.YellowBarrel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("noRandomSpawningOnline", true)]
  [EditorGroup("stuff|props")]
  public class YellowBarrel : Holdable, IPlatform, ISequenceItem
  {
    public EditorProperty<bool> valid;
    protected float damageMultiplier = 1f;
    protected SpriteMap _sprite;
    protected float _fluidLevel = 1f;
    protected int _alternate;
    private List<FluidStream> _holes = new List<FluidStream>();
    protected FluidData _fluid;
    protected Sprite _melting;
    protected SpriteMap _toreUp;
    private bool _bottomHoles;
    private float _lossAccum;

    public YellowBarrel(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.valid = new EditorProperty<bool>(false, (Thing) this);
      this._maxHealth = 15f;
      this._hitPoints = 15f;
      this.graphic = new Sprite("yellowBarrel");
      this.center = new Vec2(7f, 8f);
      this._melting = new Sprite("yellowBarrelMelting");
      this._toreUp = new SpriteMap("yellowBarrelToreUp", 14, 12);
      this._toreUp.frame = 1;
      this._toreUp.center = new Vec2(0.0f, -8f);
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Goody;
      this.collisionOffset = new Vec2(-7f, -8f);
      this.collisionSize = new Vec2(14f, 16f);
      this.depth = new Depth(-0.1f);
      this._editorName = "Barrel Y";
      this.thickness = 4f;
      this.weight = 5f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.collideSounds.Add("barrelThud");
      this._holdOffset = new Vec2(1f, 0.0f);
      this.flammable = 0.3f;
      this._fluid = Fluid.Gas;
      this.sequence.isValid = this.valid.value;
    }

    public override void Initialize()
    {
      this.sequence.isValid = this.valid.value;
      base.Initialize();
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if ((double) this._hitPoints <= 0.0)
        return false;
      hitPos += bullet.travelDirNormalized * 2f;
      if (1.0 - ((double) hitPos.y - (double) this.top) / ((double) this.bottom - (double) this.top) < (double) this._fluidLevel)
      {
        this.thickness = 2f;
        Vec2 off = hitPos - this.position;
        bool flag = false;
        foreach (FluidStream hole in this._holes)
        {
          if ((double) (hole.offset - off).length < 2.0)
          {
            hole.offset = off;
            hole.holeThickness += 0.5f;
            flag = true;
            break;
          }
        }
        if (!flag)
          this._holes.Add(new FluidStream(0.0f, 0.0f, (-bullet.travelDirNormalized).Rotate(Rando.Float(-0.2f, 0.2f), Vec2.Zero), 1f, off));
        SFX.Play("bulletHitWater", pitch: Rando.Float(-0.2f, 0.2f));
        return base.Hit(bullet, hitPos);
      }
      this.thickness = 1f;
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      exitPos -= bullet.travelDirNormalized * 2f;
      Vec2 off = exitPos - this.position;
      bool flag = false;
      foreach (FluidStream hole in this._holes)
      {
        if ((double) (hole.offset - off).length < 2.0)
        {
          hole.offset = off;
          hole.holeThickness += 0.5f;
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this._holes.Add(new FluidStream(0.0f, 0.0f, bullet.travelDirNormalized.Rotate(Rando.Float(-0.2f, 0.2f), Vec2.Zero), 1f, off));
    }

    public override void Update()
    {
      base.Update();
      this.offDir = (sbyte) 1;
      if ((double) this._hitPoints <= 0.0)
      {
        if (this.graphic != this._toreUp)
        {
          float num1 = this._fluidLevel * 0.5f;
          float num2 = this._fluidLevel * 0.5f;
          FluidData fluid = this._fluid;
          fluid.amount = num1 / 20f;
          for (int index = 0; index < 20; ++index)
            Level.Add((Thing) new Fluid(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f), new Vec2(Rando.Float(-4f, 4f), Rando.Float(-4f, 0.0f)), fluid));
          fluid.amount = num2;
          Level.Add((Thing) new Fluid(this.x, this.y - 8f, new Vec2(0.0f, -1f), fluid));
          Level.Add((Thing) SmallSmoke.New(this.x, this.y));
          SFX.Play("bulletHitWater");
          SFX.Play("crateDestroy");
        }
        this.graphic = (Sprite) this._toreUp;
        this._onFire = false;
        this.burnt = 0.0f;
        this._weight = 0.1f;
        this._collisionSize.y = 8f;
        this._collisionOffset.y = -3f;
      }
      this.burnSpeed = 0.0015f;
      if (this._onFire && (double) this.burnt < 0.899999976158142)
      {
        if ((double) this.burnt > 0.300000011920929)
          this.graphic = this._melting;
        this.yscale = (float) (0.5 + (1.0 - (double) this.burnt) * 0.5);
        this.centery = (float) (8.0 - (double) this.burnt * 7.0);
        this._collisionOffset.y = (float) ((double) this.burnt * 7.0 - 8.0);
        this._collisionSize.y = (float) (16.0 - (double) this.burnt * 7.0);
      }
      if (!this._bottomHoles && (double) this.burnt > 0.600000023841858)
      {
        this._bottomHoles = true;
        this._holes.Add(new FluidStream(0.0f, 0.0f, new Vec2(-1f, -1f), 1f, new Vec2(-7f, 8f))
        {
          holeThickness = 2f
        });
        this._holes.Add(new FluidStream(0.0f, 0.0f, new Vec2(1f, -1f), 1f, new Vec2(7f, 8f))
        {
          holeThickness = 2f
        });
      }
      if (this._owner != null)
      {
        this.hSpeed = this.owner.hSpeed;
        this.vSpeed = this.owner.vSpeed;
      }
      if (this._alternate == 0)
      {
        foreach (FluidStream hole in this._holes)
        {
          hole.onFire = this.onFire;
          hole.hSpeed = this.hSpeed;
          hole.vSpeed = this.vSpeed;
          hole.DoUpdate();
          hole.position = this.Offset(hole.offset);
          hole.sprayAngle = this.OffsetLocal(hole.startSprayAngle);
          float num1 = (float) (1.0 - ((double) hole.offset.y - (double) this.topLocal) / ((double) this.bottomLocal - (double) this.topLocal));
          if ((double) hole.x > (double) this.left - 2.0 && (double) hole.x < (double) this.right + 2.0 && (double) num1 < (double) this._fluidLevel)
          {
            float num2 = Maths.Clamp(this._fluidLevel - num1, 0.1f, 1f) * 0.008f * hole.holeThickness;
            FluidData fluid = this._fluid;
            fluid.amount = num2;
            hole.Feed(fluid);
            this._fluidLevel -= num2;
            this._lossAccum += num2;
            while ((double) this._lossAccum > 0.0500000007450581)
            {
              this._lossAccum -= 0.05f;
              if (this.sequence != null && this.sequence.isValid && ChallengeLevel.running)
              {
                ++ChallengeLevel.goodiesGot;
                SFX.Play("tinyTick");
              }
            }
          }
        }
      }
      this.weight = this._fluidLevel * 10f;
      ++this._alternate;
      if (this._alternate <= 4)
        return;
      this._alternate = 0;
    }

    public override void Draw()
    {
      float num1 = 1f - this._fluidLevel;
      float num2 = (float) (0.600000023841858 + (1.0 - (double) this.burnt) * 0.400000005960464);
      this.graphic.color = new Color((byte) (150.0 * (double) num2), (byte) (150.0 * (double) num2), (byte) (150.0 * (double) num2));
      base.Draw();
      if ((double) this._hitPoints <= 0.0)
        return;
      this.graphic.color = new Color((byte) ((double) byte.MaxValue * (double) num2), (byte) ((double) byte.MaxValue * (double) num2), (byte) ((double) byte.MaxValue * (double) num2));
      this.graphic.angle = this.angle;
      this.graphic.depth = this.depth + 1;
      this.graphic.scale = this.scale;
      float num3 = num1 * (float) this.graphic.height;
      this.graphic.center = this.center - new Vec2(0.0f, (float) (int) num3);
      Graphics.Draw(this.graphic, this.x, this.y, new Rectangle(0.0f, (float) (int) num3, (float) this.graphic.w, (float) (int) ((double) this.graphic.h - (double) num3)));
    }
  }
}
