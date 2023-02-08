using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    public LayerMask layerMask;
    public int count = 0;
    private int _previousCount = 0;

    public UnityEvent<int> objectCountChanged;
    public UnityEvent<Collider> triggerEnter;
    public UnityEvent<Collider> triggerExit;

    // Update is called once per frame
    void Update()
    {
        if (_previousCount == count)
            return;
        objectCountChanged?.Invoke(count);
        _previousCount = count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            count++;
            triggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            count--;
            triggerExit?.Invoke(other);
        }
    }
}
