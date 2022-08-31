using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hitEffectController : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float destroyTime;
    private void Start()
    {
        Invoke("DestroyHitEffect", destroyTime);
    }

    void DestroyHitEffect()
    {
        Destroy(gameObject);
    }
    public void ShowDamage(float damage )
    {
        text.text = "-"+damage.ToString();
    }
}
