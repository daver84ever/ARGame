/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using System.Collections;
using System;

public struct DiceId {
	public DiceImageType type;
	public int diceIdx;
	public int markerId;
}

namespace Vuforia
{

    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
		[SerializeField] MarkerBehaviour Marker;
		[SerializeField] LayerMask InteractionLayer;
		int InteractionLayerIndex;

		public Action<DiceId> TargetFoundCallback;
		public Action<DiceId> TargetLostCallback;

		public DiceImageType CurrentDiceImage;

		public int MarkerID { get { return Marker.Marker.MarkerID; }}
		public MarkerBehaviour MarkerBehaviour { get { return Marker; }}

        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    	
        void Start()
        {
			CurrentDiceImage = (DiceImageType) (MarkerID % 6);
			InteractionLayerIndex = LayerMask.NameToLayer (InteractionLayer.ToString());

            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

		//void OnEnable(){
		//	Debug.Log ("OnEnabled");
		//}

		//void OnDisable(){
		//	Debug.Log ("OnDisable");
		//}

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


			if(TargetFoundCallback != null){
				DiceId diceId = new DiceId(){
					type = CurrentDiceImage,
					diceIdx = Marker.Marker.MarkerID / 6,
					markerId = Marker.Marker.MarkerID,
				};
				TargetFoundCallback (diceId);
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
				DiceId diceId = new DiceId(){
					type = CurrentDiceImage,
					diceIdx = Marker.Marker.MarkerID / 6,
					markerId = Marker.Marker.MarkerID,
				};
				TargetLostCallback (diceId);
			}

        }

        #endregion // PRIVATE_METHODS
    }
}
