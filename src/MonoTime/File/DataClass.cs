// Decompiled with JetBrains decompiler
// Type: DuckGame.DataClass
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DuckGame
{
  public class DataClass
  {
    protected string _nodeName = "DataNode";

    private static XElement SerializeDict(string name, IDictionary dict)
    {
      if (dict.Keys.Count <= 0)
        return (XElement) null;
      XElement xelement1 = new XElement((XName) name);
      foreach (object key in (IEnumerable) dict.Keys)
      {
        XElement xelement2 = new XElement((XName) "pair");
        xelement2.Add((object) new XElement((XName) "key", (object) Convert.ToString(key)));
        object content = dict[key];
        xelement2.Add((object) new XElement((XName) "val", content));
        xelement1.Add((object) xelement2);
      }
      return xelement1;
    }

    public static XElement SerializeClass(object o, string nodeName)
    {
      XElement xelement1 = new XElement((XName) nodeName);
      PropertyInfo[] properties = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      FieldInfo[] fields = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      foreach (PropertyInfo propertyInfo in properties)
      {
        if (propertyInfo.PropertyType == typeof (StatBinding))
        {
          StatBinding statBinding = propertyInfo.GetValue(o, (object[]) null) as StatBinding;
          xelement1.Add((object) new XElement((XName) propertyInfo.Name, statBinding.value));
        }
        else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof (Dictionary<,>))
        {
          IDictionary dict = propertyInfo.GetValue(o, (object[]) null) as IDictionary;
          XElement xelement2 = DataClass.SerializeDict(propertyInfo.Name, dict);
          if (xelement2 != null)
            xelement1.Add((object) xelement2);
        }
        else if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof (string)))
          xelement1.Add((object) new XElement((XName) propertyInfo.Name, propertyInfo.GetValue(o, (object[]) null)));
      }
      foreach (FieldInfo fieldInfo in fields)
      {
        if (!fieldInfo.Name.Contains("k__BackingField"))
        {
          if (fieldInfo.FieldType == typeof (StatBinding))
          {
            StatBinding statBinding = fieldInfo.GetValue(o) as StatBinding;
            xelement1.Add((object) new XElement((XName) fieldInfo.Name, statBinding.value));
          }
          else if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof (Dictionary<,>))
          {
            IDictionary dict = fieldInfo.GetValue(o) as IDictionary;
            XElement xelement2 = DataClass.SerializeDict(fieldInfo.Name, dict);
            if (xelement2 != null)
              xelement1.Add((object) xelement2);
          }
          else if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.Equals(typeof (string)))
            xelement1.Add((object) new XElement((XName) fieldInfo.Name, fieldInfo.GetValue(o)));
        }
      }
      return xelement1;
    }

    public static object ReadValue(string value, System.Type t)
    {
      if (t == typeof (string))
        return (object) value;
      if (t == typeof (float))
        return (object) Convert.ToSingle(value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (t == typeof (double))
        return (object) Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (t == typeof (byte))
        return (object) Convert.ToByte(value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (t == typeof (short))
        return (object) Convert.ToInt16(value, (IFormatProvider) CultureInfo.InvariantCulture);
      if (t == typeof (int))
        return (object) Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture);
      return t == typeof (long) ? (object) Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture) : (object) null;
    }

    private static void DeserializeDict(
      IDictionary dict,
      XElement element,
      System.Type keyType,
      System.Type valType)
    {
      dict.Clear();
      foreach (XElement element1 in element.Elements())
      {
        if (element1.Elements().Count<XElement>() == 2)
        {
          object key = DataClass.ReadValue(element1.Elements().ElementAt<XElement>(0).Value, keyType);
          object obj = DataClass.ReadValue(element1.Elements().ElementAt<XElement>(1).Value, valType);
          if (key != null && obj != null)
            dict[key] = obj;
        }
      }
    }

    public static void DeserializeClass(object output, XElement node)
    {
      if (output == null)
        return;
      System.Type type = output.GetType();
      foreach (XElement element in node.Elements())
      {
        try
        {
          PropertyInfo property = type.GetProperty(element.Name.LocalName);
          if (property != (PropertyInfo) null)
          {
            if (property.PropertyType == typeof (StatBinding))
            {
              if (!Steam.IsInitialized())
              {
                if (property.GetValue(output, (object[]) null) is StatBinding statBinding)
                {
                  if (statBinding.isFloat)
                    statBinding.valueFloat = Convert.ToSingle(element.Value);
                  else
                    statBinding.valueInt = Convert.ToInt32(element.Value);
                }
              }
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof (Dictionary<,>))
            {
              System.Type[] genericArguments = property.PropertyType.GetGenericArguments();
              System.Type keyType = genericArguments[0];
              System.Type valType = genericArguments[1];
              DataClass.DeserializeDict(property.GetValue(output, (object[]) null) as IDictionary, element, keyType, valType);
            }
            else
            {
              if (!property.PropertyType.IsPrimitive)
              {
                if (!property.PropertyType.Equals(typeof (string)))
                  continue;
              }
              property.SetValue(output, Convert.ChangeType((object) element.Value, property.PropertyType, (IFormatProvider) CultureInfo.InvariantCulture), (object[]) null);
            }
          }
          else
          {
            FieldInfo field = type.GetField(element.Name.LocalName);
            if (field != (FieldInfo) null)
            {
              if (field.FieldType == typeof (StatBinding))
              {
                if (!Steam.IsInitialized())
                {
                  if (field.GetValue(output) is StatBinding statBinding)
                  {
                    if (statBinding.isFloat)
                      statBinding.valueFloat = Convert.ToSingle(element.Value);
                    else
                      statBinding.valueInt = Convert.ToInt32(element.Value);
                  }
                }
              }
              else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof (Dictionary<,>))
              {
                System.Type[] genericArguments = field.FieldType.GetGenericArguments();
                System.Type keyType = genericArguments[0];
                System.Type valType = genericArguments[1];
                DataClass.DeserializeDict(field.GetValue(output) as IDictionary, element, keyType, valType);
              }
              else
              {
                if (!field.FieldType.IsPrimitive)
                {
                  if (!field.FieldType.Equals(typeof (string)))
                    continue;
                }
                field.SetValue(output, Convert.ChangeType((object) element.Value, field.FieldType, (IFormatProvider) CultureInfo.InvariantCulture));
              }
            }
          }
        }
        catch
        {
          Program.LogLine("Error parsing data value in " + type.ToString() + " (" + (object) element.Name + ")");
        }
      }
    }

    public virtual XElement Serialize() => DataClass.SerializeClass((object) this, this._nodeName);

    public virtual bool Deserialize(XElement node)
    {
      DataClass.DeserializeClass((object) this, node);
      return true;
    }

    public static DataClass operator -(DataClass value1, DataClass value2)
    {
      DataClass instance = Activator.CreateInstance(value1.GetType(), (object[]) null) as DataClass;
      foreach (PropertyInfo property in value1.GetType().GetProperties())
      {
        if (property.PropertyType == typeof (int))
        {
          int num1 = (int) property.GetValue((object) value1, (object[]) null);
          int num2 = (int) property.GetValue((object) value2, (object[]) null);
          property.SetValue((object) instance, (object) (num1 - num2), (object[]) null);
        }
        else if (property.PropertyType == typeof (float))
        {
          float num1 = (float) property.GetValue((object) value1, (object[]) null);
          float num2 = (float) property.GetValue((object) value2, (object[]) null);
          property.SetValue((object) instance, (object) (float) ((double) num1 - (double) num2), (object[]) null);
        }
        else if (property.PropertyType == typeof (DateTime))
        {
          DateTime dateTime1 = (DateTime) property.GetValue((object) value1, (object[]) null);
          DateTime dateTime2 = (DateTime) property.GetValue((object) value2, (object[]) null);
          property.SetValue((object) instance, (object) dateTime2, (object[]) null);
        }
        else
          property.SetValue((object) instance, property.GetValue((object) value2, (object[]) null), (object[]) null);
      }
      return instance;
    }

    public static DataClass operator +(DataClass value1, DataClass value2)
    {
      DataClass instance = Activator.CreateInstance(value1.GetType(), (object[]) null) as DataClass;
      foreach (PropertyInfo property in value1.GetType().GetProperties())
      {
        if (property.PropertyType == typeof (int))
        {
          int num1 = (int) property.GetValue((object) value1, (object[]) null);
          int num2 = (int) property.GetValue((object) value2, (object[]) null);
          property.SetValue((object) instance, (object) (num1 + num2), (object[]) null);
        }
        else if (property.PropertyType == typeof (float))
        {
          float num1 = (float) property.GetValue((object) value1, (object[]) null);
          float num2 = (float) property.GetValue((object) value2, (object[]) null);
          property.SetValue((object) instance, (object) (float) ((double) num1 + (double) num2), (object[]) null);
        }
        else if (property.PropertyType == typeof (DateTime))
        {
          DateTime dateTime1 = (DateTime) property.GetValue((object) value1, (object[]) null);
          DateTime dateTime2 = (DateTime) property.GetValue((object) value2, (object[]) null);
          if (dateTime1 > dateTime2)
            property.SetValue((object) instance, (object) dateTime1, (object[]) null);
          else
            property.SetValue((object) instance, (object) dateTime2, (object[]) null);
        }
        else
          property.SetValue((object) instance, property.GetValue((object) value2, (object[]) null), (object[]) null);
      }
      return instance;
    }
  }
}
