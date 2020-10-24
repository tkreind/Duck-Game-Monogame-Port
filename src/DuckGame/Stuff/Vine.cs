﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Vine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  [BaggedProperty("isOnlineCapable", false)]
  public class Vine : Holdable, ISwing
  {
    protected SpriteMap _sprite;
    protected Harpoon _harpoon;
    public Rope _rope;
    public int sectionIndex;
    public EditorProperty<int> length = new EditorProperty<int>(4, min: 1f, max: 16f, increment: 1f);
    public Vine nextVine;
    public Vine prevVine;
    protected Sprite _vinePartSprite;
    public float initLength;
    public bool changeSpeed = true;
    protected Vec2 _wallPoint = new Vec2();
    protected Vec2 _grappleTravel = new Vec2();

    public List<VineSection> points
    {
      get
      {
        List<VineSection> vineSectionList1 = new List<VineSection>();
        List<VineSection> vineSectionList2 = new List<VineSection>();
        Rope rope = this._rope;
        Vine vine = this;
        while (rope != null)
        {
          VineSection vineSection = new VineSection()
          {
            pos2 = rope.attach1Point,
            pos1 = rope.attach2Point
          };
          vineSection.length = (vineSection.pos1 - vineSection.pos2).length;
          rope = rope.attach2 as Rope;
          vineSectionList2.Add(vineSection);
          if (rope == null && vine.nextVine != null)
          {
            vineSectionList2.Reverse();
            vineSectionList1.AddRange((IEnumerable<VineSection>) vineSectionList2);
            vineSectionList2.Clear();
            vine = vine.nextVine;
            rope = vine._rope;
          }
        }
        if (vineSectionList2.Count > 0)
        {
          vineSectionList2.Reverse();
          vineSectionList1.AddRange((IEnumerable<VineSection>) vineSectionList2);
        }
        float num1 = 0.0f;
        foreach (VineSection vineSection in vineSectionList1)
          num1 += vineSection.length;
        int num2 = 0;
        foreach (VineSection vineSection in vineSectionList1)
        {
          vineSection.lowestSection = num2 + (int) Math.Round((double) vineSection.length / (double) num1 * (double) this.sectionIndex);
          num2 = vineSection.lowestSection;
        }
        return vineSectionList1;
      }
    }

    public Vine(float xpos, float ypos, float init)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("vine", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this._vinePartSprite = new Sprite("vine");
      this._vinePartSprite.center = new Vec2(8f, 0.0f);
      this.collisionOffset = new Vec2(-5f, -4f);
      this.collisionSize = new Vec2(11f, 7f);
      this.weight = 0.1f;
      this.thickness = 0.1f;
      this.canPickUp = false;
      this.initLength = init;
      this.depth = (Depth) -0.5f;
    }

    public override void OnPressAction()
    {
    }

    public override void OnReleaseAction()
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this._harpoon = new Harpoon((Thing) this);
      Level.Add((Thing) this._harpoon);
      if (Level.current is Editor)
        return;
      Vec2 position = this.position;
      this.position.y += (float) ((int) this.length * 16 - 8);
      this._harpoon.noisy = false;
      this._harpoon.Fire(position + new Vec2(0.0f, -8f), new Vec2(0.0f, -1f));
      this._rope = new Rope(this.x, this.y, (Thing) null, (Thing) this._harpoon, (Thing) this.duck, true, this._vinePartSprite);
      if ((double) this.initLength != 0.0)
        this._rope.properLength = this.initLength;
      Level.Add((Thing) this._rope);
    }

    public override void Terminate()
    {
      if (this._rope == null)
        return;
      this._rope.RemoveRope();
      Level.Remove((Thing) this._harpoon);
      Level.Remove((Thing) this._rope);
    }

    public Rope GetRopeParent(Thing child)
    {
      for (Rope rope = this._rope; rope != null; rope = rope.attach2 as Rope)
      {
        if (rope.attach2 == child)
          return rope;
      }
      return (Rope) null;
    }

    public void Degrapple()
    {
      if (this.nextVine != null && this.nextVine._rope != null)
      {
        this.nextVine._rope.attach2 = this._rope.attach2;
        this.nextVine._rope.properLength = (this.nextVine._rope.attach1Point - this._rope.attach2Point).length;
        this.nextVine.prevVine = (Vine) null;
        this.nextVine = (Vine) null;
      }
      if (this.prevVine != null)
        this.prevVine.nextVine = (Vine) null;
      this._harpoon.Return();
      this._harpoon.visible = false;
      if (this._rope != null)
      {
        this._rope.RemoveRope();
        this._rope.visible = false;
        this.visible = false;
      }
      this._rope = (Rope) null;
      if (this.duck != null)
      {
        this.duck.frictionMult = 1f;
        this.duck.gravMultiplier = 1f;
      }
      this.owner = (Thing) null;
      this.frictionMult = 1f;
      this.gravMultiplier = 1f;
      this.visible = false;
      Level.Remove((Thing) this._harpoon);
      Level.Remove((Thing) this);
      this.Update();
    }

    public void UpdateRopeStuff()
    {
      this._rope.Update();
      this.Update();
    }

    public void MoveDuck()
    {
      Vec2 vec2_1 = this._rope.attach1.position - this._rope.attach2.position;
      if ((double) vec2_1.length <= (double) this._rope.properLength)
        return;
      vec2_1 = vec2_1.normalized;
      if (this.duck == null)
        return;
      PhysicsObject duck = (PhysicsObject) this.duck;
      Vec2 position = duck.position;
      duck.position = this._rope.attach2.position + vec2_1 * this._rope.properLength;
      Vec2 vec2_2 = duck.position - duck.lastPosition;
    }

    public Vec2 wallPoint => this._wallPoint;

    public Vec2 grappelTravel => this._grappleTravel;

    public override void Update()
    {
      base.Update();
      if (this.owner != null)
        this.offDir = this.owner.offDir;
      if (this.duck != null && (this.duck.ragdoll != null || this.duck._trapped != null || this.duck.dead))
      {
        this.owner = (Thing) null;
        this._rope.visible = false;
      }
      if (this._rope == null)
        return;
      if (this.owner != null)
      {
        this._rope.position = this.owner.position;
      }
      else
      {
        this._rope.position = this.position;
        if (this.prevOwner != null)
        {
          PhysicsObject prevOwner = this.prevOwner as PhysicsObject;
          prevOwner.frictionMult = 1f;
          prevOwner.gravMultiplier = 1f;
          this._prevOwner = (Thing) null;
          this.frictionMult = 1f;
          this.gravMultiplier = 1f;
          Level.Remove((Thing) this);
        }
      }
      if (!this._harpoon.stuck)
        return;
      if (this.duck != null)
      {
        if (!this.duck.grounded)
        {
          this.duck.frictionMult = 0.0f;
        }
        else
        {
          this.duck.frictionMult = 1f;
          this.duck.gravMultiplier = 1f;
        }
      }
      else if (!this.grounded)
      {
        this.frictionMult = 0.0f;
      }
      else
      {
        this.frictionMult = 1f;
        this.gravMultiplier = 1f;
      }
      Vec2 vec2_1 = this._rope.attach1.position - this._rope.attach2.position;
      if ((double) this._rope.properLength < 0.0)
        this._rope.properLength = vec2_1.length;
      if ((double) vec2_1.length <= (double) this._rope.properLength)
        return;
      Vec2 normalized = vec2_1.normalized;
      if (this.duck != null)
      {
        PhysicsObject duck = (PhysicsObject) this.duck;
        if (this.duck.ragdoll != null)
        {
          this.Degrapple();
        }
        else
        {
          Vec2 position = duck.position;
          duck.position = this._rope.attach2.position + normalized * this._rope.properLength;
          Vec2 vec2_2 = duck.position - duck.lastPosition;
          if (!this.changeSpeed)
            return;
          duck.hSpeed = vec2_2.x;
          duck.vSpeed = vec2_2.y;
        }
      }
      else
      {
        Vec2 position = this.position;
        this.position = this._rope.attach2.position + normalized * this._rope.properLength;
        Vec2 vec2_2 = this.position - this.lastPosition;
        this.hSpeed = vec2_2.x;
        this.vSpeed = vec2_2.y;
      }
    }

    public override void Draw()
    {
      if (!(Level.current is Editor))
        return;
      this.graphic.center = new Vec2(8f, 8f);
      this.graphic.depth = this.depth;
      for (int index = 0; index < (int) this.length; ++index)
        Graphics.Draw(this.graphic, this.x, this.y + (float) (index * 16));
    }
  }
}
