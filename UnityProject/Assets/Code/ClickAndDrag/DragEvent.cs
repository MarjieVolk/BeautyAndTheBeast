using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class DragEvent
	{
		public readonly DragState state;
		private Dictionary<String, Object> parameters = new Dictionary<String, Object>();
		
		public DragEvent (DragState state)
		{
			this.state = state;
		}
		
		public void putParam(String id, Object val) {
			parameters.Add(id, val);
		}
		
		public Object getParam(String id) {
			return parameters[id];
		}
	}
}

