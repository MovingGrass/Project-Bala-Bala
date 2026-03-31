using UnityEngine;

public class RangeWithFormatAttribute : PropertyAttribute
{
    public float min;
    public float max;
    public string format;

    public RangeWithFormatAttribute(float min, float max, string format = "F2")
    {
        this.min = min;
        this.max = max;
        this.format = format;
    }
}
