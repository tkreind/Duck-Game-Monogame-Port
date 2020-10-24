// Decompiled with JetBrains decompiler
// Type: DuckGame.Bullet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Bullet : Thing
  {
    private new NetworkConnection _connection;
    protected Teleporter _teleporter;
    public AmmoType ammo;
    public bool randomDir;
    public Vec2 start = new Vec2();
    private Vec2 _realEnd;
    public Vec2 travelStart = new Vec2();
    public Vec2 travelEnd = new Vec2();
    public Vec2 travel = new Vec2();
    public Vec2 willCol = new Vec2();
    public bool create = true;
    public bool col;
    public bool traced;
    public bool rebound;
    protected bool _tracer;
    private bool _tracePhase;
    private bool _gravityAffected;
    private float _travelTime;
    protected float _bulletDistance;
    protected float _bulletLength = 100f;
    protected float _bulletSpeed = 28f;
    protected Vec2 _actualStart;
    private bool _didPenetrate;
    public Color color = Color.White;
    protected Thing _firedFrom;
    protected Profile _contributeToAccuracy;
    public static bool isRebound;
    public float range;
    private PhysicalBullet _physicalBullet;
    public Vec2 travelDirNormalized;
    private int _totalSteps;
    private List<MaterialThing> _currentlyImpacting = new List<MaterialThing>();
    private int hitsLogged;
    protected float _totalLength;
    protected Vec2 drawStart;
    protected Vec2 drawEnd;
    private List<Vec2> prev = new List<Vec2>();
    private List<float> vel = new List<float>();
    private float _totalArc;
    private bool doneTravelling;
    private float startpoint;
    private float drawdist;
    private bool _initializedDraw;

    public new NetworkConnection connection
    {
      get => this._connection;
      set => this._connection = value;
    }

    public Vec2 end
    {
      get => this._realEnd;
      set => this._realEnd = value;
    }

    public bool gravityAffected
    {
      get => this._gravityAffected;
      set => this._gravityAffected = value;
    }

    public float travelTime => this._travelTime;

    public float bulletDistance => this._bulletDistance;

    public float bulletSpeed => this._bulletSpeed;

    public bool didPenetrate => this._didPenetrate;

    public Thing firedFrom
    {
      get => this._firedFrom;
      set => this._firedFrom = value;
    }

    public Profile contributeToAccuracy
    {
      get => this._contributeToAccuracy;
      set => this._contributeToAccuracy = value;
    }

    public Bullet(
      float xval,
      float yval,
      AmmoType type,
      float ang = -1f,
      Thing owner = null,
      bool rbound = false,
      float distance = -1f,
      bool tracer = false,
      bool network = true)
      : base()
    {
      this._gravityAffected = type.affectedByGravity;
      this._bulletLength = type.bulletLength;
      this.depth = (Depth) -0.1f;
      if (!tracer)
      {
        this._tracePhase = true;
        if (owner != null && owner is Duck duck)
        {
          this._contributeToAccuracy = duck.profile;
          if ((double) Highlights.highlightRatingMultiplier != 0.0)
            ++duck.profile.stats.bulletsFired;
        }
      }
      this.x = xval;
      this.y = yval;
      this.ammo = type;
      this.rebound = rbound;
      this._owner = owner;
      this.angle = ang;
      this._tracer = tracer;
      this.range = type.range - Rando.Float(type.rangeVariation);
      if ((double) distance > 0.0)
        this.range = distance;
      this._bulletSpeed = type.bulletSpeed + Rando.Float(type.speedVariation);
      if (this.traced)
        return;
      if (this.randomDir)
        this.angle = Rando.Float(360f);
      this.angle += (float) (((double) Rando.Float(30f) - 15.0) * (1.0 - (double) this.ammo.accuracy));
      this.travel.x = (float) Math.Cos((double) Maths.DegToRad(this.angle)) * this.range;
      this.travel.y = (float) -Math.Sin((double) Maths.DegToRad(this.angle)) * this.range;
      this.start = new Vec2(this.x, this.y);
      this._actualStart = this.start;
      this.end = this.start + this.travel;
      this.travelDirNormalized = this.end - this.start;
      this.travelDirNormalized.Normalize();
      if (this._gravityAffected)
      {
        this.hSpeed = this.travelDirNormalized.x * this._bulletSpeed;
        this.vSpeed = this.travelDirNormalized.y * this._bulletSpeed;
        this._physicalBullet = new PhysicalBullet();
        this._physicalBullet.weight = this.ammo.weight;
      }
      if (this._tracer)
      {
        this.TravelBullet();
      }
      else
      {
        this.travelStart = this.start;
        this.travelEnd = this.end;
        this._totalLength = (this.end - this.start).length;
        this._tracePhase = false;
      }
      this.traced = true;
    }

    protected virtual void Rebound(Vec2 pos, float dir, float rng)
    {
      Bullet bullet = this.ammo.GetBullet(pos.x, pos.y, angle: (-dir), firedFrom: this.firedFrom, distance: rng, tracer: this._tracer);
      bullet._teleporter = this._teleporter;
      Level.Add((Thing) bullet);
    }

    public virtual void OnCollide(Vec2 pos, Thing t, bool willBeStopped)
    {
    }

    private bool RaycastBullet(
      Vec2 p1,
      Vec2 p2,
      Vec2 dir,
      float length,
      List<MaterialThing> collideList)
    {
      int num1 = (int) Math.Ceiling((double) length);
      Vec2 vec2_1 = p1;
      Vec2 zero = Vec2.Zero;
      bool willBeStopped = false;
      do
      {
        --num1;
        --this._totalSteps;
        IEnumerable<MaterialThing> source = Level.current.CollisionPointAll<MaterialThing>(vec2_1);
        if (!this._tracer)
        {
          for (int index = 0; index < this._currentlyImpacting.Count; ++index)
          {
            MaterialThing materialThing = this._currentlyImpacting[index];
            if (!source.Contains<MaterialThing>(materialThing))
            {
              if (this.ammo.deadly)
                materialThing.DoExitHit(this, vec2_1);
              this._currentlyImpacting.RemoveAt(index);
              --index;
            }
          }
        }
        Duck owner = this._owner as Duck;
        foreach (MaterialThing materialThing in source)
        {
          if ((materialThing != this._owner && (!(this._owner is Duck) || !(this._owner as Duck).ExtendsTo((Thing) materialThing)) || this.ammo.immediatelyDeadly) && ((owner == null || materialThing != owner.holdObject) && materialThing != this._teleporter && (!(materialThing is Teleporter) || !this._tracer)))
          {
            bool flag1 = false;
            if (DevConsole.shieldMode && materialThing is Duck && (double) (materialThing as Duck)._shieldCharge > 0.600000023841858)
            {
              flag1 = true;
              willBeStopped = true;
            }
            if (materialThing is Duck && !this._tracer && this._contributeToAccuracy != null)
            {
              if ((double) Highlights.highlightRatingMultiplier != 0.0)
                ++this._contributeToAccuracy.stats.bulletsThatHit;
              this._contributeToAccuracy = (Profile) null;
            }
            if (!flag1 && (double) materialThing.thickness >= 0.0 && !this._currentlyImpacting.Contains(materialThing))
            {
              if (!this._tracer && !this._tracePhase)
              {
                if (this.ammo.deadly)
                {
                  willBeStopped = materialThing.DoHit(this, vec2_1);
                  if (materialThing is Duck && !this.rebound && ((materialThing as Duck).dead && !(this.ammo is ATShrapnel)) && (((double) this.travelDirNormalized.x > 0.300000011920929 || (double) this.travelDirNormalized.x < -0.300000011920929) && ((double) this.travelDirNormalized.y > 0.400000005960464 || (double) this.travelDirNormalized.y < -0.400000005960464)))
                    ++Global.data.angleShots;
                }
                else
                {
                  ImpactedFrom from = (double) vec2_1.y < (double) materialThing.top + 1.0 || (double) vec2_1.y > (double) materialThing.bottom - 1.0 ? ((double) this.travelDirNormalized.y > 0.0 ? ImpactedFrom.Top : ImpactedFrom.Bottom) : ((double) this.travelDirNormalized.x > 0.0 ? ImpactedFrom.Left : ImpactedFrom.Right);
                  this._physicalBullet.position = vec2_1;
                  this._physicalBullet.velocity = this.velocity;
                  if (materialThing is Block || materialThing is IPlatform && (double) this.travelDirNormalized.y > 0.0)
                    materialThing.SolidImpact((MaterialThing) this._physicalBullet, from);
                  else if ((double) materialThing.thickness > (double) this.ammo.penetration)
                    materialThing.Impact((MaterialThing) this._physicalBullet, from, false);
                  this.velocity = this._physicalBullet.velocity;
                  willBeStopped = (double) materialThing.thickness > (double) this.ammo.penetration;
                }
                if (Recorder.currentRecording != null && this.hitsLogged < 1)
                {
                  Recorder.currentRecording.LogAction();
                  ++this.hitsLogged;
                }
              }
              else
                willBeStopped = (double) materialThing.thickness > (double) this.ammo.penetration;
              this.OnCollide(vec2_1, (Thing) materialThing, willBeStopped);
              this._currentlyImpacting.Add(materialThing);
              if ((double) materialThing.thickness > 1.5 && (double) this.ammo.penetration >= (double) materialThing.thickness)
              {
                this._didPenetrate = true;
                this.position = vec2_1;
                if (this.isLocal)
                  this.OnHit(false);
              }
            }
            if (willBeStopped)
            {
              willBeStopped = true;
              bool flag2 = false;
              if (materialThing is Teleporter)
              {
                this._teleporter = materialThing as Teleporter;
                if (this._teleporter.link != null)
                {
                  float rng = this._totalLength - (this._actualStart - vec2_1).length;
                  if ((double) rng > 0.0)
                  {
                    float dir1 = Maths.PointDirection(this._actualStart, vec2_1);
                    Vec2 vec2_2 = this._teleporter.position - vec2_1;
                    this._teleporter = this._teleporter.link;
                    this.Rebound(this._teleporter.position - vec2_2, dir1, rng);
                  }
                  flag2 = true;
                }
              }
              else if (this.rebound && materialThing is Block)
              {
                float length1 = (this._actualStart - vec2_1).length;
                if ((double) length1 > 2.0)
                {
                  float rng = this._totalLength - length1;
                  if ((double) rng > 0.0)
                  {
                    Vec2 vec2_2 = Vec2.Zero;
                    Vec2 pos = vec2_1;
                    float num2 = 999.9f;
                    if ((double) vec2_1.y > (double) materialThing.top)
                    {
                      float num3 = Math.Abs(vec2_1.y - materialThing.top);
                      if ((double) num3 < (double) num2)
                      {
                        vec2_2 = new Vec2(0.0f, -1f);
                        pos = new Vec2(vec2_1.x, materialThing.top - 1f);
                        num2 = num3;
                      }
                    }
                    if ((double) vec2_1.y < (double) materialThing.bottom)
                    {
                      float num3 = Math.Abs(vec2_1.y - materialThing.bottom);
                      if ((double) num3 < (double) num2)
                      {
                        vec2_2 = new Vec2(0.0f, 1f);
                        pos = new Vec2(vec2_1.x, materialThing.bottom + 1f);
                        num2 = num3;
                      }
                    }
                    if ((double) vec2_1.x > (double) materialThing.left)
                    {
                      float num3 = Math.Abs(vec2_1.x - materialThing.left);
                      if ((double) num3 < (double) num2)
                      {
                        vec2_2 = new Vec2(1f, 0.0f);
                        pos = new Vec2(materialThing.left - 1f, vec2_1.y);
                        num2 = num3;
                      }
                    }
                    if ((double) vec2_1.x < (double) materialThing.right)
                    {
                      if ((double) Math.Abs(vec2_1.x - materialThing.right) < (double) num2)
                      {
                        vec2_2 = new Vec2(-1f, 0.0f);
                        pos = new Vec2(materialThing.right + 1f, vec2_1.y);
                      }
                    }
                    if (vec2_2 == Vec2.Zero)
                      vec2_2 = new Vec2(0.0f, -1f);
                    Vec2 travelDirNormalized = this.travelDirNormalized;
                    float dir1 = Maths.PointDirection(Vec2.Zero, travelDirNormalized - vec2_2 * 2f * Vec2.Dot(travelDirNormalized, vec2_2));
                    this.Rebound(pos, dir1, rng);
                  }
                  flag2 = true;
                }
                else
                  willBeStopped = false;
              }
              this.end = vec2_1;
              this.travelEnd = this.end;
              this.doneTravelling = true;
              this.position = vec2_1;
              this.OnHit(!flag2);
              break;
            }
          }
        }
        vec2_1 += this.travelDirNormalized;
      }
      while (num1 > 0 && !willBeStopped);
      return willBeStopped;
    }

    protected virtual void OnHit(bool destroyed) => this.ammo.OnHit(destroyed, this);

    private void TravelBullet()
    {
      this.travelDirNormalized = this.end - this.start;
      if ((double) this.travelDirNormalized.x == double.NaN || (double) this.travelDirNormalized.y == double.NaN)
      {
        this.travelDirNormalized = Vec2.One;
      }
      else
      {
        float length = this.travelDirNormalized.length;
        if ((double) length <= 1.0 / 1000.0)
          return;
        this.travelDirNormalized.Normalize();
        this._totalSteps = (int) Math.Ceiling((double) length);
        List<MaterialThing> collideList = new List<MaterialThing>();
        Stack<TravelInfo> travelInfoStack = new Stack<TravelInfo>();
        travelInfoStack.Push(new TravelInfo(this.start, this.end, length));
        int num = 0;
        while (travelInfoStack.Count > 0 && num < 128)
        {
          ++num;
          TravelInfo travelInfo = travelInfoStack.Pop();
          if (Level.current.CollisionLine<MaterialThing>(travelInfo.p1, travelInfo.p2) != null)
          {
            if ((double) travelInfo.length < 8.0)
            {
              if (this.RaycastBullet(travelInfo.p1, travelInfo.p2, this.travelDirNormalized, travelInfo.length, collideList))
                break;
            }
            else
            {
              float len = travelInfo.length * 0.5f;
              Vec2 vec2 = travelInfo.p1 + this.travelDirNormalized * len;
              travelInfoStack.Push(new TravelInfo(vec2, travelInfo.p2, len));
              travelInfoStack.Push(new TravelInfo(travelInfo.p1, vec2, len));
            }
          }
        }
      }
    }

    public override void Update()
    {
      if (this._tracer)
        Level.Remove((Thing) this);
      if (!this._initializedDraw)
      {
        this.prev.Add(this.start);
        this.vel.Add(0.0f);
        this._initializedDraw = true;
      }
      this._travelTime += Maths.IncFrameTimer();
      this._bulletDistance += this._bulletSpeed;
      this.startpoint = Maths.Clamp(this._bulletDistance - this._bulletLength, 0.0f, 99999f);
      float num = this._bulletDistance;
      if ((double) num > (double) this._totalLength)
        num = this._totalLength;
      if ((double) this.startpoint > (double) num)
      {
        this.startpoint = num;
        Level.Remove((Thing) this);
      }
      if (this._gravityAffected)
      {
        this.end = this.start + this.velocity;
        this.vSpeed += PhysicsObject.gravity;
        if ((double) this.vSpeed > 8.0)
          this.vSpeed = 8f;
        if (!this.doneTravelling)
        {
          this.prev.Add(this.end);
          float length = (this.end - this.start).length;
          this._totalArc += length;
          this.vel.Add(length);
        }
      }
      else
        this.end = this.start + this.travelDirNormalized * this._bulletSpeed;
      if (!this.doneTravelling)
      {
        this.TravelBullet();
        this._totalLength = (this.travelStart - this.travelEnd).length;
        if ((double) this._bulletDistance >= (double) this._totalLength)
          this.doneTravelling = true;
        if (this._gravityAffected && this.doneTravelling)
        {
          this.prev[this.prev.Count - 1] = this.travelEnd;
          float length = (this.travelEnd - this.start).length;
          this._totalArc += length;
          this.vel[this.vel.Count - 1] = length;
        }
      }
      else
      {
        this.alpha -= 0.1f;
        if ((double) this.alpha <= 0.0)
          Level.Remove((Thing) this);
      }
      this.start = this.end;
      if ((double) num > (double) this._totalLength)
        num = this._totalLength;
      if ((double) this.startpoint > (double) num)
        this.startpoint = num;
      this.drawStart = this.travelStart + this.travelDirNormalized * this.startpoint;
      this.drawEnd = this.travelStart + this.travelDirNormalized * num;
      this.drawdist = num;
    }

    public Vec2 GetPointOnArc(float distanceBack)
    {
      float num1 = 0.0f;
      Vec2 vec2 = this.prev.Last<Vec2>();
      for (int index = this.prev.Count - 1; index > 0; --index)
      {
        if (index == 0)
          return this.prev[index];
        float num2 = num1;
        num1 += this.vel[index];
        if ((double) num1 >= (double) distanceBack)
        {
          if (index == 1)
            return this.prev[index - 1];
          float num3 = (distanceBack - num2) / this.vel[index];
          return this.prev[index] + (this.prev[index - 1] - this.prev[index]) * num3;
        }
        vec2 = this.prev[index];
      }
      return vec2;
    }

    public override void Draw()
    {
      if (this._tracer || (double) this._bulletDistance <= 0.100000001490116)
        return;
      if (this.gravityAffected)
      {
        if (this.prev.Count < 1)
          return;
        int num = (int) Math.Ceiling((double) (this.drawdist - this.startpoint) / 8.0);
        Vec2 p2 = this.prev.Last<Vec2>();
        for (int index = 0; index < num; ++index)
        {
          Vec2 pointOnArc = this.GetPointOnArc((float) (index * 8));
          Graphics.DrawLine(pointOnArc, p2, this.color * (float) (1.0 - (double) index / (double) num) * this.alpha, this.ammo.bulletThickness, (Depth) 0.9f);
          if (pointOnArc == this.prev.First<Vec2>())
            break;
          p2 = pointOnArc;
          if (index == 0 && this.ammo.sprite != null && !this.doneTravelling)
          {
            this.ammo.sprite.depth = (Depth) 1f;
            this.ammo.sprite.angleDegrees = -Maths.PointDirection(Vec2.Zero, this.travelDirNormalized);
            Graphics.Draw(this.ammo.sprite, p2.x, p2.y);
          }
        }
      }
      else
      {
        if (this.ammo.sprite != null && !this.doneTravelling)
        {
          this.ammo.sprite.depth = (Depth) 1f;
          this.ammo.sprite.angleDegrees = -Maths.PointDirection(Vec2.Zero, this.travelDirNormalized);
          Graphics.Draw(this.ammo.sprite, this.drawEnd.x, this.drawEnd.y);
        }
        float length = (this.drawStart - this.drawEnd).length;
        float val = 0.0f;
        float num1 = (float) (1.0 / ((double) length / 8.0));
        float num2 = 1f;
        float num3 = 8f;
        while (true)
        {
          bool flag = false;
          if ((double) val + (double) num3 > (double) length)
          {
            num3 = length - Maths.Clamp(val, 0.0f, 99f);
            flag = true;
          }
          num2 -= num1;
          --Graphics.currentDrawIndex;
          Graphics.DrawLine(this.drawStart + this.travelDirNormalized * length - this.travelDirNormalized * val, this.drawStart + this.travelDirNormalized * length - this.travelDirNormalized * (val + num3), this.color * num2, this.ammo.bulletThickness, this.depth);
          if (!flag)
            val += 8f;
          else
            break;
        }
      }
    }
  }
}
