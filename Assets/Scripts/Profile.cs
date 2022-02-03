using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Profile : MonoBehaviour
{
    public void ClickedBack()
    {
        var profileSceneKey = "Assets/Scenes/HomeScene.unity";
        var handle = Addressables.LoadSceneAsync(profileSceneKey);
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
}
