// Decompiled with JetBrains decompiler
// Type: DuckGame.EnemySpawn
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EnemySpawn : Thing
  {
    private SpriteMap _spawnSprite;

    public EnemySpawn(float xpos = 0.0f, float ypos = 0.0f)
      : base(xpos, ypos)
    {
      GraphicList graphicList = new GraphicList();
      SpriteMap spriteMap = new SpriteMap("duck", 32, 32);
      spriteMap.depth = (Depth) 0.9f;
      spriteMap.position = new Vec2(-8f, -18f);
      graphicList.Add((Sprite) spriteMap);
      this._spawnSprite = new SpriteMap("spawnSheet", 16, 16);
      this._spawnSprite.depth = (Depth) 0.95f;
      graphicList.Add((Sprite) this._spawnSprite);
      this.graphic = (Sprite) graphicList;
      this._editorName = "enemy spawn";
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
    }

    public override void Draw()
    {
      this._spawnSprite.frame = 0;
      base.Draw();
    }
  }
}
