using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15.0f;
    public float spawnRate = 2.0f;
    public float spawnDistance = 1.0f;
    public int spawnAmount = 1;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            // Get a random spawn point just above the screen
            Vector3 spawnPoint = GetSpawnPointAboveScreen();

            // Add random variance to the asteroid's downward trajectory
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.Euler(0, 0, variance);

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Set the asteroid trajectory towards the bottom of the screen
            Vector2 downwardDirection = Vector2.down; 
            asteroid.SetTrajectory(rotation * downwardDirection);
        }
    }

    private Vector3 GetSpawnPointAboveScreen()
    {
        // Get screen bounds based on the camera's view
        Vector3 screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.nearClipPlane));

        // Randomly select a point at the top of the screen within the screen's width
        float spawnX = Random.Range(-screenBounds.x, screenBounds.x);
        float spawnY = this.spawnDistance; // Slightly above the visible screen

        return new Vector3(spawnX, spawnY, 0); // Return spawn position
    }
}
