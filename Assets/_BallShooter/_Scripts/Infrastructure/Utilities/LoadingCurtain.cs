using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup curtain;
        public Image progressFilledImage;

        public float progressFillingDuration = 1f;

        public void Show(bool isAutoHide = true)
        {
            if(gameObject.activeSelf == false)
                gameObject.SetActive(true);
            Debug.Log("Show");
            curtain.alpha = 1;
            MoveProgress(isAutoHide);
        }

        public void Hide() =>
            FadeIn();

        private void Awake() => 
            DontDestroyOnLoad(this);

        private void MoveProgress(bool isAutoHide)
        {
            if (isAutoHide)
                progressFilledImage.DOFillAmount(1f, progressFillingDuration).OnComplete(FadeIn);
            else
                progressFilledImage.DOFillAmount(1f, progressFillingDuration);
        }

        private void FadeIn()
        {
            Debug.Log("Hide");
            curtain.alpha = 0;
            progressFilledImage.fillAmount = 0f;
        }
    }
}
