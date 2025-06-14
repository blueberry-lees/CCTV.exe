using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI dateTimeText;

    public enum TimeFormat
    {
        FullDateTime,       // yyyy-MM-dd HH:mm:ss
        ShortDate,          // MM/dd/yyyy
        LongDate,           // dddd, dd MMMM yyyy
        TimeOnly24Hour,     // HH:mm:ss
        TimeOnly12Hour,     // hh:mm tt
        Custom              // User-defined
    }

    public TimeFormat selectedFormat = TimeFormat.FullDateTime;
    public string customFormat = "yyyy/MM/dd HH:mm"; // Used when "Custom" is selected

    private void Awake()
    {
        dateTimeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string format = GetFormat();
        dateTimeText.text = DateTime.Now.ToString(format);
    }

    private string GetFormat()
    {
        switch (selectedFormat)
        {
            case TimeFormat.FullDateTime:
                return "yyyy-MM-dd HH:mm:ss";
            case TimeFormat.ShortDate:
                return "MM/dd/yyyy";
            case TimeFormat.LongDate:
                return "dddd, dd MMMM yyyy";
            case TimeFormat.TimeOnly24Hour:
                return "HH:mm:ss";
            case TimeFormat.TimeOnly12Hour:
                return "hh:mm tt";
            case TimeFormat.Custom:
                return customFormat;
            default:
                return "yyyy-MM-dd HH:mm:ss";
        }
    }
}
