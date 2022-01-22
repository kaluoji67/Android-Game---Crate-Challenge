using UnityEngine;
using UnityEngine.UI;
public class updateScore : MonoBehaviour
{
    public MovementControl player;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = player.globalCounter.ToString();
    }
}
