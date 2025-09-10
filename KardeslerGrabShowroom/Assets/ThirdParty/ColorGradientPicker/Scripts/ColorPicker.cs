using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IboshEngine.Runtime.Core.EventManagement;
using Sirenix.OdinInspector;

public class ColorPicker : MonoBehaviour
{
    [BoxGroup("Components")][SerializeField] private Slider sliderMain;
    [BoxGroup("Components")][SerializeField] private Slider sliderR;
    [BoxGroup("Components")][SerializeField] private Slider sliderG;
    [BoxGroup("Components")][SerializeField] private Slider sliderB;
    [BoxGroup("Components")][SerializeField] private TMP_InputField inputFieldHexa;

    private Color32 _originalColor;
    private Color32 _modifiedColor;
    private HSV _modifiedHsv;
    private ColorPickerType _colorPickerType;

    /// <summary>
    /// Returns true if the color has been modified from its original value.
    /// </summary>
    private bool IsModified
    {
        get
        {
            return !_originalColor.Equals(_modifiedColor);
        }
    }

    public Color32 OriginalColor => _originalColor;
    public Color32 ModifiedColor => _modifiedColor;


    #region Built-In
    
    private void OnEnable()
    {
        sliderMain.onValueChanged.AddListener(SetMain);
        sliderR.onValueChanged.AddListener(SetR);
        sliderG.onValueChanged.AddListener(SetG);
        sliderB.onValueChanged.AddListener(SetB);
        inputFieldHexa.onValueChanged.AddListener(SetHexa);

        SubscribeToEvents();
    }
    
    private void OnDisable()
    {
        sliderMain.onValueChanged.RemoveListener(SetMain);
        sliderR.onValueChanged.RemoveListener(SetR);
        sliderG.onValueChanged.RemoveListener(SetG);
        sliderB.onValueChanged.RemoveListener(SetB);
        inputFieldHexa.onValueChanged.RemoveListener(SetHexa);

        UnsubscribeFromEvents();
    }
    #endregion

    #region Event Subscription

    private void SubscribeToEvents()
    {
        EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerCancelled, HandleOnColorPickerCancelled);
        EventManagerProvider.UI.AddListener(UIEvent.OnColorPickerApplied, HandleOnColorPickerApplied);
    }
    
    private void UnsubscribeFromEvents()
    {
        EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerCancelled, HandleOnColorPickerCancelled);
        EventManagerProvider.UI.RemoveListener(UIEvent.OnColorPickerApplied, HandleOnColorPickerApplied);
    }

    #endregion

    #region Event Handling

    public void HandleOnColorPickerCancelled()
    {
        _modifiedColor = _originalColor;
        RecalculateMenu(true);
    }

    public void HandleOnColorPickerApplied()
    {
        RecalculateMenu(true);
    }
    
    #endregion

    #region Initialization & Recalculation
    
    /// <summary>
    /// Initializes the ColorPicker with the original color.
    /// </summary>
    /// <param name="original">The original color.</param>
    public void Initialize(Color original, ColorPickerType colorPickerType)
    {
        _originalColor = original;
        _modifiedColor = original;
        _colorPickerType = colorPickerType;
        RecalculateMenu(true);
    }

    /// <summary>
    /// Recalculates the menu when the color is modified.
    /// </summary>
    /// <param name="recalculateHSV">Whether to recalculate the HSV values.</param>
    private void RecalculateMenu(bool recalculateHSV)
    {
        if(recalculateHSV)
        {
            _modifiedHsv = new HSV(_modifiedColor);
        }
        else
        {
            _modifiedColor = _modifiedHsv.ToColor();
        }

        sliderR.value = _modifiedColor.r;
        sliderG.value = _modifiedColor.g;
        sliderB.value = _modifiedColor.b;
        sliderMain.value = (float)_modifiedHsv.H;
        sliderR.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(255, _modifiedColor.g, _modifiedColor.b, 255);
        sliderR.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(0, _modifiedColor.g, _modifiedColor.b, 255);
        sliderG.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, 255, _modifiedColor.b, 255);
        sliderG.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, 0, _modifiedColor.b, 255);
        sliderB.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, _modifiedColor.g, 255, 255);
        sliderB.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = new Color32(_modifiedColor.r, _modifiedColor.g, 0, 255);
        inputFieldHexa.text = ColorUtility.ToHtmlStringRGB(_modifiedColor);

        if (_colorPickerType == ColorPickerType.Main)
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnMainColorChanged, _modifiedColor);
        }
        else
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnSubColorChanged, _modifiedColor);
        }
    }

    #endregion

    #region Setters

    /// <summary>
    /// Sets the main slider value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void SetMain(float value)
    {
        _modifiedHsv.H = value;
        RecalculateMenu(false);
    }

    /// <summary>
    /// Sets the r slider value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void SetR(float value)
    {
        _modifiedColor.r = (byte)value;
        RecalculateMenu(true);
    }

    /// <summary>
    /// Sets the g slider value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void SetG(float value)
    {
        _modifiedColor.g = (byte)value;
        RecalculateMenu(true);
    }

    /// <summary>
    /// Sets the b slider value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void SetB(float value)
    {
        _modifiedColor.b = (byte)value;
        RecalculateMenu(true);
    }

    /// <summary>
    /// Sets the hexa input field value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void SetHexa(string value)
    {
        if (value == null || value.Length != 6)
        {
            return;
        }

        if (ColorUtility.TryParseHtmlString("#" + value, out Color c))
        {
            c.a = 1;
            _modifiedColor = c;
            RecalculateMenu(true);
        }
        else
        {
            inputFieldHexa.text = ColorUtility.ToHtmlStringRGB(_modifiedColor);
        }
    }

    #endregion

    #region UI Management

    public bool CheckCanHide()
    {
        Debug.Log("IsModified: " + IsModified);
        if (IsModified)
        {
            EventManagerProvider.UI.Broadcast(UIEvent.OnColorPickerCannotHide);
            return false;
        }
        return true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    /// <summary>
    /// HSV helper class.
    /// </summary>
    private sealed class HSV
    {
        public double H = 0, S = 1, V = 1;
        public byte A = 255;
        public HSV () { }
        public HSV (double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
        public HSV (Color color)
        {
            float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

            float hue = (float)H;
            if (min != max)
            {
                if (max == color.r)
                {
                    hue = (color.g - color.b) / (max - min);

                }
                else if (max == color.g)
                {
                    hue = 2f + (color.b - color.r) / (max - min);

                }
                else
                {
                    hue = 4f + (color.r - color.g) / (max - min);
                }

                hue *= 60;
                if (hue < 0) hue += 360;
            }

            H = hue;
            S = (max == 0) ? 0 : 1d - ((double)min / max);
            V = max;
            A = (byte)(color.a * 255);
        }
        public Color32 ToColor()
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            double value = V * 255;
            byte v = (byte)Convert.ToInt32(value);
            byte p = (byte)Convert.ToInt32(value * (1 - S));
            byte q = (byte)Convert.ToInt32(value * (1 - f * S));
            byte t = (byte)Convert.ToInt32(value * (1 - (1 - f) * S));

            switch(hi)
            {
                case 0:
                    return new Color32(v, t, p, A);
                case 1:
                    return new Color32(q, v, p, A);
                case 2:
                    return new Color32(p, v, t, A);
                case 3:
                    return new Color32(p, q, v, A);
                case 4:
                    return new Color32(t, p, v, A);
                case 5:
                    return new Color32(v, p, q, A);
                default:
                    return new Color32();
            }
        }
    }
}

public enum ColorPickerType
{
    Main,
    Sub
}