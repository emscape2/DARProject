using System.Collections.Generic;

	///<summary>
	/// Stores the Attributes desired by the query, could be done as implementation of a List containting QueryAttributes
	/// </summary>
	public struct Query
	{
		public List<QueryAttribute> Attributes;
        public int K;

        public Query(int fuckoff)
        {
            K = 10;
            Attributes = new List<QueryAttribute>();
        }

        public Query(List<QueryAttribute> attributes, int K)
        {
            Attributes = attributes;
            this.K = K;
        }

        public Query(List<QueryAttribute> attributes)
        {
            Attributes = attributes;
            K = 10;
        }
	}
