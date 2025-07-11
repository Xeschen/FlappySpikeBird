using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private Action onCollected;

    public void SetOnCollectedCallback(Action callback)
    {
        onCollected = callback;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bird"))
        {
            onCollected?.Invoke();
        }
    }
}
