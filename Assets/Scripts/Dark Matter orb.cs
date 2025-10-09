using UnityEngine;
using System.Collections;

public class DarkMatterOrb : MonoBehaviour
{
    public GameObject darkWebObject;  // Reference to existing web in scene
    public float webDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision detected with dark matter orb");
            
            
            if (darkWebObject != null)
            {
                darkWebObject.SetActive(true);
                
                
                MonoBehaviour webMono = darkWebObject.GetComponent<MonoBehaviour>();
                if (webMono == null)
                {
                    
                    WebAutoDestroy autoDestroy = darkWebObject.AddComponent<WebAutoDestroy>();
                    autoDestroy.lifetime = webDuration;
                }
                else
                {
                    
                    webMono.StartCoroutine(DeactivateAfterDelay(darkWebObject, webDuration));
                }
            }
            
            Destroy(gameObject); 
        }
    }

    IEnumerator DeactivateAfterDelay(GameObject web, float delay)
    {
        yield return new WaitForSeconds(delay);
        web.SetActive(false); 
    }
}

public class WebAutoDestroy : MonoBehaviour
{
    public float lifetime = 10f;
    
    void Start()
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}