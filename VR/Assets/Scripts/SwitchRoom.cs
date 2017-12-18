using UnityEngine;

public class SwitchRoom : MonoBehaviour {

    public int x, y;
    GameObject gamemanager;
    public Light light;
    public float normalIntensity, hiIntensity;
    Room room;

    private void Start()
    {
        room = GameObject.Find("GameManager").GetComponent<Room>();
    }

    public void ChangeRoom() {
        room.SetNewRoom(x, y);
    }

    public void HightLight()
    {
        light.intensity += .2f;
    }

    private void Update()
    {
        light.intensity -= .1f;
        if (light.intensity >= hiIntensity)
        {
            light.intensity = hiIntensity;
        }
        if(light.intensity < normalIntensity) {
            light.intensity = normalIntensity;
        }
    }


}
