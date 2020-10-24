// Decompiled with JetBrains decompiler
// Type: DuckGame.PurpleBlock
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  [EditorGroup("spawns")]
  public class PurpleBlock : ItemBox
  {
    private Sprite _scanner;
    private Sprite _projector;
    private Sprite _none;
    private Sprite _projectorGlitch;
    private Sprite _currentProjection;
    private SinWave _wave = (SinWave) 1f;
    private SinWave _projectionWave = (SinWave) 0.04f;
    private SinWave _projectionWave2 = (SinWave) 0.05f;
    private SinWave _projectionFlashWave = (SinWave) 0.8f;
    private bool _useWave;
    private bool _alternate;
    private float _double;
    private float _glitch;
    public static MTEffect _grayscaleEffect = Content.Load<MTEffect>("Shaders/greyscale");
    private static List<StoredItem> _storedItems = new List<StoredItem>();
    private List<Profile> _served = new List<Profile>();
    private List<Profile> _close = new List<Profile>();
    private float _closeWait;
    private int _closeIndex;
    private float _projectorAlpha;
    private bool _closeGlitch;
    private Holdable _hoverItem;
    private Thing _contextThing;
    private float hitWait;

    public static void Reset() => PurpleBlock._storedItems.Clear();

    public PurpleBlock(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("purpleBlock");
      this.graphic.center = new Vec2(8f, 8f);
      this._scanner = new Sprite("purpleScanner");
      this._scanner.center = new Vec2(4f, 1f);
      this._scanner.alpha = 0.7f;
      this._scanner.depth = new Depth(0.9f);
      this._projector = new Sprite("purpleProjector");
      this._projector.center = new Vec2(8f, 16f);
      this._projector.alpha = 0.7f;
      this._projector.depth = new Depth(0.9f);
      this._none = new Sprite("none");
      this._none.center = new Vec2(8f, 8f);
      this._none.alpha = 0.7f;
      this._projectorGlitch = new Sprite("projectorGlitch");
      this._projectorGlitch.center = new Vec2(8f, 8f);
      this._projectorGlitch.alpha = 0.7f;
      this._projectorGlitch.depth = new Depth(0.91f);
      this._currentProjection = this._none;
      this.impactThreshold = 0.2f;
    }

    public override void Initialize()
    {
    }

    public static StoredItem GetStoredItem(Profile p)
    {
      StoredItem storedItem = PurpleBlock._storedItems.FirstOrDefault<StoredItem>((Func<StoredItem, bool>) (i => i.profile == p));
      if (storedItem == null)
      {
        storedItem = new StoredItem() { profile = p };
        PurpleBlock._storedItems.Add(storedItem);
      }
      return storedItem;
    }

    public static void StoreItem(Profile p, Thing t)
    {
      if (Network.isActive && t is RagdollPart || t is TrappedDuck)
        return;
      if (t is WeightBall)
        t = (Thing) (t as WeightBall).collar;
      StoredItem storedItem = PurpleBlock.GetStoredItem(p);
      System.Type type = t.GetType();
      if (!(storedItem.type != type) && !(storedItem.type == typeof (RagdollPart)) && (!(t is TeamHat) || !(storedItem.thing is TeamHat) || (t as TeamHat).team == (storedItem.thing as TeamHat).team))
        return;
      Thing thing;
      if (t is RagdollPart)
      {
        if (storedItem.thing != null && storedItem.thing is SpriteThing && (storedItem.thing as SpriteThing).persona == (t as RagdollPart)._persona)
          return;
        thing = (Thing) new SpriteThing(0.0f, 0.0f, (Sprite) (t as RagdollPart)._persona.defaultHead);
        (thing as SpriteThing).persona = (t as RagdollPart)._persona;
      }
      else
        thing = Editor.CreateThing(type);
      if (thing is TeamHat)
      {
        TeamHat teamHat1 = thing as TeamHat;
        TeamHat teamHat2 = t as TeamHat;
        teamHat1.sprite = teamHat2.sprite.CloneMap();
        teamHat1.graphic = (Sprite) teamHat1.sprite;
        teamHat1.pickupSprite = teamHat2.pickupSprite.Clone();
        teamHat1.team = teamHat2.team;
      }
      else if (thing.graphic == null)
        thing.graphic = t.graphic.Clone();
      storedItem.sprite = thing.GetEditorImage(0, 0, true, (Effect) PurpleBlock._grayscaleEffect);
      storedItem.sprite.CenterOrigin();
      if (t is RagdollPart)
      {
        storedItem.sprite.centerx += 2f;
        storedItem.sprite.centery += 4f;
      }
      storedItem.type = type;
      storedItem.thing = thing;
      SFX.Play("scanBeep");
    }

    private void BreakHoverBond()
    {
      this._hoverItem.gravMultiplier = 1f;
      this._hoverItem = (Holdable) null;
    }

    public override void Update()
    {
      if ((double) this.hitWait > 0.0)
        this.hitWait -= 0.1f;
      else
        this.hitWait = 0.0f;
      this._alternate = !this._alternate;
      this._scanner.alpha = (float) (0.400000005960464 + (double) this._wave.normalized * 0.600000023841858);
      this._projector.alpha = (float) (0.400000005960464 + (double) this._wave.normalized * 0.600000023841858) * this._projectorAlpha;
      this._currentProjection.alpha = (float) (0.400000005960464 + (double) this._projectionFlashWave.normalized * 0.600000023841858);
      this._currentProjection.alpha -= this._glitch * 3f;
      this._currentProjection.alpha *= this._projectorAlpha;
      this._double = Maths.CountDown(this._double, 0.15f);
      this._glitch = Maths.CountDown(this._glitch, 0.1f);
      if ((double) Rando.Float(1f) < 0.00999999977648258)
      {
        this._glitch = 0.3f;
        this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
        this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
        this._projectorGlitch.flipH = (double) Rando.Float(1f) > 0.5;
      }
      if ((double) Rando.Float(1f) < 0.00499999988824129)
      {
        this._glitch = 0.3f;
        this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
        this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
        this._projectorGlitch.flipH = (double) Rando.Float(1f) > 0.5;
        this._useWave = !this._useWave;
      }
      if ((double) Rando.Float(1f) < 0.00800000037997961)
      {
        this._glitch = 0.3f;
        this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
        this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
        this._projectorGlitch.flipH = (double) Rando.Float(1f) > 0.5;
        this._useWave = !this._useWave;
        this._double = 0.6f + Rando.Float(0.6f);
      }
      this._close.Clear();
      if (this._hoverItem != null && this._hoverItem.owner != null)
        this.BreakHoverBond();
      if (this._hoverItem == null)
      {
        Holdable holdable = Level.Nearest<Holdable>(this.x, this.y);
        if (holdable != null && holdable.owner == null && (holdable != null && holdable.canPickUp) && ((double) holdable.bottom <= (double) this.top && (double) Math.Abs(holdable.hSpeed) + (double) Math.Abs(holdable.vSpeed) < 2.0))
        {
          float num = 999f;
          if (holdable != null)
            num = (this.position - holdable.position).length;
          if ((double) num < 24.0)
            this._hoverItem = holdable;
        }
      }
      else if ((double) Math.Abs(this._hoverItem.hSpeed) + (double) Math.Abs(this._hoverItem.vSpeed) > 2.0 || (double) (this._hoverItem.position - this.position).length > 25.0)
      {
        this.BreakHoverBond();
      }
      else
      {
        this._hoverItem.position = Lerp.Vec2Smooth(this._hoverItem.position, this.position + new Vec2(0.0f, (float) (-12.0 - (double) this._hoverItem.collisionSize.y / 2.0 + (double) (float) this._projectionWave * 2.0)), 0.2f);
        this._hoverItem.vSpeed = 0.0f;
        this._hoverItem.gravMultiplier = 0.0f;
      }
      foreach (Duck duck in this._level.things[typeof (Duck)])
      {
        if (!duck.dead && (double) (duck.position - this.position).length < 64.0)
        {
          this._close.Add(duck.profile);
          this._closeGlitch = false;
        }
      }
      this._closeWait = Maths.CountDown(this._closeWait, 0.05f);
      for (int index = 0; index < this._close.Count; ++index)
      {
        if (this._close.Count == 1)
          this._closeIndex = 0;
        else if (this._close.Count > 1 && index == this._closeIndex && (double) this._closeWait <= 0.0)
        {
          this._closeIndex = (this._closeIndex + 1) % this._close.Count;
          this._closeWait = 1f;
          this._glitch = 0.3f;
          this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
          this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
          this._projectorGlitch.flipH = (double) Rando.Float(1f) > 0.5;
          this._useWave = !this._useWave;
          this._double = 0.6f + Rando.Float(0.6f);
          break;
        }
      }
      if (this._closeIndex >= this._close.Count)
        this._closeIndex = 0;
      if (this._close.Count == 0)
      {
        if (!this._closeGlitch)
        {
          this._closeWait = 1f;
          this._glitch = 0.3f;
          this._projectorGlitch.xscale = 0.8f + Rando.Float(0.7f);
          this._projectorGlitch.yscale = 0.6f + Rando.Float(0.5f);
          this._projectorGlitch.flipH = (double) Rando.Float(1f) > 0.5;
          this._useWave = !this._useWave;
          this._double = 0.6f + Rando.Float(0.6f);
          this._closeGlitch = true;
        }
        this._projectorAlpha = Maths.CountDown(this._projectorAlpha, 0.1f);
        this._currentProjection = this._none;
      }
      else
      {
        StoredItem storedItem = PurpleBlock.GetStoredItem(this._close[this._closeIndex]);
        this._currentProjection = storedItem.sprite != null ? (!this._served.Contains(this._close[this._closeIndex]) ? storedItem.sprite : (this._alternate ? storedItem.sprite : this._none)) : this._none;
        this._projectorAlpha = Maths.CountUp(this._projectorAlpha, 0.1f);
      }
      this._projectorGlitch.alpha = this._glitch * this._projectorAlpha;
      base.Update();
    }

    public override void Draw()
    {
      base.Draw();
      if (this._alternate)
        DuckGame.Graphics.Draw(this._scanner, this.x, this.y + 9f);
      if (!this._alternate)
        DuckGame.Graphics.Draw(this._projector, this.x, this.y - 8f);
      float num = (float) (this._useWave ? this._projectionWave : this._projectionWave2);
      if ((double) this._double > 0.0)
      {
        this._currentProjection.alpha = (float) (0.200000002980232 + (double) this._projectionFlashWave.normalized * 0.200000002980232 + (double) this._glitch * 1.0) * this._projectorAlpha;
        DuckGame.Graphics.Draw(this._currentProjection, this.x - this._double * 2f, this.y - 16f - num);
        DuckGame.Graphics.Draw(this._currentProjection, this.x + this._double * 2f, this.y - 16f - num);
      }
      else
      {
        this._currentProjection.alpha = (float) (0.400000005960464 + (double) this._projectionFlashWave.normalized * 0.600000023841858 + (double) this._glitch * 1.0) * this._projectorAlpha;
        DuckGame.Graphics.Draw(this._currentProjection, this.x, this.y - 16f - num);
      }
      if ((double) this._glitch <= 0.0)
        return;
      DuckGame.Graphics.Draw(this._projectorGlitch, this.x, this.y - 16f);
    }

    public override PhysicsObject GetSpawnItem()
    {
      if (!(this.contains == typeof (RagdollPart)))
        return base.GetSpawnItem();
      if (!(this._contextThing is SpriteThing contextThing))
        return (PhysicsObject) null;
      Ragdoll ragdoll = new Ragdoll(this.x, this.y, (Duck) null, false, 0.0f, 0, Vec2.Zero, contextThing.persona);
      Level.Add((Thing) ragdoll);
      ragdoll.RunInit();
      foreach (PhysicsObject above in this._aboveList)
      {
        ragdoll.part1.clip.Add((MaterialThing) above);
        ragdoll.part2.clip.Add((MaterialThing) above);
        ragdoll.part3.clip.Add((MaterialThing) above);
      }
      ragdoll.part1.vSpeed = -3.5f;
      ragdoll.part2.vSpeed = -3.5f;
      ragdoll.part3.vSpeed = -3.5f;
      ragdoll.part1.clip.Add((MaterialThing) this);
      ragdoll.part2.clip.Add((MaterialThing) this);
      ragdoll.part3.clip.Add((MaterialThing) this);
      Block block1 = Level.CheckPoint<Block>(this.position + new Vec2(-16f, 0.0f));
      if (block1 != null)
      {
        ragdoll.part1.clip.Add((MaterialThing) block1);
        ragdoll.part2.clip.Add((MaterialThing) block1);
        ragdoll.part3.clip.Add((MaterialThing) block1);
      }
      Block block2 = Level.CheckPoint<Block>(this.position + new Vec2(16f, 0.0f));
      if (block2 != null)
      {
        ragdoll.part1.clip.Add((MaterialThing) block2);
        ragdoll.part2.clip.Add((MaterialThing) block2);
        ragdoll.part3.clip.Add((MaterialThing) block2);
      }
      SFX.Play("hitBox");
      this.containedObject = (PhysicsObject) null;
      return (PhysicsObject) null;
    }

    public override void SpawnItem()
    {
      base.SpawnItem();
      if (this.lastSpawnItem != null)
      {
        TeamHat lastSpawnItem = this.lastSpawnItem as TeamHat;
        TeamHat contextThing = this._contextThing as TeamHat;
        if (lastSpawnItem != null && contextThing != null)
        {
          lastSpawnItem.sprite = contextThing.sprite.CloneMap();
          lastSpawnItem.graphic = (Sprite) lastSpawnItem.sprite;
          lastSpawnItem.team = contextThing.team;
          lastSpawnItem.pickupSprite = contextThing.pickupSprite.Clone();
        }
      }
      this.lastSpawnItem = (PhysicsObject) null;
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (from == ImpactedFrom.Bottom && (double) this.hitWait == 0.0 && with.isServerForObject)
        with.Fondle((Thing) this);
      if (!this.isServerForObject || from != ImpactedFrom.Bottom || (double) this.hitWait != 0.0)
        return;
      this.hitWait = 1f;
      switch (with)
      {
        case Holdable holdable when holdable.lastThrownBy != null || holdable is RagdollPart && !Network.isActive:
          Duck lastThrownBy = holdable.lastThrownBy as Duck;
          if (holdable is RagdollPart)
            break;
          if (lastThrownBy != null)
            PurpleBlock.StoreItem(lastThrownBy.profile, (Thing) with);
          this.Bounce();
          break;
        case Duck duck:
          StoredItem storedItem = PurpleBlock.GetStoredItem(duck.profile);
          if (storedItem.type != (System.Type) null && !this._served.Contains(duck.profile))
          {
            this.contains = storedItem.type;
            this._contextThing = storedItem.thing;
            this.Pop();
            this._hit = false;
            this._served.Add(duck.profile);
          }
          else
          {
            if (this._served.Contains(duck.profile))
              SFX.Play("scanFail");
            this.Bounce();
          }
          if (duck.holdObject == null)
            break;
          Holdable holdObject = duck.holdObject;
          if (holdObject == null)
            break;
          PurpleBlock.StoreItem(duck.profile, (Thing) holdObject);
          break;
      }
    }
  }
}
