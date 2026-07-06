using System.Collections;
using UnityEngine;

public class FireUse : MonoBehaviour
{
    [SerializeField] private GameObject fireVFX; 
    [SerializeField] private Animator fireAnimator; 
    [SerializeField] private Light fireLight; 
    [SerializeField] private float lightDuration = 2f; 


    public void UseFire()
    {
        Debug.Log("Fire used!");
        StartCoroutine(ActivateFire());
    }

    private IEnumerator ActivateFire()
    {

        if (fireVFX != null)
        {
            fireVFX.SetActive(true);
        }

    
        if (fireAnimator != null)
        {
            fireAnimator.SetTrigger("Activate");
        }

     
        if (fireLight != null)
        {
            fireLight.enabled = true;
            yield return new WaitForSeconds(lightDuration);
            fireLight.enabled = false; 
        }

    
        if (fireVFX != null)
        {
            fireVFX.SetActive(false);
        }
    }
}
