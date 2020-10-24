// Decompiled with JetBrains decompiler
// Type: DuckGame.Rope
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class Rope : Thing
  {
    private Thing _attach1;
    private Thing _attach2;
    public Vec2 offsetDir = Vec2.Zero;
    public float linkDirectionOnSplit;
    public float offsetDegrees;
    private float _properLength = -1f;
    private BlockCorner _corner;
    private Vec2 _pos1;
    private Vec2 _pos2;
    private bool _terminated;
    public Thing _thing;
    private Sprite _vine;
    private bool _isVine;
    public Thing _belongsTo;
    private Vec2 tpos = Vec2.Zero;
    public bool serverForObject;

    public Thing attach1
    {
      get => this._attach1;
      set => this._attach1 = value;
    }

    public Thing attach2
    {
      get => this._attach2;
      set => this._attach2 = value;
    }

    public override NetworkConnection connection => this._belongsTo != null ? this._belongsTo.connection : base.connection;

    public override NetIndex8 authority => this._belongsTo != null ? this._belongsTo.authority : base.authority;

    public float linkDirection => this.attach2 is Rope attach2 ? Maths.PointDirection(new Vec2(0.0f, 0.0f), (this.attach2Point - this.attach1Point).Rotate(Maths.DegToRad(attach2.offsetDegrees), Vec2.Zero)) : 0.0f;

    public float linkDirectionNormalized => Maths.PointDirection(new Vec2(0.0f, 0.0f), this.attach2Point - this.attach1Point);

    public float properLength
    {
      get => this._properLength;
      set => this._properLength = value;
    }

    public Vec2 attach1Point
    {
      get
      {
        if (!(this._attach1 is Rope attach1) || attach1._corner == null)
          return this._attach1.position;
        Vec2 vec2 = attach1._corner.corner - attach1._corner.block.position;
        vec2.Normalize();
        return this._attach1.position + vec2 * 4f;
      }
    }

    public Vec2 attach2Point
    {
      get
      {
        if (!(this._attach2 is Rope attach2) || attach2._corner == null)
          return this._attach2.position;
        Vec2 vec2 = attach2._corner.corner - attach2._corner.block.position;
        vec2.Normalize();
        return this._attach2.position + vec2 * 4f;
      }
    }

    public float length => this._attach1 != null && this._attach2 != null ? (this._attach1.position - this._attach2.position).length : 0.0f;

    public Rope(
      float xpos,
      float ypos,
      Thing attach1Val,
      Thing attach2Val,
      Thing thing = null,
      bool vine = false,
      Sprite tex = null,
      Thing belongsTo = null)
      : base(xpos, ypos)
    {
      this._belongsTo = belongsTo;
      if (attach1Val == null)
        attach1Val = (Thing) this;
      if (attach2Val == null)
        attach2Val = (Thing) this;
      this._attach1 = attach1Val;
      this._attach2 = attach2Val;
      this._pos1 = attach1Val.position;
      this._pos2 = attach2Val.position;
      this._thing = thing;
      if (vine)
      {
        this._vine = new Sprite(nameof (vine));
        this._vine.center = new Vec2(8f, 0.0f);
      }
      if (tex != null)
        this._vine = tex;
      this._isVine = vine;
      this.depth = (Depth) -0.5f;
    }

    public void RemoveRope()
    {
      this._terminated = true;
      this.visible = false;
      Level.Remove((Thing) this);
      if (this._attach1 is Rope attach1 && !attach1._terminated)
        attach1.RemoveRope();
      if (!(this._attach2 is Rope attach2) || attach2._terminated)
        return;
      attach2.RemoveRope();
    }

    public void TerminateLaterRopes()
    {
      if (this._attach2 is Rope attach2 && !attach2._terminated)
        attach2.TerminateLaterRopesRecurse();
      this._attach2 = (Thing) null;
    }

    public void TerminateLaterRopesRecurse()
    {
      this._terminated = true;
      this.visible = false;
      Level.Remove((Thing) this);
      if (!(this._attach2 is Rope attach2) || attach2._terminated)
        return;
      attach2.TerminateLaterRopesRecurse();
    }

    public override void Terminate()
    {
    }

    public void CheckLinks()
    {
      if (!(this._attach2.GetType() == typeof (Rope)))
        return;
      Rope attach2_1 = this._attach2 as Rope;
      bool flag = false;
      if ((double) attach2_1.linkDirectionOnSplit > 0.0 && (double) this.linkDirection < 0.0)
        flag = true;
      else if ((double) attach2_1.linkDirectionOnSplit < 0.0 && (double) this.linkDirection > 0.0)
        flag = true;
      if (!flag)
        return;
      this._attach2 = attach2_1.attach2;
      this._properLength += attach2_1.properLength;
      Level.Remove((Thing) attach2_1);
      Rope attach2_2 = this._attach2 as Rope;
    }

    public void AddLength(float length)
    {
      if (this._attach2 is Rope attach2)
      {
        attach2.AddLength(length);
      }
      else
      {
        if (!(this._attach2 is Harpoon attach2))
          return;
        Vec2 vec2 = this.position - attach2.position;
        vec2.Normalize();
        Harpoon harpoon = attach2;
        harpoon.position = harpoon.position - vec2 * length;
      }
    }

    public override void Update()
    {
      if (Network.isActive && this._belongsTo != null && this._belongsTo is Grapple)
      {
        this._isVine = false;
        this._vine = (this._belongsTo as Grapple)._ropeSprite;
      }
      if (this._terminated || !this.isServerForObject || !this.serverForObject)
        return;
      bool flag = false;
      if (this._attach1.position != this._pos1)
      {
        flag = true;
        this._pos1 = this._attach1.position;
      }
      if (this._attach2.position != this._pos2)
      {
        flag = true;
        this._pos2 = this._attach2.position;
      }
      if (flag)
      {
        Vec2 attach1Point = this.attach1Point;
        Vec2 attach2Point = this.attach2Point;
        Vec2 vec2_1 = attach2Point - attach1Point;
        vec2_1.Normalize();
        int num1 = 0;
        while (Level.CheckPoint<Block>(attach2Point) != null)
        {
          ++num1;
          if (num1 <= 30)
            attach2Point -= vec2_1;
          else
            break;
        }
        int num2 = 0;
        Vec2 vec2_2 = attach2Point - attach1Point;
        float num3 = vec2_2.length;
        vec2_2.Normalize();
        while (Level.CheckPoint<Block>(attach1Point) != null)
        {
          ++num2;
          if (num2 > 30)
          {
            num3 = 0.0f;
            break;
          }
          attach1Point += vec2_2;
          --num3;
        }
        if ((double) num3 > 8.0)
        {
          Vec2 position;
          AutoBlock autoBlock = Level.CheckLine<AutoBlock>(attach1Point, attach1Point + vec2_2 * num3, out position);
          if (autoBlock != null)
          {
            BlockCorner nearestCorner = autoBlock.GetNearestCorner(position);
            if (nearestCorner != null)
            {
              BlockCorner blockCorner = nearestCorner.Copy();
              Vec2 vec2_3 = blockCorner.corner - blockCorner.block.position;
              vec2_3.Normalize();
              blockCorner.corner += vec2_3 * 1f;
              if ((double) (blockCorner.corner - this.attach2.position).length > 4.0)
              {
                Rope rope = new Rope(blockCorner.corner.x, blockCorner.corner.y, (Thing) null, this._attach2, vine: this._isVine, tex: this._vine);
                rope._corner = blockCorner;
                rope._belongsTo = this._belongsTo;
                this._attach2 = (Thing) rope;
                Level.Add((Thing) rope);
                this.properLength -= rope.length;
                rope.properLength = rope.length;
                rope.offsetDegrees = rope.linkDirectionNormalized;
                rope.offsetDir = rope.attach2Point - rope.attach1Point;
                rope.linkDirectionOnSplit = this.linkDirection;
              }
            }
          }
        }
      }
      this.CheckLinks();
    }

    public void SetServer(bool server)
    {
      this.serverForObject = server;
      if (!(this._attach2 is Rope attach2) || attach2._terminated)
        return;
      attach2.SetServer(server);
    }

    public override void Draw()
    {
      float num1 = this.length / this.properLength;
      if (!this.serverForObject)
        num1 = 1f;
      if (this._vine != null)
      {
        Vec2 vec2_1 = this.attach2Point - this.attach1Point;
        Vec2 normalized = vec2_1.normalized;
        Vec2 vec2_2 = normalized.Rotate(Maths.DegToRad(90f), Vec2.Zero);
        float length = vec2_1.length;
        Vec2 vec2_3 = this.attach1Point + normalized * 16f;
        Vec2 p1 = this.attach1Point;
        Depth depth = this.depth;
        int num2 = (int) Math.Ceiling((double) length / 16.0);
        for (int index = 0; index < num2; ++index)
        {
          float num3 = 6.283185f / (float) num2 * (float) index;
          float num4 = (float) ((1.0 - (double) num1) * 16.0);
          this._vine.angleDegrees = (float) -((double) Maths.PointDirection(p1, vec2_3 + vec2_2 * (float) Math.Sin((double) num3) * num4) + 90.0);
          this._vine.depth = depth;
          depth += 1;
          this._vine.yscale = 1.1f;
          if (index == num2 - 1)
            Graphics.Draw(this._vine, p1.x, p1.y, new Rectangle(0.0f, 0.0f, 16f, (float) (int) ((double) length % 16.0)));
          else
            Graphics.Draw(this._vine, p1.x, p1.y);
          p1 = vec2_3 + vec2_2 * (float) Math.Sin((double) num3) * num4;
          vec2_3 += normalized * 16f;
          float num5 = num3 + 6.283185f / (float) num2;
        }
      }
      else if ((double) num1 < 0.949999988079071 && (double) num1 > 0.0)
      {
        Vec2 vec2_1 = this.attach2Point - this.attach1Point;
        Vec2 vec2_2 = vec2_1.normalized.Rotate(Maths.DegToRad(90f), Vec2.Zero);
        float num2 = 0.7853982f;
        Vec2 vec2_3 = this.attach1Point + vec2_1 / 8f;
        Vec2 p1 = this.attach1Point;
        for (int index = 0; index < 8; ++index)
        {
          float num3 = (float) ((1.0 - (double) num1) * 8.0);
          Graphics.DrawLine(p1, vec2_3 + vec2_2 * (float) Math.Sin((double) num2) * num3, Color.White * 0.8f, depth: (this.depth - 1));
          p1 = vec2_3 + vec2_2 * (float) Math.Sin((double) num2) * num3;
          vec2_3 += vec2_1 / 8f;
          num2 += 0.7853982f;
        }
      }
      else
        Graphics.DrawLine(this.attach1Point, this.attach2Point, Color.White * 0.8f, depth: (this.depth - 1));
    }
  }
}
