using UnityEngine;

namespace DropRateStudio.TechTest
{
	// Make sure that managers scripts run before others to avoid errors (event subecription for exemple)
	[DefaultExecutionOrder(-1)]
	public sealed class GameManager : GenericSingleton<GameManager>
	{
		#region Fields & Properties

		private bool _isFirstMapGeneration;

		#endregion

		#region Unity Methods + Event Sub

		// Awake is called when the script instance is being loaded (Overridden from GenericSingleton).
		protected override void Awake()
		{
			base.Awake();
			// Capped the framerate to 60 for performance purpose.
			Application.targetFrameRate = 60;
			_isFirstMapGeneration = true;
		}

		#endregion

		#region Custom Methods

		/// <summary>
		/// Method called when the UI Generate Grid button is pressed.
		/// Trigger the event OnGridButtonPressed.
		/// </summary>
		public void UIGridButtonPressed()
		{
			OnGridButtonPressed?.Invoke(_isFirstMapGeneration);
			_isFirstMapGeneration = false;
		}

		#endregion

		#region Events

		/// <summary>
		/// Delegate for Map Generation signature : Return type "void" and a bool for first grid generation.
		/// </summary>
		/// <param name="isFirstGridGeneration"> True if it's the first grid generation.</param>
		public delegate void GridButtonPressed(bool isFirstGridGeneration);

		// Event to call when the Generate Grid button is pressed.
		public event GridButtonPressed OnGridButtonPressed;

		#endregion
	}
}