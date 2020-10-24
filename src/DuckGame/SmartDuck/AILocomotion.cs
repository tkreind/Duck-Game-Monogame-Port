﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.AILocomotion
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class AILocomotion
  {
    private AIPathFinder _path = new AIPathFinder();
    private DuckAI _ai;
    private float _jumpFrames;
    private float _slideFrames;
    private int _tryJump = -1;

    public AIPathFinder pathFinder => this._path;

    public Vec2 target
    {
      set => this._path.SetTarget(value);
      get => this._path.target == null ? Vec2.Zero : this._path.target.link.position;
    }

    public void RunLeft()
    {
      this._ai.Release("RIGHT");
      this._ai.Press("LEFT");
    }

    public void RunRight()
    {
      this._ai.Press("RIGHT");
      this._ai.Release("LEFT");
    }

    public void Jump(int frames)
    {
      this._jumpFrames = (float) frames;
      this._ai.Press("JUMP");
    }

    public void Slide(int frames)
    {
      this._slideFrames = (float) frames;
      this._ai.Press("DOWN");
    }

    public void TrimLastTarget()
    {
      if (this._path.path == null || this._path.path.Count <= 0)
        return;
      this._path.path.RemoveAt(this._path.path.Count - 1);
    }

    public void Update(DuckAI ai, Duck duck)
    {
      if (Mouse.right == InputState.Pressed)
      {
        this._path.followObject = (Thing) duck;
        this._path.SetTarget(Mouse.positionScreen);
        this._path.Refresh();
      }
      this._ai = ai;
      this._path.followObject = (Thing) duck;
      if ((double) this._jumpFrames == 1.0)
      {
        this._jumpFrames = 0.0f;
        this._ai.Release("JUMP");
      }
      else if ((double) this._jumpFrames > 0.0)
        --this._jumpFrames;
      if ((double) this._slideFrames == 1.0)
      {
        this._slideFrames = 0.0f;
        this._ai.Release("DOWN");
      }
      else if ((double) this._slideFrames > 0.0)
        --this._slideFrames;
      ai.Release("LEFT");
      ai.Release("RIGHT");
      if (this._path.path == null || this._path.path.Count == 0)
        return;
      Vec2 target = this.target;
      Vec2 vec2 = new Vec2(target.x - duck.x, target.y - duck.y);
      if (!PathNode.LineIsClear(duck.position, target))
      {
        this._path.Refresh();
        if (this._path.path == null)
          return;
        target = this.target;
        vec2 = new Vec2(target.x - duck.x, target.y - duck.y);
      }
      if (this._path.path == null)
        return;
      if ((double) vec2.y < (double) duck.y && (double) Math.Abs(vec2.y) > 64.0 && this._path.path.Count > 1)
      {
        this._path.Refresh();
        if (this._path.path == null)
          return;
        target = this.target;
        vec2 = new Vec2(target.x - duck.x, target.y - duck.y);
      }
      if (!PathNode.LineIsClear(duck.position, target))
      {
        this._path.Refresh();
        if (this._path.path == null)
          return;
        target = this.target;
        vec2 = new Vec2(target.x - duck.x, target.y - duck.y);
      }
      if (this._path.path == null)
        return;
      bool flag = false;
      if (this._tryJump > 0)
        --this._tryJump;
      if (this._tryJump == 0 && duck.grounded)
      {
        this._path.Refresh();
        if (this._path.path == null)
          return;
        target = this.target;
        vec2 = new Vec2(target.x - duck.x, target.y - duck.y);
        this._tryJump = -1;
      }
      if (this._path.path == null)
        return;
      float num1 = 1f;
      if ((double) this._path.target.position.y == (double) this.target.y)
        num1 = 0.0f;
      if ((double) vec2.x < ((double) duck.hSpeed * 3.0 - 2.0) * (double) num1)
      {
        if (duck.grounded && Level.CheckLine<Window>(duck.position, duck.position + new Vec2(-32f, 0.0f)) != null)
          this.Slide(30);
        this.RunLeft();
      }
      else if ((double) vec2.x > ((double) duck.hSpeed * 3.0 + 2.0) * (double) num1)
      {
        if (duck.grounded && Level.CheckLine<Window>(duck.position, duck.position + new Vec2(32f, 0.0f)) != null)
          this.Slide(30);
        this.RunRight();
      }
      if (this._path.peek.gap && duck.grounded)
      {
        this.Jump((int) ((double) Maths.Clamp(Math.Abs(vec2.x), 0.0f, 48f) / 48.0 * 16.0));
        this._tryJump = 5;
      }
      if ((double) vec2.y <= -4.0 && duck.grounded)
      {
        this.Jump((int) ((double) Maths.Clamp(Math.Abs(vec2.y), 0.0f, 48f) / 48.0 * 16.0));
        this._tryJump = 5;
      }
      if (flag)
        ai.Release("JUMP");
      float num2 = 8f;
      if ((double) Math.Abs(this._path.peek.owner.y - target.y) < 8.0)
        num2 = 200f;
      if ((double) Math.Abs(vec2.x) >= 4.0 || (double) Math.Abs(vec2.y) >= (double) num2 || !PathNode.LineIsClear(duck.position - new Vec2(0.0f, 8f), target) || ((double) this._path.peek.link.position.y < (double) duck.y - 8.0 && !duck.grounded || !duck.grounded))
        return;
      this._path.AtTarget();
      this._ai.canRefresh = true;
      this._tryJump = -1;
    }
  }
}
