using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Utils
{
    public static async UniTaskVoid LogDownloadProgress(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            var downloadStatus = handle.GetDownloadStatus();
            var downloadPercent = downloadStatus.Percent;
            var completePercent = handle.PercentComplete;
            Debug.Log(
                $"loading... downLoadPercent(IsDone:{downloadStatus.IsDone}):{downloadPercent}, completePercent:{completePercent}");
            await UniTask.Delay(200);
        }
    }
    
    public static async UniTaskVoid LogLoad(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            var downloadStatus = handle.GetDownloadStatus();
            var downloadPercent = downloadStatus.Percent;
            var completePercent = handle.PercentComplete;
            Debug.Log(
                $"loading...(IsDone:{downloadStatus.IsDone}):{downloadPercent}, completePercent:{completePercent}");
            await UniTask.Delay(200);
        }
    }
}