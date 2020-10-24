// Decompiled with JetBrains decompiler
// Type: DuckGame.Team
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace DuckGame
{
  public class Team
  {
    public Texture2D _capeTexture;
    public bool capeRequestSuccess;
    private string _name = "";
    private string _description = "";
    private SpriteMap _hat;
    private int _score;
    private int _rockScore;
    private int _wins;
    private int _prevScoreboardScore;
    private Vec2 _hatOffset;
    public bool inDemo;
    private List<Profile> _activeProfiles = new List<Profile>();
    public string hatID;
    private byte[] _customData;
    public Vec2 prevTreeDraw = Vec2.Zero;
    private bool _locked;

    public static void MapFacade(ulong steamID, Team t) => Teams.core._facadeMap[steamID] = t;

    public static void ClearFacade(ulong steamID)
    {
      if (!Teams.core._facadeMap.ContainsKey(steamID))
        return;
      Teams.core._facadeMap.Remove(steamID);
    }

    public static void ClearFacades() => Teams.core._facadeMap.Clear();

    private static byte[] ReadByteArray(Stream s)
    {
      byte[] buffer1 = new byte[4];
      if (s.Read(buffer1, 0, buffer1.Length) != buffer1.Length)
        throw new SystemException("Stream did not contain properly formatted byte array");
      byte[] buffer2 = new byte[BitConverter.ToInt32(buffer1, 0)];
      if (s.Read(buffer2, 0, buffer2.Length) != buffer2.Length)
        throw new SystemException("Did not read byte array properly");
      return buffer2;
    }

    public static Team Deserialize(string file) => File.Exists(file) ? Team.Deserialize(File.ReadAllBytes(file)) : (Team) null;

    public static Team Deserialize(byte[] teamData)
    {
      try
      {
        if (teamData == null)
          return (Team) null;
        MemoryStream memoryStream = new MemoryStream(teamData);
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        byte[] numArray1 = new byte[16]
        {
          (byte) 243,
          (byte) 22,
          (byte) 152,
          (byte) 32,
          (byte) 1,
          (byte) 244,
          (byte) 122,
          (byte) 111,
          (byte) 97,
          (byte) 42,
          (byte) 13,
          (byte) 2,
          (byte) 19,
          (byte) 15,
          (byte) 45,
          (byte) 230
        };
        rijndaelManaged.Key = numArray1;
        rijndaelManaged.IV = Team.ReadByteArray((Stream) memoryStream);
        BinaryReader binaryReader = new BinaryReader((Stream) new CryptoStream((Stream) memoryStream, rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV), CryptoStreamMode.Read));
        long num1 = binaryReader.ReadInt64();
        switch (num1)
        {
          case 402965919293045:
          case 465665919293045:
          case 630430777029345:
          case 630449177029345:
            if (num1 == 630449177029345L || num1 == 465665919293045L)
              binaryReader.ReadString();
            string varName = binaryReader.ReadString();
            int count = binaryReader.ReadInt32();
            byte[] numArray2 = binaryReader.ReadBytes(count);
            Texture2D hatTexture = Texture2D.FromStream(DuckGame.Graphics.device, (Stream) new MemoryStream(numArray2));
            Color[] data1 = new Color[hatTexture.Width * hatTexture.Height];
            hatTexture.GetData<Color>(data1);
            for (int index = 0; index < data1.Length; ++index)
            {
              if (data1[index].r == byte.MaxValue && data1[index].g == (byte) 0 && data1[index].b == byte.MaxValue)
                data1[index] = new Color(0, 0, 0, 0);
            }
            string str = CRC32.Generate(numArray2).ToString();
            Texture2D texture2D = (Texture2D) null;
            bool flag = true;
            if ((num1 == 630430777029345L || num1 == 630449177029345L) && (hatTexture.Height == 32 && hatTexture.Width == 96))
            {
              texture2D = new Texture2D(DuckGame.Graphics.device, 32, 32);
              Color[] data2 = new Color[1024];
              int num2 = 64;
              for (int index1 = 0; index1 < 32; ++index1)
              {
                for (int index2 = 0; index2 < 32; ++index2)
                  data2[index2 + index1 * 32] = data1[num2 + index2 + index1 * hatTexture.Width];
              }
              texture2D.SetData<Color>(data2);
              flag = true;
            }
            hatTexture.SetData<Color>(data1);
            binaryReader.Close();
            return new Team(varName, hatTexture)
            {
              hatID = str,
              customData = teamData,
              _capeTexture = texture2D,
              capeRequestSuccess = flag
            };
          default:
            return (Team) null;
        }
      }
      catch
      {
        return (Team) null;
      }
    }

    public Texture2D capeTexture
    {
      get
      {
        if (Network.isActive)
        {
          int index = Teams.all.IndexOf(this);
          if (index != -1 && index < 4)
          {
            Profile profile = DuckNetwork.profiles[index];
            Team team = (Team) null;
            if (Teams.core._facadeMap.TryGetValue(profile.steamID, out team))
              return team._capeTexture;
          }
        }
        return this._capeTexture;
      }
    }

    public string name => this._name;

    public string description => this._description;

    public string currentDisplayName => this.activeProfiles.Count <= 1 ? (!Profiles.IsDefault(this.activeProfiles[0]) ? this.activeProfiles[0].name : this.name) : this.name;

    public SpriteMap hat
    {
      get
      {
        if (Network.isActive)
        {
          int index = Teams.all.IndexOf(this);
          if (index != -1 && index < 4)
          {
            Profile profile = DuckNetwork.profiles[index];
            Team team = (Team) null;
            if (Teams.core._facadeMap.TryGetValue(profile.steamID, out team))
              return team.hat;
          }
        }
        return this._hat;
      }
    }

    public bool hasHat => this.hat != null && this.hat.texture.textureName != "hats/noHat";

    public int score
    {
      get => this._score;
      set => this._score = value;
    }

    public int rockScore
    {
      get => this._rockScore;
      set => this._rockScore = value;
    }

    public int wins
    {
      get => this._wins;
      set => this._wins = value;
    }

    public int prevScoreboardScore
    {
      get => this._prevScoreboardScore;
      set => this._prevScoreboardScore = value;
    }

    public Vec2 hatOffset => this._hatOffset;

    public List<Profile> activeProfiles => this._activeProfiles;

    public int numMembers => this._activeProfiles.Count;

    public void Join(Profile prof, bool set = true)
    {
      if (this._activeProfiles.Contains(prof))
        return;
      if (prof.team != null)
        prof.team.Leave(prof);
      this._activeProfiles.Add(prof);
      if (!set)
        return;
      prof.team = this;
    }

    public void Leave(Profile prof, bool set = true)
    {
      this._activeProfiles.Remove(prof);
      if (!set)
        return;
      prof.team = (Team) null;
    }

    public void ClearProfiles()
    {
      foreach (Profile prof in new List<Profile>((IEnumerable<Profile>) this._activeProfiles))
        this.Leave(prof);
      this._activeProfiles.Clear();
    }

    public void ResetTeam() => this._score = 0;

    public byte[] customData
    {
      get
      {
        if (Network.isActive)
        {
          int index = Teams.all.IndexOf(this);
          if (index != -1 && index < 4)
          {
            Profile profile = DuckNetwork.profiles[index];
            Team team = (Team) null;
            if (Teams.core._facadeMap.TryGetValue(profile.steamID, out team))
              return team.customData;
          }
        }
        return this._customData;
      }
      set => this._customData = value;
    }

    public bool locked
    {
      get => this._locked;
      set => this._locked = value;
    }

    public Team(
      string varName,
      string hatTexture,
      bool demo = false,
      bool lockd = false,
      Vec2 hatOff = default (Vec2),
      string desc = "",
      Texture2D capeTex = null)
    {
      this._name = varName;
      this._hat = new SpriteMap(hatTexture, 32, 32);
      this._hatOffset = hatOff;
      this.inDemo = demo;
      this._locked = lockd;
      this._description = desc;
      this._capeTexture = capeTex;
    }

    public Team(string varName, string hatTexture, bool demo, bool lockd, Vec2 hatOff)
    {
      this._name = varName;
      this._hat = new SpriteMap(hatTexture, 32, 32);
      this._hatOffset = hatOff;
      this.inDemo = demo;
      this._locked = lockd;
    }

    public Team(
      string varName,
      Texture2D hatTexture,
      bool demo = false,
      bool lockd = false,
      Vec2 hatOff = default (Vec2),
      string desc = "")
    {
      this._name = varName;
      this._hat = new SpriteMap((Tex2D) hatTexture, 32, 32);
      this._hatOffset = hatOff;
      this.inDemo = demo;
      this._locked = lockd;
      this._description = desc;
    }

    public Team(string varName, Texture2D hatTexture, bool demo, bool lockd, Vec2 hatOff)
    {
      this._name = varName;
      this._hat = new SpriteMap((Tex2D) hatTexture, 32, 32);
      this._hatOffset = hatOff;
      this.inDemo = demo;
      this._locked = lockd;
    }
  }
}
