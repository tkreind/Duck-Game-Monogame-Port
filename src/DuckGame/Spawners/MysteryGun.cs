// Decompiled with JetBrains decompiler
// Type: DuckGame.MysteryGun
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  [EditorGroup("spawns")]
  [BaggedProperty("isInDemo", true)]
  public class MysteryGun : Thing
  {
    private SpriteMap _sprite;
    public List<TypeProbPair> contains = new List<TypeProbPair>();

    public MysteryGun(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("mysteryGun", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 16f);
      this.collisionSize = new Vec2(10f, 10f);
      this.collisionOffset = new Vec2(-5f, -5f);
      this.depth = (Depth) 0.5f;
      this._canFlip = false;
    }

    public static System.Type PickType(int chanceGroup, List<TypeProbPair> contains)
    {
      ItemBox.GetPhysicsObjects(Editor.Placeables);
      Random random = new Random((int) ((double) Level.GetChanceGroup2(chanceGroup) * 2147483648.0 - 1.0));
      Random generator = Rando.generator;
      Rando.generator = random;
      IOrderedEnumerable<TypeProbPair> orderedEnumerable = contains.OrderBy<TypeProbPair, float>((Func<TypeProbPair, float>) (x => Rando.Float(1f)));
      System.Type type = (System.Type) null;
      float num = 0.0f;
      foreach (TypeProbPair typeProbPair in (IEnumerable<TypeProbPair>) orderedEnumerable)
      {
        if ((double) Rando.Float(1f) > 1.0 - (double) typeProbPair.probability)
        {
          type = typeProbPair.type;
          break;
        }
        if ((double) typeProbPair.probability > (double) num)
        {
          num = typeProbPair.probability;
          type = typeProbPair.type;
        }
      }
      Rando.generator = generator;
      return type;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      System.Type t = MysteryGun.PickType(this.chanceGroup, this.contains);
      Level.Remove((Thing) this);
      if (!(t != (System.Type) null))
        return;
      Thing thing = Editor.CreateObject(t) as Thing;
      thing.position = this.position;
      Level.Add(thing);
    }

    public override ContextMenu GetContextMenu()
    {
      FieldBinding radioBinding = new FieldBinding((object) this, "contains");
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.InitializeGroups(new EditorGroup(typeof (PhysicsObject)), radioBinding);
      return (ContextMenu) contextMenu;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("contains", (object) MysteryGun.SerializeTypeProb(this.contains));
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.contains = MysteryGun.DeserializeTypeProb(node.GetProperty<string>("contains"));
      return true;
    }

    public static string SerializeTypeProb(List<TypeProbPair> list)
    {
      string str = "";
      foreach (TypeProbPair typeProbPair in list)
      {
        str += ModLoader.SmallTypeName(typeProbPair.type);
        str += ":";
        str += typeProbPair.probability.ToString();
        str += "|";
      }
      return str;
    }

    public static List<TypeProbPair> DeserializeTypeProb(string list)
    {
      List<TypeProbPair> typeProbPairList = new List<TypeProbPair>();
      try
      {
        if (list == null)
          return typeProbPairList;
        string str1 = list;
        char[] chArray = new char[1]{ '|' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (str2.Length > 1)
          {
            string[] strArray = str2.Split(':');
            TypeProbPair typeProbPair = new TypeProbPair()
            {
              type = Editor.GetType(strArray[0]),
              probability = Convert.ToSingle(strArray[1])
            };
            typeProbPairList.Add(typeProbPair);
          }
        }
      }
      catch (Exception ex)
      {
      }
      return typeProbPairList;
    }

    public override void DrawHoverInfo()
    {
      float num = 0.0f;
      foreach (TypeProbPair contain in this.contains)
      {
        if ((double) contain.probability > 0.0)
        {
          Color white = Color.White;
          Color color = (double) contain.probability != 0.0 ? ((double) contain.probability >= 0.300000011920929 ? ((double) contain.probability >= 0.699999988079071 ? Color.Green : Color.Orange) : Colors.DGRed) : Color.DarkGray;
          string text = contain.type.Name + ": " + contain.probability.ToString("0.000");
          Graphics.DrawString(text, this.position + new Vec2((float) (-(double) Graphics.GetStringWidth(text, scale: 0.5f) / 2.0), (float) -(16.0 + (double) num)), color, (Depth) 0.9f, scale: 0.5f);
          num += 4f;
        }
      }
    }
  }
}
