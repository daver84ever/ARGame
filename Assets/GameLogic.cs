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
	GameState currentState;

	//game refs//
	public Camera ARcamera;
	public Button startButton;
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
	void Update () {
		if(currentState == GameState.PLAYER_GUESS)
		{
			//check if can guess//

			//check if player guess//

		}

		if(currentState == GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA){
			//check if can guess//

			//if so goto Player Guess state//
		}
	}

	public void StateChange(GameState toState)
	{
		Debug.Log ("StateChange("+toState.ToString()+")");
		LeavingGameState (currentState);

		switch(toState)
		{
		case GameState.PRESTART:
			//Player starts/
			globalTargetFoundHandler.SetTrackableMarkers(false);
			startButton.gameObject.SetActive(true);
			break;
		case GameState.CALCULATING_SPELL:
			SpellSecret = CalculateCorrectSpell ();
			if (debugGameLogic) {
				debugLogicText.text = secertSpellStr;
			}

			//after calcualtion move on to next state//
			StateChange (GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA);
			break;
		case GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA:
			//display instructions//
			InfoText.text = "Cast a spell by rolling the die. " +
			"Put all the die in the camera frame.";
			InfoText.enabled = true;

			//turn on ar detection//
			globalTargetFoundHandler.SetTrackableMarkers(true);

			break;
		case GameState.PLAYER_GUESS:
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
	}

	void LeavingGameState(GameState leavingState){
		Debug.Log ("Leaving StateChange("+leavingState.ToString()+")");

		switch(leavingState)
		{
		case GameState.PRESTART:
			startButton.gameObject.SetActive(false);
			break;
		case GameState.CALCULATING_SPELL:
			break;
		case GameState.INSTRUCT_PLAYER_ROLL_AND_POSITION_CAMERA:
			//hide display instructions//
			InfoText.text = "";
			InfoText.enabled = true;
			break;
		case GameState.PLAYER_GUESS:
			break;
		case GameState.SHOW_GUESS_RESULTS_WRONG:
			break;
		case GameState.SHOW_WINNING_GUESS:
			InfoText.text = "";
			InfoText.enabled = false;
			break;
		case GameState.TURN_ENDS:
			InfoText.text = "";
			InfoText.enabled = false;
			break;
		default:
			Debug.LogWarning ("Leaving UNKOWN STATE "+leavingState.ToString());
			break;
		}
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
		StateChange (GameState.CALCULATING_SPELL);
	}
}
