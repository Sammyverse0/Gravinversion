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
            
            // Activate the web
            if (darkWebObject != null)
            {
                darkWebObject.SetActive(true);
                
                // Start coroutine on the web itself (since it's now active)
                MonoBehaviour webMono = darkWebObject.GetComponent<MonoBehaviour>();
                if (webMono == null)
                {
                    // If web has no MonoBehaviour, add the auto-destroy component
                    WebAutoDestroy autoDestroy = darkWebObject.AddComponent<WebAutoDestroy>();
                    autoDestroy.lifetime = webDuration;
                }
                else
                {
                    // Or start coroutine on the web's existing component
                    webMono.StartCoroutine(DeactivateAfterDelay(darkWebObject, webDuration));
                }
            }
            
            Destroy(gameObject); // Destroy the orb
        }
    }

    IEnumerator DeactivateAfterDelay(GameObject web, float delay)
    {
        yield return new WaitForSeconds(delay);
        web.SetActive(false); // Deactivate instead of destroy
    }
}

public class WebAutoDestroy : MonoBehaviour
{
    public float lifetime = 5f;
    
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