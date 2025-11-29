using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemState itemState;
    [SerializeField] float rotateSpeed = 20;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);        
    }
}
