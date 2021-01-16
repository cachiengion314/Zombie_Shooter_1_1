using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WayOutEvent : MonoBehaviour {
    public string warningText;
    private bool isCollided;
    private void OnTriggerEnter2D(Collider2D collision) {
        isCollided = true;
        if (collision.CompareTag("Player")
            && GameManager.instance.GemsCount >= 6) {

            EventManager.Instance.UpdateEvent("HeroController_WayOutEvent_EndLevel",
                 new PopupEventInfo {
                     title = "Congratulation",
                     content = " \n You have done well! Would you like to go back to the main menu?",
                     okCallback = () => { SceneManager.LoadScene(0); }
                 });

        } else if (collision.CompareTag("Player")
             && GameManager.instance.GemsCount < 6) {

            EventManager.Instance.UpdateEvent("WayOutEvent_DenyPlayerEnterText",
                new TextEventInfo {
                    text = warningText,
                    position = collision.transform.position + GunsMesh.GetRandomDirection(),
                    isCollided = isCollided,
                });
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        isCollided = false;
        EventManager.Instance.UpdateEvent("WayOutEvent_DenyPlayerExitText",
             new TextEventInfo {
                 text = warningText,
                 position = collision.transform.position + GunsMesh.GetRandomDirection(),
                 isCollided = isCollided,
             });
    }
}
