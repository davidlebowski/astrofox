using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Astrofox
{
	public interface IScoreController
	{
		event Action OnScoreChanged;

		int Score { get; }
		void AddScore(int amount);
	}
}
