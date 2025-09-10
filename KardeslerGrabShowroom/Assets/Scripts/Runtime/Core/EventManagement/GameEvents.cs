namespace IboshEngine.Runtime.Core.EventManagement
{
    /// <summary>
    /// Enum defining different types of UI events.
    /// </summary>
    public enum UIEvent
    {
        OnSettingsButtonClicked,
        OnShowroomButtonClicked,
        OnAboutButtonClicked,
        OnContactButtonClicked,
        OnRequestFormButtonClicked,
        OnGrabDetailsButtonClicked,
        OnColorSwitcherButtonClicked,
        OnMainColorChanged,
        OnMainColorSelected,
        OnSubColorChanged,
        OnSubColorSelected,
        OnColorSwitcherPanelClosed,
        OnColorPickerApplied,
        OnColorPickerCancelled,
    }

    /// <summary>
    /// Enum defining different types of Data events.
    /// </summary>
    public enum DataEvent
    {
    }

    /// <summary>
    /// Enum defining different types of Camera events.
    /// </summary>
    public enum CameraEvent
    {
        OnNoneCameraStarted,
        OnNoneCameraCompleted,
        OnMenuCameraStarted,
        OnMenuCameraCompleted,
        OnShowroomCameraStarted,
        OnShowroomCameraCompleted,
        OnColorSwitcherCameraStarted,
        OnColorSwitcherCameraCompleted,
    }

    /// <summary>
    /// Enum defining different types of Showroom events.
    /// </summary>
    public enum ShowroomEvent
    {
        OnGrabMovementStarted,
        OnGrabMovementCompleted,
    }
}