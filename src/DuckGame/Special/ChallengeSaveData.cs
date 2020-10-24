// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeSaveData
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Xml.Linq;

namespace DuckGame
{
  public class ChallengeSaveData : Serializable
  {
    public string challenge;
    public TrophyType trophy;
    public int bestTime;
    public int targets;
    public int goodies;
    public string profileID = "";
    public string frameID = "";
    public string frameImage = "";

    public ChallengeSaveData Clone() => new ChallengeSaveData()
    {
      trophy = this.trophy,
      bestTime = this.bestTime,
      profileID = this.profileID,
      targets = this.targets,
      goodies = this.goodies
    };

    public BinaryClassChunk Serialize()
    {
      BinaryClassChunk element = new BinaryClassChunk();
      this.SerializeField(element, "trophy");
      this.SerializeField(element, "bestTime");
      this.SerializeField(element, "profileID");
      this.SerializeField(element, "targets");
      this.SerializeField(element, "goodies");
      this.SerializeField(element, "frameID");
      this.SerializeField(element, "frameImage");
      return element;
    }

    public bool Deserialize(BinaryClassChunk node)
    {
      this.DeserializeField(node, "trophy");
      this.DeserializeField(node, "bestTime");
      this.DeserializeField(node, "profileID");
      this.DeserializeField(node, "targets");
      this.DeserializeField(node, "goodies");
      this.DeserializeField(node, "frameID");
      this.DeserializeField(node, "frameImage");
      return true;
    }

    public XElement LegacySerialize()
    {
      XElement element = new XElement((XName) "challengeSaveData");
      this.LegacySerializeField(element, "trophy");
      this.LegacySerializeField(element, "bestTime");
      this.LegacySerializeField(element, "profileID");
      this.LegacySerializeField(element, "targets");
      this.LegacySerializeField(element, "goodies");
      this.LegacySerializeField(element, "frameID");
      this.LegacySerializeField(element, "frameImage");
      return element;
    }

    public bool LegacyDeserialize(XElement node)
    {
      this.LegacyDeserializeField(node, "trophy");
      this.LegacyDeserializeField(node, "bestTime");
      this.LegacyDeserializeField(node, "profileID");
      this.LegacyDeserializeField(node, "targets");
      this.LegacyDeserializeField(node, "goodies");
      this.LegacyDeserializeField(node, "frameID");
      this.LegacyDeserializeField(node, "frameImage");
      return true;
    }
  }
}
