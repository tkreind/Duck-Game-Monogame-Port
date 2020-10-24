﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.BinaryClassChunk
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections;
using System.Collections.Generic;

namespace DuckGame
{
  public class BinaryClassChunk
  {
    private long _magicNumber;
    private ushort _version;
    private uint _size;
    private uint _offset;
    private uint _checksum;
    private BitBuffer _data;
    private BitBuffer _serializedData;
    private Dictionary<string, BinaryClassChunk> _headerDictionary = new Dictionary<string, BinaryClassChunk>();
    private MultiMap<string, object> _extraProperties;
    private DeserializeResult _result;
    private Exception _exception;

    public BitBuffer GetData()
    {
      if (this._data == null && this._serializedData == null)
        this._serializedData = this.Serialize();
      return this._serializedData == null ? this._data : this._serializedData;
    }

    public Exception GetException() => this._exception;

    public DeserializeResult GetResult() => this._result;

    public uint GetChecksum()
    {
      if (this._data == null && this._serializedData == null)
        this._serializedData = this.Serialize();
      if (this._checksum == 0U)
        this._checksum = Editor.Checksum(this.GetData().buffer, (int) this._offset, (int) this._size);
      return this._checksum;
    }

    public void AddProperty(string id, object value)
    {
      if (this._extraProperties == null)
        this._extraProperties = new MultiMap<string, object>();
      this._extraProperties.Add(id, value);
    }

    public T GetProperty<T>(string id)
    {
      object property = this.GetProperty(id);
      return property == null ? default (T) : (T) property;
    }

    public List<T> GetProperties<T>(string id)
    {
      List<object> property = this.GetProperty(id, true) as List<object>;
      List<T> objList = new List<T>();
      foreach (object obj in property)
        objList.Add((T) obj);
      return objList;
    }

    public object GetProperty(string id, bool multiple = false)
    {
      if (this._extraProperties == null)
        return (object) null;
      List<object> list = (List<object>) null;
      this._extraProperties.TryGetValue(id, out list);
      if (list != null)
      {
        foreach (object obj in list)
        {
          if (obj is BinaryClassChunk)
          {
            BinaryClassChunk binaryClassChunk = obj as BinaryClassChunk;
            if (binaryClassChunk._result == DeserializeResult.HeaderDeserialized)
              binaryClassChunk.Deserialize();
          }
          if (obj is BitBuffer)
            (obj as BitBuffer).SeekToStart();
          if (!multiple)
            return obj;
        }
      }
      return (object) list;
    }

    public void SetData(BitBuffer data)
    {
      BinaryClassChunk.DeserializeHeader(this.GetType(), data, this);
      if (this._result != DeserializeResult.HeaderDeserialized)
        return;
      this.Deserialize();
    }

    public static T FromData<T>(BitBuffer data) where T : BinaryClassChunk
    {
      BinaryClassChunk instance = Activator.CreateInstance(typeof (T), (object[]) null) as BinaryClassChunk;
      instance.SetData(data);
      return (T) instance;
    }

    private Array DeserializeArray(System.Type type, System.Type arrayType, BitBuffer data)
    {
      int num = this._data.ReadInt();
      Array instance = Activator.CreateInstance(type, (object) num) as Array;
      for (int index = 0; index < num; ++index)
      {
        bool flag = this._data.ReadBool();
        object obj = (object) null;
        if (flag)
        {
          if (typeof (BinaryClassChunk).IsAssignableFrom(arrayType))
          {
            BinaryClassChunk binaryClassChunk = BinaryClassChunk.DeserializeHeader(arrayType, this._data, root: false, skipData: true);
            binaryClassChunk?.Deserialize();
            obj = (object) binaryClassChunk;
          }
          else
            obj = this._data.Read(arrayType);
        }
        instance.SetValue(obj, index);
      }
      return instance;
    }

    public bool Deserialize()
    {
      if (this._data == null)
      {
        this._result = DeserializeResult.NoData;
        return false;
      }
      if (this._result == DeserializeResult.Success)
        return true;
      try
      {
        this._data.position = (int) this._offset;
        ushort num1 = this._data.ReadUShort();
        for (int index = 0; index < (int) num1; ++index)
        {
          string str = this._data.ReadString();
          ClassMember classMember = (ClassMember) null;
          System.Type key = (System.Type) null;
          if (str.StartsWith("@"))
          {
            if (this._extraProperties == null)
              this._extraProperties = new MultiMap<string, object>();
            byte num2 = this._data.ReadByte();
            if (((int) num2 & 1) != 0)
            {
              byte num3 = (byte) ((uint) num2 >> 1);
              BinaryClassMember.typeMap.TryGetKey(num3, out key);
            }
            else
              key = Editor.GetType(this._data.ReadString());
            str = str.Substring(1, str.Length - 1);
          }
          else
          {
            classMember = Editor.GetMember(this.GetType(), str);
            if (classMember != null)
              key = classMember.type;
          }
          uint num4 = this._data.ReadUInt();
          if (key != (System.Type) null)
          {
            int position = this._data.position;
            if (typeof (BinaryClassChunk).IsAssignableFrom(key))
            {
              BinaryClassChunk binaryClassChunk = BinaryClassChunk.DeserializeHeader(key, this._data, root: false);
              if (classMember == null)
                this._extraProperties.Add(str, (object) binaryClassChunk);
              else
                this._headerDictionary[str] = binaryClassChunk;
              this._data.position = position + (int) num4;
            }
            else if (key.IsArray)
            {
              Array array = this.DeserializeArray(key, key.GetElementType(), this._data);
              if (classMember == null)
                this._extraProperties.Add(str, (object) array);
              else
                classMember.SetValue((object) this, (object) array);
            }
            else if (key.IsGenericType && key.GetGenericTypeDefinition() == typeof (List<>))
            {
              Array array = this.DeserializeArray(typeof (object[]), key.GetGenericArguments()[0], this._data);
              IList instance = Activator.CreateInstance(key) as IList;
              foreach (object obj in array)
                instance.Add(obj);
              if (classMember == null)
                this._extraProperties.Add(str, (object) instance);
              else
                classMember.SetValue((object) this, (object) instance);
            }
            else if (key.IsGenericType && key.GetGenericTypeDefinition() == typeof (HashSet<>))
            {
              Array array = this.DeserializeArray(typeof (object[]), key.GetGenericArguments()[0], this._data);
              IList instance1 = (IList) Activator.CreateInstance(typeof (List<>).MakeGenericType(key.GetGenericArguments()[0]));
              foreach (object obj in array)
                instance1.Add(obj);
              object instance2 = Activator.CreateInstance(key, (object) instance1);
              if (classMember == null)
                this._extraProperties.Add(str, instance2);
              else
                classMember.SetValue((object) this, instance2);
            }
            else
            {
              object element = !key.IsEnum ? this._data.Read(key, false) : (object) this._data.ReadInt();
              if (classMember == null)
                this._extraProperties.Add(str, element);
              else
                classMember.SetValue((object) this, element);
            }
          }
          else
            this._data.position += (int) num4;
        }
      }
      catch (Exception ex)
      {
        this._exception = ex;
        this._result = DeserializeResult.ExceptionThrown;
        return false;
      }
      this._result = DeserializeResult.Success;
      return true;
    }

    public static BinaryClassChunk DeserializeHeader(
      System.Type t,
      BitBuffer data,
      BinaryClassChunk target = null,
      bool root = true,
      bool skipData = false)
    {
      if (target == null)
        target = Activator.CreateInstance(t, (object[]) null) as BinaryClassChunk;
      try
      {
        long num1 = 0;
        if (root)
        {
          num1 = data.ReadLong();
          if (num1 != BinaryClassChunk.MagicNumber(t) && num1 != 5033950674723417L)
          {
            target._result = DeserializeResult.InvalidMagicNumber;
            return target;
          }
          target._checksum = data.ReadUInt();
        }
        ushort num2 = data.ReadUShort();
        ushort num3 = BinaryClassChunk.ChunkVersion(t);
        if ((int) num2 != (int) num3 && (!(target is LevelData) || num2 != (ushort) 2))
        {
          target._result = (int) num2 <= (int) num3 ? DeserializeResult.FileVersionTooOld : DeserializeResult.FileVersionTooNew;
          return target;
        }
        if (num2 == (ushort) 2 && target is LevelData && data.ReadBool())
        {
          BinaryClassChunk binaryClassChunk = BinaryClassChunk.DeserializeHeader(Editor.GetType(data.ReadString()), data, root: false);
          if (binaryClassChunk != null && binaryClassChunk._result == DeserializeResult.HeaderDeserialized)
          {
            binaryClassChunk.Deserialize();
            target._headerDictionary["metaData"] = binaryClassChunk;
          }
        }
        target._magicNumber = num1;
        target._version = num2;
        target._size = data.ReadUInt();
        target._offset = (uint) data.position;
        target._data = data;
        target._result = DeserializeResult.HeaderDeserialized;
        if (skipData)
          data.position = (int) target._offset + (int) target._size;
        return target;
      }
      catch (Exception ex)
      {
        target._exception = ex;
        target._result = DeserializeResult.ExceptionThrown;
        return target;
      }
    }

    public T GetChunk<T>(string name)
    {
      BinaryClassChunk binaryClassChunk = (BinaryClassChunk) null;
      if (!this._headerDictionary.TryGetValue(name, out binaryClassChunk))
      {
        if (this._data != null)
          return default (T);
        binaryClassChunk = Activator.CreateInstance(typeof (T), (object[]) null) as BinaryClassChunk;
        binaryClassChunk._result = DeserializeResult.Success;
        this._headerDictionary[name] = binaryClassChunk;
      }
      if (binaryClassChunk == null)
        return default (T);
      if (binaryClassChunk._result == DeserializeResult.HeaderDeserialized)
        binaryClassChunk.Deserialize();
      return (T) binaryClassChunk;
    }

    private void SerializeArray(Array array, System.Type arrayType, BitBuffer data)
    {
      data.Write(array.Length);
      for (int index = 0; index < array.Length; ++index)
      {
        object obj = array.GetValue(index);
        data.Write(obj != null);
        if (obj != null)
        {
          if (typeof (BinaryClassChunk).IsAssignableFrom(arrayType))
            (obj as BinaryClassChunk).Serialize(data, false);
          else
            data.Write(obj);
        }
      }
    }

    public BitBuffer Serialize(BitBuffer data = null, bool root = true)
    {
      if (data == null)
        data = new BitBuffer(false);
      this._serializedData = data;
      if (data.allowPacking)
        throw new Exception("This class does not support serialization with a packed bit buffer. Construct the buffer with allowPacking set to false.");
      System.Type type1 = this.GetType();
      List<ClassMember> members = Editor.GetMembers(type1);
      List<BinaryClassMember> binaryClassMemberList = new List<BinaryClassMember>();
      foreach (ClassMember classMember in members)
      {
        if (!classMember.isPrivate && (classMember.type.IsEnum || classMember.type.IsPrimitive || (classMember.type.Equals(typeof (string)) || typeof (BinaryClassChunk).IsAssignableFrom(classMember.type)) || classMember.type.IsArray || classMember.type.IsGenericType && (classMember.type.GetGenericTypeDefinition() == typeof (List<>) || classMember.type.GetGenericTypeDefinition() == typeof (HashSet<>))))
        {
          object obj = classMember.GetValue((object) this);
          if (classMember.type.IsEnum)
            obj = (object) (int) obj;
          if (obj != null)
          {
            BinaryClassMember binaryClassMember = new BinaryClassMember()
            {
              name = classMember.name,
              data = obj
            };
            binaryClassMemberList.Add(binaryClassMember);
          }
        }
      }
      if (this._extraProperties != null)
      {
        foreach (KeyValuePair<string, List<object>> extraProperty in (MultiMap<string, object, List<object>>) this._extraProperties)
        {
          if (extraProperty.Value != null)
          {
            foreach (object obj1 in extraProperty.Value)
            {
              object obj2 = obj1;
              if (obj2.GetType().IsEnum)
                obj2 = (object) (int) obj2;
              BinaryClassMember binaryClassMember = new BinaryClassMember()
              {
                name = "@" + extraProperty.Key,
                data = obj2,
                extra = true
              };
              binaryClassMemberList.Add(binaryClassMember);
            }
          }
        }
      }
      if (root)
      {
        long val = BinaryClassChunk.MagicNumber(type1);
        data.Write(val);
        data.Write(0U);
      }
      data.Write(BinaryClassChunk.ChunkVersion(type1));
      int position1 = data.position;
      data.Write(0U);
      data.Write((ushort) binaryClassMemberList.Count);
      foreach (BinaryClassMember binaryClassMember in binaryClassMemberList)
      {
        data.Write(binaryClassMember.name);
        if (binaryClassMember.extra)
        {
          byte val = 0;
          System.Type type2 = binaryClassMember.data.GetType();
          if (BinaryClassMember.typeMap.TryGetValue(type2, out val))
            val = (byte) ((int) val << 1 | 1);
          data.Write(val);
          if (val == (byte) 0)
            data.Write(ModLoader.SmallTypeName(type2));
        }
        int position2 = data.position;
        data.Write(0U);
        if (binaryClassMember.data is BinaryClassChunk)
          (binaryClassMember.data as BinaryClassChunk).Serialize(data, false);
        else if (binaryClassMember.data is Array)
          this.SerializeArray(binaryClassMember.data as Array, binaryClassMember.data.GetType().GetElementType(), data);
        else if (binaryClassMember.data.GetType().IsGenericType && binaryClassMember.data.GetType().GetGenericTypeDefinition() == typeof (List<>))
        {
          IList data1 = binaryClassMember.data as IList;
          Array array = (Array) new object[data1.Count];
          data1.CopyTo(array, 0);
          this.SerializeArray(array, binaryClassMember.data.GetType().GetGenericArguments()[0], data);
        }
        else if (binaryClassMember.data.GetType().IsGenericType && binaryClassMember.data.GetType().GetGenericTypeDefinition() == typeof (HashSet<>))
        {
          IEnumerable data1 = binaryClassMember.data as IEnumerable;
          List<object> objectList = new List<object>();
          foreach (object obj in data1)
            objectList.Add(obj);
          object[] array = new object[objectList.Count];
          objectList.CopyTo(array, 0);
          this.SerializeArray((Array) array, binaryClassMember.data.GetType().GetGenericArguments()[0], data);
        }
        else
          data.Write(binaryClassMember.data);
        int position3 = data.position;
        data.position = position2;
        data.Write((uint) (position3 - position2 - 4));
        data.position = position3;
      }
      int position4 = data.position;
      data.position = position1;
      data.Write((uint) (position4 - position1 - 4));
      if (root)
      {
        this._checksum = Editor.Checksum(data.buffer);
        data.position = 8;
        data.Write(this._checksum);
      }
      data.position = position4;
      return data;
    }

    public static long MagicNumber<T>() => BinaryClassChunk.MagicNumber(typeof (T));

    public static long MagicNumber(System.Type t)
    {
      object[] customAttributes = t.GetCustomAttributes(typeof (MagicNumberAttribute), true);
      return customAttributes.Length != 0 ? (customAttributes[0] as MagicNumberAttribute).magicNumber : 0L;
    }

    public static ushort ChunkVersion<T>() => BinaryClassChunk.ChunkVersion(typeof (T));

    public static ushort ChunkVersion(System.Type t)
    {
      object[] customAttributes = t.GetCustomAttributes(typeof (ChunkVersionAttribute), true);
      return customAttributes.Length != 0 ? (customAttributes[0] as ChunkVersionAttribute).version : (ushort) 0;
    }
  }
}
