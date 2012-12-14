using System;

namespace AssemblyCSharp
{
	public class RoomToStringMapping
	{
		
		private static RoomAndName[] mapping;
		
		static RoomToStringMapping() {	
			mapping = new RoomAndName[] {
				new RoomAndName(new TestRoom(), "Test")
			};
			mapping[0].ToString();
		}
		
		public static RoomData getRoom(String name) {
			foreach (RoomAndName rn in mapping) {
				if (rn.name.Equals(name)) {
					return rn.room;
				}
			}
			
			return null;
		}
		
		public static String getName(RoomData room) {
			foreach (RoomAndName rn in mapping) {
				if (rn.room.GetType().Equals(room.GetType())) {
					return rn.name;
				}
			}
			
			return null;
		}
		
		private class RoomAndName {
			public readonly RoomData room;
			public readonly String name;
		
			public RoomAndName(RoomData room, String name) {
				this.room = room;
				this.name = name;
			}
		}
	}
}

