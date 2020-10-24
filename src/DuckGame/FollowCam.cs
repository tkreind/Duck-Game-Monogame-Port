// Decompiled with JetBrains decompiler
// Type: DuckGame.FollowCam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class FollowCam : Camera
  {
    private HashSet<Thing> _follow = new HashSet<Thing>();
    private Dictionary<Thing, Vec2> _prevPositions = new Dictionary<Thing, Vec2>();
    private float _viewSize;
    public float manualViewSize = -1f;
    private Vec2 _center = new Vec2();
    private float _lerpSpeed = 1f;
    private float _lerpBorder;
    private float border = 80f;
    public float minSize = 60f;
    private float _prevCenterMoveX;
    private float _prevCenterMoveY;
    private float _prevViewSize;
    private bool _startedFollowing;
    private Vec2 _averagePosition = Vec2.Zero;
    private bool _startCentered = true;
    private float _lerpMult = 1f;
    private float _speed = 1f;
    private bool immediate;
    private bool _allowWarps;
    private bool _checkedZoom;
    private float _zoomMult = 1f;
    private bool _skipResize;
    private bool _overFollow;
    private CameraBounds _bounds;

    public float viewSize => this._viewSize;

    public float lerpSpeed
    {
      get => this._lerpSpeed;
      set => this._lerpSpeed = value;
    }

    public bool startCentered
    {
      get => this._startCentered;
      set => this._startCentered = value;
    }

    public float lerpMult
    {
      get => this._lerpMult;
      set => this._lerpMult = value;
    }

    public float speed
    {
      get => this._speed;
      set => this._speed = value;
    }

    public void Add(Thing t)
    {
      if (t == null)
        return;
      this._follow.Add(t);
      this._startedFollowing = true;
    }

    public void Clear()
    {
      this._follow.Clear();
      this._prevPositions.Clear();
    }

    public void Remove(Thing t)
    {
      this._follow.Remove(t);
      if (t == null || !this._prevPositions.ContainsKey(t))
        return;
      this._prevPositions.Remove(t);
    }

    public bool Contains(Thing t) => this._follow.Contains(t);

    public void Adjust()
    {
      Level.current.CalculateBounds();
      this.Update();
      this.immediate = true;
      this.Update();
      this.immediate = false;
    }

    public override void Update()
    {
      bool flag = true;
      if (this._startedFollowing)
      {
        if (!this._checkedZoom)
        {
          this._checkedZoom = true;
          CameraZoom cameraZoom = (CameraZoom)Level.First<CameraZoom>();
          if (cameraZoom != null)
          {
            this._zoomMult = cameraZoom.zoomMult;
            this._overFollow = (bool) cameraZoom.overFollow;
            this._allowWarps = (bool) cameraZoom.allowWarps;
          }
          CustomCamera customCamera = (CustomCamera)Level.First<CustomCamera>();
          if (customCamera != null)
          {
            this.width = (float) customCamera.wide.value;
            this.height = (float) customCamera.wide.value * (9f / 16f);
            this.center = customCamera.position;
            this._skipResize = true;
          }
          if (Level.First<CameraBounds>() != null)
            this.Adjust();
        }
        flag = false;
        foreach (Duck duck in Level.current.things[typeof (Duck)])
        {
          if (!duck.dead)
            flag = true;
          if (Network.isActive && !duck.dead)
            this.Add((Thing) duck);
        }
        if (Network.isActive)
        {
          foreach (RCCar rcCar in Level.current.things[typeof (RCCar)])
          {
            if (rcCar.receivingSignal)
              this.Add((Thing) rcCar);
            else
              this.Remove((Thing) rcCar);
          }
        }
        foreach (Thing thing in this._follow)
        {
          if (thing is Duck duck && !duck.dead)
            flag = true;
        }
        if (Network.isActive)
        {
          for (int index = 0; index < this._follow.Count; ++index)
          {
            if (!this._follow.ElementAt<Thing>(index).active)
            {
              this._follow.Remove(this._follow.ElementAt<Thing>(index));
              --index;
            }
          }
        }
      }
      if (this._skipResize)
        return;
      float lerpMult = this.lerpMult;
      if ((double) this._lerpSpeed > 0.899999976158142)
        this.lerpMult = 1.8f;
      this.border += (float) (((double) this._lerpBorder - (double) this.border) * ((double) this._lerpSpeed * 16.0 * (double) this._lerpMult)) * this._speed;
      if (this.immediate)
        this.border = this._lerpBorder;
      float num1 = 99999f;
      float num2 = -99999f;
      float num3 = 99999f;
      float num4 = -99999f;
      Vec2 zero = Vec2.Zero;
      List<Thing> thingList = new List<Thing>();
      foreach (Thing key in this._follow)
      {
        Vec2 current = key.cameraPosition;
        if (key.removeFromLevel)
          thingList.Add(key);
        if (this._prevPositions.ContainsKey(key))
        {
          Vec2 prevPosition = this._prevPositions[key];
          if (this._overFollow)
          {
            Vec2 vec2 = (key.cameraPosition - prevPosition) * 24f;
            if ((double) vec2.length > 100.0)
              vec2 = vec2.normalized * 100f;
            Vec2 to = key.cameraPosition + vec2;
            current = Lerp.Vec2Smooth(current, to, 0.3f);
          }
          if ((double) (prevPosition - current).length > 800.0 && !this._allowWarps)
          {
            current.x = prevPosition.x;
            current.y = prevPosition.y;
          }
          else
            this._prevPositions[key] = key.cameraPosition;
        }
        else
          this._prevPositions[key] = key.cameraPosition;
        if ((double) current.x < (double) num1)
          num1 = current.x;
        if ((double) current.x > (double) num2)
          num2 = current.x;
        if ((double) current.y < (double) num3)
          num3 = current.y;
        if ((double) current.y > (double) num4)
          num4 = current.y;
        zero += current;
      }
      foreach (Thing t in thingList)
        this.Remove(t);
      float num5 = Level.current.topLeft.y - 64f;
      float y = Level.current.bottomRight.y;
      float x1 = Level.current.topLeft.x;
      float x2 = Level.current.bottomRight.x;
      if ((double) num4 > (double) y)
      {
        num4 = y;
        if ((double) num3 > (double) num4)
          num3 = num4;
      }
      if ((double) num3 < (double) num5)
      {
        num3 = num5;
        if ((double) num4 < (double) num3)
          num4 = num3;
      }
      if ((double) num1 < (double) x1)
      {
        num1 = x1;
        if ((double) num2 < (double) num1)
          num2 = num1;
      }
      if ((double) num2 > (double) x2)
      {
        num2 = x2;
        if ((double) num1 > (double) num2)
          num1 = num2;
      }
      float num6 = num3 - this.border;
      float num7 = num4 + this.border;
      float num8 = num1 - this.border;
      float num9 = num2 + this.border;
      float num10 = (float) (((double) num8 + (double) num9) / 2.0);
      float num11 = (float) (((double) num6 + (double) num7) / 2.0);
      float num12 = (float) MonoMain.screenWidth / (float) MonoMain.screenHeight;
      float num13 = Math.Abs(num8 - num9);
      float num14 = Math.Abs(num6 - num7);
      if ((double) this._lerpSpeed > 0.899999976158142)
      {
        num13 = Level.current.bottomRight.x - Level.current.topLeft.x;
        num14 = Level.current.bottomRight.y - Level.current.topLeft.y;
      }
      float num15 = (double) num14 <= (double) num13 / (double) num12 ? num13 / num12 : num14;
      if (!flag)
        num15 = this._prevViewSize;
      else
        this._prevViewSize = num15;
      this._viewSize += (float) (((double) num15 - (double) this._viewSize) * ((double) this._lerpSpeed * (double) this._lerpMult * (double) this._speed));
      if (this.immediate)
        this._viewSize = num15;
      float num16 = this._viewSize;
      if ((double) this.manualViewSize > 0.0)
        num16 = this.manualViewSize;
      this.width = num16 * num12;
      this.height = num16;
      this._lerpBorder = Maths.Clamp((float) ((double) Math.Min(this.width, 740f) / 740.0 * (90.0 * (double) this._zoomMult)), this.minSize * this._zoomMult, 90f * this._zoomMult);
      if (!flag)
      {
        num10 = this._prevCenterMoveX;
        num11 = this._prevCenterMoveY;
      }
      else
      {
        this._prevCenterMoveX = num10;
        this._prevCenterMoveY = num11;
      }
      this._center.x += (float) (((double) num10 - (double) this._center.x) * ((double) this._lerpSpeed * (double) this._lerpMult));
      this._center.y += (float) (((double) num11 - (double) this._center.y) * ((double) this._lerpSpeed * (double) this._lerpMult));
      if (this.immediate)
      {
        this._center.x = num10;
        this._center.y = num11;
      }
      if ((double) this._lerpSpeed > 0.899999976158142 && this._startCentered)
      {
        this._center.x = (float) (((double) Level.current.bottomRight.x + (double) Level.current.topLeft.x) / 2.0);
        this._center.y = (float) (((double) Level.current.bottomRight.y + (double) Level.current.topLeft.y) / 2.0);
      }
      this.x = this._center.x - this.width / 2f;
      this.y = this._center.y - this.height / 2f;
      if ((double) this._lerpSpeed > 0.899999976158142)
        this._lerpSpeed = 0.05f;
      this.lerpMult = lerpMult;
    }
  }
}
