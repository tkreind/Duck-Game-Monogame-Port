﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SpriteMap
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class SpriteMap : Sprite, ICloneable<SpriteMap>, ICloneable
  {
    private int _globalIndex = (int) Thing.GetGlobalIndex();
    private int _width;
    private int _height;
    public float _speed = 1f;
    private bool _finished;
    private List<Animation> _animations = new List<Animation>();
    private Animation? _currentAnimation = new Animation?();
    private bool _hasAnimation;
    public int _frame;
    private int _lastFrame = -1;
    public int _imageIndex;
    private int _lastImageIndex = -1;
    private Rectangle _spriteBox = new Rectangle();
    public float _frameInc;
    private static Dictionary<string, List<bool>> _transparency = new Dictionary<string, List<bool>>();
    private int _cutWidth;
    private bool _flipFlop = true;
    private MTSpriteBatchItem _batchItem;

    public new int globalIndex
    {
      get => this._globalIndex;
      set => this._globalIndex = value;
    }

    public override int width => this._width;

    public override int height => this._height;

    public float speed
    {
      get => this._speed;
      set => this._speed = value;
    }

    public bool finished
    {
      get => this._finished;
      set => this._finished = value;
    }

    public int frame
    {
      get => this._frame;
      set
      {
        this.SetFrameWithoutReset(value);
        this._frameInc = 0.0f;
        this._finished = false;
      }
    }

    public int imageIndex
    {
      get => this._imageIndex;
      set => this._imageIndex = value;
    }

    public int animationIndex
    {
      get => this._currentAnimation.HasValue && this._currentAnimation.HasValue && this._animations.Contains(this._currentAnimation.Value) ? this._animations.IndexOf(this._currentAnimation.Value) : 0;
      set
      {
        if (this._animations == null)
          return;
        this.SetAnimation(this._animations[value].name);
      }
    }

    private bool valid => this._texture != null && this._texture.w > 0 && this.w > 0;

    public void SetFrameWithoutReset(int frame)
    {
      this._frame = frame;
      if (this._currentAnimation.HasValue && this.valid)
      {
        if (this._frame >= this._currentAnimation.Value.frames.Length)
          this._frame = this._currentAnimation.Value.frames.Length - 1;
        if (this._frame < 0)
          this._frame = 0;
        this._imageIndex = this._currentAnimation.Value.frames[this._frame];
      }
      else
        this._imageIndex = this._frame;
    }

    public string currentAnimation
    {
      get => !this._currentAnimation.HasValue ? "" : this._currentAnimation.Value.name;
      set => this.SetAnimation(value);
    }

    public SpriteMap(Tex2D tex, int frameWidth, int frameHeight)
    {
      this._texture = tex;
      tex.frameWidth = (float) frameWidth;
      tex.frameHeight = (float) frameHeight;
      this.position = new Vec2(this.x, this.y);
      this._width = frameWidth;
      this._height = frameHeight;
      this.AddDefaultAnimation();
    }

    public SpriteMap(string tex, int frameWidth, int frameHeight, bool calculateTransparency = false)
    {
      this._texture = Content.Load<Tex2D>(tex);
      this._texture.frameWidth = (float) frameWidth;
      this._texture.frameHeight = (float) frameHeight;
      this.position = new Vec2(this.x, this.y);
      this._width = frameWidth;
      this._height = frameHeight;
      this.AddDefaultAnimation();
      int num = calculateTransparency ? 1 : 0;
    }

    public bool CurrentFrameIsOpaque() => false;

    private void AddDefaultAnimation()
    {
      List<int> intList = new List<int>();
      int num = 1;
      if (this._width > 0)
        num = this._texture.width / this._width * (this._texture.height / this._height);
      for (int index = 0; index < num; ++index)
        intList.Add(index);
      this._animations.Add(new Animation("default", 1f, true, intList.ToArray()));
      this.SetAnimation("default");
      this._speed = 0.0f;
    }

    public void AddAnimation(string name, float speed, bool looping, params int[] frames)
    {
      if (!this._hasAnimation)
      {
        this.ClearAnimations();
        this._speed = 1f;
      }
      this._hasAnimation = true;
      this._animations.Add(new Animation(name, speed, looping, frames));
    }

    public void SetAnimation(string name)
    {
      if (this._currentAnimation.HasValue && this._currentAnimation.Value.name == name)
        return;
      this._finished = false;
      foreach (Animation animation in this._animations)
      {
        if (animation.name == name)
        {
          this._currentAnimation = new Animation?(animation);
          this._frameInc = 0.0f;
          this.frame = 0;
          return;
        }
      }
      this._currentAnimation = new Animation?();
    }

    public void ClearAnimations()
    {
      this._animations.Clear();
      this._currentAnimation = new Animation?();
    }

    public void CloneAnimations(SpriteMap into) => into._animations = new List<Animation>((IEnumerable<Animation>) this._animations);

    public void UpdateSpriteBox()
    {
      if (!this.valid)
        return;
      int num1 = this._texture.width / this.w;
      int num2 = this._imageIndex / num1;
      this._spriteBox = new Rectangle((float) ((this._imageIndex - num2 * num1) * this.w), (float) (num2 * this.h), (float) (this.w - this.cutWidth), (float) this.h);
      this._lastImageIndex = this._imageIndex;
    }

    public bool UpdateFrame(bool ignoreFlipFlop = false)
    {
      if (!this.valid)
        return false;
      if (this._currentAnimation.HasValue && (ignoreFlipFlop || this._flipFlop != DuckGame.Graphics.frameFlipFlop) && !VirtualTransition.doingVirtualTransition)
      {
        this._frameInc += this._currentAnimation.Value.speed * this._speed;
        if ((double) this._frameInc >= 1.0)
        {
          this._frameInc = 0.0f;
          ++this._frame;
        }
        if (this._lastFrame != this._frame)
        {
          if (this._frame >= this._currentAnimation.Value.frames.Length)
          {
            if (this._currentAnimation.Value.looping)
            {
              this.frame = 0;
            }
            else
            {
              this.frame = this._currentAnimation.Value.frames.Length - 1;
              this.finished = true;
            }
          }
          this._imageIndex = this._currentAnimation.Value.frames[this._frame];
          this._lastFrame = this._frame;
        }
        this._flipFlop = !this._flipFlop;
      }
      if (this._lastImageIndex != this._imageIndex)
        this.UpdateSpriteBox();
      return true;
    }

    public void UpdateFrameSpecial()
    {
      if (!this.valid)
        return;
      if (this._currentAnimation.HasValue && !VirtualTransition.doingVirtualTransition)
      {
        this._frameInc += this._currentAnimation.Value.speed * this._speed;
        if ((double) this._frameInc >= 1.0)
        {
          this._frameInc = 0.0f;
          ++this._frame;
        }
        if (this._frame >= this._currentAnimation.Value.frames.Length)
        {
          if (this._currentAnimation.Value.looping)
          {
            this.frame = 0;
          }
          else
          {
            this.frame = this._currentAnimation.Value.frames.Length - 1;
            this.finished = true;
          }
        }
        this._imageIndex = this._currentAnimation.Value.frames[this._frame];
      }
      this.UpdateSpriteBox();
    }

    public int cutWidth
    {
      get => this._cutWidth;
      set
      {
        this._cutWidth = value;
        this.UpdateSpriteBox();
      }
    }

    public override void Draw()
    {
      if (!this.UpdateFrame())
        return;
      this._texture.currentObjectIndex = this._globalIndex;
      if (this.w <= 0)
        return;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(this._spriteBox), this._color * this.alpha, this.angle, this.center, this.scale, this.flipH ? SpriteEffects.FlipHorizontally : (this.flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), this.depth);
    }

    public override void Draw(Rectangle r)
    {
      if (!this.UpdateFrame())
        return;
      r.x += this._spriteBox.x;
      r.y += this._spriteBox.y;
      this._texture.currentObjectIndex = this._globalIndex;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(r), this._color * this.alpha, this.angle, this.center, this.scale, this._flipH ? SpriteEffects.FlipHorizontally : (this._flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), this.depth);
    }

    public void DrawWithoutUpdate()
    {
      if (!this.valid)
        return;
      this._texture.currentObjectIndex = this._globalIndex;
      if (this.w <= 0)
        return;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(this._spriteBox), this._color * this.alpha, this.angle, this.center, this.scale, this.flipH ? SpriteEffects.FlipHorizontally : (this.flipV ? SpriteEffects.FlipVertically : SpriteEffects.None), this.depth);
    }

    public override void CheapDraw(bool flipH = false)
    {
      if (!this.valid)
        return;
      this._texture.currentObjectIndex = this._globalIndex;
      DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(this._spriteBox), this._color, this.angle, this.center, this.scale, flipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.depth);
    }

    public void ClearCache() => this._batchItem = (MTSpriteBatchItem) null;

    public override void UltraCheapStaticDraw(bool flipH = false)
    {
      if (this._batchItem == null)
      {
        if (!this.valid)
          return;
        this.UpdateFrame();
        DuckGame.Graphics.recordMetadata = true;
        this._texture.currentObjectIndex = this._globalIndex;
        DuckGame.Graphics.Draw(this._texture, this.position, new Rectangle?(this._spriteBox), this._color, this.angle, this.center, this.scale, flipH ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.depth);
        this._batchItem = DuckGame.Graphics.screen.StealLastSpriteBatchItem();
        if (this._batchItem.MetaData == null)
          this._batchItem = (MTSpriteBatchItem) null;
        DuckGame.Graphics.recordMetadata = false;
      }
      else
      {
        this._texture.currentObjectIndex = this._globalIndex;
        DuckGame.Graphics.Draw(this._batchItem);
      }
    }

    public override Sprite Clone()
    {
      SpriteMap into = new SpriteMap(this._texture, this._width, this._height);
      this.CloneAnimations(into);
      into.center = this.center;
      into.imageIndex = this.imageIndex;
      into.frame = this.frame;
      return (Sprite) into;
    }

    public SpriteMap CloneMap() => (SpriteMap) this.Clone();

    SpriteMap ICloneable<SpriteMap>.Clone() => (SpriteMap) this.Clone();

    object ICloneable.Clone() => (object) this.Clone();
  }
}
