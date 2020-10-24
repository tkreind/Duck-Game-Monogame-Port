﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Nubber
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class Nubber : MaterialThing, IPlatform, IDontMove, IDontUpdate
  {
    private SpriteMap _sprite;
    public string tileset;

    public void UpdateCustomTileset()
    {
      int num = 0;
      if (this._sprite != null)
        num = this._sprite.frame;
      if (this.tileset == "CUSTOM01")
      {
        CustomTileData data = Custom.GetData(0, CustomType.Block);
        this._sprite = data == null || data.texture == null ? new SpriteMap("blueprintTileset", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      else if (this.tileset == "CUSTOM02")
      {
        CustomTileData data = Custom.GetData(1, CustomType.Block);
        this._sprite = data == null || data.texture == null ? new SpriteMap("blueprintTileset", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      else if (this.tileset == "CUSTOM03")
      {
        CustomTileData data = Custom.GetData(2, CustomType.Block);
        this._sprite = data == null || data.texture == null ? new SpriteMap("blueprintTileset", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      else if (this.tileset == "CUSTOMPLAT01")
      {
        CustomTileData data = Custom.GetData(0, CustomType.Platform);
        this._sprite = data == null || data.texture == null ? new SpriteMap("scaffolding", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      else if (this.tileset == "CUSTOMPLAT02")
      {
        CustomTileData data = Custom.GetData(1, CustomType.Platform);
        this._sprite = data == null || data.texture == null ? new SpriteMap("scaffolding", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      else if (this.tileset == "CUSTOMPLAT03")
      {
        CustomTileData data = Custom.GetData(2, CustomType.Platform);
        this._sprite = data == null || data.texture == null ? new SpriteMap("scaffolding", 16, 16) : new SpriteMap((Tex2D) data.texture, 16, 16);
      }
      if (this._sprite == null)
        this._sprite = new SpriteMap(this.tileset, 16, 16);
      this.graphic = (Sprite) this._sprite;
      this._sprite.frame = num;
    }

    public Nubber(float x, float y, bool left, string tset)
      : base(x, y)
    {
      this.tileset = tset;
      this.UpdateCustomTileset();
      this.graphic = (Sprite) this._sprite;
      this.collisionSize = new Vec2(8f, 5f);
      this._sprite.frame = left ? 62 : 63;
      if (left)
        this.collisionOffset = new Vec2(13f, 0.0f);
      else
        this.collisionOffset = new Vec2(-5f, 0.0f);
      this._editorCanModify = false;
      this.UpdateCustomTileset();
    }

    public override void Terminate()
    {
    }

    public override void Draw() => base.Draw();
  }
}
