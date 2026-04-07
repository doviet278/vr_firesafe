using UnityEngine;

public class FlammableMaterial : MonoBehaviour
{
    [Header("Material Properties")]
    public FireClass fireClass = FireClass.A_Solid;
    public float maxFuel = 100f; // Lượng nhiên liệu (Giây cháy)
    public float currentFuel;

    [Header("Ignition Settings")]
    public float ignitionTemperature = 50f; // Nhiệt độ bắt cháy
    public float currentTemperature = 25f; // Nhiệt độ môi trường mặc định
    public bool isBurning = false;

    [Header("Fire Entity")]
    public GameObject fireNodePrefab; // Prefab ngọn lửa sẽ sinh ra
    private GameObject currentFireNode;
    private SceneTrainExtinguisherManager trainExtinguisherManager;

    void Start()
    {
        currentFuel = maxFuel;
        GameObject obj = GameObject.Find("trainExtinguisherManager");
        if (obj != null)
        {
            trainExtinguisherManager = obj.GetComponent<SceneTrainExtinguisherManager>();
        }
    }

   void Update()
    {
        if (!isBurning)
        {
            // BỔ SUNG: Chỉ bốc cháy khi CÒN NHIÊN LIỆU và đủ nhiệt độ
            if (currentFuel > 0 && currentTemperature >= ignitionTemperature)
            {
                Ignite();
            }
            // Nếu chưa đủ nhiệt độ, HOẶC đã cháy rụi hết nhiên liệu -> Từ từ tản nhiệt về mức an toàn
            else if (currentTemperature > 25f)
            {
                currentTemperature -= Time.deltaTime * 2f;
            }
        }
    }

    // Nguồn nhiệt khác (hoặc bật lửa) gọi hàm này để truyền nhiệt vào vật
    public void AddHeat(float heatAmount)
    {
        if (isBurning || currentFuel <= 0) return;

        currentTemperature += heatAmount;
        
        if (currentTemperature >= ignitionTemperature)
        {
            Ignite();
        }
    }

    private void Ignite()
    {
        if (currentFuel <= 0) return;
        isBurning = true;
        Debug.Log(gameObject.name + " DA BUI CHAY!");

        if (fireNodePrefab != null)
        {
            // Sinh ra ngọn lửa ngay tại tâm vật thể
            currentFireNode = Instantiate(fireNodePrefab, this.transform.position, Quaternion.identity);
            
            // Gắn ngọn lửa vào làm con của vật thể này
            currentFireNode.transform.SetParent(this.transform); 
            
            // Báo cho ngọn lửa biết nó đang đốt vật nào
            FireNode fireScript = currentFireNode.GetComponent<FireNode>();
            if (fireScript != null)
            {
                fireScript.Initialize(this);
            }
        }
    }

    // Bị ngọn lửa rút cạn nhiên liệu
    public void ConsumeFuel(float amount)
    {
        currentFuel -= amount;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            isBurning = false;
            // Đổi màu thành đen thui (Tro tàn)
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr != null) mr.material.color = Color.black;
            
        }
    }

    // Hàm gọi khi lửa bị dập tắt
    public void Extinguish()
    {
        isBurning = false;
        currentTemperature = 25f; 
        if (currentFireNode != null)
        {
            trainExtinguisherManager?.ReportFireExtinguished();
            Destroy(currentFireNode);
        }
    }
}