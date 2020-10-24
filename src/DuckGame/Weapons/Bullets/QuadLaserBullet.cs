﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.QuadLaserBullet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class QuadLaserBullet : Thing, ITeleport
  {
    public StateBinding _positionBinding = (StateBinding) new CompressedVec2Binding("position", doLerp: true);
    public StateBinding _travelBinding = (StateBinding) new CompressedVec2Binding(nameof (travel), 20);
    private Vec2 _travel;
    private SinWave _wave = (SinWave) 0.5f;
    private SinWave _wave2 = (SinWave) 1f;
    public int safeFrames;
    public Duck safeDuck;
    public float timeAlive;

    public Vec2 travel
    {
      get => this._travel;
      set => this._travel = value;
    }

    public QuadLaserBullet(float xpos, float ypos, Vec2 travel)
      : base(xpos, ypos)
    {
      this._travel = travel;
      this.collisionOffset = new Vec2(-1f, -1f);
      this._collisionSize = new Vec2(2f, 2f);
    }

    public override void Update()
    {
      this.timeAlive += 0.016f;
      QuadLaserBullet quadLaserBullet = this;
      quadLaserBullet.position = quadLaserBullet.position + this._travel * 0.5f;
      if (this.isServerForObject && ((double) this.x > (double) Level.current.bottomRight.x + 200.0 || (double) this.x < (double) Level.current.topLeft.x - 200.0))
        Level.Remove((Thing) this);
      foreach (MaterialThing materialThing in Level.CheckRectAll<MaterialThing>(this.topLeft, this.bottomRight))
      {
        if ((this.safeFrames <= 0 || materialThing != this.safeDuck) && materialThing.isServerForObject)
        {
          bool destroyed = materialThing.destroyed;
          materialThing.Destroy((DestroyType) new DTIncinerate((Thing) this));
          if (materialThing.destroyed && !destroyed)
          {
            if (Recorder.currentRecording != null)
              Recorder.currentRecording.LogAction(2);
            if (materialThing is Duck && !(materialThing as Duck).dead)
              Recorder.currentRecording.LogBonus();
          }
        }
      }
      if (this.safeFrames > 0)
        --this.safeFrames;
      base.Update();
    }

    public override void Draw()
    {
      Graphics.DrawRect(this.position + new Vec2(-4f, -4f), this.position + new Vec2(4f, 4f), new Color((int) byte.MaxValue - (int) ((double) this._wave.normalized * 90.0), 137 + (int) ((double) this._wave.normalized * 50.0), 31 + (int) ((double) this._wave.normalized * 30.0)), this.depth);
      Graphics.DrawRect(this.position + new Vec2(-4f, -4f), this.position + new Vec2(4f, 4f), new Color((int) byte.MaxValue, 224 - (int) ((double) this._wave2.normalized * 150.0), 90 + (int) ((double) this._wave2.normalized * 50.0)), this.depth + 1, false);
      base.Draw();
    }
  }
}
