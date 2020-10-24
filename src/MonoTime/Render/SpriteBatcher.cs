// Decompiled with JetBrains decompiler
// Type: DuckGame.MTSpriteBatcher
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DuckGame
{
  /// <summary>
  /// This class handles the queueing of batch items into the GPU by creating the triangle tesselations
  /// that are used to draw the sprite textures. This class supports int.MaxValue number of sprites to be
  /// batched and will process them into short.MaxValue groups (strided by 6 for the number of vertices
  /// sent to the GPU).
  /// </summary>
  internal class MTSpriteBatcher
  {
    /// <summary>
    /// Initialization size for the batch item list and queue.
    /// </summary>
    private const int InitialBatchSize = 256;
    /// <summary>
    /// The maximum number of batch items that can be processed per iteration
    /// </summary>
    private const int MaxBatchSize = 5461;
    /// <summary>
    /// Initialization size for the vertex array, in batch units.
    /// </summary>
    private const int InitialVertexArraySize = 256;
    /// <summary>The list of batch items to process.</summary>
    private readonly List<MTSpriteBatchItem> _batchItemList;
    private readonly List<MTSimpleSpriteBatchItem> _simpleBatchItemList;
    private readonly List<GeometryItem> _geometryBatch;
    private readonly List<GeometryItemTexture> _geometryBatchTextured;
    /// <summary>
    /// The available MTSpriteBatchItem queue so that we reuse these objects when we can.
    /// </summary>
    private readonly Queue<MTSpriteBatchItem> _freeBatchItemQueue;
    private readonly Queue<MTSimpleSpriteBatchItem> _freeSimpleBatchItemQueue;
    private readonly Queue<GeometryItem> _freeGeometryBatch;
    private readonly Queue<GeometryItemTexture> _freeGeometryBatchTextured;
    /// <summary>The target graphics device.</summary>
    private readonly GraphicsDevice _device;
    /// <summary>
    /// Vertex index array. The values in this array never change.
    /// </summary>
    private short[] _index;
    private short[] _simpleIndex;
    private short[] _geometryIndex;
    private short[] _texturedGeometryIndex;
    private VertexPositionColorTexture[] _vertexArray;
    private VertexPositionColor[] _simpleVertexArray;
    private VertexPositionColor[] _geometryVertexArray;
    private VertexPositionColorTexture[] _geometryVertexArrayTextured;
    private MTSpriteBatch _batch;

    public MTSpriteBatcher(GraphicsDevice device, MTSpriteBatch batch)
    {
      this._device = device;
      this._batch = batch;
      this._batchItemList = new List<MTSpriteBatchItem>(256);
      this._freeBatchItemQueue = new Queue<MTSpriteBatchItem>(256);
      this._simpleBatchItemList = new List<MTSimpleSpriteBatchItem>(256);
      this._freeSimpleBatchItemQueue = new Queue<MTSimpleSpriteBatchItem>(256);
      this._geometryBatch = new List<GeometryItem>(1);
      this._freeGeometryBatch = new Queue<GeometryItem>(1);
      this._geometryBatchTextured = new List<GeometryItemTexture>(1);
      this._freeGeometryBatchTextured = new Queue<GeometryItemTexture>(1);
      this.EnsureArrayCapacity(256);
      this.EnsureSimpleArrayCapacity(256);
      this.EnsureGeometryArrayCapacity(256);
      this.EnsureTexturedGeometryArrayCapacity(256);
    }

    public bool hasSimpleItems => this._simpleBatchItemList.Count != 0;

    public bool hasGeometryItems => this._geometryBatch.Count != 0;

    public bool hasTexturedGeometryItems => this._geometryBatchTextured.Count != 0;

    /// <summary>
    /// Create an instance of MTSpriteBatchItem if there is none available in the free item queue. Otherwise,
    /// a previously allocated MTSpriteBatchItem is reused.
    /// </summary>
    /// <returns></returns>
    public MTSpriteBatchItem CreateBatchItem()
    {
      MTSpriteBatchItem mtSpriteBatchItem = this._freeBatchItemQueue.Count <= 0 ? new MTSpriteBatchItem() : this._freeBatchItemQueue.Dequeue();
      this._batchItemList.Add(mtSpriteBatchItem);
      return mtSpriteBatchItem;
    }

    public MTSpriteBatchItem StealLastBatchItem()
    {
      MTSpriteBatchItem batchItem = this._batchItemList[this._batchItemList.Count - 1];
      batchItem.inPool = false;
      return batchItem;
    }

    public void SqueezeInItem(MTSpriteBatchItem item) => this._batchItemList.Add(item);

    public MTSimpleSpriteBatchItem CreateSimpleBatchItem()
    {
      MTSimpleSpriteBatchItem simpleSpriteBatchItem = this._freeSimpleBatchItemQueue.Count <= 0 ? new MTSimpleSpriteBatchItem() : this._freeSimpleBatchItemQueue.Dequeue();
      this._simpleBatchItemList.Add(simpleSpriteBatchItem);
      return simpleSpriteBatchItem;
    }

    public static GeometryItem CreateGeometryItem() => new GeometryItem()
    {
      temporary = false
    };

    public GeometryItem GetGeometryItem()
    {
      GeometryItem geometryItem;
      if (this._freeGeometryBatch.Count > 0)
      {
        geometryItem = this._freeGeometryBatch.Dequeue();
        geometryItem.material = (Material) null;
      }
      else
        geometryItem = new GeometryItem()
        {
          temporary = true
        };
      geometryItem.Clear();
      return geometryItem;
    }

    public void SubmitGeometryItem(GeometryItem item)
    {
      if (this._geometryBatch.Contains(item))
        return;
      this._geometryBatch.Add(item);
    }

    public static GeometryItemTexture CreateTexturedGeometryItem() => new GeometryItemTexture()
    {
      temporary = false
    };

    public GeometryItemTexture GetTexturedGeometryItem()
    {
      GeometryItemTexture geometryItemTexture;
      if (this._freeGeometryBatch.Count > 0)
        geometryItemTexture = this._freeGeometryBatchTextured.Dequeue();
      else
        geometryItemTexture = new GeometryItemTexture()
        {
          temporary = true
        };
      geometryItemTexture.Clear();
      return geometryItemTexture;
    }

    public void SubmitTexturedGeometryItem(GeometryItemTexture item)
    {
      if (this._geometryBatchTextured.Contains(item))
        return;
      this._geometryBatchTextured.Add(item);
    }

    /// <summary>
    /// Resize and recreate the missing indices for the index and vertex position color buffers.
    /// </summary>
    /// <param name="numBatchItems"></param>
    private void EnsureArrayCapacity(int numBatchItems)
    {
      int num1 = 6 * numBatchItems;
      if (this._index != null && num1 <= this._index.Length)
        return;
      short[] numArray = new short[6 * numBatchItems];
      int num2 = 0;
      if (this._index != null)
      {
        this._index.CopyTo((Array) numArray, 0);
        num2 = this._index.Length / 6;
      }
      for (int index = num2; index < numBatchItems; ++index)
      {
        numArray[index * 6] = (short) (index * 4);
        numArray[index * 6 + 1] = (short) (index * 4 + 1);
        numArray[index * 6 + 2] = (short) (index * 4 + 2);
        numArray[index * 6 + 3] = (short) (index * 4 + 1);
        numArray[index * 6 + 4] = (short) (index * 4 + 3);
        numArray[index * 6 + 5] = (short) (index * 4 + 2);
      }
      this._index = numArray;
      this._vertexArray = new VertexPositionColorTexture[4 * numBatchItems];
    }

    private void EnsureSimpleArrayCapacity(int numBatchItems)
    {
      int num1 = 6 * numBatchItems;
      if (this._simpleIndex != null && num1 <= this._simpleIndex.Length)
        return;
      short[] numArray = new short[6 * numBatchItems];
      int num2 = 0;
      if (this._simpleIndex != null)
      {
        this._simpleIndex.CopyTo((Array) numArray, 0);
        num2 = this._simpleIndex.Length / 6;
      }
      for (int index = num2; index < numBatchItems; ++index)
      {
        numArray[index * 6] = (short) (index * 4);
        numArray[index * 6 + 1] = (short) (index * 4 + 1);
        numArray[index * 6 + 2] = (short) (index * 4 + 2);
        numArray[index * 6 + 3] = (short) (index * 4 + 1);
        numArray[index * 6 + 4] = (short) (index * 4 + 3);
        numArray[index * 6 + 5] = (short) (index * 4 + 2);
      }
      this._simpleIndex = numArray;
      this._simpleVertexArray = new VertexPositionColor[4 * numBatchItems];
    }

    private void EnsureGeometryArrayCapacity(int numTris)
    {
      int num1 = 3 * numTris;
      if (this._geometryIndex != null && num1 <= this._geometryIndex.Length)
        return;
      short[] numArray = new short[3 * numTris];
      int num2 = 0;
      if (this._geometryIndex != null)
      {
        this._geometryIndex.CopyTo((Array) numArray, 0);
        num2 = this._geometryIndex.Length / 3;
      }
      for (int index = num2; index < numTris; ++index)
      {
        numArray[index * 3] = (short) (index * 3);
        numArray[index * 3 + 1] = (short) (index * 3 + 1);
        numArray[index * 3 + 2] = (short) (index * 3 + 2);
      }
      this._geometryIndex = numArray;
      this._geometryVertexArray = new VertexPositionColor[4 * numTris];
    }

    private void EnsureTexturedGeometryArrayCapacity(int numTris)
    {
      int num1 = 3 * numTris;
      if (this._texturedGeometryIndex != null && num1 <= this._texturedGeometryIndex.Length)
        return;
      short[] numArray = new short[3 * numTris];
      int num2 = 0;
      if (this._texturedGeometryIndex != null)
      {
        this._texturedGeometryIndex.CopyTo((Array) numArray, 0);
        num2 = this._texturedGeometryIndex.Length / 3;
      }
      for (int index = num2; index < numTris; ++index)
      {
        numArray[index * 3] = (short) (index * 3);
        numArray[index * 3 + 1] = (short) (index * 3 + 1);
        numArray[index * 3 + 2] = (short) (index * 3 + 2);
      }
      this._texturedGeometryIndex = numArray;
      this._geometryVertexArrayTextured = new VertexPositionColorTexture[4 * numTris];
    }

    /// <summary>
    /// Reference comparison of the underlying Texture objects for each given MTSpriteBatchitem.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>0 if they are not reference equal, and 1 if so.</returns>
    private static int CompareTexture(MTSpriteBatchItem a, MTSpriteBatchItem b) => !object.ReferenceEquals((object) a.Texture, (object) b.Texture) ? 1 : 0;

    /// <summary>
    /// Compares the Depth of a against b returning -1 if a is less than b,
    /// 0 if equal, and 1 if a is greater than b. The test uses float.CompareTo(float)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>-1 if a is less than b, 0 if equal, and 1 if a is greater than b</returns>
    private static int CompareDepth(MTSpriteBatchItem a, MTSpriteBatchItem b) => a.Depth.CompareTo(b.Depth);

    private static int CompareSimpleDepth(MTSimpleSpriteBatchItem a, MTSimpleSpriteBatchItem b) => 0;

    private static int CompareGeometryDepth(GeometryItem a, GeometryItem b) => a.depth.CompareTo(b.depth);

    private static int CompareTexturedGeometryDepth(GeometryItemTexture a, GeometryItemTexture b) => a.depth.CompareTo(b.depth);

    /// <summary>
    /// Implements the opposite of CompareDepth, where b is compared against a.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>-1 if b is less than a, 0 if equal, and 1 if b is greater than a</returns>
    private static int CompareReverseDepth(MTSpriteBatchItem a, MTSpriteBatchItem b) => b.Depth.CompareTo(a.Depth);

    private static int CompareSimpleReverseDepth(
      MTSimpleSpriteBatchItem a,
      MTSimpleSpriteBatchItem b)
    {
      return 0;
    }

    private static int CompareGeometryReverseDepth(GeometryItem a, GeometryItem b) => b.depth.CompareTo(a.depth);

    private static int CompareTexturedGeometryReverseDepth(
      GeometryItemTexture a,
      GeometryItemTexture b)
    {
      return b.depth.CompareTo(a.depth);
    }

    /// <summary>
    /// Sorts the batch items and then groups batch drawing into maximal allowed batch sets that do not
    /// overflow the 16 bit array indices for vertices.
    /// </summary>
    /// <param name="sortMode">The type of depth sorting desired for the rendering.</param>
    public void DrawBatch(SpriteSortMode sortMode)
    {
      if (this._batchItemList.Count == 0)
        return;
      switch (sortMode)
      {
        case SpriteSortMode.Texture:
          this._batchItemList.Sort(new Comparison<MTSpriteBatchItem>(MTSpriteBatcher.CompareTexture));
          break;
        case SpriteSortMode.BackToFront:
          this._batchItemList.Sort(new Comparison<MTSpriteBatchItem>(MTSpriteBatcher.CompareReverseDepth));
          break;
        case SpriteSortMode.FrontToBack:
          this._batchItemList.Sort(new Comparison<MTSpriteBatchItem>(MTSpriteBatcher.CompareDepth));
          break;
      }
      int index1 = 0;
      int numBatchItems;
      for (int count = this._batchItemList.Count; count > 0; count -= numBatchItems)
      {
        int start = 0;
        int end = 0;
        Texture2D texture2D = (Texture2D) null;
        Material material = (Material) null;
        numBatchItems = count;
        if (numBatchItems > 5461)
          numBatchItems = 5461;
        this.EnsureArrayCapacity(numBatchItems);
        int num1 = 0;
        while (num1 < numBatchItems)
        {
          MTSpriteBatchItem batchItem = this._batchItemList[index1];
          if (!object.ReferenceEquals((object) batchItem.Texture, (object) texture2D) || !object.ReferenceEquals((object) batchItem.Material, (object) material))
          {
            this.FlushVertexArray(start, end);
            if (material != null && batchItem.Material == null)
              this._batch.ReapplyEffect();
            material = batchItem.Material;
            texture2D = batchItem.Texture;
            start = end = 0;
            this._device.Textures[0] = (Texture) texture2D;
            material?.Apply();
          }
          VertexPositionColorTexture[] vertexArray1 = this._vertexArray;
          int index2 = end;
          int num2 = index2 + 1;
          vertexArray1[index2] = batchItem.vertexTL;
          VertexPositionColorTexture[] vertexArray2 = this._vertexArray;
          int index3 = num2;
          int num3 = index3 + 1;
          vertexArray2[index3] = batchItem.vertexTR;
          VertexPositionColorTexture[] vertexArray3 = this._vertexArray;
          int index4 = num3;
          int num4 = index4 + 1;
          vertexArray3[index4] = batchItem.vertexBL;
          VertexPositionColorTexture[] vertexArray4 = this._vertexArray;
          int index5 = num4;
          end = index5 + 1;
          vertexArray4[index5] = batchItem.vertexBR;
          if (batchItem.inPool)
          {
            batchItem.Texture = (Texture2D) null;
            batchItem.Material = (Material) null;
            this._freeBatchItemQueue.Enqueue(batchItem);
          }
          ++num1;
          ++index1;
        }
        this.FlushVertexArray(start, end);
      }
      this._batchItemList.Clear();
    }

    public void DrawSimpleBatch(SpriteSortMode sortMode)
    {
      if (this._simpleBatchItemList.Count == 0)
        return;
      switch (sortMode)
      {
        case SpriteSortMode.BackToFront:
          this._simpleBatchItemList.Sort(new Comparison<MTSimpleSpriteBatchItem>(MTSpriteBatcher.CompareSimpleReverseDepth));
          break;
        case SpriteSortMode.FrontToBack:
          this._simpleBatchItemList.Sort(new Comparison<MTSimpleSpriteBatchItem>(MTSpriteBatcher.CompareSimpleDepth));
          break;
      }
      int index1 = 0;
      int numBatchItems;
      for (int count = this._simpleBatchItemList.Count; count > 0; count -= numBatchItems)
      {
        int start = 0;
        int end = 0;
        numBatchItems = count;
        if (numBatchItems > 5461)
          numBatchItems = 5461;
        this.EnsureSimpleArrayCapacity(numBatchItems);
        int num1 = 0;
        while (num1 < numBatchItems)
        {
          MTSimpleSpriteBatchItem simpleBatchItem = this._simpleBatchItemList[index1];
          VertexPositionColor[] simpleVertexArray1 = this._simpleVertexArray;
          int index2 = end;
          int num2 = index2 + 1;
          simpleVertexArray1[index2] = simpleBatchItem.vertexTL;
          VertexPositionColor[] simpleVertexArray2 = this._simpleVertexArray;
          int index3 = num2;
          int num3 = index3 + 1;
          simpleVertexArray2[index3] = simpleBatchItem.vertexTR;
          VertexPositionColor[] simpleVertexArray3 = this._simpleVertexArray;
          int index4 = num3;
          int num4 = index4 + 1;
          simpleVertexArray3[index4] = simpleBatchItem.vertexBL;
          VertexPositionColor[] simpleVertexArray4 = this._simpleVertexArray;
          int index5 = num4;
          end = index5 + 1;
          simpleVertexArray4[index5] = simpleBatchItem.vertexBR;
          this._freeSimpleBatchItemQueue.Enqueue(simpleBatchItem);
          ++num1;
          ++index1;
        }
        this.FlushSimpleVertexArray(start, end);
      }
      this._simpleBatchItemList.Clear();
    }

    public void DrawGeometryBatch(SpriteSortMode sortMode)
    {
      if (this._geometryBatch.Count == 0)
        return;
      switch (sortMode)
      {
        case SpriteSortMode.BackToFront:
          this._geometryBatch.Sort(new Comparison<GeometryItem>(MTSpriteBatcher.CompareGeometryReverseDepth));
          break;
        case SpriteSortMode.FrontToBack:
          this._geometryBatch.Sort(new Comparison<GeometryItem>(MTSpriteBatcher.CompareGeometryDepth));
          break;
      }
      int num = 0;
      foreach (GeometryItem geometryItem in this._geometryBatch)
        num += geometryItem.length;
      this.EnsureGeometryArrayCapacity((num + 1) / 3);
      Material material = (Material) null;
      int start = 0;
      int end = 0;
      foreach (GeometryItem geometryItem in this._geometryBatch)
      {
        if (!object.ReferenceEquals((object) geometryItem.material, (object) material))
        {
          this.FlushGeometryVertexArray(start, end);
          if (material != null && geometryItem.material == null)
            this._batch.ReapplyEffect(true);
          material = geometryItem.material;
          material?.Apply();
        }
        for (int index = 0; index < geometryItem.length; index += 3)
        {
          this._geometryVertexArray[end++] = geometryItem.vertices[index];
          this._geometryVertexArray[end++] = geometryItem.vertices[index + 1];
          this._geometryVertexArray[end++] = geometryItem.vertices[index + 2];
        }
        if (geometryItem.temporary)
          this._freeGeometryBatch.Enqueue(geometryItem);
      }
      this.FlushGeometryVertexArray(start, end);
      this._geometryBatch.Clear();
    }

    public void DrawTexturedGeometryBatch(SpriteSortMode sortMode)
    {
      if (this._geometryBatchTextured.Count == 0)
        return;
      switch (sortMode)
      {
        case SpriteSortMode.BackToFront:
          this._geometryBatchTextured.Sort(new Comparison<GeometryItemTexture>(MTSpriteBatcher.CompareTexturedGeometryReverseDepth));
          break;
        case SpriteSortMode.FrontToBack:
          this._geometryBatchTextured.Sort(new Comparison<GeometryItemTexture>(MTSpriteBatcher.CompareTexturedGeometryDepth));
          break;
      }
      int num1 = 0;
      foreach (GeometryItemTexture geometryItemTexture in this._geometryBatchTextured)
        num1 += geometryItemTexture.length;
      this.EnsureTexturedGeometryArrayCapacity((num1 + 1) / 3);
      Texture2D texture2D = (Texture2D) null;
      int start = 0;
      int end = 0;
      foreach (GeometryItemTexture geometryItemTexture in this._geometryBatchTextured)
      {
        if (!object.ReferenceEquals((object) geometryItemTexture.texture, (object) texture2D))
        {
          this.FlushTexturedGeometryVertexArray(start, end);
          texture2D = geometryItemTexture.texture;
          start = end = 0;
          this._device.Textures[0] = (Texture) texture2D;
        }
        for (int index1 = 0; index1 < geometryItemTexture.length; index1 += 3)
        {
          VertexPositionColorTexture[] vertexArrayTextured1 = this._geometryVertexArrayTextured;
          int index2 = end;
          int num2 = index2 + 1;
          vertexArrayTextured1[index2] = geometryItemTexture.vertices[index1];
          VertexPositionColorTexture[] vertexArrayTextured2 = this._geometryVertexArrayTextured;
          int index3 = num2;
          int num3 = index3 + 1;
          vertexArrayTextured2[index3] = geometryItemTexture.vertices[index1 + 1];
          VertexPositionColorTexture[] vertexArrayTextured3 = this._geometryVertexArrayTextured;
          int index4 = num3;
          end = index4 + 1;
          vertexArrayTextured3[index4] = geometryItemTexture.vertices[index1 + 2];
        }
        if (geometryItemTexture.temporary)
          this._freeGeometryBatchTextured.Enqueue(geometryItemTexture);
        geometryItemTexture.texture = (Texture2D) null;
        this.FlushTexturedGeometryVertexArray(start, end);
      }
      this._geometryBatchTextured.Clear();
    }

    /// <summary>
    /// Sends the triangle list to the graphics device. Here is where the actual drawing starts.
    /// </summary>
    /// <param name="start">Start index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
    /// <param name="end">End index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
    private void FlushVertexArray(int start, int end)
    {
      if (start == end)
        return;
      int numVertices = end - start;
      this._device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, this._vertexArray, 0, numVertices, this._index, 0, numVertices / 4 * 2, VertexPositionColorTexture.VertexDeclaration);
    }

    private void FlushSimpleVertexArray(int start, int end)
    {
      if (start == end)
        return;
      int numVertices = end - start;
      this._device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, this._simpleVertexArray, 0, numVertices, this._simpleIndex, 0, numVertices / 4 * 2, VertexPositionColor.VertexDeclaration);
    }

    private void FlushGeometryVertexArray(int start, int end)
    {
      if (start == end)
        return;
      int numVertices = end - start;
      this._device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, this._geometryVertexArray, 0, numVertices, this._geometryIndex, 0, numVertices / 3, VertexPositionColor.VertexDeclaration);
    }

    private void FlushTexturedGeometryVertexArray(int start, int end)
    {
      if (start == end)
        return;
      int numVertices = end - start;
      this._device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, this._geometryVertexArrayTextured, 0, numVertices, this._texturedGeometryIndex, 0, numVertices / 3, VertexPositionColorTexture.VertexDeclaration);
    }
  }
}
