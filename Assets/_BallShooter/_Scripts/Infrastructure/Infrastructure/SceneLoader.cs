using System;
using System.Collections;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name,LoadSceneMode mode = LoadSceneMode.Single, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadScene(name, mode, onLoaded));

        public IEnumerator LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name, mode);

            while (!waitNextScene.isDone)
                yield return null;
           
            onLoaded?.Invoke();
        }
    }
}