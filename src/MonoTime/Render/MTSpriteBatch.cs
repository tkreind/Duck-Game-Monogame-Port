﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.MTSpriteBatch
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace DuckGame
{
  public class MTSpriteBatch : SpriteBatch
  {
    private int _globalIndex = (int) Thing.GetGlobalIndex();
    private readonly MTSpriteBatcher _batcher;
    private SpriteSortMode _sortMode;
    private BlendState _blendState;
    private SamplerState _samplerState;
    private DepthStencilState _depthStencilState;
    private RasterizerState _rasterizerState;
    private Effect _effect;
    private bool _beginCalled;
    private Effect _spriteEffect;
    private readonly EffectParameter _matrixTransform;
    private readonly EffectPass _spritePass;
    private readonly EffectPass _spritePassSimple;
    private Matrix _matrix;
    private Rectangle _tempRect = new Rectangle(0.0f, 0.0f, 0.0f, 0.0f);
    private Vec2 _texCoordTL = new Vec2(0.0f, 0.0f);
    private Vec2 _texCoordBR = new Vec2(0.0f, 0.0f);
    private Matrix _projMatrix;
    public Matrix fullMatrix;

    public MTSpriteBatchItem StealLastSpriteBatchItem() => this._batcher.StealLastBatchItem();

    public MTSpriteBatch(GraphicsDevice graphicsDevice)
      : base(graphicsDevice)
    {
      if (graphicsDevice == null)
        throw new ArgumentException(nameof (graphicsDevice));
      this._spriteEffect = (Effect) Content.Load<MTEffect>("Shaders/SpriteEffect");
      this._matrixTransform = this._spriteEffect.Parameters["MatrixTransform"];
      this._spritePass = this._spriteEffect.Techniques[0].Passes[0];
      this._spritePassSimple = this._spriteEffect.Techniques[1].Passes[0];
      this._batcher = new MTSpriteBatcher(graphicsDevice, this);
      this._beginCalled = false;
    }

    public new void Begin() => this.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (MTEffect) null, Matrix.Identity);

    public void Begin(
      SpriteSortMode sortMode,
      BlendState blendState,
      SamplerState samplerState,
      DepthStencilState depthStencilState,
      RasterizerState rasterizerState,
      MTEffect effect,
      Matrix transformMatrix)
    {
      DuckGame.Graphics.currentStateIndex = this._globalIndex;
      if (this._beginCalled)
        throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
      base.Begin();
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.StateChange(sortMode, blendState, samplerState, depthStencilState, rasterizerState, Layer.IsBasicLayerEffect(effect) ? Layer.basicLayerEffect : effect, transformMatrix, (Rectangle) this.GraphicsDevice.ScissorRectangle);
      if (Recorder.globalRecording != null)
        Recorder.globalRecording.StateChange(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix, (Rectangle) this.GraphicsDevice.ScissorRectangle);
      this._sortMode = sortMode;
      this._blendState = blendState ?? BlendState.AlphaBlend;
      this._samplerState = samplerState ?? SamplerState.LinearClamp;
      this._depthStencilState = depthStencilState ?? DepthStencilState.None;
      this._rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
      this._effect = (Effect) effect;
      this._matrix = transformMatrix;
      if (sortMode == SpriteSortMode.Immediate)
        this.Setup();
      this._beginCalled = true;
    }

    public new void Begin(SpriteSortMode sortMode, BlendState blendState) => this.Begin(sortMode, blendState, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (MTEffect) null, Matrix.Identity);

    public new void Begin(
      SpriteSortMode sortMode,
      BlendState blendState,
      SamplerState samplerState,
      DepthStencilState depthStencilState,
      RasterizerState rasterizerState)
    {
      this.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, (MTEffect) null, Matrix.Identity);
    }

    public void Begin(
      SpriteSortMode sortMode,
      BlendState blendState,
      SamplerState samplerState,
      DepthStencilState depthStencilState,
      RasterizerState rasterizerState,
      MTEffect effect)
    {
      this.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
    }

    public new void End()
    {
      this._beginCalled = false;
      base.End();
      if (DuckGame.Graphics.recordOnly)
        return;
      if (this._batcher.hasSimpleItems)
      {
        if (this._sortMode != SpriteSortMode.Immediate)
          this.Setup(true);
        this._batcher.DrawSimpleBatch(this._sortMode);
      }
      if (this._batcher.hasGeometryItems)
      {
        if (this._sortMode != SpriteSortMode.Immediate)
          this.Setup(true);
        this._batcher.DrawGeometryBatch(this._sortMode);
      }
      if (this._sortMode != SpriteSortMode.Immediate)
        this.Setup();
      this._batcher.DrawBatch(this._sortMode);
      if (!this._batcher.hasTexturedGeometryItems)
        return;
      if (this._sortMode != SpriteSortMode.Immediate)
        this.Setup();
      this._batcher.DrawTexturedGeometryBatch(this._sortMode);
    }

    public void ReapplyEffect(bool simple = false)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      graphicsDevice.BlendState = this._blendState;
      graphicsDevice.DepthStencilState = this._depthStencilState;
      graphicsDevice.RasterizerState = this._rasterizerState;
      graphicsDevice.SamplerStates[0] = this._samplerState;
      if (simple)
      {
        this._spriteEffect.CurrentTechnique = this._spriteEffect.Techniques[1];
        this._spritePassSimple.Apply();
      }
      else
      {
        this._spritePass.Apply();
        if (this._effect == null)
          return;
        this._effect.CurrentTechnique.Passes[0].Apply();
      }
    }

    public Matrix viewMatrix => this._matrix;

    public Matrix projMatrix => this._projMatrix;

    private void Setup(bool simple = false)
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      graphicsDevice.BlendState = this._blendState;
      graphicsDevice.DepthStencilState = this._depthStencilState;
      graphicsDevice.RasterizerState = this._rasterizerState;
      graphicsDevice.SamplerStates[0] = this._samplerState;
      Viewport viewport = graphicsDevice.Viewport;
      Matrix.CreateOrthographicOffCenter(0.0f, (float) viewport.Width, (float) viewport.Height, 0.0f, 0.0f, -1f, out this._projMatrix);
      this._projMatrix.M41 += -0.5f * this._projMatrix.M11;
      this._projMatrix.M42 += -0.5f * this._projMatrix.M22;
      Matrix result;
      Matrix.Multiply(ref this._matrix, ref this._projMatrix, out result);
      this._matrixTransform.SetValue((Microsoft.Xna.Framework.Matrix) result);
      this.fullMatrix = result;
      if (simple)
      {
        this._spriteEffect.CurrentTechnique = this._spriteEffect.Techniques[1];
        this._spritePassSimple.Apply();
      }
      else
      {
        this._spriteEffect.CurrentTechnique = this._spriteEffect.Techniques[0];
        this._spriteEffect.CurrentTechnique.Passes[0].Apply();
        this._spritePass.Apply();
      }
      if (this._effect == null)
        return;
      this._effect.CurrentTechnique = !simple || this._effect.Techniques.Count <= 1 || !(this._effect.Techniques[1].Name == "BasicSimple") ? this._effect.Techniques[0] : this._effect.Techniques[1];
      this._effect.CurrentTechnique.Passes[0].Apply();
    }

    private void CheckValid(Tex2D texture)
    {
      if (texture == null)
        throw new ArgumentNullException(nameof (texture));
      if (!this._beginCalled)
        throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
    }

    private void CheckValid(SpriteFont spriteFont, string text)
    {
      if (spriteFont == null)
        throw new ArgumentNullException(nameof (spriteFont));
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      if (!this._beginCalled)
        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
    }

    private void CheckValid(SpriteFont spriteFont, StringBuilder text)
    {
      if (spriteFont == null)
        throw new ArgumentNullException(nameof (spriteFont));
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      if (!this._beginCalled)
        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
    }

    public GeometryItem GetGeometryItem() => this._batcher.GetGeometryItem();

    public static GeometryItem CreateGeometryItem() => MTSpriteBatcher.CreateGeometryItem();

    public void SubmitGeometry(GeometryItem geo) => this._batcher.SubmitGeometryItem(geo);

    public static GeometryItemTexture CreateTexturedGeometryItem() => MTSpriteBatcher.CreateTexturedGeometryItem();

    public void SubmitTexturedGeometry(GeometryItemTexture geo) => this._batcher.SubmitTexturedGeometryItem(geo);

    /// <summary>
    /// This is a MonoGame Extension method for calling Draw() using named parameters.  It is not available in the standard XNA Framework.
    /// </summary>
    /// <param name="texture">The Texture2D to draw.  Required.</param>
    /// <param name="position">
    /// The position to draw at.  If left empty, the method will draw at drawRectangle instead.
    /// </param>
    /// <param name="drawRectangle">
    /// The rectangle to draw at.  If left empty, the method will draw at position instead.
    /// </param>
    /// <param name="sourceRectangle">
    /// The source rectangle of the texture.  Default is null
    /// </param>
    /// <param name="origin">
    /// Origin of the texture.  Default is Vector2.Zero
    /// </param>
    /// <param name="rotation">Rotation of the texture.  Default is 0f</param>
    /// <param name="scale">
    /// The scale of the texture as a Vector2.  Default is Vector2.One
    /// </param>
    /// <param name="color">
    /// Color of the texture.  Default is Color.White
    /// </param>
    /// <param name="effect">
    /// SpriteEffect to draw with.  Default is SpriteEffects.None
    /// </param>
    /// <param name="depth">Draw depth.  Default is 0f.</param>
    public void Draw(
      Tex2D texture,
      Vec2? position = null,
      Rectangle? drawRectangle = null,
      Rectangle? sourceRectangle = null,
      Vec2? origin = null,
      float rotation = 0.0f,
      Vec2? scale = null,
      Color? color = null,
      SpriteEffects effect = SpriteEffects.None,
      float depth = 0.0f)
    {
      if (!color.HasValue)
        color = new Color?(Color.White);
      if (!origin.HasValue)
        origin = new Vec2?(Vec2.Zero);
      if (!scale.HasValue)
        scale = new Vec2?(Vec2.One);
      if (drawRectangle.HasValue == position.HasValue)
        throw new InvalidOperationException("Expected drawRectangle or position, but received neither or both.");
      if (position.HasValue)
        this.Draw(texture, position.Value, sourceRectangle, color.Value, rotation, origin.Value, scale.Value, effect, depth);
      else
        this.Draw(texture, drawRectangle.Value, sourceRectangle, color.Value, rotation, origin.Value, effect, depth);
    }

    public void Draw(
      Tex2D texture,
      Vec2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      Vec2 scale,
      SpriteEffects effect,
      float depth)
    {
      this.CheckValid(texture);
      float z = (float) texture.width * scale.x;
      float w = (float) texture.height * scale.y;
      if (sourceRectangle.HasValue)
      {
        z = sourceRectangle.Value.width * scale.x;
        w = sourceRectangle.Value.height * scale.y;
      }
      this.DoDrawInternal(texture, new Vec4(position.x, position.y, z, w), sourceRectangle, color, rotation, origin * scale, effect, depth, true, (Material) null);
    }

    public void DrawWithMaterial(
      Tex2D texture,
      Vec2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      Vec2 scale,
      SpriteEffects effect,
      float depth,
      Material fx)
    {
      this.CheckValid(texture);
      float z = (float) texture.width * scale.x;
      float w = (float) texture.height * scale.y;
      if (sourceRectangle.HasValue)
      {
        z = sourceRectangle.Value.width * scale.x;
        w = sourceRectangle.Value.height * scale.y;
      }
      this.DoDrawInternal(texture, new Vec4(position.x, position.y, z, w), sourceRectangle, color, rotation, origin * scale, effect, depth, true, fx);
    }

    public void Draw(
      Tex2D texture,
      Vec2 position,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      float scale,
      SpriteEffects effect,
      float depth)
    {
      this.CheckValid(texture);
      float z = (float) texture.width * scale;
      float w = (float) texture.height * scale;
      if (sourceRectangle.HasValue)
      {
        z = sourceRectangle.Value.width * scale;
        w = sourceRectangle.Value.height * scale;
      }
      this.DoDrawInternal(texture, new Vec4(position.x, position.y, z, w), sourceRectangle, color, rotation, origin * scale, effect, depth, true, (Material) null);
    }

    public void Draw(
      Tex2D texture,
      Rectangle destinationRectangle,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      SpriteEffects effect,
      float depth)
    {
      this.CheckValid(texture);
      this.DoDrawInternal(texture, new Vec4(destinationRectangle.x, destinationRectangle.y, destinationRectangle.width, destinationRectangle.height), sourceRectangle, color, rotation, new Vec2(origin.x * (destinationRectangle.width / (!sourceRectangle.HasValue || (double) sourceRectangle.Value.width == 0.0 ? (float) texture.width : sourceRectangle.Value.width)), (float) ((double) origin.y * (double) destinationRectangle.height / (!sourceRectangle.HasValue || (double) sourceRectangle.Value.height == 0.0 ? (double) texture.height : (double) sourceRectangle.Value.height))), effect, depth, true, (Material) null);
    }

    public void DrawQuad(
      Vec2 p1,
      Vec2 p2,
      Vec2 p3,
      Vec2 p4,
      Vec2 t1,
      Vec2 t2,
      Vec2 t3,
      Vec2 t4,
      float depth,
      Tex2D tex,
      Color c)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      MTSpriteBatchItem batchItem = this._batcher.CreateBatchItem();
      batchItem.Depth = depth;
      batchItem.Texture = tex.nativeObject as Texture2D;
      batchItem.Material = (Material) null;
      batchItem.Set(p1, p2, p3, p4, t1, t2, t3, t4, c);
    }

    internal void DoDrawInternal(
      Tex2D texture,
      Vec4 destinationRectangle,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      SpriteEffects effect,
      float depth,
      bool autoFlush,
      Material fx)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      MTSpriteBatchItem batchItem = this._batcher.CreateBatchItem();
      batchItem.Depth = depth;
      batchItem.Texture = texture.nativeObject as Texture2D;
      batchItem.Material = fx;
      if (sourceRectangle.HasValue)
      {
        this._tempRect = sourceRectangle.Value;
      }
      else
      {
        this._tempRect.x = 0.0f;
        this._tempRect.y = 0.0f;
        this._tempRect.width = (float) texture.width;
        this._tempRect.height = (float) texture.height;
      }
      this._texCoordTL.x = (float) ((double) this._tempRect.x / (double) texture.width + 9.99999974737875E-06);
      this._texCoordTL.y = (float) ((double) this._tempRect.y / (double) texture.height + 9.99999974737875E-06);
      this._texCoordBR.x = (float) (((double) this._tempRect.x + (double) this._tempRect.width) / (double) texture.width - 9.99999974737875E-06);
      this._texCoordBR.y = (float) (((double) this._tempRect.y + (double) this._tempRect.height) / (double) texture.height - 9.99999974737875E-06);
      if ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None)
      {
        float y = this._texCoordBR.y;
        this._texCoordBR.y = this._texCoordTL.y;
        this._texCoordTL.y = y;
      }
      if ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
      {
        float x = this._texCoordBR.x;
        this._texCoordBR.x = this._texCoordTL.x;
        this._texCoordTL.x = x;
      }
      batchItem.Set(destinationRectangle.x, destinationRectangle.y, -origin.x, -origin.y, destinationRectangle.z, destinationRectangle.w, (float) Math.Sin((double) rotation), (float) Math.Cos((double) rotation), color, this._texCoordTL, this._texCoordBR);
      if (!DuckGame.Graphics.skipReplayRender)
      {
        if (Recorder.currentRecording != null)
        {
          if (DuckGame.Graphics.recordMetadata)
          {
            batchItem.MetaData = new MTSpriteBatchItemMetaData();
            batchItem.MetaData.texture = texture;
            batchItem.MetaData.rotation = rotation;
            batchItem.MetaData.color = color;
            batchItem.MetaData.tempRect = this._tempRect;
            batchItem.MetaData.effect = effect;
            batchItem.MetaData.depth = depth;
          }
          Recorder.currentRecording.LogDraw(texture.textureIndex, new Vec2(batchItem.vertexTL.Position.X, batchItem.vertexTL.Position.Y), new Vec2(batchItem.vertexBR.Position.X, batchItem.vertexBR.Position.Y), rotation, color, (short) this._tempRect.x, (short) this._tempRect.y, (short) ((double) this._tempRect.width * ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) this._tempRect.height * ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), depth, texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
        }
        if (Recorder.globalRecording != null)
          Recorder.globalRecording.LogDraw(texture.textureIndex, new Vec2(batchItem.vertexTL.Position.X, batchItem.vertexTL.Position.Y), new Vec2(batchItem.vertexBR.Position.X, batchItem.vertexBR.Position.Y), rotation, color, (short) this._tempRect.x, (short) this._tempRect.y, (short) ((double) this._tempRect.width * ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) this._tempRect.height * ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), depth, texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
      }
      if (!autoFlush)
        return;
      this.FlushIfNeeded();
    }

    internal void DoDrawInternalTex2D(
      Tex2D texture,
      Vec4 destinationRectangle,
      Rectangle? sourceRectangle,
      Color color,
      float rotation,
      Vec2 origin,
      SpriteEffects effect,
      float depth,
      bool autoFlush,
      Material fx)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      MTSpriteBatchItem batchItem = this._batcher.CreateBatchItem();
      batchItem.Depth = depth;
      batchItem.Texture = texture.nativeObject as Texture2D;
      batchItem.Material = fx;
      if (sourceRectangle.HasValue)
      {
        this._tempRect = sourceRectangle.Value;
      }
      else
      {
        this._tempRect.x = 0.0f;
        this._tempRect.y = 0.0f;
        this._tempRect.width = (float) texture.width;
        this._tempRect.height = (float) texture.height;
      }
      this._texCoordTL.x = (float) ((double) this._tempRect.x / (double) texture.width + 9.99999974737875E-06);
      this._texCoordTL.y = (float) ((double) this._tempRect.y / (double) texture.height + 9.99999974737875E-06);
      this._texCoordBR.x = (float) (((double) this._tempRect.x + (double) this._tempRect.width) / (double) texture.width - 9.99999974737875E-06);
      this._texCoordBR.y = (float) (((double) this._tempRect.y + (double) this._tempRect.height) / (double) texture.height - 9.99999974737875E-06);
      if ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None)
      {
        float y = this._texCoordBR.y;
        this._texCoordBR.y = this._texCoordTL.y;
        this._texCoordTL.y = y;
      }
      if ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
      {
        float x = this._texCoordBR.x;
        this._texCoordBR.x = this._texCoordTL.x;
        this._texCoordTL.x = x;
      }
      batchItem.Set(destinationRectangle.x, destinationRectangle.y, -origin.x, -origin.y, destinationRectangle.z, destinationRectangle.w, (float) Math.Sin((double) rotation), (float) Math.Cos((double) rotation), color, this._texCoordTL, this._texCoordBR);
      if (Recorder.currentRecording != null)
      {
        if (DuckGame.Graphics.recordMetadata)
        {
          batchItem.MetaData = new MTSpriteBatchItemMetaData();
          batchItem.MetaData.texture = texture;
          batchItem.MetaData.rotation = rotation;
          batchItem.MetaData.color = color;
          batchItem.MetaData.tempRect = this._tempRect;
          batchItem.MetaData.effect = effect;
          batchItem.MetaData.depth = depth;
        }
        Recorder.currentRecording.LogDraw(texture.textureIndex, new Vec2(batchItem.vertexTL.Position.X, batchItem.vertexTL.Position.Y), new Vec2(batchItem.vertexBR.Position.X, batchItem.vertexBR.Position.Y), rotation, color, (short) this._tempRect.x, (short) this._tempRect.y, (short) ((double) this._tempRect.width * ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) this._tempRect.height * ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), depth, texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
      }
      if (Recorder.globalRecording != null)
        Recorder.globalRecording.LogDraw(texture.textureIndex, new Vec2(batchItem.vertexTL.Position.X, batchItem.vertexTL.Position.Y), new Vec2(batchItem.vertexBR.Position.X, batchItem.vertexBR.Position.Y), rotation, color, (short) this._tempRect.x, (short) this._tempRect.y, (short) ((double) this._tempRect.width * ((effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) this._tempRect.height * ((effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), depth, texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
      if (!autoFlush)
        return;
      this.FlushIfNeeded();
    }

    public void DrawExistingBatchItem(MTSpriteBatchItem item)
    {
      ++DuckGame.Graphics.currentDrawIndex;
      this._batcher.SqueezeInItem(item);
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogDraw(item.MetaData.texture.textureIndex, new Vec2(item.vertexTL.Position.X, item.vertexTL.Position.Y), new Vec2(item.vertexBR.Position.X, item.vertexBR.Position.Y), item.MetaData.rotation, item.MetaData.color, (short) item.MetaData.tempRect.x, (short) item.MetaData.tempRect.y, (short) ((double) item.MetaData.tempRect.width * ((item.MetaData.effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) item.MetaData.tempRect.height * ((item.MetaData.effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), item.MetaData.depth, item.MetaData.texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
      if (Recorder.globalRecording == null)
        return;
      Recorder.globalRecording.LogDraw(item.MetaData.texture.textureIndex, new Vec2(item.vertexTL.Position.X, item.vertexTL.Position.Y), new Vec2(item.vertexBR.Position.X, item.vertexBR.Position.Y), item.MetaData.rotation, item.MetaData.color, (short) item.MetaData.tempRect.x, (short) item.MetaData.tempRect.y, (short) ((double) item.MetaData.tempRect.width * ((item.MetaData.effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None ? -1.0 : 1.0)), (short) ((double) item.MetaData.tempRect.height * ((item.MetaData.effect & SpriteEffects.FlipVertically) != SpriteEffects.None ? -1.0 : 1.0)), item.MetaData.depth, item.MetaData.texture.currentObjectIndex, DuckGame.Graphics.currentObjectIndex, DuckGame.Graphics.currentDrawIndex);
    }

    public void DrawRecorderItem(ref RecorderFrameItem frame)
    {
      MTSpriteBatchItem batchItem = this._batcher.CreateBatchItem();
      batchItem.Depth = frame.depth;
      if (frame.texture == (short) -1)
      {
        batchItem.Texture = DuckGame.Graphics.blankWhiteSquare.nativeObject as Texture2D;
      }
      else
      {
        Tex2D tex2DfromIndex = Content.GetTex2DFromIndex(frame.texture);
        if (tex2DfromIndex == null)
          return;
        batchItem.Texture = tex2DfromIndex.nativeObject as Texture2D;
      }
      if (batchItem.Texture == null)
        return;
      float num1 = (float) Math.Abs(frame.texW);
      float num2 = (float) Math.Abs(frame.texH);
      this._texCoordTL.x = (float) ((double) frame.texX / (double) batchItem.Texture.Width + 9.99999974737875E-06);
      this._texCoordTL.y = (float) ((double) frame.texY / (double) batchItem.Texture.Height + 9.99999974737875E-06);
      this._texCoordBR.x = (float) (((double) frame.texX + (double) num1) / (double) batchItem.Texture.Width - 9.99999974737875E-06);
      this._texCoordBR.y = (float) (((double) frame.texY + (double) num2) / (double) batchItem.Texture.Height - 9.99999974737875E-06);
      if (frame.texH < (short) 0)
      {
        float y = this._texCoordBR.y;
        this._texCoordBR.y = this._texCoordTL.y;
        this._texCoordTL.y = y;
      }
      if (frame.texW < (short) 0)
      {
        float x = this._texCoordBR.x;
        this._texCoordBR.x = this._texCoordTL.x;
        this._texCoordTL.x = x;
      }
      Vec2 vec2 = frame.bottomRight.Rotate(-frame.rotation, frame.topLeft);
      batchItem.Set(frame.topLeft.x, frame.topLeft.y, 0.0f, 0.0f, vec2.x - frame.topLeft.x, vec2.y - frame.topLeft.y, (float) Math.Sin((double) frame.rotation), (float) Math.Cos((double) frame.rotation), frame.color, this._texCoordTL, this._texCoordBR);
    }

    public void Flush(bool doSetup)
    {
      if (doSetup)
        this.Setup();
      this._batcher.DrawBatch(this._sortMode);
    }

    internal void FlushIfNeeded()
    {
      if (this._sortMode != SpriteSortMode.Immediate)
        return;
      this._batcher.DrawBatch(this._sortMode);
    }

    public void Draw(Tex2D texture, Vec2 position, Rectangle? sourceRectangle, Color color) => this.Draw(texture, position, sourceRectangle, color, 0.0f, Vec2.Zero, 1f, SpriteEffects.None, 0.0f);

    public void Draw(
      Tex2D texture,
      Rectangle destinationRectangle,
      Rectangle? sourceRectangle,
      Color color)
    {
      this.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, Vec2.Zero, SpriteEffects.None, 0.0f);
    }

    public void Draw(Tex2D texture, Vec2 position, Color color) => this.Draw(texture, position, new Rectangle?(), color);

    public void Draw(Tex2D texture, Rectangle rectangle, Color color) => this.Draw(texture, rectangle, new Rectangle?(), color);

    protected override void Dispose(bool disposing)
    {
      if (!this.IsDisposed && disposing && this._spriteEffect != null)
      {
        this._spriteEffect.Dispose();
        this._spriteEffect = (Effect) null;
      }
      base.Dispose(disposing);
    }
  }
}
