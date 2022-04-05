using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Entry : MonoBehaviour
{
    const string homeSceneKey = "Assets/Scenes/HomeScene.unity";

    public void ClickedCheckCatalog() => CheckCatalog().Forget();
    public void ClickedGoHome() => CheckDownload().Forget();

    private AssetReference aa;
    public void ClickedClearCache()
    {
        var cleared = Caching.ClearCache();
        Debug.Log($"Caching.ClearCache(): {cleared}");
        
        
    }

    private async UniTaskVoid CheckCatalog()
    {
        var a = await aa.LoadAssetAsync<GameObject>().ToUniTask();
        Instantiate(a);
        
        var handle = Addressables.CheckForCatalogUpdates();

        var list = await handle.ToUniTask();
        if (list == null || list.Count == 0)
        {
            Debug.Log($"No Update");
            return;
        }

        Debug.Log($"Start Update Catalog");

        var updateHandle = Addressables.UpdateCatalogs(list);

        await updateHandle.ToUniTask();
        
        Debug.Log($"Completed Update Catalog");
    }

    private async UniTaskVoid CheckDownload()
    {
        var handle = Addressables.GetDownloadSizeAsync(homeSceneKey);
        var size = await handle.ToUniTask();
        Debug.Log($"GetDownloadSizeAsync: {size}");
        var needDownload = size > 0;
        if (needDownload)
        {
            await DownloadHomeAssets();
        }
        else
        {
            await LoadHomeScene();
        }
    }

    private async UniTask DownloadHomeAssets()
    {
        Debug.Log($"start loading home asset group");
        var handle = Addressables.DownloadDependenciesAsync(homeSceneKey);
        Utils.LoadingProgress(handle).Forget();

        await handle.ToUniTask();
        // Addressables.Release(handle);
        
        await LoadHomeScene();
    }

    private async UniTask LoadHomeScene()
    {
        Debug.Log($"LoadScene: {homeSceneKey}");
        var handle = Addressables.LoadSceneAsync(homeSceneKey);
        Utils.LoadingProgress(handle).Forget();

        var sceneInstance = await handle.ToUniTask();
        // Addressables.Release(handle);
    }

    

    
}
