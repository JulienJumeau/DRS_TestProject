using UnityEngine;

namespace DropRateStudio.TechTest.GridGeneration
{
	public sealed class Tile : GridElementBase, IGridable
	{
		#region Fields & Properties

		/// <summary>
		/// Property to get/set the tile sprite
		/// </summary>
		public Sprite Sprite
		{
			get => SpriteRenderer?.sprite;

			set
			{
				if (SpriteRenderer != null)
				{
					SpriteRenderer.sprite = value;
				}
			}
		}

		/// <summary>
		/// Prpoerty to get/set sprite order in layer (for good z tile render)
		/// </summary>
		public int OrderInLayer
		{
			get => (int)(SpriteRenderer?.sortingOrder);
			set
			{
				if (SpriteRenderer != null)
				{
					SpriteRenderer.sortingOrder = value;
				}
			}
		}

		#endregion

		#region Custom Methods

		/// <summary>
		/// Method to instantiate the grid element with a parent, 2D position and a name 
		/// </summary>
		/// <param name="parent">Transform parent in unity hierarchy</param>
		/// <param name="position">2D position of the element</param>
		/// <param name="elementName">Element name</param>
		/// <returns>Return the instantiated element</returns>
		public IGridable InstantiateGridElement(Transform parent, Vector2 position, string elementName)
		{
			Tile element = Instantiate(this, parent);
			element.name = elementName;
			element.transform.localPosition = position;
			return element;
		}

		#endregion
	}
}