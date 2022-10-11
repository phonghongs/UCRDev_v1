using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPointManager : MonoBehaviour{
    int perSeconds = 0;     //0-99
    int Seconds = 0;        //0-59
    int Munites = 0;        //0-59
    int Hour = 0;           //0-12
    /*
    int StartTime_S = 0;
    int StartTime_perS = 0;
    */
    int cpflag1 = -1;       //test
    int checkpointFlag1;
    // int cpflag2 = -1;       //test
    int checkpointFlag2;

    public CheckpointManager CPManager;
    // bool timeStartActive = true;

    string perSeconds_text = "00";
    string Seconds_text = "00";
    string Munites_text = "00";
    string Hour_text = "00";

    public Text CheckpointPass;
    public Text Time_showfull;      //"00:00:00.00"
    public Text CoinEarned;

    // public Text Coin;
    // public Text FPS;
    // public Text TimeScaleShow;

    // [SerializeField]
    // public GameObject Car_1_TimeShow;       //group timing for car 1 _ its children are text
    // Text[] Car1_TimeCheckpoint;
    // [SerializeField]
    // public GameObject Car_2_TimeShow;       //group timing for car 2 _ its children are text
    // Text[] Car2_TimeCheckpoint;

    public Text[] Car_TimeCheckpointTimeLab;
    public Image[] Car_TimeCheckpointTimeLabBackground;

    // public Button playButton;
    // public Button scale_05;
    // public Button scale_1;
    // public Button scale_15;
    // public Button scale_2;
    // public Button scale_4;
    // public Button exportData;

    //  Time.time           # 1.345423
    //  Time.deltaTime      # 0.345423   0 - 0.99
    //  Time.fixedTime      # 1.34
    //  Time.frameCount     # 1 2 3 4
    //  FPS  = Time.frameCount / Time.time
    /*
    public void updateTimeStart(){
        StartTime_S = (int)Time.time;
        StartTime_perS = (int)((Time.time - (int)Time.time) * 100);
    }
    */
    public float updateTimeText(){
        float _timer_ = Time.time;
        int S = (int)_timer_;
        int perS = (int)((_timer_ - (int)_timer_) * 100);

        perSeconds = perS;
        Seconds = S % 60;
        Munites = (int)(S/60)%60;
        Hour    = (int)(S/3600)%12;

        if(perSeconds < 10){
            perSeconds_text = "0" + perSeconds.ToString();
        }else{
            perSeconds_text = perSeconds.ToString();
        }
        if(Seconds < 10){
            Seconds_text = "0" + Seconds.ToString();
        }else{
            Seconds_text = Seconds.ToString();
        }
        if(Munites < 10){
            Munites_text = "0" + Munites.ToString();
        }else{
            Munites_text = Munites.ToString();
        }
        if(Hour < 10){
            Hour_text = "0" + Hour.ToString();
        }else{
            Hour_text = Hour.ToString();
        }
        return _timer_;
    }
    public string Time2String(float T){
        int S = (int)T;
        int perS = (int)((T - (int)T) * 100);
        string A, B, C, D;

        int Sec = S % 60;
        int Mun = (int)(S/60)%60;
        int Hou    = (int)(S/3600)%12;

        if(perS < 10){
            D = "0" + perS.ToString();
        }else{
            D = perS.ToString();
        }
        if(Sec < 10){
            C = "0" + Sec.ToString();
        }else{
            C = Sec.ToString();
        }
        if(Mun < 10){
            B = "0" + Mun.ToString();
        }else{
            B = Mun.ToString();
        }
        if(Hou < 10){
            A = "0" + Hou.ToString();
        }else{
            A = Hou.ToString();
        }
        return A+":"+B+":"+C+"."+D;
    }
    bool buttonCheckPause = true;
    bool buttonPlayStatus = true;       //true -> play      flase -> pause
    public void ChangeScale2Pause(GameObject myButton){
        float valueButton = 0;
        float.TryParse(myButton.name, out valueButton);
        if(!buttonCheckPause){
            Time.timeScale = 0f;
            myButton.GetComponentInChildren<TMP_Text>().text = "=";
        }else{
            Time.timeScale = (float)valueButton;
            myButton.GetComponentInChildren<TMP_Text>().text = "x"+valueButton.ToString();
        }
        buttonCheckPause = !buttonCheckPause;
    }
    public void ChangePlay2Pause(){
        if(buttonPlayStatus){
            GameObject.Find("Play").GetComponentInChildren<TMP_Text>().text = "Play";
            Time.timeScale = 1f;

        }else{
            GameObject.Find("Play").GetComponentInChildren<TMP_Text>().text = "=";
            Time.timeScale = 0f;
        }
        buttonPlayStatus = !buttonPlayStatus;
    }
    void Start(){
    }
    float _Timer_ = 0f;
    float _TimerStart_ = 0f;
    void Update(){
        _Timer_ = updateTimeText();
        Time_showfull.text = Hour_text + ":" + Munites_text + ":" + Seconds_text + "." + perSeconds_text;

        // FPS.text = (1/Time.deltaTime).ToString("F0");
        /*test checkpoint with keycode*/
        // if(Input.GetKeyDown(KeyCode.K)){
        //     Car1_TimeCheckpoint[cpflag1].text = Time_showfull.text;
        //     cpflag1 += 1;
        // }
        // if(Input.GetKeyDown(KeyCode.L)){
        //     Car2_TimeCheckpoint[cpflag2].text = Time_showfull.text;
        //     cpflag2 += 1;
        // }

        // if(Time.timeScale == 1.5f){
        //     TimeScaleShow.text = "1.5";
        // }else{
        //     if(Time.timeScale == 0.5f){
        //         TimeScaleShow.text = "0.5";
        //     }else{
        //         TimeScaleShow.text = ((float)Time.timeScale).ToString("F0");
        //     }
        // }

        CoinEarned.text = $"{CoinsManager.Instance.coinCounting}/{CoinsManager.Instance.numCoins}";

        foreach(GameObject tcar in GameObject.FindGameObjectsWithTag("topCar")){
            if(cpflag1 != tcar.GetComponent<CheckpointPass>().numCheckpointPassed){
                // Debug.Log(Time_showfull.text);
                if(tcar.GetComponent<CheckpointPass>().playerIndex == 1){
                    if(tcar.GetComponent<CheckpointPass>().numCheckpointPassed == 0){
                        _TimerStart_ = _Timer_;
                        Debug.Log(_TimerStart_.ToString());
                    }
                    Car_TimeCheckpointTimeLab[tcar.GetComponent<CheckpointPass>().numCheckpointPassed].text =  Time2String(_Timer_ - _TimerStart_) + $" / {CoinsManager.Instance.coinCounting}/{CoinsManager.Instance.numCoins}";
                    cpflag1 += 1;
                }
            }
        }
        if(cpflag1 >= 0){
            CheckpointPass.text = cpflag1.ToString() +"/10";
        }
        if(cpflag1 >= 1){
            Car_TimeCheckpointTimeLab[cpflag1].gameObject.SetActive(true);
            Car_TimeCheckpointTimeLabBackground[cpflag1].gameObject.SetActive(true);
        }
    }
}
