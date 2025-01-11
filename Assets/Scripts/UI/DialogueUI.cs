using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textlabel;

    private void Start()
    {
        GetComponent<TypeWriterEffect>().Run("This is dio\n Hello", textlabel);
    }
}
