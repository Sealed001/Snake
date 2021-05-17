using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
	public Image player1;
	public Image player2;
	public Image player3;
	public Image player4;
	public TextMeshProUGUI joinMessage;

	private Game _game;
	private Image _pausePanel;

	private void Start()
	{
		_game = GameObject.Find("Game Manager").GetComponent<Game>();
		_pausePanel = gameObject.GetComponent<Image>();
	}

	void Update()
	{
		if (!_game._isPartyLaunched)
        {
			_pausePanel.color = new Color(.372f, .831f, .866f, 1f);
			joinMessage.color = new Color(0f, 0f, 0f, 1f);

			if (_game.players.Count == 0)
            {
				joinMessage.text = "Press any key or button to join the game";
			}
			else if (_game.players.Count == 1)
            {
				joinMessage.text = "1 more player expected to start a party";
			}
			else if (_game.players.Count == 2)
            {
				joinMessage.text = "2 more players can now join the party";
			}
			else if (_game.players.Count == 3)
            {
				joinMessage.text = "1 more players can now join the party";
			}
			else
            {
				joinMessage.text = "";
			}

			if (_game.players.Count >= 1)
			{
				player1.color = _game.players[0].GetComponent<Player>().playerColor;
			}
			else
			{
				player1.color = new Color(0f, 0f, 0f, 0f);
			}
			if (_game.players.Count >= 2)
			{
				player2.color = _game.players[1].GetComponent<Player>().playerColor;
			}
			else
			{
				player2.color = new Color(0f, 0f, 0f, 0f);
			}
			if (_game.players.Count >= 3)
			{
				player3.color = _game.players[2].GetComponent<Player>().playerColor;
			}
			else
			{
				player3.color = new Color(0f, 0f, 0f, 0f);
			}
			if (_game.players.Count == 4)
			{
				player4.color = _game.players[3].GetComponent<Player>().playerColor;
			}
			else
			{
				player4.color = new Color(0f, 0f, 0f, 0f);
			}
		}
		else
        {
			_pausePanel.color = new Color(1f, 1f, 1f, 0f);
			joinMessage.color = new Color(1f, 1f, 1f, 0f);
			player1.color = new Color(1f, 1f, 1f, 0f);
			player2.color = new Color(1f, 1f, 1f, 0f);
			player3.color = new Color(1f, 1f, 1f, 0f);
			player4.color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
