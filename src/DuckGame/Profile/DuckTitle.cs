// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckTitle
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DuckGame
{
  public class DuckTitle
  {
    private static List<DuckTitle> _titles = new List<DuckTitle>();
    private Dictionary<PropertyInfo, float> _requirementsFloat = new Dictionary<PropertyInfo, float>();
    private Dictionary<PropertyInfo, int> _requirementsInt = new Dictionary<PropertyInfo, int>();
    private Dictionary<PropertyInfo, string> _requirementsString = new Dictionary<PropertyInfo, string>();
    private string _name;
    private string _previousOwner;

    public static void Initialize()
    {
      foreach (string file in Content.GetFiles("Content/titles"))
      {
        IEnumerable<XElement> source = XDocument.Load(TitleContainer.OpenStream(file)).Elements((XName) "Title");
        if (source != null)
        {
          XAttribute xattribute1 = source.Attributes((XName) "name").FirstOrDefault<XAttribute>();
          if (xattribute1 != null)
          {
            DuckTitle duckTitle = new DuckTitle();
            duckTitle._name = xattribute1.Value;
            bool flag = false;
            foreach (XElement element in source.Elements<XElement>())
            {
              if (element.Name == (XName) "StatRequirement")
              {
                XAttribute statNameAttrib = element.Attributes((XName) "name").FirstOrDefault<XAttribute>();
                XAttribute xattribute2 = element.Attributes((XName) "value").FirstOrDefault<XAttribute>();
                if (statNameAttrib != null && xattribute2 != null)
                {
                  PropertyInfo key = ((IEnumerable<PropertyInfo>) typeof (ProfileStats).GetProperties()).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.Name == statNameAttrib.Value));
                  if (key != (PropertyInfo) null)
                  {
                    if (key.GetType() == typeof (float))
                      duckTitle._requirementsFloat.Add(key, Change.ToSingle((object) xattribute2.Value));
                    else if (key.GetType() == typeof (int))
                      duckTitle._requirementsFloat.Add(key, (float) Convert.ToInt32(xattribute2.Value));
                    else
                      duckTitle._requirementsString.Add(key, xattribute2.Value);
                  }
                  else
                  {
                    flag = true;
                    break;
                  }
                }
                else
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
              DuckTitle._titles.Add(duckTitle);
          }
        }
      }
    }

    public static DuckTitle GetTitle(string title) => DuckTitle._titles.FirstOrDefault<DuckTitle>((Func<DuckTitle, bool>) (x => x._name == title));

    public string previousOwner
    {
      get => this._previousOwner;
      set => this._previousOwner = value;
    }

    public static Dictionary<DuckTitle, float> ScoreTowardsTitles(Profile p)
    {
      Dictionary<DuckTitle, float> dictionary = new Dictionary<DuckTitle, float>();
      foreach (DuckTitle title in DuckTitle._titles)
        dictionary[title] = title.ScoreTowardsTitle(p);
      return dictionary;
    }

    public float ScoreTowardsTitle(Profile p)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      foreach (KeyValuePair<PropertyInfo, float> keyValuePair in this._requirementsFloat)
      {
        num1 += keyValuePair.Value;
        num2 += (float) keyValuePair.Key.GetValue((object) p.stats, (object[]) null);
      }
      int num3 = 0;
      int num4 = 0;
      foreach (KeyValuePair<PropertyInfo, int> keyValuePair in this._requirementsInt)
      {
        num3 += keyValuePair.Value;
        num4 += (int) keyValuePair.Key.GetValue((object) p.stats, (object[]) null);
      }
      int num5 = 0;
      int num6 = 0;
      foreach (KeyValuePair<PropertyInfo, string> keyValuePair in this._requirementsString)
      {
        ++num5;
        if ((string) keyValuePair.Key.GetValue((object) p.stats, (object[]) null) == keyValuePair.Value)
          ++num6;
      }
      return (float) (((double) num2 + (double) num4 + (double) num6) / ((double) num1 + (double) num3 + (double) num5));
    }

    public void UpdateTitles()
    {
      foreach (DuckTitle title in DuckTitle._titles)
        ;
    }
  }
}
