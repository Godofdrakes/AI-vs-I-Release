using System.Collections.Generic;
using System.Linq;
using AI_vs_I.Player;
using ExtensionMethods;
using UnityEngine;


namespace AI_vs_I.Units.AIControl
{
    [System.Serializable]
    public class AIController /*: MonoBehaviour*/ {

        // AI properties eg. biases and base strategy

        enum AIStep
        {
            SelectUnit,
            SelectTarget,
            FindPath,
            Move,
            Attack,

            Reset,
        }
        [SerializeField]
        private AIStep currentStep = AIStep.SelectUnit;
        public float stepDelay = 0.25f;

        private UnitController unitController;
        public Players myid;
        private List<UnitInstance> myUnits
        {
            get
            {
                return unitController.GetUnitsOfPlayer(myid);
            }
        }
        private List<UnitInstance> enemyUnits
        {
            get
            {
                return unitController.UnitInstances.Where(unit => unit.Owner != myid).ToList();
            }
        }

        public bool stopped = false;

        private float stepTime = 0;
        [SerializeField]
        private UnitInstance selectedUnit = null;
        private GGCell target = null;
        private List<GGCell> path = null;
        private int pathIndexer = 0;

        public void Setup(UnitController controller, Players id)
        {
            unitController = controller;
            myid = id;
        }

        public void Control()
        {
            if (!stopped && stepTime > stepDelay)
            {
                stepTime -= stepDelay;
                //Debug.Log("*AI* Loop");
                switch (currentStep)
                {
                    case AIStep.SelectUnit:
                        //Debug.Log("*AI* Selection");
                        if (!SelectUnit())
                        {
                            stopped = true;
                            //Debug.Log("*AI* Stopped!");
                        }
                        else if (selectedUnit != null)
                        {
                            currentStep = AIStep.FindPath;
                            unitController.SelectedInstance = selectedUnit;
                            //Debug.Log("*AI* Unit Selected");
                        }
                        break;


                    case AIStep.FindPath:
                        //Debug.Log("*AI* Pathing");
                        
                        //path = GGAStar.GetPath(selectedUnit.Head.Cell, target, true, false);
                        path = RegenPath();

                        
                        // GOTO Move
                        currentStep = AIStep.Move;
                        unitController.CurrentAction = UnitController.SelectedAction.Move;
                        pathIndexer = 1;
                        break;


                    case AIStep.Move:
                        //Debug.Log("*AI* Moving");
                        int pathMinus = 0;
                        if (path.LastOrDefault() != null)
                        {
                            UnitBody body = path.LastOrDefault().Objects.Select(ggo => ggo.GetComponent<UnitBody>()).FirstOrDefault(b => b != null);
                            if (path.LastOrDefault().IsOccupied && body != null && body.UnitInstance != selectedUnit)
                            {
                                pathMinus = 1;
                            }
                        }

                        if (unitController.RemainingMoves > 0 && pathIndexer < path.Count - pathMinus)
                        {
                            //Debug.Log("*AI* Move");
                            unitController.MoveSelectedUnit(path[pathIndexer]);
                            pathIndexer++;
                        }
                        else
                        {
                            //Debug.Log("*AI* Destination");
                            //unitController.CompleteTurnActions();
                            //Restart();
                            currentStep = AIStep.SelectTarget;
                        }
                        break;


                    case AIStep.SelectTarget:
                        //Debug.Log("*AI* Targeting");
                        target = GetNearestOfEnemy();

                        if (target != null && target.GetTypesInCell<UnitBody>().FirstOrDefault())
                        {
                            currentStep = AIStep.Attack;
                            unitController.CurrentAction = UnitController.SelectedAction.Action_1;
                        }
                        break;

                        
                    case AIStep.Attack:
                        //Debug.Log("*AI* Attack");
                        //target 
                        
                        if (UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, target) <= selectedUnit.Definition.GetActionModuleInclusive((int)unitController.CurrentAction).RangeValue)
                        {
                            unitController.CommitSelectedActionTo(target);
                            //unitController.CompleteTurnActions();
                        }
                        else
                        {
                            unitController.CompleteTurnActions();
                        }
                        Restart();
                        //stopped = true;
                        break;
                }
                /*if (selectedUnit.IsExausted)
                {
                    Restart();
                }*/
            }
            stepTime += Time.deltaTime;
        }


        bool SelectUnit()
        {
            //Debug.Log("*AI* Slecting Unit");
            foreach (UnitInstance i in myUnits)
            {
                if (!i.IsDead && !i.IsExausted)
                {
                    selectedUnit = i;
                    return true;
                }
            }
            //Debug.Log("*AI* No Unit to select!");
            return false;
        }

        void Restart()
        {
            //Debug.Log("*AI* Restarting Steps");
            currentStep = AIStep.SelectUnit;
            selectedUnit = null;
            target = null;
            path = null;
        }

        
        /*GGCell GetNearestOf(List<GGCell> cells)
        {
            GGCell retTarget = cells[0];
            foreach (GGCell i in cells)
            {
                if (UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, i) <
                    UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, retTarget))
                {
                    retTarget = i;
                }
            }
            return retTarget;
        }*/
        GGCell GetNearestOfEnemy()
        {
            GGCell retTarget = EnemyBodyCells[0];
            foreach (GGCell i in EnemyBodyCells)
            {
                if (UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, i) <
                    UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, retTarget))
                {
                    retTarget = i;
                }
            }
            return retTarget;
        }
        /*GGCell GetFurthestOf(List<GGCell> cells)
        {
            GGCell retTarget = cells[0];
            foreach (GGCell i in cells)
            {
                if (UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, i) >
                    UnitInstance.GetDistanceBetweenCells(selectedUnit.Head.Cell, retTarget))
                {
                    retTarget = i;
                }
            }
            return retTarget;
        }*/

        List<GGCell> RegenPath()
        {
            foreach (UnitBody i in selectedUnit.Body)
            {
                i.GGObject.occupiesCell = false;
                i.GGObject.Update();
                i.GGObject.UpdateCell();
            }
            
            List<GGCell> targetPath = GGAStar.GetPath(selectedUnit.Head.Cell, EnemyBodyCells[0], true);
            int missingHealth = selectedUnit.MaxHealth - selectedUnit.CurrentHealth;
            int preferedDist = Mathf.Min(missingHealth, selectedUnit.Definition.Movement);
            //Debug.Log(missingHealth + " missing hp");
            foreach (GGCell i in CellsInRangeOfBodies())
            {
                List<GGCell> tempPath = GGAStar.GetPath(selectedUnit.Head.Cell, i, true);
                //Debug.Log("First Path, Destination: " + tempPath.Last().GridX + "," + tempPath.Last().GridY);
                if (tempPath.Any())
                {
                    //Debug.Log(tempPath.Count + " path");
                    if (tempPath.Count - 1 >= preferedDist &&
                        tempPath.Count < targetPath.Count)
                    {
                        targetPath = tempPath;
                        //Debug.Log("Good Path, Destination: " + tempPath.Last().GridX + "," + tempPath.Last().GridY);
                    }
                }
                if (targetPath.Count <= 1)
                {
                    targetPath = tempPath;
                    //Debug.Log("Default path");
                }

            }
            //Debug.Break();
            foreach (UnitBody i in selectedUnit.Body)
            {
                i.GGObject.occupiesCell = true;
                i.GGObject.Update();
                i.GGObject.UpdateCell();
            }

            return targetPath;
        }

        List<GGCell> CellsInRangeOfBodies()
        {
            List<GGCell> tempList = new List<GGCell>();
            foreach (GGCell i in EnemyBodyCells)
            {
                foreach (GGCell j in i.GetCellsInRange((int)SelectedUnitRange))
                {
                    if (!tempList.Contains(j) &&
                        j.IsOccupied == false &&
                        j.IsPathable)
                    {
                        tempList.Add(j);
                    }
                    /*else
                    {
                        UnitBody body = j.Objects.Select(ggo => ggo.GetComponent<UnitBody>()).FirstOrDefault(b => b != null);
                        if( j.IsOccupied && body != null && body.UnitInstance == selectedUnit )
                        {
                            tempList.Add(j);
                        }
                    }*/
                }
            }
            return tempList;
        }

        List<GGCell> EnemyBodyCells
        {
            get
            {
                List<GGCell> bodies = new List<GGCell>();
                //GGCell retTarget = null;
                foreach (UnitInstance unit in enemyUnits)
                {
                    foreach (UnitBody i in unit.Body)
                    {
                        if (i.gameObject.activeInHierarchy)
                        {
                            bodies.Add(i.GGObject.Cell);
                        }
                    }
                }
                return bodies;
                /*return
                    unitController.UnitInstances.Where( instance=>instance.Owner != myid )
                                  .Aggregate( new List<UnitBody>(), ( set, instance )=>instance.Body )
                                  .Where( body=>body.gameObject.activeInHierarchy == true )
                                  .Select( body=>body.GGObject.Cell )
                                  .Distinct()
                                  .ToList();*/
            }
        }

        uint SelectedUnitRange
        {
            get
            {
                return selectedUnit.Definition.GetActionModuleInclusive(1).RangeValue;
            }
        }
    }
}
