/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using System.Collections;
using System;

public enum DiceImageType { FROG, GHOST, CRAB, CAT, EVILPUMPKIN, BUNNY, UNKOWN }

namespace Vuforia
{


    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {

		[SerializeField] LayerMask InteractionLayer;
		int InteractionLayerIndex;

		public Action<DiceImageType> TargetFoundCallback;
		public Action<DiceImageType> TargetLostCallback;

		[SerializeField] DiceImageType CurrentDiceImage;

        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    	
        void Start()
        {
			InteractionLayerIndex = LayerMask.NameToLayer (InteractionLayer.ToString());

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

			if(TargetFoundCallback !=null){
				TargetFoundCallback (CurrentDiceImage);
			}
        }


        private void OnTrackingLost()
        {
			
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
				//if (component.gameObject.layer == InteractionLayerIndex) {
					Debug.Log ("Shut off "+component.gameObject.name);
					component.enabled = false;
				//}
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
				//if (component.gameObject.layer == InteractionLayerIndex) {
					component.enabled = false;
				//}
            }


            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

			if (TargetLostCallback != null) {
				TargetLostCallback (CurrentDiceImage);
			}

        }

        #endregion // PRIVATE_METHODS
    }
}
