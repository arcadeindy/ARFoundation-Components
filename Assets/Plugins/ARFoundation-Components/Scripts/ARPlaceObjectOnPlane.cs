using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

namespace CandyCoded.ARFoundationComponents
{

    [System.Serializable]
    public class GameObjectPlacedEvent : UnityEvent<GameObject>
    {
    }

    [RequireComponent(typeof(ARSessionOrigin))]
    public class ARPlaceObjectOnPlane : MonoBehaviour
    {

#pragma warning disable CS0649
        public GameObject objectToPlace;
        public bool placeMultiple;
        public float verticalOffset = 0.01f;
#pragma warning restore CS0649

        public GameObjectPlacedEvent GameObjectPlaced;

        private bool objectToPlaceActiveState;

        public Camera mainCamera { get; private set; }

        private void Awake()
        {

            mainCamera = gameObject.GetComponent<ARSessionOrigin>().camera;

        }

        public void PlaceObjectOnPlane(Pose pose, ARPlane plane)
        {

            var objectToPlaceGameObject = objectToPlace;

            if (placeMultiple)
            {

                objectToPlaceGameObject = Instantiate(objectToPlace);

            }

            objectToPlaceGameObject.SetActive(true);

            objectToPlaceGameObject.transform.position = pose.position + new Vector3(0, verticalOffset, 0);
            objectToPlaceGameObject.transform.rotation = pose.rotation;

            if (plane.boundedPlane.Alignment == PlaneAlignment.Horizontal)
            {

                objectToPlaceGameObject.transform.LookAt(new Vector3(
                    mainCamera.transform.position.x,
                    objectToPlaceGameObject.transform.position.y,
                    mainCamera.transform.position.z
                ));

            }

            GameObjectPlaced?.Invoke(objectToPlaceGameObject);

        }

        private void OnEnable()
        {

            if (objectToPlace && objectToPlace.scene.IsValid())
            {

                objectToPlaceActiveState = objectToPlace.activeSelf;

                objectToPlace.SetActive(false);

            }

        }

        private void OnDisable()
        {

            if (objectToPlace && objectToPlace.scene.IsValid())
            {

                objectToPlace.SetActive(objectToPlaceActiveState);

            }

        }

    }

}
