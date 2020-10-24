// Decompiled with JetBrains decompiler
// Type: DuckGame.VincentProduct
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class VincentProduct
  {
    public VPType type;
    public int cost;
    public int originalCost;
    public int rarity;
    public int count;
    public Furniture furnitureData;
    public Team teamData;
    public bool sold;

    public Sprite sprite
    {
      get
      {
        if (this.furnitureData != null)
          return (Sprite) this.furnitureData.sprite;
        return this.teamData != null ? (Sprite) this.teamData.hat : (Sprite) null;
      }
    }

    public Color color => this.furnitureData != null ? this.furnitureData.group.color : Color.White;

    public string name
    {
      get
      {
        if (this.furnitureData != null)
          return this.furnitureData.name;
        return this.teamData != null ? this.teamData.name + " HAT" : "Something";
      }
    }

    public string group => this.furnitureData != null ? this.furnitureData.group.name : "HATS";

    public string description
    {
      get
      {
        if (this.furnitureData != null)
          return this.furnitureData.description;
        return this.teamData != null ? this.teamData.description : "What a fine piece of furniture.";
      }
    }

    public void Draw(Vec2 pos, float alpha, float deep)
    {
      if (this.furnitureData != null)
      {
        SpriteMap spriteMap = this.furnitureData.sprite;
        if (this.furnitureData.icon != null)
          spriteMap = this.furnitureData.icon;
        if (this.furnitureData.font != null && this.furnitureData.sprite == null)
        {
          this.furnitureData.font.scale = new Vec2(1f, 1f);
          this.furnitureData.font.Draw("F", pos + new Vec2(-3.5f, -3f), Color.Black, (Depth) (deep + 0.005f));
        }
        spriteMap.depth = (Depth) deep;
        spriteMap.frame = 0;
        spriteMap.alpha = alpha;
        Graphics.Draw((Sprite) spriteMap, pos.x, pos.y);
        spriteMap.alpha = 1f;
      }
      if (this.teamData == null)
        return;
      SpriteMap hat = this.teamData.hat;
      hat.depth = (Depth) deep;
      hat.frame = 0;
      hat.alpha = alpha;
      Graphics.Draw((Sprite) hat, pos.x - (float) (hat.w / 2), pos.y - (float) (hat.h / 2));
      hat.alpha = 1f;
    }
  }
}
