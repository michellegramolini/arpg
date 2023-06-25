using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour, IPooledObject
{
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        // speed * time
        float step = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 5f), step);
    }

    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
    }

    // TODO: object pooler
    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectPooler.Instance.Recycle("damage_popup", this.gameObject);
        //Destroy(gameObject, seconds);
    }

    public void OnObjectSpawn()
    {
        StartCoroutine(DestroyAfter(1.1f));
    }

    // TODO: grab prefab from object pooler
    //public static DamagePopup Create()
    //{

    //}
}
