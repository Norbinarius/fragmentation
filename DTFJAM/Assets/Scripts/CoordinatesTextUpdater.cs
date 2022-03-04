using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CoordinatesTextUpdater : MonoBehaviour
{
    [SerializeField] private ShipController ship;
    private float delay = 0.05f;
    private Text coordinates;

    private void Start()
    {
        coordinates = this.GetComponent<Text>();
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            coordinates.text = "X: " + ((int)ship.transform.position.x).ToString() + 
                "\nY: " + ((int)ship.transform.position.y).ToString();
        }
    }
}
