using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class LocationGraph: MonoBehaviour
	{
		private static LocationGraph instance = null;
		
		private List<Location> locations;
		private bool[,] graph;
		
		public bool inverse = false;
		
		void Start() {
			if (instance != null)
				throw new Exception("Error: multiple LocationGraphs present");
			instance = this;
			
			locations = new List<Location>();
			
			Component[] components = this.gameObject.GetComponents(typeof(Edge));
			Edge[] edges = new Edge[components.Length];
			for (int i = 0; i < components.Length; i++) {
				edges[i] = (Edge) components[i];
				
				if (!locations.Contains(edges[i].one))
					locations.Add(edges[i].one);
				if (!locations.Contains(edges[i].two))
					locations.Add(edges[i].two);
			}
			
			graph = new bool[locations.Count, locations.Count];
			for (int i = 0; i < graph.GetLength(0); i++) {
				for (int j = 0; j < graph.GetLength(1); j++) {
					graph[i,j] = inverse;
				}
			}
			
			foreach (Edge edge in edges) {				
				int indexOne = locations.IndexOf(edge.one);
				int indexTwo = locations.IndexOf(edge.two);
				
				graph[indexOne, indexTwo] = !inverse;
				graph[indexTwo, indexOne] = !inverse;
			}
		}
		
		void OnDrawGizmosSelected() {
			Component[] edges = gameObject.GetComponents(typeof(Edge));
			foreach (Component c in edges) {
				Edge e = (Edge) c;
				Gizmos.DrawLine(e.one.transform.position, e.two.transform.position);
			}
		}
		
		private bool connected(Location one, Location two) {
			int indexOne = locations.IndexOf(one);
			int indexTwo = locations.IndexOf(two);
			return graph[indexOne, indexTwo];
		}
		
		public static bool conntected(Location one, Location two) {
			return instance.connected(one, two);
		}
	}
}

