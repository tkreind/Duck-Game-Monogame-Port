// Decompiled with JetBrains decompiler
// Type: DuckGame.ForceWave
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ForceWave : Thing
  {
    private float _alphaSub;
    private float _speed;
    private float _speedv;
    private List<Thing> _hits = new List<Thing>();

    public ForceWave(
      float xpos,
      float ypos,
      int dir,
      float alphaSub,
      float speed,
      float speedv,
      Duck own)
      : base(xpos, ypos)
    {
      this.offDir = (sbyte) dir;
      this.graphic = new Sprite("sledgeForce");
      this.center = new Vec2((float) this.graphic.w, (float) this.graphic.h);
      this._alphaSub = alphaSub;
      this._speed = speed;
      this._speedv = speedv;
      this._collisionSize = new Vec2(6f, 30f);
      this._collisionOffset = new Vec2(-3f, -15f);
      this.graphic.flipH = this.offDir <= (sbyte) 0;
      this.owner = (Thing) own;
      this.depth = new Depth(-0.7f);
    }

    public override void Update()
    {
      if ((double) this.alpha > 0.100000001490116)
      {
        foreach (PhysicsObject physicsObject in Level.CheckRectAll<PhysicsObject>(this.topLeft, this.bottomRight))
        {
          if (!this._hits.Contains((Thing) physicsObject) && physicsObject != this.owner)
          {
            if (this.owner != null)
              Thing.Fondle((Thing) physicsObject, this.owner.connection);
            if (physicsObject is Grenade grenade)
              grenade.PressAction();
            physicsObject.hSpeed = (float) (((double) this._speed - 3.0) * (double) this.offDir * 1.5 + (double) this.offDir * 4.0) * this.alpha;
            physicsObject.vSpeed = (this._speedv - 4.5f) * this.alpha;
            physicsObject.clip.Add(this.owner as MaterialThing);
            if (!physicsObject.destroyed)
              physicsObject.Destroy((DestroyType) new DTImpact((Thing) this));
            this._hits.Add((Thing) physicsObject);
          }
        }
        foreach (Door door in Level.CheckRectAll<Door>(this.topLeft, this.bottomRight))
        {
          if (this.owner != null)
            Thing.Fondle((Thing) door, this.owner.connection);
          if (!door.destroyed)
            door.Destroy((DestroyType) new DTImpact((Thing) this));
        }
        foreach (Window window in Level.CheckRectAll<Window>(this.topLeft, this.bottomRight))
        {
          if (this.owner != null)
            Thing.Fondle((Thing) window, this.owner.connection);
          if (!window.destroyed)
            window.Destroy((DestroyType) new DTImpact((Thing) this));
        }
      }
      this.x += (float) this.offDir * this._speed;
      this.y += this._speedv;
      this.alpha -= this._alphaSub;
      if ((double) this.alpha > 0.0)
        return;
      Level.Remove((Thing) this);
    }
  }
}
