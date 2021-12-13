using System.Collections;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject CloudPrefab; // Шаблон для облаков
    public Vector3 CloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 CloudPosMax = new Vector3(150, 100, 10);
    public int NumClouds = 40; // Число облаков
    public float CloudScaleMin = 1; // Мин. масштаб каждого облака
    public float CloudScaleMax = 3; // Макс. масштаб каждого облака
    public float CloudSpeedMult = 0.5f; // Коэффициент скорости облаков

    private GameObject[] _cloudInstances;

    private void Awake()
    {
        // Создать массив для хранения всех экземпляров облаков
        _cloudInstances = new GameObject[NumClouds];
        // Найти родительский игровой объект CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Создать в цикле заданное количество облаков
        GameObject cloud;
        for (int i = 0; i < NumClouds; i++)
        {
            // Создать экземпляр класса CloudPrefab
            cloud = Instantiate<GameObject>(CloudPrefab);
            // Выбрать местоположение для облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(CloudPosMin.x, CloudPosMax.x);
            cPos.y = Random.Range(CloudPosMin.y, CloudPosMax.y);
            // Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(CloudScaleMin, CloudScaleMax, scaleU);
            // Меньшие облака (с меньшим значением scaleU) должны быть ближе к земле
            cPos.y = Mathf.Lerp(CloudPosMin.y, cPos.y, scaleU);
            // Меньшие облака должны быть дальше
            cPos.z = 100 - 90 * scaleU;
            // Применить полученные значения  координат и масштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Сделать облако дочерним по отношению к anchor
            cloud.transform.SetParent(anchor.transform);
            // Добавить облако в массив _cloudInstances
            _cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        // Обойти в цикле все созданные облака
        foreach (GameObject cloud in _cloudInstances)
        {
            // Получить масштаб и координаты облака
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Увеличить скорость для ближних облаков
            cPos.x -= scaleVal * Time.deltaTime * CloudSpeedMult;
            // Если облако сместилось слишком далеко влево...
            if (cPos.x <= CloudPosMin.x)
            {
                // Переместить его далеко вправо
                cPos.x = CloudPosMax.x;
            }
            cloud.transform.position = cPos;
        }
    }
}
