using Sunbox.Avatars;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRandomizer : MonoBehaviour
{
    public AvatarCustomization avatar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomizeAvatar());
    }

    private IEnumerator RandomizeAvatar()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            avatar.RandomizeBodyParameters();
        }
        catch (Exception e)
        {
            Debug.LogError("RandomizeBodyParameters failed:\n" + e);
        }

        try
        {
            avatar.RandomizeClothing();
        }
        catch (Exception e)
        {
            Debug.LogError("RandomizeClothing failed:\n" + e);
        }
    }
}
