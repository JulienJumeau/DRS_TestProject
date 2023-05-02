using System.Collections.Generic;
using UnityEngine;

namespace DropRateStudio.TechTest.Extension
{
	public sealed class Extensions
	{
		/// <summary>
		/// Call this method to get a random numbers List. 
		/// A number cannot be in the List twice.
		/// </summary>
		/// <param name="minRange">The minimal value</param>
		/// <param name="maxRange">The maximal value</param>
		/// <param name="randCount">Count of random numbers to add to the list</param>
		/// <returns>List of unique random numbers between min/max range </returns>
		public static List<int> GetUniqueRandomNumbersList(int minRange, int maxRange, int randCount)
		{
			List<int> randomNumbersList = new();

			do
			{
				int rand = Random.Range(minRange, maxRange);

				if (!randomNumbersList.Contains(rand))
				{
					randomNumbersList.Add(rand);
				}

			} while (randomNumbersList.Count < randCount);

			return randomNumbersList;
		}

		/// <summary>
		/// Call this method to destroy all children from a GameObject
		/// </summary>
		/// <param name="parentTransform">The transform component of the parent GameObject</param>
		public static void DestroyAllChildren(Transform parentTransform)
        {
            foreach (Transform child in parentTransform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}