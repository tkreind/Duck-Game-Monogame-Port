// Decompiled with JetBrains decompiler
// Type: DuckGame.WaterFlow
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  [EditorGroup("details")]
  public class WaterFlow : Thing
  {
    public static int waterFrame;
    public static int waterFrameInc;
    public static bool updatedWaterFrame;
    protected HashSet<WaterFlow> _extraWater = new HashSet<WaterFlow>();
    private new bool _initialized;
    private bool _wallLeft;
    private bool _wallRight;
    public bool processed;
    private HashSet<PhysicsObject> _held = new HashSet<PhysicsObject>();

    public WaterFlow(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("waterFlow", 16, 16);
      this.center = new Vec2(8f, 14f);
      this._collisionSize = new Vec2(16f, 4f);
      this._collisionOffset = new Vec2(-8f, -2f);
      this.layer = Layer.Foreground;
      this.depth = (Depth) 0.3f;
      this.alpha = 0.8f;
      this.hugWalls = WallHug.Floor;
    }

    public Rectangle ProcessGroupRect(Rectangle rect)
    {
      if (this.processed)
        return rect;
      if ((double) this.left < (double) rect.x)
      {
        rect.width += (float) (int) ((double) rect.x - (double) this.left);
        rect.x = (float) (int) this.left;
      }
      if ((double) this.right > (double) rect.x + (double) rect.width)
        rect.width += (float) (int) ((double) this.right - ((double) rect.x + (double) rect.width));
      this.processed = true;
      if (!this._wallLeft)
      {
        WaterFlow waterFlow1 = Level.CheckPoint<WaterFlow>(new Vec2(this.x - 16f, this.y));
        if (waterFlow1 != null && waterFlow1 != this && waterFlow1.flipHorizontal == this.flipHorizontal)
        {
          rect = waterFlow1.ProcessGroupRect(rect);
          this._extraWater.Add(waterFlow1);
          foreach (WaterFlow waterFlow2 in waterFlow1._extraWater)
            this._extraWater.Add(waterFlow2);
        }
      }
      if (!this._wallRight)
      {
        WaterFlow waterFlow1 = Level.CheckPoint<WaterFlow>(new Vec2(this.x + 16f, this.y));
        if (waterFlow1 != null && waterFlow1 != this && waterFlow1.flipHorizontal == this.flipHorizontal)
        {
          rect = waterFlow1.ProcessGroupRect(rect);
          this._extraWater.Add(waterFlow1);
          foreach (WaterFlow waterFlow2 in waterFlow1._extraWater)
            this._extraWater.Add(waterFlow2);
        }
      }
      return rect;
    }

    public override void Update()
    {
      if (!this._initialized)
      {
        this._initialized = true;
        if (Level.CheckPoint<Block>(new Vec2(this.x - 16f, this.y)) != null)
          this._wallLeft = true;
        else if (Level.CheckPoint<Block>(new Vec2(this.x + 16f, this.y)) != null)
          this._wallRight = true;
        if (!this.processed)
        {
          Rectangle rectangle = this.ProcessGroupRect(this.rectangle);
          if (this._extraWater.Count > 0)
          {
            this._extraWater.Remove(this);
            foreach (WaterFlow waterFlow in this._extraWater)
            {
              Level.Remove((Thing) waterFlow);
              waterFlow._extraWater.Clear();
            }
            this._collisionSize = new Vec2(rectangle.width, rectangle.height);
            this._collisionOffset = new Vec2(rectangle.x - this.x, this._collisionOffset.y);
          }
        }
      }
      if (!WaterFlow.updatedWaterFrame)
      {
        WaterFlow.updatedWaterFrame = true;
        ++WaterFlow.waterFrameInc;
        if (WaterFlow.waterFrameInc > 3)
        {
          WaterFlow.waterFrameInc = 0;
          ++WaterFlow.waterFrame;
          if (WaterFlow.waterFrame > 3)
            WaterFlow.waterFrame = 0;
        }
      }
      (this.graphic as SpriteMap).frame = WaterFlow.waterFrame;
      foreach (Thing thing in this._extraWater)
        (thing.graphic as SpriteMap).frame = WaterFlow.waterFrame;
      bool flipHorizontal = this.flipHorizontal;
      this.flipHorizontal = false;
      IEnumerable<PhysicsObject> source = Level.CheckRectAll<PhysicsObject>(this.topLeft, this.bottomRight);
      foreach (PhysicsObject physicsObject in source)
      {
        if (flipHorizontal && (double) physicsObject.hSpeed > -2.0)
          physicsObject.hSpeed -= 0.3f;
        else if (!flipHorizontal && (double) physicsObject.hSpeed < 2.0)
          physicsObject.hSpeed += 0.3f;
        physicsObject.sleeping = false;
        physicsObject.frictionMult = 0.3f;
        this._held.Add(physicsObject);
      }
      List<PhysicsObject> physicsObjectList = new List<PhysicsObject>();
      foreach (PhysicsObject physicsObject in this._held)
      {
        if (!source.Contains<PhysicsObject>(physicsObject))
        {
          physicsObjectList.Add(physicsObject);
          physicsObject.frictionMult = 1f;
        }
      }
      foreach (PhysicsObject physicsObject in physicsObjectList)
        this._held.Remove(physicsObject);
      this.flipHorizontal = flipHorizontal;
      base.Update();
    }

    public override void Draw()
    {
      WaterFlow.updatedWaterFrame = false;
      this.graphic.flipH = this.offDir <= (sbyte) 0;
      base.Draw();
      if (!this.flipHorizontal)
      {
        if (this._wallLeft)
          Graphics.Draw(this.graphic, this.x - 4f, this.y, new Rectangle((float) (this.graphic.w - 4), 0.0f, 4f, (float) this.graphic.h));
        if (this._wallRight)
          Graphics.Draw(this.graphic, this.x + 16f, this.y, new Rectangle(0.0f, 0.0f, 4f, (float) this.graphic.h));
      }
      else
      {
        if (this._wallRight)
          Graphics.Draw(this.graphic, this.x + 4f, this.y, new Rectangle((float) (this.graphic.w - 4), 0.0f, 4f, (float) this.graphic.h));
        if (this._wallLeft)
          Graphics.Draw(this.graphic, this.x - 16f, this.y, new Rectangle(0.0f, 0.0f, 4f, (float) this.graphic.h));
      }
      foreach (WaterFlow waterFlow in this._extraWater)
      {
        if (waterFlow != this && waterFlow._extraWater.Count == 0)
          waterFlow.Draw();
      }
    }
  }
}
