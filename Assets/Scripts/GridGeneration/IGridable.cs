using UnityEngine;

namespace DropRateStudio.TechTest.GridGeneration
{
	/// <summary>
	/// Use this interface for level design element usefull for the grid (tile,obstacle,trap...).
	/// </summary>
	public interface IGridable
	{
		#region Fields & Properties

		public Sprite Sprite
		{
			get;
			set;
		}

		public int OrderInLayer
		{
			get;
			set;
		}

		#endregion

		#region Custom Methods

		public IGridable InstantiateGridElement(Transform parent, Vector2 position, string elementName);

		#endregion
	}
}