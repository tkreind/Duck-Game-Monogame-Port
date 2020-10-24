// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamsCore
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame
{
  public class TeamsCore
  {
    public Dictionary<ulong, Team> _facadeMap = new Dictionary<ulong, Team>();
    public List<Team> teams;
    public Team nullTeam = new Team("???", "hats/cluehat");
    public List<Team> extraTeams = new List<Team>();
    public SpriteMap hats;

    public Team Player1 => this.teams[0];

    public Team Player2 => this.teams[1];

    public Team Player3 => this.teams[2];

    public Team Player4 => this.teams[3];

    public int numTeams => this.teams.Count;

    public List<Team> all
    {
      get
      {
        List<Team> teamList = new List<Team>((IEnumerable<Team>) this.teams);
        if (!Network.isActive)
        {
          teamList.AddRange((IEnumerable<Team>) this.extraTeams);
        }
        else
        {
          foreach (Team extraTeam in this.extraTeams)
          {
            if (extraTeam.capeRequestSuccess)
              teamList.Add(extraTeam);
          }
        }
        return teamList;
      }
    }

    public List<Team> allStock => new List<Team>((IEnumerable<Team>) this.teams);

    public void Initialize()
    {
      this.hats = new SpriteMap("hatCollection", 32, 32);
      this.hats.center = new Vec2(16f, 16f);
      this.teams = new List<Team>()
      {
        new Team("Player 1", "hats/noHat", true),
        new Team("Player 2", "hats/noHat", true),
        new Team("Player 3", "hats/noHat", true),
        new Team("Player 4", "hats/noHat", true),
        new Team("Sombreros", "hats/sombrero", true),
        new Team("Dappers", "hats/dapper", true),
        new Team("Dicks", "hats/dicks", true),
        new Team("Frank", "hats/frank", lockd: true),
        new Team("DUCKS", "hats/reallife", lockd: true),
        new Team("Frogs?", "hats/frogs", true),
        new Team("Drunks", "hats/drunks"),
        new Team("Joey", "hats/joey", lockd: true),
        new Team("BALLZ", "hats/ballhead"),
        new Team("Agents", "hats/agents"),
        new Team("Sailors", "hats/sailors"),
        new Team("astropal", "hats/astrobud", lockd: true),
        new Team("Cowboys", "hats/cowboys", lockd: true),
        new Team("Pulpy", "hats/pulpy", lockd: true),
        new Team("SKULLY", "hats/skelly", lockd: true),
        new Team("Hearts", "hats/hearts"),
        new Team("LOCKED", "hats/locked"),
        new Team("Jazzducks", "hats/jazzducks", false, false, new Vec2(-2f, -7f)),
        new Team("Divers", "hats/divers"),
        new Team("Uglies", "hats/uglies"),
        new Team("Dinos", "hats/dinos"),
        new Team("Caps", "hats/caps"),
        new Team("Burgers", "hats/burgers"),
        new Team("Turing", "hats/turing", true),
        new Team("Retro", "hats/retros"),
        new Team("Senpai", "hats/sensei"),
        new Team("BAWB", "hats/bawb", false, true, new Vec2(-1f, -10f)),
        new Team("SWACK", "hats/guac", true, true),
        new Team("eggpal", "hats/eggy", lockd: true),
        new Team("Valet", "hats/valet"),
        new Team("Pilots", "hats/pilots"),
        new Team("Cyborgs", "hats/cyborgs"),
        new Team("Tubes", "hats/tube", false, false, new Vec2(-1f, 0.0f)),
        new Team("Gents", "hats/gents"),
        new Team("Potheads", "hats/pots"),
        new Team("Skis", "hats/ski"),
        new Team("Fridges", "hats/fridge"),
        new Team("Witchtime", "hats/witchtime"),
        new Team("Wizards", "hats/wizbiz"),
        new Team("FUNNYMAN", "hats/FunnyMan"),
        new Team("Pumpkins", "hats/Dumplin"),
        new Team("CAPTAIN", "hats/devhat", lockd: true),
        new Team("BRICK", "hats/brick", lockd: true),
        new Team("Pompadour", "hats/pompadour"),
        new Team("Super", "hats/super"),
        new Team("Chancy", "hats/chancy", lockd: true),
        new Team("Log", "hats/log"),
        new Team("Meeee", "hats/toomany", lockd: true),
        new Team("BRODUCK", "hats/broduck", lockd: true),
        new Team("brad", "hats/handy", lockd: true),
        new Team("eyebob", "hats/gross"),
        new Team("tubes", "hats/tube", false, false, new Vec2(-1f, 0.0f)),
        new Team("gents", "hats/gents"),
        new Team("pots", "hats/pots"),
        new Team("poles", "hats/ski"),
        new Team("CYCLOPS", "hats/cyclops", lockd: true, desc: "These wounds they will not heal."),
        new Team("MOTHERS", "hats/motherduck", lockd: true, desc: "Not a goose."),
        new Team("BIG ROBO", "hats/newrobo", false, true, new Vec2()),
        new Team("TINCAN", "hats/oldrobo", false, true, new Vec2()),
        new Team("WELDERS", "hats/WELDER", lockd: true, desc: "Safety has never looked so cool."),
        new Team("PONYCAP", "hats/ponycap", false, true, new Vec2()),
        new Team("TRICORNE", "hats/tricorne", lockd: true, desc: "We fight for freedom!"),
        new Team("TWINTAIL", "hats/twintail", lockd: true, desc: "Two tails are better than one."),
        new Team("MAJESTY", "hats/royalty", lockd: true, capeTex: ((Texture2D) Content.Load<Tex2D>("hats/royalCape"))),
        new Team("MOONWALK", "hats/moonwalker", lockd: true, capeTex: ((Texture2D) Content.Load<Tex2D>("hats/moonCape")))
      };
    }
  }
}
