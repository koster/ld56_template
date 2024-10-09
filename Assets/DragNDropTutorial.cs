using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DragNDropTutorial : MonoBehaviour
{
    public Sprite up;
    public Sprite dn;

    public SpriteRenderer spriteRen;
    
    Transform fromt;
    Transform tot;

    void Start()
    {
        spriteRen.enabled = false;
    }

    public void Show(Transform from, Transform to)
    {
        fromt = from;
        tot = to;
        
        spriteRen.enabled = true;
        
        StopAllCoroutines();
        StartCoroutine(TutorializeDrag());
    }

    IEnumerator TutorializeDrag()
    {
        while (true)
        {
            Debug.Log( "from");
            spriteRen.transform.position = fromt.position;
            spriteRen.sprite = up;

            yield return new WaitForSeconds(0.25f);
            
            spriteRen.sprite = dn;

            yield return new WaitForSeconds(0.15f);

            yield return spriteRen.transform.DOMove(tot.transform.position, 1f).WaitForCompletion();
            Debug.Log( "to");
            
            yield return new WaitForSeconds(0.15f);
            
            spriteRen.sprite = up;
            
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void Hide()
    {
        spriteRen.enabled = false;
        StopAllCoroutines();
    }
}
