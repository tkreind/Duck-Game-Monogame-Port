// Decompiled with JetBrains decompiler
// Type: DuckGame.ContextSlider
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Globalization;

namespace DuckGame
{
  public class ContextSlider : ContextMenu
  {
    private SpriteMap _radioButton;
    private FieldBinding _field;
    private SpriteMap _adjusterHand;
    private float _step;
    private string _minSpecial;
    private bool _adjust;
    private bool _time;
    private System.Type _myType;

    public bool adjust
    {
      get => this._adjust;
      set => this._adjust = value;
    }

    public ContextSlider(
      string text,
      IContextListener owner,
      FieldBinding field,
      float step,
      string minSpecial,
      bool time,
      System.Type myType,
      string valTooltip)
      : base(owner)
    {
      this.itemSize.x = 150f;
      this.itemSize.y = 16f;
      this._text = text;
      this._field = field;
      this._radioButton = new SpriteMap("Editor/radioButton", 16, 16);
      this._dragMode = true;
      this._step = step;
      this._minSpecial = minSpecial;
      this._adjusterHand = new SpriteMap("adjusterHand", 18, 17);
      this._time = time;
      this._myType = myType;
      this.tooltip = valTooltip;
    }

    public ContextSlider(
      string text,
      IContextListener owner,
      FieldBinding field = null,
      float step = 0.25f,
      string minSpecial = null,
      bool time = false,
      System.Type myType = null)
      : base(owner)
    {
      this.itemSize.x = 150f;
      this.itemSize.y = 16f;
      this._text = text;
      this._field = field;
      this._radioButton = new SpriteMap("Editor/radioButton", 16, 16);
      this._dragMode = true;
      this._step = step;
      this._minSpecial = minSpecial;
      this._adjusterHand = new SpriteMap("adjusterHand", 18, 17);
      this._time = time;
      this._myType = myType;
    }

    public override void Selected()
    {
      if (Editor.gamepadMode)
        return;
      float num = Maths.Clamp(Mouse.x - this.position.x, 0.0f, this.itemSize.x);
      if (this._field.value is List<TypeProbPair>)
      {
        if ((double) this._step > 0.0)
          num = (float) (Math.Round((double) num / (double) this.itemSize.x * 1.0 / (double) this._step) * (double) this._step / 1.0) * this.itemSize.x;
        TypeProbPair typeProbPair1 = (TypeProbPair) null;
        List<TypeProbPair> typeProbPairList = this._field.value as List<TypeProbPair>;
        foreach (TypeProbPair typeProbPair2 in typeProbPairList)
        {
          if (typeProbPair2.type == this._myType)
          {
            typeProbPair1 = typeProbPair2;
            break;
          }
        }
        if (typeProbPair1 == null)
        {
          typeProbPair1 = new TypeProbPair()
          {
            probability = 0.0f,
            type = this._myType
          };
          typeProbPairList.Add(typeProbPair1);
        }
        typeProbPair1.probability = (float) (0.0 + (double) num / (double) this.itemSize.x * 1.0);
        if ((double) typeProbPair1.probability != 0.0)
          return;
        typeProbPairList.Remove(typeProbPair1);
      }
      else
      {
        if ((double) this._step > 0.0)
          num = (float) Math.Round((double) num / (double) this.itemSize.x * (double) this._field.max / (double) this._step) * this._step / this._field.max * this.itemSize.x;
        if (this._field.value is float)
        {
          this._field.value = (object) (float) ((double) this._field.min + (double) num / (double) this.itemSize.x * (double) this._field.max);
        }
        else
        {
          if (!(this._field.value is int))
            return;
          this._field.value = (object) (int) Math.Round((double) this._field.min + (double) num / (double) this.itemSize.x * ((double) Math.Abs(this._field.min) + (double) this._field.max));
        }
      }
    }

    public void Increment()
    {
      if (this._field.value is List<TypeProbPair>)
      {
        TypeProbPair typeProbPair1 = (TypeProbPair) null;
        List<TypeProbPair> typeProbPairList = this._field.value as List<TypeProbPair>;
        foreach (TypeProbPair typeProbPair2 in typeProbPairList)
        {
          if (typeProbPair2.type == this._myType)
          {
            typeProbPair1 = typeProbPair2;
            break;
          }
        }
        if (typeProbPair1 == null)
        {
          typeProbPair1 = new TypeProbPair()
          {
            probability = 0.0f,
            type = this._myType
          };
          typeProbPairList.Add(typeProbPair1);
        }
        typeProbPair1.probability += this._step;
        typeProbPair1.probability = Maths.Clamp(typeProbPair1.probability, 0.0f, 1f);
      }
      else if (this._field.value is float)
      {
        this._field.value = (object) Maths.Clamp((float) this._field.value + this._step, this._field.min, this._field.max);
      }
      else
      {
        if (!(this._field.value is int))
          return;
        this._field.value = (object) (int) Maths.Clamp((float) ((int) this._field.value + (int) this._step), this._field.min, this._field.max);
      }
    }

    public void Decrement()
    {
      if (this._field.value is List<TypeProbPair>)
      {
        TypeProbPair typeProbPair1 = (TypeProbPair) null;
        List<TypeProbPair> typeProbPairList = this._field.value as List<TypeProbPair>;
        foreach (TypeProbPair typeProbPair2 in typeProbPairList)
        {
          if (typeProbPair2.type == this._myType)
          {
            typeProbPair1 = typeProbPair2;
            break;
          }
        }
        if (typeProbPair1 == null)
        {
          typeProbPair1 = new TypeProbPair()
          {
            probability = 0.0f,
            type = this._myType
          };
          typeProbPairList.Add(typeProbPair1);
        }
        if ((double) typeProbPair1.probability == 0.0)
        {
          typeProbPairList.Remove(typeProbPair1);
        }
        else
        {
          typeProbPair1.probability -= this._step;
          typeProbPair1.probability = Maths.Clamp(typeProbPair1.probability, 0.0f, 1f);
        }
      }
      else if (this._field.value is float)
      {
        this._field.value = (object) Maths.Clamp((float) this._field.value - this._step, this._field.min, this._field.max);
      }
      else
      {
        if (!(this._field.value is int))
          return;
        this._field.value = (object) (int) Maths.Clamp((float) ((int) this._field.value - (int) this._step), this._field.min, this._field.max);
      }
    }

    public override void Update()
    {
      if (Editor.gamepadMode)
      {
        if (this._hover)
        {
          if (Input.Pressed("SELECT"))
            this._adjust = true;
          if (Input.Released("SELECT"))
            this._adjust = false;
        }
        if (this._adjust)
        {
          Editor.tookInput = true;
          int num = 1;
          float step = this._step;
          if (Input.Down("SHOOT"))
            num = 5;
          if (Input.Down("STRAFE"))
            this._step *= 0.1f;
          if (Input.Pressed("LEFT"))
          {
            for (int index = 0; index < num; ++index)
              this.Decrement();
          }
          if (Input.Pressed("RIGHT"))
          {
            for (int index = 0; index < num; ++index)
              this.Increment();
          }
          this._step = step;
        }
      }
      else if (this._hover)
      {
        this._adjust = true;
        if ((double) Mouse.scroll > 0.0)
          this.Decrement();
        if ((double) Mouse.scroll < 0.0)
          this.Increment();
      }
      else
        this._adjust = false;
      base.Update();
    }

    public override void Draw()
    {
      float num1 = 0.0f;
      string text1 = "";
      if (this._field.value is List<TypeProbPair>)
      {
        TypeProbPair typeProbPair1 = (TypeProbPair) null;
        foreach (TypeProbPair typeProbPair2 in this._field.value as List<TypeProbPair>)
        {
          if (typeProbPair2.type == this._myType)
          {
            typeProbPair1 = typeProbPair2;
            break;
          }
        }
        num1 = typeProbPair1 == null ? 0.0f : typeProbPair1.probability;
        text1 = num1.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture);
      }
      else if (this._field.value is float)
      {
        num1 = (float) this._field.value;
        text1 = num1.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture);
      }
      else if (this._field.value is int)
      {
        num1 = (float) (int) this._field.value;
        text1 = Change.ToString((object) (int) this._field.value);
      }
      if (this._minSpecial != null && (double) num1 == (double) this._field.min)
        text1 = this._minSpecial;
      else if (this._time)
        text1 = MonoMain.TimeString(TimeSpan.FromSeconds((double) (int) num1), 2);
      if (this._adjust)
      {
        float num2 = this._field.max - this._field.min;
        float x1 = (float) (4.0 + ((double) num2 - ((double) this._field.max - (double) num1)) / (double) num2 * ((double) this.itemSize.x - 8.0));
        if (Editor.gamepadMode)
        {
          float x2 = 0.0f;
          float x3 = this.itemSize.x;
          string text2 = this._text + ": " + text1;
          Color color = Color.White;
          if (this._field.value is List<TypeProbPair>)
            color = (double) num1 != 0.0 ? ((double) num1 >= 0.300000011920929 ? ((double) num1 >= 0.699999988079071 ? Color.Green : Color.Orange) : Colors.DGRed) : Color.DarkGray;
          float num3 = 0.05f;
          Graphics.DrawString(text2, this.position + new Vec2(this.itemSize.x + 8f, 5f), color, (Depth) (0.82f + num3));
          float x4 = x3 + (Graphics.GetStringWidth(text2) + 10f);
          Graphics.DrawRect(this.position + new Vec2(x1 - 2f, 3f), this.position + new Vec2(x1 + 2f, this.itemSize.y - 3f), new Color(250, 250, 250), (Depth) (0.85f + num3));
          Graphics.DrawRect(this.position + new Vec2(x2, 0.0f), this.position + new Vec2(x4, this.itemSize.y), new Color(70, 70, 70), (Depth) (0.75f + num3));
          Graphics.DrawRect(this.position + new Vec2(4f, (float) ((double) this.itemSize.y / 2.0 - 2.0)), this.position + new Vec2(this.itemSize.x - 4f, (float) ((double) this.itemSize.y / 2.0 + 2.0)), new Color(150, 150, 150), (Depth) (0.82f + num3));
          Vec2 vec2 = this.position + new Vec2(x1, 0.0f);
          this._adjusterHand.depth = new Depth(0.9f);
          Graphics.Draw((Sprite) this._adjusterHand, vec2.x - 6f, vec2.y - 6f);
        }
        else
        {
          float x2 = (float) (-(double) Graphics.GetStringWidth(this._text) - 10.0);
          float x3 = this.itemSize.x;
          Color color = Color.White;
          if (this._field.value is List<TypeProbPair>)
            color = (double) num1 != 0.0 ? ((double) num1 >= 0.300000011920929 ? ((double) num1 >= 0.699999988079071 ? Color.Green : Color.Orange) : Colors.DGRed) : Color.DarkGray;
          float num3 = 0.1f;
          Graphics.DrawString(this._text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(this._text) - 8.0), 5f), color, (Depth) (0.82f + num3));
          Graphics.DrawString(text1, this.position + new Vec2(this.itemSize.x + 8f, 5f), Color.White, (Depth) (0.82f + num3));
          float x4 = x3 + (Graphics.GetStringWidth(text1) + 10f);
          Graphics.DrawRect(this.position + new Vec2(x1 - 2f, 3f), this.position + new Vec2(x1 + 2f, this.itemSize.y - 3f), new Color(250, 250, 250), (Depth) (0.85f + num3));
          Graphics.DrawRect(this.position + new Vec2(x2, 0.0f), this.position + new Vec2(x4, this.itemSize.y), new Color(70, 70, 70), (Depth) (0.75f + num3));
          Graphics.DrawRect(this.position + new Vec2(4f, (float) ((double) this.itemSize.y / 2.0 - 2.0)), this.position + new Vec2(this.itemSize.x - 4f, (float) ((double) this.itemSize.y / 2.0 + 2.0)), new Color(150, 150, 150), (Depth) (0.82f + num3));
        }
      }
      else
      {
        if (this._hover)
          Graphics.DrawRect(this.position, this.position + this.itemSize, new Color(70, 70, 70), this.depth);
        Color color = Color.White;
        if (this._field.value is List<TypeProbPair>)
          color = (double) num1 != 0.0 ? ((double) num1 >= 0.300000011920929 ? ((double) num1 >= 0.699999988079071 ? Color.Green : Color.Orange) : Colors.DGRed) : Color.DarkGray;
        Graphics.DrawString(this._text, this.position + new Vec2(2f, 5f), color, new Depth(0.82f));
        Graphics.DrawString(text1, this.position + new Vec2(this.itemSize.x - 4f - Graphics.GetStringWidth(text1), 5f), Color.White, new Depth(0.82f));
      }
    }
  }
}
