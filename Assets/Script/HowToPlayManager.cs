using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class HowToPlayPage
{
    public string title;
    [TextArea(4, 10)] public string description;
    public Sprite image;
}

public class HowToPlayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image pageImage;
    [SerializeField] private TMP_Text pageNumberText;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    // teks default sudah terisi, tinggal pasang gambar per halaman di Inspector
    [SerializeField]
    private HowToPlayPage[] pages =
    {
        new HowToPlayPage
        {
            title = "Welcome to Ghost In The Street",
            description = "The street is haunted and the ghosts are coming for you.\n\n" +
                          "Your goal is simple: stay alive for as long as you can. " +
                          "The longer you survive, the more enemies appear."
        },
        new HowToPlayPage
        {
            title = "Movement",
            description = "Use W A S D or the Arrow Keys to move.\n\n" +
                          "Keep moving! Standing still lets enemies surround you."
        },
        new HowToPlayPage
        {
            title = "Aiming",
            description = "Move the mouse to aim. Your shooting weapon fires toward the crosshair.\n\n" +
                          "You do not need to click. All weapons attack automatically."
        },
        new HowToPlayPage
        {
            title = "Weapons",
            description = "You start with one random weapon.\n\n" +
                          "- Shooting weapon: fires projectiles toward the mouse.\n" +
                          "- Spinning weapon: orbits around you and hits nearby enemies.\n" +
                          "- Area weapon: damages every enemy inside its area."
        },
        new HowToPlayPage
        {
            title = "Enemies",
            description = "Touching an enemy hurts you.\n\n" +
                          "- Type 1: weak, but comes in large groups.\n" +
                          "- Type 2: slow and tough.\n" +
                          "- Type 3: fast and dangerous.\n" +
                          "- Boss: huge health. Keep your distance!"
        },
        new HowToPlayPage
        {
            title = "Experience and Level Up",
            description = "Defeat enemies to gain experience (EXP).\n\n" +
                          "When the EXP bar is full you level up and the game pauses. " +
                          "Choose one upgrade: a new weapon or a stronger version of a weapon you already have."
        },
        new HowToPlayPage
        {
            title = "Health",
            description = "The health bar shows how much damage you can still take.\n\n" +
                          "After getting hit you are safe for a moment. " +
                          "When your health reaches zero, the game is over."
        },
        new HowToPlayPage
        {
            title = "Pause",
            description = "Press Esc or P to pause the game.\n\n" +
                          "From the pause menu you can resume, restart, go back to the main menu, or quit."
        },
        new HowToPlayPage
        {
            title = "Score",
            description = "Every run is saved: your survival time and how many enemies of each type you defeated.\n\n" +
                          "Open Score from the main menu to see your best time and your history."
        },
        new HowToPlayPage
        {
            title = "Tips",
            description = "- Always keep moving and never get trapped in a corner.\n" +
                          "- Pick up new weapons early, then upgrade your favorites.\n" +
                          "- When a boss appears, aim at it while running around it.\n\n" +
                          "Good luck, and survive the night!"
        }
    };

    private int currentPage;

    void Start()
    {
        currentPage = 0;
        ShowPage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextPage();

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PreviousPage();

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
            //AudioController.Instance.PlaySound(AudioController.Instance.pause);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
            //AudioController.Instance.PlaySound(AudioController.Instance.unpause);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void ShowPage()
    {
        if (pages.Length == 0) return;

        HowToPlayPage page = pages[currentPage];
        if (titleText != null) titleText.text = page.title;
        if (descriptionText != null) descriptionText.text = page.description;

        if (pageImage != null)
        {
            pageImage.sprite = page.image;
            pageImage.gameObject.SetActive(page.image != null);
            pageImage.preserveAspect = true;
        }

        if (pageNumberText != null) pageNumberText.text = (currentPage + 1) + " / " + pages.Length;

        // tombol Previous mati di halaman pertama, Next mati di halaman terakhir
        if (previousButton != null) previousButton.interactable = currentPage > 0;
        if (nextButton != null) nextButton.interactable = currentPage < pages.Length - 1;
    }
}
