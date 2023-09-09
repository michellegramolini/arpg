using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopup : MonoBehaviour, IPooledObject
{
    private TextMeshPro textMesh;
    public float destroyTimeSeconds;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        float step = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 5f), step);
    }

    public void Setup(int level)
    {
        textMesh.SetText($"Level {level}!");
        textMesh.color = Color.red;
    }

    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectPooler.Instance.Recycle("damage_popup", this.gameObject);
    }

    public void OnObjectSpawn()
    {
        StopCoroutine(nameof(DestroyAfter));
        StartCoroutine(DestroyAfter(destroyTimeSeconds));
    }
}
