using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarBGscript : MonoBehaviour
{

    [SerializeField] int baseSceneBuildIndex = 1;
    // Start is called before the first frame update
    public void SceneChange()
    {
        SceneManager.LoadScene(baseSceneBuildIndex);
    }
}
