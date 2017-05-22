	///<summary>
	/// properties of the column, contains type, weighting factor, value type
	/// </summary>
	public class ColumnProperties
	{
        public bool? numerical;
        public string name;
    public double max, min;
    public int distinctValues;
    public double interval;

    public ColumnProperties(bool? numerkal, string naam)
        {
            numerical = numerkal;
            name = naam;
        max = 0;
        min = 0;
        distinctValues = 0;
        interval = 0;
        }
	}
