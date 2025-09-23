using UnityEngine;

public class ObjectsThrower : MonoBehaviour
{
    [SerializeField] private GameObject _potionPrefab;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ThrowPotion();
        }
    }

    private void ThrowPotion()
    {
        Potion potion = Instantiate(_potionPrefab, transform.position, Quaternion.identity).GetComponent<Potion>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        potion.targetPoint = mousePos;
    }
}
