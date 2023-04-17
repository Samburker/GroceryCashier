using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [System.Serializable]
    public struct DoorDefinition
    {
        public Transform door;
        public Transform closePosition;
        public Transform openPosition;
    }

    public LayerMask layerMask;
    public DoorDefinition[] doors;
    public float doorPosition = 0;
    private float _previousDoorPosition = -1f;
    public float doorSpeed = 2f;
    public int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) == 0)
            return;
        count++;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) == 0)
            return;
        count--;
    }

    // Update is called once per frame
    void Update()
    {
        float desiredPosition = count > 0 ? 1f : 0f;

        if(count > 0)
            doorPosition += doorSpeed * Time.deltaTime;
        else
            doorPosition -= doorSpeed * Time.deltaTime;

        UpdateDoorPosition();
    }

    private void UpdateDoorPosition()
    {
        doorPosition = Mathf.Clamp01(doorPosition);
        //if (_previousDoorPosition == doorPosition)
        //    return;
        foreach (var d in doors)
        {
            d.door.position = Vector3.Lerp(d.closePosition.position, d.openPosition.position, Mathf.SmoothStep(0f, 1f, doorPosition));
        }
        _previousDoorPosition = doorPosition;
    }
}
