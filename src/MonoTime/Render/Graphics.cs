// Decompiled with JetBrains decompiler
// Type: DuckGame.Graphics
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DuckGame
{
  public class Graphics
  {
    private static List<Action> _renderTasks = new List<Action>();
    private static int _currentStateIndex = 0;
    private static int _currentObjectIndex = 0;
    private static Vec2 _currentDrawOffset = Vec2.Zero;
    private static int _currentDrawIndex = 0;
    public static uint currentDepthSpan;
    public static int effectsLevel = 2;
    private static RenderTarget2D _screenTarget;
    private static bool _recordOnly = false;
    private static GraphicsDevice _base;
    public static GraphicsDeviceManager _manager;
    private static BitmapFont _biosFont;
    private static BitmapFont _biosFontCaseSensitive;
    private static FancyBitmapFont _fancyBiosFont;
    private static MTSpriteBatch _defaultBatch;
    private static MTSpriteBatch _currentBatch;
    private static Layer _currentLayer;
    private static bool _mouseVisible = true;
    private static int _width;
    private static int _height;
    private static bool _frameFlipFlop = false;
    private static RenderTarget2D _screenCapture;
    private static Tex2D _blank;
    private static Tex2D _blank2;
    private static float _depthBias = 0.0f;
    private static Matrix _projectionMatrix;
    public static float kSpanIncrement = 0.0001f;
    public static bool caseSensitiveStringDrawing = false;
    public static Vec2 topLeft;
    public static Vec2 bottomRight;
    public static bool didCalc = false;
    public static Material material;
    public static Effect tempEffect;
    public static float snap = 4f;
    public static bool skipReplayRender = false;
    public static bool recordMetadata = false;
    public static bool doSnap = true;
    public static long frame;
    private static float _baseDeviceWidth = 0.0f;
    private static float _baseDeviceHeight = 0.0f;
    private static RenderTarget2D _currentRenderTarget;
    public static RenderTarget2D defaultRenderTarget = (RenderTarget2D) null;
    private static Viewport _lastViewport;
    private static bool _lastViewportSet = false;

    public static void AddRenderTask(Action a) => DuckGame.Graphics._renderTasks.Add(a);

    public static void RunRenderTasks()
    {
      foreach (Action renderTask in DuckGame.Graphics._renderTasks)
        renderTask();
      DuckGame.Graphics._renderTasks.Clear();
    }

    public static void FlashScreen()
    {
      DuckGame.Graphics.flashAdd = 1.3f;
      Layer.Game.darken = 1.3f;
      Layer.Blocks.darken = 1.3f;
      Layer.Background.darken = -1.3f;
    }

    public static int currentStateIndex
    {
      get => DuckGame.Graphics._currentStateIndex;
      set => DuckGame.Graphics._currentStateIndex = value;
    }

    public static int currentObjectIndex
    {
      get => DuckGame.Graphics._currentObjectIndex;
      set
      {
        DuckGame.Graphics._currentObjectIndex = value;
        DuckGame.Graphics._currentDrawIndex = 0;
      }
    }

    public static Vec2 currentDrawOffset
    {
      get => DuckGame.Graphics._currentDrawOffset;
      set => DuckGame.Graphics._currentDrawOffset = value;
    }

    public static int currentDrawIndex
    {
      get => DuckGame.Graphics._currentDrawIndex;
      set => DuckGame.Graphics._currentDrawIndex = value;
    }

    public static int fps => FPSCounter.GetFPS(0);

    public static RenderTarget2D screenTarget
    {
      get => DuckGame.Graphics._screenTarget;
      set => DuckGame.Graphics._screenTarget = value;
    }

    public static bool inFocus => Program.main.IsActive;

    public static bool recordOnly
    {
      get => DuckGame.Graphics._recordOnly;
      set => DuckGame.Graphics._recordOnly = value;
    }

    public static GraphicsDevice device
    {
      get
      {
        if (Thread.CurrentThread != MonoMain.mainThread && Thread.CurrentThread != MonoMain.initializeThread)
          throw new Exception("accessing graphics device from thread other than main thread.");
        return DuckGame.Graphics._base;
      }
      set => DuckGame.Graphics._base = value;
    }

    public static MTSpriteBatch screen
    {
      get => DuckGame.Graphics._currentBatch;
      set
      {
        DuckGame.Graphics._currentBatch = value;
        if (DuckGame.Graphics._currentBatch != null)
          return;
        DuckGame.Graphics._currentBatch = DuckGame.Graphics._defaultBatch;
      }
    }

    public static Layer currentLayer
    {
      get => DuckGame.Graphics._currentLayer;
      set => DuckGame.Graphics._currentLayer = value;
    }

    public static bool mouseVisible
    {
      get => DuckGame.Graphics._mouseVisible;
      set => DuckGame.Graphics._mouseVisible = value;
    }

    public static int width
    {
      get => DuckGame.Graphics._width;
      set => DuckGame.Graphics._width = value;
    }

    public static int height
    {
      get => DuckGame.Graphics._height;
      set => DuckGame.Graphics._height = value;
    }

    public static void SetSize(int w, int h)
    {
      DuckGame.Graphics._width = w;
      DuckGame.Graphics._height = h;
    }

    public static bool frameFlipFlop
    {
      get => DuckGame.Graphics._frameFlipFlop;
      set => DuckGame.Graphics._frameFlipFlop = value;
    }

    public static RenderTarget2D screenCapture
    {
      get => DuckGame.Graphics._screenCapture;
      set => DuckGame.Graphics._screenCapture = value;
    }

    private static float _fade
    {
      get => MonoMain.core._fade;
      set => MonoMain.core._fade = value;
    }

    public static float fade
    {
      get => DuckGame.Graphics._fade;
      set => DuckGame.Graphics._fade = value;
    }

    private static float _fadeAdd
    {
      get => MonoMain.core._fadeAdd;
      set => MonoMain.core._fadeAdd = value;
    }

    public static float fadeAdd
    {
      get => DuckGame.Graphics._fadeAdd;
      set => DuckGame.Graphics._fadeAdd = value;
    }

    private static float _flashAdd
    {
      get => MonoMain.core._flashAdd;
      set => MonoMain.core._flashAdd = value;
    }

    public static float flashAdd
    {
      get => DuckGame.Graphics._flashAdd;
      set => DuckGame.Graphics._flashAdd = value;
    }

    public static bool IsBlankTexture(Tex2D tex) => tex == DuckGame.Graphics._blank || tex == DuckGame.Graphics._blank2;

    public static Tex2D blankWhiteSquare => DuckGame.Graphics._blank;

    public static Matrix projectionMatrix => DuckGame.Graphics._projectionMatrix;

    public static void IncrementSpanAdjust()
    {
    }

    public static void ResetSpanAdjust() => Depth.ResetSpan();

    public static float AdjustDepth(Depth depth)
    {
      DuckGame.Graphics._depthBias -= 1E-07f;
      float num1 = (float) (((double) depth.value + 1.0) / 2.0) * (1f - Depth.kDepthSpanMax) + depth.span;
      Vec2 vec2 = new Vec2(0.0f, 1f);
      if (DuckGame.Graphics._currentLayer != null)
        vec2 = DuckGame.Graphics._currentLayer.depthSpan;
      float num2 = vec2.y - vec2.x;
      return (float) (1.0 - ((double) vec2.x + (double) num1 * (double) num2));
    }

    public static void DrawString(
      string text,
      Vec2 position,
      Color color,
      Depth depth = default (Depth),
      InputProfile pro = null,
      float scale = 1f)
    {
      if (DuckGame.Graphics.caseSensitiveStringDrawing)
      {
        DuckGame.Graphics._biosFontCaseSensitive.scale = new Vec2(scale);
        DuckGame.Graphics._biosFontCaseSensitive.Draw(text, position.x, position.y, color, depth, pro);
        DuckGame.Graphics._biosFontCaseSensitive.scale = new Vec2(1f);
      }
      else
      {
        DuckGame.Graphics._biosFont.scale = new Vec2(scale);
        DuckGame.Graphics._biosFont.Draw(text, position.x, position.y, color, depth, pro);
        DuckGame.Graphics._biosFont.scale = new Vec2(1f);
      }
    }

    public static void DrawStringColoredSymbols(
      string text,
      Vec2 position,
      Color color,
      Depth depth = default (Depth),
      InputProfile pro = null,
      float scale = 1f)
    {
      if (DuckGame.Graphics.caseSensitiveStringDrawing)
      {
        DuckGame.Graphics._biosFontCaseSensitive.scale = new Vec2(scale);
        DuckGame.Graphics._biosFontCaseSensitive.Draw(text, position.x, position.y, color, depth, pro, true);
        DuckGame.Graphics._biosFontCaseSensitive.scale = new Vec2(1f);
      }
      else
      {
        DuckGame.Graphics._biosFont.scale = new Vec2(scale);
        DuckGame.Graphics._biosFont.Draw(text, position.x, position.y, color, depth, pro, true);
        DuckGame.Graphics._biosFont.scale = new Vec2(1f);
      }
    }

    public static void DrawStringOutline(
      string text,
      Vec2 position,
      Color color,
      Color outline,
      Depth depth = default (Depth),
      InputProfile pro = null,
      float scale = 1f)
    {
      DuckGame.Graphics._biosFont.scale = new Vec2(scale);
      DuckGame.Graphics._biosFont.DrawOutline(text, position, color, outline, depth);
      DuckGame.Graphics._biosFont.scale = new Vec2(1f);
    }

    public static float GetStringWidth(string text, bool thinButtons = false, float scale = 1f)
    {
      DuckGame.Graphics._biosFont.scale = new Vec2(scale);
      text = text.ToUpperInvariant();
      float width = DuckGame.Graphics._biosFont.GetWidth(text, thinButtons);
      DuckGame.Graphics._biosFont.scale = new Vec2(1f);
      return width;
    }

    public static float GetStringHeight(string text) => (float) ((IEnumerable<string>) text.Split('\n')).Count<string>() * DuckGame.Graphics._biosFont.height;

    public static void DrawFancyString(
      string text,
      Vec2 position,
      Color color,
      Depth depth = default (Depth),
      float scale = 1f)
    {
      DuckGame.Graphics._fancyBiosFont.scale = new Vec2(scale);
      DuckGame.Graphics._fancyBiosFont.Draw(text, position.x, position.y, color, depth);
      DuckGame.Graphics._fancyBiosFont.scale = new Vec2(1f);
    }

    public static float GetFancyStringWidth(string text, bool thinButtons = false, float scale = 1f)
    {
      DuckGame.Graphics._fancyBiosFont.scale = new Vec2(scale);
      text = text.ToUpperInvariant();
      float width = DuckGame.Graphics._fancyBiosFont.GetWidth(text, thinButtons);
      DuckGame.Graphics._fancyBiosFont.scale = new Vec2(1f);
      return width;
    }

    public static void DrawRecorderItem(ref RecorderFrameItem item) => DuckGame.Graphics._currentBatch.DrawRecorderItem(ref item);

    public static void DrawRecorderItemLerped(
      ref RecorderFrameItem item,
      ref RecorderFrameItem lerpTo,
      float dist)
    {
      RecorderFrameItem frame = item;
      frame.topLeft = Vec2.Lerp(item.topLeft, lerpTo.topLeft, dist);
      frame.bottomRight = Vec2.Lerp(item.bottomRight, lerpTo.bottomRight, dist);
      float num1 = item.rotation % 360f;
      float num2 = lerpTo.rotation % 360f;
      if ((double) num1 > 180.0)
        num1 -= 360f;
      else if ((double) num1 < -180.0)
        num1 += 360f;
      if ((double) num2 > 180.0)
        num2 -= 360f;
      else if ((double) num2 < -180.0)
        num2 += 360f;
      frame.rotation = MathHelper.Lerp(num1, num2, dist);
      frame.color = Color.Lerp(item.color, lerpTo.color, dist);
      DuckGame.Graphics._currentBatch.DrawRecorderItem(ref frame);
    }

    public static void Calc()
    {
      if (DuckGame.Graphics.didCalc)
        return;
      DuckGame.Graphics.didCalc = true;
      Viewport viewport = new Viewport(0, 0, 32, 32);
      Matrix result;
      Matrix.CreateOrthographicOffCenter(0.0f, (float) viewport.Width, (float) viewport.Height, 0.0f, 0.0f, -1f, out result);
      result.M41 += -0.5f * result.M11;
      result.M42 += -0.5f * result.M22;
      DuckGame.Graphics.bottomRight = new Vec2(32f, 32f);
      DuckGame.Graphics.bottomRight = Vec2.Transform(DuckGame.Graphics.bottomRight, result);
      DuckGame.Graphics.topLeft = new Vec2(0.0f, 0.0f);
      DuckGame.Graphics.topLeft = Vec2.Transform(DuckGame.Graphics.topLeft, result);
    }

    public static Queue<List<DrawCall>> drawCalls
    {
      get => Level.core.drawCalls;
      set => Level.core.drawCalls = value;
    }

    public static List<DrawCall> currentFrameCalls
    {
      get => Level.core.currentFrameCalls;
      set => Level.core.currentFrameCalls = value;
    }

    public static Thing currentDrawingObject
    {
      get => Level.core.currentDrawingObject;
      set => Level.core.currentDrawingObject = value;
    }

    public static bool skipFrameLog
    {
      get => Level.core.skipFrameLog;
      set => Level.core.skipFrameLog = value;
    }

    public static void Draw(MTSpriteBatchItem item) => DuckGame.Graphics._currentBatch.DrawExistingBatchItem(item);

    public static void Draw(
      Tex2D texture,
      Vec2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      Vec2 scale,
      SpriteEffects effects,
      Depth depth = default (Depth))
    {
      if (texture.nativeObject is Microsoft.Xna.Framework.Graphics.RenderTarget2D)
      {
        if ((texture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsDisposed)
          return;
        if (texture.textureIndex == (short) 0)
          Content.AssignTextureIndex(texture);
      }
      if (DuckGame.Graphics.doSnap)
      {
        position.x = (float) Math.Round((double) position.x * (double) DuckGame.Graphics.snap) / DuckGame.Graphics.snap;
        position.y = (float) Math.Round((double) position.y * (double) DuckGame.Graphics.snap) / DuckGame.Graphics.snap;
      }
      if (effects == SpriteEffects.FlipHorizontally)
        origin.x = (sourceRectangle.HasValue ? sourceRectangle.Value.width : (float) texture.w) - origin.x;
      float depth1 = DuckGame.Graphics.AdjustDepth(depth);
      if (DuckGame.Graphics.material != null)
        DuckGame.Graphics._currentBatch.DrawWithMaterial(texture, position, sourceRectangle, color, rotation, origin, scale, effects, depth1, DuckGame.Graphics.material);
      else
        DuckGame.Graphics._currentBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, depth1);
    }

    public static void Draw(Sprite g, float x, float y)
    {
      g.x = x;
      g.y = y;
      g.Draw();
    }

    public static void Draw(Sprite g, float x, float y, Rectangle sourceRectangle)
    {
      g.x = x;
      g.y = y;
      g.Draw(sourceRectangle);
    }

    public static void Draw(Sprite g, float x, float y, Rectangle sourceRectangle, Depth depth)
    {
      g.x = x;
      g.y = y;
      g.depth = depth;
      g.Draw(sourceRectangle);
    }

    public static void Draw(Sprite g, float x, float y, Depth depth = default (Depth))
    {
      g.x = x;
      g.y = y;
      g.depth = depth;
      g.Draw();
    }

    public static void Draw(Sprite g, float x, float y, float scaleX, float scaleY)
    {
      g.x = x;
      g.y = y;
      g.xscale = scaleX;
      g.yscale = scaleY;
      g.Draw();
    }

    public static void Draw(
      Tex2D target,
      float x,
      float y,
      float xscale = 1f,
      float yscale = 1f,
      Depth depth = default (Depth))
    {
      DuckGame.Graphics.Draw(target, new Vec2(x, y), new Rectangle?(), Color.White, 0.0f, Vec2.Zero, new Vec2(xscale, yscale), SpriteEffects.None, depth);
    }

    public static void Draw(
      SpriteMap g,
      int frame,
      float x,
      float y,
      float scaleX = 1f,
      float scaleY = 1f,
      bool maintainFrame = false)
    {
      g.x = x;
      g.y = y;
      g.xscale = scaleX;
      g.yscale = scaleY;
      int frame1 = g.frame;
      g.SetFrameWithoutReset(frame);
      g.Draw();
      if (!maintainFrame)
        return;
      g.SetFrameWithoutReset(frame1);
    }

    public static void DrawWithoutUpdate(
      SpriteMap g,
      float x,
      float y,
      float scaleX = 1f,
      float scaleY = 1f,
      bool maintainFrame = false)
    {
      g.x = x;
      g.y = y;
      g.xscale = scaleX;
      g.yscale = scaleY;
      g.DrawWithoutUpdate();
    }

    public static void DrawLine(Vec2 p1, Vec2 p2, Color col, float width = 1f, Depth depth = default (Depth))
    {
      ++DuckGame.Graphics.currentDrawIndex;
      p1 = new Vec2(p1.x, p1.y);
      p2 = new Vec2(p2.x, p2.y);
      float rotation = (float) Math.Atan2((double) p2.y - (double) p1.y, (double) p2.x - (double) p1.x);
      float length = (p1 - p2).length;
      DuckGame.Graphics.Draw(DuckGame.Graphics._blank, p1, new Rectangle?(), col, rotation, new Vec2(0.0f, 0.5f), new Vec2(length, width), SpriteEffects.None, depth);
    }

    public static void DrawDottedLine(
      Vec2 p1,
      Vec2 p2,
      Color col,
      float width = 1f,
      float dotLength = 8f,
      Depth depth = default (Depth))
    {
      ++DuckGame.Graphics.currentDrawIndex;
      Vec2 vec2_1 = p1;
      Vec2 vec2_2 = p2 - p1;
      float length = vec2_2.length;
      int num = (int) ((double) length / (double) dotLength);
      vec2_2.Normalize();
      bool flag = false;
      for (int index = 0; index < num; ++index)
      {
        Vec2 vec2_3 = vec2_1;
        vec2_3 += vec2_2 * dotLength;
        if ((double) (vec2_3 - p1).length > (double) length)
          vec2_3 = p2;
        if (!flag)
          DuckGame.Graphics.DrawLine(new Vec2(vec2_1.x, vec2_1.y), new Vec2(vec2_3.x, vec2_3.y), col, width, depth);
        flag = !flag;
        vec2_1 = vec2_3;
      }
    }

    public static void DrawCircle(
      Vec2 pos,
      float radius,
      Color col,
      float width = 1f,
      Depth depth = default (Depth),
      int iterations = 32)
    {
      Vec2 vec2_1 = Vec2.Zero;
      for (int index = 0; index < iterations; ++index)
      {
        float rad = Maths.DegToRad(360f / (float) (iterations - 1) * (float) index);
        Vec2 vec2_2 = new Vec2((float) Math.Cos((double) rad) * radius, (float) -Math.Sin((double) rad) * radius);
        if (index > 0)
          DuckGame.Graphics.DrawLine(pos + vec2_2, pos + vec2_1, col, width, depth);
        vec2_1 = vec2_2;
      }
    }

    public static void DrawTexturedLine(
      Tex2D texture,
      Vec2 p1,
      Vec2 p2,
      Color col,
      float width = 1f,
      Depth depth = default (Depth))
    {
      ++DuckGame.Graphics.currentDrawIndex;
      if (texture.width > 1)
      {
        p1 = new Vec2(p1.x, p1.y);
        p2 = new Vec2(p2.x, p2.y);
        float rotation = (float) Math.Atan2((double) p2.y - (double) p1.y, (double) p2.x - (double) p1.x);
        float x = (p1 - p2).length / (float) texture.width;
        DuckGame.Graphics.Draw(texture, p1, new Rectangle?(), col, rotation, new Vec2(0.0f, (float) (texture.height / 2)), new Vec2(x, width), SpriteEffects.None, depth);
      }
      else
      {
        p1 = new Vec2(p1.x, p1.y);
        p2 = new Vec2(p2.x, p2.y);
        float rotation = (float) Math.Atan2((double) p2.y - (double) p1.y, (double) p2.x - (double) p1.x);
        float length = (p1 - p2).length;
        DuckGame.Graphics.Draw(texture, p1, new Rectangle?(), col, rotation, new Vec2(0.0f, (float) (texture.height / 2)), new Vec2(length, width), SpriteEffects.None, depth);
      }
    }

    public static void DrawRect(
      Vec2 p1,
      Vec2 p2,
      Color col,
      Depth depth = default (Depth),
      bool filled = true,
      float borderWidth = 1f)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      if (filled)
      {
        DuckGame.Graphics.Draw(DuckGame.Graphics._blank2, p1, new Rectangle?(), col, 0.0f, Vec2.Zero, new Vec2((float) -((double) p1.x - (double) p2.x), (float) -((double) p1.y - (double) p2.y)), SpriteEffects.None, depth);
      }
      else
      {
        float num = borderWidth / 2f;
        DuckGame.Graphics.DrawLine(new Vec2(p1.x, p1.y + num), new Vec2(p2.x, p1.y + num), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(p1.x + num, p1.y + borderWidth), new Vec2(p1.x + num, p2.y - borderWidth), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(p2.x, p2.y - num), new Vec2(p1.x, p2.y - num), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(p2.x - num, p2.y - borderWidth), new Vec2(p2.x - num, p1.y + borderWidth), col, borderWidth, depth);
      }
    }

    public static void DrawRect(
      Rectangle r,
      Color col,
      Depth depth = default (Depth),
      bool filled = true,
      float borderWidth = 1f)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      Vec2 position = new Vec2(r.Left, r.Top);
      Vec2 vec2 = new Vec2(r.Right, r.Bottom);
      if (filled)
      {
        DuckGame.Graphics.Draw(DuckGame.Graphics._blank2, position, new Rectangle?(), col, 0.0f, Vec2.Zero, new Vec2((float) -((double) position.x - (double) vec2.x), (float) -((double) position.y - (double) vec2.y)), SpriteEffects.None, depth);
      }
      else
      {
        float num = borderWidth / 2f;
        DuckGame.Graphics.DrawLine(new Vec2(position.x, position.y + num), new Vec2(vec2.x, position.y + num), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(position.x + num, position.y + borderWidth), new Vec2(position.x + num, vec2.y - borderWidth), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(vec2.x, vec2.y - num), new Vec2(position.x, vec2.y - num), col, borderWidth, depth);
        DuckGame.Graphics.DrawLine(new Vec2(vec2.x - num, vec2.y - borderWidth), new Vec2(vec2.x - num, position.y + borderWidth), col, borderWidth, depth);
      }
    }

    public static void DrawDottedRect(
      Vec2 p1,
      Vec2 p2,
      Color col,
      Depth depth = default (Depth),
      float borderWidth = 1f,
      float dotLength = 8f)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      float num = borderWidth / 2f;
      DuckGame.Graphics.DrawDottedLine(new Vec2(p1.x, p1.y + num), new Vec2(p2.x, p1.y + num), col, borderWidth, dotLength, depth);
      DuckGame.Graphics.DrawDottedLine(new Vec2(p1.x + num, p1.y + borderWidth), new Vec2(p1.x + num, p2.y - borderWidth), col, borderWidth, dotLength, depth);
      DuckGame.Graphics.DrawDottedLine(new Vec2(p2.x, p2.y - num), new Vec2(p1.x, p2.y - num), col, borderWidth, dotLength, depth);
      DuckGame.Graphics.DrawDottedLine(new Vec2(p2.x - num, p2.y - borderWidth), new Vec2(p2.x - num, p1.y + borderWidth), col, borderWidth, dotLength, depth);
    }

    public static Tex2D Recolor(string sprite, Vec3 color) => DuckGame.Graphics.Recolor(Content.Load<Tex2D>(sprite), color);

    public static Tex2D Recolor(Tex2D sprite, Vec3 color)
    {
      Material material = (Material) new MaterialRecolor(new Vec3(color.x / (float) byte.MaxValue, color.y / (float) byte.MaxValue, color.z / (float) byte.MaxValue));
      RenderTarget2D t = new RenderTarget2D(sprite.w, sprite.h);
      DuckGame.Graphics.SetRenderTarget(t);
      DuckGame.Graphics.Clear(new Color(0, 0, 0, 0));
      material.Apply();
      DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, material.effect, Matrix.Identity);
      DuckGame.Graphics.Draw(sprite, new Vec2(), new Rectangle?(), Color.White, 0.0f, new Vec2(), new Vec2(1f, 1f), SpriteEffects.None, (Depth) 0.5f);
      DuckGame.Graphics.screen.End();
      DuckGame.Graphics.device.SetRenderTarget((Microsoft.Xna.Framework.Graphics.RenderTarget2D) null);
      return (Tex2D) t;
    }

    public static float baseDeviceWidth => DuckGame.Graphics._baseDeviceWidth;

    public static float baseDeviceHeight => DuckGame.Graphics._baseDeviceHeight;

    public static float aspect => (float) DuckGame.Graphics.height / (float) DuckGame.Graphics.width;

    public static bool sixteenTen => (double) DuckGame.Graphics.aspect > 0.569999992847443;

    public static float barSize => (float) (((double) DuckGame.Graphics.width * (double) DuckGame.Graphics.aspect - (double) DuckGame.Graphics.width * (9.0 / 16.0)) / 2.0);

    public static void InitializeBase(GraphicsDeviceManager m, int widthVal, int heightVal)
    {
      DuckGame.Graphics._manager = m;
      DuckGame.Graphics._width = widthVal;
      DuckGame.Graphics._baseDeviceWidth = (float) DuckGame.Graphics._width;
      DuckGame.Graphics._height = heightVal;
      DuckGame.Graphics._baseDeviceHeight = (float) DuckGame.Graphics._height;
      Resolution.SetVirtualResolution(widthVal, heightVal);
    }

    public static void Initialize(GraphicsDevice d)
    {
      DuckGame.Graphics._base = d;
      DuckGame.Graphics._defaultBatch = new MTSpriteBatch(DuckGame.Graphics._base);
      DuckGame.Graphics.screen = DuckGame.Graphics._defaultBatch;
      DuckGame.Graphics._blank = new Tex2D(1, 1);
      DuckGame.Graphics._blank.SetData(new Color[1]
      {
        Color.White
      });
      DuckGame.Graphics._blank2 = new Tex2D(1, 1);
      DuckGame.Graphics._blank2.SetData(new Color[1]
      {
        Color.White
      });
      DuckGame.Graphics._biosFont = new BitmapFont("biosFont", 8);
      DuckGame.Graphics._biosFontCaseSensitive = new BitmapFont("biosFontCaseSensitive", 8);
      DuckGame.Graphics._fancyBiosFont = new FancyBitmapFont("smallFont");
      Matrix.CreateOrthographicOffCenter(0.0f, (float) d.Viewport.Width, (float) d.Viewport.Height, 0.0f, 0.0f, 1f, out DuckGame.Graphics._projectionMatrix);
      DuckGame.Graphics._projectionMatrix.M41 += -0.5f * DuckGame.Graphics._projectionMatrix.M11;
      DuckGame.Graphics._projectionMatrix.M42 += -0.5f * DuckGame.Graphics._projectionMatrix.M22;
    }

    public static void ResetDepthBias() => DuckGame.Graphics._depthBias = 0.0f;

    public static RenderTarget2D currentRenderTarget => DuckGame.Graphics._currentRenderTarget;

    public static void SetRenderTarget(RenderTarget2D t)
    {
      if (t != null && t.IsDisposed)
        return;
      if (t == null)
        DuckGame.Graphics.device.SetRenderTarget(DuckGame.Graphics.defaultRenderTarget != null ? DuckGame.Graphics.defaultRenderTarget.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D : (Microsoft.Xna.Framework.Graphics.RenderTarget2D) null);
      else
        DuckGame.Graphics.device.SetRenderTarget(t.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D);
      DuckGame.Graphics._lastViewport = DuckGame.Graphics.device.Viewport;
      DuckGame.Graphics._currentRenderTarget = t;
    }

    public static RenderTarget2D GetRenderTarget() => DuckGame.Graphics._currentRenderTarget;

    public static Viewport viewport
    {
      get => DuckGame.Graphics.device.Viewport;
      set
      {
        if (!DuckGame.Graphics._lastViewportSet)
        {
          DuckGame.Graphics._lastViewport = value;
          DuckGame.Graphics._lastViewportSet = true;
        }
        if (DuckGame.Graphics.device.Viewport.Width != DuckGame.Graphics._lastViewport.Width || DuckGame.Graphics.device.Viewport.Height != DuckGame.Graphics._lastViewport.Height)
          return;
        Rectangle bounds = (Rectangle) value.Bounds;
        if (DuckGame.Graphics._currentRenderTarget != null)
          DuckGame.Graphics.ClipRectangle(bounds, new Rectangle(0.0f, 0.0f, (float) DuckGame.Graphics._currentRenderTarget.width, (float) DuckGame.Graphics._currentRenderTarget.height));
        else
          DuckGame.Graphics.ClipRectangle(bounds, (Rectangle) DuckGame.Graphics.device.PresentationParameters.Bounds);
        value.X = (int) bounds.x;
        value.Y = (int) bounds.y;
        value.Width = (int) bounds.width;
        value.Height = (int) bounds.height;
        try
        {
          DuckGame.Graphics.device.Viewport = value;
        }
        catch (Exception ex)
        {
          throw new ArgumentException("Error: Invalid Viewport (x = " + value.X.ToString() + ", y = " + value.Y.ToString() + ", w = " + value.Width.ToString() + ", h = " + value.Height.ToString() + ")");
        }
        DuckGame.Graphics._lastViewport = value;
      }
    }

    public static void SetScissorRectangle(Rectangle r)
    {
      float num = (float) DuckGame.Graphics.device.Viewport.Bounds.Width / (float) DuckGame.Graphics.width;
      r.width *= num;
      r.height *= num;
      r.x *= num;
      r.y *= num;
      DuckGame.Graphics.device.ScissorRectangle = (Microsoft.Xna.Framework.Rectangle) DuckGame.Graphics.ClipRectangle(r, (Rectangle) DuckGame.Graphics.device.Viewport.Bounds);
    }

    public static Rectangle ClipRectangle(Rectangle r, Rectangle clipTo)
    {
      if ((double) r.x > (double) clipTo.Right)
        r.x = clipTo.Right - r.width;
      if ((double) r.y > (double) clipTo.Bottom)
        r.y = clipTo.Bottom - r.height;
      if ((double) r.x < (double) clipTo.Left)
        r.x = clipTo.Left;
      if ((double) r.y < (double) clipTo.Top)
        r.y = clipTo.Top;
      if ((double) r.x < 0.0)
        r.x = 0.0f;
      if ((double) r.y < 0.0)
        r.y = 0.0f;
      if ((double) r.x + (double) r.width > (double) clipTo.x + (double) clipTo.width)
        r.width = clipTo.Right - r.x;
      if ((double) r.y + (double) r.height > (double) clipTo.y + (double) clipTo.height)
        r.height = clipTo.Bottom - r.y;
      if ((double) r.width < 0.0)
        r.width = 0.0f;
      if ((double) r.height < 0.0)
        r.height = 0.0f;
      return r;
    }

    public static void Clear(Color c) => DuckGame.Graphics.device.Clear((Microsoft.Xna.Framework.Color) c);
  }
}
