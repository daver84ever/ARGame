using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum DiceImageType : int { 
	FROG = 0, 
	GHOST = 1, 
	CRAB = 2, 
	CAT = 3, 
	BUG = 4, 
	BUNNY = 5 
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
	float trackingTimer = 0f;
	void Update () {

		if(currentState == GameState.CALCULATING_SPELL && ( SpellSecret != null && SpellSecret.Count==4)){
			//after calcualtion move on to next state//
			StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
		}

		//give the player some breathing room before we drop the dice cast button//
		if(currentState == GameState.PLAYER_GUESS)
		{
			//check if can guess otherwise kick player back to the Instruct state//
			if (globalTargetFoundHandler.NumberOfTrackedDie () < 4) {
				if (cachedTrackedDice == null) {
					cachedTrackedDice = globalTargetFoundHandler.GetTrackedDie ();
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
			}
		}

		if(currentState == GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA){
			//check if can guess//
			//if so goto Player Guess state//
			if (globalTargetFoundHandler.NumberOfTrackedDie () == 4) {
				Debug.Log ("Tracking 4 DiceId in Instruct phase");
				trackingTimer += Time.deltaTime;
				if (trackingTimer > delayBeforeMovingToGuessState) {
					StateChange (GameState.PLAYER_GUESS);
				}
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

			SpellSecret = CalculateCorrectSpell ();
			if (debugGameLogic) {
				debugLogicText.text = secertSpellStr;
			}
				
			break;
		case GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA:

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

			//reset tracking timer
			trackingTimer = 0f;
			cachedTrackedDice = null;
			InfoText.text = "";
			InfoText.enabled = false;

			castButton.gameObject.SetActive (true);
			castButton.enabled = true;
			break;
		case GameState.SHOW_GUESS_RESULTS_WRONG:
			break;
		case GameState.SHOW_WINNING_GUESS:
			InfoText.text = "Well cast! You Win!";
			InfoText.enabled = true;
			break;
		case GameState.TURN_ENDS:
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
		Debug.Log ("OnCastPress "+cachedTrackedDice.Count);
		StartCoroutine(CastRoutine(cachedTrackedDice));
	}

	IEnumerator CastRoutine(List<DiceId> castGuess){
		InfoText.enabled = false;
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

		if(CorrectCastCheck (castGuess)){
			StateChange (GameState.SHOW_WINNING_GUESS);
		} else {
			StateChange (GameState.SHOW_GUESS_RESULTS_WRONG);

		}
	}

	bool CorrectCastCheck(List<DiceId> castGuess){
		return GlobalTargetFoundHandler.CompareCasts (
			GlobalTargetFoundHandler.GetDiceTypeCounts(castGuess),
			GlobalTargetFoundHandler.GetDiceTypeCounts(SpellSecret)
		);
	}
}
