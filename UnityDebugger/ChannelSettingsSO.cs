#region License
// ====================================================
// Project Porcupine Copyright(C) 2016 Team Porcupine
// This program comes with ABSOLUTELY NO WARRANTY; This is free software, 
// and you are welcome to redistribute it under certain conditions; See 
// file GPL3, which is part of this source code package, for details.
// ====================================================
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChannelSettingsSO : ScriptableObject {
    public bool DefaultState;
    public ChannelDictionary ChannelState = new ChannelDictionary();
}
