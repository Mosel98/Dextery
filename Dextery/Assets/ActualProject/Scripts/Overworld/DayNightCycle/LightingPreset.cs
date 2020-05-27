using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script by Tamara
// change colors easily in Editor --> Supposed Help for Artists to play around with
[System.Serializable]
[CreateAssetMenu(fileName ="Lighting Preset", menuName ="Lighting Preset", order =1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
