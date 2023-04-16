using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRng : MonoBehaviour
{
    private Animator anim;
    private int randomId;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        randomId = Animator.StringToHash("Random");
    }

    // Update is called once per frame
    void Update()
    {
        if(anim != null)
            anim.SetFloat(randomId, Random.Range(0, 100f));
    }
}
