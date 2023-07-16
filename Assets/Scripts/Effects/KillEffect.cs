using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEffect : MonoBehaviour, IPooledObject
{
    // TODO: make part of interface potentially
    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectPooler.Instance.Recycle("kill_effect", this.gameObject);
    }

    public void OnObjectSpawn()
    {
        // TODO: should be length of animation, although currently wouldn't be more than this.
        StopCoroutine(nameof(DestroyAfter));
        StartCoroutine(DestroyAfter(.5f));
    }

}
