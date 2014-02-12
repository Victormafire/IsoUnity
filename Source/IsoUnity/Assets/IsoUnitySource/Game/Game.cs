﻿using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	Queue<GameEvent> events;
	Queue<Command> commands;
	public GameObject look;

	public static Game main;

	// Use this for initialization
	void Start () {
		main  = this;
		events = new Queue<GameEvent>();
		commands = new Queue<Command>();
		CameraManager.initialize();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.anyKeyDown){
			GameEvent ge = new GameEvent();
			ge.Name = "KeyPressed";

			this.enqueueEvent(ge);
		}

		this.tick();
	}

	public void enqueueEvent(GameEvent ge){
		this.events.Enqueue(ge);
	}

	public void enqueueCommand(Command c){
		this.commands.Enqueue(c);
	}

	public void tick(){

		CameraManager.lookTo(look);

		while(events.Count>0)
		{
			GameEvent ge = events.Dequeue();
			broadcastEvent(ge);
			while(commands.Count>0)
				commands.Dequeue().run();
		}

		foreach(Map map in MapManager.getInstance().getMapList())
		{
			map.tick();
		}
	}

	private void broadcastEvent(GameEvent ge){
		foreach(Map map in MapManager.getInstance().getMapList())
		{
			map.broadcastEvent(ge);
		}
	}
}
