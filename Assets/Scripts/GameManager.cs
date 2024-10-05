using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnInvulnerabilityTime = 3.0f;
    public int lives = 3;
    public int score = 0;
    public Text Score;
    public Text Live;
    public Text GameOverText;
    

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.75) {
            this.score += 100; 
        } else if (asteroid.size < 1.2f) {
            this.score += 50;
        } else {
            this.score += 25;
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        Score.text = "Score: " + score;
    }  
    
    private void LiveUpdateUI()
    {
        Live.text = "Lives: " + lives;
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;
        LiveUpdateUI();

        if(this.lives <= 0){
            GameOver();
        } else {
            Invoke(nameof(Respawn), this.respawnTime);
        } 
        
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collision");
        this.player.gameObject.SetActive(true);
        GameOverText.text = "";

        Invoke(nameof(TurnOnCollisions),respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Palyer");   
    }

    private void GameOver()
    {
        this.lives = 3;
        this.score = 0;
        LiveUpdateUI();

        UpdateScoreUI();
        GameOverText.text = "Game Over!";

        Invoke(nameof(Respawn), this.respawnTime);
    }

}
