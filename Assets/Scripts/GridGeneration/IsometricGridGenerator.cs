/*
------------- ASSIGNMENT -------------

Clicking the "Generate grid" button should instantiate a 2D isometric grid of tile gameObjects, each randomly using a sprite in the _tileSprites array. 

The exposed surface for each tile asset is 512x256 pixels, and assets are set to 256 pixels per unit.

Here's a basic example of the expected result: https://drive.google.com/file/d/1BRczZJKccQ2kr0Vv5gqALS-aRgivNvn7/view?usp=sharing


Requirements:

- Instantiated tiles must be seamlessly joined together.
- Instantiated tiles must be properly z-ordered so as to give the illusion of a 3D perspective. 
- The generated grid's size can be parameterized.
- Clicking the button at runtime destroys the existing grid and generates a new one according to current parameters.
- The solution should remain functional for any and all grid sizes and shapes
- Comment the code wherever you believe it to be relevant.

Bonus 1: take performance into consideration in your implementation, and specifically comment code snippets where performance was accounted for
Bonus 2: generate an extra layer of obstacle gameObjects above the initial layer of tiles, randomly using the sprites in the _obstacleSprites array
*/

using DropRateStudio.TechTest.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DropRateStudio.TechTest.GridGeneration
{
	public sealed class IsometricGridGenerator : GenericSingleton<IsometricGridGenerator>
	{
		#region Fields & Properties

		[Header("* --- Grid Assets --- *")]
		[Space]
		[SerializeField] private Sprite[] _tileSprites;
		[SerializeField] private Sprite[] _obstacleSprites;
		[SerializeField] private Tile _tilePrefab;
		[SerializeField] private Obstacle _obstaclePrefab;

		[Header("* --- Grid Parameters --- *")]
		[Space]
		[SerializeField] [Range(2, 15)] private int _gridHeight = 10;
		[SerializeField] [Range(2, 15)] private int _gridWidth = 10;
		[SerializeField] private int _obstaclesCount = 2;

		[Header("* --- Grid Requirement --- *")]
		[Space]
		private Transform _tilesParentTransform;
		private Transform _obstaclesParentTransform;
		private List<Tile> _generatedTilesList;
		private Coroutine _generationCoroutine;

		#endregion

		#region Unity Methods + Event Sub

		// Awake is called when the script instance is being loaded (Overridden from GenericSingleton).
		protected override void Awake()
		{
			_tilesParentTransform = transform.GetChild(0);
			_obstaclesParentTransform = transform.GetChild(1);
			_generatedTilesList = new List<Tile>();
			_generationCoroutine = null;
		}

		// This function is called when the object becomes enabled and active.
		// This will ensure the to subscribe to events. 
		private void OnEnable() => EventSubscription(true);

		// This function is called when the behaviour becomes disabled.
		// This is also called when the object is destroyed and can be used for any cleanup code.
		// This will ensure the to unsubscribe to events properly and avoid memory leak.
		private void OnDisable() => EventSubscription(false);

		/// <summary>
		/// Methods called to (un)subcribe to events
		/// </summary>
		/// <param name="mustSubscribe">True : subscribe, False : unsubcribe</param>
		private void EventSubscription(bool mustSubscribe)
		{
			if (mustSubscribe)
			{
				GameManager.Instance.OnGridButtonPressed += ManageGeneration;
			}

			else
			{
				GameManager.Instance.OnGridButtonPressed -= ManageGeneration;
			}
		}

		#endregion

		#region Custom Methods

		/// <summary>
		/// Call this coroutine to manage next grid generation..
		/// </summary>
		/// <param name="isFirstGrisGeneration">True : First grid generation, False : A grid already exist.</param>
		private void ManageGeneration(bool isFirstGrisGeneration)
		{
			// Check if a generetion isn't currently running before run a new one (avoid error and perf issue)
			if (_generationCoroutine == null)
			{
				_generationCoroutine = StartCoroutine(GenerateGridCheck(isFirstGrisGeneration));
			}
		}

		/// <summary>
		/// Call this coroutine to make check before grid generation.
		/// </summary>
		/// <param name="isFirstGrisGeneration">True : First grid generation, False : A grid already exist.</param>
		private IEnumerator GenerateGridCheck(bool isFirstGrisGeneration)
		{
			yield return null;

			/// If a grid already exist, this grid must be destoyed before generate a new one.
			/// If none, we skip the clear for performance.
			if (!isFirstGrisGeneration)
			{
				yield return StartCoroutine(ClearGrid());
			}

			yield return StartCoroutine(GenerateGrid());
			yield return StartCoroutine(GenerateObstacles());
			_generationCoroutine = null;
		}

		/// <summary>
		/// Call this coroutine to generate the grid (tiles).
		/// </summary>
		private IEnumerator GenerateGrid()
		{
			yield return null;

			// Offset to apply on tile for the grid middle appear in the middle of the camera view 
			Vector2 offset = new Vector2(
				(_gridWidth - _gridHeight) * 0.5f,
				((_gridHeight - 1) * 0.25f) + ((_gridWidth - 1) * 0.25f));

			for (int x = 0; x < _gridWidth; x++)
			{
				for (int y = 0; y < _gridHeight; y++)
				{
					float posX = (-x + y);
					float posY = (-x - y) / 2f;
					Sprite randSprite = _tileSprites[Random.Range(0, _tileSprites.Length)];
					_generatedTilesList.Add(
						(Tile)CreateGridElement(
							gridElementPrefab: _tilePrefab,
							parent: _tilesParentTransform,
							position: new Vector2(posX, posY) + offset,
							elementName: $"Tile ({x} - {y}) : {randSprite.name}",
							sprite: randSprite,
							orderInLayer: x + y));
				}
			}

			yield return null;
		}

		/// <summary>
		/// Call this coroutine to generate all obstacles on the grid.
		/// </summary>
		private IEnumerator GenerateObstacles()
		{
			yield return null;

			// If Obstacle count is less 0 or equal to 0, we skip obstacle generation for performance.
			if (_obstaclesCount > 0)
			{
				if (_obstaclesCount >= _generatedTilesList.Count)
				{
					_obstaclesCount = _generatedTilesList.Count;
				}

				List<int> randomNumbers = Extensions.GetUniqueRandomNumbersList(0, _generatedTilesList.Count, _obstaclesCount);

				// Used the list of current tiles to generate obstacles. Avoid a "for" loop for performance.
				for (int i = 0; i < _obstaclesCount; i++)
				{
					Sprite randSprite = _obstacleSprites[Random.Range(0, _obstacleSprites.Length)];
					CreateGridElement(
						gridElementPrefab: _obstaclePrefab,
						parent: _obstaclesParentTransform,
						position: _generatedTilesList[randomNumbers[i]].transform.position,
						elementName: $"Obstacle {i} : {randSprite.name}",
						sprite: randSprite,
						orderInLayer: _generatedTilesList[randomNumbers[i]].OrderInLayer);
				}
			}

			yield return null;
		}

		/// <summary>
		/// Method to generate a new grid element with a sprite and good Z order
		/// </summary>
		/// <param name="gridElementPrefab">The grid element prefab to instantiate</param>
		/// <param name="parent">The uniy hierarchy parent</param>
		/// <param name="sprite">Sprite to render for the grid element</param>
		/// <param name="orderInLayer">The Z order in the sorting layer</param>
		/// <param name="position">The grid elemeent 2D position</param>
		/// <param name="elementName">The grid element name</param>
		/// <returns>Return the new grid element</returns>
		private IGridable CreateGridElement(IGridable gridElementPrefab, Transform parent, Vector2 position, string elementName, Sprite sprite, int orderInLayer)
		{
			IGridable newElement = gridElementPrefab.InstantiateGridElement(parent, position, elementName);
			newElement.Sprite = sprite;
			// Working with sorting layers on prefabs (see Unity) + OrderInLayer for a correct Z rendering
			newElement.OrderInLayer = orderInLayer;
			return newElement;
		}

		/// <summary>
		/// Call this coroutine to clear the current grid (Destroy all tiles and obsctacles).
		/// </summary>
		private IEnumerator ClearGrid()
		{
			yield return null;

			foreach (Transform child in _tilesParentTransform)
			{
				Destroy(child.gameObject);
			}

			foreach (Transform child in _obstaclesParentTransform)
			{
				Destroy(child.gameObject);
			}

			_generatedTilesList.Clear();
			yield return null;
		}

		#endregion
	}
}