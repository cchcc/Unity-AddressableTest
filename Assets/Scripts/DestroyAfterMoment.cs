using UnityEngine;

public class DestroyAfterMoment : MonoBehaviour
{
    public float second = 2;
    
    void Start()
    {
        Invoke(nameof(DestroyAfter), second);
    }

    private void DestroyAfter()
    {
        Destroy(gameObject);
    }
}
