// Decompiled with JetBrains decompiler
// Type: DuckGame.FreeSpawn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("spawns")]
  public class FreeSpawn : SpawnPoint
  {
    public EditorProperty<int> spawnType = new EditorProperty<int>(0, max: 2f, increment: 1f);

    public FreeSpawn(float xpos = 0.0f, float ypos = 0.0f)
      : base(xpos, ypos)
    {
      SpriteMap spriteMap = new SpriteMap("duckSpawn", 32, 32);
      spriteMap.depth = new Depth(0.9f);
      this.graphic = (Sprite) spriteMap;
      this._editorName = "free spawn";
      this.center = new Vec2(16f, 23f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -16f);
      this._visibleInGame = false;
    }

    public override void Draw()
    {
      this.frame = (int) this.spawnType;
      base.Draw();
    }
  }
}
