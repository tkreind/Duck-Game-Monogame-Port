// Decompiled with JetBrains decompiler
// Type: DuckGame.SequenceCrate
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("special")]
  [BaggedProperty("isInDemo", true)]
  public class SequenceCrate : Crate, ISequenceItem
  {
    public SequenceCrate(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Goody;
      this._editorName = "Seq Crate";
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (this.sequence != null && this.sequence.isValid)
      {
        this.sequence.Finished();
        if (ChallengeLevel.running)
          ++ChallengeLevel.goodiesGot;
      }
      return base.OnDestroy(type);
    }
  }
}
