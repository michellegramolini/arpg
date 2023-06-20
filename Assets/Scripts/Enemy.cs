using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    private SpriteRenderer _sr;

    public void Hit()
    {
        //Debug.Log("hit!");
        //_isHit = true;
        StartCoroutine(HitDebug());
    }

    // Start is called before the first frame update
    void Start()
    {
        _sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator HitDebug()
    {
        _sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.blue;
    }
}
