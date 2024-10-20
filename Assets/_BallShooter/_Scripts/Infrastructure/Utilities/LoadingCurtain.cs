using UnityEngine;

namespace Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup curtain;

        public void Show()
        {
            if(gameObject.activeSelf == false)
                gameObject.SetActive(true);
            Debug.Log("Show");
            curtain.alpha = 1;
        }

        public void Hide()
        { 
            Debug.Log("Hide");
            curtain.alpha = 0;
        }

        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}
