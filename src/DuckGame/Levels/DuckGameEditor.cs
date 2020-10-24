// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckGameEditor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  public class DuckGameEditor : Editor
  {
    public override void RunTestLevel(string name)
    {
      LevGenType genType = LevGenType.Any;
      if (this._enableSingle && !this._enableMulti)
        genType = LevGenType.SinglePlayer;
      else if (!this._enableSingle && this._enableMulti)
        genType = LevGenType.Deathmatch;
      Level.current = !this._levelThings.Exists((Predicate<Thing>) (x => x is ChallengeMode)) ? (!this._levelThings.Exists((Predicate<Thing>) (x => x is ArcadeMode)) ? (Level) new DuckGameTestArea((Editor) this, name, this._procSeed, this._centerTile, genType) : (Level) new ArcadeLevel(name)) : (Level) new ChallengeLevel(name);
      Level.current.AddThing((Thing) new EditorTestLevel((Editor) this));
    }

    public override void Update() => base.Update();
  }
}
