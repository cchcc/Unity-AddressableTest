using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Entry : MonoBehaviour
{
    void Start()
    {
        
    }

    public void ClickedGoHome()
    {
        var homeSceneKey = "Assets/Scenes/HomeScene.unity";
        var handle = Addressables.LoadSceneAsync(homeSceneKey);
        StartCoroutine(nameof(LogProgress), handle);
        handle.Completed += CompletedLoadScene;
    }

    private void CompletedLoadScene(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            // 실패인경우 OperationException : ChainOperation failed because dependent operation failed
            Debug.Log(handle.OperationException);  
        }
    }

    IEnumerator LogProgress(AsyncOperationHandle<SceneInstance> handle)
    {
        while (handle.IsDone == false)
        {
            Debug.Log($"home scene loading...{handle.GetDownloadStatus().Percent}");
            yield return new WaitForSeconds(.1f);
        }
    }
}
