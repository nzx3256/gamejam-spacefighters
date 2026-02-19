using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

namespace Emptybraces.Splines
{
	[RequireComponent(typeof(SplineAnimate))]
	public class SplineAnimateOnReached : MonoBehaviour
	{
		//public UnityEvent<SplineContainer, BezierKnot, int> OnReachedKnot;
		private SplineAnimate _anim;
		private SplineContainer _containerLast;
		private int _knotIdxCur, _knotIdxPrev;
		private SplinePath<Spline> _splinePath;
		private bool _isPlaying;
		private bool _isReverse;
		private float _prevT;
		private bool IsPingPongReverse => _anim.Loop == SplineAnimate.LoopMode.PingPong && _isReverse;

		private int numberOfKnots = 0;
		private void _initNumberOfKnots()
		{
			numberOfKnots = 0;
			foreach (Spline spline in _containerLast.Splines)
			{
				numberOfKnots += spline.Count;
			}
		}

		[SerializeField]
		private List<SplineEventsWrapper> KnotEvents;
		// Dictionary Key -> KeyValuePair<KnotIndex, KnotNumberOfPasses>, 
		// Dictionary Value -> 
		private Dictionary<
			KeyValuePair<int,int>,
			UnityEvent<SplineContainer,BezierKnot, int>> KnotEventsDictionary;
		
		private int numberOfKnotsReached = 0;

        private void OnDisable()
		{
			_anim.Restart(false);
			_splinePath = null;
			_containerLast = null;
			_anim.Container = null;
			Reset();
		}
        private void Awake()
		{
			_anim = GetComponent<SplineAnimate>();
			KnotEventsDictionary = KnotEvents.GroupBy(x => new KeyValuePair<int,int>(x.KnotIndex, x.KnotNumberOfPasses)).ToDictionary(
				group => group.Key, 
				group => group.First().OnReachedKnot);
			// StringBuilder sb = new StringBuilder(KnotEventsDictionary.Count.ToString()).Append(": ");
			// foreach (var element in KnotEventsDictionary)
			// {
			// 	sb.Append("[").Append(element.Key.Key).Append(" ").Append(element.Key.Value).Append("], ");
			// }
			// Debug.Log(sb);
		}

		private void Update()
		{
			if (!_InitSpritePathIfNeeded())
				return;
			else if (numberOfKnots == 0)
			{
				_initNumberOfKnots();
			}

			if (!_isPlaying && !_anim.IsPlaying)
				return;
			_isPlaying = true;

			var t = SplineUtility.ConvertIndexUnit(_splinePath, _anim.NormalizedTime, PathIndexUnit.Knot);
			var prev_is_reverse = _isReverse;
			_isReverse = t < _prevT;
			_prevT = t;

			int nt = (int)(t + .000001f); // Accurarcy Adjustment
			if (IsPingPongReverse)
				++nt;
			if (_knotIdxCur != nt)
			{
				_knotIdxPrev = _knotIdxCur;
				_knotIdxCur = nt;
				var knot_idx = _knotIdxPrev + 1;
				if (_anim.Loop == SplineAnimate.LoopMode.PingPong)
				{
					// 最後まで行って折り返す時を除き逆走しているとき
					if (_isReverse && prev_is_reverse)
						knot_idx = _knotIdxPrev - 1;
					// 最初に戻って、折り返すとき
					else if (!_isReverse && prev_is_reverse)
						knot_idx = _knotIdxPrev - 1;
				}
				numberOfKnotsReached++;
				InvokeIfApplicable(knot_idx);
				if (IsPingPongReverse)
					knot_idx = knot_idx == 0 ? _splinePath.Count - 1 : knot_idx - 1;
				else
					knot_idx = (knot_idx + 1) % _splinePath.Count;
			}

			if (!_anim.IsPlaying)
			{
				_isPlaying = false;
				_knotIdxCur = 0;
			}
		}

		public void Reset()
		{
			_knotIdxCur = _knotIdxPrev = 0;
			_isReverse = false;
			_prevT = 0;
			numberOfKnots = 0;
			numberOfKnotsReached = 0;
		}

		private void InvokeIfApplicable(int idx)
		{
			int currentKnotNumber = numberOfKnotsReached % numberOfKnots;
			int numberOfTimesPassed = (numberOfKnotsReached) / numberOfKnots + 1;
			//Debug.Log("Knot Number: "+numberOfKnots+"; CurrentKnot: "+currentKnotNumber+"; Pass #: "+numberOfTimesPassed);
			var temp = new KeyValuePair<int, int>(currentKnotNumber, numberOfTimesPassed);
			var temp2 = new KeyValuePair<int, int>(currentKnotNumber, 0);

			if (KnotEventsDictionary.Count > 0){
				if (KnotEventsDictionary.ContainsKey(temp))
				{
					KnotEventsDictionary.FirstOrDefault(
						x => temp.Equals(x.Key))
						.Value?.Invoke(_anim.Container, _splinePath[idx], idx);
				}
				else if (KnotEventsDictionary.ContainsKey(temp2))
				{
					KnotEventsDictionary.FirstOrDefault(
						x => temp2.Equals(x.Key))
						.Value?.Invoke(_anim.Container, _splinePath[idx], idx);
				}
			}
		}

		private bool _InitSpritePathIfNeeded()
		{
			if (_splinePath == null)
			{
				if (_anim.Container == null)
					return false;
				if (_anim.Container.Splines.Count == 0)
					return false;
				_splinePath = new SplinePath<Spline>(_anim.Container.Splines);
				_containerLast = _anim.Container;
			}
			else if (_anim.Container != _containerLast)
			{
				_splinePath = new SplinePath<Spline>(_anim.Container.Splines);
				_containerLast = _anim.Container;
			}
			return _splinePath != null;
		}

		public void SetSplineAnimateLoopMode(int loopMode)
		{
			_anim.Loop = (SplineAnimate.LoopMode)loopMode;
		}
	}
}