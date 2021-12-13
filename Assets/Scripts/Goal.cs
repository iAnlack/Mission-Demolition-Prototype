using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool GoalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        // Когда в область действия триггера попадает что-то, проверить является ли это "что-то" снарядом
        if (other.gameObject.tag == "Projectile")
        {
            // Если это снаряд, присвоить полю GoalMet значение true
            Goal.GoalMet = true;
            // Так же изменить альфа-канал цвета, чтобы увеличить непрозрачность
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
