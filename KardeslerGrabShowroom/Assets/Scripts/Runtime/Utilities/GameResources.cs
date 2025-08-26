using IboshEngine.Runtime.Utilities.Singleton;
using UnityEngine;
using KardeslerGrabShowroom.Gameplay.Showroom;

namespace KardeslerGrabShowroom.Utilities
{
    public class GameResources : IboshSingleton<GameResources>
    {
        public Showroom Showroom;
    }
}