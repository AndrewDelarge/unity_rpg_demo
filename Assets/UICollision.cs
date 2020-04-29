using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollision : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other);
    }

}
