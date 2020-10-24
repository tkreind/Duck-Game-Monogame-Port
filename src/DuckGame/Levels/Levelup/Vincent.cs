﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Vincent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Vincent
  {
    public static Vincent context = new Vincent();
    public static List<VincentProduct> products = new List<VincentProduct>();
    public static float alpha = 0.0f;
    public static bool lookingAtList = false;
    public static bool lookingAtChallenge = false;
    public static bool hover = false;
    private static FancyBitmapFont _font;
    private static BitmapFont _priceFont;
    private static BitmapFont _priceFontCrossout;
    private static BitmapFont _priceFontRightways;
    private static BitmapFont _descriptionFont;
    private static SpriteMap _dealer;
    private static Sprite _tail;
    private static Sprite _photo;
    private static SpriteMap _newSticker;
    private static SpriteMap _rareSticker;
    private static Sprite _soldSprite;
    private static List<string> _lines = new List<string>();
    private static DealerMood _mood;
    private static string _currentLine = "";
    private static List<TextLine> _lineProgress = new List<TextLine>();
    private static float _waitLetter = 1f;
    private static float _waitAfterLine = 1f;
    private static float _talkMove = 0.0f;
    private static float _showLerp = 0.0f;
    private static bool _allowMovement = false;
    private static bool _hasYoYo = true;
    public static DayType type;
    public static bool show = false;
    public static bool hasKid = false;
    public static bool openedCorners = false;
    private static float _afterShowWait = 0.0f;
    private static bool _willGiveYoYo = false;
    private static float _listLerp = 0.0f;
    private static float _challengeLerp = 0.0f;
    private static float _chancyLerp = 0.0f;
    public static Sprite _furniFrame;
    public static Sprite _furniFill;
    public static Sprite _furniHov;
    public static SpriteMap _furniTag;
    public static Sprite _cheapTape;
    public static Sprite _bigBanner;
    public static Sprite _fancyBanner;
    private static List<RenderTarget2D> _priceTargets = new List<RenderTarget2D>();
    private static int _soldSelectIndex = -1;
    private static bool killSkip = false;
    private static float _extraWait = 0.0f;
    public static int _giveTickets = 0;
    public static bool afterChallenge = false;
    public static float afterChallengeWait = 0.0f;
    private static List<ChallengeData> _chancyChallenges = new List<ChallengeData>();
    public static bool showingDay = false;
    private static string lastWord = "";
    private static int wait = 0;
    public static int _selectIndex = -1;
    public static int _selectDescIndex = -2;

    public static int frame
    {
      get
      {
        if (Vincent._mood == DealerMood.Concerned)
          return Vincent._dealer.frame - 6;
        return Vincent._mood == DealerMood.Point ? Vincent._dealer.frame - 3 : Vincent._dealer.frame;
      }
      set
      {
        if (Vincent._mood == DealerMood.Concerned)
          Vincent._dealer.frame = value + 6;
        else if (Vincent._mood == DealerMood.Point)
          Vincent._dealer.frame = value + 3;
        else
          Vincent._dealer.frame = value;
      }
    }

    public static void Clear()
    {
      Vincent._lines.Clear();
      Vincent._waitLetter = 0.0f;
      Vincent._waitAfterLine = 0.0f;
      Vincent._currentLine = "";
      Vincent._mood = DealerMood.Normal;
    }

    public static void Add(string line) => Vincent._lines.Add(line);

    public static void Open(DayType t)
    {
      Vincent._lineProgress.Clear();
      Vincent.show = false;
      Vincent._afterShowWait = 0.0f;
      Vincent._showLerp = 0.0f;
      Vincent._allowMovement = false;
      Vincent._waitAfterLine = 1f;
      Vincent._waitLetter = 1f;
      Vincent._mood = DealerMood.Normal;
      Vincent._chancyLerp = 0.0f;
      Vincent.hasKid = false;
      Vincent._allowMovement = false;
      Vincent._afterShowWait = 0.0f;
      Vincent.show = false;
      if (Profiles.experienceProfile == null)
        return;
      Vincent.openedCorners = false;
      Vincent._dealer = Vincent._hasYoYo ? new SpriteMap("vincent", 113, 106) : new SpriteMap("vincentNoYo", 113, 106);
      Vincent._selectDescIndex = -2;
      Vincent._selectIndex = -1;
      Vincent.hasKid = false;
      Vincent.type = t;
      switch (t)
      {
        case DayType.Special:
          Vincent.Add("|CALM|I'VE GOT SOMETHING SPECIAL FOR YOU TODAY, |SHOW||POINT|CHECK IT OUT!");
          ++Profiles.experienceProfile.timesMetVincentSell;
          break;
        case DayType.SaleDay:
          if (Profiles.experienceProfile.timesMetVincentSale == 0)
          {
            Vincent.Add("|CALM|HEY!|2| |POINT|YOU'RE NEW TO THIS SO LET ME EXPLAIN.");
            Vincent.Add("|CALM|AT THE END OF EVERY MONTH I HAVE A SUPER SALE.");
            Vincent.Add("|CONCERNED|WHERE I SELL STUFF I CANT MOVE, |POINT|AT WAY LOW PRICES!");
            Vincent.Add("|CALM||SHOW|CHECK IT OUT!");
          }
          else
          {
            List<List<string>> stringListList = new List<List<string>>()
            {
              new List<string>()
              {
                "|CONCERNED|HEY, GUESS WHAT? |POINT||SHOW|IT'S SALE DAY!"
              },
              new List<string>()
              {
                "|CALM|DANG, I HOPE YOU'RE READY |SHOW|FOR |POINT|INSANE SAVINGS."
              },
              new List<string>() { "|CALM|+SAAALE |SHOW|DAAAAY+!" }
            };
            foreach (string line in stringListList[Rando.Int(stringListList.Count - 1)])
              Vincent.Add(line);
          }
          ++Profiles.experienceProfile.timesMetVincentSale;
          break;
        case DayType.ImportDay:
          if (Profiles.experienceProfile.timesMetVincentImport == 0)
          {
            Vincent.Add("|CALM|OK. ON THE FIRST OF EVERY MONTH I HAVE A |POINT||GREEN|SPECIAL SALE|WHITE|!");
            Vincent.Add("|CALM|TODAY I SELL ONLY THE |POINT||GREEN|FANCIEST IMPORTS|GREEN|.");
            Vincent.Add("|CONCERNED|THIS STUFF AINT CHEAP!|POINT||SHOW| SEE ANYTHING YOU LIKE?");
          }
          else
          {
            List<List<string>> stringListList = new List<List<string>>()
            {
              new List<string>() { "|POINT||SHOW|IT'S FANCY IMPORT DAY!" },
              new List<string>()
              {
                "|CALM|HOPE YOU'RE READY FOR SOME |SHOW|EXOTIC FURNITURE."
              }
            };
            foreach (string line in stringListList[Rando.Int(stringListList.Count - 1)])
              Vincent.Add(line);
          }
          ++Profiles.experienceProfile.timesMetVincentImport;
          break;
        case DayType.PawnDay:
          if (Profiles.experienceProfile.timesMetVincentSell == 0)
          {
            Vincent.Add("|CALM|I KEEP AN EYE OUT ON ALL THE FURNITURE THAT GOES AROUND HERE.");
            Vincent.Add("|CALM|EVERY SECOND WEDNESDAY I'LL COME SEE |POINT|IF YOU HAVE ANYTHING I LIKE!");
            Vincent.Add("|CONCERNED|IF I LIKE SOMETHING I'LL TRY TO BUY IT FROM YOU.|SHOW|");
          }
          IEnumerable<Furniture> source1 = Profiles.experienceProfile.GetAvailableFurnis().Where<Furniture>((Func<Furniture, bool>) (x => Profiles.experienceProfile.GetNumFurnitures((int) x.index) > Profiles.experienceProfile.GetNumFurnituresPlaced((int) x.index) && x.group != Furniture.Momento && x.group != Furniture.Default));
          IOrderedEnumerable<Furniture> source2 = source1.OrderBy<Furniture, float>((Func<Furniture, float>) (x => Rando.Float(1f) - (float) Profiles.experienceProfile.GetNumFurnitures((int) x.index) * 1f));
          if (source1.Count<Furniture>() > 0)
          {
            Vincent.Add("|CONCERNED|I LIKE THE LOOK OF THIS, CAN I BUY IT FROM YOU?|SHOW|");
            Vincent.products.Clear();
            VincentProduct vincentProduct = new VincentProduct()
            {
              type = VPType.Furniture,
              furnitureData = source2.First<Furniture>()
            };
            vincentProduct.originalCost = vincentProduct.furnitureData.price;
            vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 2.0);
            Vincent.products.Add(vincentProduct);
          }
          else
          {
            Vincent.Add("|CALM|LET'S SEE WHAT I COULD BUY FROM YOU...");
            Vincent.Add("|CONCERNED|LOOKS LIKE YOU DON'T HAVE ANYTHING I WANT, SORRY!|SHOW|");
          }
          ++Profiles.experienceProfile.timesMetVincentSell;
          break;
        default:
          if (Profiles.experienceProfile.timesMetVincent >= 19 && Profiles.experienceProfile.GetNumFurnitures((int) RoomEditor.GetFurniture("YOYO").index) <= 0)
          {
            Vincent._willGiveYoYo = true;
            Vincent.Add("|CONCERNED|YOU KNOW WHAT? |CALM|YOU'RE MY BEST CUSTOMER.");
            Vincent.Add("|POINT|HANDS DOWN. YOU ARE AWESOME.");
            Vincent.Add("|CONCERNED|I WANT YOU TO HAVE THIS...|2||GIVE|");
            Vincent.Add("|POINT|NOW THEN, |SHOW|HERE'S WHAT I GOT!");
          }
          else if (Profiles.experienceProfile.timesMetVincent == 0)
          {
            Vincent.Add("|POINT|HEY, I'M VINCENT AND I SELL TOYS.");
            Vincent.Add("|CALM|I'M AROUND EVERY |GREEN|FRIDAY|WHITE| AND HAVE NEW STUFF EVERY WEEK.");
            Vincent.Add("|CONCERNED|MY STUFF IS A SURE THING TOO. NO GAMBLING HERE.|2|");
            Vincent.Add("|POINT|SO! |SHOW|SEE ANYTHING YOU LIKE?");
          }
          else if (Profiles.experienceProfile.timesMetVincent == 1)
          {
            Vincent.Add("|POINT|HEY!");
            Vincent.Add("|CALM|THIS IS MY SON, MINI VINNY.");
            Vincent.Add("|CALM|HE'S HELPIN' ME SELL TOYS.");
            Vincent.Add("|POINT|HIS MOM|3||CONCERNED| WOULD|1| RATHER HE |RED|DIDN'T|WHITE|.|3|");
            Vincent.Add("|CONCERNED|BUT|2||CALM| THIS IS OUR TIME. |SHOW|AND OL' MINI |GREEN|LOVES|WHITE| SELLIN' TOYS.");
            Vincent.hasKid = true;
          }
          else if (Profiles.experienceProfile.timesMetVincent == 7)
          {
            Vincent.Add("|CONCERNED|HERE|SHOW| YOU GO...");
            Vincent.hasKid = false;
          }
          else if (Profiles.experienceProfile.timesMetVincent == 8)
          {
            Vincent.Add("|POINT|DANG, IT'S GOOD TO SEE YOU.");
            Vincent.Add("|CONCERNED|YOU'RE MY BEST CUSTOMER, |SHOW|YOU KNOW THAT?");
            Vincent.hasKid = false;
          }
          else if (Profiles.experienceProfile.timesMetVincent == 13)
          {
            Vincent.Add("|CALM|FRIDAY IS MY FAVOURITE DAY CAUSE WE GET TO HANG OUT.");
            Vincent.Add("|POINT|I'M NOT JUST SAYIN' THAT|SHOW| CAUSE YOU BUY STUFF!");
            Vincent.hasKid = false;
          }
          else if (Profiles.experienceProfile.timesMetVincent == 22)
          {
            Vincent.Add("|CONCERNED|TODAY WAS A DANG MESS, BUT THIS MAKES IT WORTHWHILE.");
            Vincent.Add("|POINT|+SELLIN' |SHOW|TOYS IS THE BEST+");
            Vincent.hasKid = false;
          }
          else
          {
            Vincent.hasKid = Profiles.experienceProfile.timesMetVincent % 2 == 1;
            List<List<string>> stringListList = new List<List<string>>()
            {
              new List<string>()
              {
                "|CONCERNED|I HOPE YOU'RE READY,|2| CAUSE TODAY I'VE GOT...|0|",
                "|POINT|THE |SHOW||GREEN|GREATEST PRODUCT LINEUP IN VINCENT HISTORY|WHITE|."
              },
              new List<string>()
              {
                "|CALM|JUST GOT BACK FROM THE DUMP.",
                "|POINT|I HOPE YOU LIKE |GREEN|VALUE|WHITE|.|SHOW|"
              },
              new List<string>()
              {
                "|CONCERNED|WHERE DO I EVEN FIND THIS STUFF?",
                "|CALM|YOU JUST CAN'T FIND STUFF LIKE THIS|2| |SHOW||POINT|ANY. WHERE. ELSE."
              },
              new List<string>()
              {
                "|CALM|LOOK AT ALL THIS STUFF, |CONCERENED|I DON'T EVEN WANT TO SELL IT.",
                "|POINT||SHOW|I WANNA KEEP THIS STUFF, IT'S JUST TOO COOL."
              },
              new List<string>()
              {
                "|CALM|LOOK AT THAT. |GREEN|4 GOOD REASONS|WHITE||2| |SHOW|WHY TODAY IS A GOOD DAY."
              },
              new List<string>()
              {
                "|POINT|I JUST FINISHED PUTTING PRICES ON EVERYTHING.",
                "|CONCERNED|OOPS, LOOKS LIKE I ACCIDENTALLY|SHOW| MADE THIS STUFF WAY TOO CHEAP..."
              }
            };
            if (Profiles.experienceProfile.timesMetVincent > 2 && Rando.Int(100) == 0)
              stringListList.Add(new List<string>()
              {
                "|CALM|+GONNA BUY MYSELF |SHOW|A GREY GUITAR+"
              });
            if ((Profiles.experienceProfile.timesMetVincent > 30 && Profiles.experienceProfile.timesMetVincent < 45 || Profiles.experienceProfile.timesMetVincent > 100 && Profiles.experienceProfile.timesMetVincent < 125) && Rando.Int(10000) == 0)
              stringListList.Add(new List<string>()
              {
                "|CALM|+I'M GOIN TO CALIFORNIA+ |SHOW|+GONNA LIVE THE LIFE+"
              });
            if ((Profiles.experienceProfile.timesMetVincent > 30 && Profiles.experienceProfile.timesMetVincent < 45 || Profiles.experienceProfile.timesMetVincent > 100 && Profiles.experienceProfile.timesMetVincent < 125) && Rando.Int(10000) == 0)
              stringListList.Add(new List<string>()
              {
                "|CALM|+BACKSTREET'S BACK+ |SHOW|+ALRIGHT!!+"
              });
            foreach (string line in stringListList[Rando.Int(stringListList.Count - 1)])
              Vincent.Add(line);
          }
          ++Profiles.experienceProfile.timesMetVincent;
          break;
      }
      if (t == DayType.PawnDay)
        return;
      Vincent.GenerateProducts();
    }

    public static void GenerateProducts()
    {
      Vincent.products.Clear();
      switch (Vincent.type)
      {
        case DayType.Shop:
          using (List<Furniture>.Enumerator enumerator = UIGachaBox.GetRandomFurniture(Rarity.VeryRare, 4, 0.4f, numDupes: 1).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Furniture current = enumerator.Current;
              VincentProduct vincentProduct = new VincentProduct();
              vincentProduct.type = VPType.Furniture;
              vincentProduct.furnitureData = current;
              if (Rando.Int(120) == 0)
              {
                vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 0.5);
                vincentProduct.originalCost = vincentProduct.furnitureData.price;
              }
              else
                vincentProduct.cost = vincentProduct.originalCost = vincentProduct.furnitureData.price;
              Vincent.products.Add(vincentProduct);
            }
            break;
          }
        case DayType.Special:
          IEnumerable<Team> source = new List<Team>()
          {
            Teams.GetTeam("CYCLOPS"),
            Teams.GetTeam("BIG ROBO"),
            Teams.GetTeam("TINCAN"),
            Teams.GetTeam("WELDERS"),
            Teams.GetTeam("PONYCAP"),
            Teams.GetTeam("TRICORNE"),
            Teams.GetTeam("TWINTAIL")
          }.Where<Team>((Func<Team, bool>) (x => !Global.data.boughtHats.Contains(x.name)));
          if (source.Count<Team>() <= 0)
          {
            using (List<Furniture>.Enumerator enumerator = UIGachaBox.GetRandomFurniture(Rarity.VeryVeryRare, 1, 0.4f).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Furniture current = enumerator.Current;
                VincentProduct vincentProduct = new VincentProduct()
                {
                  type = VPType.Furniture,
                  furnitureData = current
                };
                vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 0.5);
                vincentProduct.originalCost = vincentProduct.furnitureData.price;
                Vincent.products.Add(vincentProduct);
              }
              break;
            }
          }
          else
          {
            VincentProduct vincentProduct = new VincentProduct()
            {
              type = VPType.Hat
            };
            vincentProduct.cost = vincentProduct.originalCost = 150;
            vincentProduct.teamData = source.ElementAt<Team>(Rando.Int(source.Count<Team>() - 1));
            Vincent.products.Add(vincentProduct);
            break;
          }
        case DayType.SaleDay:
          using (List<Furniture>.Enumerator enumerator = UIGachaBox.GetRandomFurniture(Rarity.Common, 4, 2f).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Furniture current = enumerator.Current;
              VincentProduct vincentProduct = new VincentProduct()
              {
                type = VPType.Furniture,
                furnitureData = current
              };
              vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 0.5);
              vincentProduct.originalCost = vincentProduct.furnitureData.price;
              Vincent.products.Add(vincentProduct);
            }
            break;
          }
        case DayType.ImportDay:
          IOrderedEnumerable<Furniture> orderedEnumerable = UIGachaBox.GetRandomFurniture(Rarity.VeryVeryRare, 8, 0.4f).OrderBy<Furniture, int>((Func<Furniture, int>) (x => -x.rarity));
          int num = 0;
          using (IEnumerator<Furniture> enumerator = orderedEnumerable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Furniture current = enumerator.Current;
              VincentProduct vincentProduct = new VincentProduct();
              vincentProduct.type = VPType.Furniture;
              vincentProduct.furnitureData = current;
              if (Rando.Int(100) == 0)
              {
                vincentProduct.furnitureData = UIGachaBox.GetRandomFurniture(Rarity.SuperRare, 1, 0.3f)[0];
                vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 2.0);
                vincentProduct.originalCost = vincentProduct.furnitureData.price;
              }
              else
              {
                vincentProduct.cost = (int) ((double) vincentProduct.furnitureData.price * 1.5);
                vincentProduct.originalCost = vincentProduct.furnitureData.price;
              }
              Vincent.products.Add(vincentProduct);
              ++num;
              if (num == 4)
                break;
            }
            break;
          }
      }
    }

    public static void Initialize()
    {
      if (Vincent._tail != null)
        return;
      Vincent._dealer = new SpriteMap("vincent", 113, 106);
      Vincent._tail = new Sprite("arcade/bubbleTail");
      Vincent._font = new FancyBitmapFont("smallFont");
      Vincent._priceFont = new BitmapFont("biosFontSideways", 8);
      Vincent._priceFontCrossout = new BitmapFont("biosFontSidewaysCrossout", 8);
      Vincent._priceFontRightways = new BitmapFont("priceFontRightways", 8);
      Vincent._descriptionFont = new BitmapFont("biosFontDescriptions", 8);
      Vincent._photo = new Sprite("arcade/challengePhoto");
      Vincent._furniFrame = new Sprite("furniFrame");
      Vincent._furniFrame.CenterOrigin();
      Vincent._furniFill = new Sprite("furniFill");
      Vincent._furniFill.CenterOrigin();
      Vincent._furniHov = new Sprite("furniHov");
      Vincent._furniHov.CenterOrigin();
      Vincent._soldSprite = new Sprite("soldStamp");
      Vincent._soldSprite.CenterOrigin();
      Vincent._newSticker = new SpriteMap("newSticker", 29, 28);
      Vincent._newSticker.CenterOrigin();
      Vincent._rareSticker = new SpriteMap("rareSticker", 29, 28);
      Vincent._rareSticker.CenterOrigin();
      Vincent._furniTag = new SpriteMap("furniTag", 14, 51);
      Vincent._cheapTape = new Sprite("cheapTape");
      Vincent._cheapTape.CenterOrigin();
      Vincent._bigBanner = new Sprite("bigBanner");
      Vincent._fancyBanner = new Sprite("fancyBanner");
      Vincent._priceTargets.Add(new RenderTarget2D(64, 16));
      Vincent._priceTargets.Add(new RenderTarget2D(64, 16));
      Vincent._priceTargets.Add(new RenderTarget2D(64, 16));
      Vincent._priceTargets.Add(new RenderTarget2D(64, 16));
    }

    public static void ChangeSpeech()
    {
      Vincent.Clear();
      Vincent._selectDescIndex = Vincent._selectIndex;
      if (Vincent.products[Vincent._selectIndex].sold)
      {
        if (Vincent._soldSelectIndex == Vincent._selectIndex)
        {
          if (Vincent.products.Where<VincentProduct>((Func<VincentProduct, bool>) (x => !x.sold)).Count<VincentProduct>() == 0)
          {
            Vincent.Add("|CONCERNED|WOAH...");
            Vincent.Add("|POINT|YOU BOUGHT EVERYTHING!");
          }
          else
          {
            List<string> stringList = new List<string>()
            {
              "|CONCERNED|YEP, AND THERE YOU GO...|0|",
              "|CONCERNED|FINALLY GOT RID OF IT!",
              "|CALM|SOLD!"
            };
            if (Profiles.experienceProfile.timesMetVincent > 10)
            {
              stringList.Add("|POINT|AND NOW ITS YOURS!");
              stringList.Add("|CONCERNED|YOU LIKE THOSE HUH?");
            }
            if (Profiles.experienceProfile.timesMetVincent > 30)
            {
              stringList.Add("|POINT|TAKE GOOD CARE OF IT!");
              stringList.Add("|CONCERNED|HOPE YOU LIKE IT.");
            }
            Vincent.Add(stringList[Rando.Int(stringList.Count - 1)]);
          }
          Vincent._soldSelectIndex = -1;
        }
        else
        {
          Vincent.Add("|CONCERNED|GONNA MISS THAT ONE.");
          Vincent._soldSelectIndex = -1;
        }
      }
      else
      {
        string str = Vincent.products[Vincent._selectIndex].description;
        if (str == "")
          str = Vincent.products[Vincent._selectIndex].furnitureData != null ? "What a fine piece of furniture." : "What a fine hat.";
        int type = (int) Vincent.products[Vincent._selectIndex].type;
        Vincent.Add(str + "^|ORANGE|Part of the '" + Vincent.products[Vincent._selectIndex].group + "' line.|WHITE| " + "|YELLOW|$" + Convert.ToString(Vincent.products[Vincent._selectIndex].cost));
      }
    }

    public static void Sold()
    {
      Vincent.products[FurniShopScreen.attemptBuyIndex].sold = true;
      Vincent._soldSelectIndex = FurniShopScreen.attemptBuyIndex;
    }

    public static void Update()
    {
      if (Vincent._hasYoYo && Profiles.experienceProfile.timesMetVincent > 19 && (!FurniShopScreen.giveYoYo && !Vincent._willGiveYoYo))
      {
        Vincent._dealer = new SpriteMap("vincentNoYo", 113, 106);
        Vincent._hasYoYo = false;
      }
      else if (!Vincent._hasYoYo && Profiles.experienceProfile.timesMetVincent < 20)
      {
        Vincent._dealer = new SpriteMap("vincent", 113, 106);
        Vincent._hasYoYo = true;
      }
      Vincent.Initialize();
      if (UILevelBox.menuOpen)
        return;
      Vincent._showLerp = Lerp.FloatSmooth(Vincent._showLerp, Vincent.show ? 1f : 0.0f, 0.09f, 1.05f);
      bool flag1 = Vincent.lookingAtList && (double) Vincent._challengeLerp < 0.300000011920929;
      bool flag2 = Vincent.lookingAtChallenge && (double) Vincent._listLerp < 0.300000011920929;
      bool flag3 = FurniShopScreen.open && (double) Vincent._listLerp < 0.300000011920929;
      Vincent._listLerp = Lerp.FloatSmooth(Vincent._listLerp, flag1 ? 1f : 0.0f, 0.2f, 1.05f);
      Vincent._challengeLerp = Lerp.FloatSmooth(Vincent._challengeLerp, flag2 ? 1f : 0.0f, 0.2f, 1.05f);
      Vincent._chancyLerp = Lerp.FloatSmooth(Vincent._chancyLerp, flag3 ? 1f : 0.0f, 0.2f, 1.05f);
      Vincent.alpha = !FurniShopScreen.open ? Lerp.Float(Vincent.alpha, 0.0f, 0.05f) : Lerp.Float(Vincent.alpha, 1f, 0.05f);
      if (!FurniShopScreen.open || Vincent.showingDay)
        return;
      if (!Vincent.openedCorners)
      {
        Vincent.openedCorners = true;
        HUD.ClearCorners();
        HUD.AddCornerMessage(HUDCorner.BottomRight, "@QUACK@DITCH");
      }
      if (Vincent._allowMovement)
      {
        if ((Input.Pressed("ANY") || Vincent.type == DayType.PawnDay && Vincent.products.Count > 0) && Vincent._selectIndex == -1)
        {
          Vincent._selectIndex = 0;
          if (Vincent.products.Count == 0)
          {
            FurniShopScreen.close = true;
            return;
          }
        }
        if (Input.Pressed("LEFT"))
        {
          switch (Vincent._selectIndex)
          {
            case -1:
              Vincent._selectIndex = 0;
              break;
            case 1:
              Vincent._selectIndex = 0;
              SFX.Play("textLetter", 0.7f);
              break;
            case 3:
              Vincent._selectIndex = 2;
              SFX.Play("textLetter", 0.7f);
              break;
          }
        }
        if (Input.Pressed("RIGHT"))
        {
          switch (Vincent._selectIndex)
          {
            case -1:
              Vincent._selectIndex = 0;
              break;
            case 0:
              Vincent._selectIndex = 1;
              SFX.Play("textLetter", 0.7f);
              break;
            case 2:
              Vincent._selectIndex = 3;
              SFX.Play("textLetter", 0.7f);
              break;
          }
        }
        if (Input.Pressed("UP"))
        {
          switch (Vincent._selectIndex)
          {
            case -1:
              Vincent._selectIndex = 0;
              break;
            case 2:
              Vincent._selectIndex = 0;
              SFX.Play("textLetter", 0.7f);
              break;
            case 3:
              Vincent._selectIndex = 1;
              SFX.Play("textLetter", 0.7f);
              break;
          }
        }
        if (Input.Pressed("DOWN"))
        {
          switch (Vincent._selectIndex)
          {
            case -1:
              Vincent._selectIndex = 0;
              break;
            case 0:
              Vincent._selectIndex = 2;
              SFX.Play("textLetter", 0.7f);
              break;
            case 1:
              Vincent._selectIndex = 3;
              SFX.Play("textLetter", 0.7f);
              break;
          }
        }
        if (Vincent._selectIndex >= Vincent.products.Count)
          Vincent._selectIndex = Vincent.products.Count - 1;
        if (Input.Pressed("SHOOT") && Vincent._selectIndex != -1 && !Vincent.products[Vincent._selectIndex].sold && (Vincent.type == DayType.PawnDay || Vincent.products[Vincent._selectIndex].cost <= Profiles.experienceProfile.littleManBucks))
        {
          FurniShopScreen.attemptBuy = Vincent.products[Vincent._selectIndex];
          Vincent._selectDescIndex = -1;
          FurniShopScreen.attemptBuyIndex = Vincent._selectIndex;
          HUD.CloseAllCorners();
          HUD.AddCornerMessage(HUDCorner.BottomLeft, "|YELLOW|$" + Profiles.experienceProfile.littleManBucks.ToString());
          if (Vincent.products[Vincent._selectIndex].furnitureData == null)
            return;
          HUD.AddCornerMessage(HUDCorner.BottomRight, "|WHITE|" + (object) Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[Vincent._selectIndex].furnitureData.index) + " OWNED");
          return;
        }
        if (Vincent._selectDescIndex != Vincent._selectIndex)
        {
          if (Vincent._selectIndex == -1)
          {
            HUD.CloseAllCorners();
            HUD.AddCornerMessage(HUDCorner.BottomRight, "PRESS BUTTON");
            Vincent._selectDescIndex = Vincent._selectIndex;
          }
          else
          {
            if (Vincent.type != DayType.PawnDay)
              Vincent.ChangeSpeech();
            HUD.CloseAllCorners();
            HUD.AddCornerMessage(HUDCorner.BottomLeft, "|YELLOW|$" + Profiles.experienceProfile.littleManBucks.ToString());
            int num = Vincent.products[Vincent._selectIndex].furnitureData != null ? Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[Vincent._selectIndex].furnitureData.index) : 0;
            if (Vincent.type == DayType.PawnDay)
            {
              if (Vincent.products[Vincent._selectIndex].sold)
                HUD.AddCornerMessage(HUDCorner.BottomRight, "SOLD");
              else
                HUD.AddCornerMessage(HUDCorner.BottomRight, "@SHOOT@SELL" + (num > 0 ? "|WHITE| (" + (object) num + " OWNED)" : ""));
            }
            else if (Vincent.products[Vincent._selectIndex].sold)
              HUD.AddCornerMessage(HUDCorner.BottomRight, "BOUGHT");
            else
              HUD.AddCornerMessage(HUDCorner.BottomRight, (Vincent.products[Vincent._selectIndex].cost > Profiles.experienceProfile.littleManBucks ? "@SHOOT@|RED|BUY" : "@SHOOT@BUY") + (num > 0 ? "|WHITE| (" + (object) num + " OWNED)" : ""));
            HUD.AddCornerMessage(HUDCorner.TopRight, "@QUACK@EXIT");
            Vincent._selectDescIndex = Vincent._selectIndex;
          }
        }
      }
      if (Vincent._lines.Count > 0 && Vincent._currentLine == "")
      {
        Vincent._waitAfterLine -= 0.045f;
        if (Vincent.killSkip)
          Vincent._waitAfterLine -= 0.1f;
        Vincent._talkMove += 0.75f;
        if ((double) Vincent._talkMove > 1.0)
        {
          Vincent.frame = 0;
          Vincent._talkMove = 0.0f;
        }
        if ((double) Vincent._waitAfterLine <= 0.0)
        {
          Vincent._lineProgress.Clear();
          Vincent._currentLine = Vincent._lines[0];
          Vincent._lines.RemoveAt(0);
          Vincent._waitAfterLine = 1.3f;
          if (Vincent.show)
            Vincent._allowMovement = true;
          Vincent.killSkip = false;
        }
      }
      if (Vincent._currentLine != "")
      {
        Vincent._waitLetter -= 0.8f;
        if ((double) Vincent._waitLetter < 0.0)
        {
          Vincent._talkMove += 0.75f;
          if ((double) Vincent._talkMove > 1.0)
          {
            Vincent.frame = Vincent._currentLine[0] == ' ' || Vincent.frame != 1 || (double) Vincent._extraWait > 0.0 ? 1 : 2;
            Vincent._talkMove = 0.0f;
          }
          Vincent._waitLetter = 1f;
          while (Vincent._currentLine[0] == '@')
          {
            string str = string.Concat((object) Vincent._currentLine[0]);
            for (Vincent._currentLine = Vincent._currentLine.Remove(0, 1); Vincent._currentLine[0] != '@' && Vincent._currentLine.Length > 0; Vincent._currentLine = Vincent._currentLine.Remove(0, 1))
              str += (string) (object) Vincent._currentLine[0];
            Vincent._currentLine = Vincent._currentLine.Remove(0, 1);
            string val = str + "@";
            Vincent._lineProgress[0].Add(val);
            Vincent._waitLetter = 3f;
            if (Vincent._currentLine.Length == 0)
            {
              Vincent._currentLine = "";
              return;
            }
          }
          float num1 = 0.0f;
          while (Vincent._currentLine[0] == '|')
          {
            Vincent._currentLine = Vincent._currentLine.Remove(0, 1);
            string str = "";
            for (; Vincent._currentLine[0] != '|' && Vincent._currentLine.Length > 0; Vincent._currentLine = Vincent._currentLine.Remove(0, 1))
              str += (string) (object) Vincent._currentLine[0];
            bool flag4 = false;
            if (Vincent._currentLine.Length <= 1)
            {
              Vincent._currentLine = "";
              flag4 = true;
            }
            else
              Vincent._currentLine = Vincent._currentLine.Remove(0, 1);
            Color c = Color.White;
            bool flag5 = false;
            if (str == "RED")
            {
              flag5 = true;
              c = Color.Red;
            }
            else if (str == "WHITE")
            {
              flag5 = true;
              c = Color.White;
            }
            else if (str == "BLUE")
            {
              flag5 = true;
              c = Color.Blue;
            }
            else if (str == "ORANGE")
            {
              flag5 = true;
              c = new Color(235, 137, 51);
            }
            else if (str == "YELLOW")
            {
              flag5 = true;
              c = new Color(247, 224, 90);
            }
            else if (str == "GREEN")
            {
              flag5 = true;
              c = Color.LimeGreen;
            }
            else if (str == "CONCERNED")
            {
              Vincent._mood = DealerMood.Concerned;
              num1 = 2f;
            }
            else if (str == "POINT")
            {
              Vincent._mood = DealerMood.Point;
              num1 = 2f;
            }
            else if (str == "CALM")
            {
              Vincent._mood = DealerMood.Normal;
              num1 = 2f;
            }
            else if (str == "SHOW")
              Vincent.show = true;
            else if (str == "GIVE")
            {
              Vincent._willGiveYoYo = false;
              FurniShopScreen.giveYoYo = true;
            }
            else if (str == "0")
              Vincent.killSkip = true;
            else if (str == "1")
              num1 = 5f;
            else if (str == "2")
              num1 = 10f;
            else if (str == "3")
              num1 = 15f;
            if (flag5)
            {
              if (Vincent._lineProgress.Count == 0)
                Vincent._lineProgress.Insert(0, new TextLine()
                {
                  lineColor = c
                });
              else
                Vincent._lineProgress[0].SwitchColor(c);
            }
            if (flag4)
              return;
          }
          string str1 = "";
          int index1 = 1;
          if (Vincent._currentLine[0] == ' ')
          {
            while (index1 < Vincent._currentLine.Length && Vincent._currentLine[index1] != ' ' && Vincent._currentLine[index1] != '^')
            {
              if (Vincent._currentLine[index1] == '|')
              {
                int index2 = index1 + 1;
                while (index2 < Vincent._currentLine.Length && Vincent._currentLine[index2] != '|')
                  ++index2;
                index1 = index2 + 1;
              }
              else if (Vincent._currentLine[index1] == '@')
              {
                int index2 = index1 + 1;
                while (index2 < Vincent._currentLine.Length && Vincent._currentLine[index2] != '@')
                  ++index2;
                index1 = index2 + 1;
              }
              else
              {
                str1 += (string) (object) Vincent._currentLine[index1];
                ++index1;
              }
            }
          }
          if (Vincent._lineProgress.Count == 0 || Vincent._currentLine[0] == '^' || Vincent._currentLine[0] == ' ' && Vincent._lineProgress[0].Length() + str1.Length > 34)
          {
            Color color = Color.White;
            if (Vincent._lineProgress.Count > 0)
              color = Vincent._lineProgress[0].lineColor;
            Vincent._lineProgress.Insert(0, new TextLine()
            {
              lineColor = color
            });
            if (Vincent._currentLine[0] == ' ' || Vincent._currentLine[0] == '^')
              Vincent._currentLine = Vincent._currentLine.Remove(0, 1);
          }
          else
          {
            if (Vincent._currentLine[0] == '!' || Vincent._currentLine[0] == '?' || Vincent._currentLine[0] == '.')
              Vincent._waitLetter = 5f;
            else if (Vincent._currentLine[0] == ',')
              Vincent._waitLetter = 3f;
            Vincent._lineProgress[0].Add(Vincent._currentLine[0]);
            char ch = Vincent._currentLine[0].ToString().ToLowerInvariant()[0];
            if (Vincent.wait > 0)
              --Vincent.wait;
            if ((ch < 'a' || ch > 'z') && (ch < '0' || ch > '9') && (ch != '\'' && Vincent.lastWord != ""))
            {
              int num2 = (int) CRC32.Generate(Vincent.lastWord.Trim());
              Vincent.lastWord = "";
            }
            else
              Vincent.lastWord += (string) (object) ch;
            if (Vincent.wait > 0)
            {
              --Vincent.wait;
            }
            else
            {
              Vincent.wait = 2;
              SFX.Play("tinyTick", 0.4f, 0.2f);
            }
            Vincent._currentLine = Vincent._currentLine.Remove(0, 1);
          }
          Vincent._waitLetter += num1;
        }
      }
      else
      {
        if (Vincent.show)
        {
          Vincent._afterShowWait += 0.12f;
          if ((double) Vincent._afterShowWait >= 1.0)
            Vincent._allowMovement = true;
        }
        Vincent._talkMove += 0.75f;
        if ((double) Vincent._talkMove > 1.0)
        {
          Vincent.frame = 0;
          Vincent._talkMove = 0.0f;
        }
      }
      string str2 = "";
      for (int index = 0; index < Vincent.products.Count; ++index)
      {
        str2 += (string) (object) 9;
        Camera camera = new Camera(0.0f, 0.0f, 64f, 16f);
        DuckGame.Graphics.SetRenderTarget(Vincent._priceTargets[index]);
        DepthStencilState depthStencilState = new DepthStencilState()
        {
          StencilEnable = true,
          StencilFunction = CompareFunction.Always,
          StencilPass = StencilOperation.Replace,
          ReferenceStencil = 1,
          DepthBufferEnable = false
        };
        DuckGame.Graphics.Clear(new Color(0, 0, 0, 0));
        DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, depthStencilState, RasterizerState.CullNone, (MTEffect) null, camera.getMatrix());
        string text = "$" + Math.Min(Math.Max(Vincent.products[index].cost, 0), 9999).ToString();
        Vincent._furniTag.frame = text.Length - 1;
        Vincent._priceFontRightways.Draw(text, new Vec2((float) ((double) (5 - text.Length) / 5.0 * 20.0), 0.0f), Vincent.products[index].cost > Profiles.experienceProfile.littleManBucks ? Colors.DGRed : Color.Black, new Depth(0.97f));
        DuckGame.Graphics.screen.End();
        DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      }
    }

    public static void Draw()
    {
      Vec2 vec2_1 = new Vec2((float) ((double) Vincent._listLerp * 270.0 - 200.0), 20f);
      if ((double) Vincent._challengeLerp < 0.00999999977648258 && (double) Vincent._chancyLerp < 0.00999999977648258)
        return;
      Vec2 vec2_2 = new Vec2((float) (100.0 * (1.0 - (double) Vincent._chancyLerp)), (float) (100.0 * (1.0 - (double) Vincent._chancyLerp) - 4.0));
      Vec2 vec2_3 = new Vec2(280f, 30f);
      Vec2 vec2_4 = new Vec2(20f, 132f) + vec2_2;
      DuckGame.Graphics.DrawRect(vec2_4 + new Vec2(-2f, 0.0f), vec2_4 + vec2_3 + new Vec2(2f, 0.0f), Color.Black, new Depth(0.96f));
      int num = 0;
      for (int index1 = Vincent._lineProgress.Count - 1; index1 >= 0; --index1)
      {
        float stringWidth = DuckGame.Graphics.GetStringWidth(Vincent._lineProgress[index1].text);
        float y = vec2_4.y + 2f + (float) (num * 9);
        float x = (float) ((double) vec2_4.x + (double) vec2_3.x / 2.0 - (double) stringWidth / 2.0);
        for (int index2 = Vincent._lineProgress[index1].segments.Count - 1; index2 >= 0; --index2)
        {
          Vincent._descriptionFont.Draw(Vincent._lineProgress[index1].segments[index2].text, new Vec2(x, y), Vincent._lineProgress[index1].segments[index2].color, new Depth(0.97f));
          x += (float) (Vincent._lineProgress[index1].segments[index2].text.Length * 8);
        }
        ++num;
      }
      Vincent._tail.flipV = true;
      DuckGame.Graphics.Draw(Vincent._tail, 222f + vec2_2.x, 117f + vec2_2.y);
      if (Vincent.hasKid)
        Vincent._dealer.frame += 9;
      Vincent._dealer.depth = new Depth(0.96f);
      Vincent._dealer.alpha = Vincent.alpha;
      DuckGame.Graphics.Draw((Sprite) Vincent._dealer, 200f + vec2_2.x, 26f + vec2_2.y);
      switch (Vincent.type)
      {
        case DayType.SaleDay:
          Vincent._bigBanner.depth = new Depth(0.96f);
          DuckGame.Graphics.Draw(Vincent._bigBanner, 22f, (float) ((double) Vincent._showLerp * 100.0 - 80.0));
          DuckGame.Graphics.Draw(Vincent._bigBanner, 194f, (float) ((double) Vincent._showLerp * 100.0 - 80.0));
          break;
        case DayType.ImportDay:
          Vincent._fancyBanner.depth = new Depth(0.96f);
          DuckGame.Graphics.Draw(Vincent._fancyBanner, 22f, (float) ((double) Vincent._showLerp * 100.0 - 80.0));
          DuckGame.Graphics.Draw(Vincent._fancyBanner, 194f, (float) ((double) Vincent._showLerp * 100.0 - 80.0));
          break;
      }
      Vincent._furniFrame.alpha = Vincent.alpha;
      Vincent._cheapTape.alpha = Vincent.alpha * 0.9f;
      Vincent._furniFill.alpha = Vincent.alpha;
      Vincent._furniHov.alpha = Vincent.alpha;
      Vincent._furniTag.alpha = Vincent.alpha;
      Vincent._newSticker.alpha = Vincent.alpha;
      Vincent._rareSticker.alpha = Vincent.alpha;
      Vincent._soldSprite.alpha = Vincent.alpha;
      Vec2 vec2_5 = new Vec2(84f, 46f);
      Vincent._cheapTape.depth = new Depth(0.968f);
      Vincent._furniFrame.depth = new Depth(0.96f);
      Vincent._furniFill.depth = new Depth(0.965f);
      Vincent._furniHov.depth = new Depth(0.965f);
      Vincent._furniTag.depth = new Depth(0.972f);
      Vincent._newSticker.depth = new Depth(0.972f);
      Vincent._rareSticker.depth = new Depth(0.972f);
      Vincent._soldSprite.depth = new Depth(0.975f);
      if (Vincent.products.Count > 0)
      {
        int index1 = 0;
        Vec2 pos = new Vec2(vec2_5.x - 200f + Math.Min(Vincent._showLerp * (float) (200 + 40 * index1), 200f), vec2_5.y);
        if (Vincent.products.Count == 1)
          pos = new Vec2(vec2_5.x - 200f + Math.Min(Vincent._showLerp * 275f, 240f), vec2_5.y + 30f);
        DuckGame.Graphics.Draw(Vincent._furniFrame, pos.x, pos.y);
        int val1_1 = Vincent.products[0].cost;
        bool flag1 = false;
        if (Vincent.products[0].cost != Vincent.products[0].originalCost)
        {
          flag1 = true;
          val1_1 = Vincent.products[0].originalCost;
          DuckGame.Graphics.Draw((Tex2D) Vincent._priceTargets[0], new Vec2(pos.x - 13f, pos.y - 27f), new Rectangle?(), Color.White, 0.3f, Vec2.Zero, Vec2.One, SpriteEffects.None, new Depth(0.9685f));
          DuckGame.Graphics.Draw(Vincent._cheapTape, pos.x, pos.y);
        }
        Vincent._furniFill.color = Vincent.products[index1].color;
        DuckGame.Graphics.Draw(Vincent._furniFill, pos.x, pos.y);
        Vincent.products[index1].Draw(pos, Vincent.alpha, 0.97f);
        if (index1 == Vincent._selectIndex)
          DuckGame.Graphics.Draw(Vincent._furniHov, pos.x - 1f, pos.y);
        if (Vincent.products[index1].type == VPType.Furniture && Vincent.products[index1].furnitureData.rarity >= Rarity.SuperRare)
        {
          Vincent._rareSticker.frame = index1 == Vincent._selectIndex ? 1 : 0;
          DuckGame.Graphics.Draw((Sprite) Vincent._rareSticker, pos.x - 23f, pos.y - 19f);
        }
        else if (Vincent.products[index1].type == VPType.Hat || !Vincent.products[index1].sold && Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[index1].furnitureData.index) <= 0)
        {
          Vincent._newSticker.frame = index1 == Vincent._selectIndex ? 1 : 0;
          DuckGame.Graphics.Draw((Sprite) Vincent._newSticker, pos.x - 23f, pos.y - 19f);
        }
        if (Vincent.products[index1].sold)
        {
          DuckGame.Graphics.Draw(Vincent._soldSprite, pos.x, pos.y);
        }
        else
        {
          string str = Math.Min(Math.Max(val1_1, 0), 9999).ToString();
          Vincent._furniTag.frame = str.Length - 1;
          DuckGame.Graphics.Draw((Sprite) Vincent._furniTag, pos.x + 21f, pos.y - 25f);
          string text = "$\n";
          foreach (char ch in str)
            text = text + (object) ch + "\n";
          (!flag1 ? Vincent._priceFont : Vincent._priceFontCrossout).Draw(text, new Vec2(pos.x + 24f, pos.y - 16f), val1_1 > Profiles.experienceProfile.littleManBucks ? Colors.DGRed : (!flag1 ? Color.Black : Color.White), (Depth) (275f * (float) Math.PI / 887f));
        }
        if (Vincent.products.Count > 1)
        {
          int index2 = 1;
          pos = new Vec2((float) ((double) vec2_5.x + 70.0 - 200.0) + Math.Min(Vincent._showLerp * (float) (200 + 40 * index2), 200f), vec2_5.y);
          DuckGame.Graphics.Draw(Vincent._furniFrame, pos.x, pos.y);
          int val1_2 = Vincent.products[1].cost;
          bool flag2 = false;
          if (Vincent.products[1].cost != Vincent.products[1].originalCost)
          {
            flag2 = true;
            val1_2 = Vincent.products[1].originalCost;
            DuckGame.Graphics.Draw((Tex2D) Vincent._priceTargets[1], new Vec2(pos.x - 13f, pos.y - 27f), new Rectangle?(), Color.White, 0.3f, Vec2.Zero, Vec2.One, SpriteEffects.None, new Depth(0.9685f));
            DuckGame.Graphics.Draw(Vincent._cheapTape, pos.x, pos.y);
          }
          Vincent._furniFill.color = Vincent.products[index2].color;
          DuckGame.Graphics.Draw(Vincent._furniFill, pos.x, pos.y);
          Vincent.products[index2].Draw(pos, Vincent.alpha, 0.97f);
          if (index2 == Vincent._selectIndex)
            DuckGame.Graphics.Draw(Vincent._furniHov, pos.x - 1f, pos.y);
          if (Vincent.products[index2].type == VPType.Furniture && Vincent.products[index2].furnitureData.rarity >= Rarity.SuperRare)
          {
            Vincent._rareSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._rareSticker, pos.x - 23f, pos.y - 19f);
          }
          else if (Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[index2].furnitureData.index) <= 0)
          {
            Vincent._newSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._newSticker, pos.x - 23f, pos.y - 19f);
          }
          if (Vincent.products[index2].sold)
          {
            DuckGame.Graphics.Draw(Vincent._soldSprite, pos.x, pos.y);
          }
          else
          {
            string str = Math.Min(Math.Max(val1_2, 0), 9999).ToString();
            Vincent._furniTag.frame = str.Length - 1;
            DuckGame.Graphics.Draw((Sprite) Vincent._furniTag, pos.x + 21f, pos.y - 25f);
            string text = "$\n";
            foreach (char ch in str)
              text = text + (object) ch + "\n";
            (!flag2 ? Vincent._priceFont : Vincent._priceFontCrossout).Draw(text, new Vec2(pos.x + 24f, pos.y - 16f), val1_2 > Profiles.experienceProfile.littleManBucks ? Colors.DGRed : (!flag2 ? Color.Black : Color.White), (Depth) (275f * (float) Math.PI / 887f));
          }
        }
        if (Vincent.products.Count > 2)
        {
          int index2 = 2;
          pos = new Vec2(vec2_5.x - 200f + Math.Min(Vincent._showLerp * (float) (200 + 40 * index2), 200f), vec2_5.y + 54f);
          DuckGame.Graphics.Draw(Vincent._furniFrame, pos.x, pos.y);
          int val1_2 = Vincent.products[2].cost;
          bool flag2 = false;
          if (Vincent.products[2].cost != Vincent.products[2].originalCost)
          {
            flag2 = true;
            val1_2 = Vincent.products[2].originalCost;
            DuckGame.Graphics.Draw((Tex2D) Vincent._priceTargets[2], new Vec2(pos.x - 13f, pos.y - 27f), new Rectangle?(), Color.White, 0.3f, Vec2.Zero, Vec2.One, SpriteEffects.None, new Depth(0.9685f));
            DuckGame.Graphics.Draw(Vincent._cheapTape, pos.x, pos.y);
          }
          Vincent._furniFill.color = Vincent.products[index2].color;
          DuckGame.Graphics.Draw(Vincent._furniFill, pos.x, pos.y);
          Vincent.products[index2].Draw(pos, Vincent.alpha, 0.97f);
          if (index2 == Vincent._selectIndex)
            DuckGame.Graphics.Draw(Vincent._furniHov, pos.x - 1f, pos.y);
          if (Vincent.products[index2].type == VPType.Furniture && Vincent.products[index2].furnitureData.rarity >= Rarity.SuperRare)
          {
            Vincent._rareSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._rareSticker, pos.x - 23f, pos.y - 19f);
          }
          else if (Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[index2].furnitureData.index) <= 0)
          {
            Vincent._newSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._newSticker, pos.x - 23f, pos.y - 19f);
          }
          if (Vincent.products[index2].sold)
          {
            DuckGame.Graphics.Draw(Vincent._soldSprite, pos.x, pos.y);
          }
          else
          {
            string str = Math.Min(Math.Max(val1_2, 0), 9999).ToString();
            Vincent._furniTag.frame = str.Length - 1;
            DuckGame.Graphics.Draw((Sprite) Vincent._furniTag, pos.x + 21f, pos.y - 25f);
            string text = "$\n";
            foreach (char ch in str)
              text = text + (object) ch + "\n";
            (!flag2 ? Vincent._priceFont : Vincent._priceFontCrossout).Draw(text, new Vec2(pos.x + 24f, pos.y - 16f), val1_2 > Profiles.experienceProfile.littleManBucks ? Colors.DGRed : (!flag2 ? Color.Black : Color.White), (Depth) (275f * (float) Math.PI / 887f));
          }
        }
        if (Vincent.products.Count > 3)
        {
          int index2 = 3;
          pos = new Vec2((float) ((double) vec2_5.x + 70.0 - 200.0) + Math.Min(Vincent._showLerp * (float) (200 + 40 * index2), 200f), vec2_5.y + 54f);
          DuckGame.Graphics.Draw(Vincent._furniFrame, pos.x, pos.y);
          int val1_2 = Vincent.products[3].cost;
          bool flag2 = false;
          if (Vincent.products[3].cost != Vincent.products[3].originalCost)
          {
            flag2 = true;
            val1_2 = Vincent.products[3].originalCost;
            DuckGame.Graphics.Draw((Tex2D) Vincent._priceTargets[3], new Vec2(pos.x - 13f, pos.y - 27f), new Rectangle?(), Color.White, 0.3f, Vec2.Zero, Vec2.One, SpriteEffects.None, new Depth(0.9685f));
            DuckGame.Graphics.Draw(Vincent._cheapTape, pos.x, pos.y);
          }
          Vincent._furniFill.color = Vincent.products[index2].color;
          DuckGame.Graphics.Draw(Vincent._furniFill, pos.x, pos.y);
          Vincent.products[index2].Draw(pos, Vincent.alpha, 0.97f);
          if (index2 == Vincent._selectIndex)
            DuckGame.Graphics.Draw(Vincent._furniHov, pos.x - 1f, pos.y);
          if (Vincent.products[index2].type == VPType.Furniture && Vincent.products[index2].furnitureData.rarity >= Rarity.SuperRare)
          {
            Vincent._rareSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._rareSticker, pos.x - 23f, pos.y - 19f);
          }
          else if (Profiles.experienceProfile.GetNumFurnitures((int) Vincent.products[index2].furnitureData.index) <= 0)
          {
            Vincent._newSticker.frame = index2 == Vincent._selectIndex ? 1 : 0;
            DuckGame.Graphics.Draw((Sprite) Vincent._newSticker, pos.x - 23f, pos.y - 19f);
          }
          if (Vincent.products[index2].sold)
          {
            DuckGame.Graphics.Draw(Vincent._soldSprite, pos.x, pos.y);
          }
          else
          {
            string str = Math.Min(Math.Max(val1_2, 0), 9999).ToString();
            Vincent._furniTag.frame = str.Length - 1;
            DuckGame.Graphics.Draw((Sprite) Vincent._furniTag, pos.x + 21f, pos.y - 25f);
            string text = "$\n";
            foreach (char ch in str)
              text = text + (object) ch + "\n";
            (!flag2 ? Vincent._priceFont : Vincent._priceFontCrossout).Draw(text, new Vec2(pos.x + 24f, pos.y - 16f), val1_2 > Profiles.experienceProfile.littleManBucks ? Colors.DGRed : (!flag2 ? Color.Black : Color.White), (Depth) (275f * (float) Math.PI / 887f));
          }
        }
      }
      if (Vincent.show && Vincent.products.Count > 0)
      {
        int index = 0;
        if (Vincent._selectIndex >= 0)
          index = Vincent._selectIndex;
        Vec2 p1 = new Vec2(20f, 6f);
        Vec2 vec2_6 = new Vec2(226f, 11f);
        DuckGame.Graphics.DrawRect(p1, p1 + vec2_6, Color.Black, new Depth(0.96f));
        string name = Vincent.products[index].name;
        DuckGame.Graphics.DrawString(name, p1 + new Vec2((float) (((double) vec2_6.x - 27.0) / 2.0 - (double) DuckGame.Graphics.GetStringWidth(name) / 2.0), 2f), new Color(163, 206, 39) * Vincent.alpha, new Depth(0.97f));
        Vincent._tail.depth = new Depth(0.5f);
        Vincent._tail.alpha = Vincent.alpha;
        Vincent._tail.flipH = false;
        Vincent._tail.flipV = false;
        DuckGame.Graphics.Draw(Vincent._tail, 222f, 17f);
      }
      if (!Vincent.hasKid)
        return;
      Vincent._dealer.frame -= 9;
    }
  }
}
