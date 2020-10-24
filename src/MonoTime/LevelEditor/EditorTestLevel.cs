// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorTestLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EditorTestLevel : Thing
  {
    private Editor _editor;
    protected bool _quitTesting;

    public Editor editor => this._editor;

    public EditorTestLevel(Editor editor)
      : base()
      => this._editor = editor;

    public override void Update()
    {
      if (!this._quitTesting || Level.current is ChallengeLevel)
        return;
      Level.current = (Level) this._editor;
    }
  }
}
