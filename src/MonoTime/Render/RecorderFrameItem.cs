// Decompiled with JetBrains decompiler
// Type: DuckGame.RecorderFrameItem
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public struct RecorderFrameItem
  {
    public short texture;
    public Vec2 topLeft;
    public Vec2 bottomRight;
    public float rotation;
    public Color color;
    public short texX;
    public short texY;
    public short texW;
    public short texH;
    public float depth;
    public long spriteThingDraw;

    public void SetData(
      short textureVal,
      Vec2 topLeftVal,
      Vec2 bottomRightVal,
      float rotationVal,
      Color colorVal,
      short texXVal,
      short texYVal,
      short texWVal,
      short texHVal,
      float depthVal,
      long spriteThingDrawVal)
    {
      this.texture = textureVal;
      this.topLeft = topLeftVal;
      this.bottomRight = bottomRightVal;
      this.rotation = rotationVal;
      this.color = colorVal;
      this.texX = texXVal;
      this.texY = texYVal;
      this.texW = texWVal;
      this.texH = texHVal;
      this.depth = depthVal;
      this.spriteThingDraw = spriteThingDrawVal;
    }
  }
}
