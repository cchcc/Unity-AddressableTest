using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Home : MonoBehaviour
{
    public void ClickedProfile()
    {
        var key = "Assets/Scenes/ProfileScene.unity";
        Debug.Log($"LoadScene: {key}");
        var handle = Addressables.LoadSceneAsync(key);
        Utils.LoadingProgress(handle).Forget();
        handle.Completed += CompletedLoadScene;
        LoadingProgress(key, handle).Forget();
    }

    private void CompletedLoadScene(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            // 실패인경우 OperationException : ChainOperation failed because dependent operation failed
            Debug.Log(handle.OperationException);  
        }
    }
    
    
    
    async UniTaskVoid LoadingProgress(string key, AsyncOperationHandle handle)
    {
        
        // await UniTask.Run(async () =>
        // {
            Debug.Log($"start download: {key}");
            while (!handle.IsDone)
            {
                Debug.Log($"loading...{handle.GetDownloadStatus().Percent}");
                await UniTask.Delay(100);
            }
        // }, true, this.GetCancellationTokenOnDestroy());
    }
}
