// Decompiled with JetBrains decompiler
// Type: DuckGame.Furniture
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Furniture
  {
    public static FurnitureGroup Cheap = new FurnitureGroup()
    {
      name = "cheap",
      color = new Color(222, 181, 144),
      priceMultiplier = 0.4f,
      additionalRarity = 0
    };
    public static FurnitureGroup Fancy = new FurnitureGroup()
    {
      name = "fancy",
      color = new Color(245, 199, 91),
      font = new BitmapFont("furni/ornateFont", 8),
      priceMultiplier = 6f,
      additionalRarity = 250
    };
    public static FurnitureGroup Everyday = new FurnitureGroup()
    {
      name = "everyday",
      color = new Color(141, 182, 201)
    };
    public static FurnitureGroup Flowers = new FurnitureGroup()
    {
      name = "flowers",
      color = new Color(237, 165, 206),
      font = new BitmapFont("furni/italFont", 8),
      additionalRarity = 80
    };
    public static FurnitureGroup Bathroom = new FurnitureGroup()
    {
      name = "bathroom",
      color = new Color(211, 227, 163),
      additionalRarity = 100
    };
    public static FurnitureGroup Outdoor = new FurnitureGroup()
    {
      name = "outdoor",
      color = new Color(192, 163, 227),
      additionalRarity = 20
    };
    public static FurnitureGroup Stone = new FurnitureGroup()
    {
      name = "stone",
      color = new Color(128, 159, 178),
      priceMultiplier = 3f,
      additionalRarity = 150
    };
    public static FurnitureGroup Instruments = new FurnitureGroup()
    {
      name = "music",
      color = new Color(279, 170, 150),
      priceMultiplier = 4f,
      additionalRarity = 100
    };
    public static FurnitureGroup Momento = new FurnitureGroup()
    {
      name = "momento",
      color = new Color(241, 71, 85)
    };
    public static FurnitureGroup Bar = new FurnitureGroup()
    {
      name = "bar",
      color = new Color(180, 211, 89),
      additionalRarity = 50
    };
    public static FurnitureGroup Characters = new FurnitureGroup()
    {
      name = "characters",
      color = new Color(160, 21, 35),
      priceMultiplier = 2f,
      additionalRarity = 8
    };
    public static FurnitureGroup Default = new FurnitureGroup()
    {
      name = "default",
      color = Colors.DGPurple
    };
    private List<VariatingSprite> _eggSprites = new List<VariatingSprite>();
    public float ballRot;
    public bool rareGen;
    private SpriteMap _photoSprite;
    public bool isSurface;
    public bool stickToFloor;
    public bool stickToRoof;
    public SpriteMap sprite;
    public string name;
    public FurnitureType type;
    public SpriteMap icon;
    public SpriteMap background;
    public BitmapFont font;
    public FurnitureGroup group;
    public short index;
    public float topOffset;
    public int deep;
    private int _rarity;
    public int max;
    public string description;
    public bool canFlip;
    public bool neverFlip;
    public bool canGetInGacha = true;

    public int price
    {
      get
      {
        if (this.type == FurnitureType.Prop)
          return (int) Math.Ceiling((double) (this.sprite.width * this.sprite.height) / 12.0 * (double) this.group.priceMultiplier * (1.0 + (double) this.rarity / 100.0));
        if (this.type == FurnitureType.Theme)
          return (int) Math.Ceiling(100.0 * (double) this.group.priceMultiplier * (1.0 + (double) this.rarity / 100.0));
        return this.type == FurnitureType.Font ? (int) Math.Ceiling(60.0 * (double) this.group.priceMultiplier * (1.0 + (double) this.rarity / 100.0)) : 9999;
      }
    }

    public VariatingSprite GetSprite(ulong id, int variation, VSType t)
    {
      VariatingSprite variatingSprite1 = this._eggSprites.FirstOrDefault<VariatingSprite>((Func<VariatingSprite, bool>) (x => (long) x.id == (long) id && x.variation == variation && x.type == t));
      if (variatingSprite1 != null)
        return variatingSprite1;
      VariatingSprite variatingSprite2 = new VariatingSprite();
      variatingSprite2.variation = variation;
      variatingSprite2.id = id;
      variatingSprite2.type = t;
      if (t == VSType.Egg)
        variatingSprite2.sprite = Profile.GetEggSprite(variation, id);
      else if (t == VSType.Portrait)
        variatingSprite2.sprite = Profile.GetPaintingSprite(variation, id);
      this._eggSprites.Add(variatingSprite2);
      return (VariatingSprite) null;
    }

    public void Draw(
      Vec2 pos,
      Depth depth,
      int variation = 0,
      ulong seed = 0,
      bool affectScale = false,
      bool halfscale = false,
      float angle = 0.0f)
    {
      if (seed == 0UL && Profiles.experienceProfile != null)
        seed = Profiles.experienceProfile.steamID;
      SpriteMap spriteMap = this.sprite;
      if (this.icon != null)
        spriteMap = this.icon;
      if (spriteMap != null && this.neverFlip)
        spriteMap.flipH = false;
      if (this.name == "EGG")
      {
        VariatingSprite sprite = this.GetSprite(seed, variation, VSType.Egg);
        if (sprite != null)
        {
          if (sprite.sprite.texture != null)
          {
            sprite.sprite.depth = depth + 6;
            sprite.sprite.scale = this.sprite.scale;
            DuckGame.Graphics.Draw(sprite.sprite, pos.x - 8f * sprite.sprite.xscale, pos.y - 12f * sprite.sprite.yscale);
            spriteMap.frame = 0;
          }
          if ((sprite.sprite.renderTexture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsContentLost)
            this._eggSprites.Remove(sprite);
        }
      }
      else if (this.name == "PHOTO")
      {
        if (this._photoSprite == null)
          this._photoSprite = new SpriteMap("littleMan", 16, 16);
        this._photoSprite.frame = UILevelBox.LittleManFrame(variation, 7, seed);
        this._photoSprite.depth = depth + 6;
        this._photoSprite.scale = this.sprite.scale;
        DuckGame.Graphics.Draw((Sprite) this._photoSprite, pos.x - 6f * this._photoSprite.xscale, pos.y - 4f * this._photoSprite.yscale, new Rectangle(2f, 0.0f, 12f, 10f));
        DuckGame.Graphics.DrawRect(pos + new Vec2(-6f * this._photoSprite.xscale, -6f * this._photoSprite.yscale), pos + new Vec2(6f * this._photoSprite.xscale, 6f * this._photoSprite.yscale), Colors.DGBlue, depth - 4);
        spriteMap.frame = 0;
      }
      else if (this.name == "EASEL")
      {
        VariatingSprite sprite = this.GetSprite(seed, variation, VSType.Portrait);
        if (sprite != null)
        {
          sprite.sprite.depth = depth + 6;
          sprite.sprite.scale = this.sprite.scale;
          DuckGame.Graphics.Draw(sprite.sprite, pos.x - 9f * sprite.sprite.xscale, pos.y - 8f * sprite.sprite.yscale);
          spriteMap.frame = 0;
          if ((sprite.sprite.renderTexture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsContentLost)
            this._eggSprites.Remove(sprite);
        }
      }
      else
        spriteMap.frame = variation;
      if (this.font != null && this.sprite == null)
      {
        this.font.scale = new Vec2(1f, 1f);
        this.font.Draw("F", pos + new Vec2(-3.5f, -3f), Color.Black, depth + 8);
      }
      if (affectScale)
      {
        if (halfscale && (spriteMap.width > 30 || spriteMap.height > 30))
          spriteMap.scale = new Vec2(0.5f);
        else
          spriteMap.scale = new Vec2(1f);
      }
      spriteMap.depth = depth;
      spriteMap.angle = angle;
      DuckGame.Graphics.Draw((Sprite) spriteMap, pos.x, pos.y);
    }

    public int rarity => this._rarity + this.group.additionalRarity;

    public Furniture(
      bool canflip,
      bool neverflip,
      string desc,
      int rarityval,
      string spr,
      int wide,
      int high,
      string nam,
      FurnitureGroup gr,
      SpriteMap ico = null,
      FurnitureType t = FurnitureType.Prop,
      BitmapFont f = null,
      string bak = null,
      bool stickToroof = false,
      bool stickTofloor = false,
      int deepval = 0,
      int maxval = -1,
      bool canGacha = true)
    {
      this.neverFlip = neverflip;
      this.canGetInGacha = canGacha;
      this.canFlip = canflip;
      this.stickToFloor = stickTofloor;
      this.stickToRoof = stickToroof;
      this.max = maxval;
      this._rarity = rarityval;
      this.description = desc;
      if (spr != null)
        this.sprite = new SpriteMap("furni/" + gr.name + "/" + spr, wide, high);
      if (bak != null)
        this.background = new SpriteMap("furni/" + gr.name + "/" + bak, wide, high);
      this.name = nam;
      this.icon = ico;
      this.type = t;
      this.font = f;
      this.group = gr;
      this.deep = deepval;
      if (stickToroof)
        this.deep += 20;
      if (!stickToroof && !stickTofloor)
        --this.deep;
      if (this.sprite != null)
      {
        this.sprite.CenterOrigin();
        if ((double) this.sprite.height / 2.0 - Math.Floor((double) this.sprite.height / 2.0) == 0.0)
          --this.sprite.centery;
        else
          this.sprite.centery = (float) Math.Floor((double) this.sprite.height / 2.0);
      }
      if (this.icon != null)
        this.icon.CenterOrigin();
      if (this.background == null)
        return;
      this.background.CenterOrigin();
    }

    public Furniture(
      bool canflip,
      bool neverflip,
      string desc,
      int rarityval,
      string spr,
      int wide,
      int high,
      string nam,
      FurnitureGroup gr,
      SpriteMap ico,
      bool stickTofloor,
      bool stickToroof = false,
      bool surface = false,
      float topoff = 0.0f,
      int maxval = -1,
      bool canGacha = true)
    {
      this.neverFlip = neverflip;
      this.canGetInGacha = canGacha;
      this.canFlip = canflip;
      this.stickToFloor = stickTofloor;
      this.stickToRoof = stickToroof;
      this.isSurface = surface;
      this.topOffset = topoff;
      this.description = desc;
      this.max = maxval;
      this._rarity = rarityval;
      if (spr != null)
        this.sprite = new SpriteMap("furni/" + gr.name + "/" + spr, wide, high);
      if (stickToroof)
        this.deep += 20;
      if (!stickToroof && !stickTofloor)
        --this.deep;
      this.name = nam;
      this.icon = ico;
      this.type = FurnitureType.Prop;
      this.group = gr;
      if (this.sprite != null)
      {
        this.sprite.CenterOrigin();
        if ((double) this.sprite.height / 2.0 - Math.Floor((double) this.sprite.height / 2.0) == 0.0)
          --this.sprite.centery;
        else
          this.sprite.centery = (float) Math.Floor((double) this.sprite.height / 2.0);
      }
      if (this.icon != null)
        this.icon.CenterOrigin();
      if (this.background == null)
        return;
      this.background.CenterOrigin();
    }
  }
}
