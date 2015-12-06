﻿using UnityEngine;
using System.Collections.Generic;
using MultiPlayer;
using System;

namespace SinglePlayer {
	public class AILineOfSight : MonoBehaviour, IComparer<GameObject>, IComparer<GameUnit> {
		public EnumTeam teamFaction;
		public List<GameObject> enemies;
		public Rigidbody sphereColliderRigidBody;

		public void Start() {
			this.enemies = new List<GameObject>();
		}

		public void OnTriggerEnter(Collider other) {
			AILineOfSight sight = other.GetComponent<AILineOfSight>();
			if (sight != null && sight.teamFaction != this.teamFaction && !this.enemies.Contains(sight.gameObject)) {
				this.enemies.Add(sight.gameObject);
				this.enemies.Sort(this);
			}
			else {
				GameUnit unit = other.GetComponent<GameUnit>();
				if (unit != null && unit.teamFaction != this.teamFaction && !this.enemies.Contains(unit.gameObject)) {
					this.enemies.Add(unit.gameObject);
					this.enemies.Sort(this);
				}
			}
		}

		public void OnTriggerExit(Collider other) {
			AILineOfSight sight = other.GetComponent<AILineOfSight>();
			if (sight != null && sight.teamFaction != this.teamFaction && this.enemies.Contains(sight.gameObject)) {
				this.enemies.Remove(sight.gameObject);
			}
			else {
				GameUnit unit = other.GetComponent<GameUnit>();
				if (unit != null && unit.teamFaction != this.teamFaction && this.enemies.Contains(unit.gameObject)) {
					this.enemies.Remove(unit.gameObject);
				}
			}
		}

		public void FixedUpdate() {
			this.sphereColliderRigidBody.WakeUp();
			if (this.enemies.Count > 0) {
				for (int i = 0; i < this.enemies.Count; i++) {
					if (this.enemies[i] == null) {
						this.enemies.RemoveAt(i);
					}
				}
			}
		}

		public int Compare(GameObject xObj, GameObject yObj) {
			if (xObj != null && yObj != null) {
				AIUnit x = xObj.GetComponent<AIUnit>();
				AIUnit y = yObj.GetComponent<AIUnit>();
				if (x != null && y != null) {
					float first = Vector3.Distance(this.transform.position, x.transform.position);
					float second = Vector3.Distance(this.transform.position, y.transform.position);

					if (first > second) {
						return -1;
					}
					else if (first == second || first.Equals(second) || Mathf.Abs(first - second) < float.Epsilon) {
						return 0;
					}
					else {
						return 1;
					}
				}
			}
			return 0;
		}

		public int Compare(GameUnit x, GameUnit y) {
			if (x != null && y != null) {
				float first = Vector3.Distance(this.transform.position, x.transform.position);
				float second = Vector3.Distance(this.transform.position, y.transform.position);

				if (first > second) {
					return -1;
				}
				else if (first == second || first.Equals(second) || Mathf.Abs(first - second) < float.Epsilon) {
					return 0;
				}
				else {
					return 1;
				}
			}
			return 0;
		}
	}
}
