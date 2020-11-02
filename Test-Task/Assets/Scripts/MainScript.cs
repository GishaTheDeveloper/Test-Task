using System.IO;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    #region Singleton
    public static MainScript Instance { private set; get; }
    #endregion

    [Header("Asset Bundles Settings")]
    public string assetBundlePath;
    
    [Header("Spawning")]
    public string jsonOfNamesPath;
    public Transform parentForObjects;
    public GameObject testObjet;

    [Header("Game Data")]
    public GameData gameData;

    public class ObjectsNames
    { public string[] names; }
    ObjectsNames objectsNames;

    AssetBundle loadedAssetBundle;
    LayerMask raycastMask;
    Camera cam;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;

        // Создаем LayerMask для 9 слоя (Raycast).
        raycastMask = 1 << 9;

        // Загружаем имена объектов из локального json файла.
        objectsNames = SerializeJsonFromPath(jsonOfNamesPath);
    }

    void Start()
    {
        LoadLocalAssetBundle();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RaycastHitGeometryObject(out GeometryObjectModel o, out Vector3 contact))
                o.OnClick();
            else
                InstantiateObject(objectsNames.names[Random.Range(0, objectsNames.names.Length)], contact);
        }
    }

    // Спавним префаб, используя данные AssetBundle
    void InstantiateObject(string prefabName, Vector3 position)
    {
        var prefab = loadedAssetBundle.LoadAsset<GameObject>(prefabName);

        Instantiate(prefab, position, prefab.transform.rotation, parentForObjects);
    }

    bool RaycastHitGeometryObject(out GeometryObjectModel outObject, out Vector3 outContactPoint)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 999f, raycastMask))
        {
            if (hitInfo.collider.CompareTag("GeometryObject"))
            {
                outObject = hitInfo.collider.GetComponent<GeometryObjectModel>();
                outContactPoint = hitInfo.point;
                return true;
            }
        }

        outObject = null;
        outContactPoint = hitInfo.point;
        return false;
    }

    void LoadLocalAssetBundle()
    {
        loadedAssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        if (loadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
    }

    ObjectsNames SerializeJsonFromPath(string path)
    {
        string json = File.ReadAllText(jsonOfNamesPath) as string;
        ObjectsNames result = JsonUtility.FromJson<ObjectsNames>(json);

        if (string.IsNullOrWhiteSpace(json) || result.names.Length == 0)
        {
            Debug.LogError("JSON file is empty!");
            return null;
        }

        return result;
    }
}
