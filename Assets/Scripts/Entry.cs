using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        // LoadingProgress(homeSceneKey, handle).Forget();
        
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
        LoadingProgress(homeSceneKey, handle).Forget();

        await handle.ToUniTask();
        Addressables.Release(handle);
        
        await LoadHomeScene();
    }

    private async UniTask LoadHomeScene()
    {
        Debug.Log($"start loading home scene");
        var handle = Addressables.LoadSceneAsync(homeSceneKey);
        LoadingProgress(homeSceneKey, handle).Forget();

        var sceneInstance = await handle.ToUniTask();
        Addressables.Release(handle);
    }

    

    async UniTaskVoid LoadingProgress(string key, AsyncOperationHandle handle)
    {
        Debug.Log($"start download: {key}");
        while (!handle.IsDone)
        {
            Debug.Log($"loading...{handle.GetDownloadStatus().Percent}");
            await UniTask.Delay(100);
        }
    }
}
