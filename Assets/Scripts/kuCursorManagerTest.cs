using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class kuCursorManagerTest : MonoBehaviour
{
    public GameObject FocusedGameObject { get; private set; }

    [Tooltip("Drag the Cursor object to show when it hits a hologram.")]
    public GameObject CursorOnHolograms;

    [Tooltip("Drag the Cursor object to show when it does not hit a hologram.")]
    public GameObject CursorOffHolograms;

    [Tooltip("Distance, in meters, to offset the cursor from the collision point.")]
    public float DistanceFromCollision = 0.01f;

    private void Awake () {
        // Hide the Cursors to begin with.
        if (CursorOnHolograms != null)
        {
            CursorOnHolograms.SetActive(false);
        }
        if (CursorOffHolograms != null)
        {
            CursorOffHolograms.SetActive(false);
        }

        // Make sure there is a GazeManager in the scene
        if (GazeManager.Instance == null)
        {
            Debug.LogWarning("CursorManager requires a GazeManager in your scene.");
            enabled = false;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	private void Update () {
        // Enable/Disable the cursor based whether gaze hit a hologram
        if (CursorOnHolograms != null)
        {
            CursorOnHolograms.SetActive(GazeManager.Instance.Hit);
        }
        if (CursorOffHolograms != null)
        {
            CursorOffHolograms.SetActive(!GazeManager.Instance.Hit);
        }

        // Place the cursor at the calculated position.
        gameObject.transform.position = GazeManager.Instance.Position
                                      + GazeManager.Instance.Normal * DistanceFromCollision;
        
        // Orient the cursor to match the surface being gazed at.
        gameObject.transform.up = GazeManager.Instance.Normal;

        // The "gameObject" here means the object who is plugged this component script, I think.
        // That is, these two lines determine where the object should be moved to according to the gaze raycast position, 
        // and the orientation of the object according to the gaze raycast normal.
    }

    private void LateUpdate()
    {
        if (GazeManager.Instance.Hit)
        {
            RaycastHit hitInfo = GazeManager.Instance.HitInfo;

            if (hitInfo.collider != null)
            {
                FocusedGameObject = hitInfo.collider.gameObject;
            }
            else
            {
                FocusedGameObject = null;
            }
        }
        else
        {
            FocusedGameObject = null;
        }

        if (FocusedGameObject !=null && FocusedGameObject.tag == "MedicalDataObj")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
