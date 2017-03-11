// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR.WSA.Input;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// A component for moving an object via the GestureManager manipulation gesture.
    /// </summary>
    /// <remarks>
    /// When an active GestureManipulator component is attached to a GameObject it will subscribe
    /// to GestureManager's manipulation gestures, and move the GameObject when a ManipulationGesture occurs.
    /// If the GestureManipulator is disabled it will not respond to any manipulation gestures.
    /// 
    /// This means that if multiple GestureManipulators are active in a given scene when a manipulation
    /// gesture is performed, all the relevant GameObjects will be moved.  If the desired behavior is that only
    /// a single object be moved at a time, it is recommended that objects which should not be moved disable
    /// their GestureManipulators, then re-enable them when necessary (e.g. the object is focused).
    /// </remarks>
    public class RotationManipulator : MonoBehaviour
    {
        [Tooltip("How much to scale each axis of movement (camera relative) when manipulating the object")]
        public Vector3 PositionScale = new Vector3(2.0f, 2.0f, 4.0f);  // Default tuning values, expected to be modified per application

        private Vector3 initialManipulationPosition;

        private Vector3 initialObjectPosition;

        private Interpolator targetInterpolator;

        private GestureManager gestureManager;

        private bool Manipulating { get; set; }

        private void Awake()
        {
            gestureManager = GestureManager.Instance;

            if (gestureManager == null)
            {
                Debug.LogError(string.Format("GestureManipulator on {0} could not find GestureManager instance, manipulation will not function", name));
            }
        }

        private void OnEnable()
        {
            gestureManager.OnManipulationStarted += BeginManipulation;
            gestureManager.OnManipulationCompleted += EndManipulation;
            gestureManager.OnManipulationCanceled += EndManipulation;
        }

        private void OnDisable()
        {
            gestureManager.OnManipulationStarted -= BeginManipulation;
            gestureManager.OnManipulationCompleted -= EndManipulation;
            gestureManager.OnManipulationCanceled -= EndManipulation;

            Manipulating = false;
        }

        private void BeginManipulation(InteractionSourceKind sourceKind)
        {
            // Check if the gesture manager is not null, we're currently focused on this Game Object, and a current manipulation is in progress.
            if (gestureManager != null && gestureManager.FocusedObject != null && gestureManager.FocusedObject == gameObject && gestureManager.ManipulationInProgress && this.GetComponent<ItemModeSelect>().isRotation == false)
            {
                Manipulating = true;

                targetInterpolator = gameObject.GetComponent<Interpolator>();

                // In order to ensure that any manipulated objects move with the user, we do all our math relative to the camera,
                // so when we save the initial manipulation position and object position we first transform them into the camera's coordinate space
                initialManipulationPosition = Camera.main.transform.InverseTransformPoint(gestureManager.ManipulationPosition);
                initialObjectPosition = Camera.main.transform.InverseTransformPoint(transform.position);
            }
        }

        private void EndManipulation(InteractionSourceKind sourceKind)
        {
            Manipulating = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Manipulating)
            {
                transform.Rotate(new Vector3(0, -1 * 2, 0));
            }
        }
    }
}
