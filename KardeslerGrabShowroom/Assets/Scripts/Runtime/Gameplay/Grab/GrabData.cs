using UnityEngine;
using Sirenix.OdinInspector;

namespace KardeslerGrabShowroom.Gameplay.Grab
{
    [CreateAssetMenu(fileName = "GrabData", menuName = "Data/Grab")]
    public class GrabData : ScriptableObject 
    {
        [BoxGroup("Base Properties")] public string Name;
        [BoxGroup("Base Properties")] public string Description;
        [BoxGroup("Color Properties")] public Color MainColor;
        [BoxGroup("Color Properties")] public Color SubColor;
    }
}