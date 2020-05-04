using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    //Поля, установленые в инспекторе Unity
    [Header("Set in Inspector")]
    public GameObject cloudShpere;
    public int numSpheresMin = 6;
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(10, 2, 1);
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleMin = 2f;

    private List<GameObject> spheres;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudShpere);
            spheres.Add(sp);
            Transform spTans = sp.transform;
            sp.transform.SetParent(this.transform);

            //выбираем случайное место расположение
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTans.localPosition = offset;

            //Выбираем случайный маштаб
            Vector3 scale = Vector3.one;
            scale.x *= Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y *= Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z *= Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //Скоректировать маштаб y по растоянию x от центра
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleMin);

            spTans.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Restart();
        //}
    }
    void Restart()
    {
        //Удаляем страые сферы, составляющие облако
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
