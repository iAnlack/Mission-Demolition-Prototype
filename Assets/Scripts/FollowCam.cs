using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на интересующий объект (POI - point of interest)

    [Header("Set in Inspector")]
    public float Easing = 0.05f;
    public Vector2 MinXY = Vector2.zero;

    [Header("Set in Dynamically")]
    public float CamZ; // Желаемая координата Z камеры

    private void Awake()
    {
        CamZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        //if (POI == null)
        //{
        //    return; // Выйти, если нет интересующего объекта
        //}

        //// Получить позицию интерусющего объекта
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        // Если нет интересующего объекта, то вернуть P: [0, 0, 0]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // Получить позицию интересующего объекта
            destination = POI.transform.position;
            // Если интересующий объект - снаряд, убедиться, что он остановился
            if (POI.tag == "Projectile")
            {
                // Если он стоит на месте (то есть не двигается)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // Вернуть исходные настройки поля зрения камеры
                    POI = null;
                    // в следующем кадре
                    return;
                }
            }
        }

        // Ограничить X и Y минимальными значениями
        destination.x = Mathf.Max(MinXY.x, destination.x);
        destination.y = Mathf.Max(MinXY.y, destination.y);
        // Определить точку между текущим местоположением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, Easing);
        // Принудительно установить значение destination.z равным CamZ, чтобы отодвинуть камеру подальше
        destination.z = CamZ;
        // Поместить камеру в позицию destination
        transform.position = destination;
        // Изменить размер orthographicSize камеры, чтобы земля оставалась в поле зрения
        Camera.main.orthographicSize = destination.y + 10;
    }
}
