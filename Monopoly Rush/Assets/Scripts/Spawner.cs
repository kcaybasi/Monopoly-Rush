using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

public class Spawner : MonoBehaviour
{
    // Components
    private ObjectPooler _objectPooler;
    
    [Header("Brick Spawn")]
    [SerializeField] Transform brickSpawnTransform;
    readonly int _brickSpawnTime=300; // Milliseconds
    private bool _isBrickSpawningAllowed;
    readonly int _brickSpawnLimit=500;
    private int _activeBrick;
    
    private void Start()
    {
        _isBrickSpawningAllowed = true;
        _objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if (brickSpawnTransform != null)
        {
            if (_isBrickSpawningAllowed)
            {
                if (_activeBrick < _brickSpawnLimit)
                {
                   SpawnBrickAsync();
                }
            }
        }
    }
    async void SpawnBrickAsync()
    {
        _isBrickSpawningAllowed = false; // Prevents spawning more than one brick at a time
        GameObject brickObj = _objectPooler.BrickPool.Get(); // Get a brick from the pool
        _activeBrick++; // Increase the active brick count
        RandomizeBrickPosition(brickObj);
        await PlayBrickSpawnTween(brickObj);
        brickObj.GetComponent<BoxCollider>().enabled = true; // Enable the brick's collider because it is disabled in collector script to prevent double collection
        await Task.Delay(_brickSpawnTime); // Wait for the specified time
        _isBrickSpawningAllowed = true; // Allow spawning again
    }

    private static async Task PlayBrickSpawnTween(GameObject brickObj)
    {
        Tween tween = brickObj.transform.DOPunchScale(Vector3.one * 0.5f, 0.75f, 5, 0f); // Scale the brick up and down
        await tween.AsyncWaitForCompletion(); // Wait for the tween to complete
    }

    private void RandomizeBrickPosition(GameObject obj)
    {
        float randomX = Random.Range(-5.0f, 5.0f); // Randomize the X position
        float randomZ = Random.Range(-5.0f, 5.0f); // Randomize the Z position
        Vector3 offsetVector = new Vector3(randomX, 0.25f, randomZ); // Create a vector with the randomized values
        Vector3 spawnPos = brickSpawnTransform.position + offsetVector * 2; // Add the offset vector to the spawn position
        obj.transform.position = spawnPos; // Set the brick's position to the spawn position
    }
}
