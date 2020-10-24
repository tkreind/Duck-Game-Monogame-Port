// Decompiled with JetBrains decompiler
// Type: DuckGame.News
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
  public class News
  {
    private static List<NewsStory> _availableStories = new List<NewsStory>();

    public static void Initialize()
    {
      System.Type newsType = typeof (NewsStory);
      foreach (System.Type type in ((IEnumerable<System.Type>) Assembly.GetAssembly(typeof (NewsStory)).GetTypes()).Where<System.Type>((Func<System.Type, bool>) (t => newsType.IsAssignableFrom(t))))
        News._availableStories.Add(Activator.CreateInstance(type) as NewsStory);
    }

    public static List<NewsStory> GetStories()
    {
      Stats.CalculateStats();
      List<Team> active = Teams.active;
      List<NewsStory> stories = new List<NewsStory>();
      foreach (NewsStory availableStorey in News._availableStories)
      {
        availableStorey.DoCalculate(active);
        stories.Add(availableStorey);
      }
      News.FilterBest(stories, NewsSection.MatchComments, 1);
      News.FilterBest(stories, NewsSection.PlayerComments, 2);
      stories.Sort((Comparison<NewsStory>) ((a, b) =>
      {
        if (a.section == b.section)
          return 0;
        return a.section >= b.section ? 1 : -1;
      }));
      return stories;
    }

    public static void FilterBest(List<NewsStory> stories, NewsSection section, int numToPick)
    {
      List<NewsStory> list = stories.Where<NewsStory>((Func<NewsStory, bool>) (x => x.section == section)).ToList<NewsStory>();
      list.OrderBy<NewsStory, float>((Func<NewsStory, float>) (x => x.weight * x.importance));
      int num = 0;
      foreach (NewsStory newsStory in list)
      {
        if (num >= numToPick)
          stories.Remove(newsStory);
        ++num;
      }
    }
  }
}
