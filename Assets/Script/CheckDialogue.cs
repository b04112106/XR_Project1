﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CheckDialogue : MonoBehaviour
{
    private GameObject GM_obj;
    private GameManager GM;
    private GameObject[] controllers;
    private GameObject target_obj;
    private ObjAttribute target_attr;
    private GameObject script_obj;
    private DisplayDialogue win_fun;
    int i ;
    // Start is called before the first frame update
    void Start()
    {
        init();
        //GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        get_grab_item();

        if(target_attr.trigger_scene == GameState.OTHER){
            Debug.Log("Other scene");
            activate_dialogue();
        }
        else if(GM.get_state() != target_attr.trigger_scene){
            Debug.Log("Wrong scene");
            activate_hint();
        }
        else{
            Debug.Log("Correct scene");
            activate_dialogue();
        }
    }

    private void init(){
        // get current game state (store in GameManager)
        GM_obj = GameObject.Find("GameManager");
        GM = GM_obj.GetComponent<GameManager>();
        script_obj = GameObject.Find("Script");
        win_fun = script_obj.GetComponent<DisplayDialogue>();
    }
    // get the grabbed item on controller
    private void get_grab_item(){
        Debug.Log("Game state : " + GM.get_state());
        controllers = GameObject.FindGameObjectsWithTag("GameController");
        for(i = 0 ; i < controllers.Length ; i++){
            target_obj = controllers[i].GetComponent<VRTK_InteractGrab>().GetGrabbedObject();
            if(target_obj != null){
                break;
            }
        }
        target_attr = target_obj.GetComponent<ObjAttribute>();
        Debug.Log("Obj scene : " + target_attr.trigger_scene);
    }
    // initial parameter to start displaying dialogue
    private void activate_dialogue(){
        GM.set_talking();
        win_fun.dialogue_filename = target_attr.dialogue_filename;
        win_fun.next_state = target_attr.next_scene;
        win_fun.enabled = true;
        this.enabled = false;
    }
    // show hint dialogue when touch the wrong scene item
    private void activate_hint(){
        if((win_fun.dialogue_filename = GM.get_hint_path()) == null){
            this.enabled = false;
            return;
        }
        GM.set_talking();
        win_fun.next_state = GameState.OTHER;
        win_fun.enabled = true;
        this.enabled = false;
    }
}
