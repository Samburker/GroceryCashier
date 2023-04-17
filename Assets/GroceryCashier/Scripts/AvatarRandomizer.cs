using Sunbox.Avatars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRandomizer : MonoBehaviour
{
    public AvatarCustomization avatar;

    // Start is called before the first frame update
    void Start()
    {
        avatar.RandomizeBodyParameters();
        avatar.RandomizeClothing();
    }
}
