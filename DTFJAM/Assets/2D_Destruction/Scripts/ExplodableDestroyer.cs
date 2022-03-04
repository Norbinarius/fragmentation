using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableDestroyer : MonoBehaviour
{
    public bool isExploded;
    public float timer;

    // Update is called once per frame
    void Update()
    {
       if(isExploded){
            timer += Time.deltaTime;
            if(timer > 2){
                GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, new Color(0,0,0,0), 0.002f);
            }
            if(timer > 12){
                Destroy(this.gameObject);
            }
        }
    }
}
