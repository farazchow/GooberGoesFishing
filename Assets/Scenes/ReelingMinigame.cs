using UnityEngine;
using UnityEngine.InputSystem;



public class ReelingMinigame : MonoBehaviour
{
    public GameObject player;
    public float scaleAmount;
    private float playerScaleX;
    private float playerScaleY;
    private InputAction mashButton;
    private int mashPresses = 0;
    private Animator gooberAnimator;
    private int mashHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScaleX = player.transform.localScale.x;
        playerScaleY = player.transform.localScale.y;
        mashButton = InputSystem.actions.FindAction("General Action");
        gooberAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mashButton.WasPerformedThisFrame())
        {
            playerScaleX += scaleAmount;
            playerScaleY += scaleAmount;
            mashPresses++;
            Debug.Log($"Mashed {mashPresses} times!");
            player.transform.localScale = new Vector3(playerScaleX, playerScaleY, player.transform.localScale.z);
            gooberAnimator.SetTrigger("Mash");
        }
    }
}
