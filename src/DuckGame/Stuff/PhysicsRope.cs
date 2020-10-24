﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.PhysicsRope
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [BaggedProperty("canSpawn", false)]
  [EditorGroup("stuff|ropes")]
  public class PhysicsRope : Thing
  {
    protected bool chain;
    protected Sprite _vine;
    protected Sprite _vineEnd;
    protected Sprite _beam;
    private bool _root;
    private List<PhysicsRopeSection> _nodes = new List<PhysicsRopeSection>();
    private int _divisions = 10;
    private float _length = 0.5f;
    private float _lenDiv;
    private float _gravity = 0.25f;
    private bool _create = true;
    public Vine _lowestVine;
    private int _lowestVineSection;
    public EditorProperty<int> length = new EditorProperty<int>(4, min: 1f, max: 16f, increment: 1f);
    private float soundWait;

    public bool root
    {
      get => this._root;
      set => this._root = value;
    }

    public PhysicsRope(float xpos, float ypos, PhysicsRope next = null)
      : base(xpos, ypos)
    {
      this._vine = (Sprite) new SpriteMap("vine", 16, 16);
      this.graphic = this._vine;
      this.center = new Vec2(8f, 8f);
      this._vineEnd = new Sprite("vineStretchEnd");
      this._vineEnd.center = new Vec2(8f, 0.0f);
      this.collisionOffset = new Vec2(-5f, -4f);
      this.collisionSize = new Vec2(11f, 7f);
      this.graphic = this._vine;
      this._beam = new Sprite("vineStretch");
      this._editorName = "Vine";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this.position.y -= 8f;
      this._length = (float) this.length.value / 6.5f;
      this._divisions = (int) ((double) this.length.value * 16.0 / 8.0);
      this._lenDiv = this._length / (float) this._divisions;
      for (int index = 0; index <= this._divisions; ++index)
      {
        this._nodes.Add(new PhysicsRopeSection(this.x + this._lenDiv * (float) index, this.y, this));
        Level.Add((Thing) this._nodes[this._nodes.Count - 1]);
      }
    }

    public override void Terminate()
    {
      foreach (Thing node in this._nodes)
        Level.Remove(node);
      base.Terminate();
    }

    public virtual Vine GetSection(float x, float y, int div) => new Vine(x, y, (float) div);

    public Vine LatchOn(PhysicsRopeSection section, Duck d)
    {
      this.UpdateVineProgress();
      for (int index = 0; index <= this._divisions; ++index)
      {
        if (section == this._nodes[index])
        {
          if (index < this._lowestVineSection)
          {
            int num = index;
            Vine lowestVine1 = this._lowestVine;
            Vec2 vec2 = new Vec2(this.x, this.y + 8f);
            this._lowestVine = this.GetSection(vec2.x, vec2.y, num * 8);
            this._lowestVine.length.value = num / 2;
            this._lowestVine.owner = (Thing) d;
            this._lowestVine.sectionIndex = index;
            Level.Add((Thing) this._lowestVine);
            if (lowestVine1 != null)
            {
              lowestVine1._rope.attach2 = this._lowestVine.owner;
              lowestVine1._rope.properLength = (lowestVine1._rope.attach2Point - lowestVine1._rope.attach1Point).length;
              this._lowestVine.nextVine = lowestVine1;
              lowestVine1.prevVine = this._lowestVine;
            }
            this._lowestVine.changeSpeed = false;
            if ((double) d.vSpeed > 0.0)
              d.vSpeed = 0.0f;
            this._lowestVine.UpdateRopeStuff();
            this._lowestVine.UpdateRopeStuff();
            this._lowestVine.changeSpeed = true;
            Vine lowestVine2 = this._lowestVine;
            this._lowestVine = lowestVine1;
            return lowestVine2;
          }
          int num1 = index - this._lowestVineSection;
          Vine lowestVine = this._lowestVine;
          Vec2 vec2_1 = new Vec2(this.x, this.y + 8f);
          if (this._lowestVine != null)
            vec2_1 = this._lowestVine._rope.attach1Point;
          this._lowestVine = this.GetSection(vec2_1.x, vec2_1.y, num1 * 8);
          this._lowestVine.length.value = num1 / 2;
          this._lowestVine.owner = (Thing) d;
          this._lowestVine.sectionIndex = index;
          Level.Add((Thing) this._lowestVine);
          if (lowestVine != null)
          {
            this._lowestVine._rope.attach2 = lowestVine.owner;
            lowestVine.nextVine = this._lowestVine;
            this._lowestVine.prevVine = lowestVine;
          }
          this._lowestVine.changeSpeed = false;
          if ((double) d.vSpeed > 0.0)
            d.vSpeed = 0.0f;
          this._lowestVine.UpdateRopeStuff();
          this._lowestVine.UpdateRopeStuff();
          this._lowestVine.changeSpeed = true;
          this._lowestVineSection = index;
          return this._lowestVine;
        }
      }
      return (Vine) null;
    }

    public Vine highestVine
    {
      get
      {
        if (this._lowestVine == null || this._lowestVine.removeFromLevel)
          return (Vine) null;
        Vine vine = this._lowestVine;
        while (vine.prevVine != null)
          vine = vine.prevVine;
        return vine;
      }
    }

    public void UpdateVineProgress()
    {
      if (this._lowestVine != null && this._lowestVine.owner != null)
        this._nodes[this._lowestVineSection].position = this._lowestVine.owner.position;
      else if (this._lowestVine != null && this._lowestVine.prevVine != null)
      {
        this._lowestVine = this._lowestVine.prevVine;
        this._lowestVineSection = this._lowestVine.sectionIndex;
      }
      else
      {
        this._lowestVineSection = 0;
        this._lowestVine = (Vine) null;
      }
    }

    public override void Update()
    {
      if (this._create)
      {
        int num = 0;
        foreach (Transform node in this._nodes)
        {
          node.position = this.position + new Vec2(0.0f, (float) (num * 8));
          ++num;
        }
        this._create = false;
        for (int index = 0; index < 10; ++index)
          this.Update();
      }
      base.Update();
      if (this._nodes.Count == 0)
        return;
      this.UpdateVineProgress();
      this._nodes[0].position = this.position;
      foreach (PhysicsRopeSection node in this._nodes)
      {
        node.accel.y = this._gravity;
        node.calcPos = node.position;
      }
      for (int index = 1; index <= this._divisions; ++index)
      {
        float x = this._nodes[index].position.x;
        this._nodes[index].calcPos.x += (float) (0.999000012874603 * (double) this._nodes[index].calcPos.x - 0.999000012874603 * (double) this._nodes[index].tempPos.x) + this._nodes[index].accel.x;
        float y = this._nodes[index].position.y;
        this._nodes[index].calcPos.y += (float) (0.999000012874603 * (double) this._nodes[index].calcPos.y - 0.999000012874603 * (double) this._nodes[index].tempPos.y) + this._nodes[index].accel.y;
        this._nodes[index].tempPos.x = x;
        this._nodes[index].tempPos.y = y;
      }
      for (int index1 = 1; index1 <= this._divisions; ++index1)
      {
        for (int index2 = 1; index2 <= this._divisions; ++index2)
        {
          float num1 = (float) (((double) this._nodes[index2].calcPos.x - (double) this._nodes[index2 - 1].calcPos.x) / 100.0);
          float num2 = (float) (((double) this._nodes[index2].calcPos.y - (double) this._nodes[index2 - 1].calcPos.y) / 100.0);
          float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
          float num4 = (float) (((double) num3 - (double) this._lenDiv) * 50.0);
          this._nodes[index2].calcPos.x -= num1 / num3 * num4;
          this._nodes[index2].calcPos.y -= num2 / num3 * num4;
          this._nodes[index2 - 1].calcPos.x += num1 / num3 * num4;
          this._nodes[index2 - 1].calcPos.y += num2 / num3 * num4;
        }
      }
      Vine highestVine = this.highestVine;
      List<VineSection> vineSectionList = (List<VineSection>) null;
      VineSection vineSection = (VineSection) null;
      int index3 = 0;
      if (highestVine != null)
      {
        vineSectionList = highestVine.points;
        vineSection = vineSectionList[0];
      }
      bool flag1 = false;
      bool flag2 = false;
      int num5 = 0;
      for (int index1 = 0; index1 <= this._divisions; ++index1)
      {
        if (vineSection != null)
        {
          if (index1 >= vineSection.lowestSection)
          {
            ++index3;
            vineSection = index3 >= vineSectionList.Count ? (VineSection) null : vineSectionList[index3];
            num5 = 0;
          }
          if (vineSection != null && index1 < vineSection.lowestSection)
          {
            Vec2 vec2 = vineSection.pos2 - vineSection.pos1;
            vec2.Normalize();
            this._nodes[index1].position = vineSection.pos1 + vec2 * (float) num5 * 8f;
            this._nodes[index1].calcPos = this._nodes[index1].position;
          }
        }
        ++num5;
        this._nodes[index1].frictionMult = 0.0f;
        this._nodes[index1].gravMultiplier = 0.0f;
        this._nodes[index1].hSpeed = this._nodes[index1].calcPos.x - this._nodes[index1].position.x;
        this._nodes[index1].vSpeed = this._nodes[index1].calcPos.y - this._nodes[index1].position.y;
        float num1 = 5f;
        if ((double) this._nodes[index1].hSpeed > 0.0 && (double) this._nodes[index1].hSpeed > (double) num1)
          this._nodes[index1].hSpeed = num1;
        if ((double) this._nodes[index1].hSpeed < 0.0 && (double) this._nodes[index1].hSpeed < -(double) num1)
          this._nodes[index1].hSpeed = -num1;
        foreach (PhysicsObject physicsObject in Level.CheckPointAll<PhysicsObject>(this._nodes[index1].position))
        {
          if ((double) physicsObject.hSpeed > 0.0 && (double) this._nodes[index1].hSpeed < (double) physicsObject.hSpeed)
          {
            this._nodes[index1].hSpeed += physicsObject.hSpeed;
            if ((double) Math.Abs(physicsObject.hSpeed) > 2.0)
            {
              if ((double) Math.Abs(physicsObject.hSpeed) > 4.0)
                flag2 = true;
              flag1 = true;
            }
          }
          if ((double) physicsObject.hSpeed < 0.0 && (double) this._nodes[index1].hSpeed > (double) physicsObject.hSpeed)
          {
            this._nodes[index1].hSpeed += physicsObject.hSpeed;
            if ((double) Math.Abs(physicsObject.hSpeed) > 2.0)
            {
              if ((double) Math.Abs(physicsObject.hSpeed) > 4.0)
                flag2 = true;
              flag1 = true;
            }
          }
        }
        this._nodes[index1].UpdatePhysics();
      }
      if ((double) this.soundWait > 0.0)
        this.soundWait -= 0.01f;
      if (!this.chain || !flag1 || (double) this.soundWait > 0.0)
        return;
      this.soundWait = 0.1f;
      if (!flag1)
        return;
      int num6 = Rando.Int(2);
      if (flag2)
      {
        if (num6 == 0)
          SFX.Play("chainShake01", Rando.Float(0.6f, 0.8f), Rando.Float(-0.2f, 0.2f));
        else if (num6 == 1)
          SFX.Play("chainShake02", Rando.Float(0.6f, 0.8f), Rando.Float(-0.2f, 0.2f));
        else
          SFX.Play("chainShake03", Rando.Float(0.6f, 0.8f), Rando.Float(-0.2f, 0.2f));
      }
      else if (num6 == 0)
        SFX.Play("chainShakeSmall", Rando.Float(0.3f, 0.5f), Rando.Float(-0.2f, 0.2f));
      else if (num6 == 1)
        SFX.Play("chainShakeSmall02", Rando.Float(0.3f, 0.5f), Rando.Float(-0.2f, 0.2f));
      else
        SFX.Play("chainShakeSmall03", Rando.Float(0.3f, 0.5f), Rando.Float(-0.2f, 0.2f));
    }

    public override void Draw()
    {
      if (Level.current is Editor)
      {
        this.graphic.center = new Vec2(8f, 8f);
        this.graphic.depth = this.depth;
        for (int index = 0; index < (int) this.length; ++index)
          Graphics.Draw(this.graphic, this.x, this.y + (float) (index * 16));
      }
      else
      {
        this.UpdateVineProgress();
        Depth depth = (Depth) -0.5f;
        Vec2 p1 = this.position + new Vec2(0.0f, -4f);
        if (this._lowestVine != null && this._lowestVine.owner != null)
        {
          p1 = this._lowestVine.owner.position;
          if (this.highestVine != null && this.highestVine._rope.attach2 is Harpoon)
            Graphics.DrawTexturedLine(this._beam.texture, this.position + new Vec2(0.0f, -4f), this._nodes[0].position + new Vec2(0.0f, 2f), Color.White, depth: depth);
        }
        int num = -1;
        foreach (PhysicsRopeSection node in this._nodes)
        {
          depth += 1;
          ++num;
          if (num >= this._lowestVineSection)
          {
            Vec2 normalized = (node.position - p1).normalized;
            if (num == this._nodes.Count - 1)
              Graphics.DrawTexturedLine(this._vineEnd.texture, p1, node.position + normalized, Color.White, depth: depth);
            else
              Graphics.DrawTexturedLine(this._beam.texture, p1, node.position + normalized, Color.White, depth: depth);
            p1 = node.position;
          }
        }
      }
    }
  }
}
