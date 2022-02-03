using UnityEngine;
using UnityEngine.AddressableAssets;

public class Spawn : MonoBehaviour
{
    public AssetReference spawnPrefab;
    public float spawnTimeSec = 1f;
    
    void Start()
    {
        spawnPrefab.LoadAssetAsync<GameObject>().Completed += _ => StartSpawn();
    }

    private void StartSpawn()
    {
        Instantiate(spawnPrefab.Asset);
        Invoke(nameof(StartSpawn), spawnTimeSec);
    }

}
