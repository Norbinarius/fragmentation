using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private Text endText;

    public void ActivateCamera()
    {
        Camera.main.GetComponent<CameraFollow>().enabled = true;
    }

    private IEnumerator RestartGameE(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void RestartGame(){
        StartCoroutine(RestartGameE());
    }

    public void ChangeEndText(string text){
        endText.text = text;
    }
}
