using UnityEngine;

public class bounce : MonoBehaviour
{
	[SerializeField] float speed = 40f;
	Vector3[] directions = new Vector3[] {new Vector3(1,0,0),new Vector3(0,1,0),new Vector3(0,-1,0),new Vector3(-1,0,0)};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
	    int indexDirection = UnityEngine.Random.Range(0,4);
       transform.Translate(directions[indexDirection] * speed * Time.deltaTime);
    }
}
