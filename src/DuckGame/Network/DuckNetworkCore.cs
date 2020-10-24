// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckNetworkCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class DuckNetworkCore
  {
    public Dictionary<string, XPPair> _xpEarned = new Dictionary<string, XPPair>();
    public int levelTransferSize;
    public int levelTransferProgress;
    public UIMenu xpMenu;
    public UIComponent ducknetUIGroup;
    public List<Profile> profiles = new List<Profile>();
    public int localDuckIndex = -1;
    public int hostDuckIndex = -1;
    public List<NetMessage> pending = new List<NetMessage>();
    public byte levelIndex;
    public MemoryStream compressedLevelData;
    public bool enteringText;
    public string currentEnterText = "";
    public int cursorFlash;
    public ushort chatIndex;
    public ushort levelTransferSession;
    public NetworkConnection localConnection = new NetworkConnection((object) null);
    public DuckNetStatus status;
    public int randomID;
    public DuckNetErrorInfo error;
    public float attemptTimeout;
    public bool inGame;
    public bool stopEnteringText;
    public List<ChatMessage> chatMessages = new List<ChatMessage>();
    public byte migrationIndex = byte.MaxValue;
    public byte migrationSession;

    public void RecreateProfiles()
    {
      this.profiles.Clear();
      for (int index = 0; index < 4; ++index)
        this.profiles.Add(new Profile("Netduck" + (index + 1).ToString(), InputProfile.GetVirtualInput(index), varDefaultPersona: Persona.all.ElementAt<DuckPersona>(index), network: true)
        {
          networkIndex = (byte) index
        });
    }

    public DuckNetworkCore()
    {
      this.RecreateProfiles();
      this.randomID = Rando.Int(2147483646);
    }
  }
}
