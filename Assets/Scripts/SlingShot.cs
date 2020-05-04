using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Threading;
class SoliderModel
{
    public Vec3f pos { get; set; }
    public Vec3f velocity { get; set; }
}

class Vec3f
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

public class SlingShot : MonoBehaviour
{
    static private SlingShot S;

    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public Material[] materials;
    public float velocityMult = 8f;
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile; // b
    public bool aimingMode;
    private Rigidbody projectileRigidbody;
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    private SoliderModel _solider;//модель збереження координат точок з бек-енда

    //*********************************************
    public Material Material_In;
    private int i = 0;
    //*********************************************
    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // b
        launchPos = launchPointTrans.position;
    }
    // Start is called before the first frame update
    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false); //    
    }
    void OnMouseDown()
    { // d
      // Игрок нажал кнопку мыши, когда указатель находился над рогаткой
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate(prefabProjectile) as GameObject;
        //List<Component> hingeJoints = new List<Component>();
        //projectile.GetComponents(typeof(GameObject), hingeJoints);
        //Debug.Log(hingeJoints.ToString());

        //******************************************************************************

        if (i >= materials.Length)
        {
            i = 0;
        }
        Material[] mats = projectile.GetComponent<Renderer>().materials;
        mats[0] = materials[i];
        projectile.GetComponent<Renderer>().materials = mats;
        i++;

        //******************************************************************************

        // Поместить в точку launchPoint
        projectile.transform.position = launchPos;
        // Сделать его кинематическим
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    void Start()
    {
        int id = Thread.CurrentThread.ManagedThreadId;
        StartCoroutine(GetRequest("http://13.66.95.204/api/game"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string text = webRequest.downloadHandler.text;
                _solider = JsonConvert.DeserializeObject<SoliderModel>(text);
                Shoot();
            }
        }
    }
   
    void Shoot()
    {
        projectile = Instantiate(prefabProjectile) as GameObject;
        int id = Thread.CurrentThread.ManagedThreadId;
        if (i >= materials.Length)
        {
            i = 0;
        }
        Material[] mats = projectile.GetComponent<Renderer>().materials;
        mats[0] = materials[i];
        projectile.GetComponent<Renderer>().materials = mats;
        i++;

        // Сделать его кинематическим
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;

        Vector3 myPos = new Vector3(_solider.pos.x, _solider.pos.y, _solider.pos.z);
        projectile.transform.position = myPos;

        projectileRigidbody.isKinematic = false;

        Vector3 v = new Vector3(_solider.velocity.x, _solider.velocity.y, _solider.velocity.z);
        projectileRigidbody.velocity = v;

        FollowCam.POI = projectile;
        projectile = null;


        MissionDemolition.ShotFired(); // a
        ProjectileLine.S.poi = projectile;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aimingMode) return;
        Vector3 mousePos2D = Input.mousePosition; // с
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        { // e
          // Кнопка мыши отпущена
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;


            MissionDemolition.ShotFired(); // a
            ProjectileLine.S.poi = projectile;
        }
    }
}
