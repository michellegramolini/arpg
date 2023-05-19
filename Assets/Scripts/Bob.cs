using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    [SerializeField]
    private float _bobForce = 10f;
    [SerializeField]
    private float _bobSpeed = 8f;
    [SerializeField]
    private bool _bobLeftRight;

    private float _startYPos;
    private float _startXPos;


    // Start is called before the first frame update
    void Start()
    {
        this._startYPos = this.transform.position.y;
        this._startXPos = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_bobLeftRight)
        {
            transform.position = new Vector3(transform.position.x, _startYPos + ((float)Mathf.Sin(Time.time * _bobSpeed) * _bobForce), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(_startXPos + ((float)Mathf.Sin(Time.time * _bobSpeed) * _bobForce), transform.position.y, transform.position.z);
        }

    }
}
