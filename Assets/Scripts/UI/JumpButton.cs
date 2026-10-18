using UnityEngine;
using UnityEngine.EventSystems;


public class JumpButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController player = PlayerController.instance != null
            ? PlayerController.instance
            : FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            player.StartCoroutine(player.Jump());
        }
    }

}
