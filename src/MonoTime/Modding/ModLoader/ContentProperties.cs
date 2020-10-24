﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.ContentProperties
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  /// <summary>
  /// A class for retrieving property bags associated with Types.
  /// </summary>
  public static class ContentProperties
  {
    internal static bool EditingAllowed = true;
    private static readonly Dictionary<System.Type, PropertyBag> _propertyBags = new Dictionary<System.Type, PropertyBag>();
    private static readonly ContentProperties.EmptyBag _emptyBag = new ContentProperties.EmptyBag();

    /// <summary>Initializes the bag of a single type.</summary>
    /// <param name="type">The type.</param>
    internal static void InitializeBag(System.Type type)
    {
      PropertyBag propertyBag = new PropertyBag();
      foreach (BaggedPropertyAttribute propertyAttribute in ((IEnumerable<object>) type.GetCustomAttributes(typeof (BaggedPropertyAttribute), true)).Select<object, BaggedPropertyAttribute>((Func<object, BaggedPropertyAttribute>) (attrib => (BaggedPropertyAttribute) attrib)).Reverse<BaggedPropertyAttribute>())
        propertyBag.Set<object>(propertyAttribute.Property, propertyAttribute.Value);
      ContentProperties._propertyBags[type] = propertyBag;
    }

    /// <summary>Initializes the bags of multiple types.</summary>
    /// <param name="types">The types.</param>
    internal static void InitializeBags(IEnumerable<System.Type> types)
    {
      foreach (System.Type type in types)
        ContentProperties.InitializeBag(type);
    }

    /// <summary>
    /// Gets a read-only property bag associated with the type.
    /// </summary>
    /// <param name="t">The type to get the bag from.</param>
    /// <returns>The property bag</returns>
    public static IReadOnlyPropertyBag GetBag(System.Type t)
    {
      PropertyBag propertyBag;
      return ContentProperties._propertyBags.TryGetValue(t, out propertyBag) ? (IReadOnlyPropertyBag) propertyBag : (IReadOnlyPropertyBag) ContentProperties._emptyBag;
    }

    /// <summary>
    /// Gets a read-only property bag associated with the type.
    /// </summary>
    /// <typeparam name="T">The type to get the bag from</typeparam>
    /// <returns>The property bag</returns>
    public static IReadOnlyPropertyBag GetBag<T>() => ContentProperties.GetBag(typeof (T));

    public class EmptyBag : IReadOnlyPropertyBag
    {
      public IEnumerable<string> Properties => Enumerable.Empty<string>();

      public bool Contains(string property) => false;

      public object Get(string property) => throw new PropertyNotFoundException();

      public T Get<T>(string property) => throw new PropertyNotFoundException();

      public T GetOrDefault<T>(string property, T defaultValue) => defaultValue;

      public bool? IsOfType<T>(string property) => new bool?();

      public T? TryGet<T>(string property) where T : struct => new T?();

      public bool TryGet<T>(string property, out T value)
      {
        value = default (T);
        return false;
      }

      public System.Type TypeOf(string property) => (System.Type) null;
    }
  }
}
