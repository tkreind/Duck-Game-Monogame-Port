// Decompiled with JetBrains decompiler
// Type: DuckGame.Vote
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Vote
  {
    private static List<RegisteredVote> _votes = new List<RegisteredVote>();
    private static string _voteButton = "";
    private static bool _votingOpen = false;

    public static void OpenVoting(string voteMessage, string voteButton, bool openCorners = true)
    {
      if (openCorners)
      {
        Vote._voteButton = voteButton;
        HUD.CloseAllCorners();
        HUD.AddCornerControl(HUDCorner.BottomRight, "@" + voteButton + "@" + voteMessage);
      }
      Vote._votingOpen = true;
    }

    public static RegisteredVote GetVote(Profile who) => Vote._votes.FirstOrDefault<RegisteredVote>((Func<RegisteredVote, bool>) (x => x.who == who));

    public static void RegisterVote(Profile who, VoteType vote)
    {
      if (!Vote._votingOpen && Network.isActive)
        return;
      RegisteredVote registeredVote = Vote._votes.FirstOrDefault<RegisteredVote>((Func<RegisteredVote, bool>) (x => x.who == who));
      if (registeredVote == null)
        Vote._votes.Add(new RegisteredVote()
        {
          who = who,
          vote = vote
        });
      else
        registeredVote.wobble = 1f;
    }

    public static void CloseVoting()
    {
      foreach (RegisteredVote vote in Vote._votes)
        vote.doClose = true;
      Vote._voteButton = "";
      Vote._votingOpen = false;
    }

    public static void ClearVotes() => Vote._votes.Clear();

    public static bool Passed(VoteType type)
    {
      int num = 0;
      foreach (RegisteredVote vote in Vote._votes)
      {
        if (vote.open && vote.vote == type)
          ++num;
      }
      return Profiles.all.Where<Profile>((Func<Profile, bool>) (x => x.team != null)).Count<Profile>() == num;
    }

    public static void Update()
    {
      if (Vote._voteButton != "")
      {
        foreach (Profile who in Profiles.all.Where<Profile>((Func<Profile, bool>) (x => x.team != null)))
        {
          if (who.inputProfile != null && who.inputProfile.Pressed(Vote._voteButton))
            Vote.RegisterVote(who, VoteType.Skip);
        }
      }
      if (!Vote._votes.Exists((Predicate<RegisteredVote>) (x => x.open && (double) x.slide < 0.899999976158142)))
      {
        foreach (RegisteredVote vote in Vote._votes)
        {
          if (vote.doClose)
            vote.open = false;
        }
      }
      foreach (RegisteredVote vote in Vote._votes)
      {
        vote.slide = Lerp.FloatSmooth(vote.slide, vote.open ? 1f : -0.1f, 0.1f, 1.1f);
        vote.wobble = Lerp.Float(vote.wobble, 0.0f, 0.05f);
        vote.wobbleInc += 0.5f;
      }
      Vote._votes.RemoveAll((Predicate<RegisteredVote>) (x => !x.open && (double) x.slide < 0.00999999977648258));
    }

    public static void Draw()
    {
      int num1 = 0;
      foreach (RegisteredVote vote in Vote._votes)
      {
        if (vote.vote == VoteType.Skip)
        {
          float num2 = (float) (Math.Sin((double) vote.wobbleInc) * (double) vote.wobble * 3.0);
          Vec2 vec2 = Network.isActive ? vote.leftStick : vote.who.inputProfile.leftStick;
          vote.who.persona.skipSprite.angle = (float) ((double) num2 * 0.0299999993294477 + (double) vec2.y * 0.400000005960464);
          vote.who.persona.skipSprite.frame = 0;
          Graphics.Draw((Sprite) vote.who.persona.skipSprite, (float) ((double) Layer.HUD.width + 49.0 - (double) vote.slide * 48.0 + (double) vec2.x * 3.0), (float) ((double) Layer.HUD.height - 28.0 - (double) (num1 * 16) - (double) vec2.y * 3.0), new Depth(0.9f));
          vote.who.persona.skipSprite.frame = 1;
          Vec2 p2 = Network.isActive ? vote.rightStick : vote.who.inputProfile.rightStick;
          vote.who.persona.skipSprite.angle = num2 * 0.03f + Maths.DegToRad(Maths.PointDirection(Vec2.Zero, p2) - 180f);
          Graphics.Draw((Sprite) vote.who.persona.skipSprite, (float) ((double) Layer.HUD.width + 68.0 - (double) vote.slide * 48.0 + (double) p2.x * 20.0), (float) ((double) Layer.HUD.height - 32.0 - (double) (num1 * 16) - (double) p2.y * 20.0), new Depth(0.9f));
        }
        ++num1;
      }
    }
  }
}
