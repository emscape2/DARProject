	///<summary>
	/// Contains column name and desired value
	/// TODO: Add support for larger / smaller than values
	/// </summary>
	public struct QueryAttribute
	{
		public string ColumnName;
		public object DesiredValue;
        public bool Numeric;

		public QueryAttribute(string columnName, object desiredValue, bool numeric)
		{
			ColumnName = columnName;
			DesiredValue = desiredValue;
            Numeric = numeric;
		}

        public bool Equals(QueryAttribute queryAttribute)
        {
                return queryAttribute.ColumnName == this.ColumnName;
        }
	}
