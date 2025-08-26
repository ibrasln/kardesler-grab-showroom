namespace IboshEngine.Runtime.Core.EventManagement
{
    /// <summary>
    /// Provides access to the event managers for different systems.
    /// </summary>
    public static class EventManagerProvider
    {
        public static readonly UIEventManager UI = new();
        public static readonly DataEventManager Data = new();
        public static readonly CameraEventManager Camera = new();
        public static readonly ShowroomEventManager Showroom = new();
    }

    /// <summary>
    /// Event manager for UI events.
    /// </summary>
    public class UIEventManager : BaseEventManager<UIEvent>
    {
    }

    /// <summary>
    /// Event manager for data events.
    /// </summary>
    public class DataEventManager : BaseEventManager<DataEvent>
    {
    }

    /// <summary>
    /// Event manager for camera events.
    /// </summary>
    public class CameraEventManager : BaseEventManager<CameraEvent>
    {
    }

    /// <summary>
    /// Event manager for showroom events.
    /// </summary>
    public class ShowroomEventManager : BaseEventManager<ShowroomEvent>
    {
    }
}