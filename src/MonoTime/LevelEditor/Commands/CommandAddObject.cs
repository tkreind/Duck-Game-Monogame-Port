// Decompiled with JetBrains decompiler
// Type: DuckGame.CommandAddObject
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CommandAddObject : Command
  {
    private Thing _object;

    public CommandAddObject(Thing obj) => this._object = obj;

    public override void OnDo()
    {
      if (!(Level.current is Editor current))
        return;
      current.AddObject(this._object);
    }

    public override void OnUndo()
    {
      if (!(Level.current is Editor current))
        return;
      current.RemoveObject(this._object);
    }
  }
}
