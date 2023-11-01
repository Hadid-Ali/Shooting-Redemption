using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CoverShooter
{
    /// <summary>
    /// Possible states for the fighting AI to take.
    /// </summary>
    public enum CoverStates
    {
        none,
        idle,
        patrol,
        standAndFight,
        takeCover,
        fightInCover,
        assault,
        flee,
    }


    [Serializable]
    public struct CoverRetreatSettings
    {
        /// <summary>
        /// Health value at which the AI will retreat.
        /// </summary>
        [Tooltip("Health value at which the AI will retreat.")]
        public float Health;

        /// <summary>
        /// Duration in seconds the frightened AI will wait and hide in cover before peeking again.
        /// </summary>
        [Tooltip("Duration in seconds the frightened AI will wait and hide in cover before peeking again.")]
        public float HideDuration;

        public static CoverRetreatSettings Default()
        {
            var settings = new CoverRetreatSettings();
            settings.Health = 25;
            settings.HideDuration = 3;

            return settings;
        }
    }


    /// <summary>
    /// Simulates a fighter, takes cover, assaults, uses weapons, throws grenades, etc.
    /// </summary>
    [RequireComponent(typeof(BaseActor))]
    [RequireComponent(typeof(CharacterMotor))]
    public class CoverBrain : BaseBrain, IAlertListener, ICharacterHealthListener
    {
        #region Properties

        /// <summary>
        /// Is the AI currently alarmed.
        /// </summary>
        public bool IsAlerted
        {
            get
            {
                var state = State;

                return
                       state != CoverStates.none&&
                       state != CoverStates.idle&&
                       state != CoverStates.patrol;
            }
        }

        /// <summary>
        /// AI state the brain is at.
        /// </summary>
        public CoverStates State
        {
            get
            {
                if (_futureSetState != CoverStates.none)
                    return _futureSetState;
                else
                    return _state;
            }
        }

        /// <summary>
        /// Time in seconds to wait for inspecting the last seen threat position.
        /// </summary>
        public float InvestigationWait
        {
            get { return ThreatCover ? Investigation.WaitForCovered : Investigation.WaitForUncovered; }
        }

        /// <summary>
        /// Is the AI currently in an agressive mode. Aggressivness determines if the AI immediately attacks threats seen.
        /// </summary>
        public bool IsInAggressiveMode
        {
            get { return _isInAggressiveMode; }
        }

        /// <summary>
        /// Threat actor that the AI is locked on. Other threats are ignored.
        /// </summary>
        public BaseActor LockedThreat
        {
            get { return _lockedTarget; }
        }

        #endregion

        #region Public fields

        /// <summary>
        /// Enemy distance to trigger slow retreat.
        /// </summary>
        [Tooltip("Enemy distance to trigger slow retreat.")]
        public float AvoidDistance = 4;

        /// <summary>
        /// Enemy distance to trigger slow retreat.
        /// </summary>
        [Tooltip("AI will avoid standing to allies closer than this distance.")]
        public float AllySpacing = 1.2f;

        /// <summary>
        /// Duration in seconds to stand fighting before changing state.
        /// </summary>
        [Tooltip("Duration in seconds to stand fighting before changing state.")]
        public float StandDuration = 2;

        /// <summary>
        /// Duration in seconds to fight circling the enemy before changing state.
        /// </summary>
        [Tooltip("Duration in seconds to fight circling the enemy before changing state.")]
        public float CircleDuration = 2;

        /// <summary>
        /// Time in seconds for the AI to wait before switching to a better cover.
        /// </summary>
        [Tooltip("Time in seconds for the AI to wait before switching to a better cover.")]
        public float CoverSwitchWait = 10;

        /// <summary>
        /// Distance at which the AI guesses the position on hearing instead of knowing it precisely.
        /// </summary>
        [Tooltip("Distance at which the AI guesses the position on hearing instead of knowing it precisely.")]
        public float GuessDistance = 30;

        /// <summary>
        /// Chance the AI will take cover immediately after learning of existance of an enemy.
        /// </summary>
        [Tooltip("Chance the AI will take cover immediately after learning of existance of an enemy.")]
        [Range(0, 1)]
        public float TakeCoverImmediatelyChance = 0;

        /// <summary>
        /// AI won't go to cover if it is closer to the enemy than this value. Only used when the enemy has been known for awhile.
        /// </summary>
        [Tooltip("AI won't go to cover if it is closer to the enemy than this value. Only used when the enemy has been known for awhile.")]
        public float DistanceToGoToCoverFromStandOrCircle = 6;

        /// <summary>
        /// Should the AI attack threats immedietaly on seeing them.
        /// </summary>
        [Tooltip("Should the AI attack threats immedietaly on seeing them.")]
        public bool ImmediateThreatReaction = true;

        /// <summary>
        /// Should the AI switch to attacking enemies that deal damage to the AI.
        /// </summary>
        [Tooltip("Should the AI switch to attacking enemies that deal damage to the AI.")]
        public bool AttackAggressors = true;

        /// <summary>
        /// Settings for AI startup.
        /// </summary>
        [Tooltip("Settings for AI startup.")]
        public AIStartSettings Start = AIStartSettings.Default();

        /// <summary>
        /// Speed of the motor during various AI states.
        /// </summary>
        [Tooltip("Speed of the motor during various AI states.")]
        public FighterSpeedSettings Speed = FighterSpeedSettings.Default();

        /// <summary>
        /// How accurately the AI guesses the position of an enemy.
        /// </summary>
        [Tooltip("How accurately the AI guesses the position of an enemy.")]
        public AIApproximationSettings Approximation = new AIApproximationSettings(0, 10, 5, 30);

        /// <summary>
        /// Settings for AI retreats.
        /// </summary>
        [Tooltip("Settings for AI retreats.")]
        public FighterRetreatSettings Retreat = FighterRetreatSettings.Default();

        /// <summary>
        /// Settings for how long the AI waits before investigating.
        /// </summary>
        [Tooltip("Settings for how long the AI waits before investigating.")]
        public FighterInvestigationWaitSettings Investigation = FighterInvestigationWaitSettings.Default();

        /// <summary>
        /// Settings for how the fighter avoids other grenades.
        /// </summary>
        [Tooltip("Settings for how the fighter avoids other grenades.")]
        public FighterGrenadeAvoidSettings GrenadeAvoidance = FighterGrenadeAvoidSettings.Default();

        /// <summary>
        /// Settings for AI grenades.
        /// </summary>
        [Tooltip("Settings for AI fighting and aiming.")]
        public AIGrenadeSettings Grenades = AIGrenadeSettings.Default();

        /// <summary>
        /// Should a debug line be drawn towards the current threat.
        /// </summary>
        [Tooltip("Should a debug line be drawn towards the current threat.")]
        public bool DebugThreat = false;

        #endregion

        #region Private fields

        private CharacterMotor _motor;
        private CharacterInventory _inventory;
        private CharacterHealth _health;
        private AISight _sight;

        private bool _isInAggressiveMode;

        private int _thrownGrenadeCount;

        private HashSet<BaseBrain> _friends = new HashSet<BaseBrain>();
        private HashSet<BaseActor> _friendsThatCanSeeMe = new HashSet<BaseActor>();
        private HashSet<BaseActor> _visibleCivilians = new HashSet<BaseActor>();

        private CoverStates _previousState;
        private CoverStates _state;
        private float _stateTime;

        private Vector3 _maintainPosition;
        private bool _maintainPositionIndefinitely;
        private float _maintainDuration;
        private bool _hasReachedMaintainPosition;
        private float _maintainPositionReachTime;
        private AIBaseRegrouper _regrouper;

        private Vector3 _defensePosition;

        private bool _failedToAvoidInThisState;
        

        private float _grenadeTimer;
        private float _grenadeCheckTimer;
        private bool _hasThrowFirstGrenade;
        private Vector3[] _grenadePath = new Vector3[128];

        private CoverStates _futureSetState;
        private bool _hasFailedToFindCover;
        private bool _hasSucceededToFindCover;
        private bool _isLookingForCover;

        private bool _wasAlerted;
        private bool _wasAlarmed;

        private bool _assaultCheck;
        private bool _callCheck;
        private bool _investigationCheck;
        private bool _searchCheck;
        private bool _waypointCheck;

        private bool _isInDarkness;

        private BaseActor _lockedTarget;

        private bool _isCombatProcess;

        private List<BaseActor> _visibleActors = new List<BaseActor>();

        private KeepCloseTo _keepCloseTo;
        private Vector3 _moveTarget;
        private bool _hasMoveTarget;
        private bool _hadMoveTargetBeforeAllySpacing;
        private Vector3 _moveTargetBeforeAllySpacing;
        private bool _hasOpenFire;

        private bool _hasCheckedIfTheLastKnownPositionIsNearCover;

        private NavMeshPath _reachablePath;
        private bool _hasCheckedReachabilityAndFailed;
        private float _reachabilityCheckTime;

        private float _invisibleTime;

        #endregion

        #region Commands

        /// <summary>
        /// Registers that the AI is currently firing.
        /// </summary>
        public void ToOpenFire()
        {
            _hasOpenFire = true;
        }

        /// <summary>
        /// Registers that the AI has stopped firing.
        /// </summary>
        public void ToCloseFire()
        {
            _hasOpenFire = false;
        }

        /// <summary>
        /// Registers that the AI has started sprinting to a location.
        /// </summary>
        public void ToSprintTo(Vector3 position)
        {
            _moveTarget = position;
            _hasMoveTarget = true;
            if (_hadMoveTargetBeforeAllySpacing)
                _moveTargetBeforeAllySpacing = position;
        }

        /// <summary>
        /// Registers that the AI has started running to a location.
        /// </summary>
        public void ToRunTo(Vector3 position)
        {
            _moveTarget = position;
            _hasMoveTarget = true;
            if (_hadMoveTargetBeforeAllySpacing)
                _moveTargetBeforeAllySpacing = position;
        }

        /// <summary>
        /// Registers that the AI has started walking to a location.
        /// </summary>
        public void ToWalkTo(Vector3 position)
        {
            _moveTarget = position;
            _hasMoveTarget = true;
            if (_hadMoveTargetBeforeAllySpacing)
                _moveTargetBeforeAllySpacing = position;
        }

        /// <summary>
        /// Registers that the AI has stopped moving.
        /// </summary>
        public void ToStopMoving()
        {
            _hasMoveTarget = false;
            _hadMoveTargetBeforeAllySpacing = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToCircle(Vector3 threat)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToWalkInDirection(Vector3 vector)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToRunInDirection(Vector3 vector)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToSprintInDirection(Vector3 vector)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToWalkFrom(Vector3 target)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToRunFrom(Vector3 target)
        {
            _hasMoveTarget = false;
        }

        /// <summary>
        /// Registers that the AI has no target location to move to.
        /// </summary>
        public void ToSprintFrom(Vector3 target)
        {
            _hasMoveTarget = false;
        }
        


        /// <summary>
        /// Sets the target threat and starts either taking cover or assaulting.
        /// </summary>
        public void ToAttack(BaseActor target)
        {
            _lockedTarget = target;
            setSeenThreat(target, target.transform.position, target.Cover);
            
            takeCoverOrAssault();
        }

        /// <summary>
        /// Locks the AI on the given threat.
        /// </summary>
        public void ToSetThreat(Threat threat)
        {
            _lockedTarget = threat.Actor;
            setSeenThreat(threat.Actor, threat.Position, null);
        }

        /// <summary>
        /// Forget about the enemy and return to patrol or idle state.
        /// </summary>
        public override void ToForget()
        {
            base.ToForget();
            setState(CoverStates.patrol);
        }

        /// <summary>
        /// Told by a component to be scared.
        /// </summary>
        public void ToBecomeScared()
        {
            setState(CoverStates.flee);
        }
        

        /// <summary>
        /// Told by a component to find a cover.
        /// </summary>
        public void ToFindCover()
        {
            setState(CoverStates.takeCover, true);
        }

        /// <summary>
        /// Told by an outside command to regroup around a unit.
        /// </summary>
        public void ToRegroupAround(AIBaseRegrouper regrouper)
        {
            _regrouper = regrouper;

            setState(CoverStates.takeCover, true);
        }
 
        #endregion

        #region Checks

        /// <summary>
        /// Registers existance of an assault component.
        /// </summary>
        public void AssaultResponse()
        {
            _assaultCheck = true;
        }

        /// <summary>
        /// Registers that a call is being made.
        /// </summary>
        public void CallResponse()
        {
            _callCheck = true;
        }

        /// <summary>
        /// Registers existance of an investigation component.
        /// </summary>
        public void InvestigationResponse()
        {
            _investigationCheck = true;
        }

        /// <summary>
        /// Registers existance of a search component.
        /// </summary>
        public void SearchResponse()
        {
            _searchCheck = true;
        }

        /// <summary>
        /// Returns true if there is a component that handles assaults.
        /// </summary>
        private bool tryAssault()
        {
            _assaultCheck = false;
            Message("AssaultCheck", LastKnownThreatPosition);
            return _assaultCheck;
        }

        /// <summary>
        /// Returns true if there is a component that handles investigations.
        /// </summary>
        private bool tryInvestigate()
        {
            _investigationCheck = false;
            Message("InvestigationCheck");
            return _investigationCheck;
        }

        /// <summary>
        /// Returns true if there is a component that handles searches.
        /// </summary>
        private bool trySearch()
        {
            _searchCheck = false;
            Message("SearchCheck");
            return _searchCheck;
        }

        #endregion

        #region Events

        /// <summary>
        /// Notified that a search has finished. Forgets about the previous threat as nothing was found during the search.
        /// </summary>
        public void OnFinishSearch()
        {
            ToForget();
        }
        
        public void OnDead()
        {
        }

        public void OnResurrect()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Notified that the waypoint system has waypoints to visit.
        /// </summary>
        public void OnWaypointsFound()
        {
            _waypointCheck = true;

            if (State == CoverStates.patrol && _isInDarkness)
                Message("ToLight");
        }

        /// <summary>
        /// Event called during a spawning process.
        /// </summary>
        public void OnSpawn(BaseActor caller)
        {
            var brain = caller != null ? caller.GetComponent<BaseBrain>() : null;

            if (brain != null)
                setThreat(false, false, false, true, brain.Threat, brain.LastKnownThreatPosition, brain.ThreatCover, brain.LastSeenThreatTime);
            else
                setState(CoverStates.patrol);
        }
        


        /// <summary>
        /// Registers damage done by a weapon.
        /// </summary>
        public void OnHit(Hit hit)
        {
            if (hit.Attacker != null && canSetThreat && (Threat == null || !Threat.IsAlive || AttackAggressors))
            {
                var threat = hit.Attacker.GetComponent<BaseActor>();

                if (threat != null && threat.Side != Actor.Side && canChangeTarget)
                {
                    if (_visibleActors.Contains(threat) || Vector3.Distance(threat.transform.position, transform.position) <= GuessDistance)
                        setSeenThreat(threat, threat.transform.position, threat.Cover);
                    else
                        guessThreat(threat, threat.transform.position, true);
                }
            }

            foreach (var friend in _friends)
                friend.Message("OnFriendHit", Actor);
        }

        /// <summary>
        /// Notified that a friend was hit.
        /// </summary>
        public void OnFriendHit(BaseActor friend)
        {
            if (canSetThreat && (Threat == null || !Threat.IsAlive || AttackAggressors))
            {
                var brain = friend.GetComponent<FighterBrain>();

                if (brain != null)
                {
                    var threat = brain.Threat;

                    if (threat != null && threat.Side != Actor.Side && canChangeTarget)
                    {
                        if (_visibleActors.Contains(threat))
                            setSeenThreat(threat, threat.transform.position, threat.Cover);
                        else if (Vector3.Distance(threat.transform.position, transform.position) <= GuessDistance)
                            setUnseenThreat(false, true, brain.CanSeeTheThreat, threat, threat.transform.position, threat.Cover);
                        else
                            guessThreat(threat, threat.transform.position, false);
                    }
                }
            }
        }

        /// <summary>
        /// Notified by cover AI that target cover was taken.
        /// </summary>
        public void OnFinishTakeCover()
        {
            switch (State)
            {
                case CoverStates.takeCover:
                    if (Threat != null && Threat.IsAlive)
                        setState(CoverStates.fightInCover);
                    break;
            }
        }

        /// <summary>
        /// Notified by an alert system of an alert.
        /// </summary>
        public void OnAlert(ref GeneratedAlert alert)
        {
            if (!canChangeTarget)
                return;

            if (alert.Actor != null && alert.Actor.gameObject != gameObject)
            {
                if (alert.Actor.Side != Actor.Side)
                {
                    if (alert.Actor.IsAlive)
                    {
                        if (Threat == null ||
                            !Threat.IsAlive ||
                            (alert.Actor == Threat && !CanSeeTheThreat && alert.IsDirect) ||
                            (InvestigationWait < ThreatAge && !CanSeeTheThreat))
                        {
                            if (Vector3.Distance(alert.Position, transform.position) >= GuessDistance)
                                guessThreat(alert.Actor, alert.Position, true);
                            else
                                setUnseenThreat(true, false, false, alert.Actor, alert.Position, null);
                        }
                    }
                }
                else
                {
                    if (Threat == null || !Threat.IsAlive || InvestigationWait < ThreatAge)
                    {
                        var brain = alert.Actor.GetComponent<BaseBrain>();

                        if (brain != null)
                        {
                            if (brain.Threat != null && brain.CanSeeTheThreat && brain.Threat.IsAlive)
                                setUnseenThreat(false, false, true, brain.Threat, alert.Position, null);
                        }
                        else if (alert.IsHostile)
                            setUnseenThreat(false, false, false, null, alert.Position, null);
                    }
                }
            }
        }

        /// <summary>
        /// Notified by communication AI that a friend was found.
        /// </summary>
        public void OnFoundFriend(BaseActor friend)
        {
            var brain = friend.GetComponent<FighterBrain>();

            if (brain != null && !_friends.Contains(brain))
            {
                _friends.Add(brain);

                if (brain.Threat != null)
                {
                    if (brain.HasSeenTheEnemy && brain.ThreatAge < brain.InvestigationWait)
                        Message("OnFriendFoundEnemy", friend);
                    else
                        Message("OnFriendKnowsEnemy", friend);
                }
            }
        }

        /// <summary>
        /// Notified by communication AI that a friend got out of range.
        /// </summary>
        public void OnLostFriend(BaseActor friend)
        {
            var brain = friend.GetComponent<BaseBrain>();

            if (brain != null && _friends.Contains(brain))
                _friends.Remove(brain);
        }


        /// <summary>
        /// Notified by a friend that they found a new enemy position.
        /// </summary>
        public void OnFriendKnowsEnemy(BaseActor friend)
        {
            if (friend == null || friend.Side != Actor.Side)
                return;

            if (Threat != null && !Threat.IsAlive)
                return;

            if (!canSetThreat)
                return;

            var brain = friend.GetComponent<FighterBrain>();
            if (brain == null)
                return;

            if (brain.Threat != Threat && brain.Threat != ForgottenThreat)
                setThreat(false, false, brain.IsActualThreatPosition, brain.IsActualThreatPosition, brain.Threat, brain.LastKnownThreatPosition, brain.ThreatCover, brain.LastSeenThreatTime);
        }

        /// <summary>
        /// Notified by a friend that they found a new enemy position.
        /// </summary>
        public void OnFriendFoundEnemy(BaseActor friend)
        {
            if (friend == null || friend.Side != Actor.Side)
                return;

            var brain = friend.GetComponent<FighterBrain>();
            if (brain == null || !brain.HasSeenTheEnemy || !brain.IsActualThreatPosition)
                return;

            if (Threat != null && Threat.IsAlive && CanSeeTheThreat)
                return;

            if (!canSetThreat)
                return;

            var isOk = false;

            if (Threat == null || !Threat.IsAlive)
                isOk = true;
            else if (!HasSeenTheEnemy)
                isOk = brain.LastSeenThreatTime > LastSeenThreatTime + 0.1f;
            else if (!IsActualThreatPosition)
                isOk = brain.LastSeenThreatTime > LastSeenThreatTime + 0.1f;
            else if (InvestigationWait < ThreatAge && brain.InvestigationWait > brain.ThreatAge)
                isOk = true;

            if (isOk)
                setThreat(false, false, brain.IsActualThreatPosition, brain.IsActualThreatPosition, brain.Threat, brain.LastKnownThreatPosition, brain.ThreatCover, brain.LastSeenThreatTime);
        }

        /// <summary>
        /// Notified by a friend that the AI is seen by them.
        /// </summary>
        public void OnSeenByFriend(BaseActor friend)
        {
            if (!_friendsThatCanSeeMe.Contains(friend))
                _friendsThatCanSeeMe.Add(friend);
        }

        /// <summary>
        /// Notified by a friend that the AI is no longer visible by them.
        /// </summary>
        public void OnUnseenByFriend(BaseActor friend)
        {
            if (_friendsThatCanSeeMe.Contains(friend))
                _friendsThatCanSeeMe.Remove(friend);
        }


        /// <summary>
        /// Notified by the sight AI that an actor has entered the view.
        /// </summary>
        public void OnSeeActor(BaseActor actor)
        {
            _visibleActors.Add(actor);

            if (actor.Side == Actor.Side)
            {
                if (actor.IsAggressive)
                    actor.SendMessage("OnSeenByFriend", Actor, SendMessageOptions.DontRequireReceiver);
                else
                    _visibleCivilians.Add(actor);
            }
            else if (canChangeTarget && canSetThreat)
                if (Threat == null || 
                    !Threat.IsAlive ||
                    InvestigationWait < ThreatAge || 
                    Threat == actor)
                    setSeenThreat(actor, actor.transform.position, actor.Cover);
        }

        /// <summary>
        /// Notified by the sight AI that an actor has dissappeared from the view.
        /// </summary>
        public void OnUnseeActor(BaseActor actor)
        {
            _visibleActors.Remove(actor);

            if (actor.Side == Actor.Side)
            {
                if (actor.IsAggressive)
                    actor.SendMessage("OnUnseenByFriend", Actor, SendMessageOptions.DontRequireReceiver);
                else
                    _visibleCivilians.Remove(actor);
            }
            else if (Threat == actor)
            {
                UnseeThreat();

                if (State == CoverStates.standAndFight && canLeavePosition)
                {
                        takeCoverOrAssault();
                }
            }
        }

        /// <summary>
        /// Notified by the cover AI that the current cover is no longer valid.
        /// </summary>
        public void OnInvalidCover()
        {
            if (State == CoverStates.takeCover || State == CoverStates.fightInCover)
                setState(CoverStates.takeCover, true);
        }

        /// <summary>
        /// Notified by the cover AI that a cover was found.
        /// </summary>
        public void OnFoundCover()
        {
            _hasSucceededToFindCover = true;
            _hasFailedToFindCover = false;
        }

        /// <summary>
        /// Notified that no cover was found.
        /// </summary>
        public void OnNoCover()
        {
            _hasSucceededToFindCover = false;
            _hasFailedToFindCover = true;
        }

        /// <summary>
        /// Notified that a component has started to look for covers.
        /// </summary>
        public void OnCoverSearch()
        {
            _isLookingForCover = true;
            _hasSucceededToFindCover = false;
            _hasFailedToFindCover = false;
        }

        #endregion

        #region Behaviour

        protected override void Awake()
        {
            base.Awake();
            Actor.IsAggressive = true;

            _health = GetComponent<CharacterHealth>();
            _motor = GetComponent<CharacterMotor>();
            _inventory = GetComponent<CharacterInventory>();

            _sight = GetComponent<AISight>();

            _reachablePath = new NavMeshPath();

            switch (Start.Mode)
            {
                case AIStartMode.idle:
                    _futureSetState = CoverStates.idle;
                    break;

                case AIStartMode.patrol:
                    _futureSetState = CoverStates.patrol;
                    break;
                
            }
        }

        private void Update()
        {
            if (Actor == null || !Actor.IsAlive)
                return;

            _stateTime += Time.deltaTime;

            if (_futureSetState != CoverStates.none)
            {
                var state = _futureSetState;
                _futureSetState = CoverStates.none;
                setStateImmediately(state);
            }

            if (Threat == null || !Threat.IsAlive)
                foreach (var civilian in _visibleCivilians)
                {
                    if (civilian.IsAlerted)
                    {
                        break;
                    }
                }

            if (Threat != null && CanSeeTheThreat)
            {
                if (_sight != null)
                    _sight.DoubleCheck(Threat);

                if (CanSeeTheThreat)
                {
                    setSeenThreat(Threat, Threat.transform.position, Threat.Cover);
                    _invisibleTime = 0;
                }
                else
                    _invisibleTime += Time.deltaTime;
            }
            else
                _invisibleTime += Time.deltaTime;

            if (DebugThreat && Threat != null)
                Debug.DrawLine(transform.position, LastKnownThreatPosition, Color.cyan);
            

            switch (_state)
            {
                case CoverStates.none:
                    setState(CoverStates.patrol);
                    break;

                case CoverStates.idle:
                    break;


                case CoverStates.patrol:
                    break;
                
                case CoverStates.standAndFight:
                    if (_stateTime >= StandDuration)
                    {
                        if (Vector3.Distance(transform.position, LastKnownThreatPosition) > DistanceToGoToCoverFromStandOrCircle)
                            takeCoverOrAssault();
                        else if (tryAssault())
                            setState(CoverStates.assault);

                    }
                    else
                    {
                        turnAndAimAtTheThreat();

                        if (!_failedToAvoidInThisState)
                        {
                            checkAllySpacingAndMoveIfNeeded();
                            checkAvoidanceAndSetTheState();
                        }
                    }
                    break;
                
                case CoverStates.takeCover:
                    if (_hasSucceededToFindCover)
                    {
                        if (_isLookingForCover)
                        {
                            Message("ToArm");

                            if (Threat != null)
                                Message("ToOpenFire");
                            else
                                Message("ToFaceWalkDirection");

                            _isLookingForCover = false;
                        }

                        turnAndAimAtTheThreat();
                        checkAvoidanceAndSetTheState();
                    }
                    else if (_hasFailedToFindCover)
                    {
                        if (tryAssault())
                            setState(CoverStates.assault);
                    }
                    break;
                
                case CoverStates.fightInCover:
                    turnAndAimAtTheThreat();

                    if (Actor.Cover == null)
                        setState(CoverStates.takeCover);


                    checkAvoidanceAndSetTheState();
                    checkAllySpacingAndMoveIfNeeded();
                    break;
                
                case CoverStates.assault:
                    turnAndAimAtTheThreat();
                    break;
            }
        }

        #endregion

        #region State

        private void setState(CoverStates state, bool forceRestart = false, bool allowCancelProcess = false)
        {
            if (!allowCancelProcess)
                return;

            if (_futureSetState != CoverStates.none ||
                _state != state ||
                forceRestart)
            {
                _futureSetState = state;
            }
        }

        private void setStateImmediately(CoverStates state)
        {
            if (_state != state)
                _previousState = _state;

            _failedToAvoidInThisState = false;
            _hasOpenFire = false;

            closeState(_state, state);
            _stateTime = 0;
            _state = state;
            openState(_state, _previousState);

            if (IsAlerted)
            {
                if (!_wasAlerted)
                {
                    _wasAlerted = true;
                    Message("OnAlerted");
                }
            }
            else
                _wasAlerted = false;
        }

        private void openState(CoverStates state, CoverStates previous)
        {
            switch (state)
            {
                case CoverStates.none:
                case CoverStates.idle:
                    if (Speed.Enabled) _motor.Speed = Speed.Patrol;
                    Message("ToDisarm");

                    if (Start.ReturnOnIdle && Vector3.Distance(transform.position, StartingLocation) > 0.25f)
                    {
                        Message("ToWalkTo", StartingLocation);
                        Message("ToFaceWalkDirection");
                    }
                    else
                        Message("ToStopMoving");

                    break;
                

                case CoverStates.patrol:
                    if (Speed.Enabled) _motor.Speed = Speed.Patrol;

                    if (Actor.Cover != null)
                        Message("ToLeaveCover");

                    Message("ToDisarm");

                    _waypointCheck = false;
                    Message("ToStartVisitingWaypoints");

                    if (!_waypointCheck)
                        setState(CoverStates.idle);

                    break;

                case CoverStates.standAndFight:
                    Message("ToStopMoving");
                    Message("ToArm");
                    turnAndAimAtTheThreat();
                    Message("ToOpenFire");
                    alarm();
                    break;


                case CoverStates.takeCover:

                        if (state == CoverStates.takeCover)
                            _motor.Speed = Speed.TakeCover;
                        else
                            _motor.Speed = Speed.RetreatToCover;

                    Message("ToRunToCovers");

                    _isLookingForCover = false;
                    _hasSucceededToFindCover = false;
                    _hasFailedToFindCover = true;
                        

                    break;

                case CoverStates.fightInCover:
                    if (Actor.Cover == null)
                        setState(CoverStates.takeCover);
                    else
                    {
                        Message("ToArm");
                        turnAndAimAtTheThreat();
                        Message("ToOpenFire");
                        alarm();
                    }
                    break;

                case CoverStates.assault:
                    if (Speed.Enabled) _motor.Speed = Speed.Assault;
                    Message("ToArm");
                    turnAndAimAtTheThreat();
                    Message("ToOpenFire");
                    Message("ToStartAssault", LastKnownThreatPosition);
                    alarm();
                    break;

                
            }

            if (IsAlerted && _isInDarkness)
                Message("ToTurnOnLight");
        }

        private void closeState(CoverStates state, CoverStates next)
        {
            switch (state)
            {

                case CoverStates.flee:
                    Message("ToStopFleeing");
                    break;

                case CoverStates.patrol:
                    if (_isInDarkness)
                        Message("ToHideFlashlight");

                    Message("ToStopVisitingWaypoints");
                    break;
                
                case CoverStates.takeCover:
                case CoverStates.fightInCover:
                    if (next != CoverStates.fightInCover)
                        Message("ToLeaveCover");
                    break;

                case CoverStates.assault:
                    Message("ToStopAssault");
                    break;
            }

            switch (state)
            {
                case CoverStates.fightInCover:
                case CoverStates.standAndFight:
                case CoverStates.assault:
                case CoverStates.takeCover:
                    Message("ToCloseFire");
                    break;
            }
        }

        #endregion

        #region State checks

        private bool canLeavePosition
        {
            get
            {
                return true;
            }
        }

        private bool canChangeTarget
        {
            get
            {
                return Threat == null || !Threat.IsAlive || Threat != _lockedTarget;
            }
        }

        private bool checkInvestigationManageFireAndSetTheState(bool checkVisibility, bool checkTime, bool startInvestigation = true)
        {
            if (Threat == null)
            {
                if (_hasOpenFire)
                    Message("ToCloseFire");

                return false;
            }

            var needsToInvestigate = false;

            var distance = Vector3.Distance(transform.position, LastKnownThreatPosition);
            var hasTimedOut = (checkTime && InvestigationWait < ThreatAge);
            var isInvisibleAndOutsideCover = checkVisibility && _invisibleTime > 1 && ThreatCover == null;

            if (!hasTimedOut && isInvisibleAndOutsideCover && !_hasCheckedIfTheLastKnownPositionIsNearCover)
            {
                _hasCheckedIfTheLastKnownPositionIsNearCover = true;

                Cover cover = null;
                Vector3 position = LastKnownThreatPosition;

                if (Util.GetClosestCover(position, 3, ref cover, ref position) &&
                    AIUtil.IsObstructed(transform.position + Vector3.up * 2, position + Vector3.up * 0.5f))
                {
                    isInvisibleAndOutsideCover = false;
                    setThreat(false, false, false, false, Threat, position, cover, 0);
                }
            }

            var isInDarkness = _sight != null && _sight.IsInDarkness(Threat);

            if (hasTimedOut || (isInvisibleAndOutsideCover && !isInDarkness))
            {
                var closestPosition = LastKnownThreatPosition;
                var isPossibleToReach = AIUtil.GetClosestStandablePosition(ref closestPosition);

                if (_hasCheckedReachabilityAndFailed && Time.timeSinceLevelLoad - _reachabilityCheckTime > 5)
                    isPossibleToReach = false;

                if (isPossibleToReach)
                {
                    AIUtil.Path(ref _reachablePath, transform.position, closestPosition);
                    isPossibleToReach = _reachablePath.status == NavMeshPathStatus.PathComplete;
                }

                if (isPossibleToReach && tryInvestigate())
                {
                    _hasCheckedReachabilityAndFailed = false;
                    Message("ToClearSearchHistory");

                    needsToInvestigate = true;
                }
                else if (_hasOpenFire)
                {
                    _hasCheckedReachabilityAndFailed = true;
                    _reachabilityCheckTime = Time.timeSinceLevelLoad;
                    Message("ToCloseFire");
                }
            }
            else
            {
                _hasCheckedReachabilityAndFailed = false;
                var sightDistance = (_sight != null && _sight.enabled) ? _sight.Distance : 0;

                if (distance >= sightDistance)
                {
                    if (_hasOpenFire)
                        Message("ToCloseFire");
                }
                else if (!_hasOpenFire && Threat.IsAlive)
                    Message("ToOpenFire");
            }

            return needsToInvestigate;
        }

        private void checkAndThrowGrenade()
        {
            if (Threat == null || InvestigationWait < ThreatAge || _thrownGrenadeCount >= Grenades.GrenadeCount)
                return;

            if (!CanSeeTheThreat && ThreatCover == null && !_isInDarkness)
                return;

            var doThrow = false;

            if (_hasThrowFirstGrenade)
            {
                if (_grenadeTimer < Grenades.Interval)
                    _grenadeTimer += Time.deltaTime;
                else
                    doThrow = true;
            }
            else
            {
                if (_grenadeTimer < Grenades.FirstCheckDelay)
                    _grenadeTimer += Time.deltaTime;
                else
                    doThrow = true;
            }

            if (doThrow && _motor.PotentialGrenade != null)
            {
                if (_grenadeCheckTimer <= float.Epsilon)
                {
                    GrenadeDescription desc;
                    desc.Gravity = _motor.Grenade.Gravity;
                    desc.Duration = _motor.PotentialGrenade.Timer;
                    desc.Bounciness = _motor.PotentialGrenade.Bounciness;

                    var isOk = true;

                    for (int i = 0; i < GrenadeList.Count; i++)
                    {
                        var grenade = GrenadeList.Get(i);

                        if (Vector3.Distance(grenade.transform.position, LastKnownThreatPosition) < grenade.ExplosionRadius * 0.5f)
                        {
                            isOk = false;
                            break;
                        }
                    }

                    int pathLength = 0;

                    if (isOk)
                    {
                        pathLength = GrenadePath.Calculate(GrenadePath.Origin(_motor, Util.HorizontalAngle(LastKnownThreatPosition - transform.position)),
                                                           LastKnownThreatPosition,
                                                           _motor.Grenade.MaxVelocity,
                                                           desc,
                                                           _grenadePath,
                                                           _motor.Grenade.Step);

                        isOk = Vector3.Distance(_grenadePath[pathLength - 1], LastKnownThreatPosition) < Grenades.MaxRadius;
                    }

                    if (isOk)
                    {
                        var count = AIUtil.FindActors(_grenadePath[pathLength - 1], Grenades.AvoidDistance);

                        for (int i = 0; i < count; i++)
                            if (AIUtil.Actors[i] == Actor || AIUtil.Actors[i].Side == Actor.Side)
                            {
                                isOk = false;
                                break;
                            }
                    }

                    if (isOk)
                    {
                        _motor.InputThrowGrenade(_grenadePath, pathLength, _motor.Grenade.Step);
                        _thrownGrenadeCount++;

                        _grenadeTimer = 0;
                        _hasThrowFirstGrenade = true;
                    }
                    else
                        _grenadeCheckTimer = Grenades.CheckInterval;
                }
                else
                    _grenadeCheckTimer -= Time.deltaTime;
            }
            else
                _grenadeCheckTimer = 0;
        }
        
        private bool checkAllySpacingAndMoveIfNeeded()
        {
            var has = false;
            var closest = 0f;
            var vector = Vector3.zero;

            var count = AIUtil.FindActors(transform.position, AllySpacing, Actor);

            for (int i = 0; i < count; i++)
                if (AIUtil.Actors[i].Side == Actor.Side)
                {
                    var v = AIUtil.Actors[i].transform.position - transform.position;
                    var d = v.magnitude;

                    if (!has || d < closest)
                    {
                        has = true;
                        closest = d;
                        vector = v;
                    }
                }

            if (has)
            {
                _hadMoveTargetBeforeAllySpacing = _hasMoveTarget;
                _moveTargetBeforeAllySpacing = _moveTarget;
                Message("ToWalkInDirection", -vector);
                return true;
            }
            else
            {
                if (_hadMoveTargetBeforeAllySpacing)
                    Message("ToRunTo", _moveTargetBeforeAllySpacing);

                return false;
            }
        }

        private bool checkAvoidanceAndSetTheState()
        {
            if (Threat == null || !Threat.IsAlive || !CanSeeTheThreat || Vector3.Distance(LastKnownThreatPosition, transform.position) > AvoidDistance)
                return false;
            
            return true;
        }

        private void takeCoverOrAssault()
        {
            if (tryAssault())
                setState(CoverStates.assault);
            else
                setState(CoverStates.takeCover);
        }

        private void fightOrRunAway()
        {
            if (tryFire())
            {
                if (Actor.Cover != null)
                    setState(CoverStates.fightInCover);
                else
                    setState(CoverStates.standAndFight);
            }
            else if (tryAssault())
                setState(CoverStates.assault);

        }

        private void alarm()
        {
            if (!_wasAlarmed)
            {
                _wasAlarmed = true;
                Message("OnAlarmed");
            }
        }

        private bool tryFire()
        {
            if (_motor.Weapon.Gun != null)
                return true;

            if (_inventory != null)
                for (int i = 0; i < _inventory.Weapons.Length; i++)
                    if (_inventory.Weapons[i].Gun != null)
                        return true;

            return false;
        }

        #endregion

        #region Threat

        private bool canSetThreat
        {
            get
            {
                return _isInAggressiveMode || ImmediateThreatReaction;
            }
        }

        private void turnAndAimAtTheThreat()
        {
            if (Threat == null || !Threat.IsAlive)
            {
                Message("ToTurnAt", LastKnownThreatPosition);
                Message("ToAimAt", LastKnownThreatPosition + Vector3.up * 1.0f);
            }
            else
                Message("ToTarget", new ActorTarget(LastKnownThreatPosition, Threat.RelativeTopPosition, Threat.RelativeStandingTopPosition));
        }

        private void guessThreat(BaseActor threat, Vector3 position, bool isHeard)
        {
            var error = Approximation.Get(Vector3.Distance(transform.position, position));

            if (error < 0.25f)
                setUnseenThreat(isHeard, true, false, threat, position, null);
            else
            {
                var attempts = 0;

                while (attempts < 6)
                {
                    var normal = Util.HorizontalVector(UnityEngine.Random.Range(0f, 360f));
                    var distance = UnityEngine.Random.Range(error * 0.25f, error);
                    var newPosition = position + normal * distance;

                    if (AIUtil.GetClosestStandablePosition(ref newPosition) && Mathf.Abs(newPosition.y - position.y) < 0.2f)
                    {
                        setUnseenThreat(isHeard, false, false, threat, newPosition, null);
                        return;
                    }

                    attempts++;
                }

                setUnseenThreat(isHeard, false, false, threat, position, null);
                return;
            }
        }

        private void setUnseenThreat(bool isHeard, bool isDirect, bool isSeenByFriend, BaseActor threat, Vector3 position, Cover threatCover)
        {
            setThreat(false, isHeard, isSeenByFriend, isDirect, threat, position, threatCover, Time.timeSinceLevelLoad);
        }

        private void setSeenThreat(BaseActor threat, Vector3 position, Cover threatCover)
        {
            setThreat(true, false, false, true, threat, position, threatCover, Time.timeSinceLevelLoad);
        }

        private void setThreat(bool isVisible, bool isHeard, bool isVisibleByFriends, bool isActual, BaseActor threat,
            Vector3 position, Cover threatCover, float time)
        {
            var previousThreat = Threat;
            var wasVisible = CanSeeTheThreat;

            if (threat != _lockedTarget)
                _lockedTarget = null;

            if (threat != null)
                _isInAggressiveMode = true;

            _hasCheckedIfTheLastKnownPositionIsNearCover = false;

            SetThreat(isVisible, isHeard, isActual, threat, position, threatCover, time);

            if (CanSeeTheThreat && Threat != null)
                if (!wasVisible || previousThreat != Threat)
                {
                    foreach (var friend in _friendsThatCanSeeMe)
                        friend.SendMessage("OnFriendFoundEnemy", Actor);

                    foreach (var friend in _friends)
                        if (!_friendsThatCanSeeMe.Contains(friend.Actor))
                            friend.Message("OnFriendFoundEnemy", Actor);
                }

            if (!isActiveAndEnabled)
                return;

            if (canLeavePosition)
            {
                if (!IsAlerted && UnityEngine.Random.Range(0f, 1f) <= TakeCoverImmediatelyChance)
                    setState(CoverStates.takeCover);
            }
            else if (!IsAlerted)
            {
                if (isVisible)
                    fightOrRunAway();
                else if (isVisibleByFriends)
                    takeCoverOrAssault();
                else
                    takeCoverOrAssault();
            }
        

    }

        #endregion
    }
}
