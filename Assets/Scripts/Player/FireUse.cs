using System.Collections;
using UnityEngine;

public class FireUse : MonoBehaviour
{
    [SerializeField] private GameObject fireVFX;
    [SerializeField] private Animator fireAnimator;
    [SerializeField] private Light fireLight;
    [SerializeField] private float lightDuration = 2f;

    private Coroutine _activeRoutine;

    public void UseFire()
    {
        if (_activeRoutine != null)
        {
            StopCoroutine(_activeRoutine);
        }
        _activeRoutine = StartCoroutine(ActivateFireRoutine());
    }

    private IEnumerator ActivateFireRoutine()
    {
        if (fireVFX != null) fireVFX.SetActive(true);
        if (fireAnimator != null) fireAnimator.SetTrigger("Activate");

        if (fireLight != null)
        {
            fireLight.enabled = true;
            yield return new WaitForSeconds(lightDuration);
            fireLight.enabled = false;
        }
        else
        {
            yield return new WaitForSeconds(lightDuration);
        }

        if (fireVFX != null) fireVFX.SetActive(false);
        _activeRoutine = null;
    }
}