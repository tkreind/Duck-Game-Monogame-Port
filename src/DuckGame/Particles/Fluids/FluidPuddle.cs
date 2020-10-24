﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.FluidPuddle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class FluidPuddle : MaterialThing
  {
    private WhiteRectangle _lightRect;
    public FluidData data;
    public float _wide;
    public float fluidWave;
    private FluidStream _leftStream;
    private FluidStream _rightStream;
    private BlockCorner _leftCorner;
    private BlockCorner _rightCorner;
    private BlockCorner _topLeftCorner;
    private BlockCorner _topRightCorner;
    private bool _initializedUpperCorners;
    private List<SpriteMap> _surfaceFire = new List<SpriteMap>();
    private Block _block;
    private SpriteMap _lava;
    private SpriteMap _lavaAlternate;
    private int _framesSinceFeed;
    private float _fireID;
    private float _fireRise;
    private int bubbleWait;

    public float fireID => this._fireID;

    public FluidPuddle(float xpos, float ypos, Block b)
      : base(xpos, ypos)
    {
      this._collisionOffset.y = -4f;
      this._collisionSize.y = 1f;
      this._block = b;
      this.depth = (Depth) 0.3f;
      this.flammable = 0.9f;
      this.alpha = 0.0f;
      List<BlockCorner> groupCorners = b.GetGroupCorners();
      this._leftCorner = (BlockCorner) null;
      this._rightCorner = (BlockCorner) null;
      foreach (BlockCorner blockCorner in groupCorners)
      {
        if ((double) Math.Abs(ypos - blockCorner.corner.y) < 4.0)
        {
          if ((double) blockCorner.corner.x > (double) xpos)
          {
            if (this._rightCorner == null)
              this._rightCorner = blockCorner;
            else if ((double) blockCorner.corner.x < (double) this._rightCorner.corner.x)
              this._rightCorner = blockCorner;
          }
          else if ((double) blockCorner.corner.x < (double) xpos)
          {
            if (this._leftCorner == null)
              this._leftCorner = blockCorner;
            else if ((double) blockCorner.corner.x > (double) this._leftCorner.corner.x)
              this._leftCorner = blockCorner;
          }
        }
      }
    }

    protected override bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      if (!this._onFire && (double) this.data.flammable > 0.5)
      {
        this._fireID = (float) FireManager.GetFireID();
        SFX.Play("ignite", pitch: (Rando.Float(0.3f) - 0.3f));
        this._onFire = true;
        this.alpha = 1f;
      }
      return true;
    }

    public override void AddFire()
    {
      SpriteMap spriteMap = new SpriteMap("surfaceFire", 16, 10);
      spriteMap.AddAnimation("idle", 0.2f, true, 0, 1, 2, 3);
      spriteMap.SetAnimation("idle");
      spriteMap.center = new Vec2(8f, 10f);
      spriteMap.frame = Rando.Int(3);
      this._surfaceFire.Add(spriteMap);
    }

    public override void Initialize()
    {
      if (this._leftCorner == null || this._rightCorner == null)
        Level.Remove((Thing) this);
      else
        this.y = this._leftCorner.corner.y;
    }

    public void Feed(FluidData dat)
    {
      if (this._lava == null && dat.sprite != "" && dat.sprite != null)
      {
        if (this.data.sprite == null)
          this.data.sprite = dat.sprite;
        this._lava = new SpriteMap(dat.sprite, 16, 16);
        this._lava.AddAnimation("idle", 0.1f, true, 0, 1, 2, 3);
        this._lava.SetAnimation("idle");
        this._lava.center = new Vec2(8f, 10f);
        this._lavaAlternate = new SpriteMap(dat.sprite, 16, 16);
        this._lavaAlternate.AddAnimation("idle", 0.1f, true, 2, 3, 0, 1);
        this._lavaAlternate.SetAnimation("idle");
        this._lavaAlternate.center = new Vec2(8f, 10f);
      }
      if (this._lightRect == null && Layer.lighting)
      {
        this._lightRect = new WhiteRectangle(this.x, this.y, this.width, this.height, (double) dat.heat <= 0.0);
        Level.Add((Thing) this._lightRect);
      }
      if ((double) dat.amount > 0.0)
        this._framesSinceFeed = 0;
      this.data.Mix(dat);
      this.data.amount = Maths.Clamp(this.data.amount, 0.0f, this.MaxFluidFill());
      this._wide = this.FeedAmountToDistance(this.data.amount);
      float num1 = this._wide + 4f;
      this._collisionOffset.x = (float) -((double) num1 / 2.0);
      this._collisionSize.x = num1;
      this.FeedEdges();
      if (this._leftCorner != null && this._rightCorner != null && (double) this._wide > (double) this._rightCorner.corner.x - (double) this._leftCorner.corner.x)
      {
        this._wide = this._rightCorner.corner.x - this._leftCorner.corner.x;
        this.x = this._leftCorner.corner.x + (float) (((double) this._rightCorner.corner.x - (double) this._leftCorner.corner.x) / 2.0);
      }
      float num2 = this._wide + 4f;
      this._collisionOffset.x = (float) -((double) num2 / 2.0);
      this._collisionSize.x = num2;
      if (!(this.data.sprite == "water") || this._leftCorner == null)
        return;
      Block block = this._leftCorner.block;
      while (true)
      {
        switch (block)
        {
          case null:
            goto label_19;
          case SnowTileset _:
            if ((double) block.left + 2.0 > (double) this.left && (double) block.right - 2.0 < (double) this.right)
            {
              (block as SnowTileset).Freeze();
              break;
            }
            break;
          case SnowIceTileset _:
            if ((double) block.left + 2.0 > (double) this.left && (double) block.right - 2.0 < (double) this.right)
            {
              (block as SnowIceTileset).Freeze();
              break;
            }
            break;
        }
        block = block.rightBlock;
      }
label_19:;
    }

    public float DistanceToFeedAmount(float distance) => distance / 600f;

    public float FeedAmountToDistance(float feed) => feed * 600f;

    public float MaxFluidFill()
    {
      if (this._topLeftCorner == null || this._topRightCorner == null)
        return 999999f;
      float num = this._topLeftCorner.corner.y + 8f;
      if ((double) this._topRightCorner.corner.y > (double) num)
        num = this._topRightCorner.corner.y + 8f;
      return this.DistanceToFeedAmount((this._leftCorner.corner.y - num) * this._collisionSize.x);
    }

    public void FeedEdges()
    {
      if (this._rightCorner != null && (double) this.right > (double) this._rightCorner.corner.x && this._rightCorner.wallCorner)
        this.x -= this.right - this._rightCorner.corner.x;
      if (this._leftCorner != null && (double) this.left < (double) this._leftCorner.corner.x && this._leftCorner.wallCorner)
        this.x += this._leftCorner.corner.x - this.left;
      if (this._rightCorner != null && (double) this.right > (double) this._rightCorner.corner.x && !this._rightCorner.wallCorner)
      {
        float feedAmount = this.DistanceToFeedAmount(this.right - this._rightCorner.corner.x);
        this.x -= (float) (((double) this.right - (double) this._rightCorner.corner.x) / 2.0);
        if (this._rightStream == null)
          this._rightStream = new FluidStream(this._rightCorner.corner.x - 2f, this.y, new Vec2(1f, 0.0f), 1f);
        this._rightStream.position.y = this.y - this._collisionOffset.y;
        this._rightStream.position.x = this._rightCorner.corner.x + 2f;
        this._rightStream.Feed(this.data.Take(feedAmount));
      }
      this._wide = this.FeedAmountToDistance(this.data.amount);
      float num1 = this._wide + 4f;
      this._collisionOffset.x = (float) -((double) num1 / 2.0);
      this._collisionSize.x = num1;
      if (this._leftCorner != null && (double) this.left < (double) this._leftCorner.corner.x && !this._leftCorner.wallCorner)
      {
        float feedAmount = this.DistanceToFeedAmount(this._leftCorner.corner.x - this.left);
        this.x += (float) (((double) this._leftCorner.corner.x - (double) this.left) / 2.0);
        if (this._leftStream == null)
          this._leftStream = new FluidStream(this._leftCorner.corner.x - 2f, this.y, new Vec2(-1f, 0.0f), 1f);
        this._leftStream.position.y = this.y - this._collisionOffset.y;
        this._leftStream.position.x = this._leftCorner.corner.x - 2f;
        this._leftStream.Feed(this.data.Take(feedAmount));
      }
      this._wide = this.FeedAmountToDistance(this.data.amount);
      float num2 = this._wide + 4f;
      this._collisionOffset.x = (float) -((double) num2 / 2.0);
      this._collisionSize.x = num2;
    }

    public float CalculateDepth()
    {
      float distance = this.FeedAmountToDistance(this.data.amount);
      if ((double) this._wide == 0.0)
        this._wide = 1f / 1000f;
      return Maths.Clamp(distance / this._wide, 1f, 99999f);
    }

    public override void Update()
    {
      ++this._framesSinceFeed;
      this.fluidWave += 0.1f;
      if ((double) this.data.amount < 9.99999974737875E-05)
        Level.Remove((Thing) this);
      if ((double) this.collisionSize.y > 10.0)
      {
        ++this.bubbleWait;
        if (this.bubbleWait > Rando.Int(15, 25))
        {
          for (int index = 0; index < (int) Math.Floor((double) this.collisionSize.x / 16.0); ++index)
          {
            if ((double) Rando.Float(1f) > 0.850000023841858)
              Level.Add((Thing) new TinyBubble(this.left + (float) (index * 16) + Rando.Float(-4f, 4f), this.bottom + Rando.Float(-4f), 0.0f, this.top + 10f));
          }
          this.bubbleWait = 0;
        }
        foreach (PhysicsObject physicsObject in Level.CheckRectAll<PhysicsObject>(this.topLeft, this.bottomRight))
          physicsObject.sleeping = false;
      }
      FluidPuddle fluidPuddle = Level.CheckLine<FluidPuddle>(new Vec2(this.left, this.y), new Vec2(this.right, this.y), (Thing) this);
      if (fluidPuddle != null && (double) fluidPuddle.data.amount < (double) this.data.amount)
      {
        fluidPuddle.active = false;
        float num1 = Math.Min(fluidPuddle.left, this.left);
        float num2 = Math.Max(fluidPuddle.right, this.right);
        this.x = num1 + (float) (((double) num2 - (double) num1) / 2.0);
        this.Feed(fluidPuddle.data);
        Level.Remove((Thing) fluidPuddle);
      }
      if (this._leftStream != null)
      {
        this._leftStream.Update();
        this._leftStream.onFire = this.onFire;
      }
      if (this._rightStream != null)
      {
        this._rightStream.Update();
        this._rightStream.onFire = this.onFire;
      }
      float distance = this.FeedAmountToDistance(this.data.amount);
      if ((double) this._wide == 0.0)
        this._wide = 1f / 1000f;
      float num = Maths.Clamp(distance / this._wide, 1f, 99999f);
      if (this.onFire)
      {
        this._fireRise = Lerp.FloatSmooth(this._fireRise, 1f, 0.1f, 1.2f);
        if (this._framesSinceFeed > 10)
        {
          FluidData data = this.data;
          data.amount = -1f / 1000f;
          this.Feed(data);
          if ((double) this.data.amount <= 0.0)
          {
            this.data.amount = 0.0f;
            this.alpha = Lerp.Float(this.alpha, 0.0f, 0.04f);
          }
          else
            this.alpha = Lerp.Float(this.alpha, 1f, 0.04f);
          if ((double) this.alpha <= 0.0)
            Level.Remove((Thing) this);
        }
      }
      else
      {
        this.alpha = Lerp.Float(this.alpha, 1f, 0.04f);
        if ((double) num < 3.0)
        {
          FluidData data = this.data;
          data.amount = -0.0001f;
          this.Feed(data);
        }
      }
      float depth = this.CalculateDepth();
      if ((double) depth > 4.0 && !this._initializedUpperCorners)
      {
        this._initializedUpperCorners = true;
        foreach (BlockCorner groupCorner in this._block.GetGroupCorners())
        {
          if (this._leftCorner != null && (double) groupCorner.corner.x == (double) this._leftCorner.corner.x && (double) groupCorner.corner.y < (double) this._leftCorner.corner.y)
          {
            if (this._topLeftCorner == null)
              this._topLeftCorner = groupCorner;
            else if ((double) groupCorner.corner.y > (double) this._topLeftCorner.corner.y)
              this._topLeftCorner = groupCorner;
          }
          else if (this._rightCorner != null && (double) groupCorner.corner.x == (double) this._rightCorner.corner.x && (double) groupCorner.corner.y < (double) this._rightCorner.corner.y)
          {
            if (this._topRightCorner == null)
              this._topRightCorner = groupCorner;
            else if ((double) groupCorner.corner.y > (double) this._topRightCorner.corner.y)
              this._topRightCorner = groupCorner;
          }
        }
      }
      if (this._leftStream != null)
        this._leftStream.position.y = this.y - this._collisionOffset.y;
      if (this._rightStream != null)
        this._rightStream.position.y = this.y - this._collisionOffset.y;
      this._collisionOffset.y = -depth;
      this._collisionSize.y = depth;
    }

    public override void Draw()
    {
      Graphics.DrawLine(this.position + new Vec2(-this._collisionOffset.x, (float) ((double) this.collisionOffset.y / 2.0 + 0.5)), this.position + new Vec2(this._collisionOffset.x, (float) ((double) this.collisionOffset.y / 2.0 + 0.5)), new Color(this.data.color) * this.data.transparent, this._collisionSize.y, (Depth) 0.38f);
      Graphics.DrawLine(this.position + new Vec2(-this._collisionOffset.x, (float) ((double) this.collisionOffset.y / 2.0 + 0.5)), this.position + new Vec2(this._collisionOffset.x, (float) ((double) this.collisionOffset.y / 2.0 + 0.5)), new Color(this.data.color), this._collisionSize.y, (Depth) -0.99f);
      if (this._lightRect != null)
      {
        this._lightRect.position = this.topLeft;
        this._lightRect.size = new Vec2(this.width, this.height);
      }
      int num1 = (int) Math.Ceiling((double) this._collisionSize.x / 16.0);
      float num2 = this._collisionSize.x / (float) num1;
      if (this._onFire)
      {
        while (this._surfaceFire.Count < num1)
          this.AddFire();
        float num3 = 0.0f;
        if ((double) this._collisionSize.y > 2.0)
          num3 = 2f;
        for (int index = 0; index < num1; ++index)
        {
          this._surfaceFire[index].alpha = this.alpha;
          this._surfaceFire[index].yscale = this._fireRise;
          this._surfaceFire[index].depth = this.depth + 1;
          Graphics.Draw((Sprite) this._surfaceFire[index], (float) ((double) this.left + 8.0 + (double) index * (double) num2), (float) ((double) this.y + (double) this._collisionOffset.y + 1.0) - num3);
        }
      }
      if (this._lava != null && (double) this.collisionSize.y > 2.0)
      {
        bool flag = false;
        for (int index = 0; index < num1; ++index)
        {
          SpriteMap g = this._lava;
          if (flag)
            g = this._lavaAlternate;
          g.depth = (Depth) 0.38f;
          SpriteMap spriteMap1 = g;
          spriteMap1.depth = spriteMap1.depth + index;
          g.alpha = 0.7f;
          Graphics.DrawWithoutUpdate(g, (float) Math.Round((double) this.left + 8.0 + (double) index * (double) num2), (float) ((double) this.y + (double) this._collisionOffset.y - 4.0));
          g.depth = (Depth) -0.5f;
          SpriteMap spriteMap2 = g;
          spriteMap2.depth = spriteMap2.depth + index;
          g.alpha = 1f;
          Graphics.DrawWithoutUpdate(g, (float) Math.Round((double) this.left + 8.0 + (double) index * (double) num2), (float) ((double) this.y + (double) this._collisionOffset.y - 4.0));
          flag = !flag;
        }
        this._lava.UpdateFrame();
        this._lavaAlternate.UpdateFrame();
      }
      base.Draw();
    }

    public override void Terminate()
    {
      if (this._lightRect != null)
        Level.Remove((Thing) this._lightRect);
      base.Terminate();
    }
  }
}
