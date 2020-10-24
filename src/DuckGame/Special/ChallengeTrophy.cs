// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeTrophy
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Xml.Linq;

namespace DuckGame
{
  public class ChallengeTrophy : Serializable
  {
    private TrophyType _type;
    private int _goodies = -1;
    private int _targets = -1;
    private int _timeRequirement;
    private int _timeRequirementMilliseconds;
    private ChallengeData _owner;

    public TrophyType type
    {
      get => this._type;
      set => this._type = value;
    }

    public int goodies
    {
      get => this._goodies;
      set
      {
        this._goodies = value;
        this._owner.Update();
      }
    }

    public int targets
    {
      get => this._targets;
      set
      {
        this._targets = value;
        this._owner.Update();
      }
    }

    public int timeRequirement
    {
      get => this._timeRequirement;
      set
      {
        this._timeRequirement = value;
        this._owner.Update();
      }
    }

    public int timeRequirementMilliseconds
    {
      get => this._timeRequirementMilliseconds;
      set
      {
        this._timeRequirementMilliseconds = value;
        this._owner.Update();
      }
    }

    public ChallengeTrophy(ChallengeData owner) => this._owner = owner;

    public BinaryClassChunk Serialize()
    {
      BinaryClassChunk element = new BinaryClassChunk();
      this.SerializeField(element, "type");
      this.SerializeField(element, "goodies");
      this.SerializeField(element, "targets");
      this.SerializeField(element, "timeRequirement");
      this.SerializeField(element, "timeRequirementMilliseconds");
      return element;
    }

    public bool Deserialize(BinaryClassChunk node)
    {
      this.DeserializeField(node, "type");
      this.DeserializeField(node, "goodies");
      this.DeserializeField(node, "targets");
      this.DeserializeField(node, "timeRequirement");
      this.DeserializeField(node, "timeRequirementMilliseconds");
      return true;
    }

    public XElement LegacySerialize()
    {
      XElement element = new XElement((XName) "challengeTrophy");
      this.LegacySerializeField(element, "type");
      this.LegacySerializeField(element, "goodies");
      this.LegacySerializeField(element, "targets");
      this.LegacySerializeField(element, "timeRequirement");
      this.LegacySerializeField(element, "timeRequirementMilliseconds");
      return element;
    }

    public bool LegacyDeserialize(XElement node)
    {
      this.LegacyDeserializeField(node, "type");
      this.LegacyDeserializeField(node, "goodies");
      this.LegacyDeserializeField(node, "targets");
      this.LegacyDeserializeField(node, "timeRequirement");
      this.LegacyDeserializeField(node, "timeRequirementMilliseconds");
      return true;
    }
  }
}
