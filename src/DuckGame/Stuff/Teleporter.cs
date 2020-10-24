// Decompiled with JetBrains decompiler
// Type: DuckGame.Teleporter
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class Teleporter : MaterialThing
  {
    public List<WarpLine> warpLines = new List<WarpLine>();
    private Sprite _bottom;
    private Sprite _top;
    private SinWave _pulse = (SinWave) 0.1f;
    private SinWave _float = (SinWave) 0.2f;
    private Sprite _arrow;
    private Teleporter _link;
    public EditorProperty<bool> noduck = new EditorProperty<bool>(false);
    private Sprite _warpLine;
    private bool _initLinks;
    private List<ITeleport> _teleporting = new List<ITeleport>();
    private List<ITeleport> _teleported = new List<ITeleport>();
    private Vec2 _dir;
    public int direction;

    public Teleporter link => this._link;

    public Teleporter(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.center = new Vec2(8f, 24f);
      this.collisionSize = new Vec2(6f, 32f);
      this.collisionOffset = new Vec2(-3f, -24f);
      this.depth = new Depth(-0.5f);
      this._editorName = nameof (Teleporter);
      this._bottom = new Sprite("teleporterBottom");
      this._bottom.CenterOrigin();
      this._top = new Sprite("teleporterTop");
      this._top.CenterOrigin();
      this._arrow = new Sprite("upArrow");
      this._arrow.CenterOrigin();
      this.thickness = 99f;
    }

    public override void Initialize()
    {
      if (this.noduck.value)
      {
        this._bottom = new Sprite("littleTeleBottom");
        this._bottom.CenterOrigin();
        this._top = new Sprite("littleTeleTop");
        this._top.CenterOrigin();
      }
      this._warpLine = new Sprite("warpLine2");
      base.Initialize();
    }

    public override void Update()
    {
      if (!this._initLinks)
      {
        this._initLinks = true;
        Vec2 vec2 = new Vec2(0.0f, -1f);
        if (this.direction == 1)
          vec2 = new Vec2(0.0f, 1f);
        else if (this.direction == 2)
          vec2 = new Vec2(-1f, 0.0f);
        else if (this.direction == 3)
          vec2 = new Vec2(1f, 0.0f);
        this._link = Level.CheckRay<Teleporter>(this.position + new Vec2(0.0f, -8f) + vec2 * 20f, this.position + new Vec2(0.0f, -8f) + vec2 * 5000f);
        this._dir = vec2;
      }
      if (this._link == null)
        return;
      IEnumerable<ITeleport> source = Level.CheckRectAll<ITeleport>(this.topLeft, this.bottomRight);
      for (int index = 0; index < this._teleported.Count; ++index)
      {
        ITeleport teleport = this._teleported[index];
        if (!source.Contains<ITeleport>(teleport))
        {
          this._teleported.RemoveAt(index);
          --index;
        }
      }
      foreach (ITeleport teleport1 in source)
      {
        if (this.noduck.value)
        {
          switch (teleport1)
          {
            case Duck _:
            case Ragdoll _:
            case RagdollPart _:
            case TrappedDuck _:
              continue;
          }
        }
        ITeleport teleport2 = teleport1;
        if ((teleport2 as Thing).owner == null && (teleport2 as Thing).isServerForObject && (!this._teleported.Contains(teleport2) && !this._teleporting.Contains(teleport2)))
          this._teleporting.Add(teleport2);
      }
      int num1;
      for (int index1 = 0; index1 < this._teleporting.Count; index1 = num1 + 1)
      {
        Thing thing = this._teleporting[index1] as Thing;
        this._teleporting.RemoveAt(index1);
        for (int index2 = 0; index2 < 2; ++index2)
          Level.Add((Thing) SmallSmoke.New(thing.position.x + Rando.Float(-8f, 8f), thing.position.y + Rando.Float(-8f, 8f)));
        Vec2 position1 = thing.position;
        thing.position = this._link.position - (this.position - thing.position);
        if (thing is RagdollPart)
          thing.position.y = this._link.position.y;
        this._link._teleported.Add(thing as ITeleport);
        if (thing is PhysicsObject)
        {
          if ((double) thing.hSpeed > 0.0)
            thing.position.x = this._link.position.x + 8f;
          else
            thing.position.x = this._link.position.x - 8f;
        }
        for (int index2 = 0; index2 < 2; ++index2)
          Level.Add((Thing) SmallSmoke.New(thing.position.x + Rando.Float(-8f, 8f), thing.position.y + Rando.Float(-8f, 8f)));
        num1 = index1 - 1;
        Vec2 vec2 = position1;
        Vec2 position2 = thing.position;
        if (thing is Duck && (thing as Duck).sliding)
        {
          vec2.y += 9f;
          position2.y += 9f;
        }
        if ((double) this._dir.y != 0.0)
        {
          vec2.x = this.position.x;
          position2.x = this._link.position.x;
        }
        float num2 = Math.Max((double) this._dir.x != 0.0 ? thing.height : thing.width, 8f);
        this.warpLines.Add(new WarpLine()
        {
          start = vec2,
          end = position2,
          lerp = 0.6f,
          wide = num2
        });
        thing.OnTeleport();
      }
    }

    public void DrawWarpLines()
    {
      Color color = Color.Purple;
      if ((bool) this.noduck)
        color = Color.Yellow;
      foreach (WarpLine warpLine in this.warpLines)
      {
        Vec2 vec2_1 = warpLine.start - warpLine.end;
        Vec2 vec2_2 = warpLine.end - warpLine.start;
        Graphics.DrawTexturedLine(this._warpLine.texture, warpLine.end, warpLine.end + vec2_1 * (1f - warpLine.lerp), color * 0.8f, warpLine.wide / 32f, new Depth(0.9f));
        Graphics.DrawTexturedLine(this._warpLine.texture, warpLine.start + vec2_2 * warpLine.lerp, warpLine.start, color * 0.8f, warpLine.wide / 32f, new Depth(0.9f));
        warpLine.lerp += 0.1f;
      }
      this.warpLines.RemoveAll((Predicate<WarpLine>) (v => (double) v.lerp >= 1.0));
    }

    public override void Draw()
    {
      base.Draw();
      Color purple = Color.Purple;
      if ((bool) this.noduck)
        Graphics.DrawRect(new Vec2(this.x - 2f, this.y - 23f), new Vec2(this.x + 2f, this.y + 5f), Color.Yellow * (float) ((double) this._pulse.normalized * 0.300000011920929 + 0.200000002980232), this.depth);
      else
        Graphics.DrawRect(new Vec2(this.x - 4f, this.y - 23f), new Vec2(this.x + 4f, this.y + 5f), purple * (float) ((double) this._pulse.normalized * 0.300000011920929 + 0.200000002980232), this.depth);
      this._top.depth = this.depth + 1;
      this._bottom.depth = this.depth + 1;
      Graphics.Draw(this._top, this.x, this.y - 23f);
      Graphics.Draw(this._bottom, this.x, this.y + 5f);
      this._arrow.depth = this.depth + 2;
      this._arrow.alpha = 0.5f;
      if (this.direction == 0)
        this._arrow.angleDegrees = 0.0f;
      else if (this.direction == 1)
        this._arrow.angleDegrees = 180f;
      else if (this.direction == 2)
        this._arrow.angleDegrees = -90f;
      else if (this.direction == 3)
        this._arrow.angleDegrees = 90f;
      Graphics.Draw(this._arrow, this.x, (float) ((double) this.y - 10.0 + (double) (float) this._float * 2.0));
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("direction", (object) this.direction);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.direction = node.GetProperty<int>("direction");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "direction", (object) Change.ToString((object) this.direction)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "direction");
      if (xelement != null)
        this.direction = Convert.ToInt32(xelement.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.AddItem((ContextMenu) new ContextRadio("Up", this.direction == 0, (object) 0, (IContextListener) null, new FieldBinding((object) this, "direction")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Down", this.direction == 1, (object) 1, (IContextListener) null, new FieldBinding((object) this, "direction")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Left", this.direction == 2, (object) 2, (IContextListener) null, new FieldBinding((object) this, "direction")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Right", this.direction == 3, (object) 3, (IContextListener) null, new FieldBinding((object) this, "direction")));
      return (ContextMenu) contextMenu;
    }
  }
}
