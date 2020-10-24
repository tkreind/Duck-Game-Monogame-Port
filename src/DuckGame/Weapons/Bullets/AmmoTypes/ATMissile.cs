﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ATMissile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class ATMissile : AmmoType
  {
    public ATMissile()
    {
      this.accuracy = 0.95f;
      this.range = 850f;
      this.penetration = 0.4f;
      this.bulletSpeed = 7f;
      this.bulletThickness = 2f;
      this.sprite = new Sprite("missile");
      this.sprite.CenterOrigin();
    }

    public override void PopShell(float x, float y, int dir)
    {
      PistolShell pistolShell = new PistolShell(x, y);
      pistolShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) pistolShell);
    }

    public override void OnHit(bool destroyed, Bullet b)
    {
      if (!b.isLocal)
        return;
      if (destroyed)
      {
        new ATMissileShrapnel().MakeNetEffect(b.position, false);
        Random random = (Random) null;
        if (Network.isActive && b.isLocal)
        {
          random = Rando.generator;
          Rando.generator = new Random(NetRand.currentSeed);
        }
        List<Bullet> varBullets = new List<Bullet>();
        for (int index = 0; index < 12; ++index)
        {
          float num = (float) ((double) index * 30.0 - 10.0) + Rando.Float(20f);
          ATMissileShrapnel atMissileShrapnel = new ATMissileShrapnel();
          atMissileShrapnel.range = 15f + Rando.Float(5f);
          Vec2 vec2 = new Vec2((float) Math.Cos((double) Maths.DegToRad(num)), (float) Math.Sin((double) Maths.DegToRad(num)));
          Bullet bullet = new Bullet(b.x + vec2.x * 8f, b.y - vec2.y * 8f, (AmmoType) atMissileShrapnel, num);
          bullet.firedFrom = (Thing) b;
          varBullets.Add(bullet);
          Level.Add((Thing) bullet);
          Level.Add((Thing) Spark.New(b.x + Rando.Float(-8f, 8f), b.y + Rando.Float(-8f, 8f), vec2 + new Vec2(Rando.Float(-0.1f, 0.1f), Rando.Float(-0.1f, 0.1f))));
          Level.Add((Thing) SmallSmoke.New(b.x + vec2.x * 8f + Rando.Float(-8f, 8f), b.y + vec2.y * 8f + Rando.Float(-8f, 8f)));
        }
        if (Network.isActive && b.isLocal)
        {
          Send.Message((NetMessage) new NMFireGun((Gun) null, varBullets, (byte) 0, false), NetMessagePriority.ReliableOrdered);
          varBullets.Clear();
        }
        if (Network.isActive && b.isLocal)
          Rando.generator = random;
        foreach (Window window in Level.CheckCircleAll<Window>(b.position, 30f))
        {
          if (b.isLocal)
            Thing.Fondle((Thing) window, DuckNetwork.localConnection);
          if (Level.CheckLine<Block>(b.position, window.position, (Thing) window) == null)
            window.Destroy((DestroyType) new DTImpact((Thing) b));
        }
        foreach (PhysicsObject physicsObject in Level.CheckCircleAll<PhysicsObject>(b.position, 70f))
        {
          if (b.isLocal && b.owner == null)
            Thing.Fondle((Thing) physicsObject, DuckNetwork.localConnection);
          if ((double) (physicsObject.position - b.position).length < 30.0)
            physicsObject.Destroy((DestroyType) new DTImpact((Thing) b));
          physicsObject.sleeping = false;
          physicsObject.vSpeed = -2f;
        }
        HashSet<ushort> varBlocks = new HashSet<ushort>();
        foreach (BlockGroup blockGroup1 in Level.CheckCircleAll<BlockGroup>(b.position, 50f))
        {
          if (blockGroup1 != null)
          {
            BlockGroup blockGroup2 = blockGroup1;
            List<Block> blockList = new List<Block>();
            foreach (Block block in blockGroup2.blocks)
            {
              if (Collision.Circle(b.position, 28f, block.rectangle))
              {
                block.shouldWreck = true;
                if (block is AutoBlock)
                  varBlocks.Add((block as AutoBlock).blockIndex);
              }
            }
            blockGroup2.Wreck();
          }
        }
        foreach (Block block in Level.CheckCircleAll<Block>(b.position, 28f))
        {
          switch (block)
          {
            case AutoBlock _:
              block.skipWreck = true;
              block.shouldWreck = true;
              if (block is AutoBlock)
              {
                varBlocks.Add((block as AutoBlock).blockIndex);
                continue;
              }
              continue;
            case Door _:
            case VerticalDoor _:
              Level.Remove((Thing) block);
              block.Destroy((DestroyType) new DTRocketExplosion((Thing) null));
              continue;
            default:
              continue;
          }
        }
        if (Network.isActive && b.isLocal && varBlocks.Count > 0)
          Send.Message((NetMessage) new NMDestroyBlocks(varBlocks));
      }
      base.OnHit(destroyed, b);
    }
  }
}
