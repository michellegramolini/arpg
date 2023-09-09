using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TODO: base popup class
public class DamagePopup : MonoBehaviour, IPooledObject
{
    protected TextMeshPro _textMesh;
    public float destroyTimeSeconds;

    private void Awake()
    {
        _textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        // speed * time
        float step = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 5f), step);
    }

    public virtual void Setup(int damageAmount)
    {
        _textMesh.SetText(damageAmount.ToString());
        _textMesh.color = Color.yellow;
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
