using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using System.Linq;

namespace Assets.Scripts
{
    internal class PortalPlacementManager : MonoBehaviour
    {
        [SerializeField]
        private InputAction _press = null;
        [SerializeField]
        private ARRaycastManager _raycastManager = null;
        [SerializeField]
        private ARPlaneManager _planeManager = null;
        [SerializeField]
        private GameObject _portalPrefab = null;
        [SerializeField]
        private GameObject _environmentPrefab = null;

        private List<ARRaycastHit> hits = new();
        private GameObject _portal = null;
        private GameObject _environment = null;

        private void Start()
        {
            _press.Enable();

            _press.started += (context) =>
            {
                var position = context.ReadValue<Vector2>();
                if (_raycastManager.Raycast(
                    position,
                    hits,
                    UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    if (hits.Count <= 0) return;
                    ARRaycastHit? validHit = null;
                    validHit = GetValidHit();

                    if (validHit == null) return;
                    PlacePortalAndEnvironment(validHit);
                }
            };
        }

        private ARRaycastHit? GetValidHit()
        {
            foreach (var hit in from hit in hits
                                where _planeManager.GetPlane(hit.trackableId)
                                    .alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp
                                select hit)
            {
                return hit;
            }

            return null;
        }

        private void PlacePortalAndEnvironment(ARRaycastHit? validHit)
        {
            var pose = validHit.Value.pose;
            var direction = Camera.main.transform.position - pose.position;
            direction.y = 0;
            var rotation = Quaternion.LookRotation(direction);

            if (_portal == null)
            {
                _portal = Instantiate(_portalPrefab, pose.position, rotation);
            }
            else
            {
                _portalPrefab.transform.position = pose.position;
                _portalPrefab.transform.rotation = rotation;
                _portalPrefab.SetActive(true);
            }

            if (_environment == null)
            {
                _environment = Instantiate(_environmentPrefab, pose.position, rotation);
            }
            else
            {
                _environmentPrefab.transform.position = pose.position;
                _environmentPrefab.transform.rotation = rotation;
                _environmentPrefab.SetActive(true);
            }
        }
    }
}
