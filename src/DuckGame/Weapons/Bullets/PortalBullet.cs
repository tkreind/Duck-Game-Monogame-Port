// Decompiled with JetBrains decompiler
// Type: DuckGame.PortalBullet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DuckGame
{
  public class PortalBullet : Bullet
  {
    private Texture2D _beem;
    private float _thickness;

    public PortalBullet(
      float xval,
      float yval,
      AmmoType type,
      float ang = -1f,
      Thing owner = null,
      bool rbound = false,
      float distance = -1f,
      float thick = 0.3f)
      : base(xval, yval, type, ang, owner, rbound, distance)
    {
      this._thickness = thick;
      this._beem = Content.Load<Texture2D>("laserBeam");
    }

    public override void OnCollide(Vec2 pos, Thing t, bool willBeStopped)
    {
      if (!(t is Block) || !willBeStopped || !(this.owner is PortalGun owner))
        return;
      if (!(Level.current.things[typeof (Portal)].FirstOrDefault<Thing>((Func<Thing, bool>) (p => (p as Portal).gun == this.owner)) is Portal portal))
      {
        portal = new Portal(owner);
        Level.Add((Thing) portal);
      }
      Vec2 p1 = pos - this.travelDirNormalized;
      PortalDoor door = new PortalDoor();
      door.center = pos;
      if ((double) Math.Abs(this.travelDirNormalized.y) < 0.5)
      {
        door.horizontal = false;
        door.point1 = new Vec2(pos + new Vec2(0.0f, -16f));
        door.point2 = new Vec2(pos + new Vec2(0.0f, 16f));
        AutoBlock autoBlock1 = Level.CheckLine<AutoBlock>(p1, p1 + new Vec2(0.0f, 16f));
        if (autoBlock1 != null && (double) autoBlock1.top < (double) door.point2.y)
          door.point2.y = autoBlock1.top;
        AutoBlock autoBlock2 = Level.CheckLine<AutoBlock>(p1, p1 + new Vec2(0.0f, -16f));
        if (autoBlock2 != null && (double) autoBlock2.bottom > (double) door.point1.y)
          door.point1.y = autoBlock2.bottom;
      }
      else
      {
        door.horizontal = true;
        door.point1 = new Vec2(pos + new Vec2(-16f, 0.0f));
        door.point2 = new Vec2(pos + new Vec2(16f, 0.0f));
        AutoBlock autoBlock1 = Level.CheckLine<AutoBlock>(p1, p1 + new Vec2(16f, 0.0f));
        if (autoBlock1 != null && (double) autoBlock1.left < (double) door.point2.x)
          door.point2.x = autoBlock1.left;
        AutoBlock autoBlock2 = Level.CheckLine<AutoBlock>(p1, p1 + new Vec2(-16f, 0.0f));
        if (autoBlock2 != null && (double) autoBlock2.right > (double) door.point1.x)
          door.point1.x = autoBlock2.right;
      }
      portal.AddPortalDoor(door);
    }

    public override void Draw()
    {
      if (this._tracer || (double) this._bulletDistance <= 0.100000001490116)
        return;
      float length = (this.drawStart - this.drawEnd).length;
      float val = 0.0f;
      float num1 = (float) (1.0 / ((double) length / 8.0));
      float num2 = 0.0f;
      float num3 = 8f;
      while (true)
      {
        bool flag = false;
        if ((double) val + (double) num3 > (double) length)
        {
          num3 = length - Maths.Clamp(val, 0.0f, 99f);
          flag = true;
        }
        num2 += num1;
        DuckGame.Graphics.DrawTexturedLine((Tex2D) this._beem, this.drawStart + this.travelDirNormalized * val, this.drawStart + this.travelDirNormalized * (val + num3), Color.White * num2, this._thickness, (Depth) 0.6f);
        if (!flag)
          val += 8f;
        else
          break;
      }
    }
  }
}
