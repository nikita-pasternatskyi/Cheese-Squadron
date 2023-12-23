using MykroFramework.Runtime.Controls;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.UserInterface
{

    public class WindowControl : SerializedMonoBehaviour
    {
        [SerializeField] private WindowControlSO _windowControlSO;
        [SerializeField] private InputRouterReference _inputRouterReference;
        [SerializeField] private Window _currentWindow;
        [SerializeField] public bool BlockNavigation;
        private bool _canMove;

        [SerializeField] private Selectable[,] _selectables;
        private Vector2Int _currentIndex;

        public UnityEvent SelectionChanged;
        public UnityEvent SelectionPressed;

        private void OnEnable()
        {
            _windowControlSO.BlockChangeRequested += OnBlockChangeRequested;
            _windowControlSO.WindowChangeRequested += OnWindowChangeRequested;
            _inputRouterReference.Router.UI_Moved += Move;
            _inputRouterReference.Router.UI_Applied += OnApplied;
            _inputRouterReference.Router.UI_Cancelled += OnCancelled;
        }

        private void OnBlockChangeRequested(bool obj)
        {
            BlockNavigation = obj;
        }

        private void OnDisable()
        {
            _windowControlSO.WindowChangeRequested -= OnWindowChangeRequested;
            _inputRouterReference.Router.UI_Moved -= Move;
            _inputRouterReference.Router.UI_Applied -= OnApplied;
            _inputRouterReference.Router.UI_Cancelled -= OnCancelled;
        }

        private void OnCancelled()
        {
            if (BlockNavigation)
                return;
            if (_currentWindow == null)
                return;
            if (_currentWindow.PreviousWindow != null)
                SwitchWindow(_currentWindow.PreviousWindow);
        }

        private void OnApplied()
        {
            if (BlockNavigation)
                return;
            if (_selectables != null)
            {
                _selectables[_currentIndex.x, _currentIndex.y].Press();
                SelectionPressed?.Invoke();
            }
        }

        public void Move(int x, int y)
        {
            if (BlockNavigation)
                return;
            if (_canMove)
            {
                OffsetIndex(new Vector2Int(x, -y));
                SelectionChanged?.Invoke();
            }
        }
        
        private void OnWindowChangeRequested(Window obj)
        {
            SwitchWindow(obj);
        }

        private void OffsetIndex(Vector2Int offset)
        {
            var newIndex = new Vector2Int();
            newIndex.x = Mathf.Clamp(_currentIndex.x + offset.x, 0, _selectables.GetLength(0) - 1);
            newIndex.y = Mathf.Clamp(_currentIndex.y + offset.y, 0, _selectables.GetLength(1) - 1);
            if (_selectables[newIndex.x, newIndex.y] == null)
                return;
            if (newIndex != _currentIndex)
            {
                _selectables[_currentIndex.x, _currentIndex.y].Deselect();
            }
            _currentIndex = newIndex;
            var currentlySelected = _selectables[_currentIndex.x, _currentIndex.y];
            currentlySelected.Select();
        }

        public void SwitchWindow(Window newWindow)
        {
            if (_selectables != null)
            {
                _selectables[_currentIndex.x, _currentIndex.y].Deselect();
            }
            _canMove = true;
            _currentWindow?.Close();
            _currentIndex = Vector2Int.zero;
            if (newWindow == null)
                return;
            _currentWindow = newWindow;
            _currentWindow.Open();

            if (_currentWindow.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup))
            {
                var selectables = verticalLayoutGroup.GetComponentsInChildren<Selectable>();
                _selectables = new Selectable[1, selectables.Length];

                for (int i = 0; i < _selectables.Length; i++)
                {
                    _selectables[0, i] = selectables[i];
                    selectables[i].Deselect();
                }
            }

            else if (_currentWindow.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup))
            {
                var selectables = horizontalLayoutGroup.GetComponentsInChildren<Selectable>();
                _selectables = new Selectable[selectables.Length, 1];
                for (int i = 0; i < selectables.Length; i++)
                {
                    _selectables[i, 0] = selectables[i];
                }
            }

            else if (_currentWindow.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
            {
                var selectables = gridLayoutGroup.GetComponentsInChildren<Selectable>();
                var x = selectables.Length;
                var y = selectables.Length;

                switch (gridLayoutGroup.constraint)
                {
                    case GridLayoutGroup.Constraint.Flexible:
                        break;
                    case GridLayoutGroup.Constraint.FixedColumnCount:
                        x = gridLayoutGroup.constraintCount;
                        var rest = selectables.Length % gridLayoutGroup.constraintCount;
                        y = selectables.Length / gridLayoutGroup.constraintCount;
                        if (rest != 0)
                            y++;
                        break;
                    case GridLayoutGroup.Constraint.FixedRowCount:
                        y = gridLayoutGroup.constraintCount;
                        x /= gridLayoutGroup.constraintCount;
                        break;
                }

                #region OldMethod

                _selectables = new Selectable[x, y];
                switch (gridLayoutGroup.constraint)
                {
                    case GridLayoutGroup.Constraint.Flexible:
                        break;
                    case GridLayoutGroup.Constraint.FixedColumnCount:
                        for (int ix = 0; ix < x; ix++)
                        {
                            for (int iy = 0; iy < y; iy++)
                            {
                                var index = ix + (iy * x);
                                if (index >= selectables.Length)
                                    continue;
                                _selectables[ix, iy] = selectables[index];
                            }
                        }
                        //0 - 1 - 2       0 - 1
                        //3 - 4 - 5       2 - 3  
                        //4 - 6 - 7       4 - 5
                        //8 - 9 - 10      6 - 7
                        //all even in ys
                        break;
                    case GridLayoutGroup.Constraint.FixedRowCount:
                        for (int ix = 0; ix < x; ix++)
                        {
                            for (int iy = 0; iy < y; iy++)
                            {
                                var newidx = Mathf.Clamp(ix + iy, 0, selectables.Length);
                                _selectables[ix, iy] = selectables[ix + iy];
                            }
                        }
                        break;
                }
                #endregion

            }
            if (_selectables.Length == 0)
            {
                _canMove = false;
                return;
            }
            Move(0, 0);
        }

        private void ResetSelected(Selectable selected)
        {
            selected.Deselect();
        }

        private void GetColumnAndRow(GridLayoutGroup glg, out int column, out int row)
        {
            column = 0;
            row = 0;

            if (glg.transform.childCount == 0)
                return;

            //Column and row are now 1
            column = 1;
            row = 1;

            //Get the first child GameObject of the GridLayoutGroup
            RectTransform firstChildObj = glg.transform.
                GetChild(0).GetComponent<RectTransform>();

            Vector2 firstChildPos = firstChildObj.anchoredPosition;
            bool stopCountingRow = false;

            //Loop through the rest of the child object
            for (int i = 1; i < glg.transform.childCount; i++)
            {
                //Get the next child
                RectTransform currentChildObj = glg.transform.
               GetChild(i).GetComponent<RectTransform>();

                Vector2 currentChildPos = currentChildObj.anchoredPosition;

                //if first child.x == otherchild.x, it is a column, ele it's a row
                if (firstChildPos.x == currentChildPos.x)
                {
                    column++;
                    //Stop couting row once we find column
                    stopCountingRow = true;
                }
                else
                {
                    if (!stopCountingRow)
                        row++;
                }
            }
        }
    }

}

