using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopup : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Setup();
        DestroyAfter(1.1f);
    }

    private void Update()
    {
        // speed * time
        float step = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 5f), step);
    }

    public void Setup()
    {
        textMesh.SetText("Level Up!");
        textMesh.color = Color.red;
    }

    // TODO: object pooler
    private void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
