// Decompiled with JetBrains decompiler
// Type: DuckGame.Serializable
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Reflection;
using System.Xml.Linq;

namespace DuckGame
{
  public class Serializable
  {
    public void SerializeField(BinaryClassChunk element, string name)
    {
      ClassMember member = Editor.GetMember(this.GetType(), name);
      if (member == null)
        return;
      element.AddProperty(name, member.GetValue((object) this));
    }

    public void DeserializeField(BinaryClassChunk node, string name) => Editor.GetMember(this.GetType(), name)?.SetValue((object) this, node.GetProperty(name));

    public void LegacySerializeField(XElement element, string name)
    {
      FieldInfo field = this.GetType().GetField(name);
      object name1;
      if (field != (FieldInfo) null)
      {
        name1 = field.GetValue((object) this);
      }
      else
      {
        PropertyInfo property = this.GetType().GetProperty(name);
        if (!(property != (PropertyInfo) null))
          return;
        name1 = property.GetValue((object) this, (object[]) null);
      }
      if (name1.GetType().IsEnum)
        name1 = (object) Enum.GetName(name1.GetType(), name1);
      element.Add((object) new XElement((XName) name, name1));
    }

    public void LegacyDeserializeField(XElement node, string name)
    {
      try
      {
        XElement xelement = node.Element((XName) name);
        if (xelement == null)
          return;
        FieldInfo field = this.GetType().GetField(name);
        if (field != (FieldInfo) null)
        {
          if (field.FieldType.IsEnum)
            field.SetValue((object) this, Enum.Parse(field.FieldType, xelement.Value));
          else
            field.SetValue((object) this, Convert.ChangeType((object) xelement.Value, field.FieldType));
        }
        else
        {
          PropertyInfo property = this.GetType().GetProperty(name);
          if (!(property != (PropertyInfo) null))
            return;
          if (property.PropertyType.IsEnum)
            property.SetValue((object) this, Enum.Parse(property.PropertyType, xelement.Value), (object[]) null);
          else
            property.SetValue((object) this, Convert.ChangeType((object) xelement.Value, property.PropertyType), (object[]) null);
        }
      }
      catch
      {
      }
    }
  }
}
