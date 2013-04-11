using System;

namespace AssemblyCSharp
{
	public interface GameStateListener
	{
		void stateChanged(String stateKey, object oldValue, object newValue);
	}
}

