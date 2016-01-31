using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

	public enum DiceImageType { 
		FROG = 0, 
		GHOST = 1, 
		CRAB = 2, 
		CAT = 3, 
		EVILPUMPKIN = 4, 
		BUNNY = 5 
	}

	List<DiceImageType> SpellSecret;
	GameState currentState;


	//game refs//
	public Camera ARcamera;
	public Button startButton;

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

			//turn on ar detection//

			break;
		case GameState.PLAYER_GUESS:
			break;
		case GameState.SHOW_GUESS_RESULTS_WRONG:
			break;
		case GameState.SHOW_WINNING_GUESS:
			break;
		case GameState.TURN_ENDS:
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

			break;
		case GameState.PLAYER_GUESS:
			break;
		case GameState.SHOW_GUESS_RESULTS_WRONG:
			break;
		case GameState.SHOW_WINNING_GUESS:
			break;
		case GameState.TURN_ENDS:
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


	public void OnStartPress(){
		StateChange (GameState.CALCULATING_SPELL);
	}
}
