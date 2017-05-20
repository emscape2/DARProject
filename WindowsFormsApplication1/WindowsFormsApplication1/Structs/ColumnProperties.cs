	///<summary>
	/// properties of the column, contains type, weighting factor, value type
	/// </summary>
	public struct ColumnProperties
	{
        public bool numerical;
        public string name;
        public ColumnProperties(bool numerkal, string naam)
        {
            numerical = numerkal;
            name = naam;
        }
	}
