using UnityEngine;

namespace DropRateStudio.TechTest
{
	/// <summary>
	/// Inheritance from Singleton ensures that only once instance
	/// of the GameObject will be present in the scene.
	/// If there are more, they will be destroyed.
	/// </summary>
	/// <typeparam name="T">Component type.</typeparam>
	public abstract class GenericSingleton<T> : MonoBehaviour where T : Component
	{
		#region Fields & Properties

		// Allow easy access to the single instance of the object who inherit from this script
		public static T Instance { get; private set; } = null;

		#endregion

		#region Unity Methods + Event Sub

		protected virtual void Awake()
		{
			if (Instance == null)
			{
				Instance = this as T;
			}

			else
			{
				Destroy(gameObject);
			}
		}

		#endregion
	}
}