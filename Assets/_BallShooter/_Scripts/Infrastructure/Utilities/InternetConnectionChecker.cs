using UnityEngine.Networking;
using System.Collections;

namespace Utilities
{
    public static class InternetConnectionChecker
    {
        public static IEnumerator IsConnected(System.Action<bool> result)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.google.com"))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.error != null)
                {
                    result(false);
                }
                else
                {
                    result(true);
                }
            }
        }
    }

}