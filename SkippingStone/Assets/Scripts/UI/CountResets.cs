using UnityEngine;

public class CountResets : MonoBehaviour {

    int countResets = 0;
    public int NumResets { get { return countResets; } set { countResets ++; } }

    static CountResets instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static CountResets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("CountResets").GetComponent<CountResets>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
}
