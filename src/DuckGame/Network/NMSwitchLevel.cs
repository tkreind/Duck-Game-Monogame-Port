// Decompiled with JetBrains decompiler
// Type: DuckGame.NMSwitchLevel
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.IO;

namespace DuckGame
{
  public class NMSwitchLevel : NMDuckNetworkEvent
  {
    public string level;
    public byte levelIndex;
    public ushort predictionIndex;
    public bool needsChecksum;
    public uint checksum;
    private Level _lev;

    public NMSwitchLevel()
    {
    }

    public NMSwitchLevel(
      string lev,
      byte idx,
      ushort varPredictionIndex,
      bool varNeedsChecksum = false,
      uint varChecksum = 0)
    {
      this.level = lev;
      this.levelIndex = idx;
      this.predictionIndex = varPredictionIndex;
      this.needsChecksum = varNeedsChecksum;
      this.checksum = varChecksum;
    }

    public override bool MessageIsCompleted() => this._lev.initialized;

    public override void Activate()
    {
      DuckNetwork.levelIndex = this.levelIndex;
      GhostManager.context.SetGhostIndex((NetIndex16) (int) this.predictionIndex);
      if (this.level == "@TEAMSELECT")
        this._lev = (Level) new TeamSelect2();
      else if (this.level == "@ROCKINTRO")
        this._lev = (Level) new RockIntro((Level) null);
      else if (this.level == "@ROCKTHROW|SHOWSCORE")
        this._lev = (Level) new RockScoreboard();
      else if (this.level == "@ROCKTHROW|SHOWWINNER")
        this._lev = (Level) new RockScoreboard(mode: ScoreBoardMode.ShowWinner);
      else if (this.level == "@ROCKTHROW|SHOWEND")
      {
        this._lev = (Level) new RockScoreboard(mode: ScoreBoardMode.ShowWinner, afterHighlights: true);
      }
      else
      {
        int seedVal = 0;
        if (this is NMSwitchLevelRandom)
          seedVal = (this as NMSwitchLevelRandom).seed;
        if (this.needsChecksum)
        {
          GameLevel gameLevel = new GameLevel(this.level, seedVal);
          List<LevelData> allLevels = Content.GetAllLevels(this.level);
          LevelData levelData1 = (LevelData) null;
          foreach (LevelData levelData2 in allLevels)
          {
            if ((int) levelData2.GetChecksum() == (int) this.checksum)
            {
              levelData1 = levelData2;
              break;
            }
          }
          if (levelData1 == null)
          {
            ++DuckNetwork.core.levelTransferSession;
            DuckNetwork.core.compressedLevelData = (MemoryStream) null;
            DuckNetwork.core.levelTransferSize = 0;
            DuckNetwork.core.levelTransferProgress = 0;
            Send.Message((NetMessage) new NMClientNeedsLevelData(this.levelIndex, DuckNetwork.core.levelTransferSession), this.connection);
            gameLevel.waitingOnNewData = true;
            gameLevel.networkIndex = this.levelIndex;
          }
          else
            gameLevel.data = levelData1;
          this._lev = (Level) gameLevel;
        }
        else
          this._lev = (Level) new GameLevel(this.level, seedVal);
      }
      switch (Level.current)
      {
        case TeamSelect2 _:
label_26:
          Music.Stop();
          break;
        case RockScoreboard _:
          if (this._lev is RockScoreboard)
            break;
          goto label_26;
      }
      Level.current = this._lev;
    }
  }
}
