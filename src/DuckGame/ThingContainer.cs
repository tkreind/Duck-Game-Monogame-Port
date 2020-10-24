// Decompiled with JetBrains decompiler
// Type: DuckGame.ThingContainer
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  public class ThingContainer : Thing
  {
    protected List<Thing> _things;
    protected System.Type _type;
    public bool quickSerialize;

    public List<Thing> things => this._things;

    public override void SetTranslation(Vec2 translation)
    {
      foreach (Thing thing in this._things)
        thing.SetTranslation(translation);
      base.SetTranslation(translation);
    }

    public ThingContainer(List<Thing> things, System.Type t)
      : base()
    {
      this._things = things;
      this._type = t;
    }

    public ThingContainer()
      : base()
    {
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = new BinaryClassChunk();
      binaryClassChunk.AddProperty("type", (object) ModLoader.SmallTypeName(this.GetType()));
      binaryClassChunk.AddProperty("blockType", (object) ModLoader.SmallTypeName(this._type));
      BitBuffer bitBuffer1 = new BitBuffer(false);
      bitBuffer1.Write(this._things.Count);
      if (typeof (AutoBlock).IsAssignableFrom(this._type))
      {
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.groupedWithNeighbors = false;
          autoBlock.neighborsInitialized = false;
        }
        BitBuffer bitBuffer2 = new BitBuffer(false);
        bitBuffer2.Write((ushort) 0);
        ushort val = 0;
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.InitializeNeighbors();
          bitBuffer1.Write(thing.x);
          bitBuffer1.Write(thing.y);
          bitBuffer1.Write((byte) thing.frame);
          bitBuffer1.Write(autoBlock.upBlock != null ? (short) this._things.IndexOf((Thing) autoBlock.upBlock) : (short) -1);
          bitBuffer1.Write(autoBlock.downBlock != null ? (short) this._things.IndexOf((Thing) autoBlock.downBlock) : (short) -1);
          bitBuffer1.Write(autoBlock.rightBlock != null ? (short) this._things.IndexOf((Thing) autoBlock.rightBlock) : (short) -1);
          bitBuffer1.Write(autoBlock.leftBlock != null ? (short) this._things.IndexOf((Thing) autoBlock.leftBlock) : (short) -1);
          BlockGroup blockGroup = autoBlock.GroupWithNeighbors(false);
          if (blockGroup != null)
          {
            bitBuffer2.Write(blockGroup.x);
            bitBuffer2.Write(blockGroup.y);
            bitBuffer2.Write(blockGroup.collisionOffset.x);
            bitBuffer2.Write(blockGroup.collisionOffset.y);
            bitBuffer2.Write(blockGroup.collisionSize.x);
            bitBuffer2.Write(blockGroup.collisionSize.y);
            bitBuffer2.Write(blockGroup.blocks.Count<Block>());
            foreach (Block block in blockGroup.blocks)
              bitBuffer2.Write((short) this._things.IndexOf((Thing) block));
            ++val;
          }
        }
        bitBuffer2.position = 0;
        bitBuffer2.Write(val);
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.groupedWithNeighbors = false;
          autoBlock.neighborsInitialized = false;
        }
        if (bitBuffer2.lengthInBytes > 2)
          binaryClassChunk.AddProperty("groupData", (object) bitBuffer2);
      }
      else
      {
        foreach (Thing thing in this._things)
        {
          bitBuffer1.Write(thing.x);
          bitBuffer1.Write(thing.y);
          bitBuffer1.Write((byte) thing.frame);
        }
      }
      binaryClassChunk.AddProperty("data", (object) bitBuffer1);
      return binaryClassChunk;
    }

    private bool DoDeserialize(BinaryClassChunk node)
    {
      System.Type type = Editor.GetType(node.GetProperty<string>("blockType"));
      if (type == (System.Type) null)
        return false;
      bool flag1 = typeof (AutoBlock).IsAssignableFrom(type);
      this._things = new List<Thing>();
      BitBuffer property1 = node.GetProperty<BitBuffer>("data");
      if (!typeof (AutoBlock).IsAssignableFrom(type))
        flag1 = false;
      List<AutoBlock> autoBlockList = new List<AutoBlock>();
      int num1 = property1.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        float num2 = property1.ReadFloat();
        float num3 = property1.ReadFloat();
        int num4 = (int) property1.ReadByte();
        bool flag2 = Level.flipH;
        if (Level.loadingOppositeSymmetry)
          flag2 = !flag2;
        if (flag2)
          num2 = (float) (192.0 - (double) num2 - 16.0);
        Thing thing = Editor.CreateThing(type);
        if (flag2 && thing is AutoBlock)
        {
          (thing as AutoBlock).needsRefresh = true;
          (thing as AutoBlock).isFlipped = true;
        }
        if (thing is BackgroundTile)
        {
          if (flag2)
            (thing as BackgroundTile).isFlipped = true;
          (thing as BackgroundTile).oppositeSymmetry = !Level.loadingOppositeSymmetry;
        }
        if (flag2 && thing is AutoPlatform)
          (thing as AutoPlatform).needsRefresh = true;
        thing.x = num2;
        thing.y = num3;
        thing.placed = true;
        if (thing.isStatic)
          this._isStatic = true;
        if (flag1)
        {
          short num5 = property1.ReadShort();
          short num6 = property1.ReadShort();
          short num7 = property1.ReadShort();
          short num8 = property1.ReadShort();
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.northIndex = (int) num5;
          autoBlock.southIndex = (int) num6;
          if (flag2)
          {
            autoBlock.westIndex = (int) num7;
            autoBlock.eastIndex = (int) num8;
          }
          else
          {
            autoBlock.eastIndex = (int) num7;
            autoBlock.westIndex = (int) num8;
          }
          autoBlockList.Add(autoBlock);
        }
        bool flag3 = true;
        if (Level.symmetry)
        {
          if (Level.leftSymmetry && (double) num2 > 80.0)
            flag3 = false;
          if (!Level.leftSymmetry && (double) num2 < 96.0)
            flag3 = false;
        }
        if (flag3)
        {
          thing.frame = num4;
          this._things.Add(thing);
        }
      }
      if (flag1 && !(Level.current is Editor))
      {
        foreach (AutoBlock autoBlock in autoBlockList)
        {
          if (autoBlock.northIndex != -1)
            autoBlock.upBlock = (Block) autoBlockList[autoBlock.northIndex];
          if (autoBlock.southIndex != -1)
            autoBlock.downBlock = (Block) autoBlockList[autoBlock.southIndex];
          if (autoBlock.eastIndex != -1)
            autoBlock.rightBlock = (Block) autoBlockList[autoBlock.eastIndex];
          if (autoBlock.westIndex != -1)
            autoBlock.leftBlock = (Block) autoBlockList[autoBlock.westIndex];
          autoBlock.neighborsInitialized = true;
        }
        BitBuffer property2 = node.GetProperty<BitBuffer>("groupData");
        if (property2 != null)
        {
          ushort num2 = property2.ReadUShort();
          int num3;
          for (int index1 = 0; index1 < (int) num2; index1 = num3 + 1)
          {
            BlockGroup blockGroup = new BlockGroup();
            blockGroup.position = new Vec2(property2.ReadFloat(), property2.ReadFloat());
            bool flag2 = Level.flipH;
            if (Level.loadingOppositeSymmetry)
              flag2 = !flag2;
            if (flag2)
              blockGroup.position.x = (float) (192.0 - (double) blockGroup.position.x - 16.0);
            blockGroup.collisionOffset = new Vec2(property2.ReadFloat(), property2.ReadFloat());
            blockGroup.collisionSize = new Vec2(property2.ReadFloat(), property2.ReadFloat());
            float num4 = 88f;
            if (Level.symmetry)
            {
              if (Level.leftSymmetry)
              {
                if ((double) blockGroup.left < (double) num4 && (double) blockGroup.right > (double) num4)
                {
                  float num5 = blockGroup.right - num4;
                  float x = blockGroup.collisionSize.x - num5;
                  blockGroup.position.x -= num5;
                  blockGroup.position.x += x / 2f;
                  blockGroup.collisionSize = new Vec2(x, blockGroup.collisionSize.y);
                  blockGroup.collisionOffset = new Vec2((float) -((double) x / 2.0), blockGroup.collisionOffset.y);
                  blockGroup.right = num4;
                }
              }
              else
              {
                num4 = 88f;
                if ((double) blockGroup.right > (double) num4 && (double) blockGroup.left < (double) num4)
                {
                  float num5 = num4 - blockGroup.left;
                  float x = blockGroup.collisionSize.x - num5;
                  blockGroup.position.x += num5;
                  blockGroup.position.x -= x / 2f;
                  blockGroup.collisionSize = new Vec2(x, blockGroup.collisionSize.y);
                  blockGroup.collisionOffset = new Vec2((float) -((double) x / 2.0), blockGroup.collisionOffset.y);
                  blockGroup.left = num4;
                }
              }
            }
            int num6 = property2.ReadInt();
            for (int index2 = 0; index2 < num6; ++index2)
            {
              int index3 = (int) property2.ReadShort();
              if (index3 >= 0)
              {
                AutoBlock autoBlock = autoBlockList[index3];
                bool flag3 = true;
                if (Level.symmetry)
                {
                  if (Level.leftSymmetry && (double) autoBlock.x > 80.0)
                    flag3 = false;
                  if (!Level.leftSymmetry && (double) autoBlock.x < 96.0)
                    flag3 = false;
                }
                if (flag3)
                {
                  autoBlock.groupedWithNeighbors = true;
                  blockGroup.Add((Block) autoBlock);
                  blockGroup.physicsMaterial = autoBlock.physicsMaterial;
                  blockGroup.thickness = autoBlock.thickness;
                }
                this._things.Remove((Thing) autoBlock);
              }
            }
            num3 = index1 + num6;
            if (flag2)
              blockGroup.needsRefresh = true;
            if (Level.symmetry)
            {
              if (Level.leftSymmetry && (double) blockGroup.left < (double) num4)
                this._things.Add((Thing) blockGroup);
              else if (!Level.leftSymmetry && (double) blockGroup.right > (double) num4)
                this._things.Add((Thing) blockGroup);
            }
            else
              this._things.Add((Thing) blockGroup);
          }
        }
      }
      return true;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      if (!Level.symmetry)
        return this.DoDeserialize(node);
      Level.leftSymmetry = true;
      Level.loadingOppositeSymmetry = false;
      this.DoDeserialize(node);
      List<Thing> thingList = new List<Thing>((IEnumerable<Thing>) this._things);
      Level.loadingOppositeSymmetry = true;
      Level.leftSymmetry = false;
      this.DoDeserialize(node);
      this._things.AddRange((IEnumerable<Thing>) thingList);
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = new XElement((XName) "Object");
      xelement.Add((object) new XElement((XName) "type", (object) this.GetType().AssemblyQualifiedName));
      xelement.Add((object) new XElement((XName) "blockType", (object) this._type.AssemblyQualifiedName));
      string str1 = "n,";
      string str2 = "";
      if (typeof (AutoBlock).IsAssignableFrom(this._type))
      {
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.groupedWithNeighbors = false;
          autoBlock.neighborsInitialized = false;
        }
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.InitializeNeighbors();
          str1 = str1 + Change.ToString((object) thing.x) + ",";
          str1 = str1 + Change.ToString((object) thing.y) + ",";
          str1 = str1 + (object) thing.frame + ",";
          str1 = autoBlock.upBlock == null ? str1 + "-1," : str1 + Change.ToString((object) this._things.IndexOf((Thing) autoBlock.upBlock)) + ",";
          str1 = autoBlock.downBlock == null ? str1 + "-1," : str1 + Change.ToString((object) this._things.IndexOf((Thing) autoBlock.downBlock)) + ",";
          str1 = autoBlock.rightBlock == null ? str1 + "-1," : str1 + Change.ToString((object) this._things.IndexOf((Thing) autoBlock.rightBlock)) + ",";
          str1 = autoBlock.leftBlock == null ? str1 + "-1," : str1 + Change.ToString((object) this._things.IndexOf((Thing) autoBlock.leftBlock)) + ",";
          BlockGroup blockGroup = autoBlock.GroupWithNeighbors(false);
          if (blockGroup != null)
          {
            str2 = str2 + Change.ToString((object) blockGroup.x) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.y) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.collisionOffset.x) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.collisionOffset.y) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.collisionSize.x) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.collisionSize.y) + ",";
            str2 = str2 + Change.ToString((object) blockGroup.blocks.Count<Block>()) + ",";
            foreach (Block block in blockGroup.blocks)
              str2 = str2 + Change.ToString((object) this._things.IndexOf((Thing) block)) + ",";
          }
        }
        foreach (Thing thing in this._things)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.groupedWithNeighbors = false;
          autoBlock.neighborsInitialized = false;
        }
        if (str2.Length > 0)
        {
          string str3 = str2.Substring(0, str2.Length - 1);
          xelement.Add((object) new XElement((XName) "groupData", (object) str3));
        }
      }
      else
      {
        foreach (Thing thing in this._things)
        {
          str1 = str1 + Change.ToString((object) thing.x) + ",";
          str1 = str1 + Change.ToString((object) thing.y) + ",";
          str1 = str1 + (object) thing.frame + ",";
        }
      }
      string str4 = str1.Substring(0, str1.Length - 1);
      xelement.Add((object) new XElement((XName) "data", (object) str4));
      return xelement;
    }

    private bool LegacyDoDeserialize(XElement node)
    {
      System.Type type = Editor.GetType(node.Element((XName) "blockType").Value);
      bool flag1 = typeof (AutoBlock).IsAssignableFrom(type);
      this._things = new List<Thing>();
      string[] strArray1 = node.Element((XName) "data").Value.Split(',');
      bool flag2 = strArray1[0] == "n";
      if (!flag2)
        flag1 = false;
      List<AutoBlock> autoBlockList = new List<AutoBlock>();
      for (int index = flag2 ? 1 : 0; index < ((IEnumerable<string>) strArray1).Count<string>(); index += 3)
      {
        float num = Change.ToSingle((object) strArray1[index]);
        float single = Change.ToSingle((object) strArray1[index + 1]);
        int int32 = Convert.ToInt32(strArray1[index + 2]);
        bool flag3 = Level.flipH;
        if (Level.loadingOppositeSymmetry)
          flag3 = !flag3;
        if (flag3)
          num = (float) (192.0 - (double) num - 16.0);
        Thing thing = Editor.CreateThing(type);
        if (flag3 && thing is AutoBlock)
        {
          (thing as AutoBlock).needsRefresh = true;
          (thing as AutoBlock).isFlipped = true;
        }
        if (flag3 && thing is AutoPlatform)
          (thing as AutoPlatform).needsRefresh = true;
        thing.x = num;
        thing.y = single;
        thing.placed = true;
        if (thing.isStatic)
          this._isStatic = true;
        if (flag1)
        {
          AutoBlock autoBlock = thing as AutoBlock;
          autoBlock.northIndex = Convert.ToInt32(strArray1[index + 3]);
          autoBlock.southIndex = Convert.ToInt32(strArray1[index + 4]);
          if (flag3)
          {
            autoBlock.westIndex = Convert.ToInt32(strArray1[index + 5]);
            autoBlock.eastIndex = Convert.ToInt32(strArray1[index + 6]);
          }
          else
          {
            autoBlock.eastIndex = Convert.ToInt32(strArray1[index + 5]);
            autoBlock.westIndex = Convert.ToInt32(strArray1[index + 6]);
          }
          autoBlockList.Add(autoBlock);
          index += 4;
        }
        bool flag4 = true;
        if (Level.symmetry)
        {
          if (Level.leftSymmetry && (double) num > 80.0)
            flag4 = false;
          if (!Level.leftSymmetry && (double) num < 96.0)
            flag4 = false;
        }
        if (flag4)
        {
          thing.frame = int32;
          this._things.Add(thing);
        }
      }
      if (flag1 && !(Level.current is Editor))
      {
        foreach (AutoBlock autoBlock in autoBlockList)
        {
          if (autoBlock.northIndex != -1)
            autoBlock.upBlock = (Block) autoBlockList[autoBlock.northIndex];
          if (autoBlock.southIndex != -1)
            autoBlock.downBlock = (Block) autoBlockList[autoBlock.southIndex];
          if (autoBlock.eastIndex != -1)
            autoBlock.rightBlock = (Block) autoBlockList[autoBlock.eastIndex];
          if (autoBlock.westIndex != -1)
            autoBlock.leftBlock = (Block) autoBlockList[autoBlock.westIndex];
          autoBlock.neighborsInitialized = true;
        }
        XElement xelement = node.Element((XName) "groupData");
        if (xelement != null)
        {
          string[] strArray2 = xelement.Value.Split(',');
          int num1;
          for (int index1 = 0; index1 < ((IEnumerable<string>) strArray2).Count<string>(); index1 = num1 + 7)
          {
            BlockGroup blockGroup = new BlockGroup();
            blockGroup.position = new Vec2(Change.ToSingle((object) strArray2[index1]), Change.ToSingle((object) strArray2[index1 + 1]));
            bool flag3 = Level.flipH;
            if (Level.loadingOppositeSymmetry)
              flag3 = !flag3;
            if (flag3)
              blockGroup.position.x = (float) (192.0 - (double) blockGroup.position.x - 16.0);
            blockGroup.collisionOffset = new Vec2(Change.ToSingle((object) strArray2[index1 + 2]), Change.ToSingle((object) strArray2[index1 + 3]));
            blockGroup.collisionSize = new Vec2(Change.ToSingle((object) strArray2[index1 + 4]), Change.ToSingle((object) strArray2[index1 + 5]));
            float num2 = 88f;
            if (Level.symmetry)
            {
              if (Level.leftSymmetry)
              {
                if ((double) blockGroup.left < (double) num2 && (double) blockGroup.right > (double) num2)
                {
                  float num3 = blockGroup.right - num2;
                  float x = blockGroup.collisionSize.x - num3;
                  blockGroup.position.x -= num3;
                  blockGroup.position.x += x / 2f;
                  blockGroup.collisionSize = new Vec2(x, blockGroup.collisionSize.y);
                  blockGroup.collisionOffset = new Vec2((float) -((double) x / 2.0), blockGroup.collisionOffset.y);
                  blockGroup.right = num2;
                }
              }
              else
              {
                num2 = 88f;
                if ((double) blockGroup.right > (double) num2 && (double) blockGroup.left < (double) num2)
                {
                  float num3 = num2 - blockGroup.left;
                  float x = blockGroup.collisionSize.x - num3;
                  blockGroup.position.x += num3;
                  blockGroup.position.x -= x / 2f;
                  blockGroup.collisionSize = new Vec2(x, blockGroup.collisionSize.y);
                  blockGroup.collisionOffset = new Vec2((float) -((double) x / 2.0), blockGroup.collisionOffset.y);
                  blockGroup.left = num2;
                }
              }
            }
            int int32_1 = Convert.ToInt32(strArray2[index1 + 6]);
            for (int index2 = 0; index2 < int32_1; ++index2)
            {
              int int32_2 = Convert.ToInt32(strArray2[index1 + 7 + index2]);
              AutoBlock autoBlock = autoBlockList[int32_2];
              bool flag4 = true;
              if (Level.symmetry)
              {
                if (Level.leftSymmetry && (double) autoBlock.x > 80.0)
                  flag4 = false;
                if (!Level.leftSymmetry && (double) autoBlock.x < 96.0)
                  flag4 = false;
              }
              if (flag4)
              {
                autoBlock.groupedWithNeighbors = true;
                blockGroup.Add((Block) autoBlock);
                blockGroup.physicsMaterial = autoBlock.physicsMaterial;
                blockGroup.thickness = autoBlock.thickness;
              }
              this._things.Remove((Thing) autoBlock);
            }
            num1 = index1 + int32_1;
            if (flag3)
              blockGroup.needsRefresh = true;
            if (Level.symmetry)
            {
              if (Level.leftSymmetry && (double) blockGroup.left < (double) num2)
                this._things.Add((Thing) blockGroup);
              else if (!Level.leftSymmetry && (double) blockGroup.right > (double) num2)
                this._things.Add((Thing) blockGroup);
            }
            else
              this._things.Add((Thing) blockGroup);
          }
        }
      }
      return true;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      if (!Level.symmetry)
        return this.LegacyDoDeserialize(node);
      Level.leftSymmetry = true;
      Level.loadingOppositeSymmetry = false;
      this.LegacyDoDeserialize(node);
      List<Thing> thingList = new List<Thing>((IEnumerable<Thing>) this._things);
      Level.loadingOppositeSymmetry = true;
      Level.leftSymmetry = false;
      this.LegacyDoDeserialize(node);
      this._things.AddRange((IEnumerable<Thing>) thingList);
      return true;
    }
  }
}
