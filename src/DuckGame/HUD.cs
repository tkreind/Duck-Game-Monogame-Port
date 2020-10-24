// Decompiled with JetBrains decompiler
// Type: DuckGame.HUD
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class HUD
  {
    private static List<CornerDisplay> _cornerDisplays = new List<CornerDisplay>();
    private static List<CornerDisplay> _inputChangeDisplays = new List<CornerDisplay>();
    private static bool _hide = false;

    public static bool hide
    {
      get => HUD._hide;
      set => HUD._hide = value;
    }

    public static CornerDisplay AddCornerMessage(HUDCorner corner, string text)
    {
      CornerDisplay cornerDisplay = new CornerDisplay();
      cornerDisplay.corner = corner;
      cornerDisplay.text = text;
      HUD._cornerDisplays.Add(cornerDisplay);
      return cornerDisplay;
    }

    public static CornerDisplay AddCornerControl(
      HUDCorner corner,
      string text,
      InputProfile pro = null)
    {
      CornerDisplay cornerDisplay = new CornerDisplay();
      cornerDisplay.corner = corner;
      cornerDisplay.text = text;
      cornerDisplay.isControl = true;
      cornerDisplay.profile = pro;
      HUD._cornerDisplays.Add(cornerDisplay);
      return cornerDisplay;
    }

    public static void AddInputChangeDisplay(string text)
    {
      HUD._inputChangeDisplays.Clear();
      HUD._inputChangeDisplays.Add(new CornerDisplay()
      {
        text = text,
        isControl = true,
        life = 3f
      });
    }

    public static void AddCornerTimer(HUDCorner corner, string text, Timer timer) => HUD._cornerDisplays.Add(new CornerDisplay()
    {
      corner = corner,
      text = text,
      timer = timer
    });

    public static void AddCornerCounter(
      HUDCorner corner,
      string text,
      FieldBinding counter,
      int max = 0,
      bool animateCount = false)
    {
      HUD._cornerDisplays.Add(new CornerDisplay()
      {
        corner = corner,
        text = text,
        counter = counter,
        maxCount = max,
        animateCount = animateCount,
        curCount = (int) counter.value,
        realCount = (int) counter.value
      });
    }

    public static void CloseAllCorners()
    {
      foreach (CornerDisplay cornerDisplay in HUD._cornerDisplays)
        cornerDisplay.closing = true;
    }

    public static void CloseCorner(HUDCorner corner)
    {
      foreach (CornerDisplay cornerDisplay in HUD._cornerDisplays)
      {
        if (cornerDisplay.corner == corner)
          cornerDisplay.closing = true;
      }
    }

    public static void ClearCorners() => HUD._cornerDisplays.Clear();

    public static void Update()
    {
      for (int index = 0; index < HUD._inputChangeDisplays.Count; ++index)
      {
        CornerDisplay inputChangeDisplay = HUD._inputChangeDisplays[index];
        if (inputChangeDisplay.closing)
        {
          inputChangeDisplay.slide = Lerp.FloatSmooth(inputChangeDisplay.slide, -0.3f, 0.15f);
          if ((double) inputChangeDisplay.slide < -0.150000005960464)
          {
            HUD._inputChangeDisplays.RemoveAt(index);
            --index;
          }
        }
        else
        {
          inputChangeDisplay.life -= Maths.IncFrameTimer();
          inputChangeDisplay.slide = Lerp.FloatSmooth(inputChangeDisplay.slide, 1f, 0.15f, 1.2f);
          if ((double) inputChangeDisplay.life <= 0.0)
            inputChangeDisplay.closing = true;
        }
      }
      for (int index = 0; index < HUD._cornerDisplays.Count; ++index)
      {
        CornerDisplay d = HUD._cornerDisplays[index];
        if (d.closing)
        {
          d.slide = Lerp.FloatSmooth(d.slide, -0.3f, 0.15f);
          if ((double) d.slide < -0.150000005960464)
          {
            HUD._cornerDisplays.RemoveAt(index);
            --index;
          }
        }
        else if (!HUD._cornerDisplays.Exists((Predicate<CornerDisplay>) (v => v.corner == d.corner && v.closing)))
        {
          if (d.counter != null)
          {
            if (d.addCount != 0)
            {
              d.addCountWait -= 0.05f;
              if ((double) d.addCountWait <= 0.0)
              {
                d.addCountWait = 0.05f;
                if (d.addCount > 0)
                {
                  --d.addCount;
                  ++d.curCount;
                }
                else if (d.addCount < 0)
                {
                  ++d.addCount;
                  --d.curCount;
                }
                SFX.Play("tinyTick", 0.6f, 0.3f);
              }
            }
            int num = (int) d.counter.value;
            if (num != d.realCount)
            {
              if (d.animateCount)
              {
                d.addCountWait = 1f;
                d.addCount = num - d.realCount;
                d.curCount = d.realCount;
                d.realCount = num;
              }
              else
              {
                d.realCount = num;
                d.curCount = num;
              }
            }
          }
          if (d.timer != null && d.timer.maxTime.TotalSeconds != 0.0 && (int) (d.timer.maxTime - d.timer.elapsed).TotalSeconds == d.lowTimeTick)
          {
            --d.lowTimeTick;
            SFX.Play("cameraBeep", 0.8f);
          }
          d.slide = Lerp.FloatSmooth(d.slide, 1f, 0.15f, 1.2f);
        }
      }
    }

    public static void Draw()
    {
      if (HUD._hide)
        return;
      foreach (CornerDisplay inputChangeDisplay in HUD._inputChangeDisplays)
      {
        Vec2 vec2_1 = new Vec2(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height);
        string text = inputChangeDisplay.text ?? "";
        float stringWidth = Graphics.GetStringWidth(text);
        float x = stringWidth;
        float stringHeight = Graphics.GetStringHeight(text);
        float num1 = stringHeight + 4f;
        float num2 = 0.0f;
        Vec2 vec2_2 = vec2_1;
        Vec2 vec2_3 = vec2_1;
        vec2_2.x -= stringWidth / 2f;
        vec2_3.x -= stringWidth / 2f;
        float num3 = Layer.HUD.camera.width / 32f + num1;
        Vec2 vec2_4 = Vec2.Zero;
        vec2_4 = new Vec2(0.0f, -num3);
        Graphics.DrawRect(vec2_3 + vec2_4 * inputChangeDisplay.slide, vec2_3 + new Vec2(x, num1 - 1f) + vec2_4 * inputChangeDisplay.slide, Color.Black, (Depth) 0.95f);
        Graphics.DrawString(text, vec2_2 + new Vec2((float) (((double) x - (double) stringWidth) / 2.0), (float) (((double) num1 - (double) stringHeight) / 2.0) + num2) + vec2_4 * inputChangeDisplay.slide, Color.White, (Depth) 0.97f, inputChangeDisplay.profile);
      }
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      foreach (CornerDisplay cornerDisplay in HUD._cornerDisplays)
      {
        Vec2 vec2_1 = new Vec2(0.0f, 0.0f);
        switch (cornerDisplay.corner)
        {
          case HUDCorner.TopLeft:
            vec2_1 = new Vec2(0.0f, (float) (num4 * 10));
            ++num4;
            break;
          case HUDCorner.TopRight:
            vec2_1 = new Vec2(Layer.HUD.camera.width, (float) (num5 * 10));
            ++num5;
            break;
          case HUDCorner.BottomLeft:
            vec2_1 = new Vec2(0.0f, Layer.HUD.camera.height - (float) (num6 * 10));
            ++num6;
            break;
          case HUDCorner.BottomRight:
            vec2_1 = new Vec2(Layer.HUD.camera.width, Layer.HUD.camera.height - (float) (num7 * 10));
            ++num7;
            break;
        }
        string text = cornerDisplay.text ?? "";
        bool flag = false;
        if (cornerDisplay.timer != null)
        {
          if (cornerDisplay.timer.maxTime.TotalSeconds != 0.0)
          {
            TimeSpan span = cornerDisplay.timer.maxTime - cornerDisplay.timer.elapsed;
            text = text + cornerDisplay.text + MonoMain.TimeString(span, small: true);
            if (span.TotalSeconds < 10.0)
              flag = true;
          }
          else
            text = text + cornerDisplay.text + MonoMain.TimeString(cornerDisplay.timer.elapsed, small: true);
        }
        else if (cornerDisplay.counter != null && cornerDisplay.counter.value is int)
        {
          int curCount = cornerDisplay.curCount;
          if (cornerDisplay.addCount != 0)
          {
            text += Convert.ToString(curCount);
            if (cornerDisplay.addCount > 0)
              text = text + " |GREEN|+" + Convert.ToString(cornerDisplay.addCount);
            else if (cornerDisplay.addCount < 0)
              text = text + " |RED|" + Convert.ToString(cornerDisplay.addCount);
          }
          else
            text = cornerDisplay.maxCount == 0 ? text + Convert.ToString(curCount) : text + Convert.ToString(curCount) + "/" + Convert.ToString(cornerDisplay.maxCount);
        }
        float stringWidth1 = Graphics.GetStringWidth(text);
        float stringWidth2 = Graphics.GetStringWidth(text, cornerDisplay.isControl);
        float num1 = stringWidth1 + 8f;
        float x = stringWidth2 + 8f;
        float stringHeight = Graphics.GetStringHeight(text);
        float num2 = stringHeight + 4f;
        float num3 = 0.0f;
        Vec2 vec2_2 = vec2_1;
        Vec2 vec2_3 = vec2_1;
        if (cornerDisplay.corner == HUDCorner.TopRight || cornerDisplay.corner == HUDCorner.BottomRight)
        {
          vec2_2.x -= num1 * cornerDisplay.slide;
          vec2_3.x -= x * cornerDisplay.slide;
        }
        else
        {
          vec2_2.x -= num1 * (1f - cornerDisplay.slide);
          vec2_3.x -= x * (1f - cornerDisplay.slide);
        }
        if (cornerDisplay.corner == HUDCorner.BottomLeft || cornerDisplay.corner == HUDCorner.BottomRight)
        {
          vec2_2.y -= num2;
          vec2_3.y -= num2;
        }
        if (cornerDisplay.corner == HUDCorner.TopRight || cornerDisplay.corner == HUDCorner.TopLeft)
          num3 = 0.0f;
        if (cornerDisplay.corner == HUDCorner.BottomLeft || cornerDisplay.corner == HUDCorner.BottomRight)
          num3 = 0.0f;
        float num8 = Layer.HUD.camera.width / 32f;
        Vec2 vec2_4 = Vec2.Zero;
        if (cornerDisplay.corner == HUDCorner.TopLeft)
          vec2_4 = new Vec2(num8, num8);
        else if (cornerDisplay.corner == HUDCorner.TopRight)
          vec2_4 = new Vec2(-num8, num8);
        else if (cornerDisplay.corner == HUDCorner.BottomLeft)
          vec2_4 = new Vec2(num8, -num8);
        else if (cornerDisplay.corner == HUDCorner.BottomRight)
          vec2_4 = new Vec2(-num8, -num8);
        Graphics.DrawRect(vec2_3 + vec2_4 * cornerDisplay.slide, vec2_3 + new Vec2(x, num2 - 1f) + vec2_4 * cornerDisplay.slide, Color.Black, (Depth) 0.95f);
        Graphics.DrawRect(vec2_3 + new Vec2(x, 1f) + vec2_4 * cornerDisplay.slide, vec2_3 + new Vec2(x + 1f, num2 - 2f) + vec2_4 * cornerDisplay.slide, Color.Black, (Depth) 0.95f);
        Graphics.DrawRect(vec2_3 + new Vec2(0.0f, 1f) + vec2_4 * cornerDisplay.slide, vec2_3 + new Vec2(-1f, num2 - 2f) + vec2_4 * cornerDisplay.slide, Color.Black, (Depth) 0.95f);
        Graphics.DrawString(text, vec2_2 + new Vec2((float) (((double) num1 - (double) stringWidth1) / 2.0), (float) (((double) num2 - (double) stringHeight) / 2.0) + num3) + vec2_4 * cornerDisplay.slide, flag ? Color.Red : Color.White, (Depth) 0.98f, cornerDisplay.profile);
      }
    }
  }
}
