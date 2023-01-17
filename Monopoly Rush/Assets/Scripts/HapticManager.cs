using System;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class HapticManager : MonoBehaviour
{
    public Haptic[] haptics;

    private void Awake()
    {
        for(int i = 0; i < haptics.Length; i++)
        {
            Haptic haptic = haptics[i];

            
        }
    }

    public void Vibrate(string name)
    {
        Haptic h = Array.Find(haptics, haptic => haptic.name == name);
        if (name == null)
        {
            Debug.LogError("No sound name with " + name + " found");
            return;
        }
        MMVibrationManager.Haptic(h.haptic_Type);
       
    }
}
