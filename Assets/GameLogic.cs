using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public enum DiceImageType : int { 
	FEATHER = 0,
	CHICKEN = 1,
	BONE = 2,
	DYNAMITE = 3,
	DIAMOND = 4,
	APPLE = 5
}

public class GameLogic : MonoBehaviour {

	public enum GameState { 
		PRESTART, 
		CALCULATING_SPELL, 
		INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA, 
		PLAYER_GUESS,
		SHOW_GUESS_RESULTS_WRONG,
		SHOW_WINNING_GUESS,
		TURN_ENDS,
	}

	List<DiceImageType> SpellSecret;
	[SerializeField] GameState currentState;

	//game refs//
	public Camera ARcamera;
	public Button startButton;
	public Button castButton;
	public GlobalTargetFoundHandler globalTargetFoundHandler;
	public CauldronLevels cauldronLevels;

	//ui refs//
	public Text InfoText;

	//debug//
	[SerializeField] Text debugLogicText;
	[SerializeField] bool debugGameLogic;
		
	// Use this for initialization
	void Start () {
		StateChange (GameState.PRESTART);
	}

	// Update is called once per frame
	[SerializeField] float delayBeforeMovingToGuessState = 1f;
	[SerializeField] float delayBeforeMovingLeavingGuessState = 3.5f;

	List<DiceId> cachedTrackedDice;
	bool useCachedRead = false;
	float trackingTimer = 0f;
	void Update () {

		if(currentState == GameState.CALCULATING_SPELL && ( SpellSecret != null && SpellSecret.Count==4)){
			//after calcualtion move on to next state//
			StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
		}

		//give the player some breathing room before we drop the dice cast button//
		if(currentState == GameState.PLAYER_GUESS)
		{
			/*
			//check if can guess otherwise kick player back to the Instruct state//
			if (globalTargetFoundHandler.NumberOfTrackedDie () < 4) {
				useCachedRead = true;
				if (cachedTrackedDice == null) {
					cachedTrackedDice = globalTargetFoundHandler.GetTrackedDie ();
					string str = "";
					foreach(DiceId dice in cachedTrackedDice){
						str+=dice.type+",";
					}
					Debug.LogWarning ("cachedTrackedDice:"+str);
				}

				//record how long the the dice have be out of focus//
				trackingTimer += Time.deltaTime;
				if (trackingTimer > delayBeforeMovingLeavingGuessState) {
					StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
				}

			} else {
				if (cachedTrackedDice != null) {
					cachedTrackedDice = null;
				}
				trackingTimer = 0f;
				useCachedRead = false;

			}
			*/
			if (globalTargetFoundHandler.NumberOfTrackedDie () < 4) {
				StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
			}
		}

		if(currentState == GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA){
			//check if can guess//
			//if so goto Player Guess state//
			if (globalTargetFoundHandler.NumberOfTrackedDie () == 4) {
				Debug.Log ("Tracking 4 DiceId in Instruct phase");
				StateChange (GameState.PLAYER_GUESS);

				/*
				trackingTimer += Time.deltaTime;
				if (trackingTimer > delayBeforeMovingToGuessState) {
					StateChange (GameState.PLAYER_GUESS);
				}
				*/
			} 
		}


	}

	public void StateChange(GameState toState)
	{
		Debug.LogWarning ("StateChange("+toState.ToString()+")");

		switch(toState)
		{
		case GameState.PRESTART:
			//Player starts/
			globalTargetFoundHandler.SetTrackableMarkers (false);

			castButton.gameObject.SetActive (false);
			castButton.enabled = false;

			startButton.gameObject.SetActive (true);
			startButton.enabled = true;

			break;
		case GameState.CALCULATING_SPELL:
			startButton.gameObject.SetActive (false);
			startButton.enabled = false;

			castButton.gameObject.SetActive (false);
			castButton.enabled = false;

			SpellSecret = CalculateCorrectSpell ();
			if (debugGameLogic) {
				debugLogicText.text = secertSpellStr;
			}
				
			break;
		case GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA:
			castButton.gameObject.SetActive (false);
			castButton.enabled = false;

			//reset tracking timer
			trackingTimer = 0f;

			//display instructions//
			InfoText.enabled = true;
			InfoText.text = "Cast a spell by rolling the die. " +
			"Put all the die in the camera frame.";

			//turn on ar detection//
			globalTargetFoundHandler.SetTrackableMarkers(true);

			break;
		case GameState.PLAYER_GUESS:
			InfoText.text = "";

			//reset tracking timer
			trackingTimer = 0f;
			cachedTrackedDice = null;
			castButton.gameObject.SetActive (true);
			castButton.enabled = true;
			break;
		case GameState.SHOW_GUESS_RESULTS_WRONG:
			castButton.gameObject.SetActive (false);
			castButton.enabled = false;

			StartCoroutine (NextTurnTimer(4f));
			break;
		case GameState.SHOW_WINNING_GUESS:
			castButton.gameObject.SetActive (false);
			castButton.enabled = false;

			InfoText.text = "Well cast! You Win!";
			InfoText.enabled = true;
			StartCoroutine (NextGameTimer(5f));
			break;
		case GameState.TURN_ENDS:
			castButton.gameObject.SetActive (false);
			castButton.enabled = false;
			globalTargetFoundHandler.SetTrackableMarkers (false);

			//display for a set amount of time
			InfoText.text = "Pass the phone to the next player.";
			InfoText.enabled = true;
			StartCoroutine (PassPhoneTimer(5f));
			break;
		default:
			Debug.LogWarning ("UNKOWN STATE "+toState.ToString());
			break;
		}

		currentState = toState;
		Debug.LogWarning ("currentState = toState ("+toState.ToString()+")");

	}
		
	//1296 possible//
	[SerializeField] string secertSpellStr ="";
	List<DiceImageType> CalculateCorrectSpell(){
		List<DiceImageType>  correctSpellSecret = new List<DiceImageType> ();
		correctSpellSecret.Add((DiceImageType)Random.Range (0, 6));
		correctSpellSecret.Add((DiceImageType)Random.Range (0, 6));
		correctSpellSecret.Add((DiceImageType)Random.Range (0, 6));
		correctSpellSecret.Add((DiceImageType)Random.Range (0, 6));

		string str = "( ";
		foreach(DiceImageType selectedType in correctSpellSecret)
		{
			str += "|"+selectedType.ToString()+"| ";
		}
		str+=")";
		secertSpellStr = str;
		Debug.Log (str);

		return correctSpellSecret;

	}

	IEnumerator PassPhoneTimer(float waitTime){
		yield return new WaitForSeconds (waitTime);
		StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
	}

	IEnumerator NextTurnTimer(float waitTime){
		StartCoroutine (CauldronLevels(waitTime));
		yield return new WaitForSeconds (waitTime);
		StateChange (GameState.TURN_ENDS);
	}

	IEnumerator CauldronLevels(float waitTime){
		yield return new WaitForSeconds (waitTime*.5f);
		cauldronLevels.SetLevel (0);
	}

	IEnumerator NextGameTimer(float waitTime){
		yield return new WaitForSeconds (waitTime);
		cauldronLevels.SetLevel (0);
		StateChange (GameState.PRESTART);
	}

	public void OnStartPress(){
		if(currentState != GameState.PRESTART){
			return;
		}
		Debug.Log ("OnStartPress");
		StateChange (GameState.CALCULATING_SPELL);
	}

	public void OnCastPress(){
		if(currentState != GameState.PLAYER_GUESS){
			return;
		}
		if (useCachedRead) {
			StartCoroutine (CastRoutine (cachedTrackedDice));
		} else {
			StartCoroutine(CastRoutine(globalTargetFoundHandler.GetTrackedDie ()));
		}
		Debug.Log ("OnCastPress ");
	}

	int Correctness = 0;
	IEnumerator CastRoutine(List<DiceId> castGuess){
		castButton.gameObject.SetActive (false);
		castButton.enabled = false;

		foreach(DiceId dice in castGuess){
			DefaultTrackableEventHandler marker = globalTargetFoundHandler.FindMarkerObject (dice.markerId);
			marker.GetComponentInChildren<ModelController> ().FadeIn ();
		}

		InfoText.enabled = true;
		InfoText.text = "Incanting.";
		yield return new WaitForSeconds (.5f);
		InfoText.text = "Incanting..";
		yield return new WaitForSeconds (.5f);
		InfoText.text = "Incanting...";
		yield return new WaitForSeconds (.5f);
		InfoText.text = "Incanting....";
		yield return new WaitForSeconds (.5f);
		InfoText.text = "Incanting.....";
		yield return new WaitForSeconds (.5f);
		InfoText.text = "Incanting......";

		foreach(DiceId dice in castGuess){
			DefaultTrackableEventHandler marker = globalTargetFoundHandler.FindMarkerObject (dice.markerId);
			marker.GetComponentInChildren<ModelController> ().FadeOut ();
		}

		int correctness;
		if(CorrectCastCheck (castGuess, out correctness)){
			StateChange (GameState.SHOW_WINNING_GUESS);
		} else {
			
			StateChange (GameState.SHOW_GUESS_RESULTS_WRONG);
			Correctness = correctness;
			InfoText.text = "You Got "+correctness+" out of 4 right.";

		}
		cauldronLevels.SetLevel (correctness);
	}

	bool CorrectCastCheck(List<DiceId> castGuess, out int correctness){
		correctness = 0;
		List<DiceImageType>  tempSecret = new List<DiceImageType> (SpellSecret);
		for(int i=0; i<4; i++){

			for(int k=0; k<tempSecret.Count; k++){
				if (castGuess [i].type == tempSecret [k]) {
					correctness++;
					tempSecret.Remove (tempSecret [k]);
					break;
				}
			}
		}

		if (correctness == 4) {
			return true;
		}
		return false;
	}

		/*
		return GlobalTargetFoundHandler.CompareCasts (
			GlobalTargetFoundHandler.GetDiceTypeCounts(castGuess),
			GlobalTargetFoundHandler.GetDiceTypeCounts(SpellSecret)
		)
		*/
}
