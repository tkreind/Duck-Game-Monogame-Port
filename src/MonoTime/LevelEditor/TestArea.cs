// Decompiled with JetBrains decompiler
// Type: DuckGame.TestArea
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  public class TestArea : Level
  {
    private Editor _editor;
    protected int _seed;
    protected RandomLevelData _center;

    public TestArea(Editor editor, string level, int seed = 0, RandomLevelData center = null)
    {
      this._editor = editor;
      this._level = level;
      this._seed = seed;
      this._center = center;
    }

    public override void Initialize()
    {
      if (this._level == "RANDOM")
      {
        LevelGenerator.MakeLevel(allowSymmetry: (this._center.left && this._center.right), seed: this._seed).LoadParts(0.0f, 0.0f, (Level) this, this._seed);
      }
      else
      {
        IEnumerable<XElement> source = XDocument.Load(this._level).Element((XName) "Level").Elements((XName) "Objects");
        if (source == null)
          return;
        foreach (XElement element in source.Elements<XElement>((XName) "Object"))
        {
          Thing t = Thing.LegacyLoadThing(element);
          if (t != null)
            this.AddThing(t);
        }
      }
    }

    public override void Update() => base.Update();
  }
}
