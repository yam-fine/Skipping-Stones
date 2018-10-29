using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{

    CountResets cr;

    private void Start()
    {
        cr = CountResets.Instance;
    }

    public void ShowAd()
    {
        if (cr.NumResets % 2 == 0)
            if (Advertisement.IsReady("video"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("video", options);
            }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}