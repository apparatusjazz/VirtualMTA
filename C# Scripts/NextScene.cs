using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    void OnTriggerEnter(Collider FPSControllerr)
    {
        
    
         Scene sceneToLoad = SceneManager.GetSceneByName("Scenes/Broadway–LafayetteSt");
         SceneManager.LoadScene(sceneToLoad.name, LoadSceneMode.Additive);
         SceneManager.MoveGameObjectToScene(FPSControllerr.gameObject, sceneToLoad);
        

    }
}
