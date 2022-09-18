using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Entry : MonoBehaviour
{
    const string homeSceneKey = "Assets/Scenes/HomeScene.unity";

    async void Awake()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
    }
    
    public struct UserAttributes
    {
    }
    public struct AppAttributes
    {
    }

    private async UniTaskVoid InitUGS()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
    }

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
        var catalogKey = Application.platform == RuntimePlatform.Android ? "CATALOG_Android" : "CATALOG_iOS";
        
        var catalogPath = RemoteConfigService.Instance.appConfig.GetString(catalogKey);
        Debug.Log($"Path from remote config: {catalogPath}");
        var locator = await Addressables.LoadContentCatalogAsync(catalogPath);
        Debug.Log($"LoadContentCatalogAsync : {locator.Keys.Count()}");
        var catalogIds = await Addressables.CheckForCatalogUpdates();
        Debug.Log($"CheckForCatalogUpdates : {catalogIds.Count}");

        if (catalogIds is {Count: > 0})
        {
            await Addressables.UpdateCatalogs(catalogIds);
            Debug.Log($"UpdateCatalogs : {catalogIds.Count}");
        }
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
        Utils.LogDownloadProgress(handle).Forget();
        await handle.ToUniTask();
        await LoadHomeScene();
    }

    private async UniTask LoadHomeScene()
    {
        Debug.Log($"LoadScene: {homeSceneKey}");
        var handle = Addressables.LoadSceneAsync(homeSceneKey);
        Utils.LogLoad(handle).Forget();

        var sceneInstance = await handle.ToUniTask();
    }

    

    
}
