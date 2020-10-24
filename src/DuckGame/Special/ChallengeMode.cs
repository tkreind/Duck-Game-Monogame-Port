// Decompiled with JetBrains decompiler
// Type: DuckGame.ChallengeMode
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("special")]
  public class ChallengeMode : Thing
  {
    public EditorProperty<bool> random = new EditorProperty<bool>(false);
    public EditorProperty<string> music = new EditorProperty<string>("");
    private List<ChallengeTrophy> _eligibleTrophies = new List<ChallengeTrophy>();
    private List<ChallengeTrophy> _wonTrophies = new List<ChallengeTrophy>();
    private int _startGoodies;
    private bool _ended;
    public Duck duck;
    private int restartWait;
    private ContextMenu _hatMenu;
    private int _hatIndex;
    private ChallengeData _challenge = new ChallengeData();

    public ChallengeMode()
      : base()
    {
      this.graphic = new Sprite("challengeIcon");
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(0.9f);
      this.layer = Layer.Foreground;
      this._editorName = "Challenge";
      this._canFlip = false;
      this._canHaveChance = false;
      this.random._tooltip = "If enabled, this challenge will activate a random target sequence whenever all targets are down.";
      this.music._tooltip = "The name of a music file (without extension) from the Duck Game Content/Audio/Music/InGame folder.";
    }

    public List<ChallengeTrophy> wonTrophies => this._wonTrophies;

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      if (Level.current.camera is FollowCam camera)
        camera.minSize = 90f;
      this._eligibleTrophies.AddRange((IEnumerable<ChallengeTrophy>) this._challenge.trophies);
      if (ChallengeLevel.timer != null)
      {
        if (this._challenge.trophies[0].timeRequirement != 0)
          ChallengeLevel.timer.maxTime = new TimeSpan(0, 0, this._challenge.trophies[0].timeRequirement);
        else
          ChallengeLevel.timer.maxTime = new TimeSpan();
      }
      this._startGoodies = Level.current.things[typeof (Goody)].Count<Thing>();
    }

    public override void Update()
    {
      if (this._ended || ChallengeLevel.timer == null)
        return;
      if (this.duck != null && this.duck.dead)
      {
        ++this.restartWait;
        if (this.restartWait >= 3)
        {
          if (this._wonTrophies.Count > 0)
          {
            this._eligibleTrophies.Clear();
          }
          else
          {
            this._ended = true;
            if (Level.current is ChallengeLevel current1)
              current1.RestartChallenge();
          }
        }
      }
      bool flag1 = true;
      for (int index = 0; index < this._eligibleTrophies.Count; ++index)
      {
        bool flag2 = true;
        bool flag3 = false;
        ChallengeTrophy eligibleTrophy = this._eligibleTrophies[index];
        if (eligibleTrophy.type == TrophyType.Developer && (eligibleTrophy.goodies == -1 && eligibleTrophy.targets == -1 && eligibleTrophy.timeRequirement == 0 || !flag1))
        {
          flag2 = false;
        }
        else
        {
          bool flag4 = false;
          if (eligibleTrophy.timeRequirement != 0 && (int) ChallengeLevel.timer.elapsed.TotalSeconds >= eligibleTrophy.timeRequirement && (eligibleTrophy.type == TrophyType.Baseline || Math.Abs(ChallengeLevel.timer.elapsed.TotalSeconds - (double) eligibleTrophy.timeRequirement) > 0.00999999977648258) && (eligibleTrophy.timeRequirementMilliseconds == 0 || (int) Math.Round(ChallengeLevel.timer.elapsed.TotalSeconds % 1.0 * 100.0) > eligibleTrophy.timeRequirementMilliseconds))
          {
            flag2 = false;
            flag4 = true;
          }
          if (!flag4)
          {
            flag3 = true;
            if (eligibleTrophy.targets == -1 && !SequenceItem.IsFinished(SequenceItemType.Target))
              flag3 = false;
            else if (eligibleTrophy.targets != -1 && ChallengeLevel.targetsShot < eligibleTrophy.targets)
              flag3 = false;
            if (eligibleTrophy.goodies == -1 && !SequenceItem.IsFinished(SequenceItemType.Goody))
              flag3 = false;
            else if (ChallengeLevel.goodiesGot < eligibleTrophy.goodies)
              flag3 = false;
          }
          if (flag3)
          {
            flag2 = false;
            if (eligibleTrophy.type != TrophyType.Baseline)
              this._wonTrophies.Add(eligibleTrophy);
          }
        }
        if (!flag2)
        {
          this._eligibleTrophies.RemoveAt(index);
          if (eligibleTrophy.type == TrophyType.Baseline && !flag3)
            this._eligibleTrophies.Clear();
          --index;
        }
      }
      if (Level.current != this.level || this._eligibleTrophies.Count != 0)
        return;
      if (this._wonTrophies.Count > 1)
      {
        ChallengeTrophy challengeTrophy = this._wonTrophies[0];
        foreach (ChallengeTrophy wonTrophy in this._wonTrophies)
        {
          if (wonTrophy.type > challengeTrophy.type)
            challengeTrophy = wonTrophy;
        }
        this._wonTrophies.Clear();
        this._wonTrophies.Add(challengeTrophy);
      }
      ChallengeLevel.timer.Stop();
      if (Level.current is ChallengeLevel current)
        current.ChallengeEnded(this);
      this._ended = true;
    }

    public override void Draw()
    {
      if (!(Level.current is Editor))
        return;
      base.Draw();
    }

    public int hatIndex
    {
      get => this._hatIndex;
      set
      {
        this._hatIndex = value;
        this.UpdateMenuHat();
      }
    }

    private void UpdateMenuHat()
    {
      if (this._hatMenu == null)
        return;
      if (Teams.all[this._hatIndex].hasHat)
      {
        this._hatMenu.image = Teams.all[this._hatIndex].hat.CloneMap();
        this._hatMenu.image.center = new Vec2(12f, 12f) + Teams.all[this._hatIndex].hatOffset;
      }
      else
        this._hatMenu.image = (SpriteMap) null;
    }

    public ChallengeData challenge => this._challenge;

    public override ContextMenu GetContextMenu()
    {
      FieldBinding fieldBinding = new FieldBinding((object) this, "hatIndex");
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      ContextTextbox contextTextbox1 = new ContextTextbox("Name", (IContextListener) null, new FieldBinding((object) this._challenge, "name"));
      contextMenu.AddItem((ContextMenu) contextTextbox1);
      ContextTextbox contextTextbox2 = new ContextTextbox("Desc", (IContextListener) null, new FieldBinding((object) this._challenge, "description"));
      contextMenu.AddItem((ContextMenu) contextTextbox2);
      ContextTextbox contextTextbox3 = new ContextTextbox("Goal", (IContextListener) null, new FieldBinding((object) this._challenge, "goal"));
      contextMenu.AddItem((ContextMenu) contextTextbox3);
      ContextTextbox contextTextbox4 = new ContextTextbox("Requires", (IContextListener) null, new FieldBinding((object) this._challenge, "requirement"), "Number of arcade trophies required to unlock. B15 = 15 Bronze. B2P2 = 2 Bronze, 2 Platinum.");
      contextMenu.AddItem((ContextMenu) contextTextbox4);
      ContextTextbox contextTextbox5 = new ContextTextbox("noun(plural)", (IContextListener) null, new FieldBinding((object) this._challenge, "prefix"), "Name of goal object pluralized, for example \"Ducks\", \"Stars\", etc.");
      contextMenu.AddItem((ContextMenu) contextTextbox5);
      contextMenu.AddItem((ContextMenu) new ContextFile("Prev", (IContextListener) null, new FieldBinding((object) this._challenge, "prevchal"), ContextFileType.Level, "If set, Chancy will offer this challenge as a special challenge after PREV is completed, if REQUIRES is met."));
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Goodies", (IContextListener) null, new FieldBinding((object) this._challenge, "countGoodies"), (System.Type) null, "If set, the goal for this challenge is to collect goodies (stars, finish flags, etc)."));
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Targets", (IContextListener) null, new FieldBinding((object) this._challenge, "countTargets"), (System.Type) null, "If set, the goal for this challenge is to knock down targets."));
      int num = 0;
      foreach (ChallengeTrophy trophy in this._challenge.trophies)
      {
        SpriteMap image = new SpriteMap("challengeTrophyIcons", 16, 16);
        image.frame = num;
        ++num;
        EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) contextMenu, image: image);
        editorGroupMenu.text = trophy.type.ToString();
        editorGroupMenu.AddItem((ContextMenu) new ContextSlider("Goodies", (IContextListener) null, new FieldBinding((object) trophy, "goodies", -1f, 300f), 1f, "ALL", false, (System.Type) null, "Collect this many items to get this trophy"));
        editorGroupMenu.AddItem((ContextMenu) new ContextSlider("Targets", (IContextListener) null, new FieldBinding((object) trophy, "targets", -1f, 300f), 1f, "ALL", false, (System.Type) null, "Knock down this many targets to get this trophy"));
        editorGroupMenu.AddItem((ContextMenu) new ContextSlider("Time", (IContextListener) null, new FieldBinding((object) trophy, "timeRequirement", max: 600f), 1f, "NONE", true, (System.Type) null, "Complete challenge in this time or less to get this trophy."));
        editorGroupMenu.AddItem((ContextMenu) new ContextSlider("Milis", (IContextListener) null, new FieldBinding((object) trophy, "timeRequirementMilliseconds", max: 99f), 1f, "NONE", true, (System.Type) null, "Fine control over challenge time requirement."));
        contextMenu.AddItem((ContextMenu) editorGroupMenu);
      }
      return (ContextMenu) contextMenu;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("hatIndex", (object) this.hatIndex);
      binaryClassChunk.AddProperty("challengeData", (object) this._challenge.Serialize());
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.hatIndex = node.GetProperty<int>("hatIndex");
      BinaryClassChunk property = node.GetProperty<BinaryClassChunk>("challengeData");
      if (property != null)
      {
        this._challenge = new ChallengeData();
        this._challenge.Deserialize(property);
      }
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "hatIndex", (object) this.hatIndex));
      xelement.Add((object) this._challenge.LegacySerialize());
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "hatIndex");
      if (xelement != null)
        this.hatIndex = Convert.ToInt32(xelement.Value);
      XElement node1 = node.Element((XName) "challengeData");
      if (node1 != null)
      {
        this._challenge = new ChallengeData();
        this._challenge.LegacyDeserialize(node1);
      }
      return true;
    }
  }
}
