using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafte : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40; //число облоков
    public GameObject cloudPrefab; //Шаблон для облоков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10); //
    public Vector3 cloudPosMax = new Vector3(150, 100, 10); //
    public float cloudScaleMin = 1; //Мін. маштаб каождого облоко
    public float cloudScaleMax = 3; //Макс. маштаб каождого облоко
    public float cloudSpeedMult = 0.5f; //Коефициент скорости облаков

    private GameObject[] cloudInstances;

    private void Awake()
    {
        //Создаем масив для хранение всех экзепляров облаков 
        cloudInstances = new GameObject[numClouds];
        //Найти родительский игровой обект CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Cоздаем в цикле заданое количество облаков
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //Создаем экземпляр cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            //Выбираем место расположение для облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //маштабируем облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Меньшие облака (с меньшим значением scaleU) должны быть ближе
            //к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //Меньшие облака должны быть дальше
            cPos.z = 100 - 90 * scaleU;
            //Применяем полученые значение координат и маштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Сделаем облако дочерним по отношению к anchor
            cloud.transform.SetParent(anchor.transform);
            //Добавлем облако в масив cloudInstances
            cloudInstances[i] = cloud;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Обойти в цыкле все созданые облака
        foreach (GameObject cloud in cloudInstances)
        {
            //получить маштаб и координаты облака
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //Увеличить скорость для ближних облаков
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //Если облако сместилось слишком 
            if (cPos.x <= cloudPosMin.x)
            {
                //
                cPos.x = cloudPosMax.x;
            }
            //
            cloud.transform.position = cPos;
        }
    }
}
