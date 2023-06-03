using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMainPanel : MonoBehaviour
{
    #region Variables
    #endregion

    #region UnityMethods
    #endregion

    #region Main Methods
    public void OnClickStartGame(int modeIndex)
    {
        SceneType sceneName = modeIndex switch
        {
            1 => SceneType.GameScene1,
            2 => SceneType.GameScene2,
            3 => SceneType.GameScene3,
            4 => SceneType.GameScene4,
            _ => SceneType.Lobby,
        };

        SceneControllManager.Instance.LoadScene(sceneName, false);
    }
    #endregion
}
