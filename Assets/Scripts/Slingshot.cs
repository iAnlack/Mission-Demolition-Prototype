using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    // Поля, устанавливаемые в Инспекторе
    [Header("Set in Inspector")]
    public GameObject PrefubProjectule;
    public float VelocityMult = 8f;

    // Поля, устанавливаемые динамическ
    [Header("Set Dynamically")]
    public GameObject LaunchPoint;
    public Vector3 LaunchPos;
    public GameObject Projectile;
    public bool AimingMode;

    private Rigidbody _projectileRigidbody;

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        LaunchPoint = launchPointTrans.gameObject;
        LaunchPoint.SetActive(false);
        LaunchPos = launchPointTrans.position;
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Slingshot: OnMouseEnter()");
        LaunchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //Debug.Log("Slingshot: OnMouseExit()");
        LaunchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        // Игрок нажал кнопку мыши, когда указатель находился над рогаткой
        AimingMode = true;
        // Создать снаряд
        Projectile = Instantiate(PrefubProjectule) as GameObject;
        // Поместить точку в launchPoint
        Projectile.transform.position = LaunchPos;
        // Сделать его кинематическим
        _projectileRigidbody = Projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        // Если рогатка не в режиме прицеливания, не выполнять этот код
        if (!AimingMode)
        {
            return;
        }

        // Получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Найти разность координат между LaunchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;
        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude >maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Передвинуть снаряд в новую позицию
        Vector3 projPos = LaunchPos + mouseDelta;
        Projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            // Кнопка мыши отпущена
            AimingMode = false;
            _projectileRigidbody.isKinematic = false;
            _projectileRigidbody.velocity = -mouseDelta * VelocityMult;
            FollowCam.POI = Projectile;
            MissionDemolition.ShotFired();
            ProjectileLine.S.PointOfInterest = Projectile;
            Projectile = null;
        }
    }

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null)
            {
                return Vector3.zero;
            }
            return S.LaunchPos;
        }
    }
}
