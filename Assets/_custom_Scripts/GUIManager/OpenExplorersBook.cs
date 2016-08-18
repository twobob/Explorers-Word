﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace ExplorersBook
{
	public class OpenExplorersBook : MonoBehaviour
	{
		private GameObject explBook;
		private GameObject inputField;
		private CharacterController CharacterController;
		private MouseCrosshair MouseCrosshair;
		private UnityStandardAssets.Characters.FirstPerson.CustomFirstPersonController FPSControllerScript;
		private AudioSource PlayerSound;
		private Text StoryText;
		private bool bookIsOpen = false;
		private float lastWalkingSpeed;
		private float lastRunningSpeed;
		private List<string> ExplorersStory = new List<string>();
		private int currentStoryIndex = 0;
		private bool subtract = false;

		void Start () {
			explBook = GameObject.Find ("Explorers Book");
			inputField = GameObject.Find ("ExplorersWord");
			MouseCrosshair = GameObject.Find ("FirstPersonCharacter").GetComponent<MouseCrosshair> ();
			CharacterController = gameObject.GetComponentInParent<CharacterController> ();
			FPSControllerScript = GameObject.Find ("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.CustomFirstPersonController> ();
			PlayerSound = gameObject.GetComponentInParent<AudioSource> ();
			StoryText = GameObject.Find ("Text").GetComponent<Text> ();
			//StoryManager = gameObject.GetComponent<StoryManager> ();
			//RoomDescription = GameObject.Find("RoomDescription");

			if (explBook) {
				explBook.SetActive (false);
			}
			if (inputField) {
				inputField.SetActive (false);
			}
			if (StoryText) {
				StoryText.enabled = false;	
				Debug.Log (StoryText);
			}
		}

		void LateUpdate() {
			if ((Input.GetKeyUp(KeyCode.B) && !bookIsOpen) || (Input.GetKeyUp(KeyCode.Escape) && bookIsOpen) ) {
				openExplorersBook ();
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				subtract = false;
				depictExplorersStory ();
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				subtract = true;
				depictExplorersStory();
			}
		}

		private void openExplorersBook() { 
			bookIsOpen = !bookIsOpen;
			ActivateExplorersBook ();
			DisablePlayerSound();
			DisablePlayerMovement ();
			UnlockMouseMovement ();
			ActivateUserInputField ();
			depictExplorersStory ();
			//ActivateStoryManager ();

		}

		private void ActivateStoryManager() {
			//StoryManager.enabled = bookIsOpen;
			//RoomDescription.SetActive (bookIsOpen);
		}

		private void ActivateExplorersBook() {
			if (explBook && bookIsOpen) {
				explBook.SetActive (bookIsOpen);
				StartCoroutine (playAnimation ());
			} else {
				StartCoroutine(playAnimation ());
			}
		}

		private void ActivateUserInputField() {
			if (inputField) {
				inputField.SetActive(bookIsOpen);
				InputField inputFieldComponent = inputField.GetComponent<InputField> ();
				inputFieldComponent.ActivateInputField ();
			}
		}

		private void DisablePlayerSound() {
			if (PlayerSound) {
				PlayerSound.mute = bookIsOpen;
			}
		}

		private void DisablePlayerMovement() {
			if (CharacterController) {
				CharacterController.enabled = !bookIsOpen;
			}
			if (FPSControllerScript && bookIsOpen) {
				lastRunningSpeed = FPSControllerScript.m_RunSpeed;
				lastWalkingSpeed = FPSControllerScript.m_WalkSpeed;
				changeWalkingAndRunningSpeed (0, 0);
			} else {
				changeWalkingAndRunningSpeed (lastWalkingSpeed, lastRunningSpeed);		
			}
		}

		private void changeWalkingAndRunningSpeed(float walkingSpeed, float runningSpeed) {
			FPSControllerScript.m_RunSpeed = runningSpeed;
			FPSControllerScript.m_WalkSpeed = walkingSpeed;		
		}

		private void UnlockMouseMovement() {
			Cursor.visible = bookIsOpen;
			Cursor.lockState = CursorLockMode.Confined;
			if (MouseCrosshair) {
				MouseCrosshair.enabled = !bookIsOpen;
			}
		}
			
		IEnumerator playAnimation() {
			Animator anim = explBook.GetComponent<Animator> ();
			if (anim) {
				anim.SetBool ("open", bookIsOpen);
				yield return new WaitForSeconds (getAnimationDuration (anim, "open"));
				explBook.SetActive (bookIsOpen); //only matters when book is Open == false. 
			}
		}

		private float getAnimationDuration(Animator a, string n) {
			foreach (AnimationClip clip in a.runtimeAnimatorController.animationClips)
				if (clip.name == n) return clip.length;
			return 0;
		}

		private void depictExplorersStory() {
			LevelManager lvlManager = GameObject.Find("LevelManager").GetComponent<LevelManager> ();
			if (lvlManager) {
				ExplorersStory = lvlManager.getParagraphs();
			}
			if (StoryText) {
				StoryText.enabled = bookIsOpen;
				Debug.Log (StoryText);
				Debug.Log (ExplorersStory.Count);
				StoryText.text = GetCorrectParagraph ();

			}
		}

		private string GetCorrectParagraph() {
			string result = "";
			if (currentStoryIndex > 0 || currentStoryIndex < ExplorersStory.Count) {
				if (subtract) {
					currentStoryIndex--;
				} else {
					currentStoryIndex++;
				}
			}
			if (ExplorersStory.Count > 0) {
				result = ExplorersStory [currentStoryIndex];
			}
			return result;
		}

		void OnGUI() {
		}

	}
}

