// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamSpawn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("spawns")]
  [BaggedProperty("isInDemo", true)]
  public class TeamSpawn : SpawnPoint
  {
    public TeamSpawn(float xpos = 0.0f, float ypos = 0.0f)
      : base(xpos, ypos)
    {
      GraphicList graphicList = new GraphicList();
      for (int index = 0; index < 3; ++index)
      {
        SpriteMap spriteMap = new SpriteMap("duck", 32, 32);
        spriteMap.CenterOrigin();
        spriteMap.depth = (Depth) (float) (0.899999976158142 + 0.00999999977648258 * (double) index);
        spriteMap.position = new Vec2((float) ((double) index * 9.41176414489746 - 16.0 + 16.0), -2f);
        graphicList.Add((Sprite) spriteMap);
      }
      this.graphic = (Sprite) graphicList;
      this._editorName = "team spawn";
      this.center = new Vec2(8f, 5f);
      this.collisionSize = new Vec2(32f, 16f);
      this.collisionOffset = new Vec2(-16f, -8f);
      this._visibleInGame = false;
    }
  }
}
