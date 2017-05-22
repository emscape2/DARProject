///<summary>
/// properties of the column, contains type, weighting factor, value type
/// </summary>
public class ColumnProperties
{
    public bool? numerical;
    public string name;
    public double max, min;
    public int distinctValues;
    private double interval;

    public ColumnProperties(bool? numerkal, string naam)
    {
        numerical = numerkal;
        name = naam;
        max = 0;
        min = 0;
        distinctValues = 0;
        interval = 0;
    }


    public double GetInterval()
    {
        if (interval != 0)
            return interval;
        else
            return TableProccessor.GetIntervalSize(name);

    }

    public void SetInterval(double interVal)
    {
        interval = interVal;
    }
}