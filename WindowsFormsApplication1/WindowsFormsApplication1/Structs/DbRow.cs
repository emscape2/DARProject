using System.Collections.Generic;

	public struct DbRow
	{
        private List<QueryAttribute> _attributes;
        public List<QueryAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
    }
