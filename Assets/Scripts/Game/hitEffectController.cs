using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hitEffectController : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float destroyTime;
    bool changed = false;
    
    private void Start()
    {
        //Invoke("DestroyHitEffect", destroyTime);
    }

    void DestroyHitEffect()
    {
        //if (!changed)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
            Destroy(gameObject);
        //}
    }
    public void ShowDamage(float damage)
    {
        text.text = "-" + damage.ToString();
    }
    public void ShowDamage(float damage, float _destroyTime)
    {
        text.text = "-" + damage.ToString();
        changed = true;
        destroyTime = _destroyTime;
    }
    public void ShowDamage()
    {
        text.text = string.Empty;
    }
    private void FixedUpdate()
    {
        if (destroyTime > 0)
        {
            destroyTime -= Time.fixedDeltaTime;
            transform.position = new Vector2(transform.position.x, transform.position.y + Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
