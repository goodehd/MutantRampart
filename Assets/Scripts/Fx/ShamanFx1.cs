using UnityEngine;

public class ShamanFx1 : MonoBehaviour
{
    private Vector3 dir;
    private float dis;
    public Vector3 Pos { get; set; }

    private void Start()
    {
        Initialized();
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void Initialized()
    {
        dir = Vector3.up;
        dis = 0f;
    }

    void Update()
    {
        Vector3 movement = dir * 1.2f * Time.deltaTime;
        dis += movement.magnitude;
        transform.position += movement;

        if(dis >= 0.7f)
        {
            GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}ShamanFx2");
            Vector3 targetpos = Pos;
            targetpos.x += -0.7f;
            targetpos.y += 0.25f;
            go.transform.position = targetpos;
            Main.Get<ResourceManager>().Destroy(gameObject);
        }
    }
}
