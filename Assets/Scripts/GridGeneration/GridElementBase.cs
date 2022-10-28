using UnityEngine;

namespace DropRateStudio.TechTest.GridGeneration
{
	/// <summary>
	/// Base class to inherit to all grid elements. 
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class GridElementBase : MonoBehaviour
	{
		protected SpriteRenderer SpriteRenderer;

		// Awake is called when the script instance is being loaded.
		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}
	}
}