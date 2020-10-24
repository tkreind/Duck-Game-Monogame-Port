// Decompiled with JetBrains decompiler
// Type: DuckGame.RandomLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class RandomLevel : DeathmatchLevel
  {
    private RandomLevelNode _level;

    public RandomLevel()
      : base("RANDOM")
      => this._level = LevelGenerator.MakeLevel();

    public override void Initialize()
    {
      this._level.LoadParts(0.0f, 0.0f, (Level) this);
      OfficeBackground officeBackground = new OfficeBackground(0.0f, 0.0f);
      officeBackground.visible = false;
      Level.Add((Thing) officeBackground);
      base.Initialize();
    }

    public override void Update() => base.Update();
  }
}
