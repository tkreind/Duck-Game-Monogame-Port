// Decompiled with JetBrains decompiler
// Type: DuckGame.Profile
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  public class Profile
  {
    public float storeValue;
    private static BitmapFont _defaultFont;
    private List<FurniturePosition> _internalFurniturePositions = new List<FurniturePosition>();
    private Dictionary<int, int> _internalFurnitures = new Dictionary<int, int>();
    private List<Furniture> _availableList;
    public int roundsSinceXP;
    public int littleManBucks;
    public int numLittleMen;
    public int littleManLevel = 1;
    public int milkFill;
    public int numSandwiches;
    public int currentDay;
    private string _name = "";
    public static bool loading = false;
    private string _id;
    private static MTSpriteBatch _batch;
    private static SpriteMap _egg;
    private static SpriteMap _eggShine;
    private static SpriteMap _eggBorder;
    private static SpriteMap _eggOuter;
    private static SpriteMap _eggSymbols;
    private static List<Color> _allowedColors;
    private static SpriteMap _easel;
    private static SpriteMap _easelSymbols;
    private List<string> _unlocks = new List<string>();
    private int _ticketCount;
    public int timesMetVincent;
    public int timesMetVincentSale;
    public int timesMetVincentSell;
    public int timesMetVincentImport;
    public float timeOfDay;
    private int _xp;
    public byte flippers;
    private float _funSlider = 0.5f;
    private ProfileStats _stats = new ProfileStats();
    private ProfileStats _junkStats;
    private ProfileStats _prevStats = new ProfileStats();
    private ProfileStats _endOfRoundStats;
    private CurrentGame _currentGame = new CurrentGame();
    public string lastKnownName;
    private ulong _steamID;
    private NetworkConnection _connection;
    private bool _acceptedMigration;
    private DuckNetStatus _networkStatus;
    private float _currentStatusTimeout;
    private int _currentStatusTries;
    private Profile _linkedProfile;
    private bool _hasCustomHats;
    private bool _ready = true;
    private byte _networkIndex = byte.MaxValue;
    private SlotType _slotType;
    private object _reservedUser;
    private Team _reservedTeam;
    private Duck _duck;
    private DuckPersona _persona;
    private InputProfile _inputProfile;
    private Team _team;
    private int _wins;
    private bool _wasRockThrower;
    private List<DeviceInputMapping> _inputMappingOverrides = new List<DeviceInputMapping>();
    public bool isNetworkProfile;
    public string fileName = "";

    public BitmapFont font
    {
      get
      {
        if (Profile._defaultFont == null)
          Profile._defaultFont = new BitmapFont("biosFont", 8);
        BitmapFont bitmapFont = Profile._defaultFont;
        foreach (FurniturePosition furniturePosition in this._furniturePositions)
        {
          if (furniturePosition != null)
          {
            Furniture furniture = RoomEditor.GetFurniture((int) furniturePosition.id);
            if (furniture != null && furniture.type == FurnitureType.Font && furniture.font != null)
            {
              bitmapFont = furniture.font;
              break;
            }
          }
        }
        return bitmapFont;
      }
    }

    private List<FurniturePosition> _furniturePositions
    {
      get
      {
        this._internalFurniturePositions.RemoveAll((Predicate<FurniturePosition>) (x => x == null));
        return this._linkedProfile != null ? this._linkedProfile._furniturePositions : this._internalFurniturePositions;
      }
      set
      {
        if (this._linkedProfile != null)
          this._linkedProfile._furniturePositions = value;
        else
          this._furniturePositions = value;
      }
    }

    public List<FurniturePosition> furniturePositions => this._furniturePositions;

    public BitBuffer furniturePositionData
    {
      get
      {
        if (this._linkedProfile != null)
          return this._linkedProfile.furniturePositionData;
        BitBuffer bitBuffer = new BitBuffer();
        foreach (FurniturePosition furniturePosition in this._furniturePositions)
        {
          bitBuffer.Write(furniturePosition.x);
          bitBuffer.Write(furniturePosition.y);
          bitBuffer.Write(furniturePosition.variation);
          bitBuffer.WriteBits((object) furniturePosition.id, 15);
          bitBuffer.WriteBits((object) (furniturePosition.flip ? 1 : 0), 1);
        }
        return bitBuffer;
      }
      set
      {
        if (this._linkedProfile != null)
        {
          this._linkedProfile.furniturePositionData = value;
        }
        else
        {
          this._furniturePositions.Clear();
          try
          {
            while (value.position != value.lengthInBytes)
              this._furniturePositions.Add(new FurniturePosition()
              {
                x = value.ReadByte(),
                y = value.ReadByte(),
                variation = value.ReadByte(),
                id = value.ReadBits<ushort>(15),
                flip = value.ReadBits<byte>(1) > (byte) 0
              });
          }
          catch (Exception ex)
          {
            DevConsole.Log(DCSection.General, "Failed to load furniture position data.");
            this._furniturePositions.Clear();
          }
        }
      }
    }

    public int GetNumFurnituresPlaced(int idx)
    {
      int num = 0;
      foreach (FurniturePosition furniturePosition in this._furniturePositions)
      {
        if ((int) furniturePosition.id == idx)
          ++num;
      }
      return num;
    }

    public int GetTotalFurnituresPlaced()
    {
      int num = 0;
      foreach (FurniturePosition furniturePosition in this._furniturePositions)
      {
        Furniture furniture = RoomEditor.GetFurniture((int) furniturePosition.id);
        if (furniture != null && furniture.type != FurnitureType.Theme && furniture.type != FurnitureType.Font)
          ++num;
      }
      return num;
    }

    public BitBuffer furnitureOwnershipData
    {
      get
      {
        if (this._linkedProfile != null)
          return this._linkedProfile.furnitureOwnershipData;
        BitBuffer bitBuffer = new BitBuffer();
        foreach (KeyValuePair<int, int> furniture in this._furnitures)
        {
          bitBuffer.Write(furniture.Key);
          bitBuffer.Write(furniture.Value);
        }
        return bitBuffer;
      }
      set
      {
        if (this._linkedProfile != null)
        {
          this._linkedProfile.furnitureOwnershipData = value;
        }
        else
        {
          this._furnitures.Clear();
          this._availableList = (List<Furniture>) null;
          try
          {
            while (value.position != value.lengthInBytes)
            {
              FurniturePosition furniturePosition = new FurniturePosition();
              this._furnitures[value.ReadInt()] = value.ReadInt();
            }
          }
          catch (Exception ex)
          {
            DevConsole.Log(DCSection.General, "Failed to load furniture ownership data.");
          }
        }
      }
    }

    public void ClearFurnitures()
    {
      this._furnitures.Clear();
      this._furniturePositions.Clear();
    }

    private Dictionary<int, int> _furnitures
    {
      get => this._linkedProfile != null ? this._linkedProfile._furnitures : this._internalFurnitures;
      set
      {
        if (this._linkedProfile != null)
          this._linkedProfile._furnitures = value;
        else
          this._internalFurnitures = value;
      }
    }

    public int GetNumFurnitures(int idx)
    {
      if (NetworkDebugger.enabled)
        return 99;
      int num = 0;
      Furniture furniture = RoomEditor.GetFurniture(idx);
      if (furniture != null && Profiles.experienceProfile != null)
      {
        if (furniture.group == Furniture.Default)
          return 1;
        if (furniture.name == "EGG" || furniture.name == "PHOTO")
          return Profiles.experienceProfile.numLittleMen + 1;
      }
      this._furnitures.TryGetValue(idx, out num);
      return num;
    }

    public void SetNumFurnitures(int idx, int num)
    {
      this._furnitures[idx] = num;
      this._availableList = (List<Furniture>) null;
    }

    public int GetTotalFurnitures()
    {
      if (NetworkDebugger.enabled)
        return 99;
      int num = 0;
      foreach (KeyValuePair<int, int> furniture in this._furnitures)
        num += furniture.Value;
      return num;
    }

    private string Stringlonger(int length)
    {
      string str = "";
      for (int index = 0; index < length; ++index)
        str += "z";
      return str;
    }

    public List<Furniture> GetAvailableFurnis()
    {
      if (NetworkDebugger.enabled)
        return RoomEditor.AllFurnis();
      if (this._availableList == null)
      {
        this._availableList = new List<Furniture>();
        foreach (KeyValuePair<int, int> furniture1 in this._furnitures)
        {
          if (furniture1.Value > 0)
          {
            Furniture furniture2 = RoomEditor.GetFurniture(furniture1.Key);
            if (furniture2 != null)
              this._availableList.Add(furniture2);
          }
        }
        this._availableList = this._availableList.OrderBy<Furniture, string>((Func<Furniture, string>) (x => x.group.name + this.Stringlonger(RoomEditor._furniGroupMap[x.group].IndexOf(x)))).ToList<Furniture>();
        foreach (Furniture allFurni in RoomEditor.AllFurnis())
        {
          if (allFurni.group == Furniture.Default)
            this._availableList.Add(allFurni);
        }
      }
      return this._availableList;
    }

    public string rawName => this._name;

    public string name
    {
      get
      {
        if (this.steamID != 0UL && this.slotType != SlotType.Local)
        {
          if (Steam.user != null && (long) this.steamID == (long) Steam.user.id)
            return Steam.user.name;
          if (this.lastKnownName != null)
            return this.lastKnownName;
          if (this._name == this.steamID.ToString())
          {
            if (Steam.IsInitialized())
            {
              User user = User.GetUser(this.steamID);
              if (user != null && user.id != 0UL)
              {
                this.lastKnownName = user.name;
                return this.lastKnownName;
              }
            }
            return "STEAM PROFILE";
          }
        }
        return this._name;
      }
      set => this._name = value;
    }

    public static bool logStats => true;

    public string id => this._id;

    public void SetID(string varID) => this._id = varID;

    private static Color PickColor()
    {
      int index = Rando.Int(Profile._allowedColors.Count - 1);
      Color allowedColor = Profile._allowedColors[index];
      Profile._allowedColors.RemoveAt(index);
      return allowedColor;
    }

    public static Random GetLongGenerator(ulong id)
    {
      Random random = new Random(Math.Abs((int) (id % (ulong) int.MaxValue)));
      for (int index = 0; index < (int) (id % 252UL); ++index)
        Rando.Int(100);
      return random;
    }

    public static Random steamGenerator
    {
      get
      {
        if (Steam.user == null)
          return new Random(90210);
        Random random = new Random(Math.Abs((int) (Steam.user.id % (ulong) int.MaxValue)));
        for (int index = 0; index < (int) (Steam.user.id % 252UL); ++index)
          Rando.Int(100);
        return random;
      }
    }

    public static Sprite GetEggSprite(int index = 0, ulong seed = 0)
    {
      if (seed == 0UL && Profiles.experienceProfile != null)
        seed = Profiles.experienceProfile.steamID;
      Sprite s = new Sprite()
      {
        renderTexture = new RenderTarget2D(16, 16, false, RenderTargetUsage.PreserveContents)
      };
      s.texture = (Tex2D) s.renderTexture;
      DuckGame.Graphics.AddRenderTask((Action) (() => Profile.GetEggTexture(index, s.renderTexture, seed)));
      return s;
    }

    public static void GetEggTexture(int index, RenderTarget2D targ, ulong seed)
    {
      if (Profile._egg == null)
      {
        Profile._batch = new MTSpriteBatch(DuckGame.Graphics.device);
        Profile._egg = new SpriteMap("online/eggWhite", 16, 16);
        Profile._eggShine = new SpriteMap("online/eggShine", 16, 16);
        Profile._eggBorder = new SpriteMap("online/eggBorder", 16, 16);
        Profile._eggOuter = new SpriteMap("online/eggOuter", 16, 16);
        Profile._eggSymbols = new SpriteMap("online/eggSymbols", 16, 16);
      }
      Random generator = Rando.generator;
      Rando.generator = Profile.GetLongGenerator(seed);
      for (int index1 = 0; index1 < index; ++index1)
        Rando.Int(100);
      bool flag1 = (double) Rando.Float(1f) > 0.0199999995529652;
      bool flag2 = (double) Rando.Float(1f) > 0.899999976158142;
      bool flag3 = (double) Rando.Float(1f) > 0.400000005960464;
      bool flag4 = Rando.Int(8) == 1;
      Profile._allowedColors = new List<Color>()
      {
        Colors.DGBlue,
        Colors.DGYellow,
        Colors.DGRed,
        Color.White,
        new Color(48, 224, 242),
        new Color(199, 234, 96)
      };
      Profile._allowedColors.Add(Colors.DGPink);
      Profile._allowedColors.Add(new Color((byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200))));
      if (Rando.Int(6) == 1)
      {
        Profile._allowedColors.Add(Colors.DGPurple);
        Profile._allowedColors.Add(Colors.DGEgg);
      }
      else if (Rando.Int(100) == 1)
      {
        Profile._allowedColors.Add(Colors.SuperDarkBlueGray);
        Profile._allowedColors.Add(Colors.BlueGray);
        Profile._allowedColors.Add(Colors.DGOrange);
        Profile._allowedColors.Add(new Color((byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200))));
      }
      else if (Rando.Int(1200) == 1)
        Profile._allowedColors.Add(Colors.Platinum);
      else if (Rando.Int(100000) == 1)
        Profile._allowedColors.Add(new Color(250, 10, 250));
      else if (Rando.Int(1000000) == 1)
        Profile._allowedColors.Add(new Color(229, 245, 181));
      DuckGame.Graphics.SetRenderTarget(targ);
      DuckGame.Graphics.Clear(Color.Black);
      Profile._batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect) null, Matrix.Identity);
      int num1 = 0;
      int num2 = 8 + Rando.Int(12);
      if (Rando.Int(100) == 1)
        num1 = 1;
      int num3 = 3;
      if (Rando.Int(10) == 1)
        num2 = 1;
      if (Rando.Int(30) == 1)
        num2 = 5;
      if (Rando.Int(100) == 1)
        num2 = 2;
      else if (Rando.Int(200) == 1)
        num2 = 3;
      else if (Rando.Int(1000) == 1)
        num2 = 4;
      else if (Rando.Int(10000) == 1)
        num2 = 7;
      else if (Rando.Int(1000000) == 1)
        num2 = 6;
      bool flag5 = Rando.Int(300) == 1;
      MTSpriteBatch screen = DuckGame.Graphics.screen;
      DuckGame.Graphics.screen = Profile._batch;
      Profile._batch.Draw(Profile._egg.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num3 * 16), 0.0f, 16f, 16f)), Color.White, 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
      if (flag3)
      {
        if (flag5)
        {
          char character = BitmapFont._characters[Rando.Int(33, 59)];
          if (Rando.Int(5) == 1)
            character = BitmapFont._characters[Rando.Int(16, 26)];
          else if (Rando.Int(50) == 1)
            character = BitmapFont._characters[Rando.Int(BitmapFont._characters.Length - 1)];
          DuckGame.Graphics.DrawString(string.Concat((object) character), new Vec2(4f, 6f), new Color(60, 60, 60, 200), (Depth) 0.9f);
        }
        else
          Profile._batch.Draw(Profile._eggSymbols.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num2 * 16), 0.0f, 16f, 16f)), new Color(60, 60, 60, 200), 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 0.9f);
      }
      Profile._batch.Draw(Profile._eggOuter.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num3 * 16), 0.0f, 16f, 16f)), Color.White, 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
      Profile._batch.End();
      DuckGame.Graphics.screen = screen;
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      Color[] data = targ.GetData();
      float num4 = 0.09999999f;
      Color color1 = Profile.PickColor();
      Color color2 = Profile.PickColor();
      Profile.PickColor();
      Color color3 = Profile.PickColor();
      float num5 = Rando.Float(100000f);
      float num6 = Rando.Float(100000f);
      for (int index1 = 0; index1 < targ.height; ++index1)
      {
        for (int index2 = 0; index2 < targ.width; ++index2)
        {
          float num7 = (float) (index2 + 32) * 0.75f;
          int num8 = index1 + 32;
          float num9 = (float) (((double) Noise.Generate((float) (((double) num5 + (double) num7) * ((double) num4 * 1.0)), (float) (((double) num6 + (double) num8) * ((double) num4 * 1.0))) + 1.0) / 2.0 * (flag1 ? 1.0 : 0.0));
          float num10 = (float) (((double) Noise.Generate(num5 + (float) (((double) num7 + 100.0) * ((double) num4 * 2.0)), (float) (((double) num6 + (double) num8 + 100.0) * ((double) num4 * 2.0))) + 1.0) / 2.0 * (flag2 ? 1.0 : 0.0));
          float num11 = (double) num9 >= 0.5 ? 1f : 0.0f;
          float num12 = (double) num10 >= 0.5 ? 1f : 0.0f;
          Color color4 = data[index2 + index1 * targ.width];
          float num13 = 1f;
          if ((double) num12 > 0.0)
            num13 = 0.9f;
          if (color4.r == (byte) 0)
            data[index2 + index1 * targ.width] = new Color(0, 0, 0, 0);
          else if (color4.r < (byte) 110)
          {
            if (flag4)
            {
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color3.r * 0.600000023841858), (byte) ((double) color3.g * 0.600000023841858), (byte) ((double) color3.b * 0.600000023841858));
            }
            else
            {
              float num14 = (double) num13 != 1.0 ? 1f : 0.9f;
              if ((double) num11 > 0.0)
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * 0.600000023841858 * (double) num14), (byte) ((double) color1.g * 0.600000023841858 * (double) num14), (byte) ((double) color1.b * 0.600000023841858 * (double) num14));
              else
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * 0.600000023841858 * (double) num14), (byte) ((double) color2.g * 0.600000023841858 * (double) num14), (byte) ((double) color2.b * 0.600000023841858 * (double) num14));
            }
          }
          else if (color4.r < (byte) 120)
          {
            if (flag4)
            {
              data[index2 + index1 * targ.width] = new Color(color3.r, color3.g, color3.b);
            }
            else
            {
              float num14 = (double) num13 != 1.0 ? 1f : 0.9f;
              if ((double) num11 > 0.0)
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * (double) num14), (byte) ((double) color1.g * (double) num14), (byte) ((double) color1.b * (double) num14));
              else
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * (double) num14), (byte) ((double) color2.g * (double) num14), (byte) ((double) color2.b * (double) num14));
            }
          }
          else if (color4.r < byte.MaxValue)
          {
            if ((double) num11 > 0.0)
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * 0.600000023841858 * (double) num13), (byte) ((double) color2.g * 0.600000023841858 * (double) num13), (byte) ((double) color2.b * 0.600000023841858 * (double) num13));
            else
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * 0.600000023841858 * (double) num13), (byte) ((double) color1.g * 0.600000023841858 * (double) num13), (byte) ((double) color1.b * 0.600000023841858 * (double) num13));
          }
          else if ((double) num11 > 0.0)
            data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * (double) num13), (byte) ((double) color2.g * (double) num13), (byte) ((double) color2.b * (double) num13));
          else
            data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * (double) num13), (byte) ((double) color1.g * (double) num13), (byte) ((double) color1.b * (double) num13));
        }
      }
      targ.SetData(data);
      DuckGame.Graphics.SetRenderTarget(targ);
      Profile._batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect) null, Matrix.Identity);
      Profile._batch.Draw(Profile._eggShine.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num3 * 16), 0.0f, 16f, 16f)), Color.White, 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
      Profile._batch.Draw(Profile._eggBorder.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num3 * 16), 0.0f, 16f, 16f)), Color.White, 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
      Profile._batch.End();
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      Rando.generator = generator;
    }

    public static Sprite GetPaintingSprite(int index = 0, ulong seed = 0)
    {
      if (seed == 0UL && Profiles.experienceProfile != null)
        seed = Profiles.experienceProfile.steamID;
      Sprite s = new Sprite()
      {
        renderTexture = new RenderTarget2D(19, 12, false, RenderTargetUsage.PreserveContents)
      };
      s.texture = (Tex2D) s.renderTexture;
      DuckGame.Graphics.AddRenderTask((Action) (() => Profile.GetPainting(index, s.renderTexture, seed)));
      return s;
    }

    public static void GetPainting(int index, RenderTarget2D targ, ulong seed)
    {
      if (Profile._easel == null)
      {
        Profile._batch = new MTSpriteBatch(DuckGame.Graphics.device);
        Profile._easel = new SpriteMap("online/easelWhite", 19, 12);
        Profile._eggShine = new SpriteMap("online/eggShine", 16, 16);
        Profile._eggBorder = new SpriteMap("online/eggBorder", 16, 16);
        Profile._eggOuter = new SpriteMap("online/eggOuter", 16, 16);
        Profile._easelSymbols = new SpriteMap("online/easelPic", 19, 12);
      }
      Random generator = Rando.generator;
      Rando.generator = Profile.GetLongGenerator(seed);
      for (int index1 = 0; index1 < index; ++index1)
        Rando.Int(100);
      bool flag1 = (double) Rando.Float(1f) > 0.0299999993294477;
      bool flag2 = (double) Rando.Float(1f) > 0.800000011920929;
      double num1 = (double) Rando.Float(1f);
      bool flag3 = Rando.Int(6) == 1;
      Profile._allowedColors = new List<Color>()
      {
        Colors.DGBlue,
        Colors.DGYellow,
        Colors.DGRed,
        Color.White,
        new Color(48, 224, 242),
        new Color(199, 234, 96)
      };
      Profile._allowedColors.Add(Colors.DGPink);
      Profile._allowedColors.Add(new Color((byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200))));
      if (Rando.Int(6) == 1)
      {
        Profile._allowedColors.Add(Colors.DGPurple);
        Profile._allowedColors.Add(Colors.DGEgg);
      }
      else if (Rando.Int(100) == 1)
      {
        Profile._allowedColors.Add(Colors.SuperDarkBlueGray);
        Profile._allowedColors.Add(Colors.BlueGray);
        Profile._allowedColors.Add(Colors.DGOrange);
        Profile._allowedColors.Add(new Color((byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200)), (byte) (54 + Rando.Int(200))));
      }
      else if (Rando.Int(1200) == 1)
        Profile._allowedColors.Add(Colors.Platinum);
      else if (Rando.Int(100000) == 1)
        Profile._allowedColors.Add(new Color(250, 10, 250));
      else if (Rando.Int(1000000) == 1)
        Profile._allowedColors.Add(new Color(229, 245, 181));
      DuckGame.Graphics.SetRenderTarget(targ);
      DuckGame.Graphics.Clear(Color.Black);
      Profile._batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, (MTEffect) null, Matrix.Identity);
      int num2 = 8 + Rando.Int(12);
      Rando.Int(100);
      Rando.Int(15);
      Rando.Int(300);
      MTSpriteBatch screen = DuckGame.Graphics.screen;
      DuckGame.Graphics.screen = Profile._batch;
      Profile._batch.Draw(Profile._easel.texture, new Vec2(0.0f, 0.0f), new Rectangle?(), Color.White, 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
      Profile._batch.Draw(Profile._easelSymbols.texture, new Vec2(0.0f, 0.0f), new Rectangle?(new Rectangle((float) (num2 * 19), 0.0f, 19f, 12f)), new Color(60, 60, 60, 200), 0.0f, new Vec2(0.0f, 0.0f), 1f, SpriteEffects.None, 0.9f);
      Profile._batch.End();
      DuckGame.Graphics.screen = screen;
      DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      Color[] data = targ.GetData();
      float num3 = 0.09999999f;
      Color color1 = Profile.PickColor();
      Color color2 = Profile.PickColor();
      Profile.PickColor();
      Color color3 = Profile.PickColor();
      float num4 = Rando.Float(100000f);
      float num5 = Rando.Float(100000f);
      for (int index1 = 0; index1 < targ.height; ++index1)
      {
        for (int index2 = 0; index2 < targ.width; ++index2)
        {
          float num6 = (float) (index2 + 32) * 0.75f;
          int num7 = index1 + 32;
          float num8 = (float) (((double) Noise.Generate((float) (((double) num4 + (double) num6) * ((double) num3 * 1.0)), (float) (((double) num5 + (double) num7) * ((double) num3 * 1.0))) + 1.0) / 2.0 * (flag1 ? 1.0 : 0.0));
          float num9 = (float) (((double) Noise.Generate(num4 + (float) (((double) num6 + 100.0) * ((double) num3 * 2.0)), (float) (((double) num5 + (double) num7 + 100.0) * ((double) num3 * 2.0))) + 1.0) / 2.0 * (flag2 ? 1.0 : 0.0));
          float num10 = (double) num8 >= 0.5 ? 1f : 0.0f;
          float num11 = (double) num9 >= 0.5 ? 1f : 0.0f;
          Color color4 = data[index2 + index1 * targ.width];
          float num12 = 1f;
          if ((double) num11 > 0.0)
            num12 = 0.9f;
          if (color4.r == (byte) 0)
            data[index2 + index1 * targ.width] = new Color(0, 0, 0, 0);
          else if (color4.r < (byte) 110)
          {
            if (flag3)
            {
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color3.r * 0.600000023841858), (byte) ((double) color3.g * 0.600000023841858), (byte) ((double) color3.b * 0.600000023841858));
            }
            else
            {
              float num13 = (double) num12 != 1.0 ? 1f : 0.9f;
              if ((double) num10 > 0.0)
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * 0.600000023841858 * (double) num13), (byte) ((double) color1.g * 0.600000023841858 * (double) num13), (byte) ((double) color1.b * 0.600000023841858 * (double) num13));
              else
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * 0.600000023841858 * (double) num13), (byte) ((double) color2.g * 0.600000023841858 * (double) num13), (byte) ((double) color2.b * 0.600000023841858 * (double) num13));
            }
          }
          else if (color4.r < (byte) 120)
          {
            if (flag3)
            {
              data[index2 + index1 * targ.width] = new Color(color3.r, color3.g, color3.b);
            }
            else
            {
              float num13 = (double) num12 != 1.0 ? 1f : 0.9f;
              if ((double) num10 > 0.0)
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * (double) num13), (byte) ((double) color1.g * (double) num13), (byte) ((double) color1.b * (double) num13));
              else
                data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * (double) num13), (byte) ((double) color2.g * (double) num13), (byte) ((double) color2.b * (double) num13));
            }
          }
          else if (color4.r < byte.MaxValue)
          {
            if ((double) num10 > 0.0)
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * 0.600000023841858 * (double) num12), (byte) ((double) color2.g * 0.600000023841858 * (double) num12), (byte) ((double) color2.b * 0.600000023841858 * (double) num12));
            else
              data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * 0.600000023841858 * (double) num12), (byte) ((double) color1.g * 0.600000023841858 * (double) num12), (byte) ((double) color1.b * 0.600000023841858 * (double) num12));
          }
          else if ((double) num10 > 0.0)
            data[index2 + index1 * targ.width] = new Color((byte) ((double) color2.r * (double) num12), (byte) ((double) color2.g * (double) num12), (byte) ((double) color2.b * (double) num12));
          else
            data[index2 + index1 * targ.width] = new Color((byte) ((double) color1.r * (double) num12), (byte) ((double) color1.g * (double) num12), (byte) ((double) color1.b * (double) num12));
        }
      }
      targ.SetData(data);
      Rando.generator = generator;
    }

    public List<string> unlocks
    {
      get => this._unlocks;
      set => this._unlocks = value;
    }

    public int ticketCount
    {
      get => this._ticketCount;
      set => this._ticketCount = value;
    }

    public int xp
    {
      get
      {
        if (Steam.user == null || this != Profiles.experienceProfile || (int) Steam.GetStat(nameof (xp)) != 0)
          return this._xp;
        Steam.SetStat(nameof (xp), this._xp);
        return this._xp;
      }
      set
      {
        if (Steam.user != null && this == Profiles.experienceProfile)
          Steam.SetStat(nameof (xp), value);
        this._xp = value;
      }
    }

    public static byte CalculateLocalFlippers()
    {
      bool flag1 = true;
      bool flag2 = true;
      foreach (ChallengeData challengeData in Challenges.challengesInArcade)
      {
        bool flag3 = false;
        bool flag4 = false;
        foreach (Profile universalProfile in Profiles.universalProfileList)
        {
          ChallengeSaveData saveData = Challenges.GetSaveData(challengeData.levelID, universalProfile, true);
          if (saveData != null && saveData.trophy > TrophyType.Baseline)
          {
            flag4 = true;
            if (saveData.trophy == TrophyType.Developer)
            {
              flag3 = true;
              break;
            }
          }
        }
        if (!flag3)
          flag1 = false;
        if (!flag4)
          flag2 = false;
        if (!flag2)
          break;
      }
      return (byte) ((int) (byte) ((uint) (byte) ((int) (byte) ((uint) (byte) ((int) (byte) ((uint) (byte) ((int) (byte) ((uint) (byte) (0 | (flag1 ? 1 : 0)) << 1) | ((int) Global.data.onlineWins >= 50 ? 1 : 0)) << 1) | ((int) Global.data.matchesPlayed >= 100 ? 1 : 0)) << 1) | (flag2 ? 1 : 0)) << 1) | (Options.Data.shennanigans ? 1 : 0));
    }

    public bool switchStatus => ((int) this.flippers & 1) != 0;

    public bool GetLightStatus(int index) => ((int) this.flippers >> index + 1 & 1) != 0;

    public float funslider
    {
      get => this._funSlider;
      set => this._funSlider = value;
    }

    public ProfileStats stats
    {
      get
      {
        if (!Profile.logStats)
        {
          if (this._junkStats == null)
          {
            XElement node = this._stats.Serialize();
            this._junkStats = new ProfileStats();
            this._junkStats.Deserialize(node);
          }
          return this._junkStats;
        }
        this._junkStats = (ProfileStats) null;
        return this._linkedProfile != null ? this._linkedProfile.stats : this._stats;
      }
      set
      {
        if (this._linkedProfile != null)
          this._linkedProfile.stats = value;
        this._stats = value;
      }
    }

    public ProfileStats prevStats
    {
      get => this._linkedProfile != null ? this._linkedProfile.prevStats : this._prevStats;
      set
      {
        if (this._linkedProfile != null)
          this._linkedProfile.prevStats = value;
        this._prevStats = value;
      }
    }

    public void RecordPreviousStats()
    {
      XElement node = this.stats.Serialize();
      this.prevStats = new ProfileStats();
      this.prevStats.Deserialize(node);
      this._endOfRoundStats = (ProfileStats) null;
    }

    public static int totalFansThisGame
    {
      get
      {
        int num = 0;
        foreach (Profile profile in Profiles.active)
          num += profile.stats.GetFans();
        return num;
      }
    }

    public ProfileStats endOfRoundStats
    {
      get
      {
        this._endOfRoundStats = (DataClass) this.stats - (DataClass) this.prevStats as ProfileStats;
        return this._endOfRoundStats;
      }
      set => this._endOfRoundStats = value;
    }

    public CurrentGame currentGame => this._currentGame;

    public ulong steamID
    {
      get
      {
        if (this.connection == DuckNetwork.localConnection)
          return DG.localID;
        return this.connection != null && this.connection.data is User ? (this.connection.data as User).id : this._steamID;
      }
      set => this._steamID = value;
    }

    public NetworkConnection connection
    {
      get => this._connection;
      set
      {
        this._connection = value;
        if (this._connection != null)
          return;
        this._networkStatus = DuckNetStatus.Disconnected;
      }
    }

    public bool acceptedMigration
    {
      get => this._acceptedMigration;
      set => this._acceptedMigration = value;
    }

    public DuckNetStatus networkStatus
    {
      get => this._networkStatus;
      set
      {
        if (value != this._networkStatus)
        {
          this._currentStatusTimeout = 1f;
          this._currentStatusTries = 0;
          if (value == DuckNetStatus.WaitingForLoadingToBeFinished)
            this._currentStatusTimeout = 5f;
        }
        this._networkStatus = value;
      }
    }

    public float currentStatusTimeout
    {
      get => this._currentStatusTimeout;
      set => this._currentStatusTimeout = value;
    }

    public int currentStatusTries
    {
      get => this._currentStatusTries;
      set => this._currentStatusTries = value;
    }

    public Profile linkedProfile
    {
      get => this._linkedProfile;
      set => this._linkedProfile = value;
    }

    public bool hasCustomHats
    {
      get => this.connection == DuckNetwork.localConnection && Teams.core.extraTeams.Count > 0 || this._hasCustomHats;
      set => this._hasCustomHats = value;
    }

    public bool isHost => this.connection == Network.host;

    public bool ready
    {
      get => this._ready;
      set => this._ready = value;
    }

    public byte networkIndex
    {
      get => this._networkIndex;
      set => this._networkIndex = value;
    }

    public bool localPlayer => !Network.isActive || this._connection == DuckNetwork.localConnection;

    public SlotType slotType
    {
      get => this._slotType;
      set
      {
        if (this._slotType == value)
          return;
        this._slotType = value;
        DuckNetwork.ChangeSlotSettings();
      }
    }

    public object reservedUser
    {
      get => this._reservedUser;
      set => this._reservedUser = value;
    }

    public Team reservedTeam
    {
      get => this._reservedTeam;
      set => this._reservedTeam = value;
    }

    public Duck duck
    {
      get => this._duck;
      set => this._duck = value;
    }

    public DuckPersona persona
    {
      get => this._persona;
      set => this._persona = value;
    }

    public InputProfile inputProfile
    {
      get => this._inputProfile;
      set
      {
        if (this._inputProfile != null && this._inputProfile != value)
          Input.ApplyDefaultMapping(this._inputProfile);
        if (value != null && value != this._inputProfile)
          Input.ApplyDefaultMapping(value, this);
        this._inputProfile = value;
      }
    }

    public Team team
    {
      get => this._team;
      set
      {
        if (value != null)
        {
          value.Join(this, false);
          this._team = value;
        }
        else
        {
          if (this._team == null)
            return;
          this._team.Leave(this, false);
          this._team = (Team) null;
        }
      }
    }

    public int wins
    {
      get => this._wins;
      set => this._wins = value;
    }

    public bool wasRockThrower
    {
      get => this._wasRockThrower;
      set => this._wasRockThrower = value;
    }

    public List<DeviceInputMapping> inputMappingOverrides => this._linkedProfile != null ? this._linkedProfile.inputMappingOverrides : this._inputMappingOverrides;

    public void ClearCurrentGame() => this._currentGame = new CurrentGame();

    public Profile(
      string varName,
      InputProfile varProfile = null,
      Team varStartTeam = null,
      DuckPersona varDefaultPersona = null,
      bool network = false,
      string varID = null)
    {
      this._name = varName;
      this._inputProfile = varProfile;
      varStartTeam?.Join(this);
      this._persona = varDefaultPersona;
      this._id = varID != null ? varID : Guid.NewGuid().ToString();
      this.isNetworkProfile = network;
    }
  }
}
