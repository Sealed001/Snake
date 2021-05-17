using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUpdater : MonoBehaviour
{
    [Range(1, 4)] public int playerIndex;

    private Game _game;
    private TextMeshProUGUI _playerScoreText;
    private Image _playerScorePanel;

    public void Start()
    {
        _game = GameObject.Find("Game Manager").GetComponent<Game>();
        _playerScoreText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _playerScorePanel = gameObject.GetComponent<Image>();
    }

    public void Update()
    {
        if (_game.players.Count >= playerIndex)
        {
            _playerScorePanel.color = _game.players[playerIndex - 1].GetComponent<Player>().playerColor;
            _playerScoreText.color = new Color(0f, 0f, 0f, 1f);
            _playerScoreText.text = _game.players[playerIndex - 1].GetComponent<Player>().playerScore.ToString();

        }
        else
        {
            _playerScorePanel.color = new Color(0f, 0f, 0f, 0f);
            _playerScoreText.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
