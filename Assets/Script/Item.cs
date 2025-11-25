using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 20;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);        
    }
}
