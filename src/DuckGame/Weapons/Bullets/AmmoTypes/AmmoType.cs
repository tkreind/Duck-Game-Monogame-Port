// Decompiled with JetBrains decompiler
// Type: DuckGame.AmmoType
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public abstract class AmmoType
  {
    public float accuracy;
    public float range;
    public float rangeVariation;
    public float penetration;
    public float bulletSpeed = 28f;
    public float speedVariation = 3f;
    public float bulletLength = 100f;
    public Color bulletColor = Color.White;
    public bool rebound;
    public float bulletThickness = 1f;
    public bool affectedByGravity;
    public bool deadly = true;
    public float barrelAngleDegrees;
    public bool immediatelyDeadly;
    public float weight;
    public bool combustable;
    public float impactPower = 2f;
    private static Map<byte, System.Type> _types = new Map<byte, System.Type>();
    public Sprite sprite;
    public System.Type bulletType = typeof (Bullet);

    public static Map<byte, System.Type> indexTypeMap => AmmoType._types;

    public static void InitializeTypes()
    {
      if (MonoMain.moddingEnabled)
      {
        byte key = 0;
        foreach (System.Type sortedType in ManagedContent.AmmoTypes.SortedTypes)
        {
          AmmoType._types[key] = sortedType;
          ++key;
        }
      }
      else
      {
        List<System.Type> list = Editor.GetSubclasses(typeof (AmmoType)).ToList<System.Type>();
        byte key = 0;
        foreach (System.Type type in list)
        {
          AmmoType._types[key] = type;
          ++key;
        }
      }
    }

    public virtual void MakeNetEffect(Vec2 pos, bool fromNetwork = false)
    {
    }

    public virtual void WriteAdditionalData(BitBuffer b)
    {
    }

    public virtual void ReadAdditionalData(BitBuffer b)
    {
    }

    public virtual void PopShell(float x, float y, int dir)
    {
    }

    public Bullet GetBullet(
      float x,
      float y,
      Thing owner = null,
      float angle = -1f,
      Thing firedFrom = null,
      float distance = -1f,
      bool tracer = false,
      bool network = true)
    {
      angle *= -1f;
      Bullet bullet;
      if (this.bulletType == typeof (Bullet))
        bullet = new Bullet(x, y, this, angle, owner, this.rebound, distance, tracer, network);
      else
        bullet = Activator.CreateInstance(this.bulletType, (object) x, (object) y, (object) this, (object) angle, (object) owner, (object) this.rebound, (object) distance, (object) tracer, (object) network) as Bullet;
      bullet.firedFrom = firedFrom;
      bullet.color = this.bulletColor;
      return bullet;
    }

    public virtual Bullet FireBullet(
      Vec2 position,
      Thing owner = null,
      float angle = 0.0f,
      Thing firedFrom = null)
    {
      Bullet bullet = this.GetBullet(position.x, position.y, owner, angle, firedFrom);
      Level.current.AddThing((Thing) bullet);
      return bullet;
    }

    public virtual void OnHit(bool destroyed, Bullet b)
    {
    }
  }
}
