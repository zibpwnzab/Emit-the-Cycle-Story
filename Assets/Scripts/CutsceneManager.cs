using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public Animator animator; // Ссылка на аниматор контроллер
    public GameObject player; // Ссылка на игрока
    public Transform cutscenePosition; // Позиция, куда переместится игрок во время катсцены

    public IEnumerator PlayCutscene()
    {
        // Отключаем управление игроком
        player.GetComponent<PlayerController>().enabled = false;

        // Перемещаем игрока в нужную позицию

        animator.SetTrigger("StartCutscene");

        // Ожидаем завершения катсцены
        yield return new WaitForSeconds(0.3f);
        player.transform.position = cutscenePosition.position;

        // Включаем управление игроком
        player.GetComponent<PlayerController>().enabled = true;

        // Дополнительные действия после катсцены
    }
}
