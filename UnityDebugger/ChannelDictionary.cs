#region License
// ====================================================
// Project Porcupine Copyright(C) 2016 Team Porcupine
// This program comes with ABSOLUTELY NO WARRANTY; This is free software, 
// and you are welcome to redistribute it under certain conditions; See 
// file GPL3, which is part of this source code package, for details.
// ====================================================
#endregion
using System.Collections.Generic;
using System;
using com.spacepuppy.Collections;


[Serializable]
public class ChannelDictionary : SerializableDictionaryBase<string, bool>
{
    public ChannelDictionary()
    {
    }

    public ChannelDictionary(ChannelDictionary channelState)
    {
        foreach (KeyValuePair<string,bool> kvp in channelState)
        {
            this.Add(kvp.Key, kvp.Value);
        }
    }
}
