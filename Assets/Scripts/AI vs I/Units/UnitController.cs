using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AI_vs_I.CommandSystems;
using AI_vs_I.CommandSystems.Commands;
using AI_vs_I.UserInterface;
using AI_vs_I.Modules;
using AI_vs_I.Units.AIControl;
using AI_vs_I.Player;
using Newtonsoft.Json;
using SmallTools;
using UnityEngine;

using ExtensionMethods;
using Zenject;


namespace AI_vs_I.Units {
    
    [ JsonObject( MemberSerialization.OptIn ) ]
    public class UnitController : Singleton<UnitController> {
        
        public enum SelectedAction
        {
            
            Move = 0,
            Action_1,
            Action_2,
            Action_3,
            Action_4,

        }

        public enum BattleState
        {
            Spawning = 0,
            TurnOne,
            TurnTwo,
            Victory,
            Defeat,
        }

        public Dictionary<Players, Color> PlayerColors =
            new Dictionary<Players, Color> {
                                               { Players.None, Color.white },
                                               { Players.One, new Color(0.5f,0,1) },
                                               { Players.Two, new Color(1,0.5f,0) },
                                           };


        #region Rumtime Fields

        [SerializeField]
        private BattleState m_battleState = BattleState.Spawning;

        [SerializeField]
        private SelectedAction m_selectedAction = SelectedAction.Move;

        private SpawnPoint m_selectedSpawnPoint = null;
        
        private UnitInstance m_selectedInstance = null;

        private UnitRevertState m_selectedRevert;

        private bool m_isSelectedInstance = false;

        private int m_remainingMoves = 0;

        private Players m_playerID = Players.One;

        private List<MoveCommand> m_moveCommandStorage = new List<MoveCommand>();
        private ActionCommand m_actionCommandStorage = null;

        private Vector2 mouseHoldSpot;
        private float mouseHold = 0;

        #endregion


        #region Functions

        public UnitInstance AddNewInstance( UnitDefinition definition, GGCell location, Players owner = Players.One) {
            UnitInstance newInstance = null;

            newInstance = new UnitInstance(Players.One, definition);
            newInstance.Definition.RecalculateStats();
            newInstance.Spawn(location);
            newInstance.Owner = owner;
            newInstance.UnitColor = PlayerColors[newInstance.Owner];
            m_unitInstances.Add(newInstance);
            newInstance.DrawBody();

            return newInstance;
        }

        /*public void AddTestInstance(GGCell location, Players owner = Players.One) {
            UnitInstance newInstance = AddNewInstance( UnitDefinition.PlaceHolder, null );
            newInstance.Spawn( location );
            newInstance.Owner = owner;
            newInstance.DrawBody();
        }*/

        public bool SpawnUnit( SpawnPoint spawnPoint ) {
            Debug.Log("Controller, Spawning Unit: " + spawnPoint.transform.position.x + ", " + spawnPoint.transform.position.y);
            
            UnitInstance newInstance = AddNewInstance(spawnPoint.EnforcedUnit.UnitDefinition, spawnPoint.GGObject.Cell, spawnPoint.PlayerOwner);
            Destroy(spawnPoint.gameObject);

            newInstance.Definition.RecalculateStats();
            newInstance.UnitColor = PlayerColors[newInstance.Owner];
            newInstance.DrawBody();
            
            return newInstance != null;
        }

        #endregion


        #region Button Setup/Teardown

        private void OnSelectedActionChange() {
            TeardownMoveButtons();
            TeardownActionButtons();
            if (IsSelectedInstance)
            {
                if (m_selectedAction == 0 && m_remainingMoves > 0)// if moving
                {
                    SetupMoveButtons(SelectedInstance.Head.Cell);
                }
                else if (m_selectedAction >= (SelectedAction)1)// if acting
                {
                    SetupActionButtons(SelectedInstance, SelectedInstance.Definition.GetActionModuleInclusive((int)m_selectedAction));
                }
            }
        }

        private void SetupActionButtons( UnitInstance actingUnit, ActionModule action ) {
            GGCell center = actingUnit.Head.Cell;
            List<GGCell> targets = center.GetCellsInRange( (int)action.RangeValue );

            foreach( GGCell i in targets ) {
                ActionButton button = Instantiate( m_actionButtonPrefab );
                button.transform.SetParent( null, false );
                button.transform.position = (Vector3)i.CenterPoint2D + new Vector3( 0, 0, -1 );
                button.GetComponent<GGObject>().Update();

                bool containsSelf =
                    i.Objects.Intersect( actingUnit.Body.Select( body=>body.GGObject ) ).Any();
                if( containsSelf && action.CanSelfTarget ) { button.IsTargetValid = true; }

                bool containsFriend =
                    i.Objects.Select( ggo=>ggo.GetComponent<UnitBody>() )
                     .Any( body=>body != null && body.UnitInstance.Owner == actingUnit.Owner );
                if( containsFriend && action.CanFriendTarget ) { button.IsTargetValid = true; }

                bool containsHostile =
                    i.Objects.Select( ggo=>ggo.GetComponent<UnitBody>() )
                     .Any( body=>body != null && body.UnitInstance.Owner != actingUnit.Owner );
                if( containsHostile && action.CanEnemyTarget ) { button.IsTargetValid = true; }
            }
        }

        private void SetupMoveButtons( GGCell center ) {
            foreach(GGDirection direction in new GGDirection[] {
                                          GGDirection.Up,
                                          GGDirection.Down,
                                          GGDirection.Left,
                                          GGDirection.Right,
                                      } )
            {
                GGCell cell = center.GetCellInDirection( direction );
                if( cell != null && cell.IsPathable) {
                    if (!cell.IsOccupied) {
                        BuildMoveButton((Vector3)cell.CenterPoint2D + new Vector3(0,0,-1));
                    }
                    else if (cell.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance == SelectedInstance) {
                        BuildMoveButton((Vector3)cell.CenterPoint2D + new Vector3(0, 0, -1));
                    }
                }
            }

            SetupMoveRange( center, RemainingMoves );
        }

        private void SetupMoveRange( GGCell _center, int moverange ) {
            Profiler.BeginSample( "SetupMoveRange", this );

            GGCell center = _center;

            List<GGCell> projectedMoveList =
                _center.GetCellsInRange( moverange )
                       .Except( _center.GetCellsInRange( 1 ) ).ToList();

            foreach( GGCell cell in projectedMoveList ) {
                if( cell == null ) {
                    continue;
                }

                // Can't walk through an unpathable cell
                if( !cell.IsPathable ) {
                    continue;
                }

                // Does the cell have someone in it?
                if( cell.IsOccupied ) {
                    List<UnitBody> bodiesInCell = cell.GetTypesInCell<UnitBody>().ToList();

                    // It's something other than a unit, can't work with that.
                    if( !bodiesInCell.Any() ) {
                        continue;
                    }

                    // It's another unit, also no good.
                    if( bodiesInCell.Except( SelectedInstance.Body ).Any() ) {
                        continue;
                    }

                    // If we got this far the only thing in the cell is the unit trying to move. That's ok.
                }

                BuildMoveButton( (Vector3)cell.CenterPoint2D + new Vector3( 0, 0, -1 ), true );
            }

            Profiler.EndSample();
        }

        private void BuildMoveButton(Vector3 pos, bool isDisplay = false)
        {
            MoveButton button = Instantiate(m_moveButtonPrefab);
            button.transform.SetParent(null, false);
            button.transform.position = pos;
            button.IsDisplay = isDisplay;
        }

        private void TeardownActionButtons() {
            foreach (ActionButton button in FindObjectsOfType<ActionButton>())
            {
                Destroy(button.gameObject);
            }
        }

        private void TeardownMoveButtons() {
            foreach( MoveButton button in FindObjectsOfType<MoveButton>() ) {
                Destroy( button.gameObject );
            }
        }

        public List<UnitInstance> GetUnitsOfPlayer(Players player)
        {
            return UnitInstances.Where(x => x.Owner == player).ToList();
        }

        public bool AreAllExausted(Players player)
        {
            foreach (UnitInstance i in GetUnitsOfPlayer(player))
            {
                if (!i.IsExausted)
                {
                    return false;
                }
            }
            return true;
        }
        public bool AreAllDead(Players player)
        {
            foreach (UnitInstance i in GetUnitsOfPlayer(player))
            {
                if (!i.IsDead)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region Inspector Fields

        [ SerializeField ]
        private UnitBody m_unitBodyPrefab = null;

        [ SerializeField ]
        private MoveButton m_moveButtonPrefab = null;

        [SerializeField]
        private ActionButton m_actionButtonPrefab = null;

        [SerializeField]
        private SpawnPoint m_spawnPrefab = null;
        
        [ SerializeField, JsonProperty ]
        private List<UnitInstance> m_unitInstances = new List<UnitInstance>();

        #endregion


        #region Controls

        [SerializeField]
        private AIController m_aiController = new AIController();

        private void Start() {
            // Level data is now injected. loading it is unnecessary

            /*foreach (SpawnPoint spawn in FindObjectsOfType<SpawnPoint>().Where(point => point.EnforcedUnit != null))
            {
                SpawnUnit(spawn);
            }*/
            
            m_aiController.Setup(this, Players.Two);
        }

        private void Update()
        {
            
            //UCTUAL UPDATE:
            switch (CurrentBattleState)
            {
                case BattleState.Spawning:
                    Debug.Log("Update, BattleState:Spawning.");
                    foreach (SpawnPoint spawn in FindObjectsOfType<SpawnPoint>().Where(point => point.EnforcedUnit != null))
                    {
                        SpawnUnit(spawn);
                        //Destroy(spawn.gameObject);
                    }
                    
                    /*if (Input.GetMouseButtonDown(0))
                    {
                        GGCell targetCell = ClickCell(MouseWorld);
                        if (targetCell != null)
                        {
                            m_selectedSpawnPoint =
                                targetCell.GetTypesInCell<SpawnPoint>().FirstOrDefault();
                            if (m_selectedSpawnPoint != null)
                            {
                                // Load a placeholder unit if necessary
                                if (m_selectedSpawnPoint.SelectedDefinition == null)
                                {
                                    m_selectedSpawnPoint.SelectedDefinition =
                                        SRResources.Units.LongSword.Load().UnitDefinition;
                                }
                                SpawnUnit(m_selectedSpawnPoint);
                                Destroy(m_selectedSpawnPoint.gameObject);
                            }
                        }
                    }*/
                    if (!FindObjectsOfType<SpawnPoint>().Any())
                    {
                        CurrentBattleState = BattleState.TurnOne;
                    }
                    break;

                case BattleState.TurnOne:
                    PlayerControl();
                    PlayerCameraControl();
                    if (AreAllExausted(Players.One))
                    {
                        CycleTurn();
                    }
                    CheckWin();
                    break;

                case BattleState.TurnTwo:
                    m_aiController.Control();
                    if (SelectedInstance != null)
                    {
                        FindObjectOfType<CameraController>().Target = SelectedInstance.Head.Cell.CenterPoint2D;
                    }
                    if (AreAllExausted(Players.Two))
                    {
                        CycleTurn();
                    }
                    CheckWin();
                    break;

                case BattleState.Victory:
                    break;
            }

            //Inspector
            if (FindObjectOfType<UnitInspectorUIScript>() != null)
            {
                if (Input.GetMouseButton(0))
                {
                    GGCell targetCell = ClickCell(MouseWorld);
                    if (targetCell != null) {
                        UnitBody unitInCell = targetCell.GetTypesInCell<UnitBody>().FirstOrDefault();
                        if( unitInCell == null )
                        {
                            FindObjectOfType<UnitInspectorUIScript>().inspectTarget = null;
                        }
                        else
                        {
                            FindObjectOfType<UnitInspectorUIScript>().inspectTarget = unitInCell.UnitInstance;
                        }

                        /*if (targetCell.IsOccupied == false)
                        {
                            FindObjectOfType<UnitInspectorUIScript>().inspectTarget = null;
                        }
                        else
                        {
                            if (targetCell.GetTypesInCell<UnitBody>().FirstOrDefault() != null)
                            {
                                FindObjectOfType<UnitInspectorUIScript>().inspectTarget = targetCell.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance.Definition;

                            }
                        }*/
                    }
                }
            }

        }

        void PlayerControl()
        {

            Profiler.BeginSample("PlayerControl");
            if (Input.GetMouseButtonDown(0))
            {
                // Player Input
                GGCell targetCell = ClickCell(MouseWorld);

                if (!IsSelectedInstance && targetCell != null)
                {
                    //SELECTING A UNIT:
                    if (targetCell.IsOccupied)
                    {
                        if (targetCell.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance.IsExausted == false
                            && targetCell.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance.Owner == m_playerID)
                        {
                            SelectedInstance = targetCell.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance;
                            CurrentAction = SelectedAction.Move;
                        }
                    }
                }
                else if (targetCell != null)
                {
                    if (m_selectedAction == 0)
                    {
                        // Moving:
                        if (targetCell.GetTypesInCell<MoveButton>().FirstOrDefault() != null)
                        {
                            if (targetCell.GetTypesInCell<MoveButton>().FirstOrDefault().IsDisplay == false)
                            {
                                MoveSelectedUnit(targetCell);
                            }
                        }
                    }
                    else if (m_selectedAction >= (SelectedAction)1)
                    {
                        // Commiting an Action:
                        if (targetCell.GetTypesInCell<ActionButton>().FirstOrDefault() != null)
                        {
                            ActionButton pressedButton = targetCell.GetTypesInCell<ActionButton>().FirstOrDefault();
                            if (pressedButton.IsTargetValid)
                            {
                                CommitSelectedActionTo(targetCell);
                            }
                        }
                    }
                }
            }
            // Hold/Drag Movement
            /*if (Input.GetMouseButton(0))
            {
                GGCell targetCell = ClickCell(MouseWorld);
                if (SelectedInstance != null && targetCell != null)
                {
                    if (m_selectedAction == 0)
                    {
                        // Moving:
                        if (targetCell.GetTypesInCell<MoveButton>().FirstOrDefault() != null)
                        {
                            if (targetCell.GetTypesInCell<MoveButton>().FirstOrDefault().IsDisplay == false)
                            {
                                MoveSelectedUnit(targetCell);
                            }
                        }
                    }
                }
            }*/

            Profiler.EndSample();
        }

        void PlayerCameraControl()
        {
            Profiler.BeginSample("PlayerCameraControl");
            if (Input.GetMouseButton(0))
            {
                mouseHold += Time.deltaTime;

                if (mouseHold >= 0.1f)
                {
                    Vector2 mouseDif = (Vector2)MouseWorld - mouseHoldSpot;
                    FindObjectOfType<CameraController>().Target = FindObjectOfType<CameraController>().Position - mouseDif;
                }
            }
            else
            {
                mouseHold = 0;
                mouseHoldSpot = (Vector2)MouseWorld;
            }
            Profiler.EndSample();
        }

        public void MoveSelectedUnit( GGCell destination ) {
            MoveCommand move = new MoveCommand( UnitInstances.IndexOf( m_selectedInstance ),
                                                destination.GridX,
                                                destination.GridY );
            //m_selectedInstance.Move( destination );
            move.Do();
            m_moveCommandStorage.Add(move);
            //
            //m_selectedInstance.DrawBody();
            //
            m_remainingMoves--;
            TeardownMoveButtons();
            if (m_remainingMoves > 0)
            {
                SetupMoveButtons(SelectedInstance.Head.Cell);
            }
            else
            {
                CurrentAction = SelectedAction.Action_1;
            }
        }

        public void CommitSelectedActionTo( GGCell target )
        {
            Profiler.BeginSample("CommitSelectedAction");
            if (target.GetTypesInCell<UnitBody>().FirstOrDefault() == null)
            {
                Debug.LogWarning("There is no Unit to Target!");
                CompleteTurnActions();
            }
            else
            {
                UnitInstance targetedUnit = target.GetTypesInCell<UnitBody>().FirstOrDefault().UnitInstance;
                ActionEffect[] targetEffects = SelectedInstance.Definition.GetActionModuleInclusive((int)CurrentAction).TargetEffects;
                ActionEffect[] userEffects = SelectedInstance.Definition.GetActionModuleInclusive((int)CurrentAction).UserEffects;
                
                m_actionCommandStorage = new ActionCommand(UnitInstances.IndexOf(SelectedInstance), UnitInstances.IndexOf(targetedUnit), targetEffects, userEffects);
                m_actionCommandStorage.Do();
                //targetedUnit.DrawBody();

                CompleteTurnActions();
            }
            Profiler.EndSample();
        }

        public void CompleteTurnActions()
        {
            Profiler.BeginSample("CompleteTurnActions");
            if (IsSelectedInstance)
            {
                SelectedInstance.IsExausted = true;
                SelectedInstance = null;
                // Command Manage
                CommandSystem commandSystem = GameObject.FindObjectOfType<CommandSystem>();
                foreach (MoveCommand i in m_moveCommandStorage)
                {
                    commandSystem.AddCommand(i);
                }
                if (m_actionCommandStorage != null)
                {
                    commandSystem.AddCommand(m_actionCommandStorage);
                }
                CleanStorage();
            }
            Profiler.EndSample();
        }
        
        public void CancelActions()
        {
            if (IsSelectedInstance)
            {
                m_selectedInstance.RevertTo(m_selectedRevert);
                //m_selectedInstance.DrawBody();
                SelectedInstance = null;
                // Command Clean
                CleanStorage();
            }
        }

        public void ChangeCurrentAction(int action)
        {
            CurrentAction = (SelectedAction)action;
        }

        void CleanStorage()
        {
            m_moveCommandStorage.Clear();
            m_actionCommandStorage = null;
        }

        private float m_turn = 0;

        public void CycleTurn()
        {
            Profiler.BeginSample("CycleTurn");
            Energize();
            switch (CurrentBattleState)
            {
                case BattleState.TurnOne:
                    CurrentBattleState = BattleState.TurnTwo;
                    foreach (UnitInstance i in GetUnitsOfPlayer(Players.One))
                    {
                        i.TurnHasPassed();
                    }
                    break;
                case BattleState.TurnTwo:
                    CurrentBattleState = BattleState.TurnOne;
                    // Snap Camera to First Alive Player unit at the begining of the players turn
                    FindObjectOfType<CameraController>().Target = GetUnitsOfPlayer(Players.One).Where(uni => uni.IsDead == false).FirstOrDefault().Head.Cell.CenterPoint2D;
                    foreach (UnitInstance i in GetUnitsOfPlayer(Players.Two))
                    {
                        i.TurnHasPassed();
                    }
                    break;
            }
            m_turn += 0.5f;
            Debug.Log("TURN: " + m_turn);
            SelectedInstance = null;
            Profiler.EndSample();
        }

        public void Energize()
        {
            foreach (UnitInstance i in m_unitInstances)
            {
                if (i.CurrentStatus != StatusEffect.Freeze)
                {
                    i.IsExausted = false;
                }
                else
                {
                    i.IsExausted = true;
                }
            }
            //m_aiController.stopped = false;
        }

        void CheckWin()
        {
            Profiler.BeginSample("CheckWin");
            if (AreAllDead(Players.One))
            {
                CurrentBattleState = BattleState.Defeat;
                Debug.LogFormat( this, "#{0}# Player has lost!", typeof( UnitController ).Name );
            }
            else if (AreAllDead(Players.Two))
            {
                CurrentBattleState = BattleState.Victory;
                Debug.LogFormat( this, "#{0}# Player has won!", typeof( UnitController ).Name );
            }
            Profiler.EndSample();
        }

        GGCell ClickCell(Vector3 worldmouse)
        {
            if (GGGrid.Rect2D.Contains((Vector2)worldmouse))
            {
                return GGGrid.GetCell(MouseWorld);
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region Properties

        public static Vector3 MouseWorld {
            get {
                return
                    Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x,
                                                                 Input.mousePosition.y,
                                                                 1 ) );
            }
        }

        public UnitBody UnitBodyPrefab {
            get { return m_unitBodyPrefab; }
        }

        public BattleState CurrentBattleState
        {
            get
            {
                return m_battleState;
            }
            set
            {
                m_battleState = value;
            }
        }

        public UnitInstance SelectedInstance {
            get {
                return m_selectedInstance
                       /*?? ( m_selectedInstance = m_unitInstances.FirstOrDefault() )*/;
            }
            set {
                m_selectedInstance = value;
                if (value != null)
                {
                    m_selectedRevert = value.BuildRevertState();
                    m_remainingMoves = value.MaxMove;
                    IsSelectedInstance = true;
                }
                else
                {
                    IsSelectedInstance = false;
                    CurrentAction = SelectedAction.Move;
                }
                //Debug.LogFormat( "#UnitController# Selected unit has changed." );
            }
        }

        public int RemainingMoves
        {
            get
            {
                return m_remainingMoves;
            }
        }

        public bool IsSelectedInstance
        {
            get
            {
                return m_isSelectedInstance;
            }
            set
            {
                m_isSelectedInstance = value;
            }
        }

        public SelectedAction CurrentAction {
            get { return m_selectedAction; }
            set {
                m_selectedAction = value;
                OnSelectedActionChange();
            }
        }

        public ReadOnlyCollection<UnitInstance> UnitInstances {
            get { return m_unitInstances.AsReadOnly(); }
        }

        [Inject( "Selected Level" )]
        public GGGrid GGGrid { get; private set; }

        #endregion
    }

}
