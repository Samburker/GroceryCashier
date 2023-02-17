using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    public bool stopped;
    public float beltSpeed;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopped)
            return;
        Vector3 pos = rigidBody.position;
        rigidBody.position -= transform.forward * beltSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(pos);
    }

    public void StoppingItems(int count)
    {
        stopped = count > 0;
    }
}
