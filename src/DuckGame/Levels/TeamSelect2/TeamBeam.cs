// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamBeam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class TeamBeam : MaterialThing
  {
    private Sprite _selectBeam;
    private float _spawnWait;
    private SinWave _wave = (SinWave) 0.016f;
    private SinWave _wave2 = (SinWave) 0.02f;
    private List<BeamDuck> _ducks = new List<BeamDuck>();
    private List<Thing> _guns = new List<Thing>();
    private float _beamHeight = 180f;
    private float _flash;

    public TeamBeam(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._selectBeam = new Sprite("selectBeam");
      this._selectBeam.alpha = 0.9f;
      this._selectBeam.depth = (Depth) -0.8f;
      this._selectBeam.center = new Vec2((float) (this._selectBeam.w / 2), 0.0f);
      this.depth = (Depth) 0.0f;
      this._collisionOffset = new Vec2((float) -((double) (this._selectBeam.w / 2) * 0.800000011920929), 0.0f);
      this._collisionSize = new Vec2((float) this._selectBeam.w * 0.8f, 180f);
      this.center = new Vec2((float) (this._selectBeam.w / 2));
      this.thickness = 10f;
    }

    public override void Initialize() => base.Initialize();

    public void TakeDuck(Duck d)
    {
      if (this._ducks.Any<BeamDuck>((Func<BeamDuck, bool>) (t => t.duck == d)))
        return;
      float num = (double) d.y >= 100.0 ? 130f : 40f;
      SFX.Play("stepInBeam");
      d.beammode = true;
      d.immobilized = true;
      d.crouch = false;
      d.sliding = false;
      if (d.holdObject != null)
        this._guns.Add((Thing) d.holdObject);
      d.ThrowItem();
      d.solid = false;
      d.grounded = false;
      d.offDir = (sbyte) 1;
      this._ducks.Add(new BeamDuck()
      {
        duck = d,
        entryHeight = num,
        leaving = false,
        entryDir = (double) d.x < (double) this.x ? -1 : 1,
        sin = new SinWave(0.1f),
        sin2 = new SinWave(0.05f)
      });
    }

    public void ClearBeam()
    {
      foreach (BeamDuck duck in this._ducks)
        duck.leaving = true;
    }

    public override void Update()
    {
      this._selectBeam.color = new Color(0.3f, (float) (0.300000011920929 + (double) this._wave2.normalized * 0.200000002980232), (float) (0.5 + (double) this._wave.normalized * 0.300000011920929)) * (1f + this._flash);
      this._flash = Maths.CountDown(this._flash, 0.1f);
      this._spawnWait -= 0.1f;
      if ((double) this._spawnWait < 0.0)
      {
        Level.Add((Thing) new BeamParticle(this.x, this.y + 190f, -0.8f - this._wave.normalized, false, Color.Cyan * 0.8f));
        Level.Add((Thing) new BeamParticle(this.x, this.y + 190f, -0.8f - this._wave2.normalized, true, Color.LightBlue * 0.8f));
        this._spawnWait = 1f;
      }
      foreach (Duck d in Level.CheckRectAll<Duck>(this.topLeft, this.bottomRight))
      {
        if (d.isServerForObject)
          this.TakeDuck(d);
      }
      foreach (Holdable holdable in Level.CheckRectAll<Holdable>(this.topLeft, this.bottomRight))
      {
        if (holdable.isServerForObject)
        {
          if (holdable is RagdollPart)
          {
            Duck captureDuck = (holdable as RagdollPart).doll.captureDuck;
            if (captureDuck != null)
            {
              (holdable as RagdollPart).doll.Unragdoll();
              this.TakeDuck(captureDuck);
            }
          }
          else if (holdable.owner == null && !this._guns.Contains((Thing) holdable))
            this._guns.Add((Thing) holdable);
        }
      }
      int count = this._ducks.Count;
      int num1 = 0;
      float num2 = (float) ((double) this._beamHeight / (double) count / 2.0 + 20.0 * (count > 1 ? 1.0 : 0.0));
      float num3 = (float) (((double) this._beamHeight - (double) num2 * 2.0) / (count > 1 ? (double) (count - 1) : 1.0));
      for (int index = 0; index < this._ducks.Count; ++index)
      {
        BeamDuck duck = this._ducks[index];
        if (duck.leaving)
        {
          duck.duck.solid = true;
          duck.duck.y = MathHelper.Lerp(duck.duck.position.y, duck.entryHeight, 0.35f);
          duck.duck.vSpeed = 0.0f;
          if ((double) Math.Abs(duck.duck.position.y - duck.entryHeight) < 4.0)
          {
            duck.duck.position.y = duck.entryHeight;
            duck.duck.hSpeed = (float) duck.entryDir * 3f;
            duck.duck.vSpeed = 0.0f;
          }
          if ((double) Math.Abs(duck.duck.position.x - this.x) > 24.0)
          {
            duck.duck.gravMultiplier = 1f;
            duck.duck.immobilized = false;
            duck.duck.beammode = false;
            this._ducks.RemoveAt(index);
            --index;
            continue;
          }
        }
        else
        {
          duck.duck.position.x = Lerp.FloatSmooth(duck.duck.position.x, this.position.x + (float) duck.sin2 * 1f, 0.4f);
          duck.duck.position.y = Lerp.FloatSmooth(duck.duck.position.y, (float) ((double) num2 + (double) num3 * (double) index + (double) (float) duck.sin * 2.0), 0.1f);
          duck.duck.vSpeed = 0.0f;
          duck.duck.hSpeed = 0.0f;
          duck.duck.gravMultiplier = 0.0f;
        }
        if (duck.duck.inputProfile.Pressed("QUACK") && (double) Math.Abs(duck.duck.position.x - this.x) < 2.0)
          duck.leaving = true;
        ++num1;
      }
      for (int index = 0; index < this._guns.Count; ++index)
      {
        Thing gun = this._guns[index];
        gun.vSpeed = 0.0f;
        gun.hSpeed = 0.0f;
        if ((double) Math.Abs(this.position.x - gun.position.x) < 6.0)
        {
          gun.position = Vec2.Lerp(gun.position, new Vec2(this.position.x, gun.position.y - 3f), 0.1f);
          gun.alpha = Maths.LerpTowards(gun.alpha, 0.0f, 0.1f);
          if ((double) gun.alpha <= 0.0)
          {
            gun.y = -200f;
            this._guns.RemoveAt(index);
            --index;
          }
        }
        else
          gun.position = Vec2.Lerp(gun.position, new Vec2(this.position.x, gun.position.y), 0.2f);
      }
      base.Update();
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      for (int index = 0; index < 6; ++index)
        Level.Add((Thing) new GlassParticle(hitPos.x, hitPos.y, new Vec2(Rando.Float(-1f, 1f), Rando.Float(-1f, 1f))));
      this._flash = 1f;
      return true;
    }

    public override void Draw()
    {
      base.Draw();
      for (int index = 0; index < 6; ++index)
        Graphics.Draw(this._selectBeam, this.x, this.y + (float) (index * 32));
    }
  }
}
