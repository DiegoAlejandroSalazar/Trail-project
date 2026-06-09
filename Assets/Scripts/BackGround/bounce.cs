using UnityEngine;

public class bounce : MonoBehaviour
{
    [SerializeField] float speed = 40f;
    readonly Vector3[] directions = new Vector3[] { new(1, 0, 0), new(0, 1, 0), new(0, -1, 0), new(-1, 0, 0) };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        int indexDirection = Random.Range(0, 4);
        transform.Translate(speed * Time.deltaTime * directions[indexDirection]);
    }
}
